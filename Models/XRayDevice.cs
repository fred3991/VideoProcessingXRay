using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VideoProcessingXRay.Models
{/// <summary>
/// генерирует пачку изображений, которую мы потом будем использовать
/// </summary>
    public class XRayDevice
    {
        public List<string> ImagesSet = new List<string>();

        public XRayDevice()
        {
            GenerateImagesDB();
        }

        public void GenerateImagesDB()
        {
            int fileCount = Directory.GetFiles(@"C:\Users\Viva_\Source\Repos\VideoProcessingXRay\ImageDB").Length;
            for (int i=1; i< fileCount+1; i++)
            {
                string filePath = @"C:\Users\Viva_\Source\Repos\VideoProcessingXRay\ImageDB\"+i+".jpg";
                ImagesSet.Add(filePath);
            }       
        }
    }
}
