



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

    public abstract string GenerateBarCode();

    protected Item(){
        DateOfIssue  = default ;
        Id = 0 ;
        Publisher =  "" ;
        Title = "" ;
    }
}

class Journal : Item {
    public int Number{ get ; set ; } 
    public Journal(): base() {
        Number = 0 ;
    }
    public Journal( string title , int id , string publisher , DateTime dataOfIssue , int number){
        DateOfIssue = dataOfIssue ;
        Id = id ;
        Publisher = publisher ;
        Title = title ; 
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

    public Book(  string title , int id , string publisher , DateTime dataOfIssue , int pageCount , List<Author> authors  )  {  
            DateOfIssue = dataOfIssue ;
        Id = id ;
        Publisher = publisher ;
        Title = title ; 
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



interface IItemManagement{

}


class Catalog : IItemManagement{


}

class Library : IItemManagement {

}





class GroupedItemsbyKeyReport<TKey, TValue>{

}


static class GroupItemsHelper{

}







class Program{
    static void Main(){
        Console.WriteLine( "Hello world");
        Console.Read();
    }

}
