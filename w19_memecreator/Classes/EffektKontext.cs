using ImageProcessor;
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
        Canvas canvas_TargetCanvas;
        
        Slider sld_Brightness = new Slider();
        TextBox txtBox_Brightness = new TextBox();
        Label lbl_Brightness = new Label();

        ImageFactory imgFac_Main = new ImageFactory();
        string pth_TargetFile = Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp\\img_TargetImage.jpeg";
        string pth_BufferFile = Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp\\img_BufferImage.jpeg";

        //Constructor
        public EffektKontext()
        {

        }

        //Functions
        public void setWindowProperties(ref Canvas canvas_in)
        {
            canvas_TargetCanvas = canvas_in;

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
            sld_Brightness.ValueChanged += sliderValueChanged_event;

            txtBox_Brightness.Height = 20;
            txtBox_Brightness.Width = 60;
            txtBox_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            txtBox_Brightness.VerticalAlignment = VerticalAlignment.Top;
            txtBox_Brightness.Margin = new Thickness(230, 70, 0, 0);
            txtBox_Brightness.TextWrapping = TextWrapping.Wrap;
            txtBox_Brightness.TextChanged += textBoxValueChanged_event;

            lbl_Brightness.Height = 30;
            lbl_Brightness.Width = 80;
            lbl_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            lbl_Brightness.VerticalAlignment = VerticalAlignment.Top;
            lbl_Brightness.Margin = new Thickness(10, 40, 0, 0);
            lbl_Brightness.Content = "Brightness";
            lbl_Brightness.Foreground = Brushes.White;
        }

        public void generateEffect(Image img_in)
        {
            int imgFac_Brightness = (int)sld_Brightness.Value;

            //Applying effects
            imgFac_Main.Load(pth_TargetFile);
            imgFac_Main.Brightness(imgFac_Brightness);
            imgFac_Main.Save(pth_BufferFile); //2x Eingeben und GDI stirbt

            //Loading modified Picture on canvas
            /*BitmapImage bpm_BufferImage = new BitmapImage(new Uri(pth_BufferFile));
            img_in.Source = bpm_BufferImage; //Muss noch auf MainCanvas*/
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
        //Event Handler
        private void sliderValueChanged_event(object sender, RoutedEventArgs e)
        {
            txtBox_Brightness.Text = sld_Brightness.Value.ToString();
        }

        private void btn_effectField_Preview_Click(object sender, RoutedEventArgs e)
        {
            Uri uri_ImgIn = new Uri(pth_TargetFile);
            Image img_in = new Image();
            img_in.Source = new BitmapImage(uri_ImgIn);
            
            generateEffect(img_in);
        }

        private void textBoxValueChanged_event(object sender, RoutedEventArgs e)
        {
            try
            {
                sld_Brightness.Value = Convert.ToInt32(txtBox_Brightness.Text);
            } catch (Exception ex)
            {
                if(txtBox_Brightness.Text != "") 
                {
                    MessageBox.Show("Only integers between 0 and 100 are allowed!");
                    sld_Brightness.Value = 0;
                }
            }
        }
    }
}
