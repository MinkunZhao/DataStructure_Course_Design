using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClassLibrary.classes;
using Ecosystem.service;
using static Ecosystem.GlobalObject;
using ClassLibrary;

namespace Ecosystem.controller
{
    public class Chooser
    {
        /**
         * Function: let every animal in screen choose an action and act it
         * Input: Empty
         * Output: Empty
         */
        public static async Task ChooseAndAction()
        {
            foreach (var o in AllAnimal)
            {
                switch (o)
                {
                    case STLHelper sobj:
                        STLChooser(sobj);
                        break;
                    case TTLHelper tobj:
                        TTLChooser(tobj);
                        break;
                }
            }
            //return Task.CompletedTask;
        }

        /**
         * Function: Realize the action decision of the current second nutritional level entity.
         * Input: The object presents the entity at second nutritional level.
         * Output: Empty
         */
        public static void STLChooser(STLHelper sobj)
        {
            //First save the entities in objectinsight according to their level, and judge according to this
            List<FNLHelper> firstLevels = new List<FNLHelper>();
            List<STLHelper> secondLevels = new List<STLHelper>();
            List<TTLHelper> thirdLevels = new List<TTLHelper>();
            double sum = ratioOfFirst + ratioOfSecond + ratioOfThird;
            foreach (var o in sobj.entity.ObjectInSight)
            {
                switch (o)
                {
                    case FNLHelper first:
                    {
                        firstLevels.Add(first);
                    }
                        break;

                    case STLHelper second:
                    {
                        secondLevels.Add(second);
                    }
                        break;

                    case TTLHelper third:
                    {
                        thirdLevels.Add(third);
                    }
                        break;
                }
            }
            //If there is a third nutritional level entity (escape)
            if (thirdLevels.Count != 0)
            {
                sobj.current_action = GlobalObject.Action.Escape;
                STLEscape(sobj);
            }
            //Tiredness is high and have a rest.
            else if (sobj.entity.Tiredness > SecondTrophicLevel.THRES_TIRENESS)
            {
                sobj.current_action = GlobalObject.Action.Stay;
                STLStay(sobj);
            }
            //Lack of energy, hunt.
            else if (sobj.entity.Energy < SecondTrophicLevel.THRES_ENERGY)
            {
                /*First judge the quantity ratio between the second nutritional level and the first nutritional level.
                 * If there is much more entities at second nutritional level, 
                 * stop predation to prevent the number of first nutritional level decreases quickly.
                 */
                if (SecondTrophicLevel.Count * 1.0 / FirstNutritionalLevel.Count < 2 * ratioOfFirst / ratioOfSecond)
                {
                    sobj.current_action = GlobalObject.Action.Hunt;
                    STLHunt(sobj);
                }
                else
                {
                    sobj.current_action = GlobalObject.Action.Run;
                    STLRun(sobj);
                }
            }
            /*When the proportion of the number of nutrition level the current entity belongs to is small,
             *the aggregation mechanism is triggered.
             */
            else if ((double)SecondTrophicLevel.Count / AllAnimal.Count < ratioOfSecond / (sum + 1))
            {
                sobj.current_action = GlobalObject.Action.GetTogether;
                STLGetTogether(sobj);
            }
            // Random action
            else
            {
                var rand = new Random();
                if (rand.NextDouble() < 0.3)
                {
                    sobj.current_action = GlobalObject.Action.Seek;
                    STLSeek(sobj);
                }
                else if(rand.NextDouble() < 0.5)
                {
                    sobj.current_action = GlobalObject.Action.GetTogether;
                    STLGetTogether(sobj);
                }
                else
                {
                    sobj.current_action = GlobalObject.Action.Run;
                    STLRun(sobj);
                }
            }
        }

        /**
         * Function: Realize the action decision of the current third nutritional level entity.
         * Input: The object presents the entity at second nutritional level.
         * Output: Empty
         */
        public static void TTLChooser(TTLHelper tobj)
        {
            double sum = ratioOfFirst + ratioOfSecond + ratioOfThird;
            if (tobj.entity.Tiredness > ThirdTrophicLevel.THRES_TIRENESS)    // it is tired
            {
                tobj.current_action = GlobalObject.Action.Stay;
                TTLStay(tobj);
            }

            else if (tobj.entity.Energy < ThirdTrophicLevel.THRES_ENERGY)   // it is hungry
            {
                if (ThirdTrophicLevel.Count * 1.0 / SecondTrophicLevel.Count < 2 * ratioOfThird / ratioOfSecond)
                {
                    tobj.current_action = GlobalObject.Action.Hunt;
                    TTLHunt(tobj);
                }
                else // If the number of the 3rd trophic level is much more than that of the 2nd trophic level, then the predators let the preys go temporarily, otherwise the 2nd nutritional level will be eaten up
                {
                    tobj.current_action = GlobalObject.Action.Run;
                    TTLRun(tobj);
                }
            }

            else    // Neither tired or hungry
            {
                int companionInSight = 0;   // the number of TTL (its companion) in its sight
                foreach (var o in tobj.entity.ObjectInSight)
                {
                    if (o is TTLHelper) companionInSight++;
                }
                /* Compare the current quantity ratio of the this species with the initial one (default: 5:3:2).
                 * If it is greatly smaller than the initial ratio, then this species choose to get together.
                 * Because aggregation is beneficial for the animals to propagate and survive.
                 */
                if ((double)ThirdTrophicLevel.Count / AllAnimal.Count < ratioOfThird / (sum + 1))   // there are many companions in its sight
                {
                    tobj.current_action = GlobalObject.Action.GetTogether;
                    TTLGetTogether(tobj);
                }
                // Random action
                else
                {
                    var rand = new Random();
                    if (rand.NextDouble() < 0.3)
                    {
                        tobj.current_action = GlobalObject.Action.Seek;
                        TTLSeek(tobj);
                    }
                    else if (rand.NextDouble() < 0.5)
                    {
                        tobj.current_action = GlobalObject.Action.GetTogether;
                        TTLGetTogether(tobj);
                    }
                    else
                    {
                        tobj.current_action = GlobalObject.Action.Run;
                        TTLRun(tobj);
                    }
                }
            }
        }

        /**
         * Function: The entity will only stay where it is and will not move.
         * Input: The object presents the entity at second nutritional level.
         * Output: Empty
         */
        public static void STLStay(STLHelper stl)
        {
            stl.entity.Tiredness = 0;
        }

        /**
         * Function: The entity will only stay where it is and will not move.
         * Input: The object presents the entity at third nutritional level.
         * Output: Empty
         */
        public static void TTLStay(TTLHelper ttl)
        {
            ttl.entity.Tiredness = 0;
        }

        /**
         * Function: (Aggregation Function) Control the second nutritional level's animals to get together.
         * Input: The object presents the entity at second nutritional level.
         * Output: Empty
         */
        public static void STLGetTogether(STLHelper stl)
        {
            Random rd = new Random();
            int companionInSight = 0;
            double centerX = 0;
            double centerY = 0;
            foreach (var o in stl.entity.ObjectInSight)
            {
                if (o is STLHelper)
                {
                    companionInSight++;
                    STLHelper temp = o as STLHelper;
                    centerX += temp.location.Left;
                    centerY += temp.location.Top;
                }
            }
            centerX = (centerX + stl.location.Left) / (companionInSight + 1);
            centerY = (centerY + stl.location.Top) / (companionInSight + 1);
            double targetX = rd.NextDouble() * 2 * 0.5 * MAX_SIGHT_RANGE + centerX - 0.5 * MAX_SIGHT_RANGE;
            double targetY = rd.NextDouble() * 2 * 0.5 * MAX_SIGHT_RANGE + centerY - 0.5 * MAX_SIGHT_RANGE;

            // speed threshold value
            double V0 = 0;
            switch (SecondChoice)
            {
                case SecondLevel.Horse:
                    V0 = 35;
                    break;
                case SecondLevel.Rabbit:
                    V0 = 30;
                    break;
                case SecondLevel.Sheep:
                    V0 = 33;
                    break;
            }
            // calculated max speed according to the formula (set parameter a to 0.1)
            double Vmax = V0 + 0.1 * stl.entity.State;
            // select a random speed between 0.7 times threshold speed and the max speed as the current speed
            // Since the moving time is 1 second, the value of movement equals to the value of speed
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            // speed cannot more than 20
            movement = movement > 20 ? 20 : movement;
            // speed cannot less than 0.7V0
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            // move towards the center of target
            double dis = SqrtSumSquare(targetX - stl.location.Left, targetY - stl.location.Top);
            double LeftOffset = (targetX - stl.location.Left) * movement / dis;
            double TopOffset = (targetY - stl.location.Top) * movement / dis;
 

            // Out of the map's bound is not allowed.
            if (stl.location.Left + LeftOffset < 0)
            {
                LeftOffset = 0 - stl.location.Left;
            }
            if (stl.location.Left + LeftOffset > 990)
            {
                LeftOffset = 990 - stl.location.Left;
            }
            if (stl.location.Top + TopOffset < 0)
            {
                TopOffset = 0 - stl.location.Top;
            }
            if (stl.location.Top + TopOffset > 490)
            {
                TopOffset = 490 - stl.location.Top;
            }
            Moving(stl, LeftOffset,TopOffset);


            // Update the energy and tiredness here directly because passing parameters such as movement is quite hard.
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
        }

        /**
         * Function: (Aggregation Function) Control the second nutritional level's animals to get together.
         * Input: The object presents the entity at second nutritional level.
         * Output: Empty
         */
        public static void TTLGetTogether(TTLHelper ttl)
        {
            Random rd = new Random();
            int companionInSight = 0;
            double centerX = 0;
            double centerY = 0;
            foreach (var o in ttl.entity.ObjectInSight)
            {
                if (o is TTLHelper)
                {
                    companionInSight++;
                    TTLHelper temp = o as TTLHelper;
                    centerX += temp.location.Left;
                    centerY += temp.location.Top;
                }
            }
            centerX = (centerX + ttl.location.Left) / (companionInSight + 1);
            centerY = (centerY + ttl.location.Top) / (companionInSight + 1);
            double targetX = rd.NextDouble() * 2 * 0.5 * MAX_SIGHT_RANGE + centerX - 0.5 * MAX_SIGHT_RANGE;
            double targetY = rd.NextDouble() * 2 * 0.5 * MAX_SIGHT_RANGE + centerY - 0.5 * MAX_SIGHT_RANGE;

            // speed threshold value (greater than that of the 2nd trophic level in order to ensure the predators can catch up with the preys)
            double V0 = 0;
            switch (ThirdChoice)
            {
                case ThirdLevel.Tiger:
                    V0 = 45;
                    break;
                case ThirdLevel.Wolf:
                    V0 = 40;
                    break;
            }
            // calculated max speed according to the formula (set parameter a to 0.1)
            double Vmax = V0 + 0.1 * ttl.entity.State;
            // select a random speed between 0.7 times threshold speed and the max speed as the current speed
            // Since the moving time is 1 second, the value of movement equals to the value of speed
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            // speed cannot more than 25
            movement = movement > 25 ? 25 : movement;
            // speed cannot less than 0.7V0
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            // move towards the center of target
            double dis = SqrtSumSquare(targetX - ttl.location.Left, targetY - ttl.location.Top);
            double LeftOffset = (targetX - ttl.location.Left) * movement / dis;
            double TopOffset = (targetY - ttl.location.Top) * movement / dis;

            // Out of the map's bound is not allowed.
            if (ttl.location.Left + LeftOffset < 0)
            {
                LeftOffset = 0 - ttl.location.Left;
            }
            if (ttl.location.Left + LeftOffset > 990)
            {
                LeftOffset = 990 - ttl.location.Left;
            }
            if (ttl.location.Top + TopOffset < 0)
            {
                TopOffset = 0 - ttl.location.Top;
            }
            if (ttl.location.Top + TopOffset > 490)
            {
                TopOffset = 490 - ttl.location.Top;
            }
            Moving(ttl, LeftOffset, TopOffset);


            // Update the energy and tiredness here directly because passing parameters such as movement is quite hard.
            ttl.entity.Energy -= movement;
            ttl.entity.Tiredness += movement * 0.1;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
        }

        /**
         * Function: Realize the escape mechanism of the entity at second nutritional level.
         * Input: The object represents the entity at second nutritional level.
         * Output: Empty
         */
        public static void STLEscape(STLHelper stl)
        {
            //Calculate the velocity according to the formula.
            double V0 = 0;
            switch (SecondChoice)
            {
                case SecondLevel.Horse:
                    V0 = 35;
                    break;
                case SecondLevel.Rabbit:
                    V0 = 30;
                    break;
                case SecondLevel.Sheep:
                    V0 = 33;
                    break;
            }
            double Vmax = V0 + 0.1 * stl.entity.State;
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //Control the range of the velocity.
            movement = movement > 20 ? 20 : movement;
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;
            List<TTLHelper> ttls = new List<TTLHelper>();
            foreach (var obj in stl.entity.ObjectInSight)
            {
                switch (obj)
                {
                    case TTLHelper third:
                        {
                            ttls.Add(third);
                        }
                    break;
                }
            }
            TTLHelper predator = ttls[0];
            double disOfMaxIndex = 50;
            /*According to the location of predators and their state,
             *find the predator that is most likely to catch up with the current entity.
             */
            for (int i = 0; i < ttls.Count; i++)
            {

                double dis = SqrtSumSquare(stl.location.Left - ttls[i].location.Left, stl.location.Top - ttls[i].location.Top);
                if ((ttls[i].entity.State * 0.3 - dis * 0.7) > (predator.entity.State * 0.3 - disOfMaxIndex * 0.7))
                {
                    predator = ttls[i];
                    disOfMaxIndex = dis;
                }
            }

            //Move away from the predator found above
            double Left = (stl.location.Left - predator.location.Left) * movement / disOfMaxIndex + stl.location.Left;
            double Top = (stl.location.Top - predator.location.Top) * movement / disOfMaxIndex + stl.location.Top;
            stl.entity.Energy -= movement;
            stl.entity.Tiredness += movement * 0.5;
            if (0 <= Left && Left <= 990 && 0 <= Top && Top <= 490) { Moving(stl, Left - stl.location.Left, Top - stl.location.Top); return; }

            //Check for crossing the boundaries. If so, change the direction to move.
            List<Location> locations = new List<Location>();
            double angle = Math.Atan2(stl.location.Top - predator.location.Top, stl.location.Left - predator.location.Left) * 180 / Math.PI;
            for(int i = 0; i <=150; i+=30)
            {
                double left = movement * Math.Cos((angle + i) * Math.PI / 180) + stl.location.Left;
                double top = movement * Math.Sin((angle + i) * Math.PI / 180) + stl.location.Top;
                if (0 <= left && left <= 990 && 0 <= top && top <= 490)
                {
                    Moving(stl, left - stl.location.Left, top - stl.location.Top);
                    return;
                }
                left = movement * Math.Cos((angle - i) * Math.PI / 180) + stl.location.Left;
                top = movement * Math.Sin((angle - i) * Math.PI / 180) + stl.location.Top;
                if (0 <= left && left <= 990 && 0 <= top && top <= 490)
                {
                    Moving(stl, left - stl.location.Left, top - stl.location.Top);
                    return;
                }
            }

            //Update the state.
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
        }

        /**
         * Function: Get the sum of squares of two double number
         * Input: double number x and y
         * Output: an double number
         */
        public static double SqrtSumSquare(double x, double y) => Math.Sqrt(x * x + y * y);

        /**
         * Function: Control the second nutritional level's animals to go hunting
         * Input: The object represents the entity at second nutritional level.
         * Output: Empty
         */
        public static void STLHunt(STLHelper stl)
        {
            List<FNLHelper> alternativeGrass = new List<FNLHelper>();
            foreach (var o in stl.entity.ObjectInSight)
            {
                if (o is FNLHelper)
                    alternativeGrass.Add(o as FNLHelper);
            }
            int targetGrass_index = 0;

            // If there is no plant in its sight, it expands its searching range.
            if (alternativeGrass.Count == 0)
            {
                STLSeek(stl);
                return;
            }
            // Select the plant that is closest to itself as its hunting target.
            double min_distance = SqrtSumSquare(stl.location.Left - alternativeGrass[0].location.Left, stl.location.Top - alternativeGrass[0].location.Top);
            if (alternativeGrass.Count >= 2)
            {
                for (int i = 1; i < alternativeGrass.Count; i++)
                {
                    if (SqrtSumSquare(stl.location.Left - alternativeGrass[i].location.Left, stl.location.Top - alternativeGrass[i].location.Top) < min_distance)
                    {
                        targetGrass_index = i;
                        min_distance = SqrtSumSquare(stl.location.Left - alternativeGrass[i].location.Left, stl.location.Top - alternativeGrass[i].location.Top);
                    }
                }
            }

            //Calculate the velocity according to the formula.
            double V0 = 0;
            switch (SecondChoice)
            {
                case SecondLevel.Horse:
                    V0 = 35;
                    break;
                case SecondLevel.Rabbit:
                    V0 = 30;
                    break;
                case SecondLevel.Sheep:
                    V0 = 33;
                    break;
            }
            double Vmax = V0 + 0.1 * stl.entity.State;
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //Control the range of the velocity.
            movement = movement > 20 ? 20 : movement;
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //The current entity move towards the direction of the target prey according to the proportion.
            double LeftOffset = (alternativeGrass[targetGrass_index].location.Left - stl.location.Left) * movement / min_distance;
            double TopOffset = (alternativeGrass[targetGrass_index].location.Top - stl.location.Top) * movement / min_distance;
            //Check for crossing boundaries 
            if (stl.location.Left + LeftOffset < 0)
            {
                LeftOffset = 0 - stl.location.Left;
            }
            if (stl.location.Left + LeftOffset > 990)
            {
                LeftOffset = 990 - stl.location.Left;
            }
            if (stl.location.Top + TopOffset < 0)
            {
                TopOffset = 0 - stl.location.Top;
            }
            if (stl.location.Top + TopOffset > 490)
            {
                TopOffset = 490 - stl.location.Top;
            }
            Moving(stl, LeftOffset, TopOffset);
            Chase21(stl, alternativeGrass[targetGrass_index]);

            // Update
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness=stl.entity.Tiredness < 0 ? 0:stl.entity.Tiredness;
        }

        /**
         * Function: Control the third nutritional level's animals to go hunting
         * Input: The object represents the entity at third nutritional level.
         * Output: Empty
         */
        public static void TTLHunt(TTLHelper ttl)
        {
            List<STLHelper> alternativePreys = new List<STLHelper>();   // preys that can be chosen as the hunting target
            foreach (var o in ttl.entity.ObjectInSight)
            {
                if (o is STLHelper)
                    alternativePreys.Add(o as STLHelper);
            }
            int targetPrey_index = 0;
            // If there is no prey in its sight, it expands its searching range.
            if (alternativePreys.Count == 0)
            {
                TTLSeek(ttl);
                return;
            }

            if (ttl.entity.Energy < ThirdTrophicLevel.THRES_ENERGY * 0.4)   // extremely hungry; 参数0.4后期调整
            {
                // Now it is extremely hungry. Thus it chooses the prey that is closest to itself in order to get some food as soon as possible.
                double min_distance = SqrtSumSquare(ttl.location.Left - alternativePreys[0].location.Left, ttl.location.Top - alternativePreys[0].location.Top);
                if (alternativePreys.Count >= 2)
                {
                    for (int i = 1; i < alternativePreys.Count; i++)
                    {
                        if (SqrtSumSquare(ttl.location.Left - alternativePreys[i].location.Left, ttl.location.Top - alternativePreys[i].location.Top) < min_distance)
                        {
                            targetPrey_index = i;
                            min_distance = SqrtSumSquare(ttl.location.Left - alternativePreys[i].location.Left, ttl.location.Top - alternativePreys[i].location.Top);
                        }
                    }
                }
            }
            else    // hungry but not too hungry
            {
                // Now it is slightly hungry. Thus it chooses the prey with the poorest state as its hunting target, which is beneficial for sustainable development.
                double least_state = alternativePreys[0].entity.State;
                if (alternativePreys.Count >= 2)
                {
                    for (int i = 1; i < alternativePreys.Count; i++)
                    {
                        if (alternativePreys[i].entity.State < least_state)
                        {
                            targetPrey_index = i;
                            least_state = alternativePreys[i].entity.State;
                        }
                    }
                }
            }
            //Calculate the velocity according to the formula.
            double V0 = 0;
            switch (ThirdChoice)
            {
                case ThirdLevel.Tiger:
                    V0 = 45;
                    break;
                case ThirdLevel.Wolf:
                    V0 = 40;
                    break;
            }
            double Vmax = V0 + 0.1 * ttl.entity.State;
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //Control the range of the velocity.
            movement = movement > 25 ? 25 : movement;
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //Calculate the distance
            double dis = SqrtSumSquare(alternativePreys[targetPrey_index].location.Left - ttl.location.Left, alternativePreys[targetPrey_index].location.Top - ttl.location.Top);

            //The current entity move towards the direction of the target prey according to the proportion.
            double LeftOffset = (alternativePreys[targetPrey_index].location.Left - ttl.location.Left) * movement / dis;
            double TopOffset = (alternativePreys[targetPrey_index].location.Top - ttl.location.Top) * movement / dis;
            //Check for crossing boundaries 
            if (ttl.location.Left + LeftOffset < 0)
            {
                LeftOffset = 0 - ttl.location.Left;
            }
            if (ttl.location.Left + LeftOffset > 990)
            {
                LeftOffset = 990 - ttl.location.Left;
            }
            if (ttl.location.Top + TopOffset < 0)
            {
                TopOffset = 0 - ttl.location.Top;
            }
            if (ttl.location.Top + TopOffset > 490)
            {
                TopOffset = 490 - ttl.location.Top;
            }
            Moving(ttl, LeftOffset, TopOffset);
            Chase32(ttl, alternativePreys[targetPrey_index]);

            //Update
            ttl.entity.Energy -= movement;
            ttl.entity.Tiredness += movement * 0.1;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
        }

        /**
         * Function:When there is no prey in the visiom,use this function to search for prey outward. Other settings are the same as Hunt funtion.
         * Input: The object represents the entity at second nutritional level.
         * Output: Empty
         */
        public static void STLSeek(STLHelper stl)
        {
            FNLHelper prey = null;
            double disOfMiniIndex = 50;
            //Search the prey in the expanded vision.
            foreach (var o in GlobalObject.AllAnimal)
            {
                if (o is FNLHelper)
                {
                    FNLHelper first = o as FNLHelper;
                    if (prey == null)
                    {
                        prey = first;
                        disOfMiniIndex = SqrtSumSquare(stl.location.Left - prey.location.Left, stl.location.Top - prey.location.Top);
                        continue;
                    }
                    if (SqrtSumSquare(stl.location.Left - prey.location.Left, stl.location.Top - prey.location.Top) >
                        SqrtSumSquare(stl.location.Left - first.location.Left, stl.location.Top - first.location.Top))
                    {
                        prey = first;
                        disOfMiniIndex = SqrtSumSquare(stl.location.Left - first.location.Left, stl.location.Top - prey.location.Top);
                    }
                }
            }
            //Calculate the velocity of the entity based on the formula. 
            double V0 = 0;
            switch (SecondChoice)
            {
                case SecondLevel.Horse:
                    V0 = 35;
                    break;
                case SecondLevel.Rabbit:
                    V0 = 30;
                    break;
                case SecondLevel.Sheep:
                    V0 = 33;
                    break;
            }
            double Vmax = V0 + 0.1 * stl.entity.State;
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //Control the range of the velocity.
            movement = movement > 20 ? 20 : movement;
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;
            if (prey == null)
            {
                STLStay(stl);
                return;
            }
            double LeftOffset = (prey.location.Left - stl.location.Left) * movement / disOfMiniIndex;
            double TopOffset = (prey.location.Top - stl.location.Top) * movement / disOfMiniIndex;
            Moving(stl, LeftOffset, TopOffset);
            //Update some parameters.
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
        }

        /**
         * Function:When there is no prey in the visiom,use this function to search for prey outward. Other settings are the same as Hunt funtion.
         * Input: The object represents the entity at third nutritional level.
         * Output: Empty
         */
        public static void TTLSeek(TTLHelper ttl)
        {
            STLHelper prey = null;
            double disOfMiniIndex = 50;
            //Search the prey in the expanded vision.
            foreach (var o in GlobalObject.AllAnimal)
            {
                if (o is STLHelper)
                {
                    STLHelper first = o as STLHelper;
                    if (prey == null)
                    {
                        prey = first;
                        disOfMiniIndex = SqrtSumSquare(ttl.location.Left - prey.location.Left, ttl.location.Top - prey.location.Top);
                        continue;
                    }
                    if (SqrtSumSquare(ttl.location.Left - prey.location.Left, ttl.location.Top - prey.location.Top) >
                        SqrtSumSquare(ttl.location.Left - first.location.Left, ttl.location.Top - first.location.Top))
                    {
                        prey = first;
                        disOfMiniIndex = SqrtSumSquare(ttl.location.Left - first.location.Left, ttl.location.Top - prey.location.Top);
                    }
                }
            }
            //Calculate the velocity of the entity based on the formula. 
            double V0 = 0;
            switch (ThirdChoice)
            {
                case ThirdLevel.Tiger:
                    V0 = 45;
                    break;
                case ThirdLevel.Wolf:
                    V0 = 40;
                    break;
            }
            double Vmax = V0 + 0.1* ttl.entity.State;
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //Control the range of the velocity.
            movement = movement > 25 ? 25 : movement;
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;
            if (prey == null)
            {
                TTLStay(ttl);
                return;
            }
            double LeftOffset = (prey.location.Left - ttl.location.Left) * movement / disOfMiniIndex;
            double TopOffset = (prey.location.Top - ttl.location.Top) * movement / disOfMiniIndex;
            Moving(ttl, LeftOffset, TopOffset);
            //Update some parameters.
            ttl.entity.Energy -= movement;
            ttl.entity.Tiredness += movement * 0.1;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
        }

        /**
         * Function: Realize the random movement for the entity if they don't choose any other actions.
         * Input: The object represents the entity at second nutritional level.
         * Output: Empty
         */
        public static void STLRun(STLHelper stl)
        {
            var rand = new Random();
            //Set the speed threshold to 15
            double V0 = 0;
            switch (SecondChoice)
            {
                case SecondLevel.Horse:
                    V0 = 35;
                    break;
                case SecondLevel.Rabbit:
                    V0 = 30;
                    break;
                case SecondLevel.Sheep:
                    V0 = 33;
                    break;
            }
            //Calculate the maximum velocity of the 
            double Vmax = V0 + 0.1 * stl.entity.State;
            /*Randomly select a value between the speed threshold of 0.7 and the maximum value as the current speed, 
             *because the movement time is 1 second, and the speed is equal to the movement distance.
             */
            double movement = 0.7 * V0 + rand.NextDouble() * (Vmax - 0.7 * V0);
            //The speed can not beyond 20.
            movement = movement > 20 ? 20 : movement;
            //The speed can not be smaller than 0.7*V0.
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //Randomly select one angle which is closer to the previous to move to prevent moving back and forth .
            stl.angle = rand.Next(stl.angle - 50, stl.angle + 50);
            double Left = stl.location.Left + movement * Math.Sin(stl.angle * Math.PI / 180);
            double Top = stl.location.Top + movement * Math.Cos(stl.angle * Math.PI / 180);

            //Check for crossing the boundaries.
            if (Left < 0)
            {
                Left = 0 - Left;
                stl.angle += 90;
            }
            if (Left > 990)
            {
                Left = 2 * 990 - Left;
                stl.angle += 90;
            }
            if (Top < 0)
            {
                Top = 0 - Top;
                stl.angle += 90;
            }
            if (Top > 490)
            {
                Top = 2 * 490 - Top;
                stl.angle += 90;
            }
            //Update some parameters
            Moving(stl, Left - stl.location.Left, Top - stl.location.Top);
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
        }

        /**
         * Function: Realize the random movement for the entity if they don't choose any other actions.
         * Input: The object represents the entity at third nutritional level.
         * Output: Empty
         */
        public static void TTLRun(TTLHelper ttl)
        {
            //Randomly select one angle which is closer to the previous to move to prevent moving back and forth .
            var rand = new Random();
            //Set the speed threshold to 15
            double V0 = 0;
            switch (ThirdChoice)
            {
                case ThirdLevel.Tiger:
                    V0 = 45;
                    break;
                case ThirdLevel.Wolf:
                    V0 = 40;
                    break;
            }
            //Calculate the maximum velocity of the 
            double Vmax = V0 + 0.1 * ttl.entity.State;
            /*Randomly select a value between the speed threshold of 0.7 and the maximum value as the current speed, 
             *because the movement time is 1 second, and the speed is equal to the movement distance.
             */
            double movement = 0.7 * V0 + rand.NextDouble() * (Vmax - 0.7 * V0);
            //The speed can not beyond 20.
            movement = movement > 20 ? 20 : movement;
            //The speed can not be smaller than 0.7*V0.
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //Randomly select one angle which is closer to the previous to move to prevent moving back and forth .
            ttl.angle = rand.Next(ttl.angle - 50, ttl.angle + 50);
            double Left = ttl.location.Left + movement * Math.Sin(ttl.angle * Math.PI / 180);
            double Top = ttl.location.Top + movement * Math.Cos(ttl.angle * Math.PI / 180);
            //Check for crossing the boundaries.
            if (Left < 0)
            {
                Left = 0 - Left;
                ttl.angle += 90;
            }
            if (Left > 990)
            {
                Left = 2 * 990 - Left;
                ttl.angle += 90;
            }
            if (Top < 0)
            {
                Top = 0 - Top;
                ttl.angle += 90;
            }
            if (Top > 490)
            {
                Top = 2 * 490 - Top;
                ttl.angle += 90;
            }
            // Update
            Moving(ttl, Left - ttl.location.Left, Top - ttl.location.Top);
            ttl.entity.Energy -= movement;
            ttl.entity.Tiredness += movement * 0.1;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
        }
    }
}
