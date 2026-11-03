using System;
using System.Collections.Generic;
using System.Linq;

var employee = new Employee("Иван", "Разработчик", 80000);
var manager = new Manager("Анна", "Менеджер", 100000, "Разработка");
var projectManager = new ProjectManager("Ольга", "Руководитель проекта", 130000, "Разработка", "Портал", new[] {"Иван", "Анна"});
Employee[] employees = { employee, manager, projectManager };
foreach (Employee person in employees) person.PrintInfo();
IReportable reporter = projectManager;
ITeamLeader leader = projectManager;
Console.WriteLine(reporter.GenerateReport());
Console.WriteLine(string.Join(", ", leader.GetTeamMembers()));
Verify.That(reporter.GenerateReport().Contains("Портал"), "Отчёт");
string[] copy = leader.GetTeamMembers(); copy[0] = "Подмена";
Verify.That(leader.GetTeamMembers()[0] == "Иван", "Защита списка команды");
Verify.Throws<ArgumentOutOfRangeException>(() => new Employee("Иван", "Разработчик", double.NaN));
Console.WriteLine("Проверки выполнены успешно.");

public class Employee
{
    public string Name { get; }
    public string Position { get; }
    public double Salary { get; }
    public Employee(string name, string position, double salary)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(position)) throw new ArgumentException("Пустые данные сотрудника");
        if (!double.IsFinite(salary) || salary < 0) throw new ArgumentOutOfRangeException(nameof(salary));
        Name = name; Position = position; Salary = salary;
    }
    public virtual void PrintInfo() => Console.WriteLine($"{Name}, {Position}, зарплата {Salary}");
}
public class Manager : Employee
{
    public string Department { get; }
    public Manager(string name, string position, double salary, string department) : base(name, position, salary)
    {
        Department = department;
    }
    public override void PrintInfo() { base.PrintInfo(); Console.WriteLine($"Отдел: {Department}"); }
}
public interface IReportable { string GenerateReport(); }
public interface ITeamLeader { string[] GetTeamMembers(); }
public sealed class ProjectManager : Manager, IReportable, ITeamLeader
{
    private readonly string[] team;
    public string ProjectName { get; }
    public ProjectManager(string name, string position, double salary, string department, string projectName, IEnumerable<string> team)
        : base(name, position, salary, department)
    {
        ArgumentNullException.ThrowIfNull(team);
        ProjectName = projectName; this.team = team.ToArray();
    }
    public override void PrintInfo() { base.PrintInfo(); Console.WriteLine($"Проект: {ProjectName}"); }
    public string GenerateReport() => $"Проект {ProjectName}, руководитель {Name}, отдел {Department}, участников {team.Length}";
    public string[] GetTeamMembers() => (string[])team.Clone();
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
