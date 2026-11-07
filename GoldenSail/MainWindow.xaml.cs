using System.Windows;
using System.Windows.Controls;

namespace GoldenSail
{
    /// <summary>
    /// Главное окно и переходы между основными страницами приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowTours();
        }

        private void ShowTours()
        {
            FrameMain.Navigate(new Pages.ToursPage());
            TbPageTitle.Text = "Каталог туров";
        }

        private void BtnTours_Click(object sender, RoutedEventArgs e) { ShowTours(); }

        private void BtnBookings_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.BookingsPage());
            TbPageTitle.Text = "Бронирования";
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ClientsPage());
            TbPageTitle.Text = "Клиенты";
        }

        private void BtnEmployees_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.EmployeesPage());
            TbPageTitle.Text = "Сотрудники";
        }

        private void BtnPayments_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Страница оплат пока не создана.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Страница отчётов пока не создана.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Журнал действий пока не создан.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выход пока не настроен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
