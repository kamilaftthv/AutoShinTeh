using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AutoShinTeh
{
    public partial class MainWindow : Window
    {
        private Commission _calculator = new Commission();
        private List<Seller> _sellers;

        public MainWindow()
        {
            InitializeComponent();

            _sellers = new List<Seller>
            {
                new Seller { StoreId = 10153, FullName = "Чупашева А.И.", HireDate = new DateTime(2015, 10, 1) },
                new Seller { StoreId = 20174, FullName = "Иванов А.В.", HireDate = new DateTime(2017, 1, 10) },
                new Seller { StoreId = 30191, FullName = "Кривцов О.П.", HireDate = new DateTime(2019, 2, 5) },
                new Seller { StoreId = 40140, FullName = "Янаева Э.С.", HireDate = new DateTime(2014, 12, 15) }
            };

        }

        private int CalculateYearsOfWork(DateTime hireDate)
        {
            int years = 0;
            if (HireDateDP.SelectedDate != null)
                years = DateTime.Now.Year - HireDateDP.SelectedDate.Value.Year;

            return years;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = SellerNameTextBox.Text.Trim();
            if (!DateTime.TryParse(HireDateDP.SelectedDate?.ToString(), out DateTime hireDate))
            {
                MessageBox.Show("Введите корректную дату приема на работу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(DailyRevenueTextBox.Text, out decimal dailyRevenue))
            {
                MessageBox.Show("Введите корректную среднюю дневную выручку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var seller = _sellers.FirstOrDefault(s => s.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase) && s.HireDate == hireDate);

            if (seller == null)
            {
                MessageBox.Show("Продавец с таким именем и датой приема на работу не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal commission = _calculator.CalculateCommission(dailyRevenue, seller.HireDate);
            int yearsOfWork = CalculateYearsOfWork(seller.HireDate);

            var newSellerData = new
            {
                FullName = fullName,
                Commission = commission.ToString("F2"),
                DailyRevenue = dailyRevenue.ToString("F2"),
                YearsOfWork = yearsOfWork
            };

            var currentData = (dataGrid.ItemsSource as List<dynamic>) ?? new List<dynamic>();
            currentData.Add(newSellerData);

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = currentData;

            CommissionResultTextBlock.Text = $"Комиссионные продавца {fullName}: {commission:F2} руб.";
        }

        private void UpdateDataGrid()
        {
            var initialData = _sellers.Select(seller =>
            {
                decimal dailyRevenue = 0;
                return new
                {
                    seller.FullName,
                    Commission = "",
                    DailyRevenue = dailyRevenue.ToString("F2"),
                    YearsOfWork = CalculateYearsOfWork(seller.HireDate)
                };
            }).ToList();

            dataGrid.ItemsSource = initialData;
        }
    }
}
