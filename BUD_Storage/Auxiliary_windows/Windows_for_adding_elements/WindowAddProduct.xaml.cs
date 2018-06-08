using BUD_Storage.App_Data;
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
using System.Windows.Shapes;

namespace BUD_Storage.Auxiliary_windows.Windows_for_adding_elements
{
    public partial class WindowAddProduct : Window
    {
        private DatabaseBudStorage db = new DatabaseBudStorage();

        public Product new_product = new Product();
        public WindowAddProduct()
        {
            InitializeComponent();

            var unit = db.Entities_Unit_Of_Measurements.ToList();

            foreach (var un in unit)
            {
                UnitOFMeasurementNewProduct.Items.Add(un.Marking);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (NameNewProduct.Text != String.Empty && KingNewProduct.Text != String.Empty 
                && UnitOFMeasurementNewProduct.Text != String.Empty)
            {
                new_product.Code = (db.Entities_Products.Max(pr => pr.Code) + 1);
                new_product.Name = NameNewProduct.Text;
                new_product.King = KingNewProduct.Text;
                new_product.IdUnit_Of_Measurement = (from un in db.Entities_Unit_Of_Measurements
                                                    where un.Marking == UnitOFMeasurementNewProduct.Text
                                                    select un.Id).SingleOrDefault();

                this.DialogResult = true;
            }
            else
            {
                MessageForForms msg = new MessageForForms("Помилка!", "Не всі поля заповнені!");

                msg.ShowDialog();
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

        private void NameNewProduct_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TemplateForKeyDown(NameNewProduct.Text, e);
        }

        private void KingNewProduct_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TemplateForKeyDown(KingNewProduct.Text, e);
        }
    }
}
