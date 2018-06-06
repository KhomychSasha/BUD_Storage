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
    /// Interaction logic for MessageForForms.xaml
    /// </summary>
    public partial class MessageForForms : Window
    {
        public MessageForForms(string title, string message)
        {
            InitializeComponent();

            WindMessage.Title = title;
            Message.Text = message;
        }
    }
}
