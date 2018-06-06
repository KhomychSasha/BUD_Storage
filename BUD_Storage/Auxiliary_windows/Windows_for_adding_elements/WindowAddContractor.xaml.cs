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
                this.DialogResult = true;

                new_contractor.Short_Name = ShortNameNewContractor.Text;
                new_contractor.Full_Name = FullNameNewContractor.Text;
                new_contractor.EDRPOU = EDRPOUNewContractor.Text;
            }
            else
            {
                MessageForForms msg = new MessageForForms("Помилка!", "Не всі поля заповнені!");

                msg.ShowDialog();
            }
        }

        private void EDRPOUNewContractor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }
    }
}
