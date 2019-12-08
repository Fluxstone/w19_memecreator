using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace w19_memecreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
      
    public partial class MainWindow : Window
    {
        TextKontext textWindow = new TextKontext();
        EffektKontext effectWindow = new EffektKontext();

        public MainWindow()
        {
            InitializeComponent();
            canvas_Bearbeitungsfenster.AddHandler(Canvas.MouseLeftButtonDownEvent, new RoutedEventHandler(canvas_Bearbeitungsfenster_MouseLeftButtonDown));
        }
        public void drawTextContext()
        {
            grid_Kontextfenster.Children.Add(textWindow.get_btn_txtField_Apply());
            grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontMenu());
            grid_Kontextfenster.Children.Add(textWindow.get_cmBox_fontSize());
            grid_Kontextfenster.Children.Add(textWindow.get_txtField_Text());
        }
        
        public void drawEffectContext()
        {
            grid_Kontextfenster.Children.Add(effectWindow.get_btn_effectField_Apply());
        }

        //Eventhandler

        //Menuitem
        public void addSprite_Click(object sender, RoutedEventArgs e)
        {
            drawEffectContext();
        }

        public void canvas_Bearbeitungsfenster_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            effectWindow.set_Cursor(canvas_Bearbeitungsfenster);
            effectWindow.drawSprite(canvas_Bearbeitungsfenster);
        }
    }
}

