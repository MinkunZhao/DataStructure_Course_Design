using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ecosystem
{
    /// <summary>
    /// HelpPage2.xaml 的交互逻辑
    /// </summary>
    public partial class HelpPage2 : Window
    {
        public HelpPage2()
        {
            InitializeComponent();
            rich_text.Text =
                "\t In the main window, green point represents first nutritional level, blue point represents second trophic level, red point represents third trophic level.\n" + 
                "\tIn the control panel, we can start and stop the process by click the corresponding button. After some time, we can generate the statistic report for the whole process. Meanwhile, we can see the information of any entity in the main screen by click any circle in the main program.\n" +
                "\tWhen we click start button, the process starts. When we click one entity in the main screen, we can see that this entity is wrap by a circle, and its information is display in the control panel.\n" +
                "\tAge is the age of this entity, energy represented the remained energy of this entity, and energy can only be obtained by eating prey. Tiredness represents the tiredness of this entity. If this animal run too fast, it will become tired. Type represents its type among first nutritional level, second trophic level and third trophic level. x and y represent the x and y position of this entity. Action represent the action this entity do now.\n";
        }
    }
}
