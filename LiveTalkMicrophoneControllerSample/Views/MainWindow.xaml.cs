/*
 * Copyright 2020 FUJITSU SOCIAL SCIENCE LABORATORY LIMITED
 * クラス名　：MainWindow
 * 概要      ：MainWindow.xaml の相互作用ロジック
*/
using System.Windows;

namespace LiveTalkMicrophoneControllerSample.Views
{
    public partial class MainWindow : Window
    {
        public ViewModels.MainViewModel ViewModel { get; } = App.MainVM;

        public MainWindow()
        {
            InitializeComponent();
        }


    }
}
