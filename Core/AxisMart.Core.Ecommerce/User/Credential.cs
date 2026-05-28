using AxisMart.Framework;

namespace AxisMart.Core.Ecommerce.User;

public class Credential : Entity
{
    public Credential()
    {

    }

    private Credential(Guid id, string hash, Guid userId)
    {
        Id = id;
        Hash = hash;
        //Salt = salt;
        UserId = userId;
    }

    public string Hash { get; private set; }
    //public string Salt { get; private set; }
    public short ChangeTimes { get; private set; } = 0;

    public Guid UserId { get; private set; }
    public User User { get; set; } = null!;


    public static Credential Create(string hash, Guid userId)
    {
        Credential credential = new(Guid.CreateVersion7(), hash, userId);

        return credential;
    }

    public void SetPasswod(string hash)
    {
        Hash = hash;
    }
}
