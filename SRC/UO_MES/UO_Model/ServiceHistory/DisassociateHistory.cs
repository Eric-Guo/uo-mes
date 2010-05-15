using System;
using System.Collections.Generic;
using System.Text;
using UO_Model.Base;
using Telerik.OpenAccess;
using UO_Model.Execution;

namespace UO_Model.ServiceHistory
{
    [Persistent(IdentityField = "disassociateHistory_id")]
    public class DisassociateHistory : ServiceHistorySummary
    {
        private int disassociateHistory_id;
        [FieldAlias("disassociateHistory_id")]
        public int DisassociateHistory_ID
        {
            get { return disassociateHistory_id; }
            set { disassociateHistory_id = value; }
        }

        private IList<DisassociateHistoryDetail> disassociateHistoryDetails = new List<DisassociateHistoryDetail>();
        [FieldAlias("disassociateHistoryDetails")]
        public override IList<ServiceHistoryDetail> ServiceHistoryDetails
        {
            get { return disassociateHistoryDetails as IList<ServiceHistoryDetail>; }
        }

        private bool disassociateAll = false;
        [FieldAlias("disassociateAll")]
        public bool DisassociateAll
        {
            get { return disassociateAll; }
            set { disassociateAll = value; }
        }
    }

    [Persistent(IdentityField = "disassociateHistoryDetail_id")]
    public class DisassociateHistoryDetail : ServiceHistoryDetail
    {
        private int disassociateHistoryDetail_id;
        [FieldAlias("disassociateHistoryDetail_id")]
        public int DisassociateHistoryDetail_ID
        {
            get { return disassociateHistoryDetail_id; }
            set { disassociateHistoryDetail_id = value; }
        }

        private DisassociateHistory disassociateHistory;
        [FieldAlias("disassociateHistory")]
        public override ServiceHistorySummary ServiceHistorySummary
        {
            get { return disassociateHistory; }
            set { disassociateHistory = value as DisassociateHistory; }
        }

        private Container childContainer;
        [FieldAlias("childContainer")]
        public Container ChildContainer
        {
            get { return childContainer; }
            set { childContainer = value; }
        }
    }
}
