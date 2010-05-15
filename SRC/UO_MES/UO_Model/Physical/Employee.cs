using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.Physical
{
    [DataContract]
    [Persistent(IdentityField = "employee_id")]
    public class Employee : UO_Model.Base.NamedObject
    {
        private int employee_id;
        [FieldAlias("employee_id")]
        public int Employee_ID
        {
            get { return employee_id; }
            set { employee_id = value; }
        }

        private string fullName;
        [DataMember]
        [FieldAlias("fullName")]
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        private string email;
        [DataMember]
        [FieldAlias("email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string password;
        [DataMember]
        [FieldAlias("password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string passwordQuestion;
        [DataMember]
        [FieldAlias("passwordQuestion")]
        public string PasswordQuestion
        {
            get { return passwordQuestion; }
            set { passwordQuestion = value; }
        }

        private string passwordAnswer;
        [DataMember]
        [FieldAlias("passwordAnswer")]
        public string PasswordAnswer
        {
            get { return passwordAnswer; }
            set { passwordAnswer = value; }
        }

        private bool isApproved = false;
        [DataMember]
        [FieldAlias("isApproved")]
        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        private DateTime lastActivityDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastActivityDate")]
        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        private DateTime lastLoginDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastLoginDate")]
        public DateTime LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
        }

        private DateTime lastPasswordChangedDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastPasswordChangedDate")]
        public DateTime LastPasswordChangedDate
        {
            get { return lastPasswordChangedDate; }
            set { lastPasswordChangedDate = value; }
        }

        private DateTime creationDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("creationDate")]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        private bool isOnLine = false;
        [DataMember]
        [FieldAlias("isOnLine")]
        public bool IsOnLine
        {
            get { return isOnLine; }
            set { isOnLine = value; }
        }

        private bool isLockedOut = false;
        [DataMember]
        [FieldAlias("isLockedOut")]
        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }

        private DateTime lastLockedOutDate = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("lastLockedOutDate")]
        public DateTime LastLockedOutDate
        {
            get { return lastLockedOutDate; }
            set { lastLockedOutDate = value; }
        }

        private int failedPwdAttemptCount = 0;
        [DataMember]
        [FieldAlias("failedPwdAttemptCount")]
        public int FailedPasswordAttemptCount
        {
            get { return failedPwdAttemptCount; }
            set { failedPwdAttemptCount = value; }
        }

        private DateTime failedPwdAttemptWindowStart = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("failedPwdAttemptWindowStart")]
        public DateTime FailedPasswordAttemptWindowStart
        {
            get { return failedPwdAttemptWindowStart; }
            set { failedPwdAttemptWindowStart = value; }
        }

        private int failedPwdAnsAttemptCount = 0;
        [DataMember]
        [FieldAlias("failedPwdAnsAttemptCount")]
        public int FailedPasswordAnswerAttemptCount
        {
            get { return failedPwdAnsAttemptCount; }
            set { failedPwdAnsAttemptCount = value; }
        }

        private DateTime failedPwdAnsAttemptWindowStart = Constants.NullDateTime;
        [DataMember]
        [FieldAlias("failedPwdAnsAttemptWindowStart")]
        public DateTime FailedPasswordAnswerAttemptWindowStart
        {
            get { return failedPwdAnsAttemptWindowStart; }
            set { failedPwdAnsAttemptWindowStart = value; }
        }

        private IList<Role> roles = new List<Role>();  // inverse Role.employees
        [DataMember]
        [FieldAlias("roles")]
        public IList<Role> Roles
        {
            get { return roles; }
        }
    }
}
