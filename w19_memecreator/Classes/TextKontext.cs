using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace w19_memecreator
{
    class TextKontext
    {
        //Variables
        TextBox txtBox_txtField_Text = new TextBox();
        Button btn_txtField_Apply = new Button();
        ComboBox cmBox_fontMenu = new ComboBox();
        ComboBox cmBox_fontSize = new ComboBox();
        Label lbl_targetLbl;

        //Constructor
        public TextKontext()
        {

        }

        //Set KontextWindow Controls
        public void setWindowProperties()
        {
            txtBox_txtField_Text.Height = 100;
            txtBox_txtField_Text.Width = 300;
            txtBox_txtField_Text.Text = "";
            txtBox_txtField_Text.HorizontalAlignment = HorizontalAlignment.Left;
            txtBox_txtField_Text.VerticalAlignment = VerticalAlignment.Top;
            txtBox_txtField_Text.AcceptsReturn = true;
            txtBox_txtField_Text.Margin = new Thickness(10, 20, 0, 0);

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
            cmBox_fontMenu.ItemsSource = new List<string> { "impact", "Arial", "Comic Sans MS" };
            cmBox_fontMenu.SelectedIndex = 0;
            
            cmBox_fontSize.Height = 25;
            cmBox_fontSize.Width = 120;
            cmBox_fontSize.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontSize.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontSize.Margin = new Thickness(160, 160, 0, 0);
            cmBox_fontSize.ItemsSource = new List<string> { "4", "6", "10", "15", "30" };
            cmBox_fontSize.SelectedIndex = 0;
        }

        public void generateLabel()
        {
            int i_fontSize = Int32.Parse(cmBox_fontSize.Text);

            if (cmBox_fontMenu.Text == "impact")
            {
                lbl_targetLbl.FontFamily = new FontFamily("impact");
            }
            else if (cmBox_fontMenu.Text == "Arial")
            {
                lbl_targetLbl.FontFamily = new FontFamily("Arial");
            }
            else if (cmBox_fontMenu.Text == "Comic Sans MS")
            {
                lbl_targetLbl.FontFamily = new FontFamily("Comic Sans MS");
            }
            else
            {
                MessageBox.Show("Error: str_cmBox_fontMenu_Selection not found");
            }

            lbl_targetLbl.FontSize = i_fontSize;
            lbl_targetLbl.Content = txtBox_txtField_Text.Text;
        }

        public void set_targetLbl(Label lbl_in)
        {
            lbl_targetLbl = lbl_in;
        }

        //Getter und Setter
        public TextBox get_txtField_Text()
        {
            txtBox_txtField_Text.Text = lbl_targetLbl.Content.ToString();
            return txtBox_txtField_Text;
        }
        public Button get_btn_txtField_Apply()
        {
            return btn_txtField_Apply;
        }
        public ComboBox get_cmBox_fontMenu()
        {
            return cmBox_fontMenu;
        }public ComboBox get_cmBox_fontSize()
        {
            return cmBox_fontSize;
        }

        //Event Handler
        private void btn_txtField_Apply_Click(object sender, RoutedEventArgs e)
        {
            generateLabel();
        }
    }
}
