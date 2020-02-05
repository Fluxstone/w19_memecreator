using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using w19_memecreator.Classes;
using Cursors = System.Windows.Input.Cursors;
using Image = System.Windows.Controls.Image;

namespace w19_memecreator {
    
    public partial class MainWindow : Window
    {
        TextKontext textWindow = new TextKontext();
        EffektKontext effectWindow = new EffektKontext();
        BildKontext pictureWindow = new BildKontext();

        double d_canvas_initial_width;
        double d_canvas_initial_height;
        double d_canvas_margin_top;
        double d_canvas_margin_left;
        
        double[] a_meme_measurements = new double[4];
        bool bool_meme_is_selected = false;
        bool bool_filter_is_added = false;

        List<Grid> li_canvas_child_grid = new List<Grid>();
        List<Label> li_canvas_child_label = new List<Label>();
        int i_border_index_in_canvas = -1;
        int i_effect_counter = -1;


        Dictionary<string, SolidColorBrush> dict_brushes_str_scb = new Dictionary<string, SolidColorBrush>();
        Dictionary<SolidColorBrush, string> dict_brushes_scb_str = new Dictionary<SolidColorBrush, string>();

        public MainWindow()
        {
            InitializeComponent();

            // Ausgangsmaße des Canvas zur späteren Darstellung der Memes speichern
            d_canvas_initial_height = canvas_Bearbeitungsfenster.Height;
            d_canvas_initial_width = canvas_Bearbeitungsfenster.Width;
            d_canvas_margin_top = canvas_Bearbeitungsfenster.Margin.Top;
            d_canvas_margin_left = canvas_Bearbeitungsfenster.Margin.Left;

            // Kontext-Klassen konfigurieren
            textWindow.setWindowProperties();
            pictureWindow.setWindowProperties();
            effectWindow.setWindowProperties();
            
            try
            {
                // temp-Order für Filter-Erstellung leeren oder erstellen, falls er fehlt
                string s_temp_path = Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp";
                DirectoryInfo di = new DirectoryInfo(s_temp_path);
                if (di.Exists)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                else
                {
                    Directory.CreateDirectory(s_temp_path);
                }
            
                // Dictionaries mit allen SolidColorBrushes und deren Namen für die Schriftfarben der Labels im Meme
                Type type_brushes = typeof(Brushes);
                var properties = type_brushes.GetProperties(BindingFlags.Static | BindingFlags.Public);
                foreach (var prop in properties)
                {
                    dict_brushes_str_scb[prop.Name] = (SolidColorBrush)prop.GetValue(null, null);
                    dict_brushes_scb_str[(SolidColorBrush)prop.GetValue(null, null)] = prop.Name;
                }
            
                // Default-Werte
                a_meme_measurements[0] = d_canvas_margin_left; // Canvas linke Margin
                a_meme_measurements[1] = d_canvas_margin_top; // Canvas obere Margin
                a_meme_measurements[2] = canvas_Bearbeitungsfenster.Height; // Breite
                a_meme_measurements[3] = canvas_Bearbeitungsfenster.Height; // Höhe

                // Template-Datei lesen und Thumbnails anzeigen
                using (StreamReader r = new StreamReader(Environment.CurrentDirectory + "\\..\\..\\Templates\\templates.json"))
                {
                    string json = r.ReadToEnd();
                    dynamic json_templates = JsonConvert.DeserializeObject(json);

                    foreach (var template in json_templates)
                    {
                        Image img_template_thumbnail = new Image();
                        img_template_thumbnail.Height = 100;
                        img_template_thumbnail.Margin = new Thickness(0, 0, 10, 0);
                        img_template_thumbnail.Cursor = Cursors.Hand;

                        BitmapImage bmp_thumbnail_source = new BitmapImage();
                        bmp_thumbnail_source.BeginInit();
                        bmp_thumbnail_source.UriSource = new Uri(Environment.CurrentDirectory + "\\..\\..\\Templates\\Previews\\" + template.previewSource, UriKind.Absolute);
                        bmp_thumbnail_source.DecodePixelHeight = 100;
                        bmp_thumbnail_source.EndInit();
                        
                        img_template_thumbnail.Source = bmp_thumbnail_source;
                        img_template_thumbnail.MouseLeftButtonUp += LoadTemplateToCanvas; // Handler für Klick auf Thumbnail
                        img_template_thumbnail.Tag = JsonConvert.SerializeObject(template); // Meme-Daten aus Template als Json-Text in Thumbnail-Tag gespeichert zur Weiterverarbeitung
                        stackP_Timeline.Children.Add(img_template_thumbnail);
                    }
                }
            } catch(FileNotFoundException e)
            {
                MessageBox.Show("We couldn't find the templates, oh...", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Forms.Application.Exit();
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("We couldn't find the templates, oh...", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Forms.Application.Exit();
            }
            catch (IOException e)
            {
                MessageBox.Show("The templates are introverts and don't want to play right now D:", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Forms.Application.Exit();
            }
            catch
            {
                MessageBox.Show("Things went wrong, but we don't know why...", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Forms.Application.Exit();
            }
            
        }

        // Wird beim Klick auf ein Tumbnail in der Templates-Leiste ausgeführt und stellt das Meme im Canvas dar
        private void LoadTemplateToCanvas(object sender, MouseButtonEventArgs e)
        {
            canvas_Bearbeitungsfenster.Children.Clear();
            grid_Kontextfenster.Children.Clear();
            li_canvas_child_grid.Clear();
            li_canvas_child_label.Clear();
            no_more_filter();

            Image img_selected_template = (Image)sender;
            dynamic json_template = JsonConvert.DeserializeObject(img_selected_template.Tag.ToString()); // Meme-Daten aus der Template-Datei

            // Default-Werte für quadratisches Meme
            double d_max_size_Y = d_canvas_initial_height;
            double d_max_size_X = d_canvas_initial_height;
            double d_left_padding = (d_canvas_initial_width - d_canvas_initial_height) / 2;
            double d_top_padding = d_canvas_margin_top;
            canvas_Bearbeitungsfenster.Width = d_canvas_initial_height;
            canvas_Bearbeitungsfenster.Height = d_canvas_initial_height;

            try {
                double d_template_format_width = (double)json_template.formatWidth;
                double d_template_format_height = (double)json_template.formatHeight;

                double d_template_aspect_ratio = (d_template_format_width / d_template_format_height) / (d_canvas_initial_width / d_canvas_initial_height);

                // Wenn Seitenverhältnis des Memes höher ist als breit
                if (d_template_aspect_ratio < 1)
                {
                    d_max_size_Y = d_canvas_initial_height;
                    canvas_Bearbeitungsfenster.Width = d_template_format_width / d_template_format_height * d_canvas_initial_height;
                    d_max_size_X = canvas_Bearbeitungsfenster.Width;
                    d_left_padding = (d_canvas_initial_width - d_max_size_X) / 2;

                }

                // Wenn Seitenverhältnis des Memes breiter ist als hoch
                if (d_template_aspect_ratio > 1)
                {
                    d_max_size_X = d_canvas_initial_width;
                    canvas_Bearbeitungsfenster.Width = d_canvas_initial_width;
                    canvas_Bearbeitungsfenster.Height = d_template_format_height / d_template_format_width * d_canvas_initial_width;
                    d_max_size_Y = canvas_Bearbeitungsfenster.Height;
                    d_top_padding = (d_canvas_initial_height - d_max_size_Y) / 2;
                    d_left_padding = d_canvas_margin_left;
                }

                a_meme_measurements[0] = d_left_padding;
                a_meme_measurements[1] = d_top_padding;
                a_meme_measurements[2] = d_max_size_X;
                a_meme_measurements[3] = d_max_size_Y;

                int i_canvas_child_index = 0;

                // Bilder in Meme werden Anhand der Daten im Template ins Canvas geladen und platziert
                foreach (var image in json_template.components.images)
                {
                    // Grid ist nötig, um die Bilder zentriert in ihrem zugewiesenen Bereich angezeigt werden können und nicht überstehen oder nur den oberen Abschnitt zeigen
                    Grid grid_meme_component_img = new Grid();

                    grid_meme_component_img.Height = (double)image.height / 100.0 * d_max_size_Y;
                    grid_meme_component_img.Width = (double)image.width / 100.0 * d_max_size_X;
                    Image img_meme_component = GenerateImageComponent(Environment.CurrentDirectory + "\\..\\..\\Templates\\Images\\" + image.relSource, (double)image.height, (double)image.width, d_max_size_X, i_canvas_child_index);
                    grid_meme_component_img.Children.Add(img_meme_component);

                    double d_position_x = (double)image.xPos / 100.0 * d_max_size_X;
                    double d_position_y = (double)image.yPos / 100.0 * d_max_size_Y;
                    Canvas.SetLeft(grid_meme_component_img, d_position_x);
                    Canvas.SetTop(grid_meme_component_img, d_position_y);
                    img_meme_component.Tag = $"{i_canvas_child_index}##{d_position_x}##{d_position_y}##{grid_meme_component_img.Width}##{grid_meme_component_img.Height}##{image.width}##{image.height}";
                    i_canvas_child_index++;
                    li_canvas_child_grid.Add(grid_meme_component_img);
                }

                // Texte in Meme werden Anhand der Daten im Template als Label ins Canvas geladen und platziert
                foreach (var data_text_component in json_template.components.text)
                {

                    Label lbl_meme_component_text = new Label();
                    lbl_meme_component_text.Cursor = Cursors.Hand;
                    lbl_meme_component_text.Content = data_text_component.content;
                    lbl_meme_component_text.FontFamily = data_text_component.font;
                    lbl_meme_component_text.FontSize = data_text_component.fontsize;
                    lbl_meme_component_text.Foreground = dict_brushes_str_scb[(string)data_text_component.color];
                    lbl_meme_component_text.HorizontalContentAlignment = HorizontalAlignment.Center;
                    lbl_meme_component_text.VerticalContentAlignment = VerticalAlignment.Center;
                    lbl_meme_component_text.Height = (double)data_text_component.height / 100.0 * d_max_size_Y;
                    lbl_meme_component_text.Width = (double)data_text_component.width / 100.0 * d_max_size_X;
                    lbl_meme_component_text.MouseLeftButtonUp += LabelInCanvasClicked;

                    double d_position_x = (double)data_text_component.xPos / 100.0 * d_max_size_X;
                    double d_position_y = (double)data_text_component.yPos / 100.0 * d_max_size_Y;
                    Canvas.SetLeft(lbl_meme_component_text, d_position_x);
                    Canvas.SetTop(lbl_meme_component_text, d_position_y);

                    lbl_meme_component_text.Tag = $"{i_canvas_child_index}##{d_position_x}##{d_position_y}";
                    i_canvas_child_index++;
                    li_canvas_child_label.Add(lbl_meme_component_text);
                }
                
                canvas_Bearbeitungsfenster.Background = Brushes.White;
            }
            catch (ApplicationException ae)
            {
                MessageBox.Show(ae.Message, "Oopsies", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("This template seems to have a bad day.\nTry a different template?", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            PopulateCanvas();
            bool_meme_is_selected = true;
        }

        public Image GenerateImageComponent(String source_uri, double d_img_height, double d_img_width, double d_canvas_width, int i_canvas_child_index)
        {
            Image img_meme_component = new Image();
            img_meme_component.Cursor = Cursors.Hand;
            img_meme_component.Stretch = Stretch.UniformToFill;
            img_meme_component.VerticalAlignment = VerticalAlignment.Center;
            img_meme_component.HorizontalAlignment = HorizontalAlignment.Center;

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
                throw new ApplicationException("The images are introverts and don't want to play right now D:\nTry a different template?");

            }

            double d_source_ascpect_ratio = (double)bmp_meme_component_source.PixelHeight / (double)bmp_meme_component_source.PixelWidth; // höher als breit wenn > 1, breiter als hoch wenn < 1

            if (d_source_ascpect_ratio < 1) // Source breiter als hoch
            {
                img_meme_component.Height = d_img_height / 100d * d_canvas_width;
                img_meme_component.Width = img_meme_component.Height * (2d - d_source_ascpect_ratio);
            }
            else // Quadrat oder höher als breit
            {
                img_meme_component.Width = d_img_width / 100d * d_canvas_width;
                img_meme_component.Height = img_meme_component.Width * d_source_ascpect_ratio;
            }


            //set image source
            img_meme_component.Source = bmp_meme_component_source;
            img_meme_component.Tag = i_canvas_child_index;
            img_meme_component.MouseLeftButtonUp += ImageInCanvasClicked;
            return img_meme_component;
        }

        public void PopulateCanvas()
        {
            canvas_Bearbeitungsfenster.Children.Clear();
            foreach (Grid grid_canvas_child in li_canvas_child_grid)
            {
                canvas_Bearbeitungsfenster.Children.Add(grid_canvas_child);
            }

            foreach (Label lbl_canvas_child in li_canvas_child_label)
            {
                canvas_Bearbeitungsfenster.Children.Add(lbl_canvas_child);
            }
        }

        public void event_remove_filter_button_clicked(object sender, RoutedEventArgs e)
        {
            PopulateCanvas();
            grid_Kontextfenster.Children.Clear();
            no_more_filter();
        }

        private void no_more_filter()
        {
            button_Add_Filter.Visibility = Visibility.Visible;
            button_Remove_Filter.Visibility = Visibility.Hidden;
            bool_filter_is_added = false;
        }

        private void LabelInCanvasClicked(object sender, MouseButtonEventArgs e)
        {
            // Das angeklickte Label wird als globale Variable in der Klasse gespeichert, um es später direkt ansprechen und im Kontextfenster verändern zu können
            Label lbl_clicked = (Label)sender;
            
            // Kontextfenster leeren und mit Label-Kontext-Controls füllen
            grid_Kontextfenster.Children.Clear();
            drawTextContext(lbl_clicked);
            DrawBorder(lbl_clicked);
            no_more_filter();
        }

        private void ImageInCanvasClicked(object sender, MouseButtonEventArgs e)
        {
            Image img_clicked = (Image)sender;
            grid_Kontextfenster.Children.Clear();
            drawBildKontext(img_clicked);
            DrawBorder(img_clicked);
            no_more_filter();
        }

        // Rahmen malen, wenn Bild angeklickt
        private void DrawBorder(Image img_clicked)
        {
            String[] separator = { "##" };
            String[] tags = img_clicked.Tag.ToString().Split(separator, StringSplitOptions.None); // Reihenfolge: child index, xPos in canvas, xPos in canvas
            Border border_image = new Border();
            border_image.Height = Double.Parse(tags[4]);
            border_image.Width = Double.Parse(tags[3]);
            border_image.BorderBrush = new SolidColorBrush(Colors.Aquamarine);
            border_image.BorderThickness = new Thickness(3,3,3,3);
            Canvas.SetLeft(border_image, Double.Parse(tags[1]));
            Canvas.SetTop(border_image, Double.Parse(tags[2]));
            PopulateCanvas();
            canvas_Bearbeitungsfenster.Children.Insert(Int32.Parse(tags[0])+1, border_image);
        }

        // Rahmen malen, wenn Label angeklickt
        private void DrawBorder(Label lbl_clicked)
        {
            String[] separator = { "##" };
            String[] tags = lbl_clicked.Tag.ToString().Split(separator, StringSplitOptions.None); // child index, xPos in canvas, xPos in canvas
            Border border_label = new Border();
            border_label.Height = lbl_clicked.Height;
            border_label.Width = lbl_clicked.Width;
            border_label.BorderBrush = new SolidColorBrush(Colors.Aquamarine);
            border_label.BorderThickness = new Thickness(3, 3, 3, 3);
            Canvas.SetLeft(border_label, Double.Parse(tags[1]));
            Canvas.SetTop(border_label, Double.Parse(tags[2]));
            PopulateCanvas();
            i_border_index_in_canvas = Int32.Parse(tags[0]) + 1;
            canvas_Bearbeitungsfenster.Children.Insert(Int32.Parse(tags[0]) + 1, border_label);
        }

        // Meme ohne Effekt im Canvas rendern und zuschneiden
        public CroppedBitmap cb_render_canvas()
        {
            // eventuellen Rahmen vorm Rendern entfernen, falls nicht schon ein Filter angewendet wurde
            if (!bool_filter_is_added)
                PopulateCanvas();

            // Canvas rendern und Canvas-Maße um Margins beim Rendern erweitert, um Out-of-Bounds-Exception beim Zuschneiden zu verhindern
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas_Bearbeitungsfenster.RenderSize.Width + (int)a_meme_measurements[0], (int)canvas_Bearbeitungsfenster.RenderSize.Height + (int)a_meme_measurements[1], 96d, 96d, PixelFormats.Default);
            rtb.Render(canvas_Bearbeitungsfenster);

            // Render-Ergebnis auf Meme zuschneiden und kleine Ränder außen entfernen (+-2)
            return new CroppedBitmap(rtb, new Int32Rect((int)a_meme_measurements[0] + 2, (int)a_meme_measurements[1] + 2, (int)a_meme_measurements[2] - 2, (int)a_meme_measurements[3] - 2));
        }

        // Fertiges Meme als Bild exportieren
        public void SaveImage(object sender, RoutedEventArgs e)
        {
            if (bool_meme_is_selected)
            {
                CroppedBitmap cb_crop = cb_render_canvas();

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(cb_crop));


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
                catch
                {
                    MessageBox.Show("The meme could not be saved and the world is lost!", "Onoez", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You gotta select the meme before you can save the meme!", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }





        
        //Code Yannic
        public void drawTextContext(Label label)
        {
            textWindow.set_targetLbl(label);
            grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontMenu(label.FontFamily.ToString()));
            grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontSize(label.FontSize.ToString()));
            grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontStyle(label.FontStyle.ToString()));
            grid_Kontextfenster.Children.Add(textWindow.get_txtField_Text(label.Content.ToString()));
            grid_Kontextfenster.Children.Add(textWindow.get_btn_txtField_Apply());
            grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontColor(dict_brushes_scb_str[(SolidColorBrush)label.Foreground]));
            grid_Kontextfenster.Children.Add(textWindow.get_lbl_Color());
            grid_Kontextfenster.Children.Add(textWindow.get_lbl_Font_Family());
            grid_Kontextfenster.Children.Add(textWindow.get_lbl_Font_Size());
            grid_Kontextfenster.Children.Add(textWindow.get_lbl_Content());
        }



        public void drawBildKontext(Image img_in)
        {
            pictureWindow.set_img_targetImg(img_in);
            pictureWindow.d_canvas_width = canvas_Bearbeitungsfenster.Width;
            pictureWindow.d_canvas_height = canvas_Bearbeitungsfenster.Height;
            String[] separator = { "##" };
            String[] tags = img_in.Tag.ToString().Split(separator, StringSplitOptions.None); // Reihenfolge: child index, xPos in canvas, xPos in canvas
            pictureWindow.d_template_img_height = Double.Parse(tags[6]);
            pictureWindow.d_template_img_width = Double.Parse(tags[5]);
            pictureWindow.d_parent_grid_height = Double.Parse(tags[4]);
            pictureWindow.d_parent_grid_width = Double.Parse(tags[3]);
            grid_Kontextfenster.Children.Add(pictureWindow.get_wrapP_content());
        }
        
        // Canvas für Effekt rendern und zuschneiden
        public CroppedBitmap cb_render_canvas_filter()
        {
            PopulateCanvas();

            // Canvas rendern und Canvas-Maße um Margins beim Rendern erweitert, um Out-of-Bounds-Exception beim Zuschneiden zu verhindern
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas_Bearbeitungsfenster.RenderSize.Width + (int)a_meme_measurements[0], (int)canvas_Bearbeitungsfenster.RenderSize.Height + (int)a_meme_measurements[1], 96d, 96d, PixelFormats.Default);
            rtb.Render(canvas_Bearbeitungsfenster);

            // Render-Ergebnis auf Meme zuschneiden
            return new CroppedBitmap(rtb, new Int32Rect((int)a_meme_measurements[0], (int)a_meme_measurements[1], (int)a_meme_measurements[2], (int)a_meme_measurements[3]));
        }

        //Eventhandler
        private void event_add_filter_button_clicked(object sender, RoutedEventArgs e)
        {

            if (bool_meme_is_selected)
            {
                MessageBoxResult result = MessageBox.Show("You can no longer edit images or text in your meme when you apply a filter. Do you still want to proceed?", "Add the filter", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    bool_filter_is_added = true;
                    button_Remove_Filter.Visibility = Visibility.Visible;
                    button_Add_Filter.Visibility = Visibility.Hidden;

                    CroppedBitmap cb_crop = cb_render_canvas_filter();

                    BitmapEncoder pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(cb_crop));
                    i_effect_counter++;
                    
                    FileStream stream = File.Create(Environment.CurrentDirectory + "\\..\\..\\MemeResources\\temp\\img_TargetImage.jpg");
                    pngEncoder.Save(stream);
                    stream.Dispose();
                    stream.Close();

                    effectWindow.set_canvas_target(canvas_Bearbeitungsfenster);

                    grid_Kontextfenster.Children.Clear();
                    Slider sld_Brightness = effectWindow.get_sld_Brightness();
                    sld_Brightness.Value = 0;
                    grid_Kontextfenster.Children.Add(sld_Brightness);
                    grid_Kontextfenster.Children.Add(effectWindow.get_lbl_Brightness_value());
                    grid_Kontextfenster.Children.Add(effectWindow.get_lbl_Brightness());
                    ComboBox cmBox_Filter = effectWindow.get_cmBox_Filter();
                    cmBox_Filter.SelectedIndex = 0;
                    grid_Kontextfenster.Children.Add(cmBox_Filter);
                    grid_Kontextfenster.Children.Add(effectWindow.get_lbl_Filter());
                    Slider sld_Contrast = effectWindow.get_sld_Contrast();
                    sld_Contrast.Value = 0;
                    grid_Kontextfenster.Children.Add(sld_Contrast);
                    grid_Kontextfenster.Children.Add(effectWindow.get_lbl_Contrast_value());
                    grid_Kontextfenster.Children.Add(effectWindow.get_lbl_Contrast());
                }
            }
            else
            {
                MessageBox.Show("You gotta select the meme before you can filter the meme!", "Oopsies", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}

