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


        const string path_seehofer1 = "C:/Users/yanni/Source/Repos/Fluxstone/w19_memecreator/w19_memecreator/Resources/Seehofer1.PNG";
        const string path_seehofer2 = "C:/Users/yanni/Source/Repos/Fluxstone/w19_memecreator/w19_memecreator/Resources/Seehofer2.PNG";
        Image img_seehofer1 = new Image();
        Image img_seehofer2 = new Image();
        //Construktor
        public BildKontext()
        {
            img_seehofer1.Source = new BitmapImage(new Uri(path_seehofer1));
            img_seehofer2.Source = new BitmapImage(new Uri(path_seehofer2));
        }

        //Set KontextWindow Controlls
        public void setWindowProperties()
        {
            wrapP_content.Background = Brushes.Yellow;
            wrapP_content.Width = 344;
            wrapP_content.Height = 450;
            wrapP_content.HorizontalAlignment = HorizontalAlignment.Left;
            wrapP_content.VerticalAlignment = VerticalAlignment.Top;

            img_seehofer1.Width = 100;
            img_seehofer1.Height = 100;
            img_seehofer2.Width = 100;
            img_seehofer2.Height = 100;

            wrapP_content.Children.Add(img_seehofer1);
            wrapP_content.Children.Add(img_seehofer2);



            img_seehofer1.AddHandler(Image.MouseDownEvent, new RoutedEventHandler(img_seehofer1_MouseDownEvent));
            img_seehofer2.AddHandler(Image.MouseDownEvent, new RoutedEventHandler(img_seehofer2_MouseDownEvent));

        }

        public void generatePicture(ref Image img_in){
            //Do stuff in here
        }

        //Getter und Setter
        public WrapPanel get_wrapP_content()
        {
            return wrapP_content;
        }
        //Event Handler
        public void img_seehofer1_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ok1");
        }

        public void img_seehofer2_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ok2");
        }

    }
}
