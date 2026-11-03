using System;
using System.Collections.Generic;
using System.Linq;

var student = new Student("Иван", 20, "Мужской", "ТИУ", 2);
var employee = new Employee("Анна", 30, "Женский", "ИТ Компания", 90000m);
var teacher = new Teacher("Ольга", 40, "Женский", "ООП", 15);
Person[] people = { student, employee, teacher };
foreach (Person person in people)
{
    Console.WriteLine(person.GetInfo());
    Console.WriteLine(person.SayHello());
}
Verify.That(student.SayHello().Contains("2 курсе"), "Приветствие студента");
Verify.That(employee.GetInfo().Contains("90000"), "Информация о сотруднике");
Verify.That(teacher.SayHello().Contains("ООП"), "Приветствие преподавателя");

    student.Name = "Пётр";
    student.Course = 3;
    Console.WriteLine(student.IntroduceTo(teacher));
    Verify.That(student.IntroduceTo(teacher).Contains("ООП"), "Взаимодействие");
    Verify.Throws<ArgumentOutOfRangeException>(() => employee.Salary = -1);
    Verify.Throws<ArgumentOutOfRangeException>(() => student.Course = 0);
    Verify.Throws<ArgumentException>(() => student.Name = " ");
Console.WriteLine("Проверки выполнены успешно.");

public class Person
{
    private string name = "";
    private int age;
    public string Name { get => name; set { name = Text(value); } }
    public int Age
    {
        get => age;
        set { if (value < 0 || value > 130) throw new ArgumentOutOfRangeException(nameof(value)); age = value; }
    }
    public string Gender { get; }
    public Person(string name, int age, string gender) { Name = name; Age = age; Gender = Text(gender); }
    protected static string Text(string value) => string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Значение не может быть пустым") : value.Trim();
    public virtual string GetInfo() => $"{Name}, {Age} лет, пол: {Gender}";
    public virtual string SayHello() => $"Здравствуйте, меня зовут {Name}";
    public string IntroduceTo(Person other) => $"{Name} знакомится с {other.Name}. {other.SayHello()}";




}
public class Student : Person
{
    private int course;
    public string University { get; }
    public int Course
    {
        get => course;
        set { if (value < 1 || value > 6) throw new ArgumentOutOfRangeException(nameof(value)); course = value; }
    }
    public Student(string name, int age, string gender, string university, int course) : base(name, age, gender)
    { University = Text(university); Course = course; }
    public override string SayHello() => $"{base.SayHello()}, я учусь в {University} на {Course} курсе";
    public override string GetInfo() => $"{base.GetInfo()}, {University}, курс {Course}";

}
public class Employee : Person
{
    private decimal salary;
    public string Company { get; }
    public decimal Salary
    {
        get => salary;
        set { if (value < 0) throw new ArgumentOutOfRangeException(nameof(value)); salary = value; }
    }
    public Employee(string name, int age, string gender, string company, decimal salary) : base(name, age, gender)
    { Company = Text(company); Salary = salary; }
    public override string GetInfo() => $"{base.GetInfo()}, компания {Company}, зарплата {Salary}";

}
public class Teacher : Person
{
    public string Subject { get; }
    public int Experience { get; }
    public Teacher(string name, int age, string gender, string subject, int experience) : base(name, age, gender)
    {
        if (experience < 0 || experience > age) throw new ArgumentOutOfRangeException(nameof(experience));
        Subject = Text(subject); Experience = experience;
    }
    public override string SayHello() => $"{base.SayHello()}, я преподаю {Subject}";
    public override string GetInfo() => $"{base.GetInfo()}, предмет {Subject}, стаж {Experience}";

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
