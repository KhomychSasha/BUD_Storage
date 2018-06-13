using BUD_Storage.App_Data;
using BUD_Storage.Auxiliary_windows.Windows_for_adding_elements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BUD_Storage.Auxiliary_windows
{
    /// <summary>
    /// Interaction logic for WindAddCreateNewMoving.xaml
    /// </summary>
    public partial class WindAddCreateNewMoving : Window
    {
        private DatabaseBudStorage db = new DatabaseBudStorage();

        private int idElectWarehouse = 0;

        public WindAddCreateNewMoving(int idWarehouse)
        {
            InitializeComponent();

            idElectWarehouse = idWarehouse;

            TextBlockNameWarehouse.Text = db.Entities_Warehouses.Where(p => p.Id == idElectWarehouse).SingleOrDefault().Name;

            var queryWarehouses = db.Entities_Warehouses.Where(w => w.Id != idElectWarehouse).ToList();

            foreach (var wr in queryWarehouses)
            {
                Warehouses.Items.Add(wr.Name);
            }

            SetDataGrid();
        }

        private void SetDataGrid()
        {
            var queryProductInTheWarehouses = from prwr in db.Entities_Product_In_The_Warehouses
                                              join pr in db.Entities_Products on prwr.IdProduct equals pr.Id
                                              where prwr.IdWarehouse == idElectWarehouse
                                              select new
                                              {
                                                  IDProductInWarehouse = prwr.Id,
                                                  NameProduct = pr.Name,
                                                  Quantity = prwr.Quantity,
                                              };

            DataGridForProductsOnWarehouse.ItemsSource = queryProductInTheWarehouses.ToList();
        }

        private void ChangeWarehouse_Click(object sender, RoutedEventArgs e)
        {
            var newWarehouse = db.Entities_Warehouses.Where(w => w.Name == Warehouses.Text).SingleOrDefault();

            List<int> list_prod_id = new List<int>();

            int numberProduct = DataGridForProductsOnWarehouse.Items.Count;

            if (numberProduct > 0)
            {
                for (int i = 0; i < numberProduct; ++i)
                {
                    CheckBox checkbox = DataGridForProductsOnWarehouse.Columns[3].GetCellContent(DataGridForProductsOnWarehouse.Items[i]) as CheckBox;

                    if (checkbox.IsChecked == true)
                    {
                        TextBlock idProd = DataGridForProductsOnWarehouse.Columns[0].GetCellContent(DataGridForProductsOnWarehouse.Items[i]) as TextBlock;

                        list_prod_id.Add(Int32.Parse(idProd.Text));
                    }
                }
            }
            if (list_prod_id.Count == 0)
            {
                MessageForForms msg = new MessageForForms("Помилка!", "Не вибрано жодного товару!");
                msg.ShowDialog();
            }
            else
            {
                int codeNewMoving = 0;
                if (db.Entities_Movings.Count() > 0)
                {
                    codeNewMoving = db.Entities_Movings.ToList().Last().Code + 1;
                }

                foreach (var ls in list_prod_id)
                {
                    WindowMoving moving = new WindowMoving(ls, newWarehouse.Id, codeNewMoving);
                    moving.ShowDialog();
                }
                SetDataGrid();
                Warehouses.Text = String.Empty;
                ChangeWarehouse.IsEnabled = false;
            }
        }

        private void Warehouses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeWarehouse.IsEnabled = true;
        }
    }

}
