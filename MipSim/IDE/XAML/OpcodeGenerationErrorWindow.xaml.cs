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
    public partial class OpcodeGenerationErrorWindow
    {

        public OpcodeGenerationErrorWindow(OpcodeGenerationException ex)
        {
            InitializeComponent();
            ErrorGrid.ItemsSource = ex.ErrorSet.Errors;
        }
    }
}
