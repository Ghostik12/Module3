using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;

namespace Module6
{
    //class Module6
    //{

    //    static void Main()
    //    {
    //        Human human = new Human();
    //        human.Greetings();

    //        human.name = "Seva";
    //        human.age = 10;
    //        human.Greetings();

    //        Pens pens = new Pens();
    //        pens.ShowPens();

    //        pens = new Pens("green", 200);
    //        pens.ShowPens();

    //        Rectangle r = new Rectangle();
    //        r.Square();
    //        r = new Rectangle(2);
    //        r.Square();
    //    }
    //}

    //class Human
    //{
    //    public string name;
    //    public int age;
    //    public void Greetings()
    //    {
    //        Console.WriteLine("My name {0}, age {1}", name, age);
    //    }
    //}

    //struct Animal
    //{
    //    public string name;
    //    public int age;
    //    public string type;

    //    public void Info()
    //    {
    //        Console.WriteLine("This is {0}, name {1}, age {2}", type, name, age);
    //    }
    //}

    //class Pens
    //{
    //    public string color;
    //    public int cost;

    //    public void ShowPens()
    //    {
    //        Console.WriteLine("Color {0}, cost {1}", color, cost);
    //    }

    //    public Pens()
    //    {
    //        color = "black";
    //        cost = 100;
    //    }

    //    public Pens(string penColor, int penCost)
    //    {
    //        color= penColor;
    //        cost= penCost;
    //    }
    //}

    //class Rectangle
    //{
    //    public int a, b;

    //    public int Square()
    //    {
    //        Console.WriteLine("A: {0}, B: {1}", a, b);
    //        int c = a * b;
    //        return c;
    //    }

    //    public Rectangle()
    //    {
    //        a = 6;
    //        b = 4;
    //    }

    //    public Rectangle(int side)
    //    {
    //        a = side;
    //        b = side;
    //    }

    //    public Rectangle(int first, int two)
    //    {
    //        a = first;
    //        b = two;
    //    }
    //}

    //class Company
    //{
    //    public Company(string name, string type) 
    //    {
    //        this.Name = name;
    //        this.Type = type;
    //    }
    //    public string Name;
    //    public string Type;
    //}

    //class Department
    //{
    //    public Company Company;
    //    public City City;
    //}

    //class City
    //{
    //    public City(string cityname)
    //    {
    //        this.CityName = cityname;
    //    }
    //    public string CityName;
    //}

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Company comp = new Company("Sber", "Bank");
    //        City city = new City("Spb");
    //        GetSpbBank(comp, city);
    //    }

    //    static Department GetSpbBank(Company dep, City cep)
    //    {
    //        if (dep?.Type == "Bank" && cep?.CityName == "Spb")
    //        {
    //            Console.WriteLine("{0}", dep.Name ?? "Pisec");
    //        }
    //        else
    //        {
    //            Console.WriteLine("Mda");
    //        }
    //        return null;
    //    }
    //}
     
    //class Bus
    //{
    //    public int? Load;

    //    public void PrintStatus()
    //    {
    //        if (Load.HasValue)
    //        {
    //            Console.WriteLine("В авбтобусе {0} пассажиров", Load.Value);
    //        }
    //        else
    //        {
    //            Console.WriteLine("Автобус пуст!");
    //        }
    //    }
    //}

    //class Circle
    //{
    //    public double radius;

    //    public double Squre()
    //    {

    //    }

    //    public double Lenght()
    //    {

    //    }
    //}

    //class Squre
    //{
    //    public int a;
    //    public int b;

    //    public double SqureSum()
    //    {

    //    }

    //    public double Perimetr()
    //    {

    //    }
    //}

    //class Triangle
    //{
    //    public int x;
    //    public int y;
    //    public int z;

    //    public double Squre()
    //    {

    //    }

    //    public double Perimetr()
    //    {

    //    }
    //}


    class User
    {
        private int age;
        
        public int Age 
        { 
            get 
            { 
                return age; 
            } 
            set 
            { 
                if (value < 18)
                {
                    Console.WriteLine("Возраст должен быть больше 18");
                }
                else
                {
                    age = value;
                }
            } 
        }

        private string login;
        
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                if (value.Length < 3)
                {
                    Console.WriteLine("Логин должен содержать больше трех символов");
                }
                else
                {
                    login = value;
                }
            }
        }

        private string email;

        public string Email
        {
            get
            {
                return email;
            }
            set 
            {
                if (!value.Contains("@"))
                {
                    Console.WriteLine("Поле почты должно содержать знак @");
                }
                else
                {
                    email = value;
                }
            }
        }
    }

}
