using BUD_Storage.App_Data;
using BUD_Storage.Auxiliary_windows;
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

namespace BUD_Storage.Windows
{
    /// <summary>
    /// Interaction logic for ComeOn.xaml
    /// </summary>
    public partial class ComeOn : UserControl
    {
        private MainWindow mainWin = null;  

        public ComeOn(MainWindow mw)
        {
            InitializeComponent();
            mainWin = mw;
        }

        private void BtnReturnToMailPage_Click(object sender, RoutedEventArgs e)
        {
            mainWin.WorkArea.Children.Clear();
            mainWin.WorkArea.Children.Add(new WelcomeWindow());
        }

        private void NumberInvoice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void EDRPOU_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void NumberWarehouse_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void NumberProduct_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
               && (!Price.Text.Contains(".")
               && Price.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }

        private void CleanFields()
        {
            NumberInvoice.Text = String.Empty;
            DateInvoice.Text = String.Empty;
            EDRPOU.Text = String.Empty;
            NumberWarehouse.Text = String.Empty;
            NumberProduct.Text = String.Empty;
            Quantity.Text = String.Empty;
            Price.Text = String.Empty;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CleanFields();
        }

        private void CreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (NumberInvoice.Text != String.Empty && DateInvoice.Text != String.Empty
                && EDRPOU.Text != String.Empty && NumberWarehouse.Text != String.Empty
                && NumberProduct.Text != String.Empty && Quantity.Text != String.Empty && Price.Text != String.Empty)
            {


                MessageForForms msg = new MessageForForms("Накладна!", "Накладну оформлено!");
                msg.ShowDialog();

                CleanFields();
            }
            else
            {
                MessageForForms msg = new MessageForForms("Помилка!", "Не всі поля заповнені!");

                msg.ShowDialog();
            }
        }

        private void InfoAboutEDRPOU_Click(object sender, RoutedEventArgs e)
        {
            WindInfoAboutEDRPOU windInfoEDRPOU = new WindInfoAboutEDRPOU();

            if (windInfoEDRPOU.ShowDialog() == true)
            {

            }
        }

        private void InfoAboutNumberWarehouse_Click(object sender, RoutedEventArgs e)
        {
            WindInfoAboutNumberWarehouse windInfoNumberWarehouse = new WindInfoAboutNumberWarehouse();

            if (windInfoNumberWarehouse.ShowDialog() == true)
            {

            }
        }

        private void InfoAboutNumberProduct_Click(object sender, RoutedEventArgs e)
        {
            WindInfoAboutNumberProduct windInfoNumberProduct = new WindInfoAboutNumberProduct();

            if (windInfoNumberProduct.ShowDialog() == true)
            {

            }
        }
    }
}
