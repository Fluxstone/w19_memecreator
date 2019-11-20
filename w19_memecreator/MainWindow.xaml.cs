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



        int i_currentSelection_Tag = 0;

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
            btn_txtField_Apply.Tag = i_currentSelection_Tag;

            cmBox_fontMenu.Height = 25;
            cmBox_fontMenu.Width = 120;
            cmBox_fontMenu.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontMenu.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontMenu.Margin = new Thickness(10, 160, 0, 0);
            cmBox_fontMenu.ItemsSource = new List<string> { "Arial", "Comic Sans" };
            cmBox_fontMenu.SelectedIndex = 0;
            //cmBox_fontMenu.AddHandler(ComboBox.SelectionChangedEvent, new RoutedEventHandler(cmBox_fontMenu_SelectionChangedEvent));
        }

        public void draw_KontextFenster()
        {
            set_KontextFenster_TextView_properties();

            grid_Kontextfenster.Children.Add(txtBox_txtField_Text);
            grid_Kontextfenster.Children.Add(btn_txtField_Apply);
            grid_Kontextfenster.Children.Add(cmBox_fontMenu);
        }

        public string generateString()
        {
            string str_text = txtBox_txtField_Text.Text;
            return str_text;
        }

        public MainWindow()
        {
            InitializeComponent();
        }



        //EventHandler
        private void btn_txtField_Apply_Click(object sender, RoutedEventArgs e)
        {
            string str_content = generateString();
            if (i_currentSelection_Tag == 1)
            {
                lbl_DEBUG_ShowText1.Content = str_content;
            } else if (i_currentSelection_Tag == 2)
            {
                lbl_DEBUG_ShowText2.Content = str_content;
            }


        }

        private void btn_DEBUG_lbl1_Click(object sender, RoutedEventArgs e)
        {
            string tag_buffer = btn_DEBUG_lbl1.Tag.ToString();
            i_currentSelection_Tag = Int32.Parse(tag_buffer);
            draw_KontextFenster();
            
            
        }

        private void btn_DEBUG_lbl2_Click(object sender, RoutedEventArgs e)
        {
            string tag_buffer = btn_DEBUG_lbl2.Tag.ToString();
            i_currentSelection_Tag = Int32.Parse(tag_buffer);
            draw_KontextFenster();
        }






        /*private void cmBox_fontMenu_SelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            String str_cmBox_fontMenu_Selection = cmBox_fontMenu.Text;
            MessageBox.Show(str_cmBox_fontMenu_Selection);


                   String str_cmBox_fontMenu_Selection = cmBox_fontMenu.Text;
            Label lbl_target = null;
            
            
            if(str_cmBox_fontMenu_Selection == "Arial")
            {
                lbl_target.FontFamily = new FontFamily("Arial"); 
            } else if(str_cmBox_fontMenu_Selection == "Comic Sans")
            {   
                lbl_target.FontFamily = new FontFamily("Comic Sans");
            } else
            {
                MessageBox.Show("Error: str_cmBox_fontMenu_Selection not found");
            }

            lbl_target.Content = txtBox_txtField_Text.Text;
        }*/


    }
}
