/*
 * Copyright 2020 FUJITSU SOCIAL SCIENCE LABORATORY LIMITED
 * システム名：LiveTalkMicrophoneControllerSample.csproj
 * 概要      ：LiveTalkのマイク状態を制御するサンプルアプリ
*/
using System.Windows;

namespace LiveTalkMicrophoneControllerSample
{
    public partial class App : Application
    {
        private static ViewModels.MainViewModel _MainVM = null;
        public static ViewModels.MainViewModel MainVM
        {
            get
            {
                if (_MainVM == null)
                {
                    _MainVM = new ViewModels.MainViewModel();
                }
                return _MainVM;
            }
        }
    }
}
