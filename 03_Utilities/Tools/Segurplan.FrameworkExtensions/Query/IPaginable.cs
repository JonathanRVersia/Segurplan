namespace Segurplan.FrameworkExtensions.Query {
    public interface IPaginable {
        bool IsPaginated { get; }
        int? Page { get; }
        int? PageSize { get; }
        int? SkippedRows { get; }
        int? TotalCount { get; }
    }
}
