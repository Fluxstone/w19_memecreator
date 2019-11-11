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

namespace w19_memecreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Button btn_DEBUG_Button1 = new Button();

        public void set_btn_DEBUG_Button1_properties()
        {
            btn_DEBUG_Button1.Height = 25;
            btn_DEBUG_Button1.Width = 80;
            btn_DEBUG_Button1.Content = "Codebutton";

            btn_DEBUG_Button1.HorizontalAlignment = HorizontalAlignment.Left;
            btn_DEBUG_Button1.VerticalAlignment = VerticalAlignment.Top;
            btn_DEBUG_Button1.Margin = new Thickness(700, 200, 0, 0);

            btn_DEBUG_Button1.AddHandler(Button.ClickEvent, new RoutedEventHandler(btn_DEBUG_Button1_Click));
        }
       

        private void btn_DEBUG_Control1_Click(object sender, RoutedEventArgs e)
        {
            set_btn_DEBUG_Button1_properties();
            grid_mainGrid.Children.Add(btn_DEBUG_Button1);
        }

        private void btn_DEBUG_Control2_Click(object sender, RoutedEventArgs e)
        {
            grid_mainGrid.Children.Remove(btn_DEBUG_Button1);
        }

        private void btn_DEBUG_Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ok");
        }


    }
}
