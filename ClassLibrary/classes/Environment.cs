using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes;
public class Environment
{
    [Flags]
    public enum Weather
    {
        Sunny = 1,  // probability: 60%
        Rainy = 2,  // probability: 13%
        Windy = 4,  // probability: 25%
        Snowy = 8   // probability: 2%
    }
}
