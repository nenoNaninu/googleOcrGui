using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using System.Windows;
using System.Diagnostics;
using sysDraw = System.Drawing;
using System.IO;
using ImageMagick;
using Reactive.Bindings;

namespace googoleOcr
{
    class Model
    {
        List<BoundingText> boundingTextList = null;
        ViewModel parentViewModel = null;

        public ReactiveProperty<int> MaxValue { get; set; } = new ReactiveProperty<int>(100);
        public ReactiveProperty<int> ProgressValue { get; set; } = new ReactiveProperty<int>(0);

        public List<BoundingText> BoundingTextList
        {
            get { return boundingTextList; }
        }

        public Model(ViewModel viewModel)
        {
            this.parentViewModel = viewModel;
            this.boundingTextList = new List<BoundingText>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">選択されたpdfのフルパス</param>
        private void OcrPdfData(string fileName,string dstDir)
        {
            MagickReadSettings setting = new MagickReadSettings();
            setting.Density = new Density(300, 300);
            MagickImageCollection imgs = new MagickImageCollection(fileName, setting);
            
            var pdfname = Path.GetFileNameWithoutExtension(fileName);
            var dirFullpath = Path.GetDirectoryName(fileName);
            this.MaxValue.Value = imgs.Count -1;
            this.ProgressValue.Value = 0;
            for (int i = 0, n = imgs.Count; i < n; i++)
            {
                this.ProgressValue.Value = i;
                string pngName = string.Format("{0}-{1}--.png", pdfname, i);
                imgs[i].Write(dstDir + "\\" + pngName);
                this.boundingTextList.Clear();
                OcrImageData(dstDir + "\\" + pngName, dstDir);
            }
            imgs.Clear();
        }


        private void OcrImageData(string fileName,string dstDir)
        {
            sysDraw.Image img = new sysDraw.Bitmap(fileName);
            float bairitu = 1.8f;
            this.parentViewModel.CanvasWidth.Value = (int)(img.Width * bairitu);
            this.parentViewModel.CanvasHeight.Value = (int)(img.Height * bairitu);
            
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            var imageForGoogle = Image.FromFile(fileName);
            TextAnnotation response = client.DetectDocumentText(imageForGoogle);
            if(response == null)
            {
                return;
            }
            foreach (var page in response.Pages)
            {
                foreach (var block in page.Blocks)
                {
                    string box = string.Join(" - ", block.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                    Debug.Print($"Block {block.BlockType} at {box}");
                    foreach (var paragraph in block.Paragraphs)
                    {
                        box = string.Join(" - ", paragraph.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                        Debug.Print($"  Paragraph at {box}");
                        string text = "";
                        foreach (var word in paragraph.Words)
                        {
                            Debug.Print($"    Word: {string.Join("", word.Symbols.Select(s => s.Text))}");
                            text += string.Join("", word.Symbols.Select(s => s.Text))+" ";
                        }
                        int top = paragraph.BoundingBox.Vertices[0].Y;
                        int left = paragraph.BoundingBox.Vertices[0].X;
                        int height = paragraph.BoundingBox.Vertices[2].Y - top;
                        int width = paragraph.BoundingBox.Vertices[2].X - left;
                        this.boundingTextList.Add(new BoundingText(text, left, top, width, height));
                    }
                }
            }
            Write2FileOcrResult(this.boundingTextList, fileName,dstDir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">OCRしたいデータのフルパス</param>
        /// <param name="dstDir">出力したいディレクトリのフルパス</param>
        /// <returns></returns>
        public async Task<List<BoundingText>> GetOcrData(string fileName, string dstDir)
        {
            await Task.Run(() =>
            {
                var extension = Path.GetExtension(fileName);
                if (extension == ".pdf")
                {
                    OcrPdfData(fileName, dstDir);
                }
                else
                {
                    OcrImageData(fileName, dstDir);
                }
            });
            return this.boundingTextList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName">処理を行っているファイルのフルパス</param>
        void Write2FileOcrResult(List<BoundingText> list, string fileName,string dstDir)
        {
            string fileNameWithoutEx = Path.GetFileNameWithoutExtension(fileName);
            string dirFullpath = Path.GetDirectoryName(fileName);
            FileStream fs = new FileStream(dstDir+"\\"+fileNameWithoutEx + "ocrResult.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach (var it in list)
            {
                sw.WriteLine(it.Text);
                sw.WriteLine("========");
            }

            sw.Close();
            fs.Close();
        }
    }
}
