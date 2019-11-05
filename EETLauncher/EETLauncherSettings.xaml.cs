//Copyright © alienquake@hotmail.com
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static EETLauncherWPF.EETLauncherConfig;
using static EETLauncherWPF.EETLauncherGlobal;

namespace EETLauncherWPF {
    /// <summary>
    /// Interaction logic for EETLauncherSettings.xaml
    /// </summary>
    public partial class EETLauncherSettings {

        public EETLauncherEETGui EETGui { get; set; }

        public static readonly RoutedUICommand OpenEETLua = new RoutedUICommand();
        public static readonly RoutedUICommand ChangeEETGuiAsync = new RoutedUICommand();
        public static readonly RoutedUICommand SettingsPageBack = new RoutedUICommand();
        public static readonly RoutedUICommand WindowMouseDown = new RoutedUICommand();

        public EETLauncherSettings() {
            var eetgui = new EETLauncherEETGui();
            EETGui = eetgui;
            EETGui.Current = GetEETCurrentGUI();
            EETGui.ChangeTo = GetEETChangeToGUI(EETGui.Current);

            InitializeComponent();

            DataContext = EETGui;
            CommandBindings.Add(new CommandBinding(OpenEETLua, OpenEETLua_OnExecuted, OpenEETLua_CanExecute));
            CommandBindings.Add(new CommandBinding(ChangeEETGuiAsync, ChangeEETGuiAsync_OnExecuted, ChangeEETGuiAsync_CanExecute));
            CommandBindings.Add(new CommandBinding(SettingsPageBack, SettingsPageBack_OnExecuted, SettingsPageBack_CanExecute));
            CommandBindings.Add(new CommandBinding(WindowMouseDown, WindowMouseDown_OnExecuted, WindowMouseDown_CanExecute));
        }

        private void WindowMouseDown_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void WindowMouseDown_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            try { DragMove(); } catch { }
        }

        private void OpenEETLua_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void OpenEETLua_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (TestEETBaldurLua()) {
                Process.Start(EETBaldurLua);
            } else {
                EETLauncherSettings_TB_Log.Visibility = Visibility.Visible;
                EETLauncherSettings_TB_Log.Text = EETRequireFirstRun;
            }
        }

        private void ChangeEETGuiAsync_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        public async void ChangeEETGuiAsync_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            EETLauncherSettings_LB_Back.Visibility = Visibility.Hidden;
            EETLauncherSettings_LB_ChangeGui.Visibility = Visibility.Hidden;
            EETLauncherSettings_L_CurrentGui.Visibility = Visibility.Hidden;
            EETLauncherSettings_L_CurrentGui.Foreground = new SolidColorBrush(Colors.White);

            var EETGuiProcess = new Process { StartInfo = SetEETGUI(EETGui.ChangeTo) };

            try {
                // because we want to target .NET 4.0, we are using TaskEx.Run from Microsoft.Bcl.Async instead of default Task.Run from .NET Framework 4.5
                var result = await TaskEx.Run(() => {
                    EETGuiProcess.Start();
                    if (EETGuiProcess.Id >= 0) {
                        EETGuiProcess?.WaitForExit();
                    } else {
                        EETGuiProcess = null;
                    }
                    return EETGuiProcess;
                });
                if (result == null) return;
                EETGui.Current = GetEETCurrentGUI();
                EETGui.ChangeTo = GetEETChangeToGUI(EETGui.Current);
                EETLauncherSettings_L_CurrentGui.Foreground = new SolidColorBrush(Colors.Green);
                EETLauncherSettings_L_CurrentGui.Visibility = Visibility.Visible;
                EETLauncherSettings_LB_ChangeGui.Visibility = Visibility.Visible;
                EETLauncherSettings_LB_Back.Visibility = Visibility.Visible;
            } catch (Exception ex) {
                EETLauncherSettings_L_CurrentGui.Foreground = new SolidColorBrush(Colors.Red);
                EETLauncherSettings_LB_Back.Visibility = Visibility.Visible;
                EETLauncherSettings_TB_Log.Visibility = Visibility.Visible;
                EETLauncherSettings_TB_Log.Text = ex.Message;
                File.AppendAllText(Environment.SpecialFolder.ApplicationData + Path.DirectorySeparatorChar + AppLogFileName, ex.Message + Environment.NewLine);
            }
        }

        private void SettingsPageBack_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void SettingsPageBack_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (Application.Current.MainWindow == null) return;
            Application.Current.MainWindow.Left = Left;
            Application.Current.MainWindow.Top = Top;
            Application.Current.MainWindow.Visibility = Visibility.Visible;
            Close();
        }
    }
}
