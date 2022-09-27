﻿using System;
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
    /// HelpPage.xaml 的交互逻辑
    /// </summary>
    public partial class HelpPage : Window
    {
        public HelpPage()
        {
            InitializeComponent();
            rich_text.Text = "\tIn initial setting screen, we can input data as running condition.\n" +
                             "\tIn radio group named Generation, we can select the generation way. There are two ways: By Group and Randomly. If user choose By Group, animal in second trophic level and third trophic level will be generated by group.If user choose Randomly, animal will not be generated in group.\n" +
                             "\tIn radio group named Second trophic level and Third trophic level, we can choose the type of animal in different nutritional level. The program will give different types of animal different attributes, like speed, life time and so on.\n" +
                             "\tIn input field named Entity number, we can decide the number of animal generated in the screen.\n" +
                             "\tIn input field named Desired Framerate, we can decide the framerate of the program, that is, to decide how many frames will be displayed in one second. This function is mainly provided for the computer with low configuration.\n" +
                             "\tIn input field named Ratio of Three Nutritional Level, we can decide the ratio of three nutritional level in the screen. For example, if the number of animal is 100, the ratio is 5:3:2, then numbers of three nutritional level are 50, 30 and 20.\n" +
                             "\tAfter we complete the settings, we can click the confirm button.";
        }
    }
}
