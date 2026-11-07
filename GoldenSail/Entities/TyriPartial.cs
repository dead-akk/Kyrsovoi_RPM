using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GoldenSail.Entities
{
    /// <summary>
    /// Дополнительные свойства тура для отображения изображения и отметки.
    /// </summary>
    public partial class Tyri
    {
        /// <summary>
        /// Преобразует фотографию из базы данных в изображение WPF.
        /// </summary>
        public ImageSource FotoImage
        {
            get
            {
                if (Foto == null || Foto.Length == 0) return null;
                try
                {
                    BitmapImage image = new BitmapImage();
                    using (MemoryStream stream = new MemoryStream(Foto))
                    {
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                        image.Freeze();
                    }
                    return image;
                }
                catch
                {
                    return null;
                }
            }
        }

        public Visibility BadgeVisibility
        {
            get { return IsGoryashiy ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}
