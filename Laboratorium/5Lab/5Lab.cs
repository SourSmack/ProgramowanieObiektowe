
public class Office
{
    public string Name { get; set; }
    public int OfficeId { get; set; }
    public ICollection<Trip> Trips { get; set; }
}

public class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class TourGuide
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TourGuideId { get; set; }
}

public class Attraction
{
    public int AttractionId { get; set; }
    public string Name { get; set; }
}

public class Trip
{
    public ICollection<Attraction> Attractions { get; set; }
    public ICollection<Customer> Customers { get; set; }
    public string Name { get; set; }
    public Office Office { get; set; }
    public int OfficeId { get; set; }
    public TourGuide TourGuide { get; set; }
    public int TourGuideId { get; set; }
    public int TripId { get; set; }
}
