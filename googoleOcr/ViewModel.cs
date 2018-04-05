using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace googoleOcr
{
    class ViewModel : INotifyPropertyChanged
    {
        public Model Model{ get { return this.model; } }
        
        Model model;
        ScrollViewer scrollView;
        public ViewModel(ScrollViewer scrollView)
        {
            this.scrollView = scrollView;
            model = new Model();
            //model.GetOcrData();
            canvasHeight = (int)scrollView.ActualHeight;
            canvasWidth = (int)scrollView.ActualWidth;
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
                CanvasWidth = (int)(CanvasScale * scrollView.ActualWidth);
                CanvasHeight = (int)(CanvasScale * scrollView.ActualHeight);
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
