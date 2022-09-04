#region License

/*
 * DjToKey
 *
 * Copyright (C) Marcin Badurowicz 2015
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files
 * (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion License

using Ktos.DjToKey.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Forms;

namespace Ktos.DjToKey.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private NotifyIcon trayIcon;
        private MainWindowViewModel vm;

        /// <summary>
        /// Creates a new Window and prepares the Tray Icon
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            trayIcon = new NotifyIcon();
            trayIcon.BalloonTipIcon = ToolTipIcon.Info;

            trayIcon.Icon = DjToKey.Resources.AppResources.icon;
            trayIcon.Text = DjToKey.Resources.AppResources.AppName;
            trayIcon.DoubleClick += TrayIcon_DoubleClick;

            vm = DataContext as MainWindowViewModel;

            if (vm.Devices.Count == 0)
            {
                System.Windows.MessageBox.Show(
                    DjToKey.Resources.AppResources.NoMidiMessage,
                    DjToKey.Resources.AppResources.AppName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                trayIcon.Visible = true;
                trayIcon.Text = string.Format(
                    "{0} - {1}",
                    DjToKey.Resources.AppResources.AppName,
                    vm.CurrentDevice.Name
                );
                trayIcon.ShowBalloonTip(
                    1000,
                    DjToKey.Resources.AppResources.AppName,
                    DjToKey.Resources.AppResources.TrayMessage,
                    ToolTipIcon.Info
                );
                this.ShowInTaskbar = false;
            }
            else
            {
                trayIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            vm.About();
        }

        private void Border_MouseLeftButtonUp(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e
        )
        {
            vm.SetCurrentScript(
                (sender as System.Windows.Controls.Border).DataContext as Models.ViewControl
            );
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            vm.UpdateCurrentScript(tbScript.Text);
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.OnClosing();
        }
    }
}
