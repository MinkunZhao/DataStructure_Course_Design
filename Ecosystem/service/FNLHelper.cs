using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ClassLibrary;
using ClassLibrary.classes;
using static Ecosystem.GlobalObject;
namespace Ecosystem.service;
public class FNLHelper
{
    public FirstNutritionalLevel entity;

    public Shape shape;
    public Location location;
    public void ShowInformation(object sender, MouseButtonEventArgs e)
    {
        if (!canvasObject.Children.Contains(circle))
            canvasObject.Children.Add(circle);
        WindowObject.GetWindow().panel.type_information.Text = "First Nutritional Level";
        WindowObject.GetWindow().panel.age_information.Text = entity.Age.ToString();
        WindowObject.GetWindow().panel.energy_information.Text = "";
        WindowObject.GetWindow().panel.state_information.Text = "";
        WindowObject.GetWindow().panel.tiredness_information.Text = "";
        tracedObject = this;
    }
}

