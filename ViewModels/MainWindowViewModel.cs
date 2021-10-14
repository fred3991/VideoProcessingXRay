using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Win32;
using VideoProcessingXRay.Commands;
using VideoProcessingXRay.Commands.Base;

namespace VideoProcessingXRay.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private double variable;
        public double Variable
        {
            get => variable;
            set => Set(ref variable, value);
        }

        private string _titile = "Video Processing";
        public string Title
        {
            get => _titile;
            set => Set(ref _titile, value);
        }


        public ICommand EmptyCommand { get; }
        private void OnEmptyCommandExecuted(object p)
        {
           

        }
        private bool CanEmptyCommandExecute(object p) => true;

        public MainWindowViewModel()
        {

            EmptyCommand = new LambdaCommand(OnEmptyCommandExecuted, CanEmptyCommandExecute);

            this.PropertyChanged += MainWindowViewModel_PropertyChanged;
        }

        private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

    }

}
