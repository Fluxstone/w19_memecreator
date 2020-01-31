using ImageProcessor;
using ImageProcessor.Imaging.Filters.Photo;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.IO;


namespace w19_memecreator
{
    class EffektKontext
    {
        //Variables
        Slider sld_Brightness = new Slider();
        Label lbl_Brightness_value = new Label();
        Label lbl_Brightness = new Label();

        Slider sld_Contrast = new Slider();
        Label lbl_Contrast_value = new Label();
        Label lbl_Contrast = new Label();

        ComboBox cmBox_Filter = new ComboBox();
        Label lbl_Filter = new Label();
        string[] mat_Filters = { "No filter", "BlackWhite", "Comic", "Gotham", "GreyScale", "HiSatch", "Invert", "Lomograph", "LoSatch", "Polaroid", "Sepia" };
        
        Canvas canvas_target;
        SolidColorBrush brush_bright = new SolidColorBrush(Color.FromRgb(130, 130, 130));
        
        string pth_TargetFile = Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp\\img_TargetImage.jpg";

        //Constructor
        public EffektKontext() {}

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
            sld_Brightness.Minimum = -100;
            sld_Brightness.Maximum = 100;
            sld_Brightness.AddHandler(Slider.ValueChangedEvent, new RoutedEventHandler(sliderValueChanged_event_Brightness));
            sld_Brightness.Value = 0;

            lbl_Brightness_value.Height = 30;
            lbl_Brightness_value.Width = 60;
            lbl_Brightness_value.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Brightness_value.VerticalAlignment = VerticalAlignment.Top;
            lbl_Brightness_value.Margin = new Thickness(220, 35, 0, 0);
            lbl_Brightness_value.Foreground = brush_bright;
            
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
            sld_Contrast.Minimum = -100;
            sld_Contrast.Maximum = 100;
            sld_Contrast.AddHandler(Slider.ValueChangedEvent, new RoutedEventHandler(sliderValueChanged_event_Quality));
            sld_Contrast.Value = 0;

            lbl_Contrast_value.Height = 30;
            lbl_Contrast_value.Width = 60;
            lbl_Contrast_value.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Contrast_value.VerticalAlignment = VerticalAlignment.Top;
            lbl_Contrast_value.Margin = new Thickness(220, 95, 0, 0);
            lbl_Contrast_value.Foreground = brush_bright;

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
            cmBox_Filter.SelectedIndex = 0;
            cmBox_Filter.SelectionChanged += cmBox_Filter_SelectionChanged_event;
        }

        public void generateEffect()
        {
            byte[] phBytes = File.ReadAllBytes(pth_TargetFile);

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

                        imageFactory.Save(outStream);
                        imageFactory.Dispose();
                    }
                    PngBitmapDecoder decoder = new PngBitmapDecoder(outStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    BitmapSource bitmapSource = decoder.Frames[0];
                    Image img_in = new Image();
                    img_in.Source = bitmapSource;
                    canvas_target.Children.Clear();
                    canvas_target.Children.Add(img_in);
                    outStream.Close();
                }
                inStream.Close();
            }
        }

        //Getter und Setter
        public Slider get_sld_Brightness()
        {
            return sld_Brightness;
        }
        public Label get_lbl_Brightness_value()
        {
            return lbl_Brightness_value;
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
        public Label get_lbl_Contrast_value()
        {
            return lbl_Contrast_value;
        }
        public Label get_lbl_Contrast()
        {
            return lbl_Contrast;
        }
        public void set_canvas_target(Canvas canvas_in)
        {
            canvas_target = canvas_in;
        }
        public void set_rendered_canvas(byte[] image)
        {

        }

        //Event Handler

        private void sliderValueChanged_event_Brightness(object sender, RoutedEventArgs e)
        {
            generateEffect();
            if (sld_Brightness.Value > 0)
                lbl_Brightness_value.Content = "+" + sld_Brightness.Value.ToString() + "%";
            else
                lbl_Brightness_value.Content = sld_Brightness.Value.ToString() + "%";
        }

        private void sliderValueChanged_event_Quality(object sender, RoutedEventArgs e)
        {
            generateEffect();
            if (sld_Contrast.Value > 0)
                lbl_Contrast_value.Content = "+" + sld_Contrast.Value.ToString() + "%";
            else
                lbl_Contrast_value.Content = sld_Contrast.Value.ToString() + "%";
        }

        private void cmBox_Filter_SelectionChanged_event(object sender, RoutedEventArgs e)
        {
            generateEffect();
        }
    }
}
