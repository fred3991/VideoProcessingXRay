using System;

using System.Windows.Input;
using System.Windows.Threading;

using VideoProcessingXRay.Commands.Base;
using VideoProcessingXRay.Models;

namespace VideoProcessingXRay.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
     
        private string _titile = "Video Processing";
        public string Title
        {
            get => _titile;
            set => Set(ref _titile, value);
        }

        public XRayDevice xrDevice; 

        private string _activeFrame = @"C:\Users\Viva_\Source\Repos\VideoProcessingXRay\ImageDB\1.jpg";
        public string ActiveFrame
        {
            get => _activeFrame;
            set => Set(ref _activeFrame, value);
        }

        private int _startFrameNum = 1;
        public int StartFrameNum
        {
            get => _startFrameNum;
            set => Set(ref _startFrameNum, value);
        }

        private int _stopFrameNum = 30;
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

        //private Image _currentImage = Image.FromFile
        ////public Image CurrentImage
        ////{
        ////    get => _currentImage;
        ////    set => Set(ref _currentImage, value);
        ////}




        #region Task1

        private int _frameRate;
        public int FrameRate
        {
            get => _frameRate;
            set => Set(ref _frameRate, value);
        }

        private int _xRes;
        public int XRes
        {
            get => _xRes;
            set => Set(ref _xRes, value);
        }

        private int _yRes;
        public int Yres
        {
            get => _yRes;
            set => Set(ref _yRes, value);
        }


        public ICommand TaskOneGenerateCommand { get; }
        private void OnTaskOneGenerateCommandExecuted(object p)
        {
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);

            var frameRate = 1 / FrameNum;
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += timer_Tick;
            timer.Start();


            //var inputFile = @"C:\Users\Viva_\Source\Repos\VideoProcessingXRay\ImageDB\1.jpg";
            //var binInputFile = File.ReadAllBytes(inputFile);
            //var memInput = new MemoryStream(binInputFile);
            //var bitmap = Bitmap.FromStream(memInput) as Bitmap;



        }
        private bool CanTaskOneGenerateCommandExecute(object p) => true;
        #endregion


        public ICommand EmptyCommand { get; }
        private void OnEmptyCommandExecuted(object p)
        {


        }
        private bool CanEmptyCommandExecute(object p) => true;

        public MainWindowViewModel()
        {
            xrDevice = new XRayDevice();
            EmptyCommand = new LambdaCommand(OnEmptyCommandExecuted, CanEmptyCommandExecute);
            TaskOneGenerateCommand = new LambdaCommand(OnTaskOneGenerateCommandExecuted, CanTaskOneGenerateCommandExecute);


        }



        public void TimerThread()
        {
           

        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (ActiveFrameNum < StopFrameNum)
            {
                ActiveFrame = @"c:\users\viva_\source\repos\videoprocessingxray\imagedb\" + ActiveFrameNum + ".jpg";
                ActiveFrameNum++;
            }
            else
            {
                ActiveFrameNum = 1;
            }
                     
        }

        //public void ImageSequenceThread()
        //{
        //    Thread ImageThread = new Thread(new ThreadStart(ImageSequence));
        //    ImageThread.Start();
        //    ImageThread.Priority = ThreadPriority.Highest;
        //}
        //private void TimerTick(object sender, EventArgs e)
        //{
        //    ActiveFrame = @"c:\users\viva_\source\repos\videoprocessingxray\imagedb\" + n + ".jpg";

        //    if (++imageIndex == 5)
        //    {
        //        imageIndex = 2;
        //    }
        //}

  
        public void SetFrameImage(object x)
        {           
            int n = (int)x;
            FrameNum = n;
            ActiveFrame = @"c:\users\viva_\source\repos\videoprocessingxray\imagedb\" + n + ".jpg";

        }

        ///// <summary>
        ///// Resize the image to the specified width and height.
        ///// </summary>
        ///// <param name="image">The image to resize.</param>
        ///// <param name="width">The width to resize to.</param>
        ///// <param name="height">The height to resize to.</param>
        ///// <returns>The resized image.</returns>
        //public static Image ResizeImage(Image image, int width, int height)
        //{
        //    var destRect = new Rectangle(0, 0, width, height);
        //    var destImage = new Bitmap(width, height);

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        graphics.CompositingMode = CompositingMode.SourceCopy;
        //        graphics.CompositingQuality = CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }

        //    return destImage;
        //}
    }
}
