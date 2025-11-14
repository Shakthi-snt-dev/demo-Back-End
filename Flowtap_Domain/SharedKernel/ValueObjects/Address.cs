using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.SharedKernel.ValueObjects;

/// <summary>
/// Value Object representing an address.
/// Value objects are immutable and compared by value, not reference.
/// </summary>
public class Address
{
    // Private parameterless constructor for EF Core
    private Address() { }

    public Address(string streetNumber, string streetName, string city, string state, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(streetNumber))
            throw new ArgumentException("Street number cannot be empty", nameof(streetNumber));
        if (string.IsNullOrWhiteSpace(streetName))
            throw new ArgumentException("Street name cannot be empty", nameof(streetName));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State cannot be empty", nameof(state));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));

        StreetNumber = streetNumber;
        StreetName = streetName;
        City = city;
        State = state;
        PostalCode = postalCode;
    }

    [Required]
    public string StreetNumber { get; private set; } = string.Empty;

    [Required]
    public string StreetName { get; private set; } = string.Empty;

    [Required]
    public string City { get; private set; } = string.Empty;

    [Required]
    public string State { get; private set; } = string.Empty;

    [Required]
    public string PostalCode { get; private set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not Address other) return false;
        return StreetNumber == other.StreetNumber &&
               StreetName == other.StreetName &&
               City == other.City &&
               State == other.State &&
               PostalCode == other.PostalCode;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StreetNumber, StreetName, City, State, PostalCode);
    }

    public override string ToString()
    {
        return $"{StreetNumber} {StreetName}, {City}, {State} {PostalCode}";
    }
}

