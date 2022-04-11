using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecosystem.service;
using static Ecosystem.GlobalObject;
using ClassLibrary.classes;

namespace Ecosystem.controller
{
    public class Updater
    {
        public static void UpdateState()
        {
            AllAnimal.RemoveAll(o =>
            {
                switch (o)
                {
                    //因为繁殖改成5个tick一次了，这里也改成5个tick增加一岁
                    case FNLHelper fnl:
                        {
                            if (count20 % 5 == 1)    // 每20个tick周期的末尾，年龄增长一岁
                            {
                                fnl.entity.Age++;
                                if (fnl.entity.Age == FirstNutritionalLevel.MAX_AGE)
                                // 到寿命了，自然死亡
                                {
                                    canvasObject.Children.Remove(fnl.shape);
                                    FirstNutritionalLevel.Count--;
                                    return true;
                                }
                            }

                            fnl.entity.StateUpdate();  // (类中的成员函数)更新阳光储存量、水分储存量

                            if (fnl.entity.Illumination < FirstNutritionalLevel.THRES_ILLUMINATION || fnl.entity.Moisture < FirstNutritionalLevel.THRES_MOISTURE)
                            // 如果阳光储存量或水分储存量严重不足，它就死翘翘了
                            {
                                canvasObject.Children.Remove(fnl.shape);
                                FirstNutritionalLevel.Count--;
                                return true;
                            }
                            return false;
                        }
                        break;
                    case STLHelper stl:
                        {
                            if (count20 % 5 == 1)    // 每20个tick周期的末尾，年龄增长一岁
                            {
                                stl.entity.Age++;
                                if (stl.entity.Age == SecondTrophicLevel.MAX_AGE)
                                // 到寿命了，自然死亡
                                {
                                    canvasObject.Children.Remove(stl.shape);
                                    //AllAnimal.Remove(stl);
                                    SecondTrophicLevel.Count--;
                                    return true;
                                }
                            }

                            if (stl.entity.Energy <= 250 || stl.entity.Tiredness > SecondTrophicLevel.MAX_TIREDNESS)
                            // 饿死了或累死了
                            {
                                canvasObject.Children.Remove(stl.shape);
                                //AllAnimal.Remove(stl);
                                SecondTrophicLevel.Count--;
                                return true; 
                            }
                            return false;
                        }
                        break;
                    case TTLHelper ttl:
                        {
                            if (count20 % 5 == 1)    // 每20个tick周期的末尾，年龄增长一岁
                            {
                                ttl.entity.Age++;
                                if (ttl.entity.Age == ThirdTrophicLevel.MAX_AGE)
                                // 到寿命了，自然死亡
                                {
                                    canvasObject.Children.Remove(ttl.shape);
                                    //AllAnimal.Remove(ttl);
                                    ThirdTrophicLevel.Count--;
                                    return true;
                                }
                            }

                            if (ttl.entity.Energy <= 250 || ttl.entity.Tiredness > ThirdTrophicLevel.MAX_TIREDNESS)
                            // 饿死了或累死了
                            {
                                canvasObject.Children.Remove(ttl.shape);
                                //AllAnimal.Remove(ttl);
                                ThirdTrophicLevel.Count--;
                                return true;
                            }
                            return false;
                        }
                        break;
                }
                return false;
            });
            //return Task.CompletedTask;
        }


        /*
            其它说明：
                （1）若动物在本tick内，选择原地休息的抉择，虽然执行上述步骤后状态没有更新，但在stay行为函数中已有更新。
                （2）由于捕食者捕到猎物的时刻，不一定刚好在一个tick结束时，也考虑到参数传来传去很麻烦，还容易出bug，
                     所以当捕食者捕到猎物，“捕食者的饱食度增益”这一项状态更新，放在了chase32和chase21函数中实现
                     (Updater里没有考虑吃掉猎物后的饱食度增益)，
                     毕竟判断各个时刻捕食成功与否，都在这2个chase函数里。
        */
    }
}
