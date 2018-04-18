using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace googoleOcr
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel viewModel;
        string fileName = null;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel(ScrollView, CanvasInScrollView);
            this.DataContext = this.viewModel;
            OcrButton.IsEnabled = false;
        }

        private void Openfile_button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog("フォルダの選択");
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.fileName = dialog.FileName;
                OcrButton.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.CanvasInScrollView.Children.Clear();
            viewModel.excuteGoogleOcr(this.fileName);
        }
    }
}
