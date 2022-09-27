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
        /**
         * Function: Control the life and death of all livings and update their basic attributes (some attributes are not updated here since passing relevant parameters is quite hard)
         * Input: Empty
         * Output: Emtpy
         */
        public static void UpdateState()
        {
            AllAnimal.RemoveAll(o =>
            {
                switch (o)
                {
                    case FNLHelper fnl:
                        {
                            if (count20 % 5 == 1)    // The age increases by one every 5 tick
                            {
                                fnl.entity.Age++;
                                if (fnl.entity.Age == FirstNutritionalLevel.MAX_AGE)
                                // The age reaches its lifetime and it dies naturally
                                {
                                    canvasObject.Children.Remove(fnl.shape);
                                    FirstNutritionalLevel.Count--;
                                    return true;
                                }
                            }

                            fnl.entity.StateUpdate();  // updates the relevant attributes of plants.

                            if (fnl.entity.Illumination < FirstNutritionalLevel.THRES_ILLUMINATION || fnl.entity.Moisture < FirstNutritionalLevel.THRES_MOISTURE)
                            // It will die if the illumination storage or moisture storage is inadequate very seriously
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
                            if (count20 % 5 == 1)    // The age increases by one every 5 tick
                            {
                                stl.entity.Age++;
                                if (stl.entity.Age == SecondTrophicLevel.MAX_AGE)
                                // The age reaches its lifetime and it dies naturally
                                {
                                    canvasObject.Children.Remove(stl.shape);
                                    SecondTrophicLevel.Count--;
                                    return true;
                                }
                            }

                            if (stl.entity.Energy <= 250 || stl.entity.Tiredness > SecondTrophicLevel.MAX_TIREDNESS)
                            // Die of hunger or die of exhaustion
                            {
                                canvasObject.Children.Remove(stl.shape);
                                SecondTrophicLevel.Count--;
                                return true; 
                            }
                            return false;
                        }
                        break;
                    case TTLHelper ttl:
                        {
                            if (count20 % 5 == 1)    // The age increases by one every 5 tick
                            {
                                ttl.entity.Age++;
                                if (ttl.entity.Age == ThirdTrophicLevel.MAX_AGE)
                                // The age reaches its lifetime and it dies naturally
                                {
                                    canvasObject.Children.Remove(ttl.shape);
                                    ThirdTrophicLevel.Count--;
                                    return true;
                                }
                            }

                            if (ttl.entity.Energy <= 250 || ttl.entity.Tiredness > ThirdTrophicLevel.MAX_TIREDNESS)
                            // Die of hunger or die of exhaustion
                            {
                                canvasObject.Children.Remove(ttl.shape);
                                ThirdTrophicLevel.Count--;
                                return true;
                            }
                            return false;
                        }
                        break;
                }
                return false;
            });
        }
    }
}
