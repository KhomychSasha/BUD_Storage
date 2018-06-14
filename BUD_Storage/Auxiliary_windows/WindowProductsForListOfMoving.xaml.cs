﻿using BUD_Storage.App_Data;
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
    /// Interaction logic for WindowProductsForListOfMoving.xaml
    /// </summary>
    public partial class WindowProductsForListOfMoving : Window
    {
        private int code_moving;

        private DatabaseBudStorage db = new DatabaseBudStorage();

        public WindowProductsForListOfMoving(int id_moving)
        {
            InitializeComponent();

            code_moving = db.Entities_Movings.Where(m => m.Id == id_moving).SingleOrDefault().Code;

            NumberMoving.Text += code_moving.ToString();
            DataMoving.Text += db.Entities_Movings.Where(m => m.Id == id_moving).SingleOrDefault().DateMoving.ToShortDateString();

            int id_first_w = db.Entities_Movings.Where(m => m.Id == id_moving).SingleOrDefault().First_Warehouse;
            FirstWarehouse.Text = db.Entities_Warehouses.Where(w => w.Id == id_first_w).SingleOrDefault().Name;

            int id_second_w = db.Entities_Movings.Where(m => m.Id == id_moving).SingleOrDefault().Second_Warehouse;
            SecondWarehouse.Text = db.Entities_Warehouses.Where(w => w.Id == id_second_w).SingleOrDefault().Name;


            SetDataGrid();
        }

        private void SetDataGrid()
        {
            var queryProducts = from mv in db.Entities_Movings
                                join pr in db.Entities_Product_In_The_Warehouses on mv.IdProduct_In_The_Warehouse equals pr.Id
                                join p in db.Entities_Products on pr.IdProduct equals p.Id
                                where mv.Code == code_moving
                                select new
                                {
                                    IDProduct = pr.Id,
                                    NameProduct = p.Name,
                                    Quantity = pr.Quantity,
                                    Price = pr.Price_VAT
                                };

            DataGridForProducts.ItemsSource = queryProducts.ToList();
        }

        private void BtnPrintAct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}