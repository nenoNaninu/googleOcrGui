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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace googoleOcr
{
    class ViewModel : INotifyPropertyChanged
    {
        ScrollViewer scrollView;
        Canvas canvasInScroll;

        public ReactiveProperty<string> SelectedFilename { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> SelectedDstDir { get; set; } = new ReactiveProperty<string>("");
        public ViewModel(ScrollViewer scrollView, Canvas canvasInScroll)
        {
            this.scrollView = scrollView;
            this.canvasInScroll = canvasInScroll;
            CanvasHeight.Value = (int)scrollView.ActualHeight;
            CanvasWidth.Value = (int)scrollView.ActualWidth;
            Scale.Subscribe(x =>
            {
                Scale.Value = x;
                CanvasScale.Value = x / 100f;
                CanvasWidth.Value = (int)(CanvasScale.Value * scrollView.ActualWidth);
                CanvasHeight.Value = (int)(CanvasScale.Value * scrollView.ActualHeight);
            });
            SelectFileCommand = new DelegateCommand(SelectFileExcute, CanOpenFileDiaolg);
            ExcuteOcrCommand = new DelegateCommand(ExcuteOcr, CanExcuteOcr);
        }
        public ViewModel()
        {
            SelectFileCommand = new DelegateCommand(SelectFileExcute, CanOpenFileDiaolg);
            ExcuteOcrCommand = new DelegateCommand(ExcuteOcr, CanExcuteOcr);
            SelectDstDirCommand = new DelegateCommand(SelectDstDir, CanSelectDstDir);
        }
        public ReactiveProperty<int> CanvasHeight { get; set; } = new ReactiveProperty<int>();

        public ReactiveProperty<int> CanvasWidth { get; set; } = new ReactiveProperty<int>();

        public ReactiveProperty<int> Scale { get; set; } = new ReactiveProperty<int>(100);

        public ReactiveProperty<double> CanvasScale { get; set; } = new ReactiveProperty<double>(1f);

        List<MoveableTextbox> moveableTextBoxList = new List<MoveableTextbox>();

        private async Task excuteGoogleOcr(string fileName, string dstDir)
        {
            Model model = new Model(this);
            model.MaxValue.Subscribe(x =>
            {
                this.ProgressBarValueMax.Value = x;
            });

            model.ProgressValue.Subscribe(x =>
            {
                this.ProgressValue.Value = x;
            });
            var boundingTextList = await model.GetOcrData(fileName, dstDir);
            //for (int i = 0; i < boundingTextList.Count; i++)
            //{
            //    MoveableTextbox moveableTextBox = new MoveableTextbox();
            //    moveableTextBoxList.Add(moveableTextBox);
            //    var it = boundingTextList[i];
            //    moveableTextBox.SetProperty(it.Top, it.Left, it.Height, it.Width, it.Text);
            //    Canvas.SetLeft(moveableTextBox, it.Left);
            //    Canvas.SetTop(moveableTextBox, it.Top);
            //    this.canvasInScroll.Children.Add(moveableTextBox);
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public DelegateCommand SelectFileCommand { get; }

        private void SelectFileExcute()
        {
            var dialog = new CommonOpenFileDialog("ファイルの選択");
            //CommonFileDialogFilterCollection filters = new CommonFileDialogFilterCollection();
            //filters.Add(new CommonFileDialogFilter("Rich Text Files (*.rtf)", ".rtf"));
            //dialog.Filters = filters;
            //dialog.Filters.Add(new CommonFileDialogFilter("jpg",))
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.SelectedFilename.Value = dialog.FileName;
                this.SelectedDstDir.Value = Path.GetDirectoryName(dialog.FileName);
            }
        }

        private bool CanOpenFileDiaolg()
        {
            return canOpenFileDialog;
        }

        private bool canOpenFileDialog = true;

        public DelegateCommand ExcuteOcrCommand { get; }

        private async void ExcuteOcr()
        {
            canOpenFileDialog = false;
            //this.canvasInScroll.Children.Clear();
            await excuteGoogleOcr(this.SelectedFilename.Value, this.SelectedDstDir.Value);
            this.SelectedFilename.Value = null;
            this.SelectedDstDir.Value = null;
            canOpenFileDialog = true;
            ProgressBarValueMax.Value = 100;
            ProgressValue.Value = 0;
            MessageBox.Show("終了しました");
        }

        private bool CanExcuteOcr()
        {
            return this.SelectedFilename.Value == null ? false : true;
        }

        public DelegateCommand SelectDstDirCommand { get;}

        private void SelectDstDir()
        {
            var dialog = new CommonOpenFileDialog("フォルダの選択");
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.SelectedDstDir.Value = dialog.FileName;
            }
        }

        private bool CanSelectDstDir()
        {
            return this.canOpenFileDialog;
        }

        //これ以降プログレスバー関係。
        public ReactiveProperty<int> ProgressBarValueMax { get; set; } = new ReactiveProperty<int>(100);
        public ReactiveProperty<int> ProgressValue { get; set; } = new ReactiveProperty<int>(0);

        

    }
}
