using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace VideoProcessingXRay.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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

        private string _activeFrame;
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

        private int _stopFrameNum = 40;
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
            object objxr = 0;
            object objdt = 0;
            TimerCallback xrtm = new TimerCallback(TimerClickXRay);
            var secondPerFrame = Convert.ToInt32((1.0 / FramePerSecond)*1000);
            XRayTimer = new Timer(xrtm, objxr, 0, secondPerFrame);

            TimerCallback dttm = new TimerCallback(TimerClickDateTime);
            DateTimeTimer = new Timer(dttm, objdt, 0, 1000);

        }

        private bool CanStartShowFramesCommandExecute(object p) => true;
        public ICommand StopShowFrames { get; }
        private void OnStopShowFramesCommandExecuted(object p)
        {
            XRayTimer.Change(Timeout.Infinite, Timeout.Infinite);
            DateTimeTimer.Change(Timeout.Infinite, Timeout.Infinite);
            GC.Collect();
        }
        private bool CanStopShowFramesCommandExecute(object p) => true;


        public ICommand ResolutionConvert { get; }
        private void OnResolutionConvertCommandExecuted(object p)
        {
            //Thread ResolutionConverterThread = new Thread(new ThreadStart(ResizeImagesThread));
            //ResolutionConverterThread.Start(); // запускаем поток

            ResizeImagesThread();
        }
        private bool CanResolutionConvertCommandExecute(object p) => true;


        public ICommand VideoConvert { get; }
        private void OnVideoConvertCommandExecuted(object p)
        {
            SaveVideo();
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
            // Выключаем предыдущий
            XRayTimer.Change(Timeout.Infinite, Timeout.Infinite);
            DateTimeTimer.Change(Timeout.Infinite, Timeout.Infinite);
            GC.Collect();
        }
        private bool CanStopShowResizedFramesCommandExecute(object p) => true;


        public MainWindowViewModel()
        {
            xrDevice = new XRayDevice();
            
            StartShowFrames = new LambdaCommand(OnStartShowFramesCommandExecuted, CanStartShowFramesCommandExecute);
            StopShowFrames = new LambdaCommand(OnStopShowFramesCommandExecuted, CanStopShowFramesCommandExecute);

            ResolutionConvert = new LambdaCommand(OnResolutionConvertCommandExecuted, CanResolutionConvertCommandExecute);
            VideoConvert = new LambdaCommand(OnVideoConvertCommandExecuted, CanVideoConvertCommandExecute);

            StartShowResizedFrames = new LambdaCommand(OnStartShowResizedFramesCommandExecuted, CanStartShowResizedFramesCommandExecute);
            StopShowResizedFrames = new LambdaCommand(OnStopShowResizedFramesCommandExecuted, CanStopShowResizedFramesCommandExecute);

        }

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
            }
        }


        public void TimerClickImagesResized(object obj)
        {
            int x = (int)obj;

            if (ActiveFrameNum < StopFrameNum)
            {
                ActiveFrame = ImagesStringResolution[ActiveFrameNum];
                ActiveFrameNum++;
                FrameNum = ActiveFrameNum;
            }
            else
            {
                ActiveFrameNum = 1;
            }
        }



        public void TimerClickDateTime(object obj)
        {
            int x = (int)obj;
            CurrentDateTime = DateTime.Now.ToString();           
        }



        public void ResizeImagesThread()
        {
           
 
            var finalPath = @"C:\Users\FedorovEA\source\repos\VideoProcessingXRay\ImageDBResized\";
            int imgNum = 1;
            ImagesListResolution = new List<Image>();
            //try
            //{                
                foreach (Image img in xrDevice.ImagesList)
                {
                    Image convertedImage = ResizeImage(img, XRes, YRes);
                    ImagesListResolution.Add(convertedImage);        
                    Image imgCopyToSave = (Image)convertedImage.Clone();
                    imgCopyToSave.Save(finalPath + "convertedImage_"+XRes+"_"+YRes+"_"+ + imgNum + ".jpg");

                    imgNum++;
                    ImagesStringResolution.Add(finalPath + "convertedImage_" + XRes + "_" + YRes + "_" + +imgNum + ".jpg");

                }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            GC.Collect();

        }

        public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(filePath, fileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
            }
        }

        public static Stream ToStream(Image image)
        {
            var stream = new MemoryStream();

            image.Save(stream, image.RawFormat);
            stream.Position = 0;

            return stream;
        }




        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
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


        public void SaveVideo()
        {
            FFmpegLoader.FFmpegPath = @"C:\Users\FedorovEA\Downloads\ffmpeg-n4.4-178-g4b583e5425-win64-gpl-shared-4.4\bin";

            var settings = new VideoEncoderSettings(width: XRes, height: YRes, framerate: FramePerSecond, codec: VideoCodec.H264);
            settings.EncoderPreset = EncoderPreset.Fast;
            settings.CRF = 17;
            var file = MediaBuilder.CreateContainer(@"C:\Users\FedorovEA\source\repos\VideoProcessingXRay\Video\out.mp4").WithVideo(settings).Create();
            var files = Directory.GetFiles(@"C:\Users\FedorovEA\source\repos\VideoProcessingXRay\ImageDBResized");
            foreach (var inputFile in files)
            {
                var binInputFile = File.ReadAllBytes(inputFile);
                var memInput = new MemoryStream(binInputFile);
                var bitmap = Bitmap.FromStream(memInput) as Bitmap;
                var rect = new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size);
                var bitLock = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                var bitmapData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, bitmap.Size);
                file.Video.AddFrame(bitmapData); // Encode the frame
                bitmap.UnlockBits(bitLock);

            }

            file.Dispose();
        }

    }
}
