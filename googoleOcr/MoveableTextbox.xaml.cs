using Reactive.Bindings;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace googoleOcr
{
    /// <summary>
    /// MoveableTextbox.xaml の相互作用ロジック
    /// </summary>
    public partial class MoveableTextbox : UserControl, INotifyPropertyChanged
    {
        //private int canvasLeftPos = 72;
        public ReactiveProperty<int> CanvasLeftPos { get; set; } = new ReactiveProperty<int>(80);
        //public int CanvasLeftPos { get; set; } = 37;
        public ReactiveProperty<int> CanvasTopPos { get; } = new ReactiveProperty<int>(80);
        public ReactiveProperty<int> ThumbWidth { get; } = new ReactiveProperty<int>(160);
        public ReactiveProperty<int> ThumbHeight { get; } = new ReactiveProperty<int>(160);
        public ReactiveProperty<int> RectAngleWidth { get; } = new ReactiveProperty<int>(140);
        public ReactiveProperty<int> RectAngleHeight { get; } = new ReactiveProperty<int>(140);
        public ReactiveProperty<int> TextBoxWidth { get; } = new ReactiveProperty<int>(140);
        public ReactiveProperty<int> TextBoxHeight { get; } = new ReactiveProperty<int>(70);
        public ReactiveProperty<string> TextContent { get; } = new ReactiveProperty<string>("Unity CHAN!");
    
        private void DragDelta(object sender,DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
        }

        public void SetProperty(int top, int left, int height, int width, string content)
        {
            if (height < 30)
            {
                height = 30;
            }
            else
            {
                height += 10;
            }
            if (width < 30)
            {
                width = 30;
            }
            else
            {
                width += 10;
            }

            TextBoxHeight.Value = height;
            TextBoxWidth.Value = width;

            RectAngleHeight.Value = height + 20;
            RectAngleWidth.Value = width + 20;

            TextContent.Value = content;

            CanvasLeftPos.Value = left;
            CanvasTopPos.Value = top;

            ThumbHeight.Value = height + 30;
            ThumbWidth.Value = width + 30;
        }

        public MoveableTextbox()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
