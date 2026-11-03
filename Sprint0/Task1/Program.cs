using System;
using System.Collections.Generic;
using System.Linq;

Vehicle vehicle = new Car();
Console.WriteLine(vehicle.Move());
var owner = new Owner("Анна");
var dog = new Dog("Рекс", 3, owner);
var cat = new Cat("Мурка", 2, owner);
foreach (Animal animal in new Animal[] { dog, cat })
    Console.WriteLine($"{animal.Name}: {animal.Speak()}, {animal.Breathe()}");
dog.Age = 4;
Verify.That(dog.Age == 4 && dog.Speak() == "Гав", "Состояние и поведение");
Verify.That(object.ReferenceEquals(dog.Owner, cat.Owner), "Агрегация владельца");
Verify.That(!object.ReferenceEquals(dog.Lungs, cat.Lungs), "Композиция органов");
Verify.Throws<ArgumentOutOfRangeException>(() => dog.Age = -1);
Console.WriteLine("Проверки выполнены успешно.");

public class Vehicle { public virtual string Move() => "Транспорт движется"; }
public class Car : Vehicle { public override string Move() => "Автомобиль едет"; }
public sealed class Owner
{
    public string Name { get; }
    public Owner(string name) { Name = name; }
}
public sealed class Lungs { public string Breathe() => "Дышит"; }
public abstract class Animal
{
    private int age;
    public string Name { get; }
    public int Age
    {
        get => age;
        set { if (value < 0) throw new ArgumentOutOfRangeException(nameof(value)); age = value; }
    }
    public Owner Owner { get; }
    public Lungs Lungs { get; } = new Lungs();
    protected Animal(string name, int age, Owner owner)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Пустое имя", nameof(name));
        Name = name; Age = age; Owner = owner;
    }
    public string Breathe() => Lungs.Breathe();
    public abstract string Speak();
}
public sealed class Dog : Animal
{
    public Dog(string name, int age, Owner owner) : base(name, age, owner) { }
    public override string Speak() => "Гав";
}
public sealed class Cat : Animal
{
    public Cat(string name, int age, Owner owner) : base(name, age, owner) { }
    public override string Speak() => "Мяу";
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
