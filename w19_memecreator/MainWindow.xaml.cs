using System.Windows;
using System.Windows.Controls;

namespace w19_memecreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
      
    public partial class MainWindow : Window
    {
        KontextFenster contextWindow = new KontextFenster();

        public MainWindow()
        {
            InitializeComponent();
            contextWindow.setWindowProperties();
        }
        public void drawContextWindow()
        {
            grid_Kontextfenster.Children.Add(contextWindow.get_btn_txtField_Apply());
            grid_Kontextfenster.Children.Add(contextWindow.get_cmBox_fontMenu());
            grid_Kontextfenster.Children.Add(contextWindow.get_cmBox_fontSize());
            grid_Kontextfenster.Children.Add(contextWindow.get_txtField_Text());
        }
    }
}

