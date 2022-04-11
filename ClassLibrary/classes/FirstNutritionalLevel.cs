using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes;
public partial class FirstNutritionalLevel
{
    public static int Count = 0;
    public const int MAX_AGE = 15;
    public const int THRES_ILLUMINATION = 10;   // least illumination storage for propagation
    public const int THRES_MOISTURE = 15;   // least moisture storage for propagation
    public const double ENERGY_BRINGTO_STL = 100;

    public bool flag_OKToBread;

    public static int currentWeather_Flag;

    public int Age { get; set; }
    public int Illumination { get; set; }
    public int Moisture { get; set; }

    public void StateUpdate()
    {
        if (currentWeather_Flag == 1)    // sunny
        {
            Illumination += 10;
            Moisture -= 3;
        }
        else if (currentWeather_Flag == 2)   // rainy
        {
            Illumination -= 6;
            Moisture += 15;
        }
        else if (currentWeather_Flag == 4) // windy
        {
            Illumination -= 4;
            Moisture -= 2;
        }
        else    // snowy
        {
            Illumination -= 8;
            Moisture += 5;
        }

        if (Age >= 3 && Age <= 13 && Illumination >= THRES_ILLUMINATION && Moisture >= THRES_MOISTURE)
            flag_OKToBread = true;
        else
            flag_OKToBread = false;
    }
}