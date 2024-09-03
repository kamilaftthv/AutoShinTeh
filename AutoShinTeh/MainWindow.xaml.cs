using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AutoShinTeh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

            UpdateDataGrid();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = SellerNameTextBox.Text;
            string hireDateText = HireDateTextBox.Text;
            string dailyRevenueText = DailyRevenueTextBox.Text;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Введите фамилию продавца.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DateTime.TryParse(hireDateText, out DateTime hireDate))
            {
                MessageBox.Show("Введите корректную дату приема на работу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(dailyRevenueText, out decimal dailyRevenue))
            {
                MessageBox.Show("Введите корректную дневную выручку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var seller = _sellers.FirstOrDefault(s => s.FullName == fullName && s.HireDate == hireDate);

            if (seller == null)
            {
                MessageBox.Show("Продавец с таким именем и датой приема на работу не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal commission = _calculator.CalculateCommission(dailyRevenue, seller.HireDate);
            int yearsOfWork = _calculator.CalculateYearsOfWork(seller.HireDate);

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

            CommissionResultTextBlock.Text = $"Комиссионные продавца {fullName}: {commission.ToString("F2")} руб.";
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
                    YearsOfWork = _calculator.CalculateYearsOfWork(seller.HireDate)
                };
            }).ToList();

            dataGrid.ItemsSource = initialData;
        }
    }
}
