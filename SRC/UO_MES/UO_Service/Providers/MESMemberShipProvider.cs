using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using Telerik.OpenAccess;
using UO_Model.Physical;
using UO_Service.Base;

namespace UO_Service
{
    public sealed class MESMembershipProvider : MembershipProvider
    {
        #region Class Variables
        private int newPasswordLength = 8;
        private string applicationName;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;
        private int maxInvalidPasswordAttempts;
        private int passwordAttemptWindow;
        private MembershipPasswordFormat passwordFormat;
        private int minRequiredNonAlphanumericCharacters;
        private int minRequiredPasswordLength;
        private string passwordStrengthRegularExpression;
        private MachineKeySection machineKey; //Used when determining encryption key values.
        #endregion

        #region Enums
        private enum FailureType
        {
            Password = 1,
            PasswordAnswer = 2
        }
        #endregion

        #region Properties
        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return enablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return enablePasswordRetrieval;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return requiresQuestionAndAnswer;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return requiresUniqueEmail;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return maxInvalidPasswordAttempts;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return passwordAttemptWindow;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return passwordFormat;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return minRequiredNonAlphanumericCharacters;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return minRequiredPasswordLength;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return passwordStrengthRegularExpression;
            }
        }
        #endregion

        #region Initialization
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name == null || name.Length == 0)
            {
                name = "MESMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "MES Membership provider");
            }

            //Initialize the abstract base class.
            base.Initialize(name, config);

            applicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredAlphaNumericCharacters"], "1"));
            minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], String.Empty));
            enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }

            switch (temp_format)
            {
                case "Hashed":
                    passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            //Get encryption and decryption key information from the configuration.
            System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = cfg.GetSection("system.web/machineKey") as MachineKeySection;

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
            {
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                {
                    throw new ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.");
                }
            }
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;
            return configValue;
        }
        #endregion

        #region Implemented Abstract Methods from MembershipProvider
        /// <summary>
        /// Change the user password.
        /// </summary>
        /// <param name="username">UserName</param>
        /// <param name="oldPwd">Old password.</param>
        /// <param name="newPwd">New password.</param>
        /// <returns>T/F if password was changed.</returns>
        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {

            if (!ValidateUser(username, oldPwd))
            {
                return false;
            }

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }
                else
                {
                    throw new Exception("Change password canceled due to new password validation failure.");
                }
            }

            IObjectScope objScope = ORM.GetNewObjectScope();
            objScope.Transaction.Begin();
            Employee e = ResolveEmployeeByName(objScope, username);
            e.Password = EncodePassword(newPwd);
            e.LastPasswordChangedDate = DateTime.Now;
            objScope.Transaction.Commit();
            return true;
        }

        /// <summary>
        /// Change the question and answer for a password validation.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="newPwdQuestion">New question text.</param>
        /// <param name="newPwdAnswer">New answer text.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPwdQuestion, string newPwdAnswer)
        {
            if (!ValidateUser(username, password))
            {
                return false;
            }

            IObjectScope objScope = ORM.GetNewObjectScope();
            objScope.Transaction.Begin();
            Employee e = ResolveEmployeeByName(objScope, username);
            e.PasswordQuestion = newPwdQuestion;
            e.PasswordAnswer = EncodePassword(newPwdAnswer);
            objScope.Transaction.Commit();
            return true;
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="email">Email address.</param>
        /// <param name="passwordQuestion">Security quesiton for password.</param>
        /// <param name="passwordAnswer">Security quesiton answer for password.</param>
        /// <param name="isApproved"></param>
        /// <param name="userID">User ID</param>
        /// <param name="status"></param>
        /// <returns>MembershipUser</returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if ((RequiresUniqueEmail && (GetUserNameByEmail(email) != String.Empty)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser membershipUser = GetUser(username, false);

            if (membershipUser == null)
            {
                IObjectScope objScope = ORM.GetNewObjectScope();
                System.DateTime createDate = DateTime.Now;
                objScope.Transaction.Begin();
                Employee e = new Employee();
                e.Name = username;
                e.Password = EncodePassword(password);
                e.Email = email;
                e.PasswordQuestion = passwordQuestion;
                e.PasswordAnswer = EncodePassword(passwordAnswer);
                e.IsApproved = isApproved;

                objScope.Add(e);
                objScope.Transaction.Commit();

                status = MembershipCreateStatus.Success;
                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="deleteAllRelatedData">Whether to delete all related data.</param>
        /// <returns>T/F if the user was deleted.</returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            objScope.Transaction.Begin();
            Employee e = ResolveEmployeeByName(objScope, username);
            if (deleteAllRelatedData)
            {
                //Process commands to delete all data for the user in the database.
            }
            objScope.Remove(e);
            objScope.Transaction.Commit();
            return true;
        }

        /// <summary>
        /// Get a collection of users.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="totalRecords">Total # of records to retrieve.</param>
        /// <returns>Collection of MembershipUser objects.</returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryAllEmployee = @"SELECT * FROM  EmployeeExtent AS o";
            return QueryMembershipUsers(objScope, queryAllEmployee, null, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Gets the number of users currently on-line.
        /// </summary>
        /// <returns>  /// # of users on-line.</returns>
        public override int GetNumberOfUsersOnline()
        {
            const string queryOnlineNum = @"ELEMENT (SELECT COUNT(*) FROM EmployeeExtent AS o WHERE o.LastActivityDate > $1)";
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            IObjectScope objScope = ORM.GetNewObjectScope();
            IQuery oqlQuery = objScope.GetOqlQuery(queryOnlineNum);
            using (IQueryResult result = oqlQuery.Execute(compareTime))
            {
                return (int)result[0];
            }
        }

        /// <summary>
        /// Get the password for a user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="answer">Answer to security question.</param>
        /// <returns>Password for the user.</returns>
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            IObjectScope objScope = ORM.GetNewObjectScope();
            Employee e = ResolveEmployeeByName(objScope, username);

            if(e.IsLockedOut == true)
                throw new MembershipPasswordException("The supplied user is locked out.");

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, e.PasswordAnswer))
            {
                UpdateFailureCount(objScope, e, FailureType.PasswordAnswer);

                throw new MembershipPasswordException("Incorrect password answer.");
            }

            string password = e.Password;
            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            Employee e = ResolveEmployeeByName(objScope, username);
            if (null == e)
                return null;
            return GetMembershipUser(objScope, userIsOnline, e);
        }

        /// <summary>
        /// Get a user based upon provider key and if they are on-line.
        /// </summary>
        /// <param name="userID">Provider key.</param>
        /// <param name="userIsOnline">T/F whether the user is on-line.</param>
        /// <returns></returns>
        public override MembershipUser GetUser(object userID, bool userIsOnline)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            Employee e = ResolveEmployeeByID(objScope, userID.ToString());
            return GetMembershipUser(objScope, userIsOnline, e);
        }

        /// <summary>
        /// Unlock a user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>T/F if unlocked.</returns>
        public override bool UnlockUser(string username)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            objScope.Transaction.Begin();
            Employee e = ResolveEmployeeByName(objScope, username);
            e.IsLockedOut = false;
            e.LastLockedOutDate = DateTime.Now;
            objScope.Transaction.Commit();
            return true;
        }

        public override string GetUserNameByEmail(string email)
        {
            const string queryEmployeeNameByEmail =
                @"ELEMENT (SELECT o.Name FROM EmployeeExtent AS o WHERE o.Email = $1)";
            IObjectScope objScope = ORM.GetNewObjectScope();
            IQuery oqlQuery = objScope.GetOqlQuery(queryEmployeeNameByEmail);
            using (IQueryResult result = oqlQuery.Execute(email))
            {
                if (result.Count > 0)
                    return result[0].ToString();
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Reset the user password.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="answer">Answer to security question.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ResetPassword(string username, string answer)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            Employee e = ResolveEmployeeByName(objScope, username);

            if(e == null)
                throw new MembershipPasswordException("The supplied user name is not found.");

            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Password Reset is not enabled.");
            }

            if ((answer == null) && (RequiresQuestionAndAnswer))
            {
                UpdateFailureCount(objScope, e, FailureType.PasswordAnswer);

                throw new ProviderException("Password answer required for password Reset.");
            }

            string newPassword =
              System.Web.Security.Membership.GeneratePassword(
              newPasswordLength,
              MinRequiredNonAlphanumericCharacters
              );

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }
                else
                {
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
                }
            }

            if(e.IsLockedOut == true)
                throw new MembershipPasswordException("The supplied user is locked out.");

            if (RequiresQuestionAndAnswer && (!CheckPassword(answer, e.PasswordAnswer)))
            {
                UpdateFailureCount(objScope, e, FailureType.PasswordAnswer);

                throw new MembershipPasswordException("Incorrect password answer.");
            }

            objScope.Transaction.Begin();
            e.Password = EncodePassword(newPassword);
            objScope.Transaction.Commit();
            return newPassword;   
        }

        /// <summary>
        /// Update the user information.
        /// </summary>
        /// <param name="_membershipUser">MembershipUser object containing data.</param>
        public override void UpdateUser(MembershipUser membershipUser)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            Employee e = ResolveEmployeeByName(objScope, membershipUser.UserName);
            objScope.Transaction.Begin();
            e.Email = membershipUser.Email;
            e.Description = membershipUser.Comment;
            e.IsApproved = membershipUser.IsApproved;
            objScope.Transaction.Commit();
        }

        /// <summary>
        /// Validate the user based upon username and password.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>T/F if the user is valid.</returns>
        public override bool ValidateUser(string username, string password)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            bool isValid = false;

            Employee e = ResolveEmployeeByName(objScope, username);
            if (e == null)
                return false;

            if (CheckPassword(password, e.Password))
            {
                if (e.IsApproved)
                {
                    isValid = true;

                    objScope.Transaction.Begin();
                    e.LastLoginDate = DateTime.Now;
                    objScope.Transaction.Commit();
                }
            }
            else
            {
                UpdateFailureCount(objScope, e, FailureType.Password);
            }
            return isValid;
        }

        /// <summary>
        /// Find all users matching a search string.
        /// </summary>
        /// <param name="usernameToMatch">Search string of user name to match.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Total records found.</param>
        /// <returns>Collection of MembershipUser objects.</returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryEmployeesByName = @"SELECT * FROM  EmployeeExtent AS o WHERE o.Name LIKE $1";
            return QueryMembershipUsers(objScope, queryEmployeesByName, usernameToMatch, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Find all users matching a search string of their email.
        /// </summary>
        /// <param name="emailToMatch">Search string of email to match.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Total records found.</param>
        /// <returns>Collection of MembershipUser objects.</returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryEmployeesByEMail = @"SELECT * FROM  EmployeeExtent AS o WHERE o.EMail LIKE $1";
            return QueryMembershipUsers(objScope, queryEmployeesByEMail, emailToMatch, pageIndex, pageSize, out totalRecords);
       }
        #endregion

        #region Utility Functions
        private Employee ResolveEmployeeByID(IObjectScope objScope, string id)
        {
            Employee e = null;
            const string queryEmployeeByID = "ELEMENT (SELECT o FROM EmployeeExtent AS o WHERE o.Employee_ID = $1)";
            IQuery oqlQuery = objScope.GetOqlQuery(queryEmployeeByID);
            using (IQueryResult result = oqlQuery.Execute(id))
            {
                if (result.Count > 0)
                    e = result[0] as Employee;
            }
            return e;
        }

        private Employee ResolveEmployeeByName(IObjectScope objScope, string name)
        {
            Employee e = null;
            const string queryEmployeeByName = "ELEMENT (SELECT o FROM EmployeeExtent AS o WHERE o.Name = $1)";
            IQuery oqlQuery = objScope.GetOqlQuery(queryEmployeeByName);
            using (IQueryResult result = oqlQuery.Execute(name))
            {
                if (result.Count > 0)
                    e = result[0] as Employee;
            }
            return e;
        }

        private MembershipUserCollection QueryMembershipUsers(IObjectScope objScope, string oqlQuery, string likeValue, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection();
            IQuery query = objScope.GetOqlQuery(oqlQuery);
            IQueryResult result;
            if (likeValue != null)
                result = query.Execute(likeValue + '*'); // OQL use * as wildcard instead of %
            else
                result = query.Execute();

            using (result)
            {
                totalRecords = result.Count;
                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                foreach (object res in result)
                {
                    if (counter >= startIndex)
                    {
                        Employee e = res as Employee;
                        users.Add(GetUserFromEmployee(e));
                    }
                    if (counter >= endIndex)
                        break;
                    counter += 1;
                }
            }
            return users;
        }

        /// <summary>
        /// Create a MembershipUser object from an employee
        /// </summary>
        /// <param name="emp">Employee.</param>
        /// <returns>MembershipUser object.</returns>
        private MembershipUser GetUserFromEmployee(Employee emp)
        {
            MembershipUser membershipUser = new MembershipUser(
              this.Name,
             emp.Name,
             emp.Employee_ID,
             emp.Email,
             emp.PasswordQuestion,
             emp.Description,
             emp.IsApproved,
             emp.IsLockedOut,
             emp.CreationDate,
             emp.LastLoginDate,
             emp.LastActivityDate,
             emp.LastPasswordChangedDate,
             emp.LastLockedOutDate);

            return membershipUser;
        }

        private MembershipUser GetMembershipUser(IObjectScope objScope, bool userIsOnline, Employee e)
        {
            MembershipUser membershipUser = GetUserFromEmployee(e);

            if (userIsOnline)
            {
                objScope.Transaction.Begin();
                e.LastActivityDate = DateTime.Now;
                objScope.Transaction.Commit();
            }
            return membershipUser;
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// Update password and answer failure information.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="failureType">Type of failure</param>
        /// <remarks></remarks>
        private void UpdateFailureCount(IObjectScope objScope, Employee e, FailureType failureType)
        {
            int failureCount = 0;
            objScope.Transaction.Begin();
            if (failureType == FailureType.Password)
            {
                failureCount = e.FailedPasswordAttemptCount;
                if (failureCount == 0
                    || DateTime.Now > e.FailedPasswordAttemptWindowStart.AddMinutes(passwordAttemptWindow))
                {
                    e.FailedPasswordAttemptCount = 1;
                    e.FailedPasswordAttemptWindowStart = DateTime.Now;
                }
                
            }
            else if(failureType == FailureType.PasswordAnswer)
            {
                failureCount = e.FailedPasswordAnswerAttemptCount;
                if (failureCount == 0
                    || DateTime.Now > e.FailedPasswordAnswerAttemptWindowStart.AddMinutes(passwordAttemptWindow))
                {
                    e.FailedPasswordAnswerAttemptCount = 1;
                    e.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
                }
            }
            failureCount++;
            if (failureCount >= maxInvalidPasswordAttempts)
            {
                e.IsLockedOut = true;
                e.LastLockedOutDate = DateTime.Now;
            }
            else
            {
                if (failureType == FailureType.Password)
                    e.FailedPasswordAttemptCount = failureCount;
                else if (failureType == FailureType.PasswordAnswer)
                    e.FailedPasswordAnswerAttemptCount = failureCount;
            }
            objScope.Transaction.Commit();
        }

        /// <summary>
        /// Check the password format based upon the MembershipPasswordFormat.
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="dbpassword"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Encode password.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <returns>Encoded password.</returns>
        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                      Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(machineKey.ValidationKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        /// <summary>
        /// UnEncode password.
        /// </summary>
        /// <param name="encodedPassword">Password.</param>
        /// <returns>Unencoded password.</returns>
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                      Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }
        #endregion
    }
}