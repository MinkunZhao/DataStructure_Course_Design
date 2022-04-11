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
        public static async Task ChooseAndAction()
        {
            /*
            foreach (var animal in AllAnimal)
            {
                var random = new Random();
                var XDirection = random.Next(0, 50) - 25;
                var YDirection = random.Next(0, 50) - 25;
                switch (animal)
                {
                    case STLHelper obj2:
                        Moving(obj2, XDirection, YDirection);
                        break;
                    case TTLHelper obj3:
                        Moving(obj3, XDirection, YDirection);
                        break;
                }
            }
            */
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

        public static void STLChooser(STLHelper sobj)
        {
            //先将ObjectInSight里面的按营养级存起来，根据这个做判断
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
            //如果有第三营养级（逃跑）
            if (thirdLevels.Count != 0)
            {
                STLEscape(sobj);
            }
            //体力不足，休息
            else if (sobj.entity.Tiredness > SecondTrophicLevel.THRES_TIRENESS)
            {
                STLStay(sobj);
            }
            //能量不足，捕食
            else if (sobj.entity.Energy < SecondTrophicLevel.THRES_ENERGY)
            {
                //先判断第二营养级与第一营养级的数量比，如果二比一多了很多，就要先放过第一营养级，不然第一营养级被吃完了
                if (SecondTrophicLevel.Count * 1.0 / FirstNutritionalLevel.Count < 2 * ratioOfFirst / ratioOfSecond)
                {
                    STLHunt(sobj);
                }
                else
                {
                    STLRun(sobj);
                }
            }
            //集群
            //这里我将集群规则改了，改成根据当前这一物种在所有生物中所占的比例与初始时定义的5:3:2的比例作比较，如果比例减小了就集群，因为没有集群有利于繁殖
            //如果按照原来的大于4就集群容易出现数量多时会一直选择集群
            else if ((double)SecondTrophicLevel.Count / AllAnimal.Count < ratioOfSecond / (sum + 1))
            {
                STLGetTogether(sobj);
            }
            else
            {
                var rand = new Random();
                if (rand.NextDouble() < 0.3)
                    STLSeek(sobj);
                else
                    STLRun(sobj);
            }
        }

        public static void TTLChooser(TTLHelper tobj)
        {
            double sum = ratioOfFirst + ratioOfSecond + ratioOfThird;
            if (tobj.entity.Tiredness > ThirdTrophicLevel.THRES_TIRENESS)    // it is tired
            {
                TTLStay(tobj);
            }

            else if (tobj.entity.Energy < ThirdTrophicLevel.THRES_ENERGY)   // it is hungry
            {
                //先判断第三营养级与第二营养级的数量比，如果三比二多了很多，就要先放过第二营养级，不然第二营养级被吃完了
                if (ThirdTrophicLevel.Count * 1.0 / SecondTrophicLevel.Count < 2 * ratioOfThird / ratioOfSecond)
                {
                    TTLHunt(tobj);
                }
                else
                {
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
                //集群
                //这里我将集群规则改了，改成根据当前这一物种在所有生物中所占的比例与初始时定义的5:3:2的比例作比较，如果比例减小了就集群，因为没有集群有利于繁殖
                //如果按照原来的大于4就集群容易出现数量多时会一直选择集群
                    if ((double)ThirdTrophicLevel.Count / AllAnimal.Count < ratioOfThird / (sum + 1))   // there are many companions in its sight
                    {
                        TTLGetTogether(tobj);
                    }
                    else
                    {
                        var rand = new Random();
                        if (rand.NextDouble() < 0.3)
                            TTLSeek(tobj);
                        else
                            TTLRun(tobj);
                    }
            }
        }

        public static void STLStay(STLHelper stl)
        {
            /*//一开始讨论设定的速度阈值，可以后期调
            double V0 = 25;
            stl.entity.Tiredness -= V0;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
            stl.entity.Energy -= V0 * 0.3;
            // 原地休息时饿得比移动时慢一些，所以这里的参数应设得比Updater里的参数小一些*/
            stl.entity.Tiredness = 0;
        }

        public static void TTLStay(TTLHelper ttl)
        {
            /*//一开始讨论设定的速度阈值，可以后期调
            double V0 = 30;
            ttl.entity.Tiredness -= V0;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
            ttl.entity.Energy -= V0 * 0.3;
            // 原地休息时饿得比移动时慢一些，所以这里的参数应设得比Updater里的参数小一些*/
            ttl.entity.Tiredness = 0;
        }

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
            // 参数0.5后期需要调

            //设置速度阈值为15
            double V0 = 35;
            //根据那个公式得出速度的最大值，这里a设为0.1
            double Vmax = V0 + 0.1 * stl.entity.State;
            //在0.7的速度阈值与最大值之间随机选择一个值作为当前速度，因为运动时间为1秒，速度与移动距离movement相等
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //速度最大不得超过20
            movement = movement > 20 ? 20 : movement;
            //速度最小不得低于0.7V0
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //朝着目标中心移动movement距离
            double dis = SqrtSumSquare(targetX - stl.location.Left, targetY - stl.location.Top);
            double LeftOffset = (targetX - stl.location.Left) * movement / dis;
            double TopOffset = (targetY - stl.location.Top) * movement / dis;
 

            //越界检查
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


            //直接在这里更新能量和疲劳度，因为在update那里很难获取移动距离
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
        }

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

            // 参数0.5后期需要调

            //设置速度阈值为20，比第二营养级快，使第三营养级能够追上第二营养级
            double V0 = 40;
            //根据那个公式得出速度的最大值，这里a设为0.1
            double Vmax = V0 + 0.1 * ttl.entity.State;
            //在0.7的速度阈值与最大值之间随机选择一个值作为当前速度，因为运动时间为1秒，速度与移动距离movement相等
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //速度最大不得超过25
            movement = movement > 25 ? 25 : movement;
            //速度最小不得低于0.7V0
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;


            //朝着目标中心移动movement距离
            double dis = SqrtSumSquare(targetX - ttl.location.Left, targetY - ttl.location.Top);
            double LeftOffset = (targetX - ttl.location.Left) * movement / dis;
            double TopOffset = (targetY - ttl.location.Top) * movement / dis;

            //越界检查
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


            //直接在这里更新能量和疲劳度，因为在update那里很难获取移动距离
            ttl.entity.Energy -= movement;
            ttl.entity.Tiredness += movement * 0.1;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
        }

        public static void STLEscape(STLHelper stl)
        {
            //设置速度阈值为15
            double V0 = 35;
            //根据那个公式得出速度的最大值，这里a设为0.1
            double Vmax = V0 + 0.1 * stl.entity.State;
            //在0.7的速度阈值与最大值之间随机选择一个值作为当前速度，因为运动时间为1秒，速度与移动距离movement相等
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //速度最大不得超过20
            movement = movement > 20 ? 20 : movement;
            //速度最小不得低于0.7V0
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
            //根据附近的捕食者的位置和它的状态综合考虑，选择最有可能追到stl的第三营养级
            for (int i = 0; i < ttls.Count; i++)
            {

                double dis = SqrtSumSquare(stl.location.Left - ttls[i].location.Left, stl.location.Top - ttls[i].location.Top);
                if ((ttls[i].entity.State * 0.3 - dis * 0.7) > (predator.entity.State * 0.3 - disOfMaxIndex * 0.7))
                {
                    predator = ttls[i];
                    disOfMaxIndex = dis;
                }
            }

            //朝着远离上文求出的捕食者的方向移动
            double Left = (stl.location.Left - predator.location.Left) * movement / disOfMaxIndex + stl.location.Left;
            double Top = (stl.location.Top - predator.location.Top) * movement / disOfMaxIndex + stl.location.Top;
            stl.entity.Energy -= movement;
            stl.entity.Tiredness += movement * 0.5;
            if (0 <= Left && Left <= 990 && 0 <= Top && Top <= 490) { Moving(stl, Left - stl.location.Left, Top - stl.location.Top); return; }

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

            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
        }

        public static double SqrtSumSquare(double x, double y) => Math.Sqrt(x * x + y * y);

        public static void STLHunt(STLHelper stl)
        {
            List<FNLHelper> alternativeGrass = new List<FNLHelper>();
            foreach (var o in stl.entity.ObjectInSight)
            {
                if (o is FNLHelper)
                    alternativeGrass.Add(o as FNLHelper);
            }
            int targetGrass_index = 0;

            //假如视野范围里没有草，扩大搜索范围向外寻找
            if (alternativeGrass.Count == 0)
            {
                STLSeek(stl);
                return;
            }
            // 直接选距离它最近的草吃
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
            //假如视野范围里没有草，扩大搜索范围向外寻找

            //设置速度阈值为15
            double V0 = 35;
            //根据那个公式得出速度的最大值，这里a设为0.1
            double Vmax = V0 + 0.1 * stl.entity.State;
            //在0.7的速度阈值与最大值之间随机选择一个值作为当前速度，因为运动时间为1秒，速度与移动距离movement相等
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //速度最大不得超过20
            movement = movement > 20 ? 20 : movement;
            //速度最小不得低于0.7V0
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //根据比例让当前生物朝目标猎物的方向移动movement的距离
            double LeftOffset = (alternativeGrass[targetGrass_index].location.Left - stl.location.Left) * movement / min_distance;
            double TopOffset = (alternativeGrass[targetGrass_index].location.Top - stl.location.Top) * movement / min_distance;
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

            //直接在这里更新能量和疲劳度，因为在update那里很难获取移动距离
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness=stl.entity.Tiredness < 0 ? 0:stl.entity.Tiredness;
        }

        public static void TTLHunt(TTLHelper ttl)
        {
            List<STLHelper> alternativePreys = new List<STLHelper>();   // 猎物备选项
            foreach (var o in ttl.entity.ObjectInSight)
            {
                if (o is STLHelper)
                    alternativePreys.Add(o as STLHelper);
            }
            int targetPrey_index = 0;   // 选定的目标猎物的index
            //假如视野范围里没有猎物，扩大搜索范围向外寻找
            if (alternativePreys.Count == 0)
            {
                TTLSeek(ttl);
                return;
            }

            if (ttl.entity.Energy < ThirdTrophicLevel.THRES_ENERGY * 0.4)   // extremely hungry; 参数0.4后期调整
            {
                // 此时它极度饥饿，所以不考虑那么多，直接捕距离它最近的
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
                // 此时它轻度饥饿，选择视野里状态最差的第二营养级作为目标猎物
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
            //设置速度阈值为20，比第二营养级快，使第三营养级能够追上第二营养级
            double V0 = 40;
            //根据那个公式得出速度的最大值，这里a设为0.1
            double Vmax = V0 + 0.1 * ttl.entity.State;
            //在0.7的速度阈值与最大值之间随机选择一个值作为当前速度，因为运动时间为1秒，速度与移动距离movement相等
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
            //速度最大不得超过25
            movement = movement > 25 ? 25 : movement;
            //速度最小不得低于0.7V0
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //计算猎物与当前的距离
            double dis = SqrtSumSquare(alternativePreys[targetPrey_index].location.Left - ttl.location.Left, alternativePreys[targetPrey_index].location.Top - ttl.location.Top);

            //根据比例让当前生物朝目标猎物的方向移动movement的距离
            double LeftOffset = (alternativePreys[targetPrey_index].location.Left - ttl.location.Left) * movement / dis;
            double TopOffset = (alternativePreys[targetPrey_index].location.Top - ttl.location.Top) * movement / dis;
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

            //直接在这里更新能量和疲劳度，因为在update那里很难获取移动距离
            ttl.entity.Energy -= movement;
            ttl.entity.Tiredness += movement * 0.1;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
        }

        public static void STLSeek(STLHelper stl)
        {
            //当饥饿找不到食物时，使用这个函数向外搜索食物。其实就是将视野扩展到全局，直接全局搜索食物，其他设置与捕食相同。
            FNLHelper prey = null;
            double disOfMiniIndex = 50;
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
            double V0 = 25;
            double Vmax = V0 + 0.1 * stl.entity.State;
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
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
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
        }

        public static void TTLSeek(TTLHelper ttl)
        {
            //当饥饿找不到食物时，使用这个函数向外搜索食物。其实就是将视野扩展到全局，直接全局搜索食物，其他设置与捕食相同。
            STLHelper prey = null;
            double disOfMiniIndex = 50;
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
            double V0 = 30;
            double Vmax = V0 + 0.1* ttl.entity.State;
            double movement = 0.7 * V0 + new Random().NextDouble() * (Vmax - 0.7 * V0);
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
            ttl.entity.Energy -= movement;
            ttl.entity.Tiredness += movement * 0.1;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
        }

        public static void STLRun(STLHelper stl)
        {
            var rand = new Random();
            //设置速度阈值为15
            double V0 = 35;
            //根据那个公式得出速度的最大值，这里a设为0.1
            double Vmax = V0 + 0.1 * stl.entity.State;
            //在0.7的速度阈值与最大值之间随机选择一个值作为当前速度，因为运动时间为1秒，速度与移动距离movement相等
            double movement = 0.7 * V0 + rand.NextDouble() * (Vmax - 0.7 * V0);
            //速度最大不得超过20
            movement = movement > 20 ? 20 : movement;
            //速度最小不得低于0.7V0
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //随便寻找一个角度为方向让生物移动
            //double angle = new Random().NextDouble() * 360;
            //double Left = stl.location.Left + movement * Math.Sin(angle);
            //double Top = stl.location.Top + movement * Math.Cos(angle);

            stl.angle = rand.Next(stl.angle - 50, stl.angle + 50);
            double Left = stl.location.Left + movement * Math.Sin(stl.angle * Math.PI / 180);
            double Top = stl.location.Top + movement * Math.Cos(stl.angle * Math.PI / 180);

            //越界判断
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
            Moving(stl, Left - stl.location.Left, Top - stl.location.Top);
            stl.entity.Energy -= movement * 1.5;
            stl.entity.Tiredness += movement * 0.1;
            stl.entity.Tiredness = stl.entity.Tiredness < 0 ? 0 : stl.entity.Tiredness;
        }

        public static void TTLRun(TTLHelper ttl)
        {
            var rand = new Random();
            //设置速度阈值为15
            double V0 = 35;
            //根据那个公式得出速度的最大值，这里a设为0.1
            double Vmax = V0 + 0.1 * ttl.entity.State;
            //在0.7的速度阈值与最大值之间随机选择一个值作为当前速度，因为运动时间为1秒，速度与移动距离movement相等
            double movement = 0.7 * V0 + rand.NextDouble() * (Vmax - 0.7 * V0);
            //速度最大不得超过20
            movement = movement > 20 ? 20 : movement;
            //速度最小不得低于0.7V0
            movement = movement < 0.7 * V0 ? 0.7 * V0 : movement;

            //随便寻找一个角度为方向让生物移动
            ttl.angle = rand.Next(ttl.angle - 50, ttl.angle + 50);
            double Left = ttl.location.Left + movement * Math.Sin(ttl.angle * Math.PI / 180);
            double Top = ttl.location.Top + movement * Math.Cos(ttl.angle * Math.PI / 180);
            //越界判断
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
            Moving(ttl, Left - ttl.location.Left, Top - ttl.location.Top);
            ttl.entity.Energy -= movement;
            ttl.entity.Tiredness += movement * 0.1;
            ttl.entity.Tiredness = ttl.entity.Tiredness < 0 ? 0 : ttl.entity.Tiredness;
        }
    }
}
