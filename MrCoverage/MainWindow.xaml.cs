using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MrCoverage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IterationItemSet ItemSet { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ItemSet = new IterationItemSet();
            this.DataContext = ItemSet;
            jgidMain.ItemSet = ItemSet;
            jgidMain.EditorTextBox = txtEdit;
            this.PreviewMouseMove += jgidMain.Window_PreviewMouseMove;

            ItemSet.AddNewColumn();
            ItemSet.AddNewSequence();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ItemSet.AddNewColumn();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = ItemSet.Colmuns;
            MouraTemplateEngine engine = new MouraTemplateEngine { Columns = item.ToList() };
            string txt = "";
            foreach (var set in engine.EnumMouraItemSet())
            {
                txt += engine.IterationCount;
                txt += set?.ToString() + "\r\n";
            }
            txtTemplate.Text = txt;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
