using System;
using System.Collections.Generic;
using System.Linq;

Animal[] animals = { new Dog("Рекс"), new Cat("Мурка"), new Bird("Кеша") };
var writer = new MemoryAnimalWriter();
var service = new AnimalService(writer);
foreach (Animal animal in animals) service.Describe(animal);
foreach (string line in writer.Lines) Console.WriteLine(line);
Console.WriteLine(((IFlyable)animals[2]).Fly());
Console.WriteLine(new Feeder(new GrainDiet()).Feed(animals[2]));
Verify.That(writer.Lines.Count == 3, "Вывод через зависимость");
Verify.That(animals.All(a => !string.IsNullOrWhiteSpace(a.Speak())), "Контракт базового типа");
Verify.That(animals.OfType<IFlyable>().Count() == 1, "Разделение интерфейсов");
Console.WriteLine("Проверки выполнены успешно.");

public abstract class Animal
{
    public string Name { get; }
    protected Animal(string name) { Name = name; }
    public abstract string Speak();
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
public interface IFlyable { string Fly(); }
public sealed class Bird : Animal, IFlyable
{
    public Bird(string name) : base(name) { }
    public override string Speak() => "Чирик";
    public string Fly() => "Птица летит";
}
public interface IAnimalWriter { void Write(string text); }
public sealed class MemoryAnimalWriter : IAnimalWriter
{
    private readonly List<string> lines = new();
    public IReadOnlyList<string> Lines => lines.AsReadOnly();
    public void Write(string text) => lines.Add(text);
}
public sealed class AnimalService
{
    private readonly IAnimalWriter writer;
    public AnimalService(IAnimalWriter writer) { this.writer = writer; }
    public void Describe(Animal animal) => writer.Write($"{animal.Name}: {animal.Speak()}");
}
public interface IDiet { string Food { get; } }
public sealed class GrainDiet : IDiet { public string Food => "Зерно"; }
public sealed class Feeder
{
    private readonly IDiet diet;
    public Feeder(IDiet diet) { this.diet = diet; }
    public string Feed(Animal animal) => $"{animal.Name}: {diet.Food}";
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
