using System;
using System.IO;
using System.Xml.Serialization;

namespace AnimalSerialization
{
    public class MyAttribute : Attribute
    {
        public string Comment { get; set; }
        MyAttribute() { }
        public MyAttribute(string Comment)
        {
            this.Comment = Comment;
        }
    }

    [MyAttribute("Классификация животных(травоядные, хищники, всеядные")]
    public enum eClassificationAnimal
    {
        Herbivores,
        Carnivores,
        Omnivores
    }

    [MyAttribute("Классификация еды(мясо, растения, всё")]
    public enum eFavouriteFood
    {
        Meat,
        Plants,
        Everything
    }

    [MyAttribute("Родительский класс Animal ")]
    [XmlInclude(typeof(Cow))]
    [XmlInclude(typeof(Lion))]
    [XmlInclude(typeof(Pig))]
    public abstract class Animal
    {
        public string Country { get; set; }
        public string HideFromOtherAnimals { get; set; }
        public string Name { get; set; }
        public string WhatAnimal { get; set; }
        public eClassificationAnimal Classifications { get; set; }
        public eFavouriteFood FavouriteFood { get; set; }

        public Animal() { } // Пустой конструктор для сериализации

        public Animal(string country, string hideFromOtherAnimals, string name, string whatAnimal, eClassificationAnimal classifications, eFavouriteFood favouriteFood)
        {
            Country = country;
            HideFromOtherAnimals = hideFromOtherAnimals;
            Name = name;
            WhatAnimal = whatAnimal;
            Classifications = classifications;
            FavouriteFood = favouriteFood;
        }

        public abstract void SayHello();
    }

    [MyAttribute("Класс коровы производный от Animal")]
    public class Cow : Animal
    {
        public Cow() { } // Пустой конструктор для сериализации

        public Cow(string country, string hideFromOtherAnimals, string name, string whatAnimal, eClassificationAnimal classifications, eFavouriteFood favouriteFood)
            : base(country, hideFromOtherAnimals, name, whatAnimal, classifications, favouriteFood) { }

        public override void SayHello() { Console.WriteLine("ММУУУУУУУУУУУ!!! Hello"); }
    }

    [MyAttribute("Класс льва производный от Animal")]
    public class Lion : Animal
    {
        public Lion() { } // Пустой конструктор для сериализации

        public Lion(string country, string hideFromOtherAnimals, string name, string whatAnimal, eClassificationAnimal classifications, eFavouriteFood favouriteFood)
            : base(country, hideFromOtherAnimals, name, whatAnimal, classifications, favouriteFood) { }

        public override void SayHello() { Console.WriteLine("РРРРРРРРРРРРрр!!!"); }
    }

    [MyAttribute("Класс свиньи производный от Animal")]
    public class Pig : Animal
    {
        public Pig() { } // Пустой конструктор для сериализации

        public Pig(string country, string hideFromOtherAnimals, string name, string whatAnimal, eClassificationAnimal classifications, eFavouriteFood favouriteFood)
            : base(country, hideFromOtherAnimals, name, whatAnimal, classifications, favouriteFood) { }

        public override void SayHello() { Console.WriteLine("ХРЮюююю ХРЮЮЮЮЮ!!!"); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Создание экземпляра класса Animal (в данном случае Cow)
            Animal myAnimal = new Cow("Россия", "Да", "Милка", "Корова", eClassificationAnimal.Herbivores, eFavouriteFood.Plants);

            // Сериализация в XML
            XmlSerializer serializer = new XmlSerializer(typeof(Cow));
            using (FileStream stream = new FileStream("animal.xml", FileMode.Create))
            {
                serializer.Serialize(stream, myAnimal);
            }

            Console.WriteLine("Сериализация завершена. Объект сохранен в animal.xml.");

            // Десериализация из XML
            Animal deserializedAnimal;
            using (FileStream stream = new FileStream("animal.xml", FileMode.Open))
            {
                deserializedAnimal = (Animal)serializer.Deserialize(stream);
            }

            // Вывод полученного объекта на консоль
            Console.WriteLine($"Страна: {deserializedAnimal.Country}");
            Console.WriteLine($"Скрывается от других животных: {deserializedAnimal.HideFromOtherAnimals}");
            Console.WriteLine($"Имя: {deserializedAnimal.Name}");
            Console.WriteLine($"Что это за животное: {deserializedAnimal.WhatAnimal}");
            Console.WriteLine($"Классификация: {deserializedAnimal.Classifications}");
            Console.WriteLine($"Любимая еда: {deserializedAnimal.FavouriteFood}");
            deserializedAnimal.SayHello();
        }
    }
}