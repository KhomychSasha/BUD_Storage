using BUD_Storage.App_Data;
using Microsoft.Win32;
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
using Word = Microsoft.Office.Interop.Word;


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

            if (queryProductInTheWarehouses.Count() > 0)
            {
                BtnSaveReport.IsEnabled = true;
            }

            decimal sum = 0;
            foreach (var pr in queryProductInTheWarehouses.ToList())
            {
                sum += pr.PriceAll;
            }

            Sum.Text = sum.ToString();
        }

        private void BtnSaveReport_Click(object sender, RoutedEventArgs e)
        {
            var app = new Word.Application();
            app.Visible = false;

            SaveFileDialog save_file = new SaveFileDialog();
            save_file.Filter = "Word|*.docx";
            save_file.FileName = TextBlockNameWarehouse.Text;

            try
            {
                if (save_file.ShowDialog() == true)
                {
                    string filePathForTempReport = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\StartDB\TemplateReport.docx";

                    var doc = app.Documents.Open(filePathForTempReport);

                    Word.Paragraph pr_title = doc.Paragraphs.Add();

                    pr_title.Range.Text = TextBlockNameWarehouse.Text;
                    pr_title.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    pr_title.Range.InsertParagraphAfter();


                    var queryProductInTheWarehouses = (from prwr in db.Entities_Product_In_The_Warehouses
                                                       join pr in db.Entities_Products on prwr.IdProduct equals pr.Id
                                                       where prwr.IdWarehouse == idElectWarehouse
                                                       select new
                                                       {
                                                           IDProductInWarehouse = prwr.Id,
                                                           NameProduct = pr.Name,
                                                           Quantity = prwr.Quantity,
                                                           Price = prwr.Price_VAT,
                                                           PriceAll = prwr.Quantity * prwr.Price_VAT
                                                       }).ToArray();

                    Word.Paragraph pr_table = doc.Paragraphs.Add();
                    Word.Table t_prod = doc.Tables.Add(pr_table.Range, queryProductInTheWarehouses.Count() + 1, 5);
                    t_prod.Borders.Enable = 1;

                    int count = 0;
                    foreach (Word.Row row in t_prod.Rows)
                    {
                        foreach (Word.Cell cell in row.Cells)
                        {
                            if (cell.RowIndex == 1)
                            {
                                if (cell.ColumnIndex == 1)
                                {
                                    cell.Range.Text = "Номер";
                                    cell.Width = 50;
                                }
                                else if (cell.ColumnIndex == 2)
                                {
                                    cell.Range.Text = "Назва";
                                    cell.Width = 200;
                                }
                                else if (cell.ColumnIndex == 3)
                                {
                                    cell.Range.Text = "Кількість";
                                    cell.Width = 60;
                                }
                                else if (cell.ColumnIndex == 4)
                                {
                                    cell.Range.Text = "Ціна за один";
                                    cell.Width = 80;
                                }
                                else if (cell.ColumnIndex == 5)
                                {
                                    cell.Range.Text = "Ціна за всі";
                                    cell.Width = 80;
                                }
                            }
                            else
                            {
                                if (cell.ColumnIndex == 1)
                                {
                                    cell.Range.Text = queryProductInTheWarehouses[count - 1].IDProductInWarehouse.ToString();
                                    cell.Width = 50;
                                }
                                else if (cell.ColumnIndex == 2)
                                {
                                    cell.Range.Text = queryProductInTheWarehouses[count - 1].NameProduct.ToString();
                                    cell.Width = 200;
                                }
                                else if (cell.ColumnIndex == 3)
                                {
                                    cell.Range.Text = queryProductInTheWarehouses[count - 1].Quantity.ToString();
                                    cell.Width = 60;
                                }
                                else if (cell.ColumnIndex == 4)
                                {
                                    cell.Range.Text = queryProductInTheWarehouses[count - 1].Price.ToString();
                                    cell.Width = 80;
                                }
                                else if (cell.ColumnIndex == 5)
                                {
                                    cell.Range.Text = queryProductInTheWarehouses[count - 1].PriceAll.ToString();
                                    cell.Width = 80;
                                }
                            }
                        }
                        ++count;
                    }

                    Word.Paragraph pr_all = doc.Paragraphs.Add();

                    pr_all.Range.Text = "Всього: " + Sum.Text;
                    pr_all.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                    pr_all.Range.InsertParagraphAfter();

                    doc.SaveAs2(save_file.FileName);

                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            app.Quit();
        } 
    }
}
