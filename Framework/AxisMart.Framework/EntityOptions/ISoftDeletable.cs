namespace AxisMart.Framework.EntityOptions;

public abstract class SoftDeletable
{
    public bool IsDeleted { get; private set; } = false;
}

public abstract class CreatationDateTime
{
    public DateTime CreatedAt { get; private set; }
}

public abstract class ModifiedDateTime
{
    public DateTime ModifiedAt { get; set; }
}
