using AxisMart.Core.Ecommerce.User.ValueObjects;
using AxisMart.Framework;
using System.ComponentModel.DataAnnotations;

namespace AxisMart.Core.Ecommerce.User;

public class OneTimePassword : Entity
{
    private OneTimePassword(Guid id, int code, string emailOrPhone, DateTime creationTime, DateTime penaltyTime)
    {
        Id = id;
        Code = code;
        EmailOrPhone = emailOrPhone;
        CreationDateTime = creationTime;
        PenaltyTime = penaltyTime;
    }

    protected OneTimePassword()
    {
        
    }

    [MaxLength(10)]
    public int Code { get; private set; } // 6 Digit Code
    public string EmailOrPhone { get; private set; } // {Phone / Email} It Could be Phone Number Or Email Address
    public DateTime CreationDateTime { get; private set; }
    public DateTime PenaltyTime { get; private set; }
    public bool IsExpired { get; private set; } = false;
    //public bool IsActived { get; set; } = false;

    public static OneTimePassword Create(Guid id, int code, string emailOrPhone, DateTime creationTime, DateTime penaltyTime)
    {
        OneTimePassword otp = new(id, code, emailOrPhone, creationTime, penaltyTime);

        otp.RaiseDomainEvent(new OneTimePasswordIssuedDomainEvent(new Phone(emailOrPhone), code));

        return otp;
    }
}


// Send SMS Or Email Notification
public sealed record OneTimePasswordIssuedDomainEvent(Phone Phone, int Otp) : IDomainEvent;