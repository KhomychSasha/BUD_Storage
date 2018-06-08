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
    /// Interaction logic for WindInfoAboutNumberContractor.xaml
    /// </summary>
    public partial class WindInfoAboutNumberContractor : Window
    {
        private DatabaseBudStorage db = new DatabaseBudStorage();

        public WindInfoAboutNumberContractor()
        {
            InitializeComponent();

            SetDataGrid();
        }


        private void SetDataGrid()
        {
            var queryContracrots = from ct in db.Entities_Contractors
                                   select new
                                   {
                                       IDContractor = ct.Id,
                                       FullName = ct.Full_Name,
                                       EDRPOU = ct.EDRPOU,
                                   };

            DataGridForContractors.ItemsSource = queryContracrots.ToList();
        }

        private void DataGridForContractors_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                        Contractor_Code.Text = ((TextBlock)cell.Content).Text;
                        BtnAddContractor.IsEnabled = true;
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

        private void BtnAddContractor_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnAddNewContractor_Click(object sender, RoutedEventArgs e)
        {
            WindowAddContractor win_add_ct = new WindowAddContractor();
            if (win_add_ct.ShowDialog() == true)
            {
                Contractor new_ctr = new Contractor()
                {
                    Short_Name = win_add_ct.new_contractor.Short_Name,
                    Full_Name = win_add_ct.new_contractor.Full_Name,
                    EDRPOU = win_add_ct.new_contractor.EDRPOU
                };

                db.Entities_Contractors.Add(new_ctr);
                db.SaveChanges();

                SetDataGrid();
            }
        }
    }
}
