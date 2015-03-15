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
        public SimulationWindow(List<Instruction> instructions, List<Register> registers, List<Memory> memoryAddresses)
        {
            InitializeComponent();
            _instructions = instructions;
            _registers = registers;
            _memoryAddresses = memoryAddresses;
            BindParameters();
        }

        private void BindParameters()
        {
            InstGrid.ItemsSource = _instructions;
            _registers.Where(x => x.Id == "R0").FirstOrDefault().HexValue = "0000000000000000";
            RegGrid.ItemsSource = _registers;
            MemGrid.ItemsSource = _memoryAddresses;
        }
    }
}
