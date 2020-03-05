/*
 * Copyright 2020 FUJITSU SOCIAL SCIENCE LABORATORY LIMITED
 * クラス名　：MicrophoneSwitchModel
 * 概要      ：Friendlyを使ってLiveTalk.exeのマイク操作と連携
*/
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LiveTalkMicrophoneControllerSample.Models
{
    public class MicrophoneSwitchModel : INotifyPropertyChanged
    {
        private WindowsAppFriend LiveTalkApp = null;
        private string ExecName = @"C:\Program Files (x86)\FUJITSU Software LiveTalk\LiveTalk.exe";

        private bool _IsSwitchOn = false;
        public bool IsSwitchOn
        {
            get { return this._IsSwitchOn; }
            set
            {
                if (this._IsSwitchOn != value)
                {
                    this._IsSwitchOn = value;
                    OnPropertyChanged();
                    if (value)
                    {
                        this.SpeechOn();
                    }
                    else
                    {
                        this.SpeechOff();
                    }
                }
            }
        }

        public MicrophoneSwitchModel()
        {
            this.GetSpeechStatus();
        }

        /// <summary>
        /// 状態取得
        /// </summary>
        private void GetSpeechStatus()
        {
            try
            {
                this.StartLiveTalk();
                if (this.LiveTalkApp != null)
                {
                    var window = new WindowControl(this.LiveTalkApp.Type<System.Windows.Application>().Current.MainWindow);
                    var page = window.VisualTree().ByType("LiveTalk.Views.MainPage");
                    while (page.Count == 0 && Process.GetProcessesByName("LiveTalk").Count() > 0)
                    {
                        // Page表示待ち
                        System.Threading.Thread.Sleep(100);
                        page = window.VisualTree().ByType("LiveTalk.Views.MainPage");
                    }
                    var speechButton = new WPFToggleButton(window.VisualTree().ByBinding("IsSpeech")[0]);
                    if (speechButton.IsEnabled)
                    {
                        this._IsSwitchOn = (bool)speechButton.IsChecked;
                        OnPropertyChanged("IsSwitchOn");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex; // エラー通知（通知方法はMainViewModelで規定）
            }
        }

        /// <summary>
        /// 音声認識開始
        /// </summary>
        private void SpeechOn()
        {
            try
            {
                this.StartLiveTalk();
                if (this.LiveTalkApp != null)
                {
                    var window = new WindowControl(this.LiveTalkApp.Type<System.Windows.Application>().Current.MainWindow);
                    var page = window.VisualTree().ByType("LiveTalk.Views.MainPage");
                    while (page.Count == 0 && Process.GetProcessesByName("LiveTalk").Count() > 0)
                    {
                        // Page表示待ち
                        System.Threading.Thread.Sleep(100);
                        page = window.VisualTree().ByType("LiveTalk.Views.MainPage");
                    }
                    var speechButton = new WPFToggleButton(window.VisualTree().ByBinding("IsSpeech")[0]);
                    if (speechButton.IsEnabled && speechButton.IsChecked == false)
                    {
                        speechButton.EmulateCheck(true);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex; // エラー通知（通知方法はMainViewModelで規定）
            }
        }

        /// <summary>
        /// 音声認識終了
        /// </summary>
        private void SpeechOff()
        {
            try
            {
                this.StartLiveTalk();
                if (this.LiveTalkApp != null)
                {
                    var window = new WindowControl(this.LiveTalkApp.Type<System.Windows.Application>().Current.MainWindow);
                    var page = window.VisualTree().ByType("LiveTalk.Views.MainPage");
                    while (page.Count == 0 && Process.GetProcessesByName("LiveTalk").Count() > 0)
                    {
                        // Page表示待ち
                        System.Threading.Thread.Sleep(100);
                        page = window.VisualTree().ByType("LiveTalk.Views.MainPage");
                    }
                    var speechButton = new WPFToggleButton(window.VisualTree().ByBinding("IsSpeech")[0]);
                    if (speechButton.IsEnabled && speechButton.IsChecked == true)
                    {
                        speechButton.EmulateCheck(false);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex; // エラー通知（通知方法はMainViewModelで規定）
            }
        }

        /// <summary>
        /// LiveTalkが起動していなかったら起動
        /// </summary>
        private void StartLiveTalk()
        {
            System.Diagnostics.Process proc = null;
            var currentProcess = System.Diagnostics.Process.GetProcessesByName("LiveTalk");
            var path = this.ExecName;
            var isStarted = false;

            // 画面が閉じている状態ならばプロセスをKillしてあらためてプロセスを取得する
            if (currentProcess.Length == 1 && currentProcess[0].MainWindowHandle == (IntPtr)0)
            {
                currentProcess[0].Kill();
                currentProcess = System.Diagnostics.Process.GetProcessesByName("LiveTalk");
            }

            // LiveTalkのメイン画面が表示されるまで待つ
            while (!isStarted && !string.IsNullOrEmpty(path))
            {
                try
                {
                    if (currentProcess.Length == 1)
                    {
                        proc = currentProcess[0];
                    }
                    else
                    {
                        proc = System.Diagnostics.Process.Start(path); //インストール先指定
                    }
                    if (!Codeer.Friendly.Windows.Inside.CpuTargetCheckUtility.IsSameCpu(proc))
                    {
                        if (System.Environment.Is64BitProcess)
                        {
                            // x64からx86を起動
                            this.LiveTalkApp = null;
                        }
                        else
                        {
                            // x86からx64を起動
                            throw new PlatformNotSupportedException(
                                String.Format("プラットフォームが適合しません。({0})",
                                System.Environment.Is64BitProcess ? "x64" : "x86"));
                        }
                    }
                    else
                    {
                        // 同じ動作モードで実行
                        this.LiveTalkApp = new WindowsAppFriend(proc);
                    }
                    isStarted = true;
                }
                catch (FriendlyOperationException ex)
                {
                    try
                    {
                        proc?.Kill();
                    }
                    catch { }
                    this.LiveTalkApp = null;
                    isStarted = false;
                    throw ex;
                }
            }
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