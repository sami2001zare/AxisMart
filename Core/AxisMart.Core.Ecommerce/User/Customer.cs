using AxisMart.Core.Ecommerce.User.Events;
using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Core.Ecommerce.User;

public class Customer : User
{
    private Customer(Guid id, FirstName firstName, LastName lastName, Phone phone)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
    }

    private Customer(Guid id, FirstName firstName, LastName lastName, Email email, Phone phone)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }

    protected Customer()
    {
        
    }

    public Email? Email { get; private set; }
    public bool IsEmailVerified { get; private set; } = false;
    public bool IsPhoneVerified { get; private set; } = false;

    public static Customer Register(Guid id, FirstName firstName, LastName lastName, Phone phone)
    {
        Customer customer = new(id, firstName, lastName, phone);

        customer.RaiseDomainEvent(new CustomerFirstStepRegisteredEvent(phone));

        return customer;
    }

    public static Customer Register(Guid id, FirstName firstName, LastName lastName, Email email, Phone phone)
    {
        Customer customer = new(id, firstName, lastName, email, phone);

        // This Events Creates An 6 Digit OTP Code With Expiration And Expiration Time Skew
        customer.RaiseDomainEvent(new CustomerFirstStepRegisteredEvent(phone));

        return customer;
    }


    public void VerifyEmail()
    {
        IsEmailVerified = true;
    }

    public void VerifyPhone()
    {
        IsEmailVerified = true;
    }
}
