using System;
using System.Collections.Generic;
using System.Drawing.Text;
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
        ComboBox cmBox_fontColor = new ComboBox();
        Label lbl_Text = new Label();
        Label lbl_Color = new Label();
        Label lbl_targetLbl;

        //Constructor
        public TextKontext()
        {
            
        }

        //Set KontextWindow Controls
        public void setWindowProperties()
        {
            lbl_Color.Height = 30;
            lbl_Color.Width = 120;
            lbl_Color.Content = "Color options";
            lbl_Color.Foreground = Brushes.LightGray;
            lbl_Color.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Color.VerticalAlignment = VerticalAlignment.Top;
            lbl_Color.Margin = new Thickness(10, 220, 0, 0);

            lbl_Text.Height = 30;
            lbl_Text.Width = 120;
            lbl_Text.Content = "Text options";
            lbl_Text.Foreground = Brushes.LightGray;
            lbl_Text.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Text.VerticalAlignment = VerticalAlignment.Top;
            lbl_Text.Margin = new Thickness(10, 160, 0, 0);

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
            cmBox_fontMenu.Margin = new Thickness(10, 190, 0, 0);

            List<string> fonts = new List<string>();
            using (InstalledFontCollection fontsCollection = new InstalledFontCollection())
            {
                System.Drawing.FontFamily[] fontFamilies = fontsCollection.Families;
                foreach (System.Drawing.FontFamily font in fontFamilies)
                {
                    fonts.Add(font.Name);
                }
            }
            cmBox_fontMenu.ItemsSource = fonts;
            cmBox_fontMenu.SelectedIndex = 0;
            
            cmBox_fontSize.Height = 25;
            cmBox_fontSize.Width = 120;
            cmBox_fontSize.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontSize.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontSize.Margin = new Thickness(160, 190, 0, 0);
            List<string> fontSizeList = new List<string>();
            for (int i = 4; i < 70; i++)
            {
                fontSizeList.Add(i.ToString());
            }
            cmBox_fontSize.ItemsSource = fontSizeList;
            cmBox_fontSize.SelectedIndex = 0;

            cmBox_fontColor.Height = 25;
            cmBox_fontColor.Width = 120;
            cmBox_fontColor.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontColor.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontColor.Margin = new Thickness(10, 250, 0, 0);
            List<string> lst_colors = new List<string>();
            foreach (System.Reflection.PropertyInfo info in typeof(Color).GetProperties())
            {
                lst_colors.Add(info.Name);
            }
            cmBox_fontColor.ItemsSource = lst_colors;
            cmBox_fontColor.SelectedIndex = 0; //DEBUG: This isnt working
        }

        public void generateLabel()
        {
            int i_fontSize = Int32.Parse(cmBox_fontSize.Text);
            lbl_targetLbl.FontFamily = new FontFamily(cmBox_fontMenu.SelectedValue.ToString());
            lbl_targetLbl.FontSize = i_fontSize;
            SolidColorBrush brush_Target = (SolidColorBrush)new BrushConverter().ConvertFromString(cmBox_fontColor.SelectedItem.ToString());
            lbl_targetLbl.Foreground = brush_Target;
            lbl_targetLbl.Content = txtBox_txtField_Text.Text;
        }

        public void set_targetLbl(Label lbl_in)
        {
            lbl_targetLbl = lbl_in;
        }

        //Getter und Setter
        public TextBox get_txtField_Text(String text)
        {
            txtBox_txtField_Text.Text = text;
            return txtBox_txtField_Text;
        }

        public Button get_btn_txtField_Apply()
        {
            return btn_txtField_Apply;
        }

        public ComboBox get_cmBox_fontMenu(String font)
        {
            cmBox_fontMenu.SelectedValue = font;
            return cmBox_fontMenu;
        }

        public ComboBox get_cmBox_fontColor(String font)
        {
            cmBox_fontColor.SelectedValue = font;
            return cmBox_fontColor;
        }

        public ComboBox get_cmBox_fontSize(String fontSize)
        {
            cmBox_fontSize.SelectedValue = fontSize;
            return cmBox_fontSize;
        }

        public Label get_lbl_Text()
        {
            return lbl_Text;
        }

        public Label get_lbl_Color()
        {
            return lbl_Color;
        }

        //Event Handler
        private void btn_txtField_Apply_Click(object sender, RoutedEventArgs e)
        {
            generateLabel();
        }
    }
}
