using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

using VideoProcessingXRay.Commands.Base;
using VideoProcessingXRay.Models;

namespace VideoProcessingXRay.Models
{/// <summary>
/// генерирует пачку изображений, которую мы потом будем использовать
/// </summary>
    public class XRayDevice
    {
        public List<string> ImagesString = new List<string>();

        public List<Image> ImagesList = new List<Image>();

        //public static string projectPath = "C:\\Users\\FedorovEA\\source\\repos\\VideoProcessingXRay\\";
        //public static string projectPath = System.IO.Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName;
        public static string projectPath = Path.Combine(Environment.CurrentDirectory);

        public XRayDevice()
        {
            GenerateImagesDB();
        }

        public void GenerateImagesDB()
        {

            //string path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            //int pos = path.LastIndexOf("\\"); 
            //var result = path.Remove(pos, path.Length - pos);
            var finalPath = projectPath+"\\ImageDB\\";

            int fileCount = Directory.GetFiles(finalPath).Length;
            for (int i=1; i< fileCount+1; i++)
            {
                string filePath = finalPath + i + ".tif";

                ImagesString.Add(filePath);

              //ImagesList.Add(Image.FromFile(filePath));

            }       
        }
    }
}
