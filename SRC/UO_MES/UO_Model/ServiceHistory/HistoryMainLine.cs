using System;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Execution;
using UO_Model.Execution.Code;
using UO_Model.Physical;
using UO_Model.Process;

namespace UO_Model.ServiceHistory
{
    [DataContract]
    [Persistent(IdentityField = "historyMainLine_id")]
    public class HistoryMainLine
    {
        private int historyMainLine_id;
        [FieldAlias("historyMainLine_id")]
        public int HistoryMainLine_ID
        {
            get { return historyMainLine_id; }
            set { historyMainLine_id = value; }
        }

        private int stepPass;
        [DataMember]
        [FieldAlias("stepPass")]
        public int StepPass
        {
            get { return stepPass; }
            set { stepPass = value; }
        }

        //String
        private string txnName;
        [DataMember]
        [FieldAlias("txnName")]
        public string TxnName
        {
            get { return txnName; }
            set { txnName = value; }
        }

        private string comments;
        [DataMember]
        [FieldAlias("comments")]
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        //DateTime
        private DateTime mfgDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("mfgDate")]
        public DateTime MfgDate
        {
            get { return mfgDate; }
            set { mfgDate = value; }
        }

        private DateTime systemDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("systemDate")]
        public DateTime SystemDate
        {
            get { return systemDate; }
            set { systemDate = value; }
        }

        private DateTime txnDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("txnDate")]
        public DateTime TxnDate
        {
            get { return txnDate; }
            set { txnDate = value; }
        }


        //Bool
        private bool bonusIncluded = false;
        [DataMember]
        [FieldAlias("bonusIncluded")]
        public bool BonusIncluded
        {
            get { return bonusIncluded; }
            set { bonusIncluded = value; }
        }

        private bool defectIncluded = false;
        [DataMember]
        [FieldAlias("defectIncluded")]
        public bool DefectIncluded
        {
            get { return defectIncluded; }
            set { defectIncluded = value; }
        }

        private bool lossIncluded = false;
        [DataMember]
        [FieldAlias("lossIncluded")]
        public bool LossIncluded
        {
            get { return lossIncluded; }
            set { lossIncluded = value; }
        }

        private bool inRework = false;
        [DataMember]
        [FieldAlias("inRework")]
        public bool InRework
        {
            get { return inRework; }
            set { inRework = value; }
        }

        private bool localReworkIncluded = false;
        [DataMember]
        [FieldAlias("localReworkIncluded")]
        public bool LocalReworkIncluded
        {
            get { return localReworkIncluded; }
            set { localReworkIncluded = value; }
        }

        //Object
        private Container container;
        [DataMember]
        [FieldAlias("container")]
        public Container Container
        {
            get { return container; }
            set { container = value; }
        }

        private Employee employee;  // Perform this txn employee
        [DataMember]
        [FieldAlias("employee")]
        public Employee Employee
        {
            get { return employee; }
            set { employee = value; }
        }

        private Employee loginEmployee;
        [DataMember]
        [FieldAlias("loginEmployee")]
        public Employee LoginEmployee
        {
            get { return loginEmployee; }
            set { loginEmployee = value; }
        }

        private Factory factory;
        [DataMember]
        [FieldAlias("factory")]
        public Factory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        private Operation operation;
        [DataMember]
        [FieldAlias("operation")]
        public Operation Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        private OwnerCode ownerCode;
        [DataMember]
        [FieldAlias("ownerCode")]
        public OwnerCode OwnerCode
        {
            get { return ownerCode; }
            set { ownerCode = value; }
        }

        private Product product;
        [DataMember]
        [FieldAlias("product")]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        private Resource resource;
        [DataMember]
        [FieldAlias("resource")]
        public Resource Resource
        {
            get { return resource; }
            set { resource = value; }
        }

        private Spec spec;
        [DataMember]
        [FieldAlias("spec")]
        public Spec Spec
        {
            get { return spec; }
            set { spec = value; }
        }
    }
}
