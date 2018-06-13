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
    /// Interaction logic for Remnants.xaml
    /// </summary>
    public partial class Remnants : UserControl
    {
        private DatabaseBudStorage db = new DatabaseBudStorage();

        private MainWindow mainWin = null;

        private int idChooseWarehouse = 0;

        public Remnants(MainWindow mw)
        {
            InitializeComponent();

            mainWin = mw;
            SetStartStackPanel();
        }

        private void SetStartStackPanel()
        {
            StackPanelForWarehouse.Children.Clear();

            var warehouses = db.Entities_Warehouses.ToList();

            foreach (var wr in warehouses)
            {
                AddWarehouseBtnToStackPanel(wr.Id, wr.Name);
            }
        }

        private void AddWarehouseBtnToStackPanel(int id, string name)
        {
            Button btn = new Button() { Height = 40, FontSize = 22, Background = (Brush)new BrushConverter().ConvertFrom("#FF95AAC3") };
            btn.Name = "St_" + id.ToString();
            btn.Content = id.ToString() + ". " + name;
            btn.Click += new RoutedEventHandler(TemplateClick);

            StackPanelForWarehouse.Children.Add(btn);
        }

        private void TemplateClick(object sender, RoutedEventArgs e)
        {
            Button tempBtn = (Button)sender;
            int idStorage = Int32.Parse(tempBtn.Name.Remove(0, 3));

            var warehous = db.Entities_Warehouses.ToArray()[idStorage - 1];

            NameWarehouse.Text = warehous.Id.ToString() + ". " + warehous.Name; 
            idChooseWarehouse = warehous.Id;

            ChooseWarehouse.IsEnabled = true;
        }

        private void BtnReturnToMailPage_Click(object sender, RoutedEventArgs e)
        {
            mainWin.WorkArea.Children.Clear();
            mainWin.WorkArea.Children.Add(new WelcomeWindow());
        }

        private void ChooseWarehouse_Click(object sender, RoutedEventArgs e)
        {
            WindInfoAboutRemnant win_info_about_remnant = new WindInfoAboutRemnant(idChooseWarehouse);
            win_info_about_remnant.ShowDialog();
        }
    }
}
