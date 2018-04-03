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
            set { CanvasScale = value / 100.0; }
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
                OnPropertyChanged("CanvasScale");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
