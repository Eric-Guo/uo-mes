using System;
using Telerik.OpenAccess;
using UO_Model.Base;
using UO_Model.Physical;
using UO_Model.ServiceHistory;

namespace UO_Service.Base
{
    abstract public class ShopFloor : Service
    {
        abstract protected string TxnName { get; }

        #region ShopFloor transaction public properties for users
        private string comments;
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private Factory factory;
        virtual public Factory Factory
        {
            get { return factory; }
            set { factory = ResolveCDO(value.GetType(), value.Name) as Factory; }
        }
        virtual public string Factory_Name
        {
            get { return factory.Name; }
            set { factory = ResolveCDO("Factory", value) as Factory; }
        }

        private Employee loginEmployee;
        public Employee LoginEmployee
        {
            get { return loginEmployee; }
        }
        virtual public string LoginEmployee_Name
        {
            get { return loginEmployee.Name; }
            set { loginEmployee = ResolveCDO("Employee", value) as Employee; }
        }

        private Employee employee;
        virtual public Employee Employee
        {
            get { return employee; }
            set { employee = ResolveCDO(value.GetType(), value.Name) as Employee; }
        }
        #endregion

        /// <summary>
        /// Override this method to validate if the service meet the pre-request
        /// </summary>
        /// <returns>return true if all input is valid</returns>
        abstract protected bool ValidateService();

        /// <summary>
        /// Override this method to do actually modify MES CDO action.
        /// </summary>
        /// <param name="objScope">ORM ObjectScope</param>
        /// <returns>return true if modify entity is successful.</returns>
        abstract protected bool ModifyEntity();

        /// <summary>
        /// Override this method to record specify service history.
        /// </summary>
        /// <returns>return true if record service history is sccessful</returns>
        abstract protected bool RecordServiceHistory();

        /// <summary>
        /// Validate and Execute the service
        /// </summary>
        /// <returns>true if service is executed successful</returns>
        public bool ExecuteService()
        {
            if (Validate() == false)
                return false;
            return Execute();
        }

        /// <summary>
        /// Validate the service to check all pre-request to execute the service is meet
        /// </summary>
        /// <returns>return true if service is all validate</returns>
        public bool Validate()
        {
            if (ValidateService() == false)
                return false;
            return true;
        }

        /// <summary>
        /// To do the acturally modify in the service.
        /// </summary>
        /// <returns>return true if execute is successful.</returns>
        public bool Execute()
        {
            bool success = true;
            bool txnActiveAlready = ObjScope.Transaction.IsActive;
            // If txn is active already, means such ShopFloor transaction is used by another txn internally
            if(false == txnActiveAlready)
                ObjScope.Transaction.Begin();

            initializeHistoryMainline();

            success = ModifyEntity();
            if (ContinueOrRollback(success) == false)
                return success;

            success = RecordServiceHistory();
            if (ContinueOrRollback(success) == false)
                return success;

            success = AssignToHistoryMainLine(historyMainLine);
            if (ContinueOrRollback(success) == false)
                return success;

            if(false == txnActiveAlready)
                ObjScope.Transaction.Commit();
            return true;
        }

        private void initializeHistoryMainline()
        {
            if (ObjScope.IsNew(historyMainLine))
            {
                ObjScope.Add(historyMainLine);
            }
            HistoryMainLine.TxnDate = TxnDate;
        }

        #region AssignTo
        protected virtual bool AssignToHistoryMainLine(HistoryMainLine h)
        {
            h.TxnName = this.TxnName;
            h.Comments = this.Comments;
            h.LoginEmployee = this.LoginEmployee;
            h.Employee = this.Employee;
            h.Factory = this.Factory;
            return true;
        }
        #endregion

        protected ShopFloor() : base() { }
        protected ShopFloor(IObjectScope existObjScope, HistoryMainLine reusedHML) : base(existObjScope)
        {
            historyMainLine = reusedHML;
        }

        private HistoryMainLine historyMainLine = new HistoryMainLine();
        protected HistoryMainLine HistoryMainLine
        {
            get { return historyMainLine; }
        }

        protected bool ContinueOrRollback(bool success)
        {
            if (success == false)
                ObjScope.Transaction.Rollback();
            return success;
        }
    }
}
