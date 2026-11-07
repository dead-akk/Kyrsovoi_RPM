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
    /// Показывает сотрудников, их роли и статус подтверждения аккаунта.
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private List<EmployeeRow> _employees;

        public EmployeesPage()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                _employees = new List<EmployeeRow>();
                List<Sotrudniki> employees = App.Context.Sotrudniki.Include("Roli").ToList();
                int i;
                for (i = 0; i < employees.Count; i++)
                {
                    Sotrudniki employee = employees[i];
                    EmployeeRow row = new EmployeeRow();
                    row.Name = (employee.Familiya + " " + employee.Imya + " " + employee.Otchestvo).Trim();
                    row.Role = employee.Roli == null ? "Не указана" : employee.Roli.Nazvanie;
                    row.Phone = employee.Telefon ?? "";
                    row.Email = employee.Email ?? "";
                    row.Confirmed = employee.IsPodtverzhden ? "Подтверждён" : "Ожидает подтверждения";
                    _employees.Add(row);
                }
                _employees.Sort(CompareEmployees);
                ApplySearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить сотрудников.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int CompareEmployees(EmployeeRow first, EmployeeRow second)
        {
            return string.Compare(first.Name, second.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        private void ApplySearch()
        {
            if (_employees == null) return;
            List<EmployeeRow> rows = new List<EmployeeRow>();
            string search = TbSearch.Text == null ? "" : TbSearch.Text.Trim().ToLower();
            int i;
            for (i = 0; i < _employees.Count; i++)
            {
                EmployeeRow employee = _employees[i];
                if (search.Length == 0 || employee.Name.ToLower().Contains(search)
                    || employee.Role.ToLower().Contains(search) || employee.Phone.ToLower().Contains(search)
                    || employee.Email.ToLower().Contains(search)) rows.Add(employee);
            }
            DgEmployees.ItemsSource = rows;
            TbCount.Text = "Сотрудников: " + rows.Count;
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e) { ApplySearch(); }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e) { LoadEmployees(); }

        private class EmployeeRow
        {
            public string Name { get; set; }
            public string Role { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Confirmed { get; set; }
        }
    }
}
