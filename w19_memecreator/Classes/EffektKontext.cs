//using ImageProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace w19_memecreator
{
    class EffektKontext
    {
        //Variables
        //ImageFactory imgFactory = new ImageFactory();
        const string path_lmao_emoji = "C:/Users/yanni/Source/Repos/Fluxstone/w19_memecreator/w19_memecreator/Resources/4506313_0.jpg";

        Point cursor = new Point(0, 0);
        Button btn_effectField_Apply = new Button();
        Button btn_effectField_Brightness = new Button();

        Image img_LMAO_Emoj = new Image();
        
        //Constructor
        public EffektKontext()
        {
            //img_LMAO_Emoj.Source = new BitmapImage(new Uri(path_lmao_emoji));
        }

        public void setWindowProperties()
        {
            btn_effectField_Apply.Height = 25;
            btn_effectField_Apply.Width = 90;
            btn_effectField_Apply.Content = "Apply Changes";
            btn_effectField_Apply.HorizontalAlignment = HorizontalAlignment.Left;
            btn_effectField_Apply.VerticalAlignment = VerticalAlignment.Top;
            btn_effectField_Apply.Margin = new Thickness(10, 130, 0, 0);

            btn_effectField_Brightness.Height = 25;
            btn_effectField_Brightness.Width = 90;
            btn_effectField_Brightness.Content = "Brightness";
            btn_effectField_Brightness.HorizontalAlignment = HorizontalAlignment.Left;
            btn_effectField_Brightness.VerticalAlignment = VerticalAlignment.Top;
            btn_effectField_Brightness.Margin = new Thickness(10, 170, 0, 0);
        }
       
        public void drawSprite(Canvas targetCanvas)
        {
            targetCanvas.Children.Add(img_LMAO_Emoj);
            Canvas.SetLeft(get_img_LMAO_Emoj(), cursor.X);
            Canvas.SetTop(get_img_LMAO_Emoj(), cursor.Y);
        }

        //Getter und Setter
        public Point get_Cursor()
        {
            return cursor;
        }

        public void set_Cursor(Canvas canvas_in)
        {
            Point p = Mouse.GetPosition(canvas_in);
            cursor = p;
        }

        public Button get_btn_effectField_Apply()
        {
            return btn_effectField_Apply;
        }

        public Button get_btn_effectField_Brightness()
        {
            return btn_effectField_Brightness;
        }

        public Image get_img_LMAO_Emoj()
        {
            return img_LMAO_Emoj;
        }
        //Event Handler
    }
}
