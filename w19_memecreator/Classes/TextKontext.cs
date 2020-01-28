using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace w19_memecreator
{
    class TextKontext
    {
        //Variables
        TextBox txtBox_txtField_Text = new TextBox();
        ComboBox cmBox_fontMenu = new ComboBox();
        ComboBox cmBox_fontSize = new ComboBox();
        Button btn_txtField_Apply = new Button();

        ComboBox cmBox_fontColor = new ComboBox();
        Label lbl_Content = new Label();
        Label lbl_Font_Family = new Label();
        Label lbl_Font_Size = new Label();
        Label lbl_Color = new Label();
        SolidColorBrush brush_bright = new SolidColorBrush(Color.FromRgb(130, 130, 130));

        Dictionary<String, SolidColorBrush> dict_lbl_colors = new Dictionary<String, SolidColorBrush>();

        Label lbl_targetLbl;

        //Constructor
        public TextKontext()
        {

        }

        //Set KontextWindow Controls
        public void setWindowProperties()
        {
            lbl_Content.Height = 30;
            //lbl_Content.Width = 120;
            lbl_Content.Content = "Content";
            lbl_Content.Foreground = brush_bright;
            lbl_Content.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Content.VerticalAlignment = VerticalAlignment.Top;
            lbl_Content.Margin = new Thickness(10, 10, 0, 0);

            txtBox_txtField_Text.Height = 100;
            txtBox_txtField_Text.Width = 300;
            txtBox_txtField_Text.Text = "";
            txtBox_txtField_Text.HorizontalAlignment = HorizontalAlignment.Left;
            txtBox_txtField_Text.VerticalAlignment = VerticalAlignment.Top;
            txtBox_txtField_Text.AcceptsReturn = true;
            txtBox_txtField_Text.Margin = new Thickness(10, 40, 0, 0);

            lbl_Font_Family.Height = 30;
            //lbl_Font_Family.Width = 120;
            lbl_Font_Family.Content = "Font family";
            lbl_Font_Family.Foreground = brush_bright;
            lbl_Font_Family.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Font_Family.VerticalAlignment = VerticalAlignment.Top;
            lbl_Font_Family.Margin = new Thickness(10, 150, 0, 0);

            cmBox_fontMenu.Height = 25;
            cmBox_fontMenu.Width = 180;
            cmBox_fontMenu.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontMenu.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontMenu.Margin = new Thickness(10, 180, 0, 0);

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
            
            lbl_Font_Size.Height = 30;
            //lbl_Font_Size.Width = 120;
            lbl_Font_Size.Content = "Font size";
            lbl_Font_Size.Foreground = brush_bright;
            lbl_Font_Size.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Font_Size.VerticalAlignment = VerticalAlignment.Top;
            lbl_Font_Size.Margin = new Thickness(220, 150, 0, 0);

            cmBox_fontSize.Height = 25;
            cmBox_fontSize.Width = 60;
            cmBox_fontSize.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontSize.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontSize.Margin = new Thickness(220, 180, 0, 0);
            List<string> fontSizeList = new List<string>();
            for (int i = 4; i < 70; i++)
            {
                fontSizeList.Add(i.ToString());
            }
            cmBox_fontSize.ItemsSource = fontSizeList;
            cmBox_fontSize.SelectedIndex = 0;

            lbl_Color.Height = 30;
            //lbl_Color.Width = 120;
            lbl_Color.Content = "Color options";
            lbl_Color.Foreground = brush_bright;
            lbl_Color.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Color.VerticalAlignment = VerticalAlignment.Top;
            lbl_Color.Margin = new Thickness(10, 220, 0, 0);
            
            cmBox_fontColor.Height = 25;
            cmBox_fontColor.Width = 120;
            cmBox_fontColor.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontColor.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontColor.Margin = new Thickness(10, 250, 0, 0);
            List<string> lst_colors = new List<string>();
            Type type_brushes = typeof(Brushes);
            var properties = type_brushes.GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var prop in properties)
            {
                dict_lbl_colors[prop.Name] = (SolidColorBrush)prop.GetValue(null, null);
            }
            cmBox_fontColor.ItemsSource = dict_lbl_colors.Keys;
            cmBox_fontColor.SelectedIndex = 0;

            btn_txtField_Apply.Height = 50;
            btn_txtField_Apply.Width = 140;
            btn_txtField_Apply.Content = "Apply Changes";
            btn_txtField_Apply.HorizontalAlignment = HorizontalAlignment.Left;
            btn_txtField_Apply.VerticalAlignment = VerticalAlignment.Top;
            btn_txtField_Apply.Margin = new Thickness(10, 290, 0, 0);
            btn_txtField_Apply.Background = new SolidColorBrush(Color.FromRgb(22, 22, 22));
            btn_txtField_Apply.Foreground = brush_bright;
            btn_txtField_Apply.FontSize = 16;
            btn_txtField_Apply.FontWeight = FontWeights.DemiBold;
            btn_txtField_Apply.BorderBrush = Brushes.Transparent;
            btn_txtField_Apply.AddHandler(Button.ClickEvent, new RoutedEventHandler(btn_txtField_Apply_Click));
        }

        public void generateLabel()
        {
            int i_fontSize = Int32.Parse(cmBox_fontSize.Text);
            lbl_targetLbl.FontFamily = new FontFamily(cmBox_fontMenu.SelectedValue.ToString());
            lbl_targetLbl.FontSize = i_fontSize;
            lbl_targetLbl.Content = txtBox_txtField_Text.Text;
            lbl_targetLbl.Foreground = dict_lbl_colors[cmBox_fontColor.SelectedValue.ToString()];
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

        public Label get_lbl_Content()
        {
            return lbl_Content;
        }

        public Label get_lbl_Font_Family()
        {
            return lbl_Font_Family;
        }

        public Label get_lbl_Font_Size()
        {
            return lbl_Font_Size;
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
