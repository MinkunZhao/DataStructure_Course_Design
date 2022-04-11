using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using static Ecosystem.GlobalObject;

namespace Ecosystem.algorithm
{
    public class Generator
    {
        public static double getDistance(Location a, Location b)
        {
            return Math.Sqrt(Math.Pow(a.Left - b.Left, 2) + Math.Pow(a.Top - b.Top, 2));
        }


        public static double Normal_Rand(double t1, double t2, double miu, double sigma)
        {
            return miu + sigma * Math.Sqrt(-2 * Math.Log(t1)) * Math.Sin(2 * Math.PI * t2);
        }


        public static List<LocationAndChoice> GetAllLocations(int number)
        {
            double Sum = ratioOfFirst + ratioOfSecond + ratioOfThird;
            double RTFirst = ratioOfFirst / Sum;
            double RTSecond = ratioOfSecond / Sum;
            double RTThird = ratioOfThird / Sum;
            int number_FirstTrophicLevel = Convert.ToInt32(number * RTFirst);
            int number_SecondTrophicLevel = Convert.ToInt32(number * RTSecond);
            int number_ThirdTrophicLevel = Convert.ToInt32(number * RTThird);
            List<LocationAndChoice> all_Loc = new List<LocationAndChoice>();  // target of return
            double tempX, tempY;
            Random rd = new Random();

            int minDistance_2and3;
            // the distance of 2nd and 3rd trophic levels's cluster should not be less than this value
            if (number <= 60)
                minDistance_2and3 = 90;
            else if (number <= 100)
                minDistance_2and3 = 85;
            else if (number <= 150)
                minDistance_2and3 = 80;
            else if (number <= 200)
                minDistance_2and3 = 75;
            else if (number <= 250)
                minDistance_2and3 = 70;
            else if (number <= 300)
                minDistance_2and3 = 65;
            else if (number <= 350)
                minDistance_2and3 = 60;
            else if (number <= 400)
                minDistance_2and3 = 55;
            else if (number < 450)
                minDistance_2and3 = 50;
            else if (number <= 500)
                minDistance_2and3 = 45;
            else if (number <= 550)
                minDistance_2and3 = 40;
            else if (number <= 600)
                minDistance_2and3 = 35;
            else if (number <= 650)
                minDistance_2and3 = 30;
            else if (number <= 700)
                minDistance_2and3 = 25;
            else
                minDistance_2and3 = 0;

            // As for the 1st trophic level
            for (int i = 0; i < number_FirstTrophicLevel; i++)
            {
                tempX = rd.NextDouble() * 990;
                tempY = rd.NextDouble() * 490;
                all_Loc.Add(new LocationAndChoice
                {
                    Location = new Location { Left = tempX, Top = tempY },
                    Choice = 0
                });
            }

            // As for the 2nd trophic level
            List<int> sizeOfGroups2 = new List<int>();
            int number_Remained = number_SecondTrophicLevel;
            double centerX, centerY;
            int size;
            while (number_Remained >= 15)
            {
                size = rd.Next(1, 15);
                sizeOfGroups2.Add(size);
                number_Remained -= size;
            }
            if (number_Remained > 0)
                sizeOfGroups2.Add(number_Remained);
            double t1, t2;  // temporary parameters: the random decimals
            List<Location> centerOfGroups2 = new List<Location>();
            for (int i = 0; i < sizeOfGroups2.Count; i++)
            {
                centerX = rd.NextDouble() * (990 - 2 * sizeOfGroups2[i] * 5) + sizeOfGroups2[i] * 5;
                centerY = rd.NextDouble() * (490 - 2 * sizeOfGroups2[i] * 5) + sizeOfGroups2[i] * 5;
                centerOfGroups2.Add(new Location { Left = centerX, Top = centerY });
                all_Loc.Add(new LocationAndChoice
                {
                    Location = new Location { Left = centerX, Top = centerY },
                    Choice = 1
                });
                for (int j = 1; j < sizeOfGroups2[i]; j++)
                {
                    t1 = rd.NextDouble();
                    t2 = rd.NextDouble();
                    tempX = Normal_Rand(t1, t2, centerX, 28);
                    if (tempX < 0)
                        tempX = 0;
                    else if (tempX > 990)
                        tempX = 990;
                    t1 = rd.NextDouble();
                    t2 = rd.NextDouble();
                    tempY = Normal_Rand(t1, t2, centerY, 28);
                    if (tempY < 0)
                        tempX = 0;
                    else if (tempY > 490)
                        tempY = 490;
                    // tempX = rd.NextDouble() * (2 * sizeOfGroups2[i] * 5) + (centerX - sizeOfGroups2[i] * 5);
                    // tempY = rd.NextDouble() * (2 * sizeOfGroups2[i] * 5) + (centerY - sizeOfGroups2[i] * 5);
                    all_Loc.Add(new LocationAndChoice
                    {
                        Location = new Location { Left = tempX, Top = tempY },
                        Choice = 1
                    });
                }
            }

            // As for the 3rd trophic level
            List<int> sizeOfGroups3 = new List<int>();
            number_Remained = number_ThirdTrophicLevel;
            while (number_Remained >= 10)
            {
                size = rd.Next(1, 10);
                sizeOfGroups3.Add(size);
                number_Remained -= size;
            }
            if (number_Remained > 0)
                sizeOfGroups3.Add(number_Remained);
            Location tempCenter;
            bool flag_OK;
            for (int i = 0; i < sizeOfGroups3.Count; i++)
            {
                do
                {
                    flag_OK = true;
                    centerX = rd.NextDouble() * (990 - 2 * sizeOfGroups3[i] * 7) + sizeOfGroups3[i] * 7;
                    centerY = rd.NextDouble() * (490 - 2 * sizeOfGroups3[i] * 7) + sizeOfGroups3[i] * 7;
                    tempCenter = new Location { Left = centerX, Top = centerY };
                    foreach (Location c in centerOfGroups2)
                    {
                        if (getDistance(tempCenter, c) <= minDistance_2and3)
                        {
                            flag_OK = false;
                            break;
                        }
                    }
                } while (!flag_OK);
                all_Loc.Add(new LocationAndChoice
                {
                    Location = new Location { Left = centerX, Top = centerY },
                    Choice = 2
                });
                for (int j = 1; j < sizeOfGroups3[i]; j++)
                {
                    t1 = rd.NextDouble();
                    t2 = rd.NextDouble();
                    tempX = Normal_Rand(t1, t2, centerX, 32);
                    if (tempX < 0)
                        tempX = 0;
                    else if (tempX > 990)
                        tempX = 990;
                    t1 = rd.NextDouble();
                    t2 = rd.NextDouble();
                    tempY = Normal_Rand(t1, t2, centerY, 32);
                    if (tempY < 0)
                        tempY = 0;
                    else if (tempY > 490)
                        tempY = 490;
                    // tempX = rd.NextDouble() * (2 * sizeOfGroups3[i] * 7) + (centerX - sizeOfGroups3[i] * 7);
                    // tempY = rd.NextDouble() * (2 * sizeOfGroups3[i] * 7) + (centerY - sizeOfGroups3[i] * 7);
                    all_Loc.Add(new LocationAndChoice
                    {
                        Location = new Location { Left = tempX, Top = tempY },
                        Choice = 2
                    });
                }
            }
            return all_Loc;
        }
    }
}