using BUD_Storage.App_Data;
using BUD_Storage.Auxiliary_windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BUD_Storage.Windows
{
    /// <summary>
    /// Interaction logic for ListOfMoving.xaml
    /// </summary>
    public partial class ListOfMoving : UserControl
    {
        private MainWindow mainWin = null;

        private DatabaseBudStorage db = new DatabaseBudStorage();

        public ListOfMoving(MainWindow mw)
        {
            InitializeComponent();

            mainWin = mw;

            SetDataGrid();
        }

        private void SetDataGrid()
        {
            var queryMoving = (from mv in db.Entities_Movings
                              join fwr in db.Entities_Warehouses on mv.First_Warehouse equals fwr.Id
                              join swr in db.Entities_Warehouses on mv.Second_Warehouse equals swr.Id
                              select new
                                  {
                                      IDMoving = mv.Id,
                                      CodeMoving = mv.Code,
                                      FirstWarehouse = fwr.Name,
                                      SecondWarehouse = swr.Name,
                                      DateMoving = mv.DateMoving
                                  }).ToArray();

            for (int i = 0; i < queryMoving.Count(); ++i)
            {
                if (i != 0)
                {
                    if (queryMoving[i].CodeMoving != queryMoving[i - 1].CodeMoving)
                    {
                        DataGridForMovings.Items.Add(queryMoving[i]);
                    }
                }
                else
                {
                    DataGridForMovings.Items.Add(queryMoving[i]);
                }
            }
        }

        private void DataGridForMovings_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                        NumberMoving.Text = ((TextBlock)cell.Content).Text;
                        BtnChooseMoving.IsEnabled = true;
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

        private void BtnReturnToMailPage_Click(object sender, RoutedEventArgs e)
        {
            mainWin.WorkArea.Children.Clear();
            mainWin.WorkArea.Children.Add(new WelcomeWindow());
        }

        private void BtnChooseMoving_Click(object sender, RoutedEventArgs e)
        {
            int idMoving = Int32.Parse(NumberMoving.Text);

            WindowProductsForListOfMoving wind_prd_for_list_of_mv = new WindowProductsForListOfMoving(idMoving);
            wind_prd_for_list_of_mv.ShowDialog();
        }
    }
}
