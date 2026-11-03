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

    student.UpdateEmail("student@example.test");
    student.Relocate("Тюмень");
    student.Deactivate(); student.Activate();
    student.ChangeSpecialty("Программная инженерия");
    student.EarnCredits(10); student.AdvanceCourse();
    employee.Promote("Разработчик", 100000m);
    employee.Transfer("Платформа"); employee.TakeVacation(); employee.ReturnToWork();
    teacher.SetDegree("Кандидат наук"); teacher.ChangeOffice("405");
    teacher.ScheduleHours(18); teacher.ChangeLanguage("Русский");
    Console.WriteLine(student.Study()); Console.WriteLine(employee.Work()); Console.WriteLine(teacher.Teach());
    Console.WriteLine(((IContactable)student).ContactInfo());
    Console.WriteLine(((IReportable)teacher).Report());
    Verify.That(teacher is Employee && student.Credits == 10 && student.IsActive, "Расширенная модель");
    Verify.That(teacher.WeeklyHours == 18 && !employee.OnVacation, "Новые методы");
    Verify.Throws<ArgumentOutOfRangeException>(() => teacher.ScheduleHours(50));

    var registry = new PersonRegistry<Person>();
    foreach (var person in people) registry.Add(person);
    var students = new PersonRegistry<Student>();
    students.Add(student);
    Console.WriteLine(student.GetInfo("Карточка"));
    Console.WriteLine(teacher.SayHello("Добрый день"));
    Verify.That(registry.People.Count == 3 && students.People.Count == 1, "Generic реестр");
    Verify.Throws<InvalidOperationException>(() => registry.Add(student));
    Verify.That(registry.Find(p => p.Age >= 30).Count() == 2, "Поиск");

    var memory = new MemoryReportWriter();
    var reports = new ReportService(memory);
    foreach (var person in people) reports.Print(person);
    new ReportService(new ConsoleReportWriter()).Print(teacher);
    Verify.That(memory.Reports.Count == 3, "Внедрение зависимости и явный интерфейс");

    var byId = people.ToDictionary(p => p.Id);
    var ids = new HashSet<Guid>(people.Select(p => p.Id));
    Func<Person, string> formatter = p => $"{p.Name}: {p.Age}";
    foreach (var person in registry.Find(p => p.IsActive)) Console.WriteLine(formatter(person));
    int changes = 0;
    Action<Person, string> handler = (person, name) => { changes++; Console.WriteLine($"Имя изменено: {name}"); };
    student.NameChanged += handler;
    student.Name = "Алексей";
    student.Name = "Алексей";
    student.NameChanged -= handler;
    student.Name = "Иван";
    Verify.That(changes == 1, "Событие изменения имени");
    Verify.That(!ids.Add(student.Id) && byId[student.Id].Name == "Иван", "Словарь и множество");
    Verify.That(registry.Remove(employee) && registry.People.Count == 2, "Удаление из реестра");
Console.WriteLine("Проверки выполнены успешно.");

public interface IReportable { string Report(); }
public interface IContactable { string ContactInfo(); }

public class Person : IReportable, IContactable
{
    private string name = "";
    private int age;
    public string Name { get => name; set { string next = Text(value); if (name == next) return; name = next; NameChanged?.Invoke(this, name); } }
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

        public Guid Id { get; } = Guid.NewGuid();
        public string Email { get; private set; } = "";
        public string City { get; private set; } = "Тюмень";
        public bool IsActive { get; private set; } = true;
        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@')) throw new ArgumentException("Некорректная почта", nameof(email));
            Email = email;
        }
        public void Relocate(string city) { City = Text(city); }
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
        public string ContactInfo() => $"{Name}: {Email}";

    string IReportable.Report() => GetInfo();
    public string GetInfo(string prefix) => $"{prefix}: {GetInfo()}";
        public string SayHello(string greeting) => $"{greeting}! {SayHello()}";
    public event Action<Person, string>? NameChanged;
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

        public string Specialty { get; private set; } = "Информатика";
        public int EnrollmentYear { get; } = 2025;
        public string StudentId { get; } = "ST-001";
        public int Credits { get; private set; }
        public void ChangeSpecialty(string value) => Specialty = Text(value);
        public void EarnCredits(int count) { if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count)); Credits = checked(Credits + count); }
        public void AdvanceCourse() { Course = checked(Course + 1); }
        public string Study() => $"{Name} изучает {Specialty}";

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

        public string Position { get; private set; } = "Специалист";
        public string Department { get; private set; } = "Разработка";
        public int HireYear { get; } = 2024;
        public bool OnVacation { get; private set; }
        public void Promote(string position, decimal salary) { string title = Text(position); Salary = salary; Position = title; }
        public void Transfer(string department) => Department = Text(department);
        public void TakeVacation() => OnVacation = true;
        public void ReturnToWork() => OnVacation = false;
        public string Work() => $"{Name} работает в {Department}";

}
public class Teacher : Employee
{
    public string Subject { get; }
    public int Experience { get; }
    public Teacher(string name, int age, string gender, string subject, int experience) : base(name, age, gender, "ТИУ", 60000m)
    {
        if (experience < 0 || experience > age) throw new ArgumentOutOfRangeException(nameof(experience));
        Subject = Text(subject); Experience = experience;
    }
    public override string SayHello() => $"{base.SayHello()}, я преподаю {Subject}";
    public override string GetInfo() => $"{base.GetInfo()}, предмет {Subject}, стаж {Experience}";

        public string AcademicDegree { get; private set; } = "Магистр";
        public string Office { get; private set; } = "301";
        public int WeeklyHours { get; private set; }
        public string TeachingLanguage { get; private set; } = "Русский";
        public void SetDegree(string value) => AcademicDegree = Text(value);
        public void ChangeOffice(string value) => Office = Text(value);
        public void ScheduleHours(int hours) { if (hours < 0 || hours > 40) throw new ArgumentOutOfRangeException(nameof(hours)); WeeklyHours = hours; }
        public void ChangeLanguage(string value) => TeachingLanguage = Text(value);
        public string Teach() => $"{Name} преподаёт {Subject}";

}

public sealed class PersonRegistry<T> where T : Person
{
    private readonly List<T> people = new();
    public IReadOnlyList<T> People => people.AsReadOnly();
    public void Add(T person)
    {
        ArgumentNullException.ThrowIfNull(person);
        if (people.Any(p => p.Id == person.Id)) throw new InvalidOperationException("Человек уже добавлен");
        people.Add(person);
    }
    public bool Remove(T person) => people.Remove(person);
    public IEnumerable<T> Find(Func<T, bool> predicate) => people.Where(predicate);
}

public interface IReportWriter { void Write(string report); }
public sealed class ConsoleReportWriter : IReportWriter
{
    public void Write(string report) => Console.WriteLine(report);
}
public sealed class MemoryReportWriter : IReportWriter
{
    private readonly List<string> reports = new();
    public IReadOnlyList<string> Reports => reports.AsReadOnly();
    public void Write(string report) => reports.Add(report);
}
public sealed class ReportService
{
    private readonly IReportWriter writer;
    public ReportService(IReportWriter writer) { this.writer = writer ?? throw new ArgumentNullException(nameof(writer)); }
    public void Print(IReportable person) => writer.Write(person.Report());
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
