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
    public partial class MainWindow
    {

        List<Register> Registers;
        List<Memory> MemoryAddresses;

        public MainWindow()
        {
            InitializeComponent();
            taCode.Content = GetNewTabEditor(String.Empty);
            InitializeRegistersAndMemory();
        }


        #region Ribbon
        // File
        private void RibbonButtonNew_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RibbonButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.DefaultExt = ".txt";
            openDialog.Filter = "Text documents (.txt)|*.txt";
            openDialog.ShowDialog();
            if (openDialog.FileName != String.Empty && openDialog.FileName != null && openDialog.CheckPathExists)
            {
                var tabname = openDialog.SafeFileName;
                var filename = openDialog.FileName;
            }
        }

        private void RibbonButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileText = "";
                var fileName = "";

                if (fileName == String.Empty)
                {
                    var saveDialog = new Microsoft.Win32.SaveFileDialog();
                    saveDialog.DefaultExt = ".txt";
                    saveDialog.Filter = "Text documents (.txt)|*.txt";
                    saveDialog.ShowDialog();
                    var fileStream = File.Create(saveDialog.FileName);
                    var fileByteStream = Encoding.ASCII.GetBytes(fileText);
                    var fileByteCount = Encoding.ASCII.GetByteCount(fileText);
                    fileStream.Write(fileByteStream, 0, fileByteCount);
                    fileStream.Close();
                }
                else if (File.Exists(fileName))
                {
                    var fileStream = File.Open(fileName, FileMode.Truncate);
                    var fileByteStream = Encoding.ASCII.GetBytes(fileText);
                    var fileByteCount = Encoding.ASCII.GetByteCount(fileText);
                    fileStream.Write(fileByteStream, 0, fileByteCount);
                    fileStream.Close();
                }
                else
                {
                    MessageBox.Show("File does not exist. It might have been deleted", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);

                }

                MessageBox.Show("File Successfully saved", "Save Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while trying to save the file", "Save Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            TextEditor textEditor = (TextEditor) taCode.Content;
            try
            {
                var instructions = Opcode.Generate(textEditor.Text);
                var sim = new SimulationWindow(instructions, Registers, MemoryAddresses);
                sim.Show();
            }
            catch (OpcodeGenerationException ex)
            {
                var errorWindow = new OpcodeGenerationErrorWindow(ex);
                errorWindow.Show();
                return;
            }

           
        }

        private void RibbonButtonRun_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RibbonButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            var help = new HelpWindow();
            help.Show();
        }

        private void RibbonButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.Show();
        }

        #endregion

        #region Helper Methods
     
        private TextEditor GetNewTabEditor(string content)
        {
            TextEditor te = new TextEditor();
  
            te.ShowLineNumbers = true;
            te.TextArea.BorderBrush = RegGrid.BorderBrush;
            te.TextArea.BorderThickness = RegGrid.BorderThickness;

            te.FontSize = 12;
            te.FontFamily = new FontFamily("Consolas");
            te.Text = content;
            te.SyntaxHighlighting = new MipsimHighlightingDefinition();
            return te;
        }

        #endregion

        public void InitializeRegistersAndMemory()
        {
            Registers = new List<Register>();
            MemoryAddresses = new List<Memory>();

            for (int i = 0; i < 32; i++)
            {
                Registers.Add(new Register(String.Format("R{0}", i)));
            }
            
            Registers.Add(new Register("HI"));
            Registers.Add(new Register("LO"));

            for (int i = 0; i < 8192; i++)
            {
                MemoryAddresses.Add(new Memory(8192 + i));
            }

            RegGrid.ItemsSource = Registers;
            MemGrid.ItemsSource = MemoryAddresses;
        }
    }
}
