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
public class TTLHelper
{
    public ThirdTrophicLevel entity;
    public bool flag;
    public int angle;
    public Location offset = new Location()
    {
        Top = 0,
        Left = 0,
    };
    public Shape shape;
    public Location originLocation;

    public Location location => new Location()
    {
        Top = originLocation.Top + offset.Top,
        Left = originLocation.Left + offset.Left,
    };
    public Storyboard storyboard = new();
    public DoubleAnimation doubleAnimationX = new();
    public DoubleAnimation doubleAnimationY = new();

    public TTLHelper()
    {
        Timeline.SetDesiredFrameRate(storyboard, desireFrameRate);
        angle = new Random().Next(50);
    }

    public void ShowInformation(object sender, MouseButtonEventArgs e)
    {
        if (!canvasObject.Children.Contains(circle))
            canvasObject.Children.Add(circle);
        WindowObject.GetWindow().panel.type_information.Text = "Third Trophic Level";
        WindowObject.GetWindow().panel.age_information.Text = entity.Age.ToString();
        WindowObject.GetWindow().panel.energy_information.Text = entity.Energy.ToString();
        WindowObject.GetWindow().panel.state_information.Text = entity.State.ToString();
        WindowObject.GetWindow().panel.tiredness_information.Text = entity.Tiredness.ToString();
        tracedObject = this;
    }
}

