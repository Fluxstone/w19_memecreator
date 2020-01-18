using ImageProcessor;
using ImageProcessor.Imaging.Filters.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace w19_memecreator
{
    class EffektKontext
    {
        //Variables
        Button btn_effectField_Preview = new Button();
        
        Slider sld_Brightness = new Slider();
        TextBox txtBox_Brightness = new TextBox();
        Label lbl_Brightness = new Label();

        Slider sld_Quality = new Slider();
        TextBox txtBox_Quality = new TextBox();
        Label lbl_Quality = new Label();

        ComboBox cmBox_Filter = new ComboBox();
        Label lbl_Filter = new Label();
        String[] mat_Filters = {"BlackWhite", "Comic", "Gotham", "GreyScale", "HiSatch", "Invert", "Lomograph", "LoSatch", "Polaroid", "Sepia"};

        ImageFactory imgFac_Main = new ImageFactory();
        

        string pth_TargetFile = Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp\\img_TargetImage.jpeg";
        string pth_BufferFile = Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp\\img_BufferImage.jpeg";

        //Constructor
        public EffektKontext()
        {

        }
        //Functions
        public void setWindowProperties()
        {
            btn_effectField_Preview.Height = 25;
            btn_effectField_Preview.Width = 100;
            btn_effectField_Preview.Content = "Preview Changes";
            btn_effectField_Preview.HorizontalAlignment = HorizontalAlignment.Left;
            btn_effectField_Preview.VerticalAlignment = VerticalAlignment.Top;
            btn_effectField_Preview.Margin = new Thickness(10, 10, 0, 0);
            btn_effectField_Preview.AddHandler(Button.ClickEvent, new RoutedEventHandler(btn_effectField_Preview_Click));

            sld_Brightness.Height = 20;
            sld_Brightness.Width = 200;
            sld_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            sld_Brightness.VerticalAlignment = VerticalAlignment.Top;
            sld_Brightness.Margin = new Thickness(10, 70, 0, 0);
            sld_Brightness.TickFrequency = 1;
            sld_Brightness.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight;
            sld_Brightness.IsSnapToTickEnabled = true;
            sld_Brightness.Maximum = 100;
            sld_Brightness.AddHandler(Slider.ValueChangedEvent, new RoutedEventHandler(sliderValueChanged_event_Brightness));
            sld_Brightness.Value = 0;

            txtBox_Brightness.Height = 20;
            txtBox_Brightness.Width = 60;
            txtBox_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            txtBox_Brightness.VerticalAlignment = VerticalAlignment.Top;
            txtBox_Brightness.Margin = new Thickness(230, 70, 0, 0);
            txtBox_Brightness.TextWrapping = TextWrapping.Wrap;
            txtBox_Brightness.AddHandler(TextBox.TextChangedEvent, new RoutedEventHandler(textBoxValueChanged_event_Brightness));

            lbl_Brightness.Height = 30;
            lbl_Brightness.Width = 80;
            lbl_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Brightness.VerticalAlignment = VerticalAlignment.Top;
            lbl_Brightness.Margin = new Thickness(10, 40, 0, 0);
            lbl_Brightness.Content = "Brightness";
            lbl_Brightness.Foreground = Brushes.White;

            sld_Quality.Height = 20;
            sld_Quality.Width = 200;
            sld_Quality.HorizontalAlignment = HorizontalAlignment.Left;
            sld_Quality.VerticalAlignment = VerticalAlignment.Top;
            sld_Quality.Margin = new Thickness(10, 120, 0, 0);
            sld_Quality.TickFrequency = 1;
            sld_Quality.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight;
            sld_Quality.IsSnapToTickEnabled = true;
            sld_Quality.Maximum = 100;
            sld_Quality.AddHandler(Slider.ValueChangedEvent, new RoutedEventHandler(sliderValueChanged_event_Quality));
            sld_Quality.Value = 0;

            txtBox_Quality.Height = 20;
            txtBox_Quality.Width = 60;
            txtBox_Quality.HorizontalAlignment = HorizontalAlignment.Left;
            txtBox_Quality.VerticalAlignment = VerticalAlignment.Top;
            txtBox_Quality.Margin = new Thickness(230, 120, 0, 0);
            txtBox_Quality.TextWrapping = TextWrapping.Wrap;
            txtBox_Quality.AddHandler(TextBox.TextChangedEvent, new RoutedEventHandler(textBoxValueChanged_event_Quality));

            lbl_Quality.Height = 30;
            lbl_Quality.Width = 80;
            lbl_Quality.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Quality.VerticalAlignment = VerticalAlignment.Top;
            lbl_Quality.Margin = new Thickness(10, 90, 0, 0);
            lbl_Quality.Content = "Quality";
            lbl_Quality.Foreground = Brushes.White;

            cmBox_Filter.Height = 30;
            cmBox_Filter.Width = 150;
            cmBox_Filter.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_Filter.VerticalAlignment = VerticalAlignment.Top;
            cmBox_Filter.Margin = new Thickness(10, 170, 0, 0);
            cmBox_Filter.ItemsSource = mat_Filters;
            cmBox_Filter.SelectedItem = 0;

            lbl_Filter.Height = 30;
            lbl_Filter.Width = 80;
            lbl_Filter.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Filter.VerticalAlignment = VerticalAlignment.Top;
            lbl_Filter.Margin = new Thickness(10, 140, 0, 0);
            lbl_Filter.Content = "Filter";
            lbl_Filter.Foreground = Brushes.White;
        }
        public void generateEffect(Image img_in)
        {
            int imgFac_Brightness = (int)sld_Brightness.Value;
            int imgFac_Quality = (int)sld_Quality.Value;
            //Applying effects
            imgFac_Main.Load(pth_TargetFile);
            imgFac_Main.Brightness(imgFac_Brightness);
            imgFac_Main.Quality(imgFac_Quality);

            if ((string)cmBox_Filter.SelectedItem == "BlackWhite")
            {
                imgFac_Main.Filter(MatrixFilters.BlackWhite);
            }
            else if ((string)cmBox_Filter.SelectedItem == "Comic") 
            {
                imgFac_Main.Filter(MatrixFilters.Comic);
            }
            else if ((string)cmBox_Filter.SelectedItem == "Gotham")
            {
                imgFac_Main.Filter(MatrixFilters.Gotham );
            }
            else if ((string)cmBox_Filter.SelectedItem == "GreyScale")
            {
                imgFac_Main.Filter(MatrixFilters.GreyScale);
            }
            else if ((string)cmBox_Filter.SelectedItem == "HiSatch")
            {
                imgFac_Main.Filter(MatrixFilters.HiSatch);
            }
            else if ((string)cmBox_Filter.SelectedItem == "Invert")
            {
                imgFac_Main.Filter(MatrixFilters.Invert);
            }
            else if ((string)cmBox_Filter.SelectedItem == "Lomograph")
            {
                imgFac_Main.Filter(MatrixFilters.Lomograph);
            }
            else if ((string)cmBox_Filter.SelectedItem == "LoSatch")
            {
                imgFac_Main.Filter(MatrixFilters.LoSatch);
            }
            else if ((string)cmBox_Filter.SelectedItem == "Polaroid")
            {
                imgFac_Main.Filter(MatrixFilters.Polaroid);
            }
            else if ((string)cmBox_Filter.SelectedItem == "Sepia")
            {
                imgFac_Main.Filter(MatrixFilters.Sepia);
            }
            else
            {
                Console.WriteLine("No Filter selected");
            }

            imgFac_Main.Save(pth_BufferFile);
        }
        private int check_txtBoxValidNumber(string str)
        {
            //sld_Brightness.Value = Convert.ToInt32(txtBox_Brightness.Text);
            int i = 0;
            try
            {
                i = Convert.ToInt32(str);
                return i;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Only integers between 0 and 100 are allowed!");
                return 0;
            }
        }
        //Getter und Setter
        public Button get_btn_effectField_Apply()
        {
            return btn_effectField_Preview;
        }
        public Slider get_sld_Brightness()
        {
            return sld_Brightness;
        }
        public TextBox get_txtBox_Brightness()
        {
            return txtBox_Brightness;
        }
        public Label get_lbl_Brightness()
        {
            return lbl_Brightness;
        }
        public ComboBox get_cmBox_Filter()
        {
            return cmBox_Filter;
        }
        public Label get_lbl_Filter()
        {
            return lbl_Filter;
        }
        public Slider get_sld_Quality()
        {
            return sld_Quality;
        }
        public TextBox get_txtBox_Quality()
        {
            return txtBox_Quality;
        }
        public Label get_lbl_Quality()
        {
            return lbl_Quality;
        }
        //Event Handler
        private void btn_effectField_Preview_Click(object sender, RoutedEventArgs e)
        {
            Uri uri_ImgIn = new Uri(pth_TargetFile);
            Image img_in = new Image();
            img_in.Source = new BitmapImage(uri_ImgIn);
            
            generateEffect(img_in);
        }
        //---------------------------------
        private void sliderValueChanged_event_Brightness(object sender, RoutedEventArgs e)
        {
            txtBox_Brightness.Text = sld_Brightness.Value.ToString();
        }
        private void sliderValueChanged_event_Quality(object sender, RoutedEventArgs e)
        {
            txtBox_Quality.Text = sld_Quality.Value.ToString();
        }
        private void textBoxValueChanged_event_Brightness(object sender, RoutedEventArgs e)
        {
            sld_Brightness.Value = check_txtBoxValidNumber(txtBox_Brightness.Text);
        }
        private void textBoxValueChanged_event_Quality(object sender, RoutedEventArgs e)
        {
            sld_Quality.Value = check_txtBoxValidNumber(txtBox_Quality.Text);
        }

    }
}
