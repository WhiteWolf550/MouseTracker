using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TrayIcon {
    /// <summary>
    /// Interakční logika pro App.xaml
    /// </summary>
    public partial class App : Application {

        




        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
            

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(MouseClick);
            
            _notifyIcon.Icon = TrayIcon.Properties.Resources.Icon1;
            _notifyIcon.Visible = true;

            


            CreateContextMenu();
        }

        private void CreateContextMenu() {
            _notifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
            
        }

        private void ExitApplication() {
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
            
        }
        public void MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                
            }else {
                ShowMainWindow();
            }
        }
        public static Point GetMousePosition() {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new Point(point.X, point.Y);
        }
        private void ShowMainWindow() {
            
            MainWindow.Top = GetMousePosition().Y - 450;
            MainWindow.Left = GetMousePosition().X - 200;
            MainWindow.Show();
            
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e) {
            if (!_isExit) {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }
    }
}
