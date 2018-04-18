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
using Reactive.Bindings;

namespace googoleOcr
{
    class ViewModel : INotifyPropertyChanged
    {
        public Model Model { get { return this.model; } }

        Model model;
        ScrollViewer scrollView;
        Canvas canvasInScroll;

        string selectedFilename = null;

        public ViewModel(ScrollViewer scrollView, Canvas canvasInScroll)
        {
            this.scrollView = scrollView;
            this.canvasInScroll = canvasInScroll;
            model = new Model(this);
            CanvasHeight.Value = (int)scrollView.ActualHeight;
            CanvasWidth.Value = (int)scrollView.ActualWidth;
            Scale.Subscribe(x =>
            {
                Scale.Value = x;
                CanvasScale.Value = x / 100f;
                CanvasWidth.Value = (int)(CanvasScale.Value * scrollView.ActualWidth);
                CanvasHeight.Value = (int)(CanvasScale.Value * scrollView.ActualHeight);
            });
        }

        public ReactiveProperty<int> CanvasHeight { get; set; } = new ReactiveProperty<int>();
 
        public ReactiveProperty<int> CanvasWidth { get; set; } = new ReactiveProperty<int>();

        public ReactiveProperty<int> Scale { get; set; } = new ReactiveProperty<int>(100);

        public ReactiveProperty<double> CanvasScale { get; set; } = new ReactiveProperty<double>(1f);

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

        public DelegateCommand SelectFileCommand { get; set; }
    }
}
