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
        TextBox txtBox_txtField_Text = new TextBox();
        Button btn_txtField_Apply = new Button();
        ComboBox cmBox_fontMenu = new ComboBox();

        public void set_KontextFenster_TextView_properties()
        {
            txtBox_txtField_Text.Height = 100;
            txtBox_txtField_Text.Width = 300;
            txtBox_txtField_Text.Text = "";
            txtBox_txtField_Text.HorizontalAlignment = HorizontalAlignment.Left;
            txtBox_txtField_Text.VerticalAlignment = VerticalAlignment.Top;
            txtBox_txtField_Text.Margin = new Thickness(10,20,0,0);
            
            btn_txtField_Apply.Height = 25;
            btn_txtField_Apply.Width = 90;
            btn_txtField_Apply.Content = "Apply Changes";
            btn_txtField_Apply.HorizontalAlignment = HorizontalAlignment.Left;
            btn_txtField_Apply.VerticalAlignment = VerticalAlignment.Top;
            btn_txtField_Apply.Margin = new Thickness(10, 130, 0, 0);
            btn_txtField_Apply.AddHandler(Button.ClickEvent, new RoutedEventHandler(btn_txtField_Apply_Click));

            cmBox_fontMenu.Height = 25;
            cmBox_fontMenu.Width = 120;
            cmBox_fontMenu.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontMenu.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontMenu.Margin = new Thickness(10, 160, 0, 0);
            cmBox_fontMenu.ItemsSource = new List<string> { "Arial", "Comic Sans" };
            cmBox_fontMenu.SelectedIndex = 0;
            //cmBox_fontMenu.AddHandler(ComboBox.SelectionChangedEvent, new RoutedEventHandler(cmBox_fontMenu_SelectionChangedEvent));
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_DEBUG_ContextTextView_Click(object sender, RoutedEventArgs e)
        {  
            grid_Kontextfenster.Children.Add(txtBox_txtField_Text);
            grid_Kontextfenster.Children.Add(btn_txtField_Apply);
            grid_Kontextfenster.Children.Add(cmBox_fontMenu);

            set_KontextFenster_TextView_properties();
        }

        //EventHandler
        private void btn_txtField_Apply_Click(object sender, RoutedEventArgs e)
        {
            String str_cmBox_fontMenu_Selection = cmBox_fontMenu.Text;
            
            
            if(str_cmBox_fontMenu_Selection == "Arial")
            {
                lbl_DEBUG_ShowText.FontFamily = new FontFamily("Arial"); 
            } else if(str_cmBox_fontMenu_Selection == "Comic Sans")
            {
                lbl_DEBUG_ShowText.FontFamily = new FontFamily("Comic Sans");
            } else
            {
                MessageBox.Show("Error: str_cmBox_fontMenu_Selection not found");
            }

            lbl_DEBUG_ShowText.Content = txtBox_txtField_Text.Text;
        }

        /*private void cmBox_fontMenu_SelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            String str_cmBox_fontMenu_Selection = cmBox_fontMenu.Text;
            MessageBox.Show(str_cmBox_fontMenu_Selection);
        }*/

        
    }
}
