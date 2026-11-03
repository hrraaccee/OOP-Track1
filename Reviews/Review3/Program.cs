using System;
using System.Collections.Generic;
using System.Linq;

var computer = ComputerFactory.Office();
foreach (Component component in computer.Components) component.GetInfo("Компонент");
Console.WriteLine($"Общая стоимость: {computer.GetTotalPrice()}");
Verify.That(computer.GetTotalPrice() == 23000m, "Сумма цен");
var cpu = (CPU)computer.Components[0];
var sameCpu = new CPU("Office CPU", 10000m, 4);
Verify.That(cpu == sameCpu && !(cpu != sameCpu), "Равенство CPU");
Verify.That(cpu.GetHashCode() == sameCpu.GetHashCode(), "Контракт хеша");
CPU? missing = null;
Verify.That(missing == null && cpu != missing, "Сравнение с null");
Verify.That(new RAM("DDR4", 4000m, 16) == new RAM("DDR4", 4000m, 16), "Равенство RAM");
Verify.That(new GPU("GPU", 5000m, 2) != new GPU("GPU", 5000m, 4), "Разная видеопамять");
Verify.That(new HDD("Disk", 4000m, 1000) == new HDD("Disk", 4000m, 1000), "Равенство HDD");
Verify.That(!computer.RemoveComponent(sameCpu), "Удаление только установленного экземпляра");
Verify.Throws<InvalidOperationException>(() => computer.AddComponent(cpu));
var other = new Computer();
Verify.Throws<InvalidOperationException>(() => other.AddComponent(cpu));
Verify.That(computer.RemoveComponent(cpu) && !cpu.IsInstalled, "Снятие компонента");
Verify.That(computer.GetTotalPrice() == 13000m, "Сумма после удаления");
other.AddComponent(cpu);
Verify.That(other.GetTotalPrice() == 10000m, "Перенос компонента");
var gaming = ComputerFactory.Gaming();
Verify.That(gaming.GetTotalPrice() == 82000m, "Игровая конфигурация");
Verify.Throws<ArgumentOutOfRangeException>(() => new RAM("DDR4", -1m, 16));
Console.WriteLine("Проверки выполнены успешно.");

public interface IInstallable { void Install(); }
public abstract class Component : IEquatable<Component>, IInstallable
{
    public string Model { get; }
    public decimal Price { get; }
    public bool IsInstalled { get; private set; }
    protected abstract string Specification { get; }
    protected Component(string model, decimal price)
    {
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException("Пустая модель", nameof(model));
        if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
        Model = model; Price = price;
    }
    public virtual void GetInfo() => Console.WriteLine(ToString());
    public void GetInfo(string prefix) { Console.Write(prefix + ": "); GetInfo(); }
    public abstract void Install();
    protected void MarkInstalled()
    {
        if (IsInstalled) throw new InvalidOperationException("Компонент уже установлен");
        IsInstalled = true;
    }
    internal void Uninstall() => IsInstalled = false;
    public override string ToString() => $"{GetType().Name}: {Model}, {Specification}, {Price} руб.";
    public bool Equals(Component? other) => other is not null && GetType() == other.GetType() && Model == other.Model && Price == other.Price && Specification == other.Specification;
    public override bool Equals(object? obj) => obj is Component other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(GetType(), Model, Price, Specification);
}

public sealed class CPU : Component, IInstallable
{
    public int Cores { get; }
    protected override string Specification => $"{Cores} ядер";
    public CPU(string model, decimal price, int value) : base(model, price)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
        Cores = value;
    }
    public override void GetInfo() => Console.WriteLine(ToString());
    public override void Install() { MarkInstalled(); Console.WriteLine($"Установлен CPU {Model}"); }
    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
    public static bool operator ==(CPU? left, CPU? right) => object.Equals(left, right);
    public static bool operator !=(CPU? left, CPU? right) => !object.Equals(left, right);
}

public sealed class RAM : Component, IInstallable
{
    public int Capacity { get; }
    protected override string Specification => $"{Capacity} ГБ ОЗУ";
    public RAM(string model, decimal price, int value) : base(model, price)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
        Capacity = value;
    }
    public override void GetInfo() => Console.WriteLine(ToString());
    public override void Install() { MarkInstalled(); Console.WriteLine($"Установлен RAM {Model}"); }
    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
    public static bool operator ==(RAM? left, RAM? right) => object.Equals(left, right);
    public static bool operator !=(RAM? left, RAM? right) => !object.Equals(left, right);
}

public sealed class GPU : Component, IInstallable
{
    public int Memory { get; }
    protected override string Specification => $"{Memory} ГБ видеопамяти";
    public GPU(string model, decimal price, int value) : base(model, price)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
        Memory = value;
    }
    public override void GetInfo() => Console.WriteLine(ToString());
    public override void Install() { MarkInstalled(); Console.WriteLine($"Установлен GPU {Model}"); }
    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
    public static bool operator ==(GPU? left, GPU? right) => object.Equals(left, right);
    public static bool operator !=(GPU? left, GPU? right) => !object.Equals(left, right);
}

public sealed class HDD : Component, IInstallable
{
    public int Capacity { get; }
    protected override string Specification => $"{Capacity} ГБ диска";
    public HDD(string model, decimal price, int value) : base(model, price)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
        Capacity = value;
    }
    public override void GetInfo() => Console.WriteLine(ToString());
    public override void Install() { MarkInstalled(); Console.WriteLine($"Установлен HDD {Model}"); }
    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
    public static bool operator ==(HDD? left, HDD? right) => object.Equals(left, right);
    public static bool operator !=(HDD? left, HDD? right) => !object.Equals(left, right);
}

public sealed class Computer
{
    private readonly List<Component> components = new();
    public IReadOnlyList<Component> Components => components.AsReadOnly();
    public void AddComponent<T>(T component) where T : Component, IInstallable
    {
        ArgumentNullException.ThrowIfNull(component);
        if (component.IsInstalled) throw new InvalidOperationException("Компонент уже установлен");
        component.Install();
        components.Add(component);
    }
    public bool RemoveComponent<T>(T component) where T : Component
    {
        int index = components.FindIndex(c => ReferenceEquals(c, component));
        if (index < 0) return false;
        components[index].Uninstall();
        components.RemoveAt(index);
        return true;
    }
    public decimal GetTotalPrice() => components.Sum(c => c.Price);
}
public static class ComputerFactory
{
    public static Computer Create<T>(params T[] components) where T : Component, IInstallable
    {
        ArgumentNullException.ThrowIfNull(components);
        if (components.Any(c => c is null || c.IsInstalled)) throw new ArgumentException("Некорректный набор компонентов");
        if (components.Distinct(ReferenceEqualityComparer.Instance).Count() != components.Length) throw new ArgumentException("Повтор компонента");
        var computer = new Computer();
        foreach (T component in components) computer.AddComponent(component);
        return computer;
    }
    public static Computer Office() => Create<Component>(new CPU("Office CPU", 10000m, 4), new RAM("DDR4", 4000m, 16), new GPU("Basic GPU", 5000m, 2), new HDD("Disk", 4000m, 1000));
    public static Computer Gaming() => Create<Component>(new CPU("Gaming CPU", 25000m, 8), new RAM("DDR5", 10000m, 32), new GPU("Gaming GPU", 40000m, 12), new HDD("Disk", 7000m, 2000));
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
