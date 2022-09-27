using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ecosystem.service;
using static Ecosystem.GlobalObject;

namespace Ecosystem.controller
{
    public class Detector
    {
        /**
         * Function: Let every animal detect animal in their view, that is, to push animals in their view into their detection set
         * Input: Empty
         * Output: Empty
         */
        public static async Task Detect()
        {
            await Task.Run(() =>
            {
                foreach (var animal in AllAnimal.ToArray())
                {
                    switch (animal)
                    {
                        case STLHelper obj1:
                        {
                            obj1.entity.ObjectInSight = new List<object>();
                            foreach (var o in AllAnimal.ToArray())
                            {
                                switch (o)
                                {
                                    case FNLHelper obj11:
                                    {
                                        var dist = SqrtSumSquare(obj11.location.Top - obj1.location.Top,
                                            obj11.location.Left - obj1.location.Left);
                                        if (dist < MAX_SIGHT_RANGE)
                                        {
                                            obj1.entity.ObjectInSight.Add(obj11);
                                        }
                                    }
                                        break;
                                    case STLHelper obj12:
                                    {
                                        if (obj12 != obj1)
                                        {
                                            var dist = SqrtSumSquare(obj12.location.Top - obj1.location.Top,
                                                obj12.location.Left - obj1.location.Left);
                                            if (dist < MAX_SIGHT_RANGE)
                                            {
                                                obj1.entity.ObjectInSight.Add(obj12);
                                            }
                                        }
                                    }
                                        break;
                                    case TTLHelper obj13:
                                    {
                                        var dist = SqrtSumSquare(obj13.location.Top - obj1.location.Top,
                                            obj13.location.Left - obj1.location.Left);
                                        if (dist < MAX_SIGHT_RANGE)
                                        {
                                            obj1.entity.ObjectInSight.Add(obj13);
                                        }
                                    }
                                        
                                        break;
                                }
                            }
                        }
                            break;
                        case TTLHelper obj2:
                        {
                            obj2.entity.ObjectInSight = new List<object>();
                            foreach (var o in AllAnimal.ToArray())
                            {
                                switch (o)
                                {
                                    case FNLHelper obj21:
                                    {
                                        var dist = SqrtSumSquare(obj21.location.Top - obj2.location.Top,
                                            obj21.location.Left - obj2.location.Left);
                                        if (dist < MAX_SIGHT_RANGE)
                                        {
                                            obj2.entity.ObjectInSight.Add(obj21);
                                        }
                                    }
                                        break;
                                    case STLHelper obj22:
                                    {
                                        var dist = SqrtSumSquare(obj22.location.Top - obj2.location.Top,
                                            obj22.location.Left - obj2.location.Left);
                                        if (dist < MAX_SIGHT_RANGE)
                                        {
                                            obj2.entity.ObjectInSight.Add(obj22);
                                        }
                                    }
                                        break;
                                    case TTLHelper obj23:
                                    {
                                        if (obj23 != obj2)
                                        {
                                            var dist = SqrtSumSquare(obj23.location.Top - obj2.location.Top,
                                                obj23.location.Left - obj2.location.Left);
                                            if (dist < MAX_SIGHT_RANGE)
                                            {
                                                obj2.entity.ObjectInSight.Add(obj23);
                                            }
                                        }
                                    }
                                        break;
                                }
                            }
                        }
                            break;
                    }
                }
            });
        }

        /**
         * Function: Get the sum of squares of two double number
         * Input: double number x and y
         * Output: an double number
         */
        public static double SqrtSumSquare(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }
    }
}
