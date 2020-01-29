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

using System.IO;
using System.Text.RegularExpressions;

namespace w19_memecreator
{
    class EffektKontext
    {
        //Variables
        Button btn_effectField_Preview = new Button();

        Slider sld_Brightness = new Slider();
        TextBox txtBox_Brightness = new TextBox();
        Label lbl_Brightness = new Label();

        Slider sld_Contrast = new Slider();
        TextBox txtBox_Contrast = new TextBox();
        Label lbl_Contrast = new Label();

        ComboBox cmBox_Filter = new ComboBox();
        Label lbl_Filter = new Label();
        String[] mat_Filters = { "BlackWhite", "Comic", "Gotham", "GreyScale", "HiSatch", "Invert", "Lomograph", "LoSatch", "Polaroid", "Sepia" };

        Image target_img;
        Canvas canvas_target;
        ImageFactory imgFac_Main = new ImageFactory();
        SolidColorBrush brush_bright = new SolidColorBrush(Color.FromRgb(130, 130, 130));

        int i_effect_counter = -1;
        int i_buffer_counter = -1;
        





        string pth_TargetFile = Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp\\img_TargetImage";
        string pth_BufferFile = Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp\\img_BufferImage";

        //Constructor
        public EffektKontext()
        {

        }
        //Functions
        public void setWindowProperties()
        {
            lbl_Brightness.Height = 30;
            lbl_Brightness.Width = 80;
            lbl_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Brightness.VerticalAlignment = VerticalAlignment.Top;
            lbl_Brightness.Margin = new Thickness(10, 10, 0, 0);
            lbl_Brightness.Content = "Brightness";
            lbl_Brightness.Foreground = brush_bright;

            sld_Brightness.Height = 20;
            sld_Brightness.Width = 200;
            sld_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            sld_Brightness.VerticalAlignment = VerticalAlignment.Top;
            sld_Brightness.Margin = new Thickness(10, 40, 0, 0);
            sld_Brightness.TickFrequency = 1;
            sld_Brightness.TickPlacement = TickPlacement.BottomRight;
            sld_Brightness.IsSnapToTickEnabled = true;
            sld_Brightness.Maximum = 100;
            sld_Brightness.AddHandler(Slider.ValueChangedEvent, new RoutedEventHandler(sliderValueChanged_event_Brightness));
            sld_Brightness.Value = 0;

            txtBox_Brightness.Height = 20;
            txtBox_Brightness.Width = 60;
            txtBox_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            txtBox_Brightness.VerticalAlignment = VerticalAlignment.Top;
            txtBox_Brightness.Margin = new Thickness(230, 40, 0, 0);
            txtBox_Brightness.TextWrapping = TextWrapping.Wrap;
            // TODO:
            txtBox_Brightness.IsEnabled = false;
            txtBox_Brightness.AddHandler(TextBox.TextChangedEvent, new RoutedEventHandler(textBoxValueChanged_event_Brightness));
            
            lbl_Contrast.Height = 30;
            lbl_Contrast.Width = 80;
            lbl_Contrast.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Contrast.VerticalAlignment = VerticalAlignment.Top;
            lbl_Contrast.Margin = new Thickness(10, 70, 0, 0);
            lbl_Contrast.Content = "Contrast";
            lbl_Contrast.Foreground = brush_bright;

            sld_Contrast.Height = 20;
            sld_Contrast.Width = 200;
            sld_Contrast.HorizontalAlignment = HorizontalAlignment.Left;
            sld_Contrast.VerticalAlignment = VerticalAlignment.Top;
            sld_Contrast.Margin = new Thickness(10, 100, 0, 0);
            sld_Contrast.TickFrequency = 1;
            sld_Contrast.TickPlacement = TickPlacement.BottomRight;
            sld_Contrast.IsSnapToTickEnabled = true;
            sld_Contrast.Maximum = 100;
            sld_Contrast.AddHandler(Slider.ValueChangedEvent, new RoutedEventHandler(sliderValueChanged_event_Quality));
            sld_Contrast.Value = 0;

            txtBox_Contrast.Height = 20;
            txtBox_Contrast.Width = 60;
            txtBox_Contrast.HorizontalAlignment = HorizontalAlignment.Left;
            txtBox_Contrast.VerticalAlignment = VerticalAlignment.Top;
            txtBox_Contrast.Margin = new Thickness(230, 100, 0, 0);
            txtBox_Contrast.TextWrapping = TextWrapping.Wrap;
            // TODO:
            txtBox_Contrast.IsEnabled = false;
            txtBox_Contrast.AddHandler(TextBox.TextChangedEvent, new RoutedEventHandler(textBoxValueChanged_event_Quality));

            lbl_Filter.Height = 30;
            lbl_Filter.Width = 150;
            lbl_Filter.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Filter.VerticalAlignment = VerticalAlignment.Top;
            lbl_Filter.Margin = new Thickness(10, 130, 0, 0);
            lbl_Filter.Content = "Additional filter";
            lbl_Filter.Foreground = brush_bright;

            cmBox_Filter.Height = 30;
            cmBox_Filter.Width = 150;
            cmBox_Filter.HorizontalAlignment = HorizontalAlignment.Left;
            cmBox_Filter.VerticalAlignment = VerticalAlignment.Top;
            cmBox_Filter.Margin = new Thickness(10, 160, 0, 0);
            cmBox_Filter.ItemsSource = mat_Filters;
            cmBox_Filter.SelectedItem = 1;

            btn_effectField_Preview.Height = 50;
            btn_effectField_Preview.Width = 140;
            btn_effectField_Preview.Content = "Apply Changes";
            btn_effectField_Preview.HorizontalAlignment = HorizontalAlignment.Left;
            btn_effectField_Preview.VerticalAlignment = VerticalAlignment.Top;
            btn_effectField_Preview.Margin = new Thickness(10, 200, 0, 0);
            btn_effectField_Preview.Background = new SolidColorBrush(Color.FromRgb(22, 22, 22));
            btn_effectField_Preview.Foreground = brush_bright;
            btn_effectField_Preview.FontSize = 16;
            btn_effectField_Preview.BorderBrush = Brushes.Transparent;
            btn_effectField_Preview.AddHandler(Button.ClickEvent, new RoutedEventHandler(btn_effectField_Preview_Click));
        }

        public void generateEffect(Image img_in)
        {
            i_buffer_counter++;
            byte[] phBytes = File.ReadAllBytes(pth_TargetFile + i_effect_counter + ".jpg");

            int imgFac_Brightness = (int)sld_Brightness.Value;
            int imgFac_Contrast = (int)sld_Contrast.Value;

            //Open the File and apply filters
            using (MemoryStream inStream = new MemoryStream(phBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory())
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream)
                                    .Brightness(imgFac_Brightness)
                                    .Contrast(imgFac_Contrast);

                        if ((string)cmBox_Filter.SelectedItem == "BlackWhite")
                        {
                            imageFactory.Filter(MatrixFilters.BlackWhite);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "Comic")
                        {
                            imageFactory.Filter(MatrixFilters.Comic);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "Gotham")
                        {
                            imageFactory.Filter(MatrixFilters.Gotham);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "GreyScale")
                        {
                            imageFactory.Filter(MatrixFilters.GreyScale);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "HiSatch")
                        {
                            imageFactory.Filter(MatrixFilters.HiSatch);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "Invert")
                        {
                            imageFactory.Filter(MatrixFilters.Invert);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "Lomograph")
                        {
                            imageFactory.Filter(MatrixFilters.Lomograph);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "LoSatch")
                        {
                            imageFactory.Filter(MatrixFilters.LoSatch);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "Polaroid")
                        {
                            imageFactory.Filter(MatrixFilters.Polaroid);
                        }
                        else if ((string)cmBox_Filter.SelectedItem == "Sepia")
                        {
                            imageFactory.Filter(MatrixFilters.Sepia);
                        }
                        else
                        {
                            Console.WriteLine("No Filter selected");
                        }

                        imageFactory.Save(outStream);
                        imageFactory.Dispose();
                    }
                    // Do something with the stream.
                    System.Drawing.Bitmap btm = new System.Drawing.Bitmap(outStream);
                    btm.Save(pth_BufferFile + i_buffer_counter + ".jpg");
                    btm.Dispose();
                    outStream.Close();
                }
                inStream.Close();
            }
            

            updateCanvasPicture(img_in);

        }

        private void updateCanvasPicture(Image img_in)
        {
            img_in.Source = null;
            img_in.Source = new BitmapImage(new Uri(pth_BufferFile + i_buffer_counter + ".jpg"));
            canvas_target.Children.Add(img_in);  //Provisorischer "Preview"
        }

        private int check_txtBoxValidNumber(string str)
        {            
            int i = 0;

            try
            {
                if (IsTextAllowed(str) == false)
                {
                    MessageBox.Show("++++Only numbers betweeen 0 and 100 are allowed!");
                    str = "0";
                    i = 0;
                    return i;
                } else
                {
                    i = Convert.ToInt32(str);
                    if (i < 0 || i > 100)
                    {
                        MessageBox.Show("++++Only numbers betweeen 0 and 100 are allowed!++++");
                        i = 0;
                    }
                    return i;
                }
                
                
                i = Convert.ToInt32(str);
                if (i < 0 || i > 100)
                {
                    MessageBox.Show("++++Only numbers betweeen 0 and 100 are allowed!++++");
                    return 0;
                }
                return i;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Only integers between 0 and 100 are allowed!");
                return 0;
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
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
        public Slider get_sld_Contrast()
        {
            return sld_Contrast;
        }
        public TextBox get_txtBox_Contrast()
        {
            return txtBox_Contrast;
        }
        public Label get_lbl_Contrast()
        {
            return lbl_Contrast;
        }
        public void set_target_img(Image img_in)
        {
            target_img = img_in;
        }
        public void set_canvas_target(Canvas canvas_in)
        {
            canvas_target = canvas_in;
            i_effect_counter++;
        }
        public void set_rendered_canvas(byte[] image)
        {

        }
        //Event Handler
        private void btn_effectField_Preview_Click(object sender, RoutedEventArgs e)
        {
            Uri uri_ImgIn = new Uri(pth_TargetFile + i_effect_counter + ".jpg");
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
            txtBox_Contrast.Text = sld_Contrast.Value.ToString();
        }
        private void textBoxValueChanged_event_Brightness(object sender, RoutedEventArgs e)
        {    
                sld_Brightness.Value = check_txtBoxValidNumber(txtBox_Brightness.Text);
        }
        private void textBoxValueChanged_event_Quality(object sender, RoutedEventArgs e)
        {
            sld_Contrast.Value = check_txtBoxValidNumber(txtBox_Contrast.Text);
        }
    }
}
