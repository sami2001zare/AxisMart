internal class OrSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override bool IsSatisfiedBy(T entity)
    {
        return left.IsSatisfiedBy(entity) || right.IsSatisfiedBy(entity);
    }
}
