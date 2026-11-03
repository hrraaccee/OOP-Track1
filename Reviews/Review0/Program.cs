using System;
using System.Collections.Generic;
using System.Linq;

var flower = new Flower("Роза", 30f, "Красный", 25);
var tree = new Tree("Дуб", 200f, 10, "Лиственные");
Plant[] plants = { flower, tree };
foreach (Plant plant in plants) { plant.Bloom(); plant.Grow(5f); }
flower.Wither(); tree.GrowOlder(2);
Verify.That(flower.Height == 35f && tree.Height == 205f, "Рост");
Verify.That(tree.Age == 12 && flower.IsWithered, "Состояние наследников");
Verify.Throws<ArgumentOutOfRangeException>(() => flower.Grow(-5));
Verify.Throws<ArgumentOutOfRangeException>(() => tree.Grow(float.NaN));
Verify.Throws<ArgumentOutOfRangeException>(() => tree.GrowOlder(-1));
Console.WriteLine("Проверки выполнены успешно.");

public class Plant
{
    public string Name { get; }
    public string Type { get; }
    public float Height { get; private set; }
    public Plant(string name, string type, float height)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Пустое имя", nameof(name));
        if (!float.IsFinite(height) || height < 0) throw new ArgumentOutOfRangeException(nameof(height));
        Name = name; Type = type; Height = height;
    }
    public virtual void Bloom() => Console.WriteLine($"Растение {Name} цветёт!");
    public void Grow(float growth)
    {
        if (!float.IsFinite(growth) || growth < 0 || !float.IsFinite(Height + growth))
            throw new ArgumentOutOfRangeException(nameof(growth));
        Height += growth;
        Console.WriteLine($"{Name}: высота {Height} см");
    }
}
public sealed class Flower : Plant
{
    public string Color { get; }
    public int PetalCount { get; }
    public bool IsWithered { get; private set; }
    public Flower(string name, float height, string color, int petals) : base(name, "Цветок", height)
    {
        if (petals < 0) throw new ArgumentOutOfRangeException(nameof(petals));
        Color = color; PetalCount = petals;
    }
    public override void Bloom() => Console.WriteLine($"Цветок {Name} распускается и радует глаз!");
    public void Wither() { IsWithered = true; Console.WriteLine($"Цветок {Name} начинает увядать"); }
}
public sealed class Tree : Plant
{
    public int Age { get; private set; }
    public string LeafType { get; }
    public Tree(string name, float height, int age, string leafType) : base(name, "Дерево", height)
    {
        if (age < 0) throw new ArgumentOutOfRangeException(nameof(age));
        Age = age; LeafType = leafType;
    }
    public override void Bloom() => Console.WriteLine($"Дерево {Name} покрывается новыми листьями!");
    public void GrowOlder(int years)
    {
        if (years < 0) throw new ArgumentOutOfRangeException(nameof(years));
        Age = checked(Age + years);
        Console.WriteLine($"Дереву {Name} теперь {Age} лет");
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
