using System.Runtime.Serialization;

namespace Customer.Domain.Filters;

public enum OrderEnum
{
    Ascending = 0,
    Descending = 1
}

[DataContract]
public class _BaseFilter
{
    public _BaseFilter()
    {
        Skip = 0;
        Take = 10;
        TotalRecords = 0;
        IsPaginated = true;
    }

    [DataMember]
    public int Skip { get; set; }

    [DataMember]
    public int Take { get; set; }

    [DataMember]
    public int TotalRecords { get; set; }

    [DataMember]
    public string SearchText { get; set; }

    [DataMember]
    public bool IsPaginated { get; set; }

    [DataMember]
    public string Sort { get; set; }
}
