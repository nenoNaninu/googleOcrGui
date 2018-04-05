using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using System.Windows;

namespace googoleOcr
{
    class Model
    {
        public void GetOcrData()
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromFile("C: \\Users\\naritomi\\Desktop\\AzureTest\\ocrSamples\\cap2.PNG");
            var response = client.DetectText(image);
            foreach(var it in response)
            {
                MessageBox.Show(it.ToString());
            }
        }
    }
}
