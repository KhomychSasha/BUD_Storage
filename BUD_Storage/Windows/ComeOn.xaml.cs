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
            NumberContractor.Text = String.Empty;
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
                && NumberContractor.Text != String.Empty && NumberWarehouse.Text != String.Empty
                && NumberProduct.Text != String.Empty && Quantity.Text != String.Empty && Price.Text != String.Empty)
            {
                Product_In_The_Warehouse newPrdInWarehouse = new Product_In_The_Warehouse()
                {
                    IdProduct = Int32.Parse(NumberProduct.Text),
                    IdWarehouse = Int32.Parse(NumberWarehouse.Text),
                    IdContractor = Int32.Parse(NumberContractor.Text),
                    Quantity = Int32.Parse(Quantity.Text),
                    Price = Decimal.Parse(Price.Text),
                    VAT = 0,
                    Price_VAT = Decimal.Parse(Price.Text)
                };

                if (CheckBoxVAT.IsChecked == true)
                {
                    newPrdInWarehouse.VAT = 20;

                    decimal vat = 0.2m;
                    newPrdInWarehouse.Price_VAT += Decimal.Parse(Price.Text) * vat;
                }

                using (DatabaseBudStorage db = new DatabaseBudStorage())
                {
                    db.Entities_Product_In_The_Warehouses.Add(newPrdInWarehouse);

                    if (NumberWarehouse.Text != "1")
                    {

                    }
                    db.SaveChanges();
                }

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

        private void InfoAboutContractor_Click(object sender, RoutedEventArgs e)
        {
            WindInfoAboutNumberContractor windInfoNumberContractor = new WindInfoAboutNumberContractor();

            if (windInfoNumberContractor.ShowDialog() == true)
            {
                NumberContractor.Text = windInfoNumberContractor.Contractor_Code.Text;
            }
        }

        private void InfoAboutNumberWarehouse_Click(object sender, RoutedEventArgs e)
        {
            WindInfoAboutNumberWarehouse windInfoNumberWarehouse = new WindInfoAboutNumberWarehouse();

            if (windInfoNumberWarehouse.ShowDialog() == true)
            {
                NumberWarehouse.Text = windInfoNumberWarehouse.Warehouse_Code.Text;
            }
        }

        private void InfoAboutNumberProduct_Click(object sender, RoutedEventArgs e)
        {
            WindInfoAboutNumberProduct windInfoNumberProduct = new WindInfoAboutNumberProduct();

            if (windInfoNumberProduct.ShowDialog() == true)
            {
                NumberProduct.Text = windInfoNumberProduct.Product_Code.Text;
            }
        }

        private void TemplateForKeyDown(string text, KeyEventArgs e)
        {
            if (text.Length != 0)
            {
                string st = text.Substring(text.Length - 1);

                if (st == " " && e.Key == Key.Space)
                {
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Space && text.Length == 0)
            {
                e.Handled = true;
            }
        }

        private void NumberInvoice_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TemplateForKeyDown(NumberInvoice.Text, e);
        }

        private void Quantity_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Price_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
