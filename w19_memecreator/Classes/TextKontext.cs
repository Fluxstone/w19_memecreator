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
        ComboBox cmBox_fontFamily = new ComboBox();
        ComboBox cmBox_fontSize = new ComboBox();
        ComboBox cmBox_fontStyle = new ComboBox();
        Button btn_txtField_Apply = new Button();

        ComboBox cmBox_fontColor = new ComboBox();
        Label lbl_Content = new Label();
        Label lbl_Font_Family = new Label();
        Label lbl_Font_Size = new Label();
        Label lbl_Color = new Label();
        SolidColorBrush brush_bright = new SolidColorBrush(Color.FromRgb(130, 130, 130));

        Dictionary<string, SolidColorBrush> dict_lbl_colors = new Dictionary<string, SolidColorBrush>();

        Label lbl_targetLbl;

        //Constructor
        public TextKontext() {}

        //Set KontextWindow Controls
        public void setWindowProperties()
        {
            lbl_Content.Height = 30;
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
            txtBox_txtField_Text.TextChanged += Text_Changed_event;

            lbl_Font_Family.Height = 30;
            lbl_Font_Family.Content = "Font family";
            lbl_Font_Family.Foreground = brush_bright;
            lbl_Font_Family.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Font_Family.VerticalAlignment = VerticalAlignment.Top;
            lbl_Font_Family.Margin = new Thickness(10, 150, 0, 0);


            String[] arr_FontStyles = {"Normal", "Italic", "Bold", "Bold+Italic"};
            cmBox_fontStyle.Height = 25;
            cmBox_fontStyle.Width = 180;
            cmBox_fontStyle.ItemsSource = arr_FontStyles;
            cmBox_fontStyle.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontStyle.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontStyle.Margin = new Thickness(10, 300, 0, 0);

            cmBox_fontFamily.Height = 25;
            cmBox_fontFamily.Width = 180;
            cmBox_fontFamily.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_fontFamily.VerticalAlignment = VerticalAlignment.Top;
            cmBox_fontFamily.Margin = new Thickness(10, 180, 0, 0);

            List<string> fonts = new List<string>();
            using (InstalledFontCollection fontsCollection = new InstalledFontCollection())
            {
                System.Drawing.FontFamily[] fontFamilies = fontsCollection.Families;
                foreach (System.Drawing.FontFamily font in fontFamilies)
                {
                    fonts.Add(font.Name);
                }
            }
            cmBox_fontFamily.ItemsSource = fonts;
            cmBox_fontFamily.SelectedIndex = 0;
            cmBox_fontFamily.SelectionChanged += FontFamily_Changed_event;

            lbl_Font_Size.Height = 30;
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
            //cmBox_fontSize.SelectedIndex = 0;
            cmBox_fontSize.SelectionChanged += FontSize_Changed_event;

            lbl_Color.Height = 30;
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
            cmBox_fontColor.SelectionChanged += FontColor_Changed_event;

            btn_txtField_Apply.Height = 50;
            btn_txtField_Apply.Width = 140;
            btn_txtField_Apply.Content = "Apply Changes";
            btn_txtField_Apply.HorizontalAlignment = HorizontalAlignment.Left;
            btn_txtField_Apply.VerticalAlignment = VerticalAlignment.Top;
            btn_txtField_Apply.Margin = new Thickness(10, 340, 0, 0);
            btn_txtField_Apply.Background = new SolidColorBrush(Color.FromRgb(22, 22, 22));
            btn_txtField_Apply.Foreground = brush_bright;
            btn_txtField_Apply.FontSize = 16;
            btn_txtField_Apply.FontWeight = FontWeights.DemiBold;
            btn_txtField_Apply.BorderBrush = Brushes.Transparent;
            btn_txtField_Apply.AddHandler(Button.ClickEvent, new RoutedEventHandler(btn_txtField_Apply_Click));
        }

        private void Text_Changed_event(object sender, RoutedEventArgs e)
        {
            lbl_targetLbl.Content = txtBox_txtField_Text.Text;
        }

        private void FontFamily_Changed_event(object sender, RoutedEventArgs e)
        {
            lbl_targetLbl.FontFamily = new FontFamily(cmBox_fontFamily.SelectedValue.ToString());
        }

        private void FontColor_Changed_event(object sender, RoutedEventArgs e)
        {
            lbl_targetLbl.Foreground = dict_lbl_colors[cmBox_fontColor.SelectedValue.ToString()];
        }

        private void FontSize_Changed_event(object sender, RoutedEventArgs e)
        {
            if (cmBox_fontSize.SelectedValue != null)
                lbl_targetLbl.FontSize = Int32.Parse(cmBox_fontSize.SelectedValue.ToString());
        }

        private void FontStyle_Changed_event(object sender, RoutedEventArgs e, String fontStyle)
        {
            /*if (fontStyle == "Italic")
            {
                cmBox_fontStyle.FontStyle = FontStyles.Italic;
            } else if(fontStyle == "Bold")
            {
                cmBox_fontStyle.FontWeight = FontWeights.Bold;
            } else if(fontStyle == "Normal")
            {
                cmBox_fontStyle.FontStyle = FontStyles.Normal;
            }*/
            
        }

        public void generateLabel()
        {
            int i_fontSize = Int32.Parse(cmBox_fontSize.Text);
            lbl_targetLbl.FontFamily = new FontFamily(cmBox_fontFamily.SelectedValue.ToString());
            lbl_targetLbl.FontSize = i_fontSize;
            lbl_targetLbl.Content = txtBox_txtField_Text.Text;
            lbl_targetLbl.Foreground = dict_lbl_colors[cmBox_fontColor.SelectedValue.ToString()];


            if (cmBox_fontStyle.SelectedItem.ToString() == "Italic")
            {
                lbl_targetLbl.FontStyle = FontStyles.Normal;
                lbl_targetLbl.FontWeight = FontWeights.Normal;

                lbl_targetLbl.FontStyle = FontStyles.Italic;
            }
            else if (cmBox_fontStyle.SelectedItem.ToString() == "Bold")
            {
                lbl_targetLbl.FontStyle = FontStyles.Normal;
                lbl_targetLbl.FontWeight = FontWeights.Normal;

                lbl_targetLbl.FontWeight = FontWeights.Bold;
            }
            else if (cmBox_fontStyle.SelectedItem.ToString() == "Normal")
            {
                lbl_targetLbl.FontStyle = FontStyles.Normal;
                lbl_targetLbl.FontWeight = FontWeights.Normal;
            }
            else if (cmBox_fontStyle.SelectedItem.ToString() == "Bold+Italic")
            {
                lbl_targetLbl.FontStyle = FontStyles.Normal;
                lbl_targetLbl.FontWeight = FontWeights.Normal;

                lbl_targetLbl.FontStyle = FontStyles.Italic;
                lbl_targetLbl.FontWeight = FontWeights.Bold;
            }

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
            cmBox_fontFamily.SelectedValue = font;
            FontFamily_Changed_event(cmBox_fontFamily, new RoutedEventArgs());
            return cmBox_fontFamily;
        }

        public ComboBox get_cmBox_fontColor(String font)
        {
            cmBox_fontColor.SelectedValue = font;
            FontColor_Changed_event(cmBox_fontColor, new RoutedEventArgs());
            return cmBox_fontColor;
        }

        public ComboBox get_cmBox_fontSize(String fontSize)
        {
            cmBox_fontSize.SelectedValue = fontSize;
            FontSize_Changed_event(cmBox_fontSize, new RoutedEventArgs());
            return cmBox_fontSize;
        }

        public ComboBox get_cmBox_fontStyle(String fontStyle)
        {
            cmBox_fontStyle.SelectedValue = fontStyle;
            FontStyle_Changed_event(cmBox_fontStyle, new RoutedEventArgs(), fontStyle);
            return cmBox_fontStyle;
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
