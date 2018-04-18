using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;
using System.IO;

namespace googoleOcr
{
    class ViewModel : INotifyPropertyChanged
    {
        public Model Model { get { return this.model; } }

        Model model;
        ScrollViewer scrollView;
        Canvas canvasInScroll;
        public ViewModel(ScrollViewer scrollView, Canvas canvasInScroll)
        {
            this.scrollView = scrollView;
            this.canvasInScroll = canvasInScroll;
            model = new Model(this);
            //model.GetOcrData();
            canvasHeight = (int)scrollView.ActualHeight;
            canvasWidth = (int)scrollView.ActualWidth;
        }

        private int canvasHeight;
        public int CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                canvasHeight = value;
                OnPropertyChanged(nameof(CanvasHeight));
            }
        }

        private int canvasWidth;
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

        List<MoveableTextbox> moveableTextBoxList = new List<MoveableTextbox>();
        public void excuteGoogleOcr(string fileName)
        {
            var boundingTextList = model.GetOcrData(fileName);
            FileStream fs = new FileStream("ocrResult.txt",FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < boundingTextList.Count; i++)
            {
                MoveableTextbox moveableTextBox = new MoveableTextbox();
                moveableTextBoxList.Add(moveableTextBox);
                var it = boundingTextList[i];
                moveableTextBox.SetProperty(it.Top, it.Left, it.Height, it.Width, it.Text);
                Canvas.SetLeft(moveableTextBox, it.Left);
                Canvas.SetTop(moveableTextBox, it.Top);
                this.canvasInScroll.Children.Add(moveableTextBox);
                sw.WriteLine(it.Text);
                sw.WriteLine("========");
            }

            sw.Close();
            fs.Close();

            Debug.Print("==================");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
