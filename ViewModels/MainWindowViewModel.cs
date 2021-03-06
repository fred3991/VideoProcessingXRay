using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using VideoProcessingXRay.Commands.Base;
using VideoProcessingXRay.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Linq;

namespace VideoProcessingXRay.ViewModels
{
    
    internal class MainWindowViewModel : ViewModel
    {
        //public static string projectPath = "C:\\Users\\FedorovEA\\source\\repos\\VideoProcessingXRay\\";
        //public static string projectPath = System.IO.Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName;
        public static string projectPath = Path.Combine(Environment.CurrentDirectory);


        public Timer XRayTimer;

        public Timer DateTimeTimer;

        private string _currentDateTime;
        public string CurrentDateTime
        {
            get => _currentDateTime;
            set => Set(ref _currentDateTime, value);
        }

        public List<Image> ImagesListResolution = new List<Image>();
        public List<string> ImagesStringResolution = new List<string>();


        public XRayDevice xrDevice;

        private string _activeFrame = projectPath+"\\ImageDB\\1.tif";
        public string ActiveFrame
        {
            get => _activeFrame;
            set => Set(ref _activeFrame, value);
        }

        private int _framePerSecond = 24;
        public int FramePerSecond
        {
            get => _framePerSecond;
            set => Set(ref _framePerSecond, value);
        }

        private int _startFrameNum = 1;
        public int StartFrameNum
        {
            get => _startFrameNum;
            set => Set(ref _startFrameNum, value);
        }

        private int _stopFrameNum = 40; // Максимум кадров 40, меньше можно, больше нельзя
        public int StopFrameNum
        {
            get => _stopFrameNum;
            set => Set(ref _stopFrameNum, value);
        }


        private int _activeFrameNum = 1;
        public int ActiveFrameNum
        {
            get => _activeFrameNum;
            set => Set(ref _activeFrameNum, value);
        }

        private int _frameNum = 1;
        public int FrameNum
        {
            get => _frameNum;
            set => Set(ref _frameNum, value);
        }

        private int _frameRate;
        public int FrameRate
        {
            get => _frameRate;
            set => Set(ref _frameRate, value);
        }

        private int _xRes = 256;
        public int XRes
        {
            get => _xRes;
            set => Set(ref _xRes, value);
        }

        private int _yRes = 256;
        public int YRes
        {
            get => _yRes;
            set => Set(ref _yRes, value);
        }


        public ICommand StartShowFrames { get; }
        private void OnStartShowFramesCommandExecuted(object p)
        {

            // директория проекта
            string projectPath = System.IO.Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName;
            //Исполняемая директория
            //string projectPath = Path.Combine(Environment.CurrentDirectory);


            // Open a Uri and decode a BMP image
            Uri myUri = new Uri(projectPath+@"\tstImageDB\1.tif");
            TiffBitmapDecoder decoder2 = new TiffBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];

            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(projectPath + @"\tstImageDB\1.tif");
            myBitmapImage.EndInit();




            BitmapPalette myPalette = new BitmapPalette(myBitmapImage, 256);


            var pF = myBitmapImage.Format;

            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();

            newFormatedBitmapSource.BeginInit();

            newFormatedBitmapSource.Source = myBitmapImage;
            newFormatedBitmapSource.DestinationFormat = createPixelFormat();
            newFormatedBitmapSource.EndInit();

            Stream imageStreamSource = new FileStream(projectPath + @"\tstImageDB\1.tif", FileMode.Open, FileAccess.Read, FileShare.Read);
            var decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];


            int stride = (int)bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel/8);
            byte[] pixels = new byte[(int)bitmapSource.PixelHeight * stride];

            newFormatedBitmapSource.CopyPixels(pixels, stride, 0);

            Array.Sort(pixels);

            int Min = 0;
            int Max = 65000;
            Random randNum = new Random();
            int[] testArr = Enumerable
                .Repeat(0, 4792608)
                .Select(i => randNum.Next(Min, Max))
                .ToArray();

            byte[] result = new byte[testArr.Length * sizeof(int)];
            Buffer.BlockCopy(testArr, 0, result, 0, result.Length);

            BitmapSource btmsrs = BitmapSource.Create(1548, 1548, 96, 96, PixelFormats.Gray16, myPalette, result, stride);


            var stream = new FileStream(projectPath + @"\tstImageDB\saved2.tif", FileMode.Create);
            var encoder = new TiffBitmapEncoder();
            encoder.Compression = TiffCompressOption.Zip;
            encoder.Frames.Add(BitmapFrame.Create(btmsrs));
            encoder.Save(stream);


            //var bitmap = (BitmapSource)new ImageSourceConverter().ConvertFrom(pixels);

            //BitmapImage image = ToImage(pixels);
            //FileStream stream = new FileStream(projectPath + @"\tstImageDB\saved.tif", FileMode.Create, FileAccess.ReadWrite);
            //encoder = new TiffBitmapEncoder();
            //encoder.Compression = TiffCompressOption.Zip;
            //encoder.Frames.Add(BitmapFrame.Create(image));
            //encoder.Save(stream);


            //PngBitmapEncoder encoder = new PngBitmapEncoder();              
            //encoder.Interlace = PngInterlaceOption.On;
            //encoder.Frames.Add(BitmapFrame.Create(newFormatedBitmapSource));
            //encoder.Save(stream);




            //var imgH = img.PixelHeight;
            //var imgW = img.PixelWidth;

            //var pxlF = img.Format;

            //BitmapImage newBmp = img.Clone();

            //Graphics gr = Graphics.FromImage(newBmp);



            //imgH = clone.Height;
            //imgW = clone.Width;

            //pxlF = clone.PixelFormat;

            //clone.Save(@"C:\Users\Viva_\Source\Repos\VideoProcessingXRay\tstImageDB\1.tif", ImageFormat.Tiff);




            object objxr = 0;
            object objdt = 0;
            TimerCallback xrtm = new TimerCallback(TimerClickXRay);
            var secondPerFrame = Convert.ToInt32((1.0 / FramePerSecond) * 1000);
            XRayTimer = new Timer(xrtm, objxr, 0, secondPerFrame);

            TimerCallback dttm = new TimerCallback(TimerClickDateTime);
            DateTimeTimer = new Timer(dttm, objdt, 0, 1000);
        }

        public BitmapImage convertByteToBitmapImage(Byte[] bytes)
        {
            return null;
        }

        //public static void SaveJpg(string fileName, int sizeX, int sizeY, ushort[] imData)
        //{
        //    var bitmap = new Bitmap(sizeX, sizeY, System.Windows.Media.PixelFormat.);
        //    int count = 0;
        //    for (int y = 0; y < sizeY; y++)
        //    {
        //        for (int x = 0; x < sizeX; x++)
        //        {
        //            bitmap.SetPixel(x, y, Color.FromArgb(imData[count], imData[count], imData[count]));
        //            count++;
        //        }
        //    }
        //    bitmap.Save(fileName, ImageFormat.Jpeg);

        //}


        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.DecodePixelHeight = 1548;
                image.DecodePixelWidth = 1548;
                image.EndInit();
                return image;
            }
        }


        private bool CanStartShowFramesCommandExecute(object p) => true;
        public ICommand StopShowFrames { get; }
        private void OnStopShowFramesCommandExecuted(object p)
        {
            XRayTimer.Change(Timeout.Infinite, Timeout.Infinite);
            DateTimeTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        private bool CanStopShowFramesCommandExecute(object p) => true;


        public ICommand ResolutionConvert { get; }
        private void OnResolutionConvertCommandExecuted(object p)
        {
            try
            {
                ResizeImagesThread();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
        }
        private bool CanResolutionConvertCommandExecute(object p) => true;


        public ICommand VideoConvert { get; }
        private void OnVideoConvertCommandExecuted(object p)
        {
            try
            {
                SaveVideo();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

           
        }
        private bool CanVideoConvertCommandExecute(object p) => true;

        public ICommand StartShowResizedFrames { get; }
        private void OnStartShowResizedFramesCommandExecuted(object p)
        {
            object objxr = 0;
            object objdt = 0;
            TimerCallback xrtm = new TimerCallback(TimerClickImagesResized);
            var secondPerFrame = Convert.ToInt32((1.0 / FramePerSecond) * 1000);
            XRayTimer = new Timer(xrtm, objxr, 0, secondPerFrame);
            TimerCallback dttm = new TimerCallback(TimerClickDateTime);
            DateTimeTimer = new Timer(dttm, objdt, 0, 1000);
        }
        private bool CanStartShowResizedFramesCommandExecute(object p) => true;



        public ICommand StopShowResizedFrames { get; }
        private void OnStopShowResizedFramesCommandExecuted(object p)
        {
            XRayTimer.Change(Timeout.Infinite, Timeout.Infinite);
            DateTimeTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        private bool CanStopShowResizedFramesCommandExecute(object p) => true;


        public MainWindowViewModel()
        {
            xrDevice = new XRayDevice();
            //FFmpegLoader.FFmpegPath = projectPath+"\\ffmpeglib";
            FFmpegLoader.FFmpegPath = Path.Combine(Environment.CurrentDirectory, @"ffmpeglib\");

            //string ppp = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //var p = System.IO.Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName;

            string pathsda = Path.Combine(Environment.CurrentDirectory);

            StartShowFrames = new LambdaCommand(OnStartShowFramesCommandExecuted, CanStartShowFramesCommandExecute);
            StopShowFrames = new LambdaCommand(OnStopShowFramesCommandExecuted, CanStopShowFramesCommandExecute);

            ResolutionConvert = new LambdaCommand(OnResolutionConvertCommandExecuted, CanResolutionConvertCommandExecute);
            VideoConvert = new LambdaCommand(OnVideoConvertCommandExecuted, CanVideoConvertCommandExecute);

            StartShowResizedFrames = new LambdaCommand(OnStartShowResizedFramesCommandExecuted, CanStartShowResizedFramesCommandExecute);
            StopShowResizedFrames = new LambdaCommand(OnStopShowResizedFramesCommandExecuted, CanStopShowResizedFramesCommandExecute);

        }

        /// <summary>
        /// Смена кадров исходных изображений по таймеру
        /// </summary>
        /// <param name="obj"></param>
        public void TimerClickXRay(object obj)
        {
            int x = (int)obj;

            if (ActiveFrameNum < StopFrameNum)
            {
                ActiveFrame = xrDevice.ImagesString[ActiveFrameNum];
                ActiveFrameNum++;
                FrameNum = ActiveFrameNum;
            }
            else
            {
                ActiveFrameNum = 1;
                FrameNum = ActiveFrameNum;
                ActiveFrame = xrDevice.ImagesString[ActiveFrameNum];
                ActiveFrameNum++;
            }
        }

        /// <summary>
        /// Смена кадров измененного изображения
        /// </summary>
        /// <param name="obj"></param>
        public void TimerClickImagesResized(object obj)
        {
            int x = (int)obj;

            if (ActiveFrameNum < StopFrameNum-1)
            {
                ActiveFrame = ImagesStringResolution[ActiveFrameNum];
                ActiveFrameNum++;
                FrameNum = ActiveFrameNum;
            }
            else
            {
                ActiveFrameNum = 1;
                FrameNum = ActiveFrameNum;
                ActiveFrame = ImagesStringResolution[ActiveFrameNum];
                ActiveFrameNum++;
            }
        }


        /// <summary>
        /// Таймер времени на форме
        /// </summary>
        /// <param name="obj"></param>
        public void TimerClickDateTime(object obj)
        {
            int x = (int)obj;
            CurrentDateTime = DateTime.Now.ToString();
        }



        /// <summary>
        /// Изменяем исходные изображения для новых xRes yRes
        /// </summary>
        public void ResizeImagesThread()
        {
            var finalPath = projectPath+"\\ImageDBResized\\";
            DeleteAllFilesInFolder(finalPath);

            int imgNum = 1;
            ImagesListResolution = new List<Image>();
            foreach (Image img in xrDevice.ImagesList)
            {
                Image convertedImage = ResizeImage(img, XRes, YRes);
                ImagesListResolution.Add(convertedImage);
                Image imgCopyToSave = (Image)convertedImage.Clone();
                imgCopyToSave.Save(finalPath + "convertedImage_" + XRes + "_" + YRes + "_" + +imgNum + ".tif");

                imgNum++;
                ImagesStringResolution.Add(finalPath + "convertedImage_" + XRes + "_" + YRes + "_" + +imgNum + ".tif");

            }
        }

        /// <summary>
        /// чтоб не ловить ошибку GDI+
        /// </summary>
        /// <param name="finalPath"></param>
        public void DeleteAllFilesInFolder(string finalPath)
        {          
            DirectoryInfo dirInfo = new DirectoryInfo(finalPath);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }



        /// <summary>
        /// Берем одно изображение и изменяем ширину/высоту
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }


        public System.Windows.Media.PixelFormat createPixelFormat()
        {
            // Create a PixelFormat object.
            System.Windows.Media.PixelFormat myPixelFormat = new System.Windows.Media.PixelFormat();

            // Make this PixelFormat a Gray32Float pixel format.
            myPixelFormat = PixelFormats.Gray16;

            // Get the number of bits-per-pixel for this format. Because
            // the format is "Gray32Float", the float value returned will be 32.
            int bpp = myPixelFormat.BitsPerPixel;

            // Get the collection of masks associated with this format.
            IList<PixelFormatChannelMask> myChannelMaskCollection = myPixelFormat.Masks;

            // Capture the mask info in a string.
            String stringOfValues = " ";
            foreach (PixelFormatChannelMask myMask in myChannelMaskCollection)
            {
                IList<byte> myBytesCollection = myMask.Mask;
                foreach (byte myByte in myBytesCollection)
                {
                    stringOfValues = stringOfValues + myByte.ToString();
                }
            }

            // Return the PixelFormat which, for example, could be 
            // used to set the pixel format of a bitmap by using it to set
            // the DestinationFormat of a FormatConvertedBitmap.
            return myPixelFormat;
        }



        /// <summary>
        /// Берем папку ImageDBResized и делаем видео с заданным ФПС и шириной/высотой;
        /// </summary>
        public void SaveVideo()
        {
           
                var settings = new VideoEncoderSettings(width: XRes, height: YRes, framerate: FramePerSecond, codec: VideoCodec.H264);
                settings.EncoderPreset = EncoderPreset.Fast;
                settings.CRF = 17;
                var file = MediaBuilder.CreateContainer(projectPath + "\\Video\\_" + XRes + "_" + YRes + "_FPS_" + FramePerSecond + "_.mp4").WithVideo(settings).Create();
                var files = Directory.GetFiles(projectPath + "\\ImageDBResized");
                foreach (var inputFile in files)
                {
                    var binInputFile = File.ReadAllBytes(inputFile);
                    var memInput = new MemoryStream(binInputFile);
                    var bitmap = Bitmap.FromStream(memInput) as Bitmap;
                    var rect = new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size);
                    var bitLock = bitmap.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    var bitmapData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, bitmap.Size);
                    file.Video.AddFrame(bitmapData); // Encode the frame
                    bitmap.UnlockBits(bitLock);
                }
                file.Dispose();

           
         

           
        }





    }
}
