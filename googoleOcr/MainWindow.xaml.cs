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
            canvasHeight = (int)ScrollView.ActualHeight;
            canvasWidth = (int)ScrollView.ActualWidth;
        }

        int canvasHeight;
        public int CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                canvasHeight = value;
                OnPropertyChanged(nameof(CanvasHeight));
            }
        }

        int canvasWidth;
        public int CanvasWidth
        {
            get { return canvasWidth; }
            set
            {
                canvasWidth = value;
                OnPropertyChanged(nameof(CanvasWidth));
            }
        }

        int scale = 100;//これはパーセント。

        public int Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                CanvasScale = value;
                OnPropertyChanged("Scale");
                CanvasWidth = (int)(CanvasScale * ScrollView.ActualWidth);
                CanvasHeight = (int)(CanvasScale * ScrollView.ActualHeight);
            }
        }

        double canvasScale = 1.0;//少数による倍率。


        public double CanvasScale
        {
            get
            { return canvasScale; }
            set
            {
                canvasScale = value / 100f;
                OnPropertyChanged(nameof(CanvasScale));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
