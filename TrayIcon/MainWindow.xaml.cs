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
        public bool isDark = false;

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
        
        private void Profiler(string name) {
            CreateNewProfile(name);
        }
        private void CreateNewProfile(string ProfileName) {
            profiles.Add(new MouseProfile(ProfileName, 10, 3, 500));
            manager.SaveProfiles(profiles);
            CreateNewItem(ProfileName);
            ProfileBox.SelectedIndex = ProfileBox.Items.Count -1;
            LoadMouseProfile(ProfileName);



        }
        private void CreateNewItem(string name) {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = name;
            item.Selected += new RoutedEventHandler(SelectProfile);
            ProfileBox.Items.Add(item);
            MessageBox.Show("Profile Created!");
            NewProfileMenuVisibility(Visibility.Hidden);
        }
        private void NewProfileMenuVisibility(Visibility visibility) {
            HideBackground.Visibility = visibility;
            NewProfileCard.Visibility = visibility;
        }
        private void DeleteProfileMenuVisibility(Visibility visibility) {
            HideBackground.Visibility = visibility;
            DeleteProfileCard.Visibility = visibility;
        }
        private void CreateProfiles() {
            profiles.Add(new MouseProfile("Ultimate Profile", 20, 15, 2000));
            profiles.Add(new MouseProfile("Pro Profile", 15, 6, 1000));
            profiles.Add(new MouseProfile("Classic Profile", 10, 3, 500));
            manager.SaveProfiles(profiles);
        }
        private void SelectProfile(object sender, RoutedEventArgs e) {
            ComboBoxItem button = (sender as ComboBoxItem);
            ChangeProfile(button.Content.ToString());
            
        }
        private void ChangeProfile(string name) {
            
            ProfilesPanel.Visibility = Visibility.Visible;
            DefaultItem.IsEnabled = false;
            if (name == "Choose Profile") {
                ProfilesPanel.Visibility = Visibility.Hidden;
            } else {
                LoadMouseProfile(name);
            }
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
            GetProfile();
            manager.SaveProfiles(profiles);
            MessageBox.Show("Profile Saved!");
        }
        public void GetProfile() {
           foreach(MouseProfile profile in profiles) {
                if (profile.Name == ProfileBox.Text) {
                    
                    profile.MouseSpeed = Int32.Parse(MouseSpeedLabel.Content.ToString());
                    profile.ScrollSpeed = Int32.Parse(MouseScrollSpeedLabel.Content.ToString());
                    profile.DoubleClickSpeed = Int32.Parse(MouseDoubleClickSpeedLabel.Content.ToString());
                } 
            }
        }
        
        private void DarkMode() {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#FF191919");
            brush.Freeze();
            BrushConverter bc2 = new BrushConverter();
            Brush brush2 = (Brush)bc.ConvertFrom("#FF1D1D1D");
            brush2.Freeze();

            Title.Foreground = Brushes.WhiteSmoke;
            Main.Background = brush;
            ProfileCard1.Background = brush2;
            ProfileCard2.Background = brush2;
            ProfileCard3.Background = brush2;
            MSpeed.Foreground = Brushes.WhiteSmoke;
            SSpeed.Foreground = Brushes.WhiteSmoke;
            DSpeed.Foreground = Brushes.WhiteSmoke;
            MouseSpeedLabel.Foreground = Brushes.WhiteSmoke;
            MouseScrollSpeedLabel.Foreground = Brushes.WhiteSmoke;
            MouseDoubleClickSpeedLabel.Foreground = Brushes.WhiteSmoke;
            ProfileBox.Foreground = Brushes.WhiteSmoke;
            NewProfileCard.Background = brush2;
            ProfileNameLabel.Foreground = Brushes.WhiteSmoke;
            NewProfileNameText.Foreground = Brushes.WhiteSmoke;
            DeleteProfileCard.Background = brush2;
            DeleteProfileNameLabel.Foreground = Brushes.WhiteSmoke;
            
        }
        private void DefaultMode() {
            Title.Foreground = Brushes.Black;
            Main.Background = Brushes.White;
            ProfileCard1.Background = Brushes.White;
            ProfileCard2.Background = Brushes.White;
            ProfileCard3.Background = Brushes.White;
            MSpeed.Foreground = Brushes.Black;
            SSpeed.Foreground = Brushes.Black;
            DSpeed.Foreground = Brushes.Black;
            MouseSpeedLabel.Foreground = Brushes.Black;
            MouseScrollSpeedLabel.Foreground = Brushes.Black;
            MouseDoubleClickSpeedLabel.Foreground = Brushes.Black;
            ProfileBox.Foreground = Brushes.Black;
            NewProfileCard.Background = Brushes.White;
            ProfileNameLabel.Foreground = Brushes.Black;
            NewProfileNameText.Foreground = Brushes.Black;
            DeleteProfileCard.Background = Brushes.White;
            DeleteProfileNameLabel.Foreground = Brushes.Black;
            
        }

        private void DarkModeButton_Click(object sender, RoutedEventArgs e) {
            if (!isDark) {
                DarkMode();
                isDark = true;
                ModeImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/WhiteMoon.ico", UriKind.Relative));
            } else {
                DefaultMode();
                isDark = false;
                ModeImage.ImageSource = new BitmapImage(new Uri(@"../../Resources/BlackMoon.ico", UriKind.Relative));
            }
        }

        private void HideBackground_Click(object sender, RoutedEventArgs e) {
            NewProfileMenuVisibility(Visibility.Hidden);
            DeleteProfileMenuVisibility(Visibility.Hidden);
        }

        private void NewProfile_Click(object sender, RoutedEventArgs e) {
            if (NewProfileNameText.Text != null) {
                Profiler(NewProfileNameText.Text.ToString());
            }
        }

        private void CreateNewProfileButton_Click(object sender, RoutedEventArgs e) {
            NewProfileMenuVisibility(Visibility.Visible);
        }

        private void DeleteProfile_Click(object sender, RoutedEventArgs e) {
            RemoveProfile();
        }
        private void RemoveProfile() {
            string profilename = ProfileBox.Text;
            foreach (MouseProfile profile in profiles) {
                if (profile.Name == profilename) {
                    profiles.Remove(profile);
                    MessageBox.Show("Profile Removed!");
                    DeleteProfileMenuVisibility(Visibility.Hidden);
                    break;
                }
            }
            RemoveItem(profilename);
            manager.SaveProfiles(profiles);
            
        }
        private void RemoveItem(string name) {
            foreach(ComboBoxItem item in ProfileBox.Items) {
                if (item.Content.ToString() == name) {
                    ProfileBox.Items.Remove(item);
                    break;
                }
            }
            ChangeProfile("Choose Profile");
        }
        private void CancelDeleteProfile_Click(object sender, RoutedEventArgs e) {
            DeleteProfileMenuVisibility(Visibility.Hidden);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            DeleteProfileMenuVisibility(Visibility.Visible);
        }
    }
}
