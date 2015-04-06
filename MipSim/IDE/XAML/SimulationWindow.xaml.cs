using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;
using System.Data;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit;

using MipSim.Core;



namespace MipSim.IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SimulationWindow
    {
        List<Instruction> _instructions;
        List<Register> _registers;
        List<Memory> _memoryAddresses;
        
        List<Register> _registersCopy;
        List<Memory> _memoryAddressesCopy;

        List<ClockCycle> _clockcycles;
        DataTable _pipelinemap;
        CPU cpu;
        int currentClockCycle = 0;
        bool executionComplete = false;

        public SimulationWindow(List<Instruction> instructions, List<Register> registers, List<Memory> memoryAddresses)
        {
            InitializeComponent();

            // Copy Base Source
            _instructions = instructions;
            _registersCopy = registers;
            _memoryAddressesCopy = memoryAddresses;

            // Initialize Working Source
            _clockcycles = new List<ClockCycle>();
            _registers = new List<Register>();
            _memoryAddresses = new List<Memory>();
            CopyToWorkingComponents();

            // Create CPU
            cpu = new CPU(_instructions, _registers, _memoryAddresses);

            // BindParameters
            BindParameters();
        }

        private void BindParameters()
        {
            InstGrid.ItemsSource = _instructions;
            _registers.Where(x => x.Id == "R0").FirstOrDefault().HexValue = "0000000000000000";
            RegGrid.ItemsSource = _registers;
            MemGrid.ItemsSource = _memoryAddresses;
            ClockcycleGrid.ItemsSource = _clockcycles;
            RegGrid.Items.Refresh();
            MemGrid.Items.Refresh();
            ClockcycleGrid.Items.Refresh();
            InitializePipelineMap();
        }

        private void ExecuteClockCycle()
        {
            try
            {
                currentClockCycle++;
                var clockCycle = new ClockCycle(currentClockCycle);
                cpu.Execute();
                cpu.CopyInternalRegisters(clockCycle);
                _clockcycles.Add(clockCycle);
                ClockcycleGrid.Items.Refresh();
                RegGrid.Items.Refresh();
                MemGrid.Items.Refresh();

                if (!cpu.Pipeline.Any())
                {
                    executionComplete = true;
                    DisableControls();
                    return;
                }

                RefreshPipelineMap(cpu.Pipeline, currentClockCycle);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error Encountered", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializePipelineMap()
        {
            _pipelinemap = new DataTable();
            _pipelinemap.Columns.Add("Address");
            _pipelinemap.Columns.Add("Label");
            _pipelinemap.Columns.Add("Instruction");
            _pipelinemap.Columns.Add("Opcode");

            foreach (var inst in _instructions)
            {
                _pipelinemap.Rows.Add(inst.AddressHex, inst.Label, inst.InstructionString, inst.OpcodeHex);
            }

            PipelineGrid.ItemsSource = _pipelinemap.DefaultView;
        }

        private void RefreshPipelineMap(Dictionary<String, String> pipeline, int clockcycle)
        {
            var clockcycleId = String.Format("CC {0}", clockcycle);
            _pipelinemap.Columns.Add(clockcycleId);

            for(int i = 0; i < _pipelinemap.Rows.Count; i++)
            {
                var key = _pipelinemap.Rows[i]["Address"].ToString();
                var item = pipeline.Where(p => p.Key == key).FirstOrDefault();
                if(!item.Equals(default(KeyValuePair<String,String>)))
                {
                    _pipelinemap.Rows[i][clockcycleId] = item.Value;
                }
            }
            PipelineGrid.ItemsSource = null;
            PipelineGrid.Items.Refresh();
            PipelineGrid.ItemsSource = _pipelinemap.DefaultView;
            PipelineGrid.Items.Refresh();
        }

        private void DisableControls()
        {
            menuRun.IsEnabled = false;
            menuStep.IsEnabled = false;
        }

        private void CopyToWorkingComponents()
        {
            _registers.Clear();
            foreach (var register in _registersCopy)
            {
                var newRegister = new Register(register);
                _registers.Add(newRegister);
            }

            _memoryAddresses.Clear();
            foreach (var memoryBlock in _memoryAddressesCopy)
            {
                var newMemoryBlock = new Memory(memoryBlock);
                _memoryAddresses.Add(newMemoryBlock);
            }

        }

        private void ResetExecutionParameters()
        {
            _clockcycles.Clear();
            currentClockCycle = 0;
            executionComplete = false;
            CopyToWorkingComponents();
            cpu = new CPU(_instructions, _registers, _memoryAddresses);
            BindParameters();
        }

        #region Event Handlers

        private void tbxGotoMem_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentText = tbxGotoMem.Text.ToUpper();
            var index = _memoryAddresses.FindIndex(m => m.Address.StartsWith(currentText));
            if (index != -1)
            {

                MemGrid.ScrollIntoView(MemGrid.Items[MemGrid.Items.Count - 1]);
                MemGrid.UpdateLayout();
                MemGrid.ScrollIntoView(MemGrid.Items[index]);
                MemGrid.SelectedItem = MemGrid.Items[index];
            }
        }

        private void menuStep_Click(object sender, RoutedEventArgs e)
        {
            if (!executionComplete)
            {
                ExecuteClockCycle();
            }

            if (executionComplete)
            {               
                MessageBox.Show("Execution Complete!", "Mips Simulator - Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void menuRun_Click(object sender, RoutedEventArgs e)
        {
            while (!executionComplete)
            {
                ExecuteClockCycle();
            }

            MessageBox.Show("Execution Complete!", "Mips Simulator - Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void menuReset_Click(object sender, RoutedEventArgs e)
        {
            ResetExecutionParameters();
            menuRun.IsEnabled = true;
            menuStep.IsEnabled = true;
        }

        private void PipelineGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            foreach (var dataGridColumn in PipelineGrid.Columns)
            {
                if (dataGridColumn.DisplayIndex > 2)
                {
                    var textColumn = dataGridColumn as DataGridTextColumn;
                    if (textColumn == null) continue;

                    textColumn.ElementStyle = FindResource("ColumnStyle") as Style;
                }
            }
        }

        #endregion Event Handlers

       

        

      

       

        

    }
}
