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
        String _currentFile;
        String _titlePrefix;
        bool _currentFileIsSaved = true;

        public MainWindow()
        {
            InitializeComponent();
            taCode.Content = GetNewTabEditor(String.Empty);
            InitializeRegistersAndMemory();
            _titlePrefix = this.Title;
            New();
        }


        #region File Menu Event Handlers

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentFileIsSaved)
            {
                var answer = MessageBox.Show("You have unsaved Changes. Do you want to save your changes before closing this document?",
                                             "Save File?", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                switch (answer)
                {
                    case MessageBoxResult.Yes: Save(); break;
                    case MessageBoxResult.No: break;
                    case MessageBoxResult.Cancel: return;
                    default: return;
                }
            }

            New();           
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentFileIsSaved)
            {
                var answer = MessageBox.Show("You have unsaved Changes. Do you want to save your changes before closing this document?",
                                             "Save File?", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                switch (answer)
                {
                    case MessageBoxResult.Yes: Save(); break;
                    case MessageBoxResult.No: break;
                    case MessageBoxResult.Cancel: return;
                    default: return;
                }
            }


            var openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.DefaultExt = ".txt";
            openDialog.Filter = "Text documents (.txt)|*.txt";
            openDialog.ShowDialog();
            if (openDialog.FileName != String.Empty && openDialog.FileName != null && openDialog.CheckPathExists)
            {
                var tabname = openDialog.SafeFileName;
                var filename = openDialog.FileName;
                Open(filename, openDialog.OpenFile()); 
            }
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        #endregion File Menu Event Handlers

        #region Text Editor Event Handlers

        private void te_TextChanged(object sender, EventArgs e)
        {
            _currentFileIsSaved = false;
        }

        #endregion Text Editor Event Handlers

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            TextEditor textEditor = (TextEditor)taCode.Content;
            try
            {
                var instructions = Opcode.Generate(textEditor.Text);
                
                if (instructions.Count == 0)
                {
                    MessageBox.Show("No Code to Execute", "Mips Simulator - Editor", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

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

        #region Help Event Handlers

        private void MenuHelp_Click(object sender, RoutedEventArgs e)
        {
            var help = new HelpWindow();
            help.Show();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.Show();
        }

        #endregion Help Event Handlers

        #region Helper Methods

        private TextEditor GetNewTabEditor(String content)
        {
            TextEditor te = new TextEditor();
  
            te.ShowLineNumbers = true;
            te.TextChanged += te_TextChanged;
            te.TextArea.BorderBrush = RegGrid.BorderBrush;
            te.TextArea.BorderThickness = RegGrid.BorderThickness;

            te.FontSize = 12;
            te.FontFamily = new FontFamily("Consolas");
            te.Text = content;
            te.SyntaxHighlighting = new MipsimHighlightingDefinition();
            return te;
        }

        private String GetFilePath(String directory, String fileName)
        {
            return String.Format("{0}{1}", directory, fileName);
        }

        private void New()
        {
            var path = System.IO.Path.GetDirectoryName(_currentFile);
            if (path == "\\")
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + path;
            }
            var ctr = 0;
            do
            {
                var file = "Untitled.txt";
                if (ctr > 0)
                {
                    file = String.Format("Untitled({0}).txt", ctr);
                }
                var tpath = path + "\\" + file;
                if (!File.Exists(tpath))
                {
                    _currentFile = tpath;
                    this.Title = String.Format("{0} [{1}]", _titlePrefix, System.IO.Path.GetFileName(_currentFile));
                    break;
                }
                ctr++;
            } while (true);

            var te = (TextEditor)taCode.Content;
            te.Text = String.Empty;
            _currentFileIsSaved = true;
        }

        private void Open(String fileName = "", Stream file = null)
        {
            try
            {
                string content = String.Empty;
                if (file != null)
                {
                    var reader = new StreamReader(file);
                    content = reader.ReadToEnd();
                    reader.Close();
                }
                var te = (TextEditor)taCode.Content;
                te.Text = content;
                _currentFile = fileName;
                _currentFileIsSaved = true;
                this.Title = String.Format("{0} [{1}]", _titlePrefix, System.IO.Path.GetFileName(fileName));
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while trying to open the file", "Open Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Save()
        {
            try
            {
                var tempFile = _currentFile;
                if (!(File.Exists(tempFile)))
                {
                    var saveDialog = new Microsoft.Win32.SaveFileDialog();
                    if(System.IO.Path.GetDirectoryName(tempFile) == "\\")
                    {
                        saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    }
                    saveDialog.FileName = System.IO.Path.GetFileNameWithoutExtension(tempFile);
                    saveDialog.DefaultExt = ".txt";
                    saveDialog.Filter = "Text documents (.txt)|*.txt";
                    if (saveDialog.ShowDialog() == true)
                    {
                        SaveUtil(saveDialog.FileName);
                    }                   
                }
                else
                {
                    SaveUtil(tempFile);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while trying to save the file", "Save Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveUtil(String fileName)
        {
            var fileTextEditor = (TextEditor)taCode.Content;
            var fileStream = File.Create(fileName);
            var fileByteStream = Encoding.ASCII.GetBytes(fileTextEditor.Text);
            var fileByteCount = Encoding.ASCII.GetByteCount(fileTextEditor.Text);
            fileStream.Write(fileByteStream, 0, fileByteCount);
            fileStream.Close();
            MessageBox.Show("File Successfully saved", "Save Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _currentFile = fileName;
            _currentFileIsSaved = true;
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

        private void tbxGotoMem_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentText = tbxGotoMem.Text.ToUpper();
            var index = MemoryAddresses.FindIndex(m => m.Address.StartsWith(currentText));
            if(index != -1)
            {

                MemGrid.ScrollIntoView(MemGrid.Items[MemGrid.Items.Count - 1]);
                MemGrid.UpdateLayout();
                MemGrid.ScrollIntoView(MemGrid.Items[index]);
                MemGrid.SelectedItem = MemGrid.Items[index];
            }
        }

        private void MemGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var x = e;
            var s = sender;
        }

    }
}
