using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Process.Code;
using UO_Model.Physical;
using UO_Model.ServiceHistory;

namespace UO_Model.Process
{
    [DataContract]
    [Persistent(IdentityField = "dataCollectionDef_id")]
    public class DataCollectionDef : RevisionedObject
    {
        private int dataCollectionDef_id;
        [FieldAlias("dataCollectionDef_id")]
        public int DataCollectionDef_ID
        {
            get { return dataCollectionDef_id; }
            set { dataCollectionDef_id = value; }
        }

        private DataCollectionDefBase revBase;
        [DataMember]
        [FieldAlias("revBase")]
        override public RevisionBase RevBase
        {
            get
            {
                if (revBase == null)
                    revBase = new DataCollectionDefBase();
                return revBase;
            }
            set { revBase = value as DataCollectionDefBase; }
        }

        private int status;
        [DataMember]
        [FieldAlias("status")]
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private string parametricDataControl;
        [DataMember]
        [FieldAlias("parametricDataControl")]
        public string ParametricDataControl
        {
            get { return parametricDataControl; }
            set { parametricDataControl = value; }
        }

        private int parametricDataDefType;
        [DataMember]
        [FieldAlias("parametricDataDefType")]
        public int ParametricDataDefType
        {
            get { return parametricDataDefType; }
            set { parametricDataDefType = value; }
        }        
    }
}
