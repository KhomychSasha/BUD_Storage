using BUD_Storage.App_Data;
using BUD_Storage.Auxiliary_windows.Windows_for_adding_elements;
using BUD_Storage.ExcelTables;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for WindInfoAboutNumberWarehouse.xaml
    /// </summary>
    public partial class WindInfoAboutNumberWarehouse : Window
    {
        private DatabaseBudStorage db = new DatabaseBudStorage();

        public WindInfoAboutNumberWarehouse()
        {
            InitializeComponent();

            SetDataGrid();
        }

        private void SetDataGrid()
        {
            var queryWarehouses = from wr in db.Entities_Warehouses
                                   select new
                                   {
                                       IDWarehouse = wr.Id,
                                       Name = wr.Name,
                                       Comment = wr.Comment
                                   };
            DataGridForWarehouses.ItemsSource = queryWarehouses.ToList();
        }

        private void DataGridForWarehouses_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                        Warehouse_Code.Text = ((TextBlock)cell.Content).Text;
                        BtnAddWarehouseCode.IsEnabled = true;
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

        private void BtnAddWarehouseCode_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnAddNewWarehouse_Click(object sender, RoutedEventArgs e)
        {
            WindowAddWarehouse win_add_wr = new WindowAddWarehouse();
            if (win_add_wr.ShowDialog() == true)
            {
                Warehouse new_wr = new Warehouse()
                {
                    Name = win_add_wr.new_warehouse.Name,
                    Comment = win_add_wr.new_warehouse.Comment
                };

                db.Entities_Warehouses.Add(new_wr);
                db.SaveChanges();

                SetDataGrid();
            }
        }
    }
}
