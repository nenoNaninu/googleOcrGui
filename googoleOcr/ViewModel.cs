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

        public void excuteGoogleOcr(string fileName)
        {
            var boundingTextList = model.GetOcrData(fileName);

            Debug.Print("==================");
            for(int i = 0; i < boundingTextList.Count; i++)
            {
                TextBox childTextBlock = new TextBox();
                childTextBlock.Text = boundingTextList[i].Text;
                Debug.Print(boundingTextList[i].Text);
                childTextBlock.TextWrapping = TextWrapping.Wrap;
                childTextBlock.Width = boundingTextList[i].Width;
                if (boundingTextList[i].Height <30)
                {
                    childTextBlock.Height = 30;
                }
                else
                {
                    childTextBlock.Height = boundingTextList[i].Height;
                }
                childTextBlock.Name = "boundingTextBox" + i.ToString();
                System.Windows.Shapes.Rectangle rectAngle = new System.Windows.Shapes.Rectangle();
                rectAngle.Stroke = new SolidColorBrush(Colors.Black);
                rectAngle.Width = boundingTextList[i].Width;
                rectAngle.Height = boundingTextList[i].Height;
                Canvas.SetLeft(rectAngle, boundingTextList[i].Left*1.5);
                Canvas.SetTop(rectAngle, boundingTextList[i].Top*1.5);
                Canvas.SetLeft(childTextBlock, boundingTextList[i].Left*1.5);
                Canvas.SetTop(childTextBlock, boundingTextList[i].Top*1.5);
                this.canvasInScroll.Children.Add(rectAngle);
                this.canvasInScroll.Children.Add(childTextBlock);
            }
            Debug.Print("==================");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
