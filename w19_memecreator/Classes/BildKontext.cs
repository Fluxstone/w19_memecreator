using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace w19_memecreator.Classes
{
    class BildKontext
    {
        //Variables
        WrapPanel wrapP_content = new WrapPanel();
        Image img_targetImg;
        public double d_canvas_width;
        public double d_canvas_height;
        public double d_template_img_height;
        public double d_template_img_width;
        public double d_parent_grid_height;
        public double d_parent_grid_width;

        //Constructor
        public BildKontext()
        {
            // Versuche Ersatzbilder für Memes zu laden
            // Nichts tun, falls keine gefunden werden, der Benutzer kann ja manuell welche nachladen
            try {
                string[] path_memeRes_Files = Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\MemeResources\\PictureContext\\", "*", SearchOption.AllDirectories);
                //Scan PictureContext folder for images and set their properties
                foreach (string path in path_memeRes_Files)
                {
                    BitmapImage bmp_meme_component_source = new BitmapImage();
                    bmp_meme_component_source.BeginInit();
                    bmp_meme_component_source.UriSource = new Uri(path, UriKind.Absolute);
                    bmp_meme_component_source.EndInit();
                    Image context_image = new Image();
                    context_image.Source = bmp_meme_component_source;
                    context_image.Height = 100;
                    context_image.Tag = path;
                    context_image.MouseLeftButtonUp += imgClicked_MouseUpEvent;
                    context_image.Margin = new Thickness(5, 0, 5, 0);
                    wrapP_content.Children.Add(context_image);
                }
            }
            catch {}
        }

        //Set KontextWindow Controls
        public void setWindowProperties()
        {
            wrapP_content.Width = 344;
            wrapP_content.Height = 450;
            wrapP_content.HorizontalAlignment = HorizontalAlignment.Left;
            wrapP_content.VerticalAlignment = VerticalAlignment.Top;
            
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(130, 130, 130));

            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            rect.Fill = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            rect.Height = 100;
            rect.Width = 100;

            Line line_vert = new Line();
            line_vert.Stroke = brush;
            line_vert.StrokeThickness = 5;
            line_vert.X1 = 50;
            line_vert.X2 = 50;
            line_vert.Y1 = 20;
            line_vert.Y2 = 80;

            Line line_hor = new Line();
            line_hor.Stroke = brush;
            line_hor.StrokeThickness = 5;
            line_hor.X1 = 20;
            line_hor.X2 = 80;
            line_hor.Y1 = 50;
            line_hor.Y2 = 50;

            Ellipse circle = new Ellipse();
            circle.StrokeThickness = 5;
            circle.Stroke = brush;
            circle.Width = 80;
            circle.Height = 80;
            Canvas.SetLeft(circle, 10);
            Canvas.SetTop(circle, 10);

            Canvas canvas_plus = new Canvas();
            canvas_plus.Height = 100;
            canvas_plus.Width = 100;
            canvas_plus.Children.Add(rect);
            canvas_plus.Children.Add(line_vert);
            canvas_plus.Children.Add(line_hor);
            canvas_plus.Children.Add(circle);
            canvas_plus.MouseLeftButtonUp += mouseUp_plus_icon;

            wrapP_content.Children.Add(canvas_plus);
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
        public void imgClicked_MouseUpEvent(object sender, RoutedEventArgs e)
        {
            Image img_in = (Image)sender;
            ReplaceImage(img_in.Tag.ToString());
            generatePicture((Image)sender);
        }

        public void mouseUp_plus_icon(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dialog_openUserFile = new Microsoft.Win32.OpenFileDialog()
                {
                    Filter = "Image Files(*.png)|*.png|All(*.*)|*",
                    //FileName = "meme.png",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                };
                if (dialog_openUserFile.ShowDialog() == true)
                {
                    Uri uri_GetFileName = new Uri(dialog_openUserFile.FileName, UriKind.Absolute);
                    Image img_GetFileName = new Image();
                    img_GetFileName.Source = new BitmapImage(uri_GetFileName);
                    generatePicture(img_GetFileName);
                    ReplaceImage(dialog_openUserFile.FileName);
                }
            } 
            catch
            {
                MessageBox.Show("This image seems to have a bad day.\nTry a different one?", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }
        
        public void ReplaceImage(String source_uri)
        {
            // Create source
            BitmapImage bmp_meme_component_source = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            try
            {
                bmp_meme_component_source.BeginInit();
                bmp_meme_component_source.UriSource = new Uri(source_uri, UriKind.Absolute);
                bmp_meme_component_source.EndInit();
            }
            catch
            {
                throw new ArgumentNullException("The images are introverts and don't want to play right now D:\nTry a different template?");
            }

            // höher als breit wenn > 1, breiter als hoch wenn < 1
            double d_source_aspect_ratio = (double)bmp_meme_component_source.PixelHeight / (double)bmp_meme_component_source.PixelWidth;
            double d_grid_aspect_ratio = d_parent_grid_height / d_parent_grid_width;

            if (d_grid_aspect_ratio < 1) // Grid breiter als hoch
            {
                if (d_source_aspect_ratio < d_grid_aspect_ratio) // Source breiter als Grid
                {
                    img_targetImg.Height = d_template_img_height / 100d * d_canvas_height;
                    img_targetImg.Width = img_targetImg.Height * (2 - d_source_aspect_ratio);
                }
                else //Source höher als Grid
                {
                    img_targetImg.Width = d_template_img_width / 100d * d_canvas_width;
                    img_targetImg.Height = img_targetImg.Width * d_source_aspect_ratio;
                }
            }
            else // Grid höher als breit oder quadratisch
            {
                if (d_source_aspect_ratio < d_grid_aspect_ratio) // Source breiter als Grid
                {
                    img_targetImg.Height = d_template_img_height / 100d * d_canvas_height;
                    img_targetImg.Width = img_targetImg.Height * (2 - d_source_aspect_ratio);
                }
                else // Source höher als Grid
                {
                    img_targetImg.Width = d_template_img_width / 100d * d_canvas_width;
                    img_targetImg.Height = img_targetImg.Width * d_source_aspect_ratio;
                }
            }

            
        }
    }
}
