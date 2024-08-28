using System.Collections.Generic;
using System;

public class Customer
{
    public int CustomerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; }

    public string Password { get; set; }

    public ICollection<Order> Orders { get; set; }
}