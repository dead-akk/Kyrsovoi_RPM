using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GoldenSail.Entities;

namespace GoldenSail.Pages
{
    /// <summary>
    /// Показывает бронирования из базы и позволяет искать их по основным данным.
    /// </summary>
    public partial class BookingsPage : Page
    {
        private List<BookingRow> _bookings;

        public BookingsPage()
        {
            InitializeComponent();
            LoadBookings();
        }

        private void LoadBookings()
        {
            try
            {
                _bookings = new List<BookingRow>();
                List<Bronirovaniya> bookings = App.Context.Bronirovaniya.Include("Tyri").Include("Klienti").Include("Sotrudniki").ToList();
                int i;
                for (i = 0; i < bookings.Count; i++)
                {
                    Bronirovaniya booking = bookings[i];
                    BookingRow row = new BookingRow();
                    row.Number = booking.ID_Broni;
                    row.Tour = booking.Tyri == null ? "Тур не указан" : booking.Tyri.Nazvanie;
                    row.Client = booking.Klienti == null ? "Клиент не указан" : MakeName(booking.Klienti.Familiya, booking.Klienti.Imya, booking.Klienti.Otchestvo);
                    row.Employee = booking.Sotrudniki == null ? "—" : MakeName(booking.Sotrudniki.Familiya, booking.Sotrudniki.Imya, booking.Sotrudniki.Otchestvo);
                    row.Date = booking.DataBroni;
                    row.Amount = booking.ItogStoimost;
                    row.Status = booking.StatusBroni ?? "Не указан";
                    _bookings.Add(row);
                }
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить бронирования.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string MakeName(string lastName, string firstName, string middleName)
        {
            return (lastName + " " + firstName + " " + middleName).Trim();
        }

        private void ApplyFilters()
        {
            if (_bookings == null) return;
            List<BookingRow> rows = new List<BookingRow>();
            string search = TBoxSearch.Text == null ? "" : TBoxSearch.Text.Trim().ToLower();
            string status = "Все статусы";
            ComboBoxItem item = ComboStatus.SelectedItem as ComboBoxItem;
            if (item != null) status = item.Content.ToString();

            int i;
            for (i = 0; i < _bookings.Count; i++)
            {
                BookingRow row = _bookings[i];
                bool hasSearch = search.Length == 0 || row.Number.ToString().Contains(search)
                    || (row.Tour ?? "").ToLower().Contains(search)
                    || (row.Client ?? "").ToLower().Contains(search)
                    || (row.Employee ?? "").ToLower().Contains(search);
                bool hasStatus = status == "Все статусы" || row.Status == status;
                if (hasSearch && hasStatus) rows.Add(row);
            }
            DgBookings.ItemsSource = rows;
            TbCount.Text = "Найдено бронирований: " + rows.Count;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e) { ApplyFilters(); }
        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e) { ApplyFilters(); }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e) { LoadBookings(); }

        private class BookingRow
        {
            public int Number { get; set; }
            public string Tour { get; set; }
            public string Client { get; set; }
            public string Employee { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
        }
    }
}
