using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
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

namespace CoreLogic.Views
{
    /// <summary>
    /// Interaction logic for MyToolWindowContent.xaml
    /// </summary>
    public partial class MyToolWindowContent : UserControl
    {
        private string _clickedName;

        public MyToolWindowContent(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = this;
        }

        public string ClickedName
        {
            get { return _clickedName; }
            set
            {
                _clickedName = value;
                myButton.Content = string.Format("Hello {0}", value);
            }
        }


    }
}
