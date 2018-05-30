using BUD_Storage.Windows;
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

namespace BUD_Storage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WorkArea.Children.Add(new WelcomeWindow());
        }

        private void BtnComeOn_Click(object sender, RoutedEventArgs e)
        {
            WorkArea.Children.Clear();
            WorkArea.Children.Add(new ComeOn(this));
        }

        private void BtnRemnants_Click(object sender, RoutedEventArgs e)
        {
            WorkArea.Children.Clear();
            WorkArea.Children.Add(new Remnants(this));
        }

        private void BtnNewMoving_Click(object sender, RoutedEventArgs e)
        {
            WorkArea.Children.Clear();
            WorkArea.Children.Add(new NewMoving(this));
        }

        private void BtnListOfMoving_Click(object sender, RoutedEventArgs e)
        {
            WorkArea.Children.Clear();
            WorkArea.Children.Add(new ListOfMoving(this));
        }
    }
}
