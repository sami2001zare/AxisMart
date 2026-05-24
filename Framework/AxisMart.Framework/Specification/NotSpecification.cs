internal class NotSpecification<T>(Specification<T> spec) : Specification<T>
{
    public override bool IsSatisfiedBy(T entity)
    {
        return spec.IsSatisfiedBy(entity);
    }
}