using ClassLibrary;
using Ecosystem.datastructure;
using Ecosystem.service;
using static Ecosystem.GlobalObject;
using static ClassLibrary.classes.Environment;
using ClassLibrary.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ecosystem.controller
{
    public class EnvironmentChanger
    {
        public static async void EnvironmentChange()
        {
            Random rd = new Random();
            double t = rd.NextDouble();
            if (t < 0.6)
                GlobalWeather = Weather.Sunny;
            else if (t < 0.73)
                GlobalWeather = Weather.Rainy;
            else if (t < 0.98)
                GlobalWeather = Weather.Windy;
            else
                GlobalWeather = Weather.Snowy;

            FirstNutritionalLevel.currentWeather_Flag = (int)GlobalWeather;
        }
        
        public static async void Propagate()
        {
            var proloc = await PropagationFn();
            foreach (var locationAndChoice in proloc)
            {
                int choice = locationAndChoice.Choice;
                double left = locationAndChoice.Location.Left;
                double top = locationAndChoice.Location.Top;
                switch (choice)
                {
                    case 0:
                        CreateShape(Brushes.Green, left, top);
                        break;
                    case 1:
                        CreateShape(Brushes.Blue, left, top);
                        break;
                    case 2:
                        CreateShape(Brushes.Red, left, top);
                        break;
                }
            }
        }
        

        //The function is used for propagation.The function will return the locations of the newly built entities.
        public static Task<List<LocationAndChoice>> PropagationFn()
        {
            //This defines three list for storing the locations of the entities for three different trophic levels.
            List<List<Location>> locationsByType = new List<List<Location>>(3);
            for (int i = 0; i < 3; i++)
            {
                locationsByType.Add(new List<Location>());
            }

            //Distinguish the three kinds of entities according to their type.
            foreach (object o in AllAnimal)
            {
                switch (o)
                {
                    case FNLHelper o1:
                    {
                        FirstNutritionalLevel entity = o1.entity;
                        if (!entity.flag_OKToBread)
                             continue;
                        locationsByType[0].Add(o1.location);
                    }
                        break;
                    case STLHelper o2:
                    {
                        SecondTrophicLevel entity = o2.entity;
                        if (entity.Age < 0.1 * SecondTrophicLevel.MAX_AGE ||
                            entity.Age > 0.9 * SecondTrophicLevel.MAX_AGE)
                            continue;
                        Location lc = o2.location;
                        locationsByType[1].Add(lc);
                    }
                        break;
                    case TTLHelper o3:
                    {
                        ThirdTrophicLevel entity = o3.entity;
                        if (entity.Age < 0.25 * ThirdTrophicLevel.MAX_AGE ||
                            entity.Age > 0.75 * ThirdTrophicLevel.MAX_AGE)
                            continue;
                        Location lc = o3.location;
                        locationsByType[2].Add(lc);
                    }
                        break;
                }
                /*
                if (o.GetType() == typeof(FNLHelper))
                {
                    Location lc = ((FNLHelper)o).location;
                    locationsByType[0].Add(lc);
                }
                else if (o.GetType() == typeof(STLHelper))
                {
                    SecondTrophicLevel entity = ((STLHelper)o).entity;
                    if (entity.Age < 0.3 * SecondTrophicLevel.MAX_AGE ||
                        entity.Age > 0.7 * SecondTrophicLevel.MAX_AGE)
                        continue;
                    Location lc = ((STLHelper)o).location;
                    locationsByType[1].Add(lc);
                }
                else
                {
                    ThirdTrophicLevel entity = ((TTLHelper)o).entity;
                    if (entity.Age < 0.3 * ThirdTrophicLevel.MAX_AGE ||
                        entity.Age > 0.7 * ThirdTrophicLevel.MAX_AGE)
                        continue;
                    Location lc = ((TTLHelper)o).location;
                    locationsByType[2].Add(lc);
                }
                */

            }

            List<LocationAndChoice> result = new List<LocationAndChoice>();

            /*
             * Call the function GetNewPositions for each of the three nutrient levels to get the locations of
             * the new entities and return the locations and the level as type LocationAndChoice.
             */
            var random = new Random();
            for (int i = 0; i < locationsByType.Count; i++)
            {
                List<Location> locations = GetNewPositions(locationsByType[i], i, 40);
                foreach (Location location in locations)
                {
                    LocationAndChoice lc = new LocationAndChoice {Choice = i, Location = location};
                    result.Add(lc);
                }

                //for the first trophic level, randomly create new entities in some places to keep them scattered.
                if (i == 0)
                {
                    //繁殖概率与当前种群数量有关，数量越多繁殖概率越低，防止无脑增殖。
                    double sum = ratioOfFirst + ratioOfSecond + ratioOfThird;
                    double poss = 0.25 / (FirstNutritionalLevel.Count * sum / ratioOfFirst / Number);
                    int num = (int) (locationsByType[0].Count * poss);
                    for (int j = 0; j < num; j++)
                    {
                        LocationAndChoice lac = new LocationAndChoice
                        {
                            Choice = i,
                            Location = new Location
                            {
                                Left = random.Next(0, 990),
                                Top = random.Next(0, 490)
                            }
                        };
                        result.Add(lac);
                    }
                }
            }

            return Task.FromResult(result);
        }
        

        //This function is used for get the new locations in different level.
        public static List<Location> GetNewPositions(List<Location> locations, int level, int radius = 30)
        {
            List<Location> result = new List<Location>();
            //For the same species, using Disjoint Set Union to get different groups according to the root node
            Dictionary<int, List<Location>> group = Group(locations, radius);


            /*
             *For every group, the proability for propagtion is from 0.3 to 0.7.
             *The higher the number, the higher the probability.
             *For the first trophic level, the proability is just 0.3. 
             */
            var rd = new Random();
            double poss = 0;
            double sum = ratioOfFirst + ratioOfSecond + ratioOfThird;
            if (level == 0)
            {
                //繁殖概率与当前种群数量有关，数量越多繁殖概率越低，防止无脑增殖。
                poss = 0.25 / (FirstNutritionalLevel.Count * sum / ratioOfFirst / Number);
                Console.WriteLine(poss);
            }
            else if (level == 1)
            {
                //繁殖概率与当前种群数量有关，数量越多繁殖概率越低，防止无脑增殖。
                poss = 1 / (SecondTrophicLevel.Count * sum / ratioOfSecond / Number);
            }
            else
            {
                //繁殖概率与当前种群数量有关，数量越多繁殖概率越低，防止无脑增殖。
                poss = 1 / (ThirdTrophicLevel.Count * sum / ratioOfThird / Number);
            }
            foreach (int i in group.Keys)
            {
                List<Location> locationsByGroup = group[i];
                for (int j = 0; j < locationsByGroup.Count - 1; j += 2)
                {
                    if (rd.NextDouble() < poss)
                    {
                        /*
                         * For the two entities, it is judged whether they will engage in propagation behavior 
                         * according to the proability,and the location of the new entity is the midpoint.
                         */
                        Location location = new Location
                        {
                            Left = (locationsByGroup[j].Left + locationsByGroup[j + 1].Left) / 2,
                            Top = (locationsByGroup[j].Top + locationsByGroup[j + 1].Top) / 2
                        };
                        result.Add(location);
                    }
                }
            }
            return result;
        }

        //This function is used for distinguish between different groups within the same species based on root nodes.
        public static Dictionary<int, List<Location>> Group(List<Location> position, int radius)
        {
            Dictionary<int, List<Location>> result = new Dictionary<int, List<Location>>();

            //Define the PriorityQueue for the edges.
            PriorityQueue<Edge> priorityQueue = new PriorityQueue<Edge>(position.Count * (position.Count - 1),
                                                                        new EdgeComparer());

            //For edges, only the edges with the distance less than radius can be added in the PriorityQueue.
            for (int i = 0; i < position.Count; i++)
            {
                for (int j = i + 1; j < position.Count; j++)
                {
                    Location point1 = position[i];
                    Location point2 = position[j];
                    double dis = Math.Sqrt(Math.Pow(point1.Left - point2.Left, 2) +
                                           Math.Pow(point1.Top - point2.Top, 2));
                    if (dis <= radius)
                    {
                        Edge edge = new Edge { from = i, to = j, distance = dis };
                        priorityQueue.Push(edge);
                    }
                }
            }

            //visited array is used for judging whether a location has been detected.
            bool[] visited = new bool[position.Count];

            //The list records the index value of locations. The points recorded first form a shorter edge.
            List<int> visitedIndexByOrder = new List<int>();
            GenTree tree = new GenTree(position.Count);

            /*
             * Using the principle similar to minimum spanning tree.
             * Finding the root node of each location and updating the tree
             */
            while (!priorityQueue.Empty())
            {
                Edge edge = priorityQueue.Pop();
                int from = edge.from;
                int to = edge.to;
                if (!visited[from]) { visitedIndexByOrder.Add(from); visited[from] = true; }
                if (!visited[to]) { visitedIndexByOrder.Add(to); visited[to] = true; }
                if (tree.Differ(from, to)) tree.Union(from, to);
            }

            /*
             * Each location is divided into different groups.
             * Put the locations into the corresponding group list according to the root nodes.
             */
            foreach (int i in visitedIndexByOrder)
            {
                int root = tree.Find(i);
                if (result.ContainsKey(root))
                    result[root].Add(position[i]);
                else
                {
                    result.Add(root, new List<Location>());
                    result[root].Add(position[i]);
                }
            }
            return result;
        }
    }
}
