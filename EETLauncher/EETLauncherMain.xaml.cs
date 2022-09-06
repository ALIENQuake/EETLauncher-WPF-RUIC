//Copyright © alienquake@hotmail.com
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using static EETLauncherWPF.EETLauncherConfig;
using static EETLauncherWPF.EETLauncherGlobal;

namespace EETLauncherWPF {
    /// <summary>
    /// Interaction logic for <see cref="EETLauncher"/>.xaml
    /// </summary>
    public partial class EETLauncherMain {

        public static readonly RoutedUICommand PlayEET = new RoutedUICommand();
        public static readonly RoutedUICommand OpenSettingsWindow = new RoutedUICommand();
        public static readonly RoutedUICommand CheckForUpdates = new RoutedUICommand();
        public static readonly RoutedUICommand OpenEETReadMe = new RoutedUICommand();
        public static readonly RoutedUICommand OpenEETHomePage = new RoutedUICommand();
        public static readonly RoutedUICommand Exit = new RoutedUICommand();
        public static readonly RoutedUICommand WindowMouseDown = new RoutedUICommand();
        public static readonly RoutedUICommand OpenModManagerPage = new RoutedUICommand();

        public EETLauncherMain() {

            InitializeComponent();
            CommandBindings.Add(new CommandBinding(PlayEET, PlayEET_OnExecuted, PlayEET_CanExecute));
            CommandBindings.Add(new CommandBinding(OpenSettingsWindow, OpenSettingsWindow_OnExecuted, OpenSettingsWindow_CanExecute));
            CommandBindings.Add(new CommandBinding(CheckForUpdates, CheckForUpdates_OnExecuted, CheckForUpdates_CanExecute));
            CommandBindings.Add(new CommandBinding(OpenEETReadMe, OpenEETReadMe_OnExecuted, OpenEETReadMe_CanExecute));
            CommandBindings.Add(new CommandBinding(OpenEETHomePage, OpenEETHomePage_OnExecuted, OpenEETHomePage_CanExecute));
            CommandBindings.Add(new CommandBinding(OpenModManagerPage, OpenModManagerPage_OnExecuted, OpenModManagerPage_CanExecute));
            CommandBindings.Add(new CommandBinding(Exit, Exit_OnExecuted, Exit_CanExecute));
            CommandBindings.Add(new CommandBinding(WindowMouseDown, WindowMouseDown_OnExecuted, WindowMouseDown_CanExecute));

            AppRootPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + Path.DirectorySeparatorChar;
            AppLogFileName = (string)FindResource("AppLogFileName");
            GameCheckFilePath = (string)FindResource("GameCheckFilePath");
            GameCfgFileName = (string)FindResource("GameCfgFileName");
            GameExeFileName = (string)FindResource("GameExeFileName");
            GameEngineFileName = (string)FindResource("GameEngineFileName");
            EETGUIComponentNumber = (string)FindResource("EETGUIComponentNumber");
            EETGUIModFileName = (string)FindResource("EETGUIModFileName");
            EETGUIExeFileName = (string)FindResource("EETGUIExeFileName");
            EETGUIUnknown = (string)FindResource("EETGUIUnknown");
            EETHomePage = (string)FindResource("EETHomePage");
            EETReadMeFilePath = (string)FindResource("EETReadMeFilePath");
            EETFlagFilePath = (string)FindResource("EETFlagFilePath");
            BG2EENotDetected = (string)FindResource("BG2EENotDetected");
            EETNotDetected = (string)FindResource("EETNotDetected");
            EETRequireFirstRun = (string)FindResource("EETRequireFirstRun");
            ModManagerHomePage = (string)FindResource("ModManagerHomePage");
            WeiDULogFileName = (string)FindResource("WeiDULogFileName");

            if (TestBG2EEDirectory()) {
                if (TestEETInstalled()) {
                    GameCfgDirectory = GetGameCfgDirectory();
                    EETBaldurLua = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + GameCfgDirectory + Path.DirectorySeparatorChar + GameCfgFileName;
                    return;
                }
                DisableEETLauncherMainLB();
                EETLauncherMain_TB_LOG.Visibility = Visibility.Visible;
                EETLauncherMain_TB_LOG.Text = EETNotDetected;
            } else {
                DisableEETLauncherMainLB();
                EETLauncherMain_TB_LOG.Visibility = Visibility.Visible;
                EETLauncherMain_TB_LOG.Text = BG2EENotDetected;
            }
        }

        public void DisableEETLauncherMainLB() {
            EETLauncherMain_LB_PLAY.Visibility = Visibility.Hidden;
            EETLauncherMain_LB_SETTINGS.Visibility = Visibility.Hidden;
            EETLauncherMain_LB_UPDATES.Visibility = Visibility.Hidden;
            EETLauncherMain_LB_README.Visibility = Visibility.Hidden;
        }

        private void WindowMouseDown_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void WindowMouseDown_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            try { DragMove(); } catch { }
        }

        private void OpenSettingsWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void OpenSettingsWindow_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            var EETLauncherSettings = new EETLauncherSettings { Owner = this };
            EETLauncherSettings.Show();
            //Hide();
            Visibility = Visibility.Hidden;
        }

        private void CheckForUpdates_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void CheckForUpdates_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            // Check for updates
        }

        private void PlayEET_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void PlayEET_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            bool EEExInstalled = File.Exists(AppRootPath + "EEEx.exe") && File.Exists(AppRootPath + @"override\M__EEex.lu");
            if (EEExInstalled) {
                Process.Start(AppRootPath + "EEEx.exe");
            } else {
                Process.Start(AppRootPath + GameExeFileName);
            }
            Close();
            Application.Current.Shutdown();
        }

        private void OpenEETReadMe_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void OpenEETReadMe_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            Process.Start(AppRootPath + EETReadMeFilePath);
        }

        private void OpenModManagerPage_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void OpenModManagerPage_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            Process.Start(ModManagerHomePage);
        }

        private void OpenEETHomePage_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void OpenEETHomePage_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            Process.Start(EETHomePage);
        }

        private void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            Close();
            Application.Current.Shutdown();
        }
    }
}
