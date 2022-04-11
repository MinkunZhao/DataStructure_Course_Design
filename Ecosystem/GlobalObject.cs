using System;
using System.Collections;
using System.Collections.Generic;
using ClassLibrary.classes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ClassLibrary;
using Ecosystem.service;
using static Ecosystem.controller.Detector;
using static Ecosystem.controller.Chooser;
using static Ecosystem.controller.EnvironmentChanger;
using static Ecosystem.controller.Updater;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ecosystem;
public enum Generation { ByGroup, Randomly }
public enum SecondLevel { Horse, Sheep, Rabbit }
public enum ThirdLevel { Tiger, Wolf }

public static class GlobalObject
{
    public static int count20 = 0;  // ranges from 0 to 19 and the age of the species increase by 1 every 20 ticks

    public static int count5 = 0;

    //Setting in the confirm window
    public static Generation FirstChoice = Generation.ByGroup;
    public static SecondLevel SecondChoice = SecondLevel.Horse;
    public static ThirdLevel ThirdChoice = ThirdLevel.Tiger;
    public static int Number;
    public static int desireFrameRate = 60;
    public static double ratioOfFirst;
    public static double ratioOfSecond;
    public static double ratioOfThird;

    public static ClassLibrary.classes.Environment.Weather GlobalWeather;

    //A set of all animal
    public static List<object> AllAnimal = new();

    //control
    public static Canvas canvasObject;
    public static Border border;

    //constant
    public const double MAX_SIGHT_RANGE = 50;

    //control
    public static bool Stop = false;

    //Statistics
    public static List<int> FNLStatistics = new();
    public static List<int> STLStatistics = new();
    public static List<int> TTLStatistics = new();
    public static List<int> TickStatistics = new();

    //traced object
    public static dynamic? tracedObject = null;

    public static Ellipse circle;

    public static int CreateRandomNumber(int maxNumber, int minNumber = 0)
    {
        var random = new Random();
        return random.Next(minNumber, maxNumber);
    }

    public static double CreateRandomNumber(double width, double start = 0.0)
    {
        var random = new Random();
        return random.NextDouble() * width + start;
    }

    public class WindowObject
    {
        private WindowObject(){}
        public MainWindow mainWindow = new();
        public ControlPanel panel = new();
        private static WindowObject? windowObject;

        public static WindowObject GetWindow(){
            if (windowObject == null)
            {
                windowObject = new WindowObject();
            }
            return windowObject;
        }
            
    }

    public static async void ToStart()
    {
        while (!Stop)
        {
            count20++;
            count5++;
            EnvironmentChange();
            UpdateState();
            //繁殖改成5个tick一次了
            if (count20 % 5 == 1)
                Propagate();
            if (count5 % 5 == 2)
                Statistics();
            await Detect();
            //await Task.Delay(500);
            await ChooseAndAction();
            await Task.Delay(1000);
        }
        
    }

    public static void Statistics()
    {
        FNLStatistics.Add(FirstNutritionalLevel.Count);
        STLStatistics.Add(SecondTrophicLevel.Count);
        TTLStatistics.Add(ThirdTrophicLevel.Count);
        TickStatistics.Add(count20);
    }

    public static void CreateMotionX(STLHelper obj, double start, double end, double duration, PropertyPath propertyPath)
    {
        obj.doubleAnimationX.From = start;
        obj.doubleAnimationX.To = end;
        obj.doubleAnimationX.Duration = TimeSpan.FromSeconds(duration);
        Storyboard.SetTarget(obj.doubleAnimationX, obj.shape);
        Storyboard.SetTargetProperty(obj.doubleAnimationX, propertyPath);
        
    }
    public static void CreateMotionX(TTLHelper obj, double start, double end, double duration, PropertyPath propertyPath)
    {
        obj.doubleAnimationX.From = start;
        obj.doubleAnimationX.To = end;
        obj.doubleAnimationX.Duration = TimeSpan.FromSeconds(duration);
        Storyboard.SetTarget(obj.doubleAnimationX, obj.shape);
        Storyboard.SetTargetProperty(obj.doubleAnimationX, propertyPath);
    }
    public static void CreateMotionY(STLHelper obj, double start, double end, double duration, PropertyPath propertyPath)
    {
        obj.doubleAnimationY.From = start;
        obj.doubleAnimationY.To = end;
        obj.doubleAnimationY.Duration = TimeSpan.FromSeconds(duration);
        Storyboard.SetTarget(obj.doubleAnimationY, obj.shape);
        Storyboard.SetTargetProperty(obj.doubleAnimationY, propertyPath);
    }
    public static void CreateMotionY(TTLHelper obj, double start, double end, double duration, PropertyPath propertyPath)
    {
        obj.doubleAnimationY.From = start;
        obj.doubleAnimationY.To = end;
        obj.doubleAnimationY.Duration = TimeSpan.FromSeconds(duration);
        Storyboard.SetTarget(obj.doubleAnimationY, obj.shape);
        Storyboard.SetTargetProperty(obj.doubleAnimationY, propertyPath);
    }

    /*
     * Moving function
     * Input: instance, direction of x and y
     */
    public static void Moving(object obj, double XDirection, double YDirection)
    {
        switch (obj)
        {
            case STLHelper obj2:
                obj2.storyboard.Children.Clear();
                obj2.shape.RenderTransform = new TranslateTransform(0, 0);
                CreateMotionX(obj2, 
                    obj2.offset.Left, obj2.offset.Left + XDirection, 1,
                    new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                CreateMotionY(obj2, 
                    obj2.offset.Top, obj2.offset.Top + YDirection, 1,
                    new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                obj2.storyboard.Children.Add(obj2.doubleAnimationX);
                obj2.storyboard.Children.Add(obj2.doubleAnimationY);
                obj2.offset.Left += XDirection;
                obj2.offset.Top += YDirection;
                obj2.storyboard.Begin();
                break;
            case TTLHelper obj3:
                obj3.storyboard.Children.Clear();
                obj3.shape.RenderTransform = new TranslateTransform(0, 0);
                CreateMotionX(obj3,
                    obj3.offset.Left, obj3.offset.Left + XDirection, 1,
                    new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                CreateMotionY(obj3,
                    obj3.offset.Top, obj3.offset.Top + YDirection, 1,
                    new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                obj3.storyboard.Children.Add(obj3.doubleAnimationX);
                obj3.storyboard.Children.Add(obj3.doubleAnimationY);
                obj3.offset.Left += XDirection;
                obj3.offset.Top += YDirection;
                obj3.storyboard.Begin();
                break;
        }
    }
    /*
     * CreateShape function
     * Input: color, x position and y position
     */
    public static Shape CreateShape(Brush brush, double left, double top)
    {
        var random = new Random();
        Shape shape = new Ellipse() { Width = 10, Height = 10 };
        shape.Fill = brush;
        Canvas.SetLeft(shape, left);
        Canvas.SetTop(shape, top);
        canvasObject.Children.Add(shape);
        if (brush == Brushes.Green)
        {
            var animal = new FNLHelper()
            {
                entity = new FirstNutritionalLevel()
                {
                    Age = CreateRandomNumber(FirstNutritionalLevel.MAX_AGE / 2, 1),
                    Illumination = CreateRandomNumber(95, 55),
                    Moisture = CreateRandomNumber(100, 60),
                },
                shape = shape,
                location = new Location(){Top = top, Left = left}
            };
            animal.shape.AddHandler(UIElement.MouseLeftButtonUpEvent, 
                new MouseButtonEventHandler(animal.ShowInformation));
            AllAnimal.Add(animal);
            FirstNutritionalLevel.Count++;
        }
        else if (brush == Brushes.Blue)
        {
            var animal = new STLHelper()
            {
                entity = new SecondTrophicLevel()
                {
                    Energy = SecondTrophicLevel.MAX_ENERGY * CreateRandomNumber(0.2, 0.8),
                    Tiredness = 0,
                    Age = CreateRandomNumber(SecondTrophicLevel.MAX_AGE * 6 / 10, SecondTrophicLevel.MAX_AGE / 10)
                },
                shape = shape,
                originLocation = new Location(){Top = top, Left = left}
            };
            animal.shape.AddHandler(UIElement.MouseLeftButtonUpEvent, 
                new MouseButtonEventHandler(animal.ShowInformation));
            AllAnimal.Add(animal);
            SecondTrophicLevel.Count++;
        }
        else
        {
            var animal = new TTLHelper()
            {
                entity = new ThirdTrophicLevel()
                {
                    Energy = ThirdTrophicLevel.MAX_ENERGY * CreateRandomNumber(0.2, 0.8),
                    Tiredness = 0,
                    Age = CreateRandomNumber(ThirdTrophicLevel.MAX_AGE * 6 / 10, ThirdTrophicLevel.MAX_AGE / 10)
                },
                shape = shape,
                originLocation = new Location(){Top = top, Left = left}
            };
            animal.shape.AddHandler(UIElement.MouseLeftButtonUpEvent, 
                new MouseButtonEventHandler(animal.ShowInformation));
            AllAnimal.Add(animal);
            ThirdTrophicLevel.Count++;
        }
        return shape;
    }

    public static double SquSum(double x, double y) => Math.Sqrt(x * x + y * y);

    public static async void Timer3(TTLHelper ttl)
    {
        ttl.flag = false;
        await Task.Delay(1000);
        ttl.flag = true;
    }

    public static async void Timer2(STLHelper stl)
    {
        stl.flag = false;
        await Task.Delay(1000);
        stl.flag = true;
    }

    /*
     * Chase function
     * When calling this function, ttl will chase stl for 1 second.
     */
    public static async void Chase32(TTLHelper ttl, STLHelper stl)
    {
        Timer3(ttl);
        await Task.Run(async () =>
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (canvasObject.Children.Contains(stl.shape))
                    {
                        var dist = ttl.shape.TranslatePoint(new Point(), stl.shape);
                        if (SquSum(dist.X, dist.Y) < 5)
                        {
                            ttl.entity.Energy += SecondTrophicLevel.ENERGY_BRINGTO_TTL;
                            if (ttl.entity.Energy > ThirdTrophicLevel.MAX_ENERGY)
                                ttl.entity.Energy = ThirdTrophicLevel.MAX_ENERGY;
                            canvasObject.Children.Remove(stl.shape);
                            AllAnimal.Remove(stl);
                            SecondTrophicLevel.Count--;
                            ttl.flag = true;
                            Console.WriteLine("eat");
                        }
                    }
                });
                await Task.Delay(5);
                if (ttl.flag)
                {
                    break;
                }
            }

        });
    }

    /*
     * Chase function
     * When calling this function, stl will hunt fnl for 1 second.
     */
    public static async void Chase21(STLHelper stl, FNLHelper fnl)
    {
        Timer2(stl);
        await Task.Run(async () =>
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (canvasObject.Children.Contains(fnl.shape))
                    {
                        //var dist = stl.shape.TranslatePoint(new Point(), stl.shape);
                        //var (distX, distY) = (stl.location.Left - fnl.location.Left,
                        //    stl.location.Top - fnl.location.Top);
                        var dist = stl.shape.TranslatePoint(new Point(), fnl.shape);
                        if (SquSum(dist.X, dist.Y) < 5)
                        {
                            stl.entity.Energy += FirstNutritionalLevel.ENERGY_BRINGTO_STL;
                            if (stl.entity.Energy > SecondTrophicLevel.MAX_ENERGY)
                                stl.entity.Energy = SecondTrophicLevel.MAX_ENERGY;
                            canvasObject.Children.Remove(fnl.shape);
                            AllAnimal.Remove(fnl);
                            FirstNutritionalLevel.Count--;
                            stl.flag = true;
                        }
                    }
                });
                await Task.Delay(5);
                if (stl.flag)
                {
                    break;
                }
            }

        });
    }
}
