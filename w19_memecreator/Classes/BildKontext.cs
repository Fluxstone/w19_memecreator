using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace w19_memecreator.Classes
{
    class BildKontext
    {
        //Variables
        WrapPanel wrapP_content = new WrapPanel();


        Uri path_seehofer1 = new Uri(Environment.CurrentDirectory + "\\..\\..\\Resources\\Seehofer1.PNG", UriKind.Absolute);
        Uri path_seehofer2 = new Uri(Environment.CurrentDirectory + "\\..\\..\\Resources\\Seehofer2.PNG", UriKind.Absolute);
        Image img_seehofer1 = new Image();
        Image img_seehofer2 = new Image();
        //Constructor
        public BildKontext()
        {
            img_seehofer1.Source = new BitmapImage(path_seehofer1);
            img_seehofer1.Width = 100;
            img_seehofer1.Height = 100;

            img_seehofer2.Source = new BitmapImage(path_seehofer2);
            img_seehofer2.Width = 100;
            img_seehofer2.Height = 100;
        }

        //Set KontextWindow Controls
        public void setWindowProperties()
        {
            wrapP_content.Width = 344;
            wrapP_content.Height = 450;
            wrapP_content.HorizontalAlignment = HorizontalAlignment.Left;
            wrapP_content.VerticalAlignment = VerticalAlignment.Top;

            wrapP_content.Children.Add(img_seehofer1);
            wrapP_content.Children.Add(img_seehofer2);

            img_seehofer1.AddHandler(Image.MouseDownEvent, new RoutedEventHandler(img_seehofer1_MouseDownEvent));
            img_seehofer2.AddHandler(Image.MouseDownEvent, new RoutedEventHandler(img_seehofer2_MouseDownEvent));

        }

        public void generatePicture(ref Image img_in){
            img_in.Source = null;
            //Selective Process here
        }

        //Getter und Setter
        public WrapPanel get_wrapP_content()
        {
            return wrapP_content;
        }
        //Event Handler
        public void img_seehofer1_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            //generatePicture();
        }

        public void img_seehofer2_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            //generatePicture();
        }

    }
}
