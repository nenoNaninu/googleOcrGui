using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using System.Windows;
using System.Diagnostics;
using sysDraw = System.Drawing;

namespace googoleOcr
{
    class Model
    {
        ViewModel parentViewModel;
        public Model(ViewModel viewModel)
        {
            this.parentViewModel = viewModel;
        }
        public void GetOcrData()
        {
            sysDraw.Image img = new sysDraw.Bitmap("C:\\Users\\naritomi\\Desktop\\AzureTest\\ocrSamples\\cap2.PNG");
            this.parentViewModel.CanvasWidth = img.Width;
            this.parentViewModel.CanvasHeight = img.Height;
            
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            var imageForGoogle = Image.FromFile("C:\\Users\\naritomi\\Desktop\\AzureTest\\ocrSamples\\cap2.PNG");
            TextAnnotation response = client.DetectDocumentText(imageForGoogle);
            foreach (var page in response.Pages)
            {
                foreach(var block in page.Blocks)
                {
                    string box = string.Join(" - ", block.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                    Debug.Print($"Block {block.BlockType} at {box}");
                    foreach (var paragraph in block.Paragraphs)
                    {
                        box = string.Join(" - ", paragraph.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                        Debug.Print($"  Paragraph at {box}");
                        foreach (var word in paragraph.Words)
                        {
                            Debug.Print($"    Word: {string.Join("", word.Symbols.Select(s => s.Text))}");
                        }
                    }
                }
            }
        }
    }
}
