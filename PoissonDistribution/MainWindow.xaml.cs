using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Poisson poissonDistribution;
        private double[] probabilities;
        private string[] mCollection;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                poissonDistribution = new Poisson(int.Parse(txtCountTests.Text),
                    double.Parse(txtProbability.Text));
                probabilities = poissonDistribution.getProbabilities();
                mCollection = poissonDistribution.getMCollection();
                printChart();
                printText();
                DataContext = this;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void printChart()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = string.Empty,
                    Values = new ChartValues<double> (probabilities)                    
                },             
            };
            XFormatter = mCollection;
            YFormatter = value => value.ToString("F6");

            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(XFormatter));
            OnPropertyChanged(nameof(YFormatter));
        }


        private void printText()
        {
            spTextcontent.Children.Clear();
            spTextcontent.Children.Add(new Label
            {
                Content = $"Математическое ожидание M[X] = λ = n * p = {poissonDistribution.Disperion}"
            });
            spTextcontent.Children.Add(new Label
            {
                Content = $"Дисперсия D[X] = λ = n * p = {poissonDistribution.Disperion}"
            });
            spTextcontent.Children.Add(new Label
            {
                Content = $"Найдём ряд распределения :"
            });
            for(int i = 0; i < probabilities.Length; i++)
            {
                spTextcontent.Children.Add(new Label
                {
                    Content = $"P({i}) = {poissonDistribution.Disperion}^{i} * e^{poissonDistribution.Disperion} / {i}! = {probabilities[i]}"
                }) ;
            }
            
            if (cbWillCome.IsChecked == true 
                && int.TryParse(txtWillCome.Text, out int resultWillCome))
            {
                spTextcontent.Children.Add(new Label
                {
                    Content = $"Вероятность того, что событие наступит ровно {resultWillCome} раза"
                });
                string strWillCome = $"P({resultWillCome}) = {poissonDistribution.Disperion}^{resultWillCome} " +
                    $"* e^{poissonDistribution.Disperion} / {resultWillCome}!";
                spTextcontent.Children.Add(new Label
                {
                    Content = resultWillCome < probabilities.Count() ? strWillCome + $" = {probabilities[resultWillCome]}" 
                        : strWillCome + " = 0",
                    FontWeight = FontWeights.Bold
                });
            }

            if(cbLess.IsChecked == true
                && int.TryParse(txtLess.Text, out int resultLess))
            {
                spTextcontent.Children.Add(new Label
                {
                    Content = $"Вероятность того, что событие наступит менее {resultLess} раз."
                });
                int index = 0;
                double sumLess = 0.0;
                string strLess = $"P(X < {resultLess}) = " ;
                while(index < resultLess)
                {
                    if (probabilities.Length >= index + 1)
                    {
                        sumLess += probabilities[index];
                        if(index != 0)
                            strLess += $" + {probabilities[index].ToString("F5")}";
                        else
                            strLess += $"{probabilities[index].ToString("F5")}";
                    }
                    index++; 
                }
                spTextcontent.Children.Add(new Label
                {
                    Content = strLess + $" = {Math.Round(sumLess,5)}",
                    FontWeight = FontWeights.Bold
                });
            }

            if(cbNotLess.IsChecked == true
                && int.TryParse(txtNotLess.Text, out int resultNotLess))
            {
                spTextcontent.Children.Add(new Label
                {
                    Content = $"Вероятность того, что событие наступит не менее {resultNotLess} раз."
                });
                int index = 0;
                double sumNotLess = 0.0;
                string strNotLess = $"P(X >= {resultNotLess}) = 1 - (";
                while (index < resultNotLess)
                {
                    if (probabilities.Length >= index + 1)
                    {
                        sumNotLess += probabilities[index];
                        strNotLess += $" + {probabilities[index].ToString("F5")}";
                    }
                    index++;
                }
                spTextcontent.Children.Add(new Label
                {
                    Content = strNotLess + $") = {Math.Round(1 - sumNotLess, 5)}",
                    FontWeight = FontWeights.Bold
                });
            }

            if(cbMore.IsChecked == true
                && int.TryParse(txtMore.Text, out int resultMore))
            {
                spTextcontent.Children.Add(new Label
                {
                    Content = $"Вероятность того, что событие наступит более {resultMore} раз."
                });
                double sumMore = 0.0;
                string strMore = $"P(x > {resultMore}) = ";
                for(int i = resultMore + 1; i < probabilities.Length; i++)
                {
                    sumMore += probabilities[i];
                    if (i == resultMore + 1)
                        strMore += $"{probabilities[i]}";
                    else
                        strMore += $" + {probabilities[i]}";
                }
                spTextcontent.Children.Add(new TextBlock
                {
                    Text = strMore + $" = {Math.Round(sumMore, 5)}",
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.WrapWithOverflow
                });
            }

            if(cbNoMore.IsChecked == true
                && int.TryParse(txtNoMore.Text, out int resultNoMore))
            {
                spTextcontent.Children.Add(new Label
                {
                    Content = $"Вероятность того, что событие наступит не более {resultNoMore} раз."
                });
                double sumNoMore = 0.0;
                string strNoMore = $"P(X <= {resultNoMore}) = ";
                for (int i = resultNoMore + 1; i < probabilities.Length; i++)
                {
                    sumNoMore += probabilities[i];
                    if (i == resultNoMore + 1)
                        strNoMore += $"1 - ({probabilities[i]}";
                    else
                        strNoMore += $" + {probabilities[i]}";
                }
                spTextcontent.Children.Add(new TextBlock
                {
                    Text = strNoMore + $") = {1.0 - Math.Round(sumNoMore, 5)}",
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.WrapWithOverflow
                });
            }

            if(cbNotLessNoMore.IsChecked == true
                && int.TryParse(txtNotLessNotMore1.Text, out int resultNotLessNoMore1)
                && int.TryParse(txtNotLessNotMore2.Text, out int resultNotLessNoMore2))
            {
                spTextcontent.Children.Add(new Label
                {
                    Content = $"Вероятность того, что событие наступит не менее {resultNotLessNoMore1} " +
                    $"и не более {resultNotLessNoMore2} раз."
                });
                double sumNotLessNoMore = 0.0;
                string strNotLessNoMore = $"P({resultNotLessNoMore1} <= X <= {resultNotLessNoMore2}) = ";
                for(int i = resultNotLessNoMore1; i < resultNotLessNoMore2 + 1; i++)
                {
                    if (probabilities.Length == i)
                        break;
                    sumNotLessNoMore += probabilities[i];
                    if (i == resultNotLessNoMore1)
                        strNotLessNoMore += $"{probabilities[i]}";
                    else
                        strNotLessNoMore += $" + {probabilities[i]}";
                }
                spTextcontent.Children.Add(new TextBlock
                {
                    Text = strNotLessNoMore + $" = {Math.Round(sumNotLessNoMore, 5)}",
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.WrapWithOverflow
                });
            }

            if(cbStepAtLeastOnce.IsChecked == true)
            {
                spTextcontent.Children.Add(new Label
                {
                    Content = $"Вероятность того, что событие наступит хотя бы раз"
                });
                string strStepAtLeastOnce = $"P(1) = {poissonDistribution.Disperion}^1 " +
                    $"* e^{poissonDistribution.Disperion} / 1!";
                spTextcontent.Children.Add(new Label
                {
                    Content = 1 < probabilities.Count() ? strStepAtLeastOnce + $" = {probabilities[1]}"
                        : strStepAtLeastOnce + " = 0",
                    FontWeight = FontWeights.Bold
                });
            }
            
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e) 
            => Close();      
        private void WindowPanel_MouseDown(object sender, MouseButtonEventArgs e) 
            => DragMove();       

    }
}
