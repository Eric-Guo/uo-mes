using Telerik.OpenAccess;

namespace UO_Model.Base
{
    /// <summary>
    /// Service History Detail
    /// 
    /// Summary of the changes caused by a single ServiceDetail CDO.
    /// </summary>
    [Persistent]
    abstract public class ServiceHistoryDetail
    {
        abstract public ServiceHistorySummary ServiceHistorySummary { get; set; }
    }
}
