using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace w19_memecreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            using (StreamReader r = new StreamReader(Environment.CurrentDirectory + "\\..\\..\\Templates\\templates.json"))
            {
                string json = r.ReadToEnd();
                dynamic templates = JsonConvert.DeserializeObject(json);

                foreach (var template in templates)
                {
                    Image myImage = new Image();
                    myImage.Height = 100;
                    myImage.Width = 120;
                    myImage.Cursor = Cursors.Hand;

                    // Create source
                    BitmapImage myBitmapImage = new BitmapImage();

                    // BitmapImage.UriSource must be in a BeginInit/EndInit block
                    myBitmapImage.BeginInit();
                    myBitmapImage.UriSource = new Uri(Environment.CurrentDirectory + "\\..\\..\\Templates\\Previews\\" + template.previewSource, UriKind.Absolute);
                    myBitmapImage.DecodePixelHeight = 100;
                    myBitmapImage.EndInit();
                    //set image source
                    myImage.Source = myBitmapImage;
                    myImage.MouseLeftButtonUp += LoadTemplateToCanvas;
                    myImage.Tag = JsonConvert.SerializeObject(template);
                    stackP_Timeline.Children.Add(myImage);
                }
            }
        }

        private void LoadTemplateToCanvas(object sender, MouseButtonEventArgs e)
        {
            canvas_Bearbeitungsfenster.Children.Clear();

            Image selctedTemplateImage = (Image)sender;
            dynamic templateData = JsonConvert.DeserializeObject(selctedTemplateImage.Tag.ToString());

            double maxSizeY = canvas_Bearbeitungsfenster.Height;
            double maxSizeX = canvas_Bearbeitungsfenster.Height;
            
            double leftPadding = (canvas_Bearbeitungsfenster.Width - canvas_Bearbeitungsfenster.Height) / 2;
            double topPadding = 0;

            if (templateData.formatWidth / templateData.formatHeight < 1)
            {
                maxSizeY = canvas_Bearbeitungsfenster.Height;
                maxSizeX = (double)templateData.formatWidth / (double)templateData.formatHeight * canvas_Bearbeitungsfenster.Height;
                leftPadding = (canvas_Bearbeitungsfenster.Width - maxSizeX) / 2;
            }

            if (templateData.formatWidth / templateData.formatHeight > 1)
            {
                maxSizeX = canvas_Bearbeitungsfenster.Width;
                maxSizeY = (double)templateData.formatHeight / (double)templateData.formatWidth * canvas_Bearbeitungsfenster.Width;
                topPadding = (canvas_Bearbeitungsfenster.Height - maxSizeY) / 2;
                leftPadding = 0;
            }

            foreach (var image in templateData.components.images)
            {
                Image newImage = new Image();
                newImage.Height = (double)image.height/100.0 * maxSizeY;
                newImage.Width = (double)image.width / 100.0 * maxSizeX;
                newImage.Cursor = Cursors.Hand;

                // Create source
                BitmapImage myBitmapImage = new BitmapImage();

                // BitmapImage.UriSource must be in a BeginInit/EndInit block
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(Environment.CurrentDirectory + "\\..\\..\\Templates\\Images\\" + image.relSource, UriKind.Absolute);
                myBitmapImage.DecodePixelHeight = (int)((double)image.height / 100.0 * maxSizeY);
                myBitmapImage.EndInit();
                //set image source
                newImage.Source = myBitmapImage;
                int xPos = (int)((double)(image.xPos)/100.0 * maxSizeX + leftPadding);
                int yPos = (int)((double)(image.yPos)/100.0 * maxSizeY + topPadding);
                Canvas.SetLeft(newImage, xPos);
                Canvas.SetTop(newImage, yPos);
                canvas_Bearbeitungsfenster.Children.Add(newImage);
            }

            foreach (var text in templateData.components.text)
            {
                Label newLabel = new Label();
                newLabel.Cursor = Cursors.Hand;
                newLabel.Content = text.content;
                newLabel.FontFamily = text.font;
                newLabel.FontSize = text.fontsize;
                newLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                newLabel.VerticalContentAlignment = VerticalAlignment.Center;
                newLabel.Height = (double)text.height / 100.0 * maxSizeY;
                newLabel.Width = (double)text.width / 100.0 * maxSizeX;
                int xPos = (int)((double)(text.xPos)/100.0 * maxSizeX + leftPadding);
                int yPos = (int)((double)(text.yPos)/100.0 * maxSizeY + topPadding);
                Canvas.SetLeft(newLabel, xPos);
                Canvas.SetTop(newLabel, yPos);
                canvas_Bearbeitungsfenster.Children.Add(newLabel);
            }
        }
    }
}
