using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace w19_memecreator.Classes
{
    class BildKontext
    {
        //Variables
        WrapPanel wrapP_content = new WrapPanel();
        Image img_targetImg;

        string[] path_memeRes_Files = Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\MemeResources\\PictureContext\\", "*", SearchOption.AllDirectories);
        List<Uri> uri_Container = new List<Uri>();
        List<Image> img_Container = new List<Image>();

        //Constructor
        public BildKontext()
        {
            //Scan PictureContext folder for images and set their properties
            for(int i = 0; i<path_memeRes_Files.Length; i++)
            {
                uri_Container.Add(new Uri(path_memeRes_Files[i], UriKind.Absolute));
                img_Container.Add(new Image());
                img_Container[i].Source = new BitmapImage(uri_Container[i]);
                img_Container[i].Width = 100;
                img_Container[i].Height = 100;
            }
        }

        //Set KontextWindow Controls
        public void setWindowProperties()
        {
            wrapP_content.Width = 344;
            wrapP_content.Height = 450;
            wrapP_content.HorizontalAlignment = HorizontalAlignment.Left;
            wrapP_content.VerticalAlignment = VerticalAlignment.Top;

            for(int i = 0; i<img_Container.Count; i++)
            {
                wrapP_content.Children.Add(img_Container[i]);
                img_Container[i].MouseDown += imgClicked;
            }
        }

        public void generatePicture(Image img_in){
            img_targetImg.Source = null;
            img_targetImg.Source = img_in.Source; 
        }

        //Getter und Setter
        public WrapPanel get_wrapP_content()
        {
            return wrapP_content;
        }
        public void set_img_targetImg(Image img_in)
        {
            img_targetImg = img_in;
        }
        //Event Handler
        private void imgClicked(object sender, MouseButtonEventArgs e)
        {
            generatePicture((Image)sender);
        }
    }
}
