using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary;
public struct LocationAndChoice
{
    public Location Location;
    public int Choice;
}
public struct Location
{
    public double Left;
    public double Top;
}

public struct Motion
{
    public double X;
    public double Y;
    public double Velocity; 
}
