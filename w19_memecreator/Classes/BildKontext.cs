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

        public string Filter { get; private set; }
        public string FileName { get; private set; }
        public string InitialDirectory { get; private set; }

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
                img_Container[i].MouseDown += imgClicked_MouseDownEvent;
            }
            string path_Icon_add = "C:/Users/yanni/Source/Repos/Fluxstone/w19_memecreator/w19_memecreator/MemeResources/OtherResources/add_icon.png";
            //string path_Icon_add = "\\..\\..\\MemeResources\\OtherResources\\add_icon.png"; BUGFIX
            Uri uri_Icon_add = new Uri(path_Icon_add, UriKind.Absolute);
            Image img_Icon_add = new Image();
            img_Icon_add.Source = new BitmapImage(uri_Icon_add);
            img_Icon_add.Width = 100;
            img_Icon_add.Height = 100;
            img_Icon_add.AddHandler(Image.MouseDownEvent, new RoutedEventHandler(Icon_Add_MouseDownEvent));
            wrapP_content.Children.Add(img_Icon_add);
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
        public void imgClicked_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            generatePicture((Image)sender);
        }

        public void Icon_Add_MouseDownEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dialog_openUserFile = new Microsoft.Win32.OpenFileDialog()
                {
                    Filter = "Image Files(*.png)|*.png|All(*.*)|*",
                    FileName = "meme.png",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                };
                if (dialog_openUserFile.ShowDialog() == true)
                {
                    Uri uri_GetFileName = new Uri(dialog_openUserFile.FileName, UriKind.Absolute);
                    Image img_GetFileName = new Image();
                    img_GetFileName.Source = new BitmapImage(uri_GetFileName);
                    generatePicture(img_GetFileName);
                }
            } 
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"There was an error opening the file: {ex.Message}");
            }
        }
    }
}
