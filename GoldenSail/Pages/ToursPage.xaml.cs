using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GoldenSail.Entities;

namespace GoldenSail.Pages
{
    /// <summary>
    /// Показывает туры и позволяет искать, фильтровать и сортировать их.
    /// </summary>
    public partial class ToursPage : Page
    {
        private List<Tyri> _allTours;

        public ToursPage()
        {
            InitializeComponent();
            LoadCountries();
            LoadTours();
        }

        private void LoadCountries()
        {
            try
            {
                List<string> countries = new List<string>();
                List<Tyri> tours = App.Context.Tyri.ToList();
                int i;
                for (i = 0; i < tours.Count; i++)
                {
                    if (!tours[i].IsUdalen && !string.IsNullOrWhiteSpace(tours[i].Strana)
                        && !countries.Contains(tours[i].Strana))
                    {
                        countries.Add(tours[i].Strana);
                    }
                }
                countries.Sort();
                countries.Insert(0, "Все страны");
                ComboCountry.ItemsSource = countries;
                ComboCountry.SelectedIndex = 0;
                ComboSort.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить список стран.\n" + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTours()
        {
            try
            {
                _allTours = new List<Tyri>();
                List<Tyri> tours = App.Context.Tyri.ToList();
                int i;
                for (i = 0; i < tours.Count; i++)
                {
                    if (!tours[i].IsUdalen)
                    {
                        _allTours.Add(tours[i]);
                    }
                }
                UpdateTours();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить каталог туров.\n" + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTours()
        {
            if (_allTours == null) return;

            List<Tyri> result = new List<Tyri>();
            string search = TBoxSearch.Text == null ? "" : TBoxSearch.Text.Trim().ToLower();
            string country = ComboCountry.SelectedItem == null ? "Все страны" : ComboCountry.SelectedItem.ToString();
            int i;

            for (i = 0; i < _allTours.Count; i++)
            {
                Tyri tour = _allTours[i];
                bool matchesSearch = search.Length == 0
                    || (tour.Nazvanie ?? "").ToLower().Contains(search)
                    || (tour.Strana ?? "").ToLower().Contains(search)
                    || (tour.Otel ?? "").ToLower().Contains(search);
                bool matchesCountry = country == "Все страны" || tour.Strana == country;

                if (matchesSearch && matchesCountry)
                {
                    result.Add(tour);
                }
            }

            if (ComboSort.SelectedIndex == 1)
            {
                result.Sort(ComparePriceAscending);
            }
            else if (ComboSort.SelectedIndex == 2)
            {
                result.Sort(ComparePriceDescending);
            }
            LViewTours.ItemsSource = result;
        }

        private int ComparePriceAscending(Tyri first, Tyri second)
        {
            return first.Tsena.CompareTo(second.Tsena);
        }

        private int ComparePriceDescending(Tyri first, Tyri second)
        {
            return second.Tsena.CompareTo(first.Tsena);
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e) { UpdateTours(); }
        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e) { UpdateTours(); }
        private void ComboCountry_SelectionChanged(object sender, SelectionChangedEventArgs e) { UpdateTours(); }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCountries();
            LoadTours();
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;
            Tyri tour = button.Tag as Tyri;
            if (tour == null) return;
            NavigationService.Navigate(new TourDetailPage(tour));
        }
    }
}
