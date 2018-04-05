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


namespace googoleOcr
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel(ScrollView, CanvasInScrollView);
            this.DataContext = this.viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Model.GetOcrData();
        }

        private void mark_DragStarted(object sender,
        System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            //mark. = new SolidColorBrush(Colors.Orange);
        }
        /// <summary>
        /// ドラッグ終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark_DragCompleted(object sender,
            System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //mark.Background = new SolidColorBrush(Colors.Purple);
        }

        /// <summary>
        /// ドラッグ中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark_DragDelta(object sender,
            System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            //printPos(mark);
            Canvas.SetLeft((Thumb)sender, Canvas.GetLeft((Thumb)sender) + e.HorizontalChange);
            Canvas.SetTop((Thumb)sender, Canvas.GetTop((Thumb)sender) + e.VerticalChange);
        }

        private void printPos(UIElement el)
        {
            int x = (int)Canvas.GetLeft(el);
            int y = (int)Canvas.GetTop(el);
            //textPos.Text = string.Format("x:{0} y:{1}", x, y);
        }
    }
}
