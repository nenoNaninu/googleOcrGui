﻿using System;
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
        List<BoundingText> boundingTextList = null;
        ViewModel parentViewModel = null;

        public List<BoundingText> BoundingTextList
        {
            get { return boundingTextList; }
        }

        public Model(ViewModel viewModel)
        {
            this.parentViewModel = viewModel;
            this.boundingTextList = new List<BoundingText>();
        }

        public List<BoundingText> GetOcrData(string fileName)
        {

            sysDraw.Image img = new sysDraw.Bitmap(fileName);
            float bairitu = 1.8f;
            this.parentViewModel.CanvasWidth = (int)(img.Width*bairitu);
            this.parentViewModel.CanvasHeight = (int)(img.Height*bairitu);
            
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            var imageForGoogle = Image.FromFile(fileName);
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
                        string text = "";
                        foreach (var word in paragraph.Words)
                        {
                            Debug.Print($"    Word: {string.Join("", word.Symbols.Select(s => s.Text))}");
                            text += string.Join("", word.Symbols.Select(s => s.Text));
                        }
                        int top = paragraph.BoundingBox.Vertices[0].Y;
                        int left = paragraph.BoundingBox.Vertices[0].X;
                        int height = paragraph.BoundingBox.Vertices[2].Y - top;
                        int width = paragraph.BoundingBox.Vertices[2].X - left;
                        this.boundingTextList.Add(new BoundingText(text, left, top, width, height));
                    }
                }
            }

            return this.boundingTextList;
        }
    }
}
