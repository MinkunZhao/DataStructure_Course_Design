using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes;
public partial class ThirdTrophicLevel
{
    public static int Count = 0;
    public const int MAX_AGE = 15;
    public const double MAX_ENERGY = 1000;
    public const double MAX_TIREDNESS = 100;
    public const int THRES_AGE = MAX_AGE / 2;
    public const double THRES_ENERGY = MAX_ENERGY * 0.6;
    public const double THRES_TIRENESS = MAX_TIREDNESS * 0.6;

    public int Age { get; set; }
    public double Energy { get; set; }
    public double Tiredness { get; set; }
    public double State
    {
        get
        {
            if (Age <= THRES_AGE)
                return Energy * 0.1 + 0.2 * Age - 0.4 * Tiredness;
            else
                return Energy * 0.1 - 0.2 * Age - 0.4 * Tiredness;
        }
    }
    public List<object> ObjectInSight = new();
}

