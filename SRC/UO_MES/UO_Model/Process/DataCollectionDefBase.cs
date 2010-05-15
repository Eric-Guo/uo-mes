using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "dataCollectionDef_id")]
    public class DataCollectionDefBase : RevisionBase
    {
        private int dataCollectionDef_id;
        [FieldAlias("dataCollectionDef_id")]
        public int DataCollectionDef_id
        {
            get { return dataCollectionDef_id; }
            set { dataCollectionDef_id = value; }
        }

        private DataCollectionDef currentDataCollectionDef;
        [DataMember]
        [FieldAlias("currentDataCollectionDef")]
        override public RevisionedObject Current
        {
            get { return currentDataCollectionDef; }
            set { currentDataCollectionDef = value as DataCollectionDef; }
        }
    }
}
