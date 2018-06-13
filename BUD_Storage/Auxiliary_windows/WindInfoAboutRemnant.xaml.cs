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

namespace BUD_Storage.Auxiliary_windows
{
    /// <summary>
    /// Interaction logic for WindInfoAboutRemnant.xaml
    /// </summary>
    public partial class WindInfoAboutRemnant : Window
    {
        private DatabaseBudStorage db = new DatabaseBudStorage();

        private int idElectWarehouse = 0;
        public WindInfoAboutRemnant(int idWarehouse)
        {
            InitializeComponent();

            idElectWarehouse = idWarehouse;

            TextBlockNameWarehouse.Text = db.Entities_Warehouses.Where(p => p.Id == idElectWarehouse).SingleOrDefault().Name;

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
                                                  Price = prwr.Price_VAT,
                                                  PriceAll = prwr.Quantity * prwr.Price_VAT
                                              };

            DataGridForProductsOnWarehouse.ItemsSource = queryProductInTheWarehouses.ToList();

            decimal sum = 0;
            foreach (var pr in queryProductInTheWarehouses.ToList())
            {
                sum += pr.PriceAll;
            }

            Sum.Text = sum.ToString();
        }
    }
}
