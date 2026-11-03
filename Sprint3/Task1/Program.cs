using System;
using System.Collections.Generic;
using System.Linq;

Animal[] animals = { new Dog("Рекс", 3), new Cat("Мурка", 2), new Bird("Кеша", 1) };
foreach (Animal animal in animals) Console.WriteLine(animal.Speak());
Vehicle[] vehicles = { new Car("Лада"), new Motorcycle("Урал") };
foreach (Vehicle vehicle in vehicles) Console.WriteLine(vehicle.GetInfo());
Verify.That(animals.Select(a => a.Speak()).SequenceEqual(new[] {"Гав", "Мяу", "Чирик"}), "Динамический полиморфизм");

    var speaker = new AnimalSpeaker();
    Console.WriteLine(speaker.Describe(animals[0]));
    Console.WriteLine(speaker.Describe(animals[0], 3));
    var driver = new Driver();
    Console.WriteLine(driver.Drive(vehicles[0]));
    Console.WriteLine(driver.Drive(vehicles[0], 40));
    Verify.That(speaker.Describe(animals[1], 2) == "Мяу Мяу", "Перегрузка");
    Verify.That(vehicles[0].Speed == 40, "Перегрузка Drive");
    Verify.Throws<ArgumentOutOfRangeException>(() => speaker.Describe(animals[0], 0));
Console.WriteLine("Проверки выполнены успешно.");

public class Vehicle
{
    public string Model { get; set; }
    public int Speed { get; private set; }
    public Vehicle(string model) { Model = model; }
    public void Accelerate(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Speed = checked(Speed + amount);
    }
    public virtual string GetInfo() => $"{Model}: {Speed} км/ч";
}
public class Car : Vehicle
{
    public int Doors { get; }
    public Car(string model, int doors = 4) : base(model) { Doors = doors; }
    public override string GetInfo() => $"{base.GetInfo()}, дверей: {Doors}";
}
public class Motorcycle : Vehicle
{
    public Motorcycle(string model) : base(model) { }
    public override string GetInfo() => $"Мотоцикл {base.GetInfo()}";
}

public interface IAnimal { string Speak(); }
public class Animal : IAnimal
{
    private string name = "Без имени";
    private int age;
    protected int energy = 50;
    internal string Habitat { get; set; } = "Дом";
    public static int Count { get; private set; }
    public string Name
    {
        get => name;
        set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Пустое имя", nameof(value)); name = value; }
    }
    public int Age
    {
        get => age;
        set { if (value < 0) throw new ArgumentOutOfRangeException(nameof(value)); age = value; }
    }
    public int Energy => energy;
    public Animal() : this("Без имени", 0) { }
    public Animal(string name, int age) { Name = name; Age = age; Count++; }
    public virtual string Speak() => "Звук животного";
    public string Meet(Animal other) => $"{Name} знакомится с {other.Name}: {other.Speak()}";
    public void Eat(int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        energy = (int)Math.Min(100L, (long)energy + amount);
    }
    public static string Statistics() => $"Создано животных: {Count}";
}
public class Dog : Animal
{
    public string Breed { get; set; } = "Метис";
    public Dog(string name, int age) : base(name, age) { }
    public override string Speak() => "Гав";
    public void Play() { if (energy >= 10) energy -= 10; }
}
public class Cat : Animal
{
    public Cat(string name, int age) : base(name, age) { }
    public override string Speak() => "Мяу";
}
public class Bird : Animal
{
    public Bird(string name, int age) : base(name, age) { }
    public override string Speak() => "Чирик";
}

public sealed class AnimalSpeaker
{
    public string Describe(Animal animal) => $"{animal.Name}: {animal.Speak()}";
    public string Describe(Animal animal, int repeats)
    {
        if (repeats < 1) throw new ArgumentOutOfRangeException(nameof(repeats));
        return string.Join(" ", Enumerable.Repeat(animal.Speak(), repeats));
    }
}
public sealed class Driver
{
    public string Drive(Vehicle vehicle) => vehicle.GetInfo();
    public string Drive(Vehicle vehicle, int speed)
    {
        vehicle.Accelerate(speed);
        return vehicle.GetInfo();
    }
}

public static class Verify
{
    public static void That(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    public static void Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Ожидалось исключение {typeof(T).Name}");
    }
}
