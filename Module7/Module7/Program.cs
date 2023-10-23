using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Module7
{


    class Program
    {
        static void Main(string[] args)
        {
            Order<Delivery> odrer1 = new() { Delivery = new HomeDelivery("Военно-морская, 16", "Seva", new DateTime()), Number = 1, Description = "Нет описания", BasketOrder = null, Product = null };
            Order<Delivery> odrer2 = new() { Delivery = new PickPointDelivery("Военно-морская, 16", "Seva", new DateTime()), Number = 2, Description = "Нет описания" };
            Order<Delivery> odrer3 = new() { Delivery = new ShopDelivery("Военно-морская, 16", "Seva", new DateTime()), Number = 3, Description = "Нет описания" };
        }
    }
}

//    class Employee
//    {
//        public string Name;
//        public int Age;
//        public int Salary;
//    }

//    class ProjectManager : Employee
//    {
//        public string ProjectName;
//    }

//    class Developer : Employee
//    {
//        public string ProgrammingLanguage;
//    }

//    class Food
//    {

//    }

//    class Fructs : Food
//    {
//        public string Name;
//    }

//    class Apples : Fructs
//    {

//    }

//    class Banans : Fructs
//    {

//    }

//    class Pears : Fructs
//    {

//    }

//    class Vegetables : Food
//    {

//    }

//    class Potato : Vegetables
//    {

//    }

//    class Carrot : Vegetables
//    {

//    }

//    //class Obj
//    //{
//    //    private string name;
//    //    private string owner;
//    //    private int length;
//    //    private int count;
//    //    public int Value;

//    //    public static Obj operator +(Obj a, Obj b)
//    //    {
//    //        return new Obj
//    //        {
//    //            Value = a.Value + b.Value
//    //        };
//    //    }

//    //    public static Obj operator - (Obj a, Obj b)
//    //    {
//    //        return new Obj
//    //        {
//    //            Value = a.Value - b.Value
//    //        };
//    //    }
//    //    //public Obj (string name, string ownerName, int objLength, int count)
//    //    //{
//    //    //    this.name = name;
//    //    //    owner = ownerName;
//    //    //    length = objLength;
//    //    //    this.count = count;
//    //    //}
//    //}

//    class BaseClass
//    {
//        protected string Name;
//        public virtual int Counter
//        {
//            get;
//            set;
//        }

//        public virtual void Display() 
//        {
//            Console.WriteLine("Метод класса BaseClass");
//        }

//        public BaseClass(string name)
//        {
//            Name = name;
//        }
//    }

//    class DerivedClass : BaseClass
//    {
//        public string Description;
//        private int counter;

//        public override int Counter 
//        {
//            get 
//            {
//                return counter;
//            }
//            set
//            {
//                if (value < 0)
//                {
//                    Console.WriteLine("Count would > 0 ");
//                }
//                else
//                {
//                    counter = value;
//                }
//            } 
//        }

//        public override void Display()
//        {
//            base.Display();
//            Console.WriteLine("Метод класса DerivedClass");
//        }

//        public DerivedClass(string name, string description) : base(name)
//        {
//            Description = description;
//        }

//        public DerivedClass(string description, int counter, string name) : base(name)
//        {
//            Description = description;
//            Counter = counter;
//        }
//    }

//    class A
//    {
//        public virtual void Display()
//        {
//            Console.WriteLine("A");
//        }
//    }

//    class B : A
//    {
//        public new void Display()
//        {
//            Console.WriteLine("B");
//        }
//    }

//    class C : A
//    {
//        public override void Display()
//        {
//            Console.WriteLine("C");
//        }
//    }

//    class D : B
//    {
//        public new void Display()
//        {
//            Console.WriteLine("D");
//        }
//    }

//    class E : C
//    {
//        public new void Display()
//        {
//            Console.WriteLine("E");
//        }
//    }

//    class IndexingClass
//    {
//        private int[] array;

//        public IndexingClass(int[] array)
//        {
//            this.array = array;
//        }

//        public int this[int index]
//        {
//            get
//            { 
//                if (index >= 0)
//                {
//                    return array[index];
//                }
//                else
//                {
//                    return 0;
//                }
//            }
//            set 
//            { 
//                if (index >= 0)
//                {
//                    array[index] = value;
//                }
//            }
//        }
//    }

//    // Абстрактные классы и методы
//    abstract class ComputerPart
//    {
//        public abstract void Work();
//    }

//    class Processor : ComputerPart
//    {
//        public override void Work()
//        {
//            throw new NotImplementedException();
//        }
//    }

//    class MotherBoard : ComputerPart
//    {
//        public override void Work()
//        {
//            throw new NotImplementedException();
//        }
//    }

//    class GrahicCard : ComputerPart
//    {
//        public override void Work()
//        {
//            throw new NotImplementedException();
//        }
//    }

//    class Obj
//    {
//        //public string Name;
//        //public string Description;

//        public static string Parent;
//        public static int DaysInWeek;
//        public static int MaxValue;

//        static Obj()
//            {
//                Parent = "System/Object";
//                DaysInWeek = 7;
//                MaxValue = 2000;
//            }
//    }

//    class Helper
//    {
//        public static void Swap(ref int num1, ref int num2)
//        {
//            int swap;
//            swap = num1;
//            num1 = num2;
//            num2 = swap;
//        }
//    }

//    static class GetNumbers
//    {
//        public static int GetPositiv(this int number)
//        {
//            if (number < 0)
//            {
//                return +number;
//            }
//            else
//            {
//                return number;
//            }
//        }

//        public static int GetNegative(this int number)
//        {
//            if (number > 0)
//            {
//                return -number;
//            }
//            else
//            {
//                return number;
//            }
//        }
//    }

//    abstract class Car<TEngine> where TEngine : Engine
//    {
//        public TEngine Engine;

//        public virtual void ChangePart<TPart> (TPart newPart) where TPart : CarPart
//        {

//        }
//    }

//    class ElectricCar : Car<ElectricEngine>
//    {
//        public override void ChangePart<TPart>(TPart newPart)
//        {

//        }
//    }

//    class GasCar : Car<GasEngine>
//    {
//        public override void ChangePart<TPart>(TPart newPart)
//        {

//        }
//    }

//    abstract class Engine { 
//    }

//    abstract class CarPart
//    {

//    }

//    class ElectricEngine : Engine
//    {

//    }

//    class GasEngine : Engine
//    {

//    }

//    class Battery : CarPart
//    {

//    }

//    class Differential : CarPart
//    {

//    }

//    class Wheel : CarPart
//    {

//    }

//    class Record <T1, T2>
//    {
//        public DateTime Date;
//        public T1 Id;
//        public T2 Value;
//    }
//}