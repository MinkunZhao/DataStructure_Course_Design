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
using static Ecosystem.GlobalObject;
using FSharpExt;

namespace Ecosystem
{
    /// <summary>
    /// ControlPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ControlPanel : Window
    {
        
        public ControlPanel()
        {
            circle = new Ellipse{Width = 20, Height = 20};
            circle.Fill = Brushes.Transparent;
            circle.Stroke = Brushes.Black;
            circle.StrokeThickness = 2;
            //canvasObject.Children.Add(circle);
            this.Top = 80;
            this.Left = 1200;
            InitializeComponent();
        }

        private async void start_click(object sender, RoutedEventArgs e)
        {
            Stop = false;
            ToStart();
            btn_start.IsEnabled = false;
            btn_stop.IsEnabled = true;
            while (true)
            {
                await Task.Delay(100);
                if (tracedObject != null)
                {
                    double x, y;
                    if (tracedObject is FNLHelper)
                    {
                        string xText = this.x_location.Text = tracedObject.shape.TranslatePoint(new Point(), canvasObject).X.ToString();
                        x = double.Parse(xText);
                        string yText = this.y_location.Text = tracedObject.shape.TranslatePoint(new Point(), canvasObject).Y.ToString();
                        y = double.Parse(yText);
                        
                        circle.SetValue(Canvas.LeftProperty, x - 5);
                        circle.SetValue(Canvas.TopProperty, y - 5);
                    }
                    else
                    {
                        string xText = this.x_location.Text = tracedObject.shape.TranslatePoint(new Point(), canvasObject).X.ToString();
                        x = double.Parse(xText);
                        string yText = this.y_location.Text = tracedObject.shape.TranslatePoint(new Point(), canvasObject).Y.ToString();
                        y = double.Parse(yText);
                        this.state_information.Text = tracedObject.entity.State.ToString();
                        this.energy_information.Text = tracedObject.entity.Energy.ToString();
                        this.tiredness_information.Text = tracedObject.entity.Tiredness.ToString();
                        this.age_information.Text = tracedObject.entity.Age.ToString();
                        
                        circle.SetValue(Canvas.LeftProperty, x - 5);
                        circle.SetValue(Canvas.TopProperty, y - 5);
                    }
                }
            }
        }

        private void stop_click(object sender, RoutedEventArgs e)
        {
            Stop = true;
            btn_stop.IsEnabled = false;
            btn_start.IsEnabled = true;
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            var filename = fileNameBox.Text;
            if (filename == "")
            {
                MessageBox.Show(this, "Filename cannot be empty!", "Error");
                return;
            }
            Ext.CreateChart(
                ticks: TickStatistics, 
                fnlStatistics: FNLStatistics, 
                stlStatistics: STLStatistics, 
                ttlStatistics: TTLStatistics,
                filename: filename
            );
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            tracedObject = null;
            this.tiredness_information.Text = "------";
            this.age_information.Text = "------";
            this.energy_information.Text = "------";
            this.state_information.Text = "------";
            this.type_information.Text = "------";
            this.x_location.Text = "------";
            this.y_location.Text = "------";
            if(canvasObject.Children.Contains(circle))
                canvasObject.Children.Remove(circle);
        }
    }
}
