using System;
using System.Collections.Generic;
using System.Linq;

Vehicle[] vehicles = { new Car("Лада"), new Motorcycle("Урал") };
foreach (var vehicle in vehicles) Console.WriteLine(vehicle.GetInfo());
Animal[] animals = { new Dog("Рекс", 3), new Cat("Мурка", 2), new Bird("Кеша", 1) };
foreach (var animal in animals) Console.WriteLine($"{animal.Name}: {animal.Speak()}");
Verify.That(animals.Select(a => a.Speak()).Distinct().Count() == 3, "Поведение наследников");
Verify.Throws<ArgumentOutOfRangeException>(() => new Dog("Рекс", -1));

    var guard = new GuardDog("Гром", 5);
    ITrainable trainee = guard;
    IGuard protector = guard;
    Console.WriteLine(trainee.Train());
    AnimalCare care = new DailyCare();
    Console.WriteLine(care.Care(guard));
    var bike = new Motorbike("Урал");
    Console.WriteLine(((IMotorized)bike).StartEngine());
    Verify.That(protector.CanGuard && guard.Energy == 60, "Интерфейсы и абстрактный класс");
    Verify.That(((ITwoWheeled)bike).Wheels == 2, "Несколько контрактов");
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

public interface ITrainable { string Train(); }
public interface IGuard { bool CanGuard { get; } }
public sealed class GuardDog : Dog, ITrainable, IGuard
{
    public GuardDog(string name, int age) : base(name, age) { }
    public string Train() => $"{Name} выполняет команду";
    public bool CanGuard => true;
}
public interface IMotorized { string StartEngine(); }
public interface ITwoWheeled { int Wheels { get; } }
public sealed class Motorbike : Motorcycle, IMotorized, ITwoWheeled
{
    public Motorbike(string model) : base(model) { }
    public string StartEngine() => "Двигатель запущен";
    public int Wheels => 2;
}
public abstract class AnimalCare { public abstract string Care(Animal animal); }
public sealed class DailyCare : AnimalCare
{
    public override string Care(Animal animal) { animal.Eat(10); return $"Уход за {animal.Name}"; }
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
