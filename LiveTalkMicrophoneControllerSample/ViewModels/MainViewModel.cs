/*
 * Copyright 2020 FUJITSU SOCIAL SCIENCE LABORATORY LIMITED
 * クラス名　：MainViewModel
 * 概要      ：MainViewModel
*/
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LiveTalkMicrophoneControllerSample.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Models.MicrophoneSwitchModel Model = new Models.MicrophoneSwitchModel();

        public bool IsSwitchOn
        {
            get { return this.Model.IsSwitchOn; }
            set { this.Model.IsSwitchOn = value; }
        }

        public MainViewModel()
        {
            this.Model.PropertyChanged += (s, e) =>
            {
                this.OnPropertyChanged(e.PropertyName);
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
