using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MrCoverage
{
    /// <summary>
    /// TextInputWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TextInputWindow : Window
    {
        public string Text { get => txtInput.Text; set => txtInput.Text = value; }
        public TextInputWindow()
        {
            InitializeComponent();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control || e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                {
                    txtInput.Text += "\r\n";
                    txtInput.Select(txtInput.Text.Length,0);
                    return;
                }
                this.Close();
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(txtInput);
            txtInput.Select(txtInput.Text.Length, 0);
        }
    }
}
