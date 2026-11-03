using System;
using System.Collections.Generic;
using System.Linq;

Animal[] animals = { new Dog("Рекс"), new Cat("Мурка"), new Bird("Кеша") };
foreach (IAnimal animal in animals) Console.WriteLine($"{animal.Name}: {animal.Speak()}");
IVehicle[] vehicles = { new Car(), new Motorcycle() };
foreach (var vehicle in vehicles) Console.WriteLine(vehicle.Start());
Verify.That(animals.OfType<IAnimal>().Count() == 3, "Реализация контракта");
Verify.Throws<ArgumentOutOfRangeException>(() => animals[0].Feed(0));

    var memory = new MemoryLogger();
    var care = new AnimalCareService(memory);
    foreach (var animal in animals) care.Feed(animal, 10);
    new AnimalCareService(new ConsoleLogger()).Feed(animals[0], 5);
    foreach (var message in memory.Messages) Console.WriteLine(message);
    Console.WriteLine(new VehicleService(new Car()).Run());
    Console.WriteLine(new VehicleService(new Motorcycle()).Run());
    Verify.That(memory.Messages.Count == 3 && animals[0].Energy == 65, "Подмена зависимости");
    Verify.Throws<ArgumentNullException>(() => new AnimalCareService(null!));
Console.WriteLine("Проверки выполнены успешно.");

public interface IAnimal
{
    string Name { get; }
    string Speak();
    void Feed(int amount);
}
public abstract class Animal : IAnimal
{
    public string Name { get; }
    public int Energy { get; private set; } = 50;
    protected Animal(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Пустое имя", nameof(name));
        Name = name;
    }
    public abstract string Speak();
    public void Feed(int amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Energy = (int)Math.Min(100L, (long)Energy + amount);
    }
}
public sealed class Dog : Animal
{
    public Dog(string name) : base(name) { }
    public override string Speak() => "Гав";
}
public sealed class Cat : Animal
{
    public Cat(string name) : base(name) { }
    public override string Speak() => "Мяу";
}
public sealed class Bird : Animal
{
    public Bird(string name) : base(name) { }
    public override string Speak() => "Чирик";
}
public interface IReportable { string Report(); }
public interface IIdentifiable { string Report(); }
public sealed class AnimalCard : IReportable, IIdentifiable
{
    private readonly Animal animal;
    public AnimalCard(Animal animal) { this.animal = animal; }
    string IReportable.Report() => $"{animal.Name}: энергия {animal.Energy}";
    string IIdentifiable.Report() => animal.Name;
}
public interface IAnimalLogger { void Write(string message); }
public sealed class MemoryLogger : IAnimalLogger
{
    private readonly List<string> messages = new();
    public IReadOnlyList<string> Messages => messages.AsReadOnly();
    public void Write(string message) => messages.Add(message);
}
public sealed class ConsoleLogger : IAnimalLogger
{
    public void Write(string message) => Console.WriteLine(message);
}
public sealed class AnimalCareService
{
    private readonly IAnimalLogger logger;
    public AnimalCareService(IAnimalLogger logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public void Feed(IAnimal animal, int amount)
    {
        animal.Feed(amount);
        logger.Write($"Накормлен {animal.Name}: {animal.Speak()}");
    }
}
public interface IVehicle { string Start(); }
public abstract class Vehicle : IVehicle { public abstract string Start(); }
public sealed class Car : Vehicle { public override string Start() => "Автомобиль запущен"; }
public sealed class Motorcycle : Vehicle { public override string Start() => "Мотоцикл запущен"; }
public interface IDiagnostics { string Start(); }
public sealed class VehicleDiagnostics : IDiagnostics
{
    string IDiagnostics.Start() => "Диагностика запущена";
}
public sealed class VehicleService
{
    private readonly IVehicle vehicle;
    public VehicleService(IVehicle vehicle) { this.vehicle = vehicle; }
    public string Run() => vehicle.Start();
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
