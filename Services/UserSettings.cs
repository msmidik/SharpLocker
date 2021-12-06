using Microsoft.Win32;
using System;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Media.Imaging;

namespace SharpLocker.Services
{
    public static class UserSettings
    {
        /// <summary>
        /// Gets the display name of the current user.
        /// e.g John Doe
        /// </summary>
        /// <returns>The display name of the current user.</returns>
        public static string GetDisplayName()
        {
            try
            {
                return UserPrincipal.Current.DisplayName;
            }
            catch (Exception)
            {
                try
                {
                    RegistryKey officeUserInfo = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\office\Common\UserInfo");
                    string userName = officeUserInfo.GetValue("UserName").ToString();
                    officeUserInfo.Close();
                    return userName;
                }
                catch
                {
                    return GetUsername().Split('\\')[1];
                }
            }
        }

        /// <summary>
        /// Gets the domain name of the current user.
        /// e.g domain\joe
        /// </summary>
        /// <returns>The display username of the current user.</returns>
        public static string GetUsername()
        {
            return WindowsIdentity.GetCurrent().Name;
        }

        /// <summary>
        /// Gets the profile image for the current user.
        /// </summary>
        /// <returns></returns>
        public static BitmapImage GetProfileImage()
        {
            try
            {
                string profileImagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\AccountPictures\");
                string profileImage = Directory.GetFiles(profileImagePath).FirstOrDefault();
                return BitmapToBitmapImage(AccountPictureConverter.GetImage448(profileImage));
            }
            catch
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/Resources/usericon.png"));
            }

        }

        public static BitmapImage GetLockScreenImage()
        {
            //Get Windows Spotlight Images Location Path. (C:\Users\[Username]\AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets\)
            string spotlight_dir_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets\");

            // Normally the larger image present in this directory is the current lock screen image.
            string imagePath;
            try
            {
                DirectoryInfo folderInfo = new DirectoryInfo(spotlight_dir_path);
                var sortedFiles = from fi in folderInfo.GetFiles()
                                  orderby fi.Length descending
                                  select fi.Name;
                string imageName = sortedFiles.ElementAt(1);                     //second largest
                imagePath = Path.Combine(spotlight_dir_path, imageName);
            }
            catch
            {
                imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Web\Screen\img103.png");    // alternative   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Themes\TranscodedWallpaper");
            }
            return new BitmapImage(new Uri(Path.Combine(spotlight_dir_path, imagePath)));
        }

        public static bool validateCreds(string username, string password)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
                {
                    return context.ValidateCredentials(username, password);
                }
            }
            catch (Exception)
            {
                return true;      // on error assume that creds are valid
            }
        }

        private static BitmapImage BitmapToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

    }
}
