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
    /// Interaction logic for WindowAddContractor.xaml
    /// </summary>
    public partial class WindowAddContractor : Window
    {
        public Contractor new_contractor = new Contractor(); 
        public WindowAddContractor()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ShortNameNewContractor.Text != String.Empty 
                && FullNameNewContractor.Text != String.Empty && EDRPOUNewContractor.Text != String.Empty)
            {

                new_contractor.Short_Name = ShortNameNewContractor.Text;
                new_contractor.Full_Name = FullNameNewContractor.Text;

                if (IsExistEDRPOU(EDRPOUNewContractor.Text) == false)
                {
                    new_contractor.EDRPOU = EDRPOUNewContractor.Text;

                    this.DialogResult = true;
                }
                else
                {
                    MessageForForms msg = new MessageForForms("Помилка!", "Такий ЄДРПОУ уже існує!");

                    msg.ShowDialog();

                    EDRPOUNewContractor.Text = String.Empty;
                }
            }
            else
            {
                MessageForForms msg = new MessageForForms("Помилка!", "Не всі поля заповнені!");

                msg.ShowDialog();
            }
        }

        private bool IsExistEDRPOU(string newEDRPOU)
        {
            bool isExist = false;

            using (DatabaseBudStorage db = new DatabaseBudStorage())
            {
                var ct = db.Entities_Contractors.ToList();

                foreach (var c in ct)
                {
                    if(c.EDRPOU == newEDRPOU)
                    {
                        isExist = true;
                    }
                }
            }

            return isExist;
        }

        private void EDRPOUNewContractor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
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

        private void ShortNameNewContractor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TemplateForKeyDown(ShortNameNewContractor.Text, e);
        }

        private void FullNameNewContractor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TemplateForKeyDown(FullNameNewContractor.Text, e);
        }

        private void EDRPOUNewContractor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TemplateForKeyDown(EDRPOUNewContractor.Text, e);
        }
    }
}
