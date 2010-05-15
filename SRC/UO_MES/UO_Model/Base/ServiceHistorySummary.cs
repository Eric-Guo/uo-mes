using System.Collections.Generic;
using Telerik.OpenAccess;
using UO_Model.ServiceHistory;

namespace UO_Model.Base
{
    /// <summary>
    /// Service History Summary
    /// 
    /// History Summary object for a Container or Resource Service.  
    /// One ServiceHistorySummary will be created for every Container or Resource transaction. 
    /// The ServiceHistorySummary object and its associated HistoryDetails subentity lists (if necessary) 
    /// will contain all the history information needed for the service that is not already contained in 
    /// the HistoryMainline object.
    /// </summary>
    [Persistent]
    abstract public class ServiceHistorySummary
    {
        private HistoryMainLine historyMainLine;
        [FieldAlias("historyMainLine")]
        public HistoryMainLine HistoryMainLine
        {
            get { return historyMainLine; }
            set { historyMainLine = value; }
        }

        abstract public IList<ServiceHistoryDetail> ServiceHistoryDetails { get; }
    }
}
