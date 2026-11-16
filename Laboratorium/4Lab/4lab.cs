using System;
using System.Collections.Generic;
using System.Linq;

public interface IClassWithIList
{
}

public class Subject
{
    private string _specialization = string.Empty;
    private int _semester;
    private int _hoursCount;

    public string Name { get; set; }

    public string Specialization
    {
        get => _specialization;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Specialization cannot be null or empty.");
            _specialization = value;
        }
    }

    public int Semester
    {
        get => _semester;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Semester), "Semester must be greater than 0.");
            _semester = value;
        }
    }

    public int HoursCount
    {
        get => _hoursCount;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(HoursCount), "HoursCount must be greater than 0.");
            _hoursCount = value;
        }
    }

    public Subject()
    {
        Name = string.Empty;
        _specialization = string.Empty;
        _semester = 1;
        _hoursCount = 1;
    }

    public Subject(string name, string specialization, int semester, int hoursCount)
    {
        Name = name;
        Specialization = specialization;
        Semester = semester;
        HoursCount = hoursCount;
    }

    public override string ToString()
    {
        return $"{Name}/{Specialization}/{Semester}/{HoursCount}";
    }
}

public class FinalGrade
{
    private Subject _subject = null!;
    private double _value;

    public Subject Subject
    {
        get => _subject;
        set => _subject = value ?? throw new ArgumentNullException(nameof(Subject));
    }

    public double Value
    {
        get => _value;
        set
        {
            if (value < 2.0 || value > 5.0)
                throw new ArgumentOutOfRangeException(nameof(Value), "Grade value must be between 2.0 and 5.0.");
            _value = value;
        }
    }

    public DateTime Date { get; set; }

    public FinalGrade()
    {
        _subject = new Subject();
        _value = 2.0;
        Date = DateTime.MinValue;
    }

    public FinalGrade(Subject subject, double value, DateTime date)
    {
        Subject = subject;
        Value = value;
        Date = date;
    }

    public override string ToString()
    {
        return $"Ocena końcowa: {Subject}/{Date.ToShortDateString()}/{Value}";
    }
}

public abstract class Person : IClassWithIList
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }

    protected Person()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        DateOfBirth = DateTime.MinValue;
    }

    protected Person(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public override string ToString()
    {
        return $"Osoba: {FirstName} {LastName}/{DateOfBirth.ToShortDateString()}";
    }
}

public class Lecturer : Person
{
    public string AcademicTitle { get; set; }
    public string Position { get; set; }

    public Lecturer() : base()
    {
        AcademicTitle = string.Empty;
        Position = string.Empty;
    }

    public Lecturer(string firstName, string lastName, DateTime dateOfBirth, string academicTitle, string position)
        : base(firstName, lastName, dateOfBirth)
    {
        AcademicTitle = academicTitle;
        Position = position;
    }

    public override string ToString()
    {
        return $"{base.ToString()}/{AcademicTitle}/{Position}";
    }
}

public class Student : Person
{
    private static int _id;
    private int _group;
    private int _semester;

    public int Id => _id;
    public IList<FinalGrade> Grades { get; set; }
    public string Specialization { get; set; }
    public int IndexId { get; set; }

    public int Group
    {
        get => _group;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Group), "Group must be greater than 0.");
            _group = value;
        }
    }

    public int Semester
    {
        get => _semester;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Semester), "Semester must be greater than 0.");
            _semester = value;
        }
    }

    public double AverageGrades => Grades.Count > 0 ? Grades.Average(g => g.Value) : 0.0;

    public Student() : base()
    {
        _id++;
        Grades = [];
        Specialization = string.Empty;
        _group = 1;
        _semester = 1;
    }

    public Student(string firstName, string lastName, DateTime dateOfBirth, string specialization, int group, int semester = 0)
        : base(firstName, lastName, dateOfBirth)
    {
        _id++;
        Grades = [];
        Specialization = specialization;
        Group = group;
        Semester = semester > 0 ? semester : 1;
    }

    public override string ToString()
    {
        string gradesText = Grades.Count > 0 ? "\n:" + string.Join("\n:", Grades) : string.Empty;
        return $"{base.ToString()}/{Specialization}/Grupa: {Group}/Semestr: {Semester}/Średnia: {AverageGrades:F2}{gradesText}";
    }
}

public class OrganizationUnit : IClassWithIList
{
    private string _name = string.Empty;
    private string _address = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be null or empty.");
            _name = value;
        }
    }

    public string Address
    {
        get => _address;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Address cannot be null or empty.");
            _address = value.Trim();
        }
    }

    public IList<Lecturer> Lecturers { get; set; }

    public OrganizationUnit()
    {
        _name = "Brak nazwy";
        _address = "Brak adresu";
        Lecturers = [];
    }

    public OrganizationUnit(string name, string address, IList<Lecturer> lecturers)
    {
        Name = name;
        Address = address;
        Lecturers = lecturers ?? [];
    }

    public override string ToString()
    {
        return $"Jednostka: {Name}/{Address}/\n{string.Join("\n", Lecturers)}";
    }
}

public class Department : IClassWithIList
{
    private string _name = string.Empty;
    private Person _dean = null!;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be null or empty.");
            _name = value;
        }
    }

    public Person Dean
    {
        get => _dean;
        set => _dean = value ?? throw new ArgumentNullException(nameof(Dean));
    }

    public IList<OrganizationUnit> OrganizationUnits { get; set; }
    public IList<Subject> Subjects { get; set; }
    public IList<Student> Students { get; set; }

    public Department()
    {
        _name = "Brak nazwy";
        OrganizationUnits = [];
        Subjects = [];
        Students = [];
    }

    public Department(string name, Person dean, IList<Subject> subjects, IList<Student> students)
    {
        Name = name;
        Dean = dean;
        OrganizationUnits = [];
        Subjects = subjects ?? [];
        Students = students ?? [];
    }

    public Department(string name, Person dean, IList<OrganizationUnit> organizationUnits, IList<Subject> subjects, IList<Student> students)
    {
        Name = name;
        Dean = dean;
        OrganizationUnits = organizationUnits ?? [];
        Subjects = subjects ?? [];
        Students = students ?? [];
    }

    public override string ToString()
    {
        return $"Wydział: {Name}\nDziekan: {Dean}\nJednostki organizacyjne:\n{string.Join("\n", OrganizationUnits)}\nPrzedmioty:\n{string.Join("\n", Subjects)}\nStudenci:\n{string.Join("\n", Students)}";
    }
}

public static class ClassWithIListGetListExtension
{
    public static IList<T>? GetList<T>(this IClassWithIList obj)
    {
        if (obj == null)
            return null;

        var property = obj.GetType()
            .GetProperties()
            .FirstOrDefault(p => typeof(IList<T>).IsAssignableFrom(p.PropertyType));

        return property?.GetValue(obj) as IList<T>;
    }
}

public static class ClassWithIListCrudOperationsExtension
{
    public static void Add<T>(this IClassWithIList obj, T itemToAdd)
    {
        var list = obj?.GetList<T>();
        if (list != null && itemToAdd != null)
        {
            list.Add(itemToAdd);
        }
    }

    public static void AddRange<T>(this IClassWithIList obj, IList<T> itemsToAdd)
    {
        var list = obj?.GetList<T>();
        if (list != null && itemsToAdd != null)
        {
            foreach (var item in itemsToAdd)
            {
                list.Add(item);
            }
        }
    }

    public static void Clear<T>(this IClassWithIList obj)
    {
        var list = obj?.GetList<T>();
        list?.Clear();
    }

    public static bool Contains<T>(this IClassWithIList obj, T itemToCheck)
    {
        var list = obj?.GetList<T>();
        if (list == null || itemToCheck == null)
            return false;

        return list.Contains(itemToCheck);
    }

    public static void ForEach<T>(this IClassWithIList obj, Action<T> action)
    {
        var list = obj?.GetList<T>();
        if (list != null && action != null)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }
    }

    public static int IndexOf<T>(this IClassWithIList obj, T itemToFind)
    {
        var list = obj?.GetList<T>();
        if (list == null || itemToFind == null)
            return -1;

        return list.IndexOf(itemToFind);
    }

    public static bool Remove<T>(this IClassWithIList obj, T itemToRemove)
    {
        var list = obj?.GetList<T>();
        if (list == null || itemToRemove == null)
            return false;

        return list.Remove(itemToRemove);
    }

    public static void RemoveAll<T>(this IClassWithIList obj, Func<T, bool> predicate)
    {
        var list = obj?.GetList<T>();
        if (list != null && predicate != null)
        {
            var itemsToRemove = list.Where(predicate).ToList();
            foreach (var item in itemsToRemove)
            {
                list.Remove(item);
            }
        }
    }

    public static void RemoveAt<T>(this IClassWithIList obj, int index)
    {
        var list = obj?.GetList<T>();
        if (list != null && index >= 0 && index < list.Count)
        {
            list.RemoveAt(index);
        }
    }

    public static void Update<T>(this IClassWithIList obj, T? oldItem, T newItem)
    {
        var list = obj?.GetList<T>();
        if (list != null && oldItem != null && newItem != null)
        {
            int index = list.IndexOf(oldItem);
            if (index != -1)
            {
                list[index] = newItem;
            }
        }
    }
}

public static class ClassWithIListGroupingExtension
{
    public class GroupReport<TKey, TElement>
    {
        public TKey Key { get; set; } = default!;
        public int Count { get; set; }
        public IList<TElement> Elements { get; set; } = [];

        public override string ToString()
        {
            return $"Klucz: {Key}, Liczba elementów: {Count}\n{string.Join("\n", Elements)}";
        }
    }

    public class GroupReport<TOuterKey, TInnerKey, TElement>
    {
        public TOuterKey Key { get; set; } = default!;
        public int Count { get; set; }
        public IList<GroupReport<TInnerKey, TElement>> Elements { get; set; } = [];

        public override string ToString()
        {
            return $"Klucz zewnętrzny: {Key}, Liczba podgrup: {Count}\n  {string.Join("\n  ", Elements)}";
        }
    }

    public static List<GroupReport<TKey, T>> GroupBy<T, TKey>(
        this IClassWithIList obj,
        Func<T, TKey> groupingKey)
    {
        var list = obj?.GetList<T>();
        if (list == null || groupingKey == null)
            return [];

        return list
            .GroupBy(groupingKey)
            .Select(g => new GroupReport<TKey, T>
            {
                Key = g.Key,
                Count = g.Count(),
                Elements = [.. g]
            })
            .ToList();
    }

    public static List<GroupReport<TOuterKey, TInnerKey, T>> GroupBy<T, TOuterKey, TInnerKey>(
        this IClassWithIList obj,
        Func<T, TOuterKey> outerKeySelector,
        Func<T, TInnerKey> innerKeySelector)
    {
        var list = obj?.GetList<T>();
        if (list == null || outerKeySelector == null || innerKeySelector == null)
            return [];

        return list
            .GroupBy(outerKeySelector)
            .Select(outerGroup =>
            {
                var innerReports = outerGroup
                    .GroupBy(innerKeySelector)
                    .Select(innerGroup => new GroupReport<TInnerKey, T>
                    {
                        Key = innerGroup.Key,
                        Count = innerGroup.Count(),
                        Elements = [.. innerGroup]
                    })
                    .ToList();

                return new GroupReport<TOuterKey, TInnerKey, T>
                {
                    Key = outerGroup.Key,
                    Count = innerReports.Count,
                    Elements = innerReports
                };
            })
            .ToList();
    }
}

class Program
{
    static void Main()
    {
        //----
        // Przygotowanie danych
        //----
        #region Przygotowanie danych
        Subject soop = new("Programowanie obiektowe", "Informatyka", 2, 60);
        Subject sdb = new("Bazy danych", "Informatyka", 3, 45);
        Subject sweb = new("Technologie internetowe", "Informatyka", 4, 45);
        Subject sma = new("Matematyka", "Informatyka", 1, 30);
        Console.WriteLine($"[] Przedmioty po zainicjowaniu:\n{soop}\n{sdb}\n{sweb}\n");

        Lecturer l1 = new("Jan", "Nowak", new DateTime(2040, 6, 1), "dr inż.", "Adiunkt");
        Lecturer l2 = new("Maja", "Kot", new DateTime(2030, 8, 15), "dr", "Asystent");
        Lecturer l3 = new("Jan", "Walas", new DateTime(2045, 12, 6), "prof.", "Profesor");
        Console.WriteLine($"[] Wykładowcy po zainicjowaniu:\n{l1}\n{l2}\n{l3}\n");

        Student s1 = new("Anna", "Misiak", new DateTime(2055, 4, 1), "Informatyka", 1, 2);
        Student s2 = new("Ewa", "Kowalska", new DateTime(2055, 3, 8), "Informatyka", 1, 3);
        Student s3 = new("Olga", "Siejska", new DateTime(2055, 1, 6), "Informatyka", 2, 3);
        Console.WriteLine($"[] Studenci po zainicjowaniu:\n{s1}\n{s2}\n{s3}\n");

        IList<FinalGrade> grades1 = [
            /*new(soop, 5.0, new (2075,6,1)),*/
            new(sdb, 4.0, new DateTime(2075, 5, 1)),
            new(sweb, 3.5, new DateTime(2075, 4, 1))
        ];

        IList<FinalGrade> grades2 = [
            new(soop, 3.0, new DateTime(2075, 6, 1)),
            new(sdb, 2.0, new DateTime(2075, 5, 1)),
            new(sweb, 4.0, new DateTime(2075, 4, 15))
        ];

        IList<FinalGrade> grades3 = [
            new(soop, 5.0, new DateTime(2075, 6, 1)),
            new(sdb, 5.0, new DateTime(2075, 3, 8)),
            new(sweb, 5.0, new DateTime(2075, 3, 15))
        ];
        Console.WriteLine($"[] Listy ocen po zainicjowaniu:\n{grades1}\n{grades2}\n{grades3}\n");

        OrganizationUnit ou1 = new("Katedra TI", "ul. Polna 1", [l1, l2]);
        OrganizationUnit ou2 = new("Katedra BD", "ul. Polna 2", [l3]);
        Lecturer dean = new("Andrzej", "Lewandowski", new DateTime(2035, 2, 14), "dr hab. inż.", "Dziekan");
        Department d1 = new("WIiSI", dean, [soop, sdb, sweb, sma], [s1, s2, s3]);
        Console.WriteLine($"[] Wydział po zainicjowaniu:\n{d1}\n");
        #endregion

        //----
        // Testowanie metod rozszerzeń (TMR): Add, AddRange
        //----
        Console.WriteLine($"[] Studenci przed dodaniem ocen:\n{s1}\n{s2}\n{s3}\n");
        s1.Add<FinalGrade>(new(soop, 5.0, new DateTime(2075, 6, 1)));
        s1.AddRange<FinalGrade>(grades1);
        s2.AddRange<FinalGrade>(grades2);
        s3.AddRange<FinalGrade>(grades3);
        Console.WriteLine($"[] Studenci po dodaniu ocen:\n{s1}\n{s2}\n{s3}\n");

        d1.AddRange<OrganizationUnit>([ou1, ou2]);
        Console.WriteLine($"[] Wydział po dodaniu jednostek:\n{d1}\n");

        //----
        // TMR: Contains, IndexOf, Remove
        //----
        FinalGrade nowaOcena = new(sdb, 4.5, new DateTime(2075, 7, 1));
        s1.Add<FinalGrade>(nowaOcena);
        bool contains1 = s1.Contains<FinalGrade>(nowaOcena); //true
        bool contains2 = s1.Contains<FinalGrade>(new(sdb, 4.5, new DateTime(2075, 7, 4))); //false
        Console.WriteLine($"[] contains1={contains1}, contains2={contains2}\n");

        int index = s1.IndexOf<FinalGrade>(nowaOcena); //3
        Console.WriteLine($"() index={index}\n");

        s1.Remove<FinalGrade>(nowaOcena);
        Console.WriteLine($"[] s1 po usunięciu nowej oceny:\n{s1}\n");

        //----
        // Dodatkowy test: RemoveAt
        //----
        Console.WriteLine("() Test RemoveAt:");
        FinalGrade ocenaMatematyka = new(sma, 5.0, new DateTime(2075, 9, 1));
        s1.Add<FinalGrade>(ocenaMatematyka);
        int idx = s1.IndexOf<FinalGrade>(ocenaMatematyka);
        if (idx != -1) s1.RemoveAt<FinalGrade>(idx);
        Console.WriteLine($"s1 po RemoveAt({idx}):\n{s1}\n");

        //----
        // TMR: Update
        //----
        Console.WriteLine($"[] s2 przed poprawą oceny z BD:\n{s2}\n");
        var stara = s2.Grades.FirstOrDefault(g => g.Subject.Name == "Bazy danych" && g.Value == 2);
        s2.Update<FinalGrade>(stara, new(sdb, 4.5, new DateTime(2075, 9, 30)));
        Console.WriteLine($"[] s2 po poprawie oceny z BD:\n{s2}\n");

        //----
        // TMR: grupowanie (dla chętnych na wyższą liczbę punktów)
        //----
        Console.WriteLine("Grupowanie ocen s1 po nazwie przedmiotu:");
        var grupyA = s1.GroupBy<FinalGrade, string>(g => g.Subject.Name);
        foreach (var g in grupyA) Console.WriteLine(g);
        Console.WriteLine();

        Console.WriteLine("[] Grupowanie ocen s3 po wartości i nazwie przedmiotu:");
        var grupyB = s3.GroupBy<FinalGrade, double, string>(
            g => g.Value,
            g => g.Subject.Name
        );
        foreach (var g in grupyB) Console.WriteLine(g);
        Console.WriteLine();

        //----
        // TMR: ForEach, RemoveAll
        //----
        Console.WriteLine("() Oceny s2 (ForEach):");
        s2.ForEach<FinalGrade>(g => Console.WriteLine($"{s2.FirstName} ma ocenę {g.Value} z {g.Subject.Name}."));
        Console.WriteLine();

        s2.RemoveAll<FinalGrade>(g => g.Value < 3.0);
        Console.WriteLine($"[] s2 po usunięciu ocen niedostatecznych: {s2}\n");

        d1.RemoveAll<Student>(s => s.FirstName == "Anna");
        Console.WriteLine($"[] Wydział po usunięciu studentów o imieniu 'Anna': {d1}\n");

        //----
        // TMR: Clear, ForEach
        //----
        Console.WriteLine("() Przedmioty przed usunięciem:");
        d1.ForEach<Subject>(p => Console.WriteLine(p));
        d1.Clear<Subject>();
        Console.WriteLine();
        Console.WriteLine("() Wykaz przedmiotów po usunięciu:");
        d1.ForEach<Subject>(p => Console.WriteLine(p));
        Console.WriteLine();
    }
}
