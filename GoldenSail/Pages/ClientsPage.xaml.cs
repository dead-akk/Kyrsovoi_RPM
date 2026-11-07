using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GoldenSail.Entities;

namespace GoldenSail.Pages
{
    /// <summary>
    /// Показывает клиентов из базы и выполняет простой поиск по контактам.
    /// </summary>
    public partial class ClientsPage : Page
    {
        private List<ClientRow> _clients;

        public ClientsPage()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                _clients = new List<ClientRow>();
                List<Klienti> clients = App.Context.Klienti.ToList();
                int i;
                for (i = 0; i < clients.Count; i++)
                {
                    ClientRow row = new ClientRow();
                    row.Name = (clients[i].Familiya + " " + clients[i].Imya + " " + clients[i].Otchestvo).Trim();
                    row.Phone = clients[i].Telefon ?? "";
                    row.Email = clients[i].Email ?? "";
                    row.Passport = (clients[i].SeriyaPasporta + " " + clients[i].NomerPasporta).Trim();
                    _clients.Add(row);
                }
                _clients.Sort(CompareClients);
                ApplySearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить клиентов.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int CompareClients(ClientRow first, ClientRow second)
        {
            return string.Compare(first.Name, second.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        private void ApplySearch()
        {
            if (_clients == null) return;
            List<ClientRow> rows = new List<ClientRow>();
            string search = TbSearch.Text == null ? "" : TbSearch.Text.Trim().ToLower();
            int i;
            for (i = 0; i < _clients.Count; i++)
            {
                ClientRow client = _clients[i];
                if (search.Length == 0 || client.Name.ToLower().Contains(search)
                    || client.Phone.ToLower().Contains(search) || client.Email.ToLower().Contains(search))
                    rows.Add(client);
            }
            DgClients.ItemsSource = rows;
            TbCount.Text = "Клиентов: " + rows.Count;
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) { ApplySearch(); }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e) { LoadClients(); }

        private class ClientRow
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Passport { get; set; }
        }
    }
}
