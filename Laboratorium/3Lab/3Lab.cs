



public abstract class Person{
    public string FirstName{ get; set ; } 
    public string LastName{ get; set ; } 

    public Person(){
        FirstName = "" ;
        LastName = "" ;
    }
    public Person( string firstName , string lastName ){
        FirstName = firstName ;
        LastName = lastName ;
    }
    public override string  ToString(){
        return FirstName + " " + LastName  ;
    }
}

class Author : Person {
    public string Nationality{ get ; set ; } 
    public Author():base() {
        Nationality = "" ;
    }
    public Author( string firstName , string lastName , string nationality)
        :base( firstName , lastName ) {
        Nationality = nationality  ;
    }
    public override string ToString(){
        return FirstName + " " + LastName  + " " + Nationality  ;
    }
}

class Librarian : Person {
    public DateTime HireDate{ get ; set; } 
    public decimal Salary{ get ; set ; }
    public Librarian():base() {
        HireDate = default ; 
        Salary = default ;
    }
    public Librarian( string firstName , string lastName , DateTime hireDate , decimal salary  ):base( firstName , lastName ) {
        HireDate = hireDate ; 
        Salary = salary ;
    }
    public override string ToString(){
        return FirstName + " " + LastName  + " " + HireDate + " " + Salary   ;
    }
}

interface IBarCodeGenerator {
   public string GenerateBarCode();

   
}


abstract class Item : IBarCodeGenerator {

    public DateTime DateOfIssue { get; set ; } 
    public int Id { get; set ; }
    public string Publisher { get ;set ; } 
    public string Title { get ; set ; } 

    public override string GenerateBarCode();

    protected Item(){
        DateOfIssue  = default ;
        Id = 0 ;
        Publisher =  "" ;
        Title = "" ;
    }
    public Item(string title, int id, string publisher, DateTime dateOfIssue) {
        DateOfIssue = dateOfIssue ;
        Id = id ;
        Publisher = publisher ;
        Title = title ; 
    }
    public override ToString(){
        return $"{Title}/{Id}/{Publisher}/{DateOfIssue}."
    }

}

class Journal : Item {
    public int Number{ get ; set ; } 
    public Journal(): base() {
        Number = 0 ;
    }
    public override string GenerateBarCode(){
        return $"J-{Id}-{Data}- {Number}." ;
    }

    public Journal( string title , int id , string publisher , DateTime dataOfIssue , int number)
    : base( title, id , publisher , dataOfIssue  )
    {
        Number = number ; 
    }
    public override string GenerateBarCode(){
        return $"J-{Id}-{DateOfIssue}-{Number}" ;
    }

}

class Book : Item {
    public List<Author> Authors{ get ; set ;}
    public int PageCount{ get ; set  ; } 

    public void AddAuthor( Author  author ){
        if (Authors.Count >= Authors.Capacity) 
        {
            throw new ArgumentOutOfRangeException(nameof(Authors), "Author capacity reached.");
        }
        Authors.Add( author) ;
    }

    public Book() : base() {
        Authors = new() ;
        PageCount = 0 ;
    }

    public Book(  string title , int id , string publisher , DateTime dataOfIssue , int pageCount ,  List<Author> authors  ) 
    : base( title, id , publisher , dataOfIssue  )
     {  
        Authors = authors ;
        PageCount = pageCount ;
    }

    public void RemoveAuthor( Author author ){
        Authors.Remove( author ) ;
    }

    public override string GenerateBarCode(){
        return $"B-{Id}-{DateOfIssue}-{Publisher}" ;
    }

    public override string ToString(){
        return $"{Title}/{Id}/{Publisher}/{DateOfIssue}/{PageCount}/{Authors}" ;
    }
}



public interface IItemManagement
{
    Item? FindItem(Func<Item, bool> predicate);
    Item? FindItemBy(int id);
    Item? FindItemBy(string title);
    string GetAllItems(string s = "");
}

public class Catalog : IItemManagement
{
    public string ThematicDepartment { get; set; }
    public List<Item> Items { get; set; }

    public Catalog(List<Item> items)
    {
        ThematicDepartment = string.Empty;
        Items = items ?? new List<Item>();
    }

    public Catalog(string thematicDepartment, List<Item> items)
    {
        ThematicDepartment = thematicDepartment ?? string.Empty;
        Items = items ?? new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (!Items.Contains(item))
        {
            Items.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        if (item != null)
        {
            Items.Remove(item);
        }
    }

    public string GetAllItems(string s = "")
    {
        string header = string.IsNullOrEmpty(s) ? "" : s + "\n";
        return header + string.Join("\n", Items.Select(i => i.ToString()));
    }

    public Item? FindItemBy(int id)
    {
        return Items.FirstOrDefault(i => i.Id == id);
    }

    public Item? FindItemBy(string title)
    {
        return Items.FirstOrDefault(i => string.Equals(i.Title, title, StringComparison.OrdinalIgnoreCase));
    }

    public Item? FindItem(Func<Item, bool> predicate)
    {
        return Items.FirstOrDefault(predicate);
    }

    public Item? FindItemWithExpression(Func<Item, bool> predicate)
    {
        return Items.FirstOrDefault(predicate);
    }

    public Item? FindItemWithPredicate(Func<Item, bool> predicate)
    {
        return Items.FirstOrDefault(predicate);
    }

    public override string ToString()
    {
        return $"Katalog: {ThematicDepartment}\nPozycje:\n" + string.Join("\n", Items.Select(i => i.ToString()));
    }
}

public class Library : IItemManagement
{
    public string Address { get; set; }
    public List<Librarian> Librarians { get; set; }
    public List<Catalog> Catalogs { get; set; }

    public Library()
    {
        Address = string.Empty;
        Librarians = new List<Librarian>();
        Catalogs = new List<Catalog>();
    }

    public Library(string address, List<Librarian> librarians, List<Catalog> catalogs)
    {
        Address = address ?? string.Empty;
        Librarians = librarians ?? new List<Librarian>();
        Catalogs = catalogs ?? new List<Catalog>();
    }

    public void AddLibrarian(Librarian librarian)
    {
        if (librarian != null && !Librarians.Contains(librarian))
        {
            Librarians.Add(librarian);
        }
    }

    public void RemoveLibrarian(Librarian librarian)
    {
        if (librarian != null)
        {
            Librarians.Remove(librarian);
        }
    }

    public void AddCatalog(Catalog catalog)
    {
        if (catalog != null && !Catalogs.Contains(catalog))
        {
            Catalogs.Add(catalog);
        }
    }

    public void RemoveCatalog(Catalog catalog)
    {
        if (catalog != null)
        {
            Catalogs.Remove(catalog);
        }
    }

    public void AddItem(Item item, string thematicDepartment)
    {
        var catalog = Catalogs.FirstOrDefault(c => string.Equals(c.ThematicDepartment, thematicDepartment, StringComparison.OrdinalIgnoreCase));
        if (catalog == null)
        {
            throw new InvalidOperationException($"Nie znaleziono katalogu o dziale tematycznym: {thematicDepartment}");
        }
        catalog.AddItem(item);
    }

    public string GetAllLibrarians(string s = "")
    {
        string header = string.IsNullOrEmpty(s) ? "" : s + "\n";
        return header + string.Join("\n", Librarians.Select(l => l.ToString()));
    }

    public string GetAllItems(string s = "")
    {
        string header = string.IsNullOrEmpty(s) ? "" : s + "\n";
        return header + string.Join("\n", Catalogs.Select(c => c.ToString()));
    }

    public Item? FindItemBy(int id)
    {
        return Catalogs.SelectMany(c => c.Items).FirstOrDefault(i => i.Id == id);
    }

    public Item? FindItemBy(string title)
    {
        return Catalogs.SelectMany(c => c.Items).FirstOrDefault(i => string.Equals(i.Title, title, StringComparison.OrdinalIgnoreCase));
    }

    public Item? FindItem(Func<Item, bool> predicate)
    {
        return Catalogs.SelectMany(c => c.Items).FirstOrDefault(predicate);
    }

    public override string ToString()
    {
        return $"Biblioteka: {Address}";
    }
}



class GroupedItemsbyKeyReport<TKey, TValue>{

}


static class GroupItemsHelper{

}







class Program{
    static void Main(){
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy operacji podstawowych");
        Console.WriteLine("-----------------------------------------------------------");
        Author a1 = new("Anna","Tracz","polska");
        Author a2 = new("Jan","Bielik","polska");
        Author a3 = new("Maria","Miłka","polska");
        Item i1 = new Book("Kompendium programisty",2,"PolPress",new
        DateTime(2065,12,06),500,[a1,a2]);
        ((Book)i1).AddAuthor(a3);
        Item i4 = new Book("Kompendium administratora baz danych",3,"PolPress",new
        DateTime(2065,05,01),500,[a3]);
        ((Book)i4).AddAuthor(a1);
        Item i2 = new Journal("Przegląd techniczny",1,"MyPress",new
        DateTime(2060,06,01),1);
        var bookBarCode = ((Book)i1).GenerateBarCode();
        Console.WriteLine($"{i1} \r\n Kod kreskowy: {bookBarCode}");
        var journalBarCode = ((Journal)i2).GenerateBarCode();
        Console.WriteLine($"{i2} \r\n Kod kreskowy: {journalBarCode}");
        List<Item> items1 = [i1,i2,i4];
        Catalog c1 = new("Książki o programowaniu",items1);
        c1.AddItem(new Journal("Wzorce programistyczne",4,"ITPress",new
        DateTime(2060,02,14),1));
        Console.WriteLine(c1);
        Console.WriteLine('\n'+c1.GetAllItems("Wszystkie pozycje w katalogu:"));
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy wyszukiwania przedmiotów");
        Console.WriteLine("-----------------------------------------------------------");
        string searchedValue = "Kompendium programisty";
        Item? foundedItemByTitle = c1?.FindItem(item => item.Title==searchedValue);
        Item? foundedItemById = c1?.FindItem(item => item.Id==1);
        Item? foundedItemByDateRange = c1?.FindItem(
        item => item.DateOfIssue>=new DateTime(2055,01,01)&&
        item.DateOfIssue<=new DateTime(2085,12,31)
        );
        Console.WriteLine("Wyszukanie po id:\n"+foundedItemById);
        Console.WriteLine("Wyszukanie po tytule:\n"+foundedItemByTitle);
        Console.WriteLine("Wyszukanie po datach:\n"+foundedItemByDateRange);
        Item? foundedItemByIdOld = c1?.FindItemBy(1);
        Item? foundedItemByTitleOld = c1?.FindItemBy(searchedValue);
        Console.WriteLine("Wyszukanie po id (wersja 2):\n"+foundedItemByIdOld);
        Console.WriteLine("Wyszukanie po tytule (wersja 2):\n"+foundedItemByTitleOld);
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy dla bibliotek");
        Console.WriteLine("-----------------------------------------------------------");
        15
        Person l1 = new Librarian("Maja","Kowal",DateTime.Now.Date,2040);
        Person l2 = new Librarian("Jan","Morus",DateTime.Now.Date,2040);
        Library lib1 = new("Częstochowa, Armii Krajowej 36",[(Librarian)l1],[]);
        lib1.AddLibrarian((Librarian)l2);
        Console.WriteLine(lib1.GetAllLibrarians("Wszyscy bibliotekarze:"));
        Catalog c2 = new("Powieści",[]);
        lib1.AddCatalog(c2);
        if(c1!=null) lib1.AddCatalog(c1);
        Item i3 = new Book("Głos większości",4,"Nasze wersy",new
        DateTime(2061,03,08),800,[ a1 ]);
        lib1.AddItem(i3,"Powieści");
        Console.WriteLine(lib1);
        Console.WriteLine(lib1.GetAllItems("Wszystkie pozycje w bibliotece:"));
        var foundedById = lib1.FindItemBy(4);
        var foundedByTitle = lib1.FindItemBy(searchedValue);
        var foundedByLambda = lib1.FindItem(x => x.Publisher=="ITPress");
        Console.WriteLine(foundedById);
        Console.WriteLine(foundedByTitle);
        Console.WriteLine(foundedByLambda);
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy dla dodawania i usuwania przedmiotów");
        Console.WriteLine("-----------------------------------------------------------");
        Catalog c3 = new("Literatura fantastyczna",[]);
        Library lib2 = new("Warszawa, Marszałkowska 12",[],[c3]);
        var bookToAdd = new Book("Lot ku centrum",5,"Super Press",new
        DateTime(2060,06,01),350,[a1]);
        c3.AddItem(bookToAdd);
        Console.WriteLine("Po dodaniu książki:");
        Console.WriteLine(c3.GetAllItems("Pozycje w katalogu:"));
        c3.RemoveItem(bookToAdd);
        Console.WriteLine("Po usunięciu książki:");
        Console.Write(c3.GetAllItems("Pozycje w katalogu:"));
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy wyszukiwania z bardziej złożonymi warunkami");
        Console.WriteLine("-----------------------------------------------------------");
        var foundByMultipleConditions =
        lib1?.FindItem(x => x.Publisher=="ITPress"&&x.DateOfIssue>=new
        DateTime(2040,01,01));
        Console.WriteLine("Wyszukiwanie po wielu warunkach:");
        Console.WriteLine(foundByMultipleConditions);
        Console.WriteLine("Pozycja zawierająca w tytule 'ę':");
        Console.WriteLine(lib1?.Catalogs[0].FindItem(c =>
        c.Title.Contains('ę'))?.ToString());
        Console.WriteLine("Pozycja zawierająca w tytule 'ę':");
        Console.WriteLine(lib1?.Catalogs[0].FindItemWithPredicate(c =>
        c.Title.Contains('ę'))?.ToString());
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy dla błędnych argumentów");
        Console.WriteLine("-----------------------------------------------------------");
        16
        Item? nonExistentItem = lib1?.FindItemBy(999);
        Console.WriteLine("Wyszukiwanie nieistniejącego przedmiotu (ID 999):");
        Console.WriteLine(nonExistentItem?.ToString()??"Nie znaleziono przedmiotu");
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy grupowania po roku wydania (metoda generyczna)");
        Console.WriteLine("-----------------------------------------------------------");
        lib1 ??= new();
        List<GroupedItemsByKeyReport<int,Item>>? groupedByYear =
        GroupItemsHelper.GroupItemsBy(
        lib1.Catalogs.SelectMany(c => c.Items).ToList(),
        item => item.DateOfIssue.Year
        );
        if(groupedByYear is not null) foreach(var e in
        groupedByYear)Console.WriteLine(e);
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy grupowania po wydawcy (metoda generyczna)");
        Console.WriteLine("-----------------------------------------------------------");
        List<GroupedItemsByKeyReport<string,Item>>? groupedByPublisher =
        GroupItemsHelper.GroupItemsBy(
        lib1.Catalogs.SelectMany(c => c.Items).ToList(),
        item => item.Publisher
        );
        if(groupedByPublisher is not null) foreach(var e in groupedByPublisher)
        Console.WriteLine(e);
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy grupowania książek po liczbie stron (m. generyczna)");
        Console.WriteLine("-----------------------------------------------------------");
        List<GroupedItemsByKeyReport<int,Book>>? groupedByPageCount =
        GroupItemsHelper.GroupItemsBy(
        lib1.Catalogs.SelectMany(c => c.Items).OfType<Book>().ToList(),
        item => item.PageCount
        );
        if(groupedByPageCount is not null) foreach(var e in groupedByPageCount)
        Console.WriteLine(e);
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("Testy grupowania autorów po narodowości (m. generyczna)");
        Console.WriteLine("-----------------------------------------------------------");
        List<GroupedItemsByKeyReport<string,Author>>? groupedByNationality =
        GroupItemsHelper.GroupItemsBy(
        lib1.Catalogs
        .SelectMany(c => c.Items)
        .OfType<Book>()
        .SelectMany(b => b.Authors)
        .Distinct()
        .ToList(),
        item => item.Nationality
        );
        if(groupedByNationality is not null) foreach(var e in groupedByNationality)
        Console.WriteLine(e);
    }

}
