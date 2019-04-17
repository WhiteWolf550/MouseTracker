using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrayIcon {
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        FileManager manager = new FileManager();
        List<MouseProfile> profiles = new List<MouseProfile>();

        public const UInt32 SPI_SETMOUSESPEED = 0x0071;
        public const UInt32 SPI_SETWHEELSCROLLLINES = 0x0069;
        public const UInt32 SPI_SETDOUBLECLICKTIME = 0x0020;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern Boolean SystemParametersInfo(
            UInt32 uiAction,
            UInt32 uiParam,
            UInt32 pvParam,
            UInt32 fWinIni);
        public MainWindow() {
            InitializeComponent();
            ProfilesPanel.Visibility = Visibility.Hidden;
            //CreateProfiles();
            LoadProfiles();
            
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            MouseSpeedLabel.Content = Math.Round(MouseSlider.Value);
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, Convert.ToUInt32(Math.Round(MouseSlider.Value)), 0);

        }
        public void LoadProfiles() {
            profiles = manager.LoadProfiles();
            foreach(MouseProfile profile in profiles) {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = profile.Name;
                item.Selected += new RoutedEventHandler(SelectProfile);
                ProfileBox.Items.Add(item);
            }
        }
        public void LoadMouseProfile(string profilename) {
            profiles = manager.LoadProfiles();
            foreach (MouseProfile profile in profiles) {
                if (profile.Name == profilename) {
                    

                    MouseSlider.Value = profile.MouseSpeed;
                    MouseSpeedLabel.Content = profile.MouseSpeed;
                    ScrollSlider.Value = profile.ScrollSpeed;
                    MouseScrollSpeedLabel.Content = profile.ScrollSpeed;
                }
            }
        }
        private void CreateProfiles() {
            profiles.Add(new MouseProfile("Ultimate Profile", 20, 15, 2000));
            profiles.Add(new MouseProfile("Pro Profile", 15, 6, 1000));
            profiles.Add(new MouseProfile("Classic Profile", 10, 3, 500));
            manager.SaveProfiles(profiles);
        }
        private void SelectProfile(object sender, RoutedEventArgs e) {
            ComboBoxItem button = (sender as ComboBoxItem);
            ProfilesPanel.Visibility = Visibility.Visible;
            DefaultItem.IsEnabled = false;
            LoadMouseProfile(button.Content.ToString());
        }

        private void ScrollSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            MouseScrollSpeedLabel.Content = Math.Round(ScrollSlider.Value);
            SystemParametersInfo(SPI_SETWHEELSCROLLLINES, Convert.ToUInt32(Math.Round(ScrollSlider.Value)), 0, 0);
        }

        private void DoubleClickSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            MouseDoubleClickSpeedLabel.Content = Math.Round(DoubleClickSlider.Value);
            SystemParametersInfo(SPI_SETDOUBLECLICKTIME, Convert.ToUInt32(Math.Round(DoubleClickSlider.Value)), 0, 0);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            SaveProfile();
        }
        public void SaveProfile() {
            
            MessageBox.Show("Profile Saved!");
        }
    }
}
