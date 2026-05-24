using AxisMart.Core.Ecommerce.User.ValueObjects;
using AxisMart.Framework;

namespace AxisMart.Core.Ecommerce.User;

public abstract class User : Entity
{
    //private User(Guid id, FirstName firstName, LastName lastName)
    //{
    //    Id = id;
    //    FirstName = firstName;
    //    LastName = lastName;
    //}

    public FirstName FirstName { get; protected set; }
    public LastName LastName { get; protected set; }
    public Phone Phone { get; protected set; }

    public Credential? Credential { get; private set; }

    public List<JsonWebToken> WebTokens { get; set; } = [];


    protected void SetCredential(string hash, Guid userId)
    {
        Credential = Credential.Create(hash, userId);
    }
}
