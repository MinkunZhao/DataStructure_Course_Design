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
using ClassLibrary.classes;
using Ecosystem.service;
using ClassLibrary;
using static Ecosystem.GlobalObject;
using static Ecosystem.algorithm.Generator;

namespace Ecosystem;
/// <summary>
/// Main.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.Top = 80;
        this.Left = 10;
        InitializeComponent();
        Initialization();
        //canvasObject = canvas;
        if (FirstChoice == Generation.ByGroup)
            GenerateItemByGroup();
        else
            RandomlyGenerateItem();
        
    }

    public void Initialization()
    {
        canvasObject = new Canvas()
        {
            Width = 1000,
            Height = 500
        };
        border = new Border()
        {
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(50),
            Height = 600,
            Width = 1100
        };
        border.Child = canvasObject;
        this.grid.Children.Add(border);
    }
    public void GenerateItemByGroup()
    {
        var locationAndChoices = GetAllLocations(Number);
        foreach (var locationAndChoice in locationAndChoices)
        {
            int choice = locationAndChoice.Choice;
            double left = locationAndChoice.Location.Left;
            double top = locationAndChoice.Location.Top;
            switch (choice)
            {
                case 0:
                    CreateShape(Brushes.Green, left, top);
                    break;
                case 1:
                    CreateShape(Brushes.Blue, left, top);
                    break;
                case 2:
                    CreateShape(Brushes.Red, left, top);
                    break;
            }
        }
    }

    public void RandomlyGenerateItem()
    {
        var random = new Random();
        double Sum = ratioOfFirst + ratioOfSecond + ratioOfThird;
        double RTFirst = ratioOfFirst / Sum;
        double RTSecond = ratioOfSecond / Sum;
        //double RTThird = ratioOfThird / Sum;
        for (int i = 0; i < Number; i++)
        {
            double choice = random.NextDouble();
            if (choice >= 0 && choice <= RTFirst)
            {
                CreateShape(Brushes.Green, CreateRandomNumber(990), CreateRandomNumber(490));
            }
            else if (choice >= RTFirst && choice <= RTFirst + RTSecond)
            {
                CreateShape(Brushes.Blue, CreateRandomNumber(990), CreateRandomNumber(490));
            }
            else
            {
                CreateShape(Brushes.Red, CreateRandomNumber(990), CreateRandomNumber(490));
            }
        }
    }
}
