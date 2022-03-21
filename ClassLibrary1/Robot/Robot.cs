using ClassLibrary1.Coords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Arms;
using System.Collections;

namespace ClassLibrary1.Robot
{
    public class Robot : IMovable
    {
        public const int MAXHEIGHT = 100;
        public Coordinate location { get; private set; }
        public bool isOccupied { get; private set; }
        public Dictionary<string, Arm> arms { get; private set; }
        public string currentAction { get; private set; }

        public Robot(int num, params string[] names)
        {
            arms = new Dictionary<string, Arm>();
            location = new Coordinate();
            isOccupied = false;
            AddArms(num, names);
            currentAction = "idle";
        }

        private void AddArms(int num, string[] names)
        {
            foreach (string name in names)
            {
                arms.Add(name, new Arm(name, location));
            }
        }
        public void MoveX(int offset)
        {
            location.x += offset;
            foreach (var (_, value) in arms)
            {
                value.ShiftX(offset);
            }
            Thread.Sleep(Math.Abs(offset) * 20);
        }
        public void MoveY(int offset)
        {
            location.y += offset;
            foreach (var (_, value) in arms)
            {
                value.ShiftY(offset);
            }
            Thread.Sleep(Math.Abs(offset) * 20);
        }
        public void MoveZ(int offset)
        {
            int newOffset = 0;
            if (offset >= 0)
                newOffset = location.z + offset > MAXHEIGHT ? MAXHEIGHT - location.z : offset;
            else
                newOffset = location.z + offset < 0 ? -location.z : offset;
            location.z += newOffset;
            foreach (var (_, value) in arms)
            {
                value.ShiftZ(newOffset);
            }
            Thread.Sleep(Math.Abs(newOffset) * 20); 
        }
        public void Zero()
        {
            location.SetZero();
        }
        public string toString()
        {
            string str =  @$"Robot coords: ({location.x},{location.y},{location.z}){Environment.NewLine}Arms: {arms.Count}{Environment.NewLine}";
            foreach(var (_, value) in arms)
            {
                str += $"{value.toString()}{Environment.NewLine}";
            }
            return str;
        }
        public Arm GetArm(string name)
        {
            return arms.GetValueOrDefault(name);
        }

        public bool MoveArm(string armName, int x, int y, int z)
        {
            Arm usedArm = null;
            if (arms.TryGetValue(armName, out usedArm))
            {
                if (!usedArm.isOccupied)
                {
                    new Thread(() =>
                    {
                        usedArm.Move(x, y, z);
                    }).Start();
                    return true;
                }
                else
                {
                    //Throw exception instead of false and add msg
                    return false;
                }
            }
            else
            {
                //Throw exception
                return false;
            }
        }

        public bool ArmActionFactory(string armName, string action)
        {
            Arm usedArm = null;
            if (arms.TryGetValue(armName, out usedArm))
            {
                if (!usedArm.isOccupied)
                {
                    switch (action.ToLower())
                    {
                        case "short":
                            new Thread(() =>
                            {
                                usedArm.PerformShortAction();
                            }).Start();
                            return true;
                        case "medium":
                            new Thread(() =>
                            {
                                usedArm.PerformMediumAction();
                            }).Start();
                            return true;
                        case "long":
                            new Thread(() =>
                            {
                                usedArm.PerformLongAction();
                            }).Start();
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    //Throw exception instead of false and add msg
                    return false;
                }
            }
            else
            {
                //Throw exception
                return false;
            }
        }
        public bool Move(params int[] offset)
        {
            int len = offset.Length;
            int x = len > 0 ? offset[0] : 0;
            int y = len > 1 ? offset[1] : 0;
            int z = len > 2 ? offset[2] : 0;
            if (!isOccupied)
            {
                isOccupied = true;
                new Thread(() =>
                {
                    currentAction = "Move";
                    MoveX(x);
                    MoveY(y);
                    MoveZ(z);
                    currentAction = "idle";
                    isOccupied = false;
                }).Start();
                return true;
            }
            return false;
        }
        public string GetStatus()
        {
            string status = "";
            status += $"Robot status: {currentAction}{Environment.NewLine}";
            foreach (var (_, value) in arms)
            {
                status += $"{value.name} status: {value.currentAction}{Environment.NewLine}";
            }
            return status;
        }
    }
}
