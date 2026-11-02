

public enum StopType {
    Standard,
    OnDemand
}

public enum RouteType {
    Tram,
    Bus
}

public enum DrivingLicense {
    Tram,
    Bus
}

public class Stop {
    public string Name { get; set; }
    public string Address { get; set; }
    public StopType Type { get; set; }

    public Stop(string name, string address,
        StopType type = StopType.Standard) {
        Name = name;
        Address = address;
        Type = type;
    }
}

public class Route {
    public int Number { get; set; }
    public string Name { get; set; }
    public double LengthKm { get; set; }
    public RouteType RouteType { get; set; }
    public List<Stop> Stops { get; } = [];

    public Route (int number, string name, double lengthKm,
        RouteType routeType) {
        if (!double.IsFinite(lengthKm) || lengthKm <= 0)
            throw new ArgumentException(
                "Długość trasy musi być dodatnia.");
        
        Number = number;
        Name = name;
        LengthKm = lengthKm;
        RouteType = routeType;
    }

    public void AddStop (Stop stop) {
        ArgumentNullException.ThrowIfNull(stop);
        Stops.Add(stop);
    }
}

public class Vehicle {
    public int FleetNumber { get; set; }
    public double Mileage { get; private set; }
    public double MileageAtLastInspection { get; private set; }
    protected double InspectionIntervalKm { get; }

    public Vehicle(int fleetNumber, double inspectionIntervalKm) {
        if (!double.IsFinite(inspectionIntervalKm) ||
            inspectionIntervalKm <= 0)
            throw new ArgumentException(
                "Limit kilometrów musi być dodatni.");
        FleetNumber = fleetNumber;
        InspectionIntervalKm = inspectionIntervalKm;
    }

    public void AddMileage (double kilometers) {
        if (!double.IsFinite(kilometers) || kilometers <= 0)
            throw new ArgumentException(
                "Liczba kilometrów musi być dodatnia.");
        Mileage += kilometers;
    }

    public bool RequiresInspection() {
        return Mileage - MileageAtLastInspection >= InspectionIntervalKm;
    }

    public void PerformInspection() {
        MileageAtLastInspection = Mileage;
    }

    public virtual decimal CalculateOperatingCost(double kilometers) {
        if (!double.IsFinite(kilometers) || kilometers < 0)
            throw new ArgumentException(
                "Liczba kilometrów nie może być ujemna.");
        return 0m;
    }
}

public class Bus : Vehicle {
    public decimal FuelConsumptionPer100Km { get; }
    public decimal FuelPricePerLiter { get; }

    public Bus(int fleetNumber, decimal fuelConsumptionPer100Km,
        decimal fuelPricePerLiter) : base (fleetNumber, 15000) {
        if (fuelConsumptionPer100Km <= 0)
            throw new ArgumentException("Spalanie musi być dodatnie.");
        if (fuelPricePerLiter < 0)
            throw new ArgumentException("Cena paliwa nie może być ujemna.");
        
        FuelConsumptionPer100Km = fuelConsumptionPer100Km;
        FuelPricePerLiter = fuelPricePerLiter;
    }

    public override decimal CalculateOperatingCost(double kilometers) {
        if (!double.IsFinite(kilometers) || kilometers < 0)
            throw new ArgumentException(
                "Liczba kilometrów nie może być ujemna.");
        
        decimal fuelConsumed =
            (decimal)kilometers * FuelConsumptionPer100Km / 100m;
        return fuelConsumed * FuelPricePerLiter;
    }
}

public class Tram : Vehicle {
    public decimal EnergyConsumptionKWhPerKm { get; }
    public decimal ElectricityPricePerKWh { get; }

    public Tram(int fleetNumber, decimal energyConsumptionKWhPerKm,
        decimal electricityPricePerKWh) : base (fleetNumber, 20000) {
        if (energyConsumptionKWhPerKm <= 0)
            throw new ArgumentException(
                "Zużycie energii musi być dodatnie.");
        if (electricityPricePerKWh < 0)
            throw new ArgumentException("Cena energii nie może być ujemna.");
        
        EnergyConsumptionKWhPerKm = energyConsumptionKWhPerKm;
        ElectricityPricePerKWh = electricityPricePerKWh;
    }

    public override decimal CalculateOperatingCost(double kilometers) {
        if (!double.IsFinite(kilometers) || kilometers < 0)
            throw new ArgumentException(
                "Liczba kilometrów nie może być ujemna.");
        
        return (decimal) kilometers * EnergyConsumptionKWhPerKm *
            ElectricityPricePerKWh;
    }
}

public class Employee {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Employee (int id, string firstName, string lastName) {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public virtual decimal CalculateSalary() {
        return 0m;
    }
}

public class Driver : Employee {
    public decimal HourlyRate { get; set; }
    public int HoursWorked { get; set; }
    public DrivingLicense DrivingLicense { get; set; }

    public Driver(int id, string firstName, string lastName,
        decimal hourlyRate, int hoursWorked,
        DrivingLicense drivingLicense) : base (id, firstName, lastName) {
        if (hourlyRate < 0)
            throw new ArgumentException(
                "Stawka godzinowa nie może być ujemna.");
        if (hoursWorked < 0)
            throw new ArgumentException(
                "Liczba godzin nie może być ujemna.");
        
        HourlyRate = hourlyRate;
        HoursWorked = hoursWorked;
        DrivingLicense = drivingLicense;
    }

    public override decimal CalculateSalary() {
        return HourlyRate * HoursWorked;
    }
}

public class OfficeEmployee : Employee {
    public decimal MonthlySalary { get; set; }

    public OfficeEmployee (int id, string firstName, string lastName,
        decimal salary) : base(id, firstName, lastName) {
        if (salary < 0)
            throw new ArgumentException("Pensja nie może być ujemna.");
        MonthlySalary = salary;
    }

    public override decimal CalculateSalary() {
        return MonthlySalary;
    }
}

public class Trip {
    public Driver Driver { get; }
    public Vehicle Vehicle { get; }
    public Route Route { get; }
    public DateTime Date { get; }

    public Trip (Driver driver, Vehicle vehicle, Route route,
        DateTime date) {
        ArgumentNullException.ThrowIfNull(driver);
        ArgumentNullException.ThrowIfNull(vehicle);
        ArgumentNullException.ThrowIfNull(route);
        
        Driver = driver;
        Vehicle = vehicle;
        Route = route;
        Date = date;
    }

    public decimal CalculateCost() {
        return Vehicle.CalculateOperatingCost(Route.LengthKm);
    }

    public void RegisterTrip() {
        Vehicle.AddMileage(Route.LengthKm);
    }
}

public class Payment {
    public Employee Employee { get; }
    public DateTime Date { get; }
    public decimal Amount { get; }

    public Payment (Employee employee, DateTime date, decimal amount) {
        Employee = employee;
        Date = date;
        Amount = amount;
    }
}

public class PublicTransportCompany {
    public List<Vehicle> Vehicles { get; } = [];
    public List<Driver> Drivers { get; } = [];
    public List<Employee> OfficeEmployees { get; } = [];
    public List<Route> Routes { get; } = [];
    public List<Trip> Trips { get; } = [];
    public List<Payment> Payments { get; } = [];

    public void AddVehicle(Vehicle vehicle) {
        ArgumentNullException.ThrowIfNull(vehicle);
        if(Vehicles.Any(v => v.FleetNumber == vehicle.FleetNumber))
            throw new InvalidOperationException("Numer taborowy już istnieje.");
        Vehicles.Add(vehicle);
    }

    public void AddDriver (Driver driver) {
        ArgumentNullException.ThrowIfNull(driver);
        if(Drivers.Any(d => d.Id == driver.Id))
            throw new InvalidOperationException(
                $"Kierowca o ID {driver.Id} już istnieje.");
        Drivers.Add(driver);
    }

    public void AddOfficeEmployee (Employee officeEmployee) {
        ArgumentNullException.ThrowIfNull(officeEmployee);
        if(OfficeEmployees.Any(d => d.Id == officeEmployee.Id))
            throw new InvalidOperationException(
                $"Pracownik o ID {officeEmployee.Id} już istnieje.");
        OfficeEmployees.Add(officeEmployee);
    }

    public void AddRoute (Route route) {
        ArgumentNullException.ThrowIfNull(route);
        if (Routes.Any(r => r.Number == route.Number))
            throw new InvalidOperationException("Numer trasy już istnieje.");
        Routes.Add(route);
    }

    public void AddTrip (Trip trip) {
        ArgumentNullException.ThrowIfNull(trip);
        if (!Vehicles.Contains(trip.Vehicle))
            throw new InvalidOperationException("Pojazd nie należy do MPK.");
        if (!Drivers.Contains(trip.Driver))
            throw new InvalidOperationException("Kierowca nie należy do MPK.");
        if (!Routes.Contains(trip.Route))
            throw new InvalidOperationException("Trasa nie należy do MPK.");
        Trips.Add(trip);
    }

    public List<Vehicle> GetVehiclesRequiringInspection() {
        return [.. Vehicles.Where(v => v.RequiresInspection())];
    }

    public static decimal CalculatePayment(Employee employee) {
        ArgumentNullException.ThrowIfNull(employee);
        return employee.CalculateSalary();
    }

    public Payment PaySalary (Employee employee, DateTime date) {
        ArgumentNullException.ThrowIfNull(employee);
        decimal amount = CalculatePayment(employee);
        Payment payment = new(employee, date, amount);
        Payments.Add(payment);
        return payment;
    }
}

public class Program {
    public static void Main() {
        PublicTransportCompany company = new();
        Driver tramDriver = new(2, "Anna", "Nowak", 40m, 160, DrivingLicense.Tram);
        Driver busDriver = new(1, "Jan", "Kowalski", 35m, 160, DrivingLicense.Bus);
        company.AddDriver(tramDriver);
        company.AddDriver( busDriver);
        Employee officeEmployee = new OfficeEmployee(3, "Piotr", "Wiśniewski", 6000m);
        company.AddOfficeEmployee(officeEmployee);
        
        Vehicle bus = new Bus(101, 30m, 6.50m);
        Vehicle tram = new Tram(201, 3m, 1.20m);
        company.AddVehicle(bus);
        company.AddVehicle(tram);
        
        Route busRoute = new(10, "Centrum - Dworzec", 20, RouteType.Bus);
        busRoute.AddStop(new Stop("Centrum", "Plac Centralny", StopType.Standard));
        busRoute.AddStop(new Stop("Park", "Ulica Parkowa", StopType.OnDemand));
        
        Route tramRoute = new(20, "Centrum - Zajezdnia", 15, RouteType.Tram);
        tramRoute.AddStop(new Stop("Dworzec", "Ulica Kolejowa", StopType.Standard));
        company.AddRoute(busRoute);
        company.AddRoute(tramRoute);
        
        Trip busTrip = new(busDriver, bus, busRoute, DateTime.Today);
        Trip tramTrip = new(tramDriver, tram, tramRoute, DateTime.Today);
        company.AddTrip(busTrip);
        company.AddTrip(tramTrip);
        
        Console.WriteLine("\n Koszty eksploatacji:");
        Console.WriteLine($"Koszt kursu autobusu: {busTrip.CalculateCost():C}");
        Console.WriteLine($"Koszt kursu tramwaju: {tramTrip.CalculateCost():C}");
        
        Console.WriteLine("\n Wynagrodzenia:");
        List<Employee> employees = [busDriver, tramDriver, officeEmployee];
        foreach (Employee employee in employees)
            Console.WriteLine(
                $"{employee.FirstName} {employee.LastName}:" +
                $" {PublicTransportCompany.CalculatePayment(employee):C}"
            );
            
        Console.WriteLine("\n Trasy i przystanki:");
        Console.WriteLine($"Trasa: {busRoute.Name}, rodzaj: {busRoute.RouteType}");
        foreach(Stop stop in busRoute.Stops)
            Console.WriteLine($"{stop.Name} {stop.Type}");
            
        Console.WriteLine($"Uprawnienia Jana: {busDriver.DrivingLicense}");
        
        Console.WriteLine("\n Zakończenia kursów: ");
        busTrip.RegisterTrip();
        tramTrip.RegisterTrip();
        Console.WriteLine($"Przebieg autobusu: {bus.Mileage} km");
        Console.WriteLine($"Przebieg tramwaju: {tram.Mileage} km");
        
        Console.WriteLine("\n Przeglądy");
        bus.AddMileage(15000);
        foreach (Vehicle vehicle in company.GetVehiclesRequiringInspection())
            Console.WriteLine(
                $"Pojazd {vehicle.FleetNumber} wymaga przeglądu." +
                $" Przebieg: {vehicle.Mileage} km"
            );
            
        bus.PerformInspection();
        Console.WriteLine(
            $"Czy autobus wymaga przeglądu po wykonaniu przeglądu?" +
            $" {(bus.RequiresInspection() ? "Tak" : "Nie")}"
        );
        
        Console.WriteLine("\n Wypłaty:");
        Payment payment = company.PaySalary(busDriver, DateTime.Today);
        Console.WriteLine(
            $"Wypłacono {payment.Employee.FirstName}" +
            $" {payment.Employee.LastName}: {payment.Amount:C}"
        );
        
        Console.ReadKey();
    }
}