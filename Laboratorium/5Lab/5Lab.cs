using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TravelAgency.Model
{
    public class Office
    {
        public int OfficeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class TourGuide
    {
        public int TourGuideId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class Attraction
    {
        public int AttractionId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Trip
    {
        public int TripId { get; set; }
        public string Name { get; set; } = string.Empty;

        public int OfficeId { get; set; }
        public Office Office { get; set; } = null!;

        public int TourGuideId { get; set; }
        public TourGuide TourGuide { get; set; } = null!;

        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
    }
}

namespace TravelAgency.DAL
{
    using TravelAgency.Model;

    public class TravelDbContext : DbContext
    {
        public DbSet<Office> Offices { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<Attraction> Attractions { get; set; }

        public TravelDbContext()
        {
        }

        public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TravelAgencyDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Office)
                .WithMany(o => o.Trips)
                .HasForeignKey(t => t.OfficeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.TourGuide)
                .WithMany()
                .HasForeignKey(t => t.TourGuideId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Customers)
                .WithMany();

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Attractions)
                .WithMany();
        }
    }
}

namespace TravelAgency.BLL
{
    using TravelAgency.DAL;
    using TravelAgency.Model;

    public interface IOfficeService
    {
        IEnumerable<Office> GetAllOffices();
        Office? GetOfficeById(int id);
        void CreateOffice(string name);
        void UpdateOffice(int id, string name);
        void DeleteOffice(int id);
    }

    public class OfficeService : IOfficeService
    {
        private readonly TravelDbContext _context;

        public OfficeService(TravelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Office> GetAllOffices()
        {
            return _context.Offices
                .Include(o => o.Trips)
                .ToList();
        }

        public Office? GetOfficeById(int id)
        {
            return _context.Offices
                .Include(o => o.Trips)
                .FirstOrDefault(o => o.OfficeId == id);
        }

        public void CreateOffice(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nazwa biura nie może być pusta.");

            var office = new Office { Name = name };
            _context.Offices.Add(office);
            _context.SaveChanges();
        }

        public void UpdateOffice(int id, string name)
        {
            var office = _context.Offices.Find(id)
                ?? throw new KeyNotFoundException("Nie znaleziono biura.");

            office.Name = name;
            _context.Offices.Update(office);
            _context.SaveChanges();
        }

        public void DeleteOffice(int id)
        {
            var office = _context.Offices.Find(id)
                ?? throw new KeyNotFoundException("Nie znaleziono biura.");

            _context.Offices.Remove(office);
            _context.SaveChanges();
        }
    }

    public interface ITourGuideService
    {
        IEnumerable<TourGuide> GetAllTourGuides();
        TourGuide? GetTourGuideById(int id);
        void CreateTourGuide(string firstName, string lastName);
        void DeleteTourGuide(int id);
    }

    public class TourGuideService : ITourGuideService
    {
        private readonly TravelDbContext _context;

        public TourGuideService(TravelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TourGuide> GetAllTourGuides() => _context.TourGuides.ToList();

        public TourGuide? GetTourGuideById(int id) => _context.TourGuides.Find(id);

        public void CreateTourGuide(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Imię i nazwisko przewodnika są wymagane.");

            var guide = new TourGuide { FirstName = firstName, LastName = lastName };
            _context.TourGuides.Add(guide);
            _context.SaveChanges();
        }

        public void DeleteTourGuide(int id)
        {
            var guide = _context.TourGuides.Find(id)
                ?? throw new KeyNotFoundException("Nie znaleziono przewodnika.");

            _context.TourGuides.Remove(guide);
            _context.SaveChanges();
        }
    }

    public interface ICustomerService
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer? GetCustomerById(int id);
        void CreateCustomer(string firstName, string lastName);
        void DeleteCustomer(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly TravelDbContext _context;

        public CustomerService(TravelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAllCustomers() => _context.Customers.ToList();

        public Customer? GetCustomerById(int id) => _context.Customers.Find(id);

        public void CreateCustomer(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Imię i nazwisko klienta są wymagane.");

            var customer = new Customer { FirstName = firstName, LastName = lastName };
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id)
                ?? throw new KeyNotFoundException("Nie znaleziono klienta.");

            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }

    public interface IAttractionService
    {
        IEnumerable<Attraction> GetAllAttractions();
        Attraction? GetAttractionById(int id);
        void CreateAttraction(string name);
        void DeleteAttraction(int id);
    }

    public class AttractionService : IAttractionService
    {
        private readonly TravelDbContext _context;

        public AttractionService(TravelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Attraction> GetAllAttractions() => _context.Attractions.ToList();

        public Attraction? GetAttractionById(int id) => _context.Attractions.Find(id);

        public void CreateAttraction(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nazwa atrakcji nie może być pusta.");

            var attraction = new Attraction { Name = name };
            _context.Attractions.Add(attraction);
            _context.SaveChanges();
        }

        public void DeleteAttraction(int id)
        {
            var attraction = _context.Attractions.Find(id)
                ?? throw new KeyNotFoundException("Nie znaleziono atrakcji.");

            _context.Attractions.Remove(attraction);
            _context.SaveChanges();
        }
    }

    public interface ITripService
    {
        IEnumerable<Trip> GetAllTrips();
        Trip? GetTripById(int id);
        void CreateTrip(string name, int officeId, int tourGuideId);
        void AssignCustomerToTrip(int tripId, int customerId);
        void AssignAttractionToTrip(int tripId, int attractionId);
        void DeleteTrip(int id);
    }

    public class TripService : ITripService
    {
        private readonly TravelDbContext _context;

        public TripService(TravelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips
                .Include(t => t.Office)
                .Include(t => t.TourGuide)
                .Include(t => t.Customers)
                .Include(t => t.Attractions)
                .ToList();
        }

        public Trip? GetTripById(int id)
        {
            return _context.Trips
                .Include(t => t.Office)
                .Include(t => t.TourGuide)
                .Include(t => t.Customers)
                .Include(t => t.Attractions)
                .FirstOrDefault(t => t.TripId == id);
        }

        public void CreateTrip(string name, int officeId, int tourGuideId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nazwa wycieczki nie może być pusta.");

            if (_context.Offices.Find(officeId) == null)
                throw new KeyNotFoundException("Wybrane biuro podróży nie istnieje.");

            if (_context.TourGuides.Find(tourGuideId) == null)
                throw new KeyNotFoundException("Wybrany przewodnik nie istnieje.");

            var trip = new Trip
            {
                Name = name,
                OfficeId = officeId,
                TourGuideId = tourGuideId
            };

            _context.Trips.Add(trip);
            _context.SaveChanges();
        }

        public void AssignCustomerToTrip(int tripId, int customerId)
        {
            var trip = GetTripById(tripId)
                ?? throw new KeyNotFoundException("Nie znaleziono wycieczki.");

            var customer = _context.Customers.Find(customerId)
                ?? throw new KeyNotFoundException("Nie znaleziono klienta.");

            if (!trip.Customers.Any(c => c.CustomerId == customerId))
            {
                trip.Customers.Add(customer);
                _context.SaveChanges();
            }
        }

        public void AssignAttractionToTrip(int tripId, int attractionId)
        {
            var trip = GetTripById(tripId)
                ?? throw new KeyNotFoundException("Nie znaleziono wycieczki.");

            var attraction = _context.Attractions.Find(attractionId)
                ?? throw new KeyNotFoundException("Nie znaleziono atrakcji.");

            if (!trip.Attractions.Any(a => a.AttractionId == attractionId))
            {
                trip.Attractions.Add(attraction);
                _context.SaveChanges();
            }
        }

        public void DeleteTrip(int id)
        {
            var trip = _context.Trips.Find(id)
                ?? throw new KeyNotFoundException("Nie znaleziono wycieczki.");

            _context.Trips.Remove(trip);
            _context.SaveChanges();
        }
    }
}

namespace TravelAgency.UI
{
    using TravelAgency.BLL;
    using TravelAgency.DAL;

    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new TravelDbContext();
            context.Database.EnsureCreated();

            IOfficeService officeService = new OfficeService(context);
            ITourGuideService guideService = new TourGuideService(context);
            ICustomerService customerService = new CustomerService(context);
            IAttractionService attractionService = new AttractionService(context);
            ITripService tripService = new TripService(context);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== SYSTEM ZARZĄDZANIA BIUREM PODRÓŻY ===");
                Console.WriteLine("1. Dodaj biuro podróży");
                Console.WriteLine("2. Dodaj przewodnika");
                Console.WriteLine("3. Dodaj klienta");
                Console.WriteLine("4. Dodaj atrakcję turystyczną");
                Console.WriteLine("5. Utwórz wycieczkę");
                Console.WriteLine("6. Przypisz klienta do wycieczki");
                Console.WriteLine("7. Przypisz atrakcję do wycieczki");
                Console.WriteLine("8. Wyświetl wszystkie wycieczki");
                Console.WriteLine("0. Wyjście");
                Console.Write("Wybierz opcję: ");

                string? choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Nazwa biura: ");
                            officeService.CreateOffice(Console.ReadLine() ?? string.Empty);
                            Console.WriteLine("Dodano biuro.");
                            break;

                        case "2":
                            Console.Write("Imię przewodnika: ");
                            string gFirst = Console.ReadLine() ?? string.Empty;
                            Console.Write("Nazwisko przewodnika: ");
                            string gLast = Console.ReadLine() ?? string.Empty;
                            guideService.CreateTourGuide(gFirst, gLast);
                            Console.WriteLine("Dodano przewodnika.");
                            break;

                        case "3":
                            Console.Write("Imię klienta: ");
                            string cFirst = Console.ReadLine() ?? string.Empty;
                            Console.Write("Nazwisko klienta: ");
                            string cLast = Console.ReadLine() ?? string.Empty;
                            customerService.CreateCustomer(cFirst, cLast);
                            Console.WriteLine("Dodano klienta.");
                            break;

                        case "4":
                            Console.Write("Nazwa atrakcji: ");
                            attractionService.CreateAttraction(Console.ReadLine() ?? string.Empty);
                            Console.WriteLine("Dodano atrakcję.");
                            break;

                        case "5":
                            foreach (var o in officeService.GetAllOffices())
                                Console.WriteLine($"[Biuro ID: {o.OfficeId}] {o.Name}");
                            Console.Write("Podaj ID biura: ");
                            int officeId = int.Parse(Console.ReadLine() ?? "0");

                            foreach (var g in guideService.GetAllTourGuides())
                                Console.WriteLine($"[Przewodnik ID: {g.TourGuideId}] {g.FirstName} {g.LastName}");
                            Console.Write("Podaj ID przewodnika: ");
                            int guideId = int.Parse(Console.ReadLine() ?? "0");

                            Console.Write("Nazwa wycieczki: ");
                            string tripName = Console.ReadLine() ?? string.Empty;

                            tripService.CreateTrip(tripName, officeId, guideId);
                            Console.WriteLine("Utworzono wycieczkę.");
                            break;

                        case "6":
                            foreach (var t in tripService.GetAllTrips())
                                Console.WriteLine($"[Wycieczka ID: {t.TripId}] {t.Name}");
                            Console.Write("Podaj ID wycieczki: ");
                            int tIdCust = int.Parse(Console.ReadLine() ?? "0");

                            foreach (var c in customerService.GetAllCustomers())
                                Console.WriteLine($"[Klient ID: {c.CustomerId}] {c.FirstName} {c.LastName}");
                            Console.Write("Podaj ID klienta: ");
                            int custId = int.Parse(Console.ReadLine() ?? "0");

                            tripService.AssignCustomerToTrip(tIdCust, custId);
                            Console.WriteLine("Przypisano klienta do wycieczki.");
                            break;

                        case "7":
                            foreach (var t in tripService.GetAllTrips())
                                Console.WriteLine($"[Wycieczka ID: {t.TripId}] {t.Name}");
                            Console.Write("Podaj ID wycieczki: ");
                            int tIdAttr = int.Parse(Console.ReadLine() ?? "0");

                            foreach (var a in attractionService.GetAllAttractions())
                                Console.WriteLine($"[Atrakcja ID: {a.AttractionId}] {a.Name}");
                            Console.Write("Podaj ID atrakcji: ");
                            int attrId = int.Parse(Console.ReadLine() ?? "0");

                            tripService.AssignAttractionToTrip(tIdAttr, attrId);
                            Console.WriteLine("Przypisano atrakcję do wycieczki.");
                            break;

                        case "8":
                            var trips = tripService.GetAllTrips();
                            foreach (var t in trips)
                            {
                                Console.WriteLine($"\nWycieczka [{t.TripId}]: {t.Name}");
                                Console.WriteLine($"  Biuro: {t.Office?.Name}");
                                Console.WriteLine($"  Przewodnik: {t.TourGuide?.FirstName} {t.TourGuide?.LastName}");
                                Console.WriteLine($"  Atrakcje: {string.Join(", ", t.Attractions.Select(a => a.Name))}");
                                Console.WriteLine($"  Uczestnicy: {string.Join(", ", t.Customers.Select(c => $"{c.FirstName} {c.LastName}"))}");
                            }
                            break;

                        case "0":
                            running = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
            }
        }
    }
}
