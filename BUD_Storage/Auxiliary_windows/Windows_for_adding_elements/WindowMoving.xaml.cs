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
    /// <summary>
    /// Interaction logic for WindowMoving.xaml
    /// </summary>
    public partial class WindowMoving : Window
    {
        private DatabaseBudStorage db = new DatabaseBudStorage();

        private Product_In_The_Warehouse prod_in_wr = null;

        private int id_warehouse;

        private int codeMoving;

        public WindowMoving(int id_prod_in_warehouse, int id_new_warehouse, int code_moving)
        {
            InitializeComponent();

            prod_in_wr = db.Entities_Product_In_The_Warehouses.Where(pr => pr.Id == id_prod_in_warehouse).SingleOrDefault();

            id_warehouse = id_new_warehouse;

            NameProduct.Text = db.Entities_Products.Where(pr => pr.Id == prod_in_wr.IdProduct).SingleOrDefault().Name;

            codeMoving = code_moving;

            if (prod_in_wr.Quantity <= 500)
            {
                for (int i = 1; i <= prod_in_wr.Quantity; ++i)
                {
                    QuantityProduct.Items.Add(i);
                }
            }
            else if (prod_in_wr.Quantity > 500 && prod_in_wr.Quantity <= 2000)
            {
                for (int i = 10; i <= prod_in_wr.Quantity; i += 10)
                {
                    QuantityProduct.Items.Add(i);
                }
            }
            else if (prod_in_wr.Quantity > 2000 && prod_in_wr.Quantity < 10000)
            {
                for (int i = 100; i <= prod_in_wr.Quantity; i += 100)
                {
                    QuantityProduct.Items.Add(i);
                }
            }
            else
            {
                for (int i = 1000; i <= prod_in_wr.Quantity; i += 1000)
                {
                    QuantityProduct.Items.Add(i);
                }
            }
        }

        private void BtnMove_Click(object sender, RoutedEventArgs e)
        {
            if (QuantityProduct.Text != String.Empty || CheckBoxAll.IsChecked == true)
            {
                int new_quantity = 0;

                if (CheckBoxAll.IsChecked == true || Int32.Parse(QuantityProduct.Text) == prod_in_wr.Quantity)
                {
                    new_quantity = prod_in_wr.Quantity;

                    db.Entities_Product_In_The_Warehouses.Attach(prod_in_wr);
                    db.Entities_Product_In_The_Warehouses.Remove(prod_in_wr);
                    db.SaveChanges();
                }
                else
                {
                    new_quantity = Int32.Parse(QuantityProduct.Text);
                    db.Entities_Product_In_The_Warehouses.ToArray()[prod_in_wr.Id - 1].Quantity -= new_quantity;
                    db.SaveChanges();
                }

                Product_In_The_Warehouse newPrdInWarehouse = new Product_In_The_Warehouse()
                {
                    IdProduct = prod_in_wr.IdProduct,
                    IdWarehouse = id_warehouse,
                    IdContractor = prod_in_wr.IdContractor,
                    Quantity = new_quantity,
                    Price = prod_in_wr.Price,
                    VAT = prod_in_wr.VAT,
                    Price_VAT = prod_in_wr.Price_VAT
                };

                db.Entities_Product_In_The_Warehouses.Add(newPrdInWarehouse);
                db.SaveChanges();

                Moving newMoving = new Moving()
                {
                    Code = codeMoving,
                    DateMoving = DateTime.Now,
                    First_Warehouse = prod_in_wr.IdWarehouse,
                    Second_Warehouse = id_warehouse,
                    Quantity = new_quantity,
                    IdProduct_In_The_Warehouse = db.Entities_Product_In_The_Warehouses.ToList().Last().Id
                };

                db.Entities_Movings.Add(newMoving);
                db.SaveChanges();

                this.DialogResult = true;
            }
            else
            {
                MessageForForms msg = new MessageForForms("Помилка!", "Не всі поля заповнені!");
                msg.ShowDialog();
            }
        }

        private void CheckBoxAll_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxAll.IsChecked == true)
            {
                QuantityProduct.IsEnabled = false;
            }
            else
            {
                QuantityProduct.IsEnabled = true;
            }
        }
    }
}
