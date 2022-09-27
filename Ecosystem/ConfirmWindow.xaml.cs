using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary.classes;
using static Ecosystem.GlobalObject;

namespace Ecosystem;
/// <summary>
/// Interaction logic for ConfirmWindow.xaml
/// </summary>
public partial class ConfirmWindow : Window
{
    public ConfirmWindow() => InitializeComponent();

    /**
     * Function: react to the click of confirm button
     */
    private void btn_confirm_click(object sender, RoutedEventArgs e)
    {
        Number = int.Parse(number.Text);
        desireFrameRate = int.Parse(desire_number.Text);
        ratioOfFirst = int.Parse(FirstRatio.Text);
        ratioOfSecond = int.Parse(SecondRatio.Text);
        ratioOfThird = int.Parse(ThirdRatio.Text);
        WindowObject.GetWindow().panel.Show();
        WindowObject.GetWindow().mainWindow.Show();
        this.Close();
    }

    /**
     * Function: react to the click of radio button named By Group
     */
    private void by_group_checked(object sender, RoutedEventArgs e) => FirstChoice = Generation.ByGroup;

    /**
     * Function: react to the click of radio button named Randomly
     */
    private void randomly_checked(object sender, RoutedEventArgs e) => FirstChoice = Generation.Randomly;

    /**
     * Function: react to the click of radio button named Horse
     */
    private void horse_checked(object sender, RoutedEventArgs e) => SecondChoice = SecondLevel.Horse;

    /**
     * Function: react to the click of radio button named Sheep
     */
    private void sheep_checked(object sender, RoutedEventArgs e) => SecondChoice = SecondLevel.Sheep;

    /**
     * Function: react to the click of radio button named Rabbit
     */
    private void rabbit_checked(object sender, RoutedEventArgs e) => SecondChoice = SecondLevel.Rabbit;

    /**
     * Function: react to the click of radio button named Tiger
     */
    private void tiger_checked(object sender, RoutedEventArgs e) => ThirdChoice = ThirdLevel.Tiger;

    /**
     * Function: react to the click of radio button named Wolf
     */
    private void wolf_checked(object sender, RoutedEventArgs e) => ThirdChoice = ThirdLevel.Wolf;

    /**
     * Function: limit the input field
     */
    private void limit_number(object sender, TextCompositionEventArgs e)
    {
        Regex re = new Regex("[^0-9]+");
        e.Handled = re.IsMatch(e.Text);
    }

    /**
     * Function: react to the click of help button
     */
    private void help_click(object sender, RoutedEventArgs e)
    {
        new HelpPage().ShowDialog();
    }
}
