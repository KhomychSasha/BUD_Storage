using BUD_Storage.App_Data;
using System;
using System.Collections.Generic;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
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
using Microsoft.Win32;

namespace BUD_Storage.Auxiliary_windows
{
    /// <summary>
    /// Interaction logic for WindowProductsForListOfMoving.xaml
    /// </summary>
    public partial class WindowProductsForListOfMoving : Window
    {
        private int code_moving;
        private string date_for_act;

        private DatabaseBudStorage db = new DatabaseBudStorage();

        public WindowProductsForListOfMoving(int id_moving)
        {
            InitializeComponent();

            date_for_act = db.Entities_Movings.Where(m => m.Id == id_moving).SingleOrDefault().DateMoving.ToShortDateString();

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

        private void BtnSaveAct_Click(object sender, RoutedEventArgs e)
        {
            Word.Application application = new Word.Application();
            application.Visible = false;

            SaveFileDialog save_file = new SaveFileDialog();
            save_file.Filter = "Word|*.docx";
            save_file.FileName = "Акт переміщення №" + code_moving.ToString();

            Word.Document doc = null;

            try
            {
                if (save_file.ShowDialog() == true)
                {
                    string filePathForTempReport = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\StartDB\TemplateAct.docx";

                    doc = application.Documents.Open(filePathForTempReport);

                    ReplaceWordStub("{act_number}", code_moving.ToString(), doc);
                    ReplaceWordStub("{date}", date_for_act, doc);
                    ReplaceWordStub("{first_warehouse1}", FirstWarehouse.Text, doc);
                    ReplaceWordStub("{second_warehouse1}", SecondWarehouse.Text, doc);
                    ReplaceWordStub("{first_warehouse2}", FirstWarehouse.Text, doc);
                    ReplaceWordStub("{second_warehouse2}", SecondWarehouse.Text, doc);
                    ReplaceWordStub("{first_warehouse3}", FirstWarehouse.Text, doc);
                    ReplaceWordStub("{second_warehouse3}", SecondWarehouse.Text, doc); 

                    Word.Range bookmarkRange = doc.Bookmarks.get_Item("Table_For_Products").Range;

                    var queryProducts = (from mv in db.Entities_Movings
                                        join pr in db.Entities_Product_In_The_Warehouses on mv.IdProduct_In_The_Warehouse equals pr.Id
                                        join p in db.Entities_Products on pr.IdProduct equals p.Id
                                        where mv.Code == code_moving
                                        select new
                                        {
                                            IDProduct = pr.Id,
                                            NameProduct = p.Name,
                                            Quantity = pr.Quantity,
                                            Price = pr.Price_VAT
                                        }).ToArray();

                    Word.Table t_prod = doc.Tables.Add(bookmarkRange, queryProducts.Count() + 1, 5);
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
                                    cell.Range.Text = "№";
                                    cell.Width = 50;
                                }
                                else if (cell.ColumnIndex == 2)
                                {
                                    cell.Range.Text = "Номенклатура і якісні характеристики речі";
                                    cell.Width = 200;
                                }
                                else if (cell.ColumnIndex == 3)
                                {
                                    cell.Range.Text = "Кількість одиниць";
                                    cell.Width = 60;
                                }
                                else if (cell.ColumnIndex == 4)
                                {
                                    cell.Range.Text = "Оціночна вартість";
                                    cell.Width = 80;
                                }
                                else if (cell.ColumnIndex == 5)
                                {
                                    cell.Range.Text = "Примітки";
                                    cell.Width = 80;
                                }
                            }
                            else
                            {
                                if (cell.ColumnIndex == 1)
                                {
                                    cell.Range.Text = queryProducts[count - 1].IDProduct.ToString();
                                    cell.Width = 50;
                                }
                                else if (cell.ColumnIndex == 2)
                                {
                                    cell.Range.Text = queryProducts[count - 1].NameProduct.ToString();
                                    cell.Width = 200;
                                }
                                else if (cell.ColumnIndex == 3)
                                {
                                    cell.Range.Text = queryProducts[count - 1].Quantity.ToString();
                                    cell.Width = 60;
                                }
                                else if (cell.ColumnIndex == 4)
                                {
                                    cell.Range.Text = queryProducts[count - 1].Price.ToString();
                                    cell.Width = 80;
                                }
                                else if (cell.ColumnIndex == 5)
                                {
                                    cell.Range.Text = "";
                                    cell.Width = 80;
                                }                  
                            }
                        }
                        ++count;
                    }
                    
                    doc.SaveAs2(save_file.FileName);

                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                doc.Close();
                MessageBox.Show(ex.Message);
            }


            application.Quit();
        }

        private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            Word.Range range = wordDocument.Content;
            range.Find.ClearFormatting();

            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        /*public void ReplaceAllStrings(string strToFind, string replaceStr, Word.Document _document)
        {

            object strToFindObj = strToFind;
            object replaceStrObj = replaceStr;

            Word.Range wordRange;

            object replaceTypeObj;

            replaceTypeObj = Word.WdReplace.wdReplaceAll;

            try
            {
                for (int i = 1; i <= _document.Sections.Count; i++)
                {
                    wordRange = _document.Sections[i].Range;

                    Word.Find wordFindObj = wordRange.Find;


                    object[] wordFindParameters = new object[15] { strToFindObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, _missingObj, replaceStrObj, replaceTypeObj, _missingObj, _missingObj, _missingObj, _missingObj };

                    wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/
    }
}
