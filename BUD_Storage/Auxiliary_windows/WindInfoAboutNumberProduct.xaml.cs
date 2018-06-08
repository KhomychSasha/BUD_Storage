using BUD_Storage.App_Data;
using BUD_Storage.Auxiliary_windows.Windows_for_adding_elements;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for WindInfoAboutNumberProduct.xaml
    /// </summary>
    public partial class WindInfoAboutNumberProduct : Window
    {
        private DatabaseBudStorage db = new DatabaseBudStorage();

        public WindInfoAboutNumberProduct()
        {
            InitializeComponent();

            SetDataGrid();
        }

        private void SetDataGrid()
        {
            var queryProducts = from pr in db.Entities_Products
                                   join ut in db.Entities_Unit_Of_Measurements on pr.IdUnit_Of_Measurement equals ut.Id
                                   select new
                                   {
                                       IDProduct = pr.Id,
                                       Code = pr.Code,
                                       Name = pr.Name,
                                       King = pr.King,
                                       UnitOfMeasurement = ut.Marking
                                   };

            DataGridForProducts.ItemsSource = queryProducts.ToList();
        }

        private void DataGridForProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]);

                if (row != null)
                {
                    DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(0) as DataGridCell;
                    if (cell != null)
                    {
                        Product_Code.Text = ((TextBlock)cell.Content).Text;
                        BtnAddCodeProdect.IsEnabled = true;
                    }
                }
            }
        }

        static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null) child = GetVisualChild<T>(v);
                if (child != null) break;
            }
            return child;
        }

        private void BtnAddCodeProdect_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            WindowAddProduct win_add_prd = new WindowAddProduct();
            if (win_add_prd.ShowDialog() == true)
            {
                Product new_prd = new Product()
                {
                    Code = win_add_prd.new_product.Code,
                    Name = win_add_prd.new_product.Name,
                    King = win_add_prd.new_product.King,
                    IdUnit_Of_Measurement = win_add_prd.new_product.IdUnit_Of_Measurement
                };

                db.Entities_Products.Add(new_prd);
                db.SaveChanges();

                SetDataGrid();
            }
        }
    }
}
