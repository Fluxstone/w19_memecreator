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
using w19_memecreator.Classes;
using Cursors = System.Windows.Input.Cursors;
using Image = System.Windows.Controls.Image;


//Musste Shit auskommentieren damits funktioniert


namespace w19_memecreator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

      
    public partial class MainWindow : Window
    {
        TextKontext textWindow = new TextKontext();
        EffektKontext effectWindow = new EffektKontext();
        BildKontext pictureWindow = new BildKontext();

        double d_canvas_initial_width;
        double d_canvas_initial_height;
        double d_canvas_margin_top;
        double d_canvas_margin_left;
        
        private int i_index_of_canvas_child;
        double[] a_meme_measurements = new double[4];
        bool b_meme_is_selected = false;
        
        public MainWindow()
        {
            InitializeComponent();
            //Der Befehl soll was auf das Bearbeitungsfenster malen
            //canvas_Bearbeitungsfenster.AddHandler(Canvas.MouseLeftButtonDownEvent, new RoutedEventHandler(canvas_Bearbeitungsfenster_MouseLeftButtonDown));

            d_canvas_initial_height = canvas_Bearbeitungsfenster.Height;
            d_canvas_initial_width = canvas_Bearbeitungsfenster.Width;
            d_canvas_margin_top = canvas_Bearbeitungsfenster.Margin.Top;
            d_canvas_margin_left = canvas_Bearbeitungsfenster.Margin.Left;

            textWindow.setWindowProperties();
            pictureWindow.setWindowProperties();
            effectWindow.setWindowProperties();

            // Default-Werte
            a_meme_measurements[0] = d_canvas_margin_left;
            a_meme_measurements[1] = d_canvas_margin_top; // Canvas obere Margin
            a_meme_measurements[2] = canvas_Bearbeitungsfenster.Height; // Breite
            a_meme_measurements[3] = canvas_Bearbeitungsfenster.Height; // Höhe

            // Template-Datei lesen und Thumbnails anzeigen
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
                    
                    BitmapImage myBitmapImage = new BitmapImage();
                    // BitmapImage.UriSource must be in a BeginInit/EndInit block
                    myBitmapImage.BeginInit();
                    myBitmapImage.UriSource = new Uri(Environment.CurrentDirectory + "\\..\\..\\Templates\\Previews\\" + template.previewSource, UriKind.Absolute);
                    myBitmapImage.DecodePixelHeight = 100;
                    myBitmapImage.EndInit();
                    //set image source
                    myImage.Source = myBitmapImage;
                    myImage.MouseLeftButtonUp += LoadTemplateToCanvas; // Handler für Klick auf Thumbnail
                    myImage.Tag = JsonConvert.SerializeObject(template); // Meme-Daten aus Template als Json-Text in Thumbnail-Tag gespeichert zur Weiterverarbeitung
                    stackP_Timeline.Children.Add(myImage);
                }
            }
        }

        // Wird beim Klick auf ein Tumbnail in der Templates-Leiste ausgeführt und stellt das Meme im Canvas dar
        private void LoadTemplateToCanvas(object sender, MouseButtonEventArgs e)
        {
            canvas_Bearbeitungsfenster.Children.Clear();
            canvas_Bearbeitungsfenster.Background = System.Windows.Media.Brushes.White;

            Image selectedTemplateImage = (Image)sender;
            dynamic templateData = JsonConvert.DeserializeObject(selectedTemplateImage.Tag.ToString()); // Meme-Daten aus der Template-Datei

            // Default-Werte für quadratisches Meme
            double d_maxSize_Y = d_canvas_initial_height;
            double d_maxSize_X = d_canvas_initial_height;
            double d_leftPadding = d_canvas_margin_left + (d_canvas_initial_width - d_canvas_initial_height) / 2;
            double d_topPadding = d_canvas_margin_top;
            canvas_Bearbeitungsfenster.Width = d_canvas_initial_height;
            canvas_Bearbeitungsfenster.Height = d_canvas_initial_height;

            // Wenn Format des Memes höher ist als breit
            if (templateData.formatWidth / templateData.formatHeight < 1)
            {
                d_maxSize_Y = d_canvas_initial_height;
                canvas_Bearbeitungsfenster.Width = (double)templateData.formatWidth / (double)templateData.formatHeight * d_canvas_initial_height;
                d_maxSize_X = canvas_Bearbeitungsfenster.Width;
                d_leftPadding = d_canvas_margin_left + (d_canvas_initial_width - d_maxSize_X) / 2;

            }

            // Wenn Format des Memes breiter ist als hoch
            if (templateData.formatWidth / templateData.formatHeight > 1)
            {
                d_maxSize_X = d_canvas_initial_width;
                canvas_Bearbeitungsfenster.Width = d_canvas_initial_width;
                canvas_Bearbeitungsfenster.Height = (double)templateData.formatHeight / (double)templateData.formatWidth * d_canvas_initial_width;
                d_maxSize_Y = canvas_Bearbeitungsfenster.Height;
                d_topPadding = d_canvas_margin_top + (d_canvas_initial_height - d_maxSize_Y) / 2;
                d_leftPadding = d_canvas_margin_left;
            }

            a_meme_measurements[0] = d_leftPadding;
            a_meme_measurements[1] = d_topPadding;
            a_meme_measurements[2] = d_maxSize_X;
            a_meme_measurements[3] = d_maxSize_Y;

            int canvasChildIndex = 0;

            // Bilder in Meme werden Anhand der Daten im Template ins Canvas geladen und platziert
            foreach (var image in templateData.components.images)
            {
                Image newImage = new Image();
                newImage.Height = (double)image.height / 100.0 * d_maxSize_Y;
                newImage.Width = (double)image.width / 100.0 * d_maxSize_X;
                newImage.Cursor = Cursors.Hand;

                // Create source
                BitmapImage myBitmapImage = new BitmapImage();

                // BitmapImage.UriSource must be in a BeginInit/EndInit block
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(Environment.CurrentDirectory + "\\..\\..\\Templates\\Images\\" + image.relSource, UriKind.Absolute);
                myBitmapImage.DecodePixelHeight = (int)((double)image.height / 100.0 * d_maxSize_Y);
                myBitmapImage.DecodePixelHeight = (int)((double)image.width / 100.0 * d_maxSize_X);
                myBitmapImage.EndInit();
                //set image source
                newImage.Source = myBitmapImage;
                newImage.Tag = canvasChildIndex;
                canvasChildIndex++;

                //newImage.MouseLeftButtonUp += Irgendwas;
                int xPos = (int)((double)(image.xPos) / 100.0 * d_maxSize_X);
                int yPos = (int)((double)(image.yPos) / 100.0 * d_maxSize_Y);
                Canvas.SetLeft(newImage, xPos);
                Canvas.SetTop(newImage, yPos);
                canvas_Bearbeitungsfenster.Children.Add(newImage);
            }
            
            // Texte in Meme werden Anhand der Daten im Template als Label ins Canvas geladen und platziert
            foreach (var text in templateData.components.text)
            {
                Label newLabel = new Label();
                newLabel.Cursor = Cursors.Hand;
                newLabel.Content = text.content;
                newLabel.FontFamily = text.font;
                newLabel.FontSize = text.fontsize;
                newLabel.Foreground = System.Windows.Media.Brushes.Black;
                newLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                newLabel.VerticalContentAlignment = VerticalAlignment.Center;
                newLabel.Height = (int)((double)text.height / 100.0 * d_maxSize_Y);
                newLabel.Width = (int)((double)text.width / 100.0 * d_maxSize_X);
                int xPos = (int)((double)(text.xPos) / 100.0 * d_maxSize_X);
                int yPos = (int)((double)(text.yPos) / 100.0 * d_maxSize_Y);
                // LabelInCanvasClicked wird ausgeführt, wenn man auf ein Label im Canvas klickt
                newLabel.MouseLeftButtonUp += LabelInCanvasClicked;
                newLabel.Tag = canvasChildIndex;
                canvasChildIndex++;
                Canvas.SetLeft(newLabel, xPos);
                Canvas.SetTop(newLabel, yPos);
                canvas_Bearbeitungsfenster.Children.Add(newLabel);
            }

            canvas_Bearbeitungsfenster.Margin = new Thickness(d_leftPadding, d_topPadding, 0, 0);
            b_meme_is_selected = true;
        }

        private void LabelInCanvasClicked(object sender, MouseButtonEventArgs e)
        {
            // Das angeklickte Label wird als globale Variable in der Klasse gespeichert, um es später direkt ansprechen und im Kontextfenster verändern zu können
            Label label = (Label)sender;
            
            // Index von Label in Canvas-Kind-Array in globale Variable nummerKind
            i_index_of_canvas_child = (int)label.Tag;

            // Kontextfenster leeren und mit Label-Kontext-Controls füllen
            grid_Kontextfenster.Children.Clear();
            drawTextContext(label);
        }

        //Code Yannic
        public void drawTextContext(Label label)
        {
                textWindow.set_targetLbl((Label)canvas_Bearbeitungsfenster.Children[i_index_of_canvas_child]);
                grid_Kontextfenster.Children.Add(textWindow.get_btn_txtField_Apply());
                grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontMenu(label.FontFamily.ToString()));
                grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontSize(label.FontSize.ToString()));
                grid_Kontextfenster.Children.Add(textWindow.get_txtField_Text(label.Content.ToString()));         
        }

        public void drawEffectContext()
        {
            grid_Kontextfenster.Children.Add(effectWindow.get_btn_effectField_Apply());
            grid_Kontextfenster.Children.Add(effectWindow.get_btn_effectField_Brightness());
        }

        public void drawBildKontext()
        {
            grid_Kontextfenster.Children.Add(pictureWindow.get_wrapP_content());
        }

        //Eventhandler
        //Menuitem
        public void addSprite_Click(object sender, RoutedEventArgs e)
        {

        }

        public void canvas_Bearbeitungsfenster_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            /*Setzt die Mausposi auf den Klickpunkt und malt dann iwas hin (FÜR EFFEKTKONTEXT)
             * effectWindow.set_Cursor(canvas_Bearbeitungsfenster);
            effectWindow.drawSprite(canvas_Bearbeitungsfenster);*/
        }

        // Fertiges Meme als Bild exportieren
        public void saveImage(object sender, RoutedEventArgs e)
        {
            if (b_meme_is_selected)
            {
                // Canvas rendern und Canvas-Maße um Margins beim Rendern erweitert, um Out-of-Bounds-Exception beim Zuschneiden zu verhindern
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas_Bearbeitungsfenster.RenderSize.Width + (int)a_meme_measurements[0],
                (int)canvas_Bearbeitungsfenster.RenderSize.Height + (int)a_meme_measurements[1], 96d, 96d, PixelFormats.Default);
                rtb.Render(canvas_Bearbeitungsfenster);
                // Render-Ergebnis auf Meme zuschneiden
                var crop = new CroppedBitmap(rtb, new Int32Rect((int)a_meme_measurements[0]+2, (int)a_meme_measurements[1]+2, (int)a_meme_measurements[2] - 2, (int)a_meme_measurements[3] - 2));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));

                try
                {
                    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog()
                    {
                        Filter = "Image Files(*.png)|*.png|All(*.*)|*",
                        FileName = "meme.png",
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                    };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            pngEncoder.Save(fileStream);
                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"There was an error saving the file: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Du musst erst ein Meme erstellen, bevor du es speichern kannst!", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
    }
}

          