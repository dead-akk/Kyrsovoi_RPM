using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using GoldenSail.Entities;
using QRCoder;

namespace GoldenSail.Pages
{
    /// <summary>
    /// Показывает подробную информацию о выбранном туре и QR-код видео.
    /// </summary>
    public partial class TourDetailPage : Page
    {
        public TourDetailPage(Tyri tour)
        {
            InitializeComponent();
            if (tour == null)
            {
                MessageBox.Show("Информация о туре не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TbNazvanie.Text = tour.Nazvanie ?? "Без названия";
            TbStrana.Text = tour.Strana ?? "Страна не указана";
            TbOtel.Text = tour.Otel ?? "Отель не указан";
            TbTsena.Text = tour.Tsena.ToString("N0") + " ₽";
            TbOpisanie.Text = tour.Opisanie ?? "Описание пока не добавлено.";
            ImgTour.Source = tour.FotoImage;
            BadgeBorder.Visibility = tour.IsGoryashiy ? Visibility.Visible : Visibility.Collapsed;

            if (!string.IsNullOrWhiteSpace(tour.Video))
            {
                try
                {
                    ImgQrCode.Source = GenerateQrCode(tour.Video);
                }
                catch (Exception ex)
                {
                    QrBorder.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Не удалось создать QR-код.\n" + ex.Message,
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                QrBorder.Visibility = Visibility.Collapsed;
            }
        }

        private BitmapImage GenerateQrCode(string url)
        {
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData data = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);
            Bitmap bitmap = code.GetGraphic(10);

            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                bitmap.Dispose();
                stream.Position = 0;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
