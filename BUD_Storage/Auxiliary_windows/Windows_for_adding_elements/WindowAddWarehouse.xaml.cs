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

namespace BUD_Storage.Auxiliary_windows.Windows_for_adding_elements
{
    /// <summary>
    /// Interaction logic for WindowAddWarehouse.xaml
    /// </summary>
    public partial class WindowAddWarehouse : Window
    {
        public Warehouse new_warehouse = new Warehouse();
        public WindowAddWarehouse()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (NameNewWarehouse.Text != String.Empty)
            {
                new_warehouse.Name = NameNewWarehouse.Text;

                if (CommentNewWarehouse.Text != String.Empty)
                {
                    new_warehouse.Comment = CommentNewWarehouse.Text;
                }

                this.DialogResult = true;
            }
            else
            {
                MessageForForms msg = new MessageForForms("Помилка!", "Не всі поля заповнені!");

                msg.ShowDialog();
            }
        }

        private void TemplateForKeyDown(string text, KeyEventArgs e)
        {
            if (text.Length != 0)
            {
                string st = text.Substring(text.Length - 1);

                if (st == " " && e.Key == Key.Space)
                {
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Space && text.Length == 0)
            {
                e.Handled = true;
            }
        }

        private void NameNewWarehouse_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TemplateForKeyDown(NameNewWarehouse.Text, e);
        }

        private void CommentNewWarehouse_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TemplateForKeyDown(CommentNewWarehouse.Text, e);
        }
    }
}
