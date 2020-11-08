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
using LiveCharts;
using LiveCharts.Wpf;

namespace PoissonDistribution
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Poisson poissonDistribution;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void WindowPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                poissonDistribution = new Poisson(int.Parse(txtCountTests.Text),
                    double.Parse(txtProbability.Text), int.Parse(txtM.Text));
                printChart(poissonDistribution);
                DataContext = this;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void printChart(Poisson pd)
        {
            double[] propabilities = pd.getProbabilities();
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Распределение Пуассона",
                    Values = new ChartValues<double> (propabilities)                    
                },             
            };
            XFormatter = pd.getMCollection();
            YFormatter = value => value.ToString("F3");
        }
    }
}
