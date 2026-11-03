using System;
using System.Collections.Generic;
using System.Linq;

Animal[] animals = { new Dog(1, "Рекс", 3), new Cat(2, "Мурка", 2), new Bird(3, "Кеша", 1) };
Transport[] transports = { new Car("Model S", "Tesla"), new Bike("Explorer", "Giant") };

    var list = new List<Animal>(animals);
    var byId = animals.ToDictionary(a => a.Id);
    foreach (Animal animal in list) Console.WriteLine($"{animal.Name}: {animal.Speak()}");
    if (byId.TryGetValue(2, out var found)) Console.WriteLine(found.Name);
    var transportList = new List<Transport>(transports);
    var registrations = new Dictionary<string, Transport> { ["A001"] = transports[0], ["B002"] = transports[1] };
    foreach (var transport in transportList) Console.WriteLine(transport.DisplayInfo());
    Verify.That(byId[2].Name == "Мурка" && !byId.ContainsKey(99), "Поиск по ключу");
    Verify.Throws<ArgumentException>(() => byId.Add(2, animals[0]));
    list.Remove(animals[0]);
    Verify.That(list.Count == 2 && byId.Count == 3, "Независимые коллекции");
    Verify.That(registrations["A001"] is Car, "Словарь транспорта");
Console.WriteLine("Проверки выполнены успешно.");

public abstract class Animal : IEquatable<Animal>
{
    public int Id { get; }
    public string Name { get; private set; }
    public int Age { get; }
    public event Action<Animal, string>? Renamed;
    protected Animal(int id, string name, int age)
    {
        if (id <= 0 || age < 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Пустое имя", nameof(name));
        Id = id; Name = name; Age = age;
    }
    public abstract string Speak();
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Пустое имя", nameof(name));
        if (name == Name) return;
        Name = name;
        Renamed?.Invoke(this, name);
    }
    public bool Equals(Animal? other) => other is not null && GetType() == other.GetType() && Id == other.Id;
    public override bool Equals(object? obj) => obj is Animal animal && Equals(animal);
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}
public sealed class Dog : Animal
{
    public Dog(int id, string name, int age) : base(id, name, age) { }
    public override string Speak() => "Гав";
}
public sealed class Cat : Animal
{
    public Cat(int id, string name, int age) : base(id, name, age) { }
    public override string Speak() => "Мяу";
}
public sealed class Bird : Animal
{
    public Bird(int id, string name, int age) : base(id, name, age) { }
    public override string Speak() => "Чирик";
}
public delegate string AnimalDescription(Animal animal);
public abstract class Transport : IEquatable<Transport>
{
    public string Model { get; }
    public string Manufacturer { get; }
    protected Transport(string model, string manufacturer) { Model = model; Manufacturer = manufacturer; }
    public virtual string DisplayInfo() => $"{Manufacturer} {Model}";
    public bool Equals(Transport? other) => other is not null && GetType() == other.GetType() && Model == other.Model && Manufacturer == other.Manufacturer;
    public override bool Equals(object? obj) => obj is Transport other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(GetType(), Model, Manufacturer);
}
public sealed class Car : Transport
{
    public Car(string model, string manufacturer) : base(model, manufacturer) { }
    public override string DisplayInfo() => $"Автомобиль {base.DisplayInfo()}";
}
public sealed class Bike : Transport
{
    public Bike(string model, string manufacturer) : base(model, manufacturer) { }
    public override string DisplayInfo() => $"Велосипед {base.DisplayInfo()}";
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
