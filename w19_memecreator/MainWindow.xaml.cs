using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Button = System.Windows.Controls.Button;
using Image = System.Windows.Controls.Image;

//Musste Shit auskommentieren damits funktioniert


namespace w19_memecreator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

      
    public partial class MainWindow : Window
    {
        private int nummerKind;
        TextKontext textWindow = new TextKontext();
        EffektKontext effectWindow = new EffektKontext();




        public MainWindow()
        {
            InitializeComponent();
            canvas_Bearbeitungsfenster.AddHandler(Canvas.MouseLeftButtonDownEvent, new RoutedEventHandler(canvas_Bearbeitungsfenster_MouseLeftButtonDown));

            textWindow.setWindowProperties();
            


            using (StreamReader r = new StreamReader(Environment.CurrentDirectory + "\\..\\..\\Templates\\templates.json"))
            {
                string json = r.ReadToEnd();
                dynamic templates = JsonConvert.DeserializeObject(json);

                foreach (var template in templates)
                {
                    Image myImage = new Image();
                    myImage.Height = 100;
                    myImage.Width = 120;
                    myImage.Cursor = System.Windows.Input.Cursors.Hand;

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

            int canvasChildIndex = 0;

            foreach (var image in templateData.components.images)
            {
                Image newImage = new Image();
                newImage.Height = (double)image.height / 100.0 * maxSizeY;
                newImage.Width = (double)image.width / 100.0 * maxSizeX;
                newImage.Cursor = System.Windows.Input.Cursors.Hand;

                // Create source
                BitmapImage myBitmapImage = new BitmapImage();

                // BitmapImage.UriSource must be in a BeginInit/EndInit block
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(Environment.CurrentDirectory + "\\..\\..\\Templates\\Images\\" + image.relSource, UriKind.Absolute);
                myBitmapImage.DecodePixelHeight = (int)((double)image.height / 100.0 * maxSizeY);
                myBitmapImage.EndInit();
                //set image source
                newImage.Source = myBitmapImage;
                newImage.Tag = canvasChildIndex;
                canvasChildIndex++;

                //newImage.MouseLeftButtonUp += Irgendwas;
                int xPos = (int)((double)(image.xPos) / 100.0 * maxSizeX + leftPadding);
                int yPos = (int)((double)(image.yPos) / 100.0 * maxSizeY + topPadding);
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
                newLabel.Height = (int)((double)text.height / 100.0 * maxSizeY);
                newLabel.Width = (int)((double)text.width / 100.0 * maxSizeX);
                newLabel.Background = System.Windows.Media.Brushes.Red;
                int xPos = (int)((double)(text.xPos) / 100.0 * maxSizeX + leftPadding);
                int yPos = (int)((double)(text.yPos) / 100.0 * maxSizeY + topPadding);
                // LabelInCanvasClicked wird ausgeführt, wenn man auf ein Label im Canvas klickt
                newLabel.MouseLeftButtonUp += LabelInCanvasClicked;
                newLabel.Tag = canvasChildIndex;
                canvasChildIndex++;
                Canvas.SetLeft(newLabel, xPos);
                Canvas.SetTop(newLabel, yPos);
                canvas_Bearbeitungsfenster.Children.Add(newLabel);
            }
        }

        private void LabelInCanvasClicked(object sender, MouseButtonEventArgs e)
        {
            // Das angeklickte Label wird als globale Variable in der Klasse gespeichert, um es später direkt ansprechen und im Kontextfenster verändern zu können
            Label label = (Label)sender;
            //newButton.Click += new RoutedEventHandler(LabelNummerLaden);
            
            // Index von Label in Canvas-Kind-Array in globale Variable nummerKind
            nummerKind = (int)label.Tag;
            drawTextContext();
        }

        private void LabelNummerLaden(object sender, RoutedEventArgs e)
        {
            int rando = 6;
            Label label = (Label)canvas_Bearbeitungsfenster.Children[nummerKind];
            label.Content = "Blabubb";
        }





        //Code Yannic
        public void drawTextContext()
        {
            // TODO: if drawn == true
            textWindow.set_targetLbl((Label)canvas_Bearbeitungsfenster.Children[nummerKind]);
            grid_Kontextfenster.Children.Add(textWindow.get_btn_txtField_Apply());
            grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontMenu());
            grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontSize());
            grid_Kontextfenster.Children.Add(textWindow.get_txtField_Text());
        }

        public void drawEffectContext()
        {
            grid_Kontextfenster.Children.Add(effectWindow.get_btn_effectField_Apply());
            grid_Kontextfenster.Children.Add(effectWindow.get_btn_effectField_Brightness());
        }

        //TODO: drawBildKontext

        //Eventhandler

        //Menuitem
        public void addSprite_Click(object sender, RoutedEventArgs e)
        {

        }

        public void canvas_Bearbeitungsfenster_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            /*effectWindow.set_Cursor(canvas_Bearbeitungsfenster);
            effectWindow.drawSprite(canvas_Bearbeitungsfenster);*/
        }


    }
}

          