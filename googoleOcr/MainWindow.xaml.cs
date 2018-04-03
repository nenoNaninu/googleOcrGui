using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace googoleOcr
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        public int Scale
        {
            get { return (int)CanvasScale * 100; }
            set
            {
                CanvasScale = value / 100.0;
            }
        }

        public string CheckStr
        {
            get
            {
                return this.CanvasWidth.ToString() + "::" + this.CanvasHeight.ToString();
            }
        }

        private double canvasScale = 1.0;
        public double CanvasScale
        {
            get { return canvasScale; }
            set
            {
                if (value > 0)
                {
                    canvasScale = value;
                }
                OnPropertyChanged("CanvasHeight");
                OnPropertyChanged("CanvasWidth");
                OnPropertyChanged("CheckStr");
                OnPropertyChanged("CanvasScale");

            }
        }

        public int CanvasWidth
        {
            get
            {
                return (int)(ScrollView.ActualWidth*CanvasScale);
            }
        } 

        public int CanvasHeight
        {
            get
            {
                return (int)(ScrollView.ActualHeight * CanvasScale);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /**別のルーチン**/

        private void printPos(UIElement el)
        {
            int x = (int)Canvas.GetLeft(el);
            int y = (int)Canvas.GetTop(el);
            //textPos.Text = string.Format("x:{0} y:{1}", x, y);
        }

        /// <summary>
        /// ドラッグ開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark_DragStarted(object sender,
            System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            mark.Background = new SolidColorBrush(Colors.Orange);
        }
        /// <summary>
        /// ドラッグ終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark_DragCompleted(object sender,
            System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            mark.Background = new SolidColorBrush(Colors.Purple);
        }

        /// <summary>
        /// ドラッグ中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark_DragDelta(object sender,
            System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            printPos(mark);
            Canvas.SetLeft(mark, Canvas.GetLeft(mark) + e.HorizontalChange);
            Canvas.SetTop(mark, Canvas.GetTop(mark) + e.VerticalChange);
        }

    }
}
