


using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

class Car : IDisposable
{
    //private static int CarCount = 0 ;
    public static int CarCount{ get  ; private set ;} = 0  ;
    public Car(){ CarCount ++ ; }
    public Car( string name , string model , int doorCount , double engineVolume , double avgConsump , string registrationNum )
    {
        Brand  = name ;
        Model = model ; 
        DoorCount = doorCount; 
        EngineVolume = engineVolume ;
        AvgConsump = avgConsump ; 
        RegistrationNumber = registrationNum ;

        CarCount ++ ;

    }
    public void Dispose()
    {
        CarCount -- ;
    }
    

    public string Brand{ get ; set ; } = "" ;
    public string Model{ get ; set ; } = "" ;
    public int DoorCount{ get ; set ; }
    private double _engineVolume = 0 ; 
    public double EngineVolume{ get => Math.Round( _engineVolume , 2 ) ; set => _engineVolume = value  ; }
    private double _avgConsump = 0 ;
    public double AvgConsump{ get => Math.Round( _avgConsump , 2 ) ; set => _avgConsump = value  ; }
    public string RegistrationNumber{ get ; set ; } = "" ;
    
    public double CalculateFuelConsumption( double roadLength )
    {
        
        return AvgConsump / 100.0 * roadLength ;
    }
    public double CalculateCostOfTheTrip(double roadLength, double petrolCost )
    {
        return Math.Round( petrolCost * CalculateFuelConsumption( roadLength )) ;
    }

    public override string ToString() {
        return string.Format(
        "Samochód: {0}/{1}/{2}/{3}/{4}/{5}",
        Brand, Model, DoorCount, EngineVolume, AvgConsump, RegistrationNumber
    );
    }
}


class Garage
{
    public string Address{ get ; set ; } = "" ;
    public int Capacity{ get ; set ; } 
    public List<Car> Cars{ get ; set ; } 
    public int CarsCount{ get => Cars.Count ; }

    public Garage() : this(string.Empty, 0) { }
    public Garage(string address, int capacity) {
    Address = address;
    Capacity = capacity;
    Cars = new List<Car>(Capacity);
    }


    public bool CarIn(Car car) {
        if (car == null || Cars.Count >= Capacity) return false;
        foreach (Car c in Cars){ 
            if (c.Equals(car)) return false;
        }
        Cars.Add(car);
        return true;
    }
    public Car? CarOut(){
        if ( Cars.Count <= 0 ) return null;
        var result =  Cars[ Cars.Count - 1 ] ;
        Cars.RemoveAt( Cars.Count - 1 ) ;
        return result ;
    }
    public bool CarOut( Car car ){
        if ( Cars.Capacity <= 0 ) return false;
        return Cars.Remove( car )  ;
    }
    public Car? CarOut( string registrationNumber ){
        Car result = new() ;
        foreach ( var car in Cars)
        {
            if ( car.RegistrationNumber == registrationNumber){ 
                return ( Cars.Remove( car ) ) ? car : null ;
            }
        }
        return null ;
    }
    public double CalculateAverageFuelConsumption() {
        double sum = 0;
        foreach ( var car in Cars ) {
            sum += car.AvgConsump ;
        }
        return Cars.Count == 0 ? 0 : sum / Cars.Count;
    }
    public List<Car> GetCarsByBrand( string brand )
    {
        List<Car> result  = new()  ;
        foreach ( var car in Cars)
        {
            if ( car.Brand == brand )  result.Add( car )  ;
        }
        return result  ;
    }
    public override string ToString() {
        string s = string.Format("Garaż {0} ({1}/{2} samochodów):", Address, Cars.Count, Capacity);
        foreach (Car car in Cars) {
            s += "\n- " + car;
        }
        return s;
    }

}

class Person
{
    private List<Car> _cars;

    public string Address{ get ; set ; } = "" ;
    public int CarsCount{ get => _cars.Count ; }
    public string FirstName{ get ; set ; } = "" ;
    public string LastName { get ; set; }  = "" ;
    public static int MaxCarCount{ get ; private set ;} = 3  ;

    public Person()
    {
        _cars = new() ;
    }
    public Person( string address , string name , string surname)
    {
        Address = address ;
        FirstName = name ;
        LastName = surname ;
        _cars = new() ;
    }
    public Person( string address , string name , string surname , List<Car> cars)
    {
        Address = address ;
        FirstName = name ;
        LastName = surname ;
        _cars = cars ;
    }

    

    public bool AddCar( Car car )
    {
        if ( _cars.Count >= MaxCarCount ) return false ;
        foreach( var parkedCar in _cars){
            if ( parkedCar == car ) return false ;
        }
        _cars.Add( car ) ;
        return true ;
    }
    public bool CanAddCar()
    {
        if ( _cars.Count >= 3 ) return false;
        return true;
    }
    
    public bool RemoveCar( string registrationNumber )
    {
        foreach ( var car in _cars )
        {
            if ( car.RegistrationNumber == registrationNumber )
            {
                _cars.Remove( car ) ;
                return true ;
            }
        }
        return false ;
    }
    public bool RemoveCar( Car car )
    {
        foreach( var parkedCar in _cars)
        {
            if ( parkedCar == car)
            {
                _cars.Remove( parkedCar ) ;
                return true; 
            }
        }
        return false ;
    }

    public override string ToString()
    {
       string s = string.Format("Osoba: {0} {1}/{2}/{3}/posiadane samochody:\n",
       FirstName, LastName, Address, CarsCount );
       foreach ( var  car in _cars) {
            s += "\n- " + car;
        }
       return s;
    }
}


class Program
{
    static int Main()
    {
        Console.WriteLine("-----------------------------------------------------------------------");
        Console.WriteLine("Testowanie klasy Car");
        Console.WriteLine("-----------------------------------------------------------------------");
        //Samochód bez parametrów (domyślny konstruktor)
        Car car1 = new Car();
        Console.WriteLine(car1); //Sprawdzamy domyślne wartości pól samochodu
        //Samochód z parametrami (konstruktor z parametrami)
        Car car2 = new Car("Fiat","500",2,1.2f,5.2,"FI500");
        Console.WriteLine(car2); //Sprawdzamy wartości po przypisaniu danych
        Car car3 = new Car("Tesla","Model S",4,0f,15.0,"TS100");
        Console.WriteLine(car3); //Sprawdzamy inny pojazd
        //Testowanie właściwości
        car1.Brand="BMW";
        car1.Model="X5";
        car1.DoorCount=5;
        car1.EngineVolume=3.0f;
        car1.AvgConsump=9.5;
        car1.RegistrationNumber="BMW1234";
        Console.WriteLine(car1); //Właściwości po zmianach
        //Testowanie metod
        double roadLength = 200; //Długość trasy w km
        double petrolCost = 5.2; //Koszt paliwa za litr
        double fuelConsumption = car2.CalculateFuelConsumption(roadLength);
        Console.WriteLine($"Zużycie paliwa dla {roadLength} km: {fuelConsumption} litrów");
        double tripCost = car2.CalculateCostOfTheTrip(roadLength,petrolCost);
        Console.WriteLine($"Koszt paliwa dla {roadLength} km: {tripCost} PLN");
        //Testowanie statycznej zmiennej CarCount
        
        Console.WriteLine($"Liczba samochodów stworzonych: {Car.CarCount}"); //Powinna wynosić 3
        //Testowanie metody ToString()
        Console.WriteLine("Opis samochodu car3: "+car3);
        



        Console.WriteLine("-----------------------------------------------------------------------");
        Console.WriteLine("Testowanie klasy Garage");
        Console.WriteLine("-----------------------------------------------------------------------");
        //Testowanie klasy Garage
        Garage garage = new Garage("Garaż przy ul. Kwiatowej",2);
        Console.WriteLine("Adres garażu: "+garage.Address);
        Console.WriteLine("Pojemność garażu: "+garage.Capacity);
        Console.WriteLine("Liczba samochodów w garażu: "+garage.CarsCount);
        //Dodawanie samochodów do garażu
        Console.WriteLine("Dodawanie samochodów do garażu...");
        garage.CarIn(car1);
        garage.CarIn(car2);
        Console.WriteLine(garage);
        //Sprawdzanie, czy samochód można dodać ponownie (jest już w garażu)
        bool addedAgain = garage.CarIn(car1); //Próba dodania ponownie car1
        Console.WriteLine($"Próba ponownego dodania car1 do garażu: {addedAgain}");
        //Usuwanie samochodu z garażu
        garage.CarOut(car1);
        Console.WriteLine("Po usunięciu car1 z garażu:");
        Console.WriteLine(garage);
        //Testowanie metody CarOut() bez argumentów
        Car? carOut = garage.CarOut();
        Console.WriteLine("Samochód wyjęty z garażu (bez numeru rejestracyjnego): "+(carOut!=null ?
        carOut.ToString() : "Brak"));
        //Testowanie metody CarOut() z numerem rejestracyjnym
        carOut=garage.CarOut("FI500");
        Console.WriteLine("Samochód wyprowadzony z garażu: "+(carOut!=null ? carOut.ToString() : "Brak"));
        //Testowanie metody GetCarsByBrand()
        
        Console.WriteLine("Samochody marki Fiat w garażu:");
        foreach(Car c in garage.GetCarsByBrand("Fiat")) {
            Console.WriteLine(c);
        }
        //Testowanie metody CalculateAverageFuelConsumption()
        double avgFuelConsump = garage.CalculateAverageFuelConsumption();
        Console.WriteLine($"Średnie zużycie paliwa aut w garażu: {avgFuelConsump} l/100km");

        return 0 ;
    }
}



