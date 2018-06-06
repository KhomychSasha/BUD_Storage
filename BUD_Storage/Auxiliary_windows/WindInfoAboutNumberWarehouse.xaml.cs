using BUD_Storage.App_Data;
using BUD_Storage.ExcelTables;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for WindInfoAboutNumberWarehouse.xaml
    /// </summary>
    public partial class WindInfoAboutNumberWarehouse : Window
    {
        public WindInfoAboutNumberWarehouse()
        {
            InitializeComponent();

            using (DatabaseBudStorage db = new DatabaseBudStorage())
            {

                var unit = db.Entities_Unit_Of_Measurements.ToArray();

                MessageBox.Show(unit[3].Name);
                db.SaveChanges();
            }
        }
    }
}
