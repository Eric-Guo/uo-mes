using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Profile;
using System;
using UO_Service.Base;
using UO_Model.Physical;
using Telerik.OpenAccess;
using System.Configuration.Provider;
using UO_Model.Process;
using UO_Service.Resx;

namespace UO_Service
{
    public class MESProfileProvider : System.Web.Profile.ProfileProvider
    {
        #region Class Variables
        private string applicationName;
        private bool trimDomainName;
        #endregion

        #region Properties
        /// <summary>
        /// Name of the application.
        /// </summary>
        /// <returns>String</returns>
        public override string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        public virtual bool TrimDomainName
        {
            get { return trimDomainName; }
            set { trimDomainName = value; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initialize the provider.
        /// </summary>
        /// <param name="name">Name of the provider.</param>
        /// <param name="config">Configuration settings.</param>
        /// <remarks></remarks>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if ((name == null) || (name.Length == 0))
                name = "MESProfileProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "MES Profile provider");
            }

            base.Initialize(name, config);

            if ((config["applicationName"] == null) || String.IsNullOrEmpty(config["applicationName"]))
                applicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            else
                applicationName = config["applicationName"];

            if ((config["trimDomainName"] == null) || String.IsNullOrEmpty(config["trimDomainName"]))
                trimDomainName = false;
            else
                trimDomainName = bool.Parse(config["trimDomainName"]);
        }
        #endregion

        #region Implemented Abstract Methods from ProfileProvider
        /// <summary>
        /// Get the property values for the user profile.
        /// </summary>
        /// <param name="context">Application context.</param>
        /// <param name="settingsProperties">Profile property settings.</param>
        /// <returns>Property setting values.</returns>
        public override System.Configuration.SettingsPropertyValueCollection GetPropertyValues(System.Configuration.SettingsContext context, System.Configuration.SettingsPropertyCollection settingsProperties)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            SettingsPropertyValueCollection settingsValues = new SettingsPropertyValueCollection();
            string userName = GetUserName((string)context["UserName"]);
            EmployeeProfile ep = ResolveEmployeeProfileByName(objScope, userName);
            if (null == ep) ep = new EmployeeProfile();
            foreach (SettingsProperty property in settingsProperties)
            {
                SettingsPropertyValue settingsPropertyValue = new SettingsPropertyValue(property);
                switch (property.Name)
                {
                    case "Language":
                        settingsPropertyValue.PropertyValue = ep.Language;
                        break;
                    case "StyleTheme":
                        settingsPropertyValue.PropertyValue = ep.StyleTheme;
                        break;
                    case "Factory_Name":
                        settingsPropertyValue.PropertyValue = ep.Factory_Name;
                        break;
                    case "Operation_Name":
                        settingsPropertyValue.PropertyValue = ep.Operation_Name;
                        break;
                    case "WorkCenter_Name":
                        settingsPropertyValue.PropertyValue = ep.WorkCenter_Name;
                        break;
                    default:
                        throw new ProviderException("Unsupported property.");
                }
                settingsValues.Add(settingsPropertyValue);
            }
            return settingsValues;
        }

        /// <summary>
        /// Set/store the property values for the user profile.
        /// </summary>
        /// <param name="context">Application context.</param>
        /// <param name="settingsPropertyValues">Profile property value settings.</param>
        public override void SetPropertyValues(System.Configuration.SettingsContext context, System.Configuration.SettingsPropertyValueCollection settingsPropertyValues)
        {
            if (true == (bool)context["IsAuthenticated"])
            {
                IObjectScope objScope = ORM.GetNewObjectScope();
                string userName = GetUserName((string)context["UserName"]);
                objScope.Transaction.Begin();
                EmployeeProfile ep = ResolveEmployeeProfileByName(objScope, userName);
                if (null == ep)
                {
                    ep = new EmployeeProfile();
                    ep.Employee = ResolveEmployeeByName(objScope, userName);
                    objScope.Add(ep);
                }
                foreach (SettingsPropertyValue settingsPropertyValue in settingsPropertyValues)
                {
                    switch (settingsPropertyValue.Property.Name)
                    {
                        case "Language":
                            ep.Language = settingsPropertyValue.PropertyValue as String;
                            ep.LastUpdatedDate = DateTime.Now;
                            break;
                        case "StyleTheme":
                            ep.StyleTheme = settingsPropertyValue.PropertyValue as String;
                            ep.LastUpdatedDate = DateTime.Now;
                            break;
                        case "Factory_Name":
                            ep.Factory_Name = settingsPropertyValue.PropertyValue as String;
                            ep.LastUpdatedDate = DateTime.Now;
                            break;
                        case "Operation_Name":
                            ep.Operation_Name = settingsPropertyValue.PropertyValue as String;
                            ep.LastUpdatedDate = DateTime.Now;
                            break;
                        case "WorkCenter_Name":
                            ep.WorkCenter_Name = settingsPropertyValue.PropertyValue as String;
                            ep.LastUpdatedDate = DateTime.Now;
                            break;
                        default:
                            throw new ProviderException("Unsupported property.");
                    }
                }
                objScope.Transaction.Commit();
            }
        }

        /// <summary>
        /// Deletes profiles that have been inactive since the specified date.
        /// </summary>
        /// <param name="authenticationOption">Current authentication option setting.</param>
        /// <param name="userInactiveSinceDate">Inactivity date for deletion.</param>
        /// <returns>Number of records deleted.</returns>
        public override int DeleteInactiveProfiles(System.Web.Profile.ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            objScope.Transaction.Begin();

            int deleteCounter = 0;
            const string queryInactiveProfiles =
                @"SELECT * FROM EmployeeProfileExtent AS o WHERE o.Employee.LastActivityDate <= $1";
            IQuery oqlQuery = objScope.GetOqlQuery(queryInactiveProfiles);
            using (IQueryResult result = oqlQuery.Execute(userInactiveSinceDate))
            {
                foreach (object p in result)
                {
                    objScope.Remove(p);
                    deleteCounter++;
                }
            }
            objScope.Transaction.Commit();
            return deleteCounter;
        }

        /// <summary>
        /// Delete profiles for an array of user names.
        /// </summary>
        /// <param name="userNames">Array of user names.</param>
        /// <returns>Number of profiles deleted.</returns>
        public override int DeleteProfiles(string[] userNames)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            int deleted = 0;
            objScope.Transaction.Begin();
            foreach (string userName in userNames)
            {
                string shotUserName = GetUserName(userName);
                EmployeeProfile ep = ResolveEmployeeProfileByName(objScope, shotUserName);
                if (ep != null)
                {
                    objScope.Remove(ep);
                    deleted++;
                }
            }
            objScope.Transaction.Commit();
            return deleted;
        }

        /// <summary>
        /// Delete profiles based upon the user names in the collection of profiles.
        /// </summary>
        /// <param name="profiles">Collection of profiles.</param>
        /// <returns>Number of profiles deleted.</returns>
        public override int DeleteProfiles(System.Web.Profile.ProfileInfoCollection profiles)
        {
            string[] userNames = new string[profiles.Count];
            int index = 0;
            foreach (ProfileInfo profileInfo in profiles)
            {
                userNames[index] = profileInfo.UserName;
                index += 1;
            }

            return DeleteProfiles(userNames);
        }

        /// <summary>
        /// Get a collection of profiles based upon a user name matching string and inactivity date.
        /// </summary>
        /// <param name="authenticationOption">Current authentication option setting.</param>
        /// <param name="userNameToMatch">Characters representing user name to match (L to R).</param>
        /// <param name="userInactiveSinceDate">Inactivity date for deletion.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Total records found (output).</param>
        /// <returns>Collection of profiles.</returns>
        public override System.Web.Profile.ProfileInfoCollection FindInactiveProfilesByUserName(System.Web.Profile.ProfileAuthenticationOption authenticationOption, string userNameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryInactiveProfilesByUserName = 
            @"SELECT * FROM EmployeeProfileExtent AS o WHERE o.Employee.Name LIKE $1 AND o.Employee.LastActivityDate <= $2";
            ProfileInfoCollection pic = new ProfileInfoCollection();
            IQuery query = objScope.GetOqlQuery(queryInactiveProfilesByUserName);
            using (IQueryResult result = query.Execute(userNameToMatch + '*', userInactiveSinceDate))
            {
                totalRecords = result.Count;
                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                foreach (object res in result)
                {
                    if (counter >= startIndex)
                    {
                        EmployeeProfile ep = res as EmployeeProfile;
                        pic.Add(new ProfileInfo(ep.Employee.Name, false, ep.Employee.LastActivityDate, ep.LastUpdatedDate, 0));
                    }
                    if (counter >= endIndex)
                        break;
                    counter += 1;
                }
            }
            return pic;
        }

        /// <summary>
        /// Get a collection of profiles based upon a user name matching string.
        /// </summary>
        /// <param name="authenticationOption">Current authentication option setting.</param>
        /// <param name="userNameToMatch">Characters representing user name to match (L to R).</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Total records found (output).</param>
        /// <returns>Collection of profiles.</returns>
        public override System.Web.Profile.ProfileInfoCollection FindProfilesByUserName(System.Web.Profile.ProfileAuthenticationOption authenticationOption, string userNameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryProfilesByUserName = @"SELECT * FROM EmployeeProfileExtent AS o WHERE o.Employee.Name LIKE $1";
            return QueryProfileInfos(objScope, queryProfilesByUserName, userNameToMatch + '*', pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Get a collection of profiles based upon an inactivity date.
        /// </summary>
        /// <param name="authenticationOption">Current authentication option setting.</param>
        /// <param name="userInactiveSinceDate">Inactivity date for deletion.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Total records found (output).</param>
        /// <returns>Collection of profiles.</returns>
        public override System.Web.Profile.ProfileInfoCollection GetAllInactiveProfiles(System.Web.Profile.ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryAllInactiveProfiles = @"SELECT * FROM EmployeeProfileExtent AS o WHERE o.Employee.LastActivityDate <= $1";
            return QueryProfileInfos(objScope, queryAllInactiveProfiles, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Get a collection of profiles.
        /// </summary>
        /// <param name="authenticationOption">Current authentication option setting.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Total records found (output).</param>
        /// <returns>Collection of profiles.</returns>
        public override System.Web.Profile.ProfileInfoCollection GetAllProfiles(System.Web.Profile.ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryAllProfiles = @"SELECT * FROM EmployeeProfileExtent AS o";
            return QueryProfileInfos(objScope, queryAllProfiles, null, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Get the number of inactive profiles based upon an inactivity date.
        /// </summary>
        /// <param name="authenticationOption">Current authentication option setting.</param>
        /// <param name="userInactiveSinceDate">Inactivity date for deletion.</param>
        /// <returns>Number of profiles.</returns>
        public override int GetNumberOfInactiveProfiles(System.Web.Profile.ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryOnlineNum = @"ELEMENT (SELECT COUNT(*) FROM EmployeeProfileExtent AS o WHERE o.Employee.LastActivityDate > $1)";
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);
            IQuery oqlQuery = objScope.GetOqlQuery(queryOnlineNum);
            using (IQueryResult result = oqlQuery.Execute(compareTime))
                return (int)result[0];
        }
        #endregion

        #region Utility Functions
        private string GetUserName(string fullUserName)
        {
            if (true == TrimDomainName && null != fullUserName )
                return fullUserName.Substring(fullUserName.IndexOf('\\') + 1);
            else
                return fullUserName;
        }

        private EmployeeProfile ResolveEmployeeProfileByName(IObjectScope objScope, string userName)
        {
            if (null == userName) return null;
            EmployeeProfile ep = null;
            const string queryEmployeeProfile =
@"ELEMENT (SELECT * FROM EmployeeProfileExtent AS o WHERE o.Employee.Name = $1)";
            IQuery oqlQuery = objScope.GetOqlQuery(queryEmployeeProfile);
            using (IQueryResult result = oqlQuery.Execute(userName))
                if (result.Count > 0)
                    ep = result[0] as EmployeeProfile;
            return ep;
        }

        private Employee ResolveEmployeeByName(IObjectScope objScope, string name)
        {
            Employee e = null;
            const string queryEmployeeByName = 
@"ELEMENT (SELECT o FROM EmployeeExtent AS o WHERE o.Name = $1 AND o.IsApproved = true AND o.isLockedOut = false)";
            IQuery oqlQuery = objScope.GetOqlQuery(queryEmployeeByName);
            using (IQueryResult result = oqlQuery.Execute(name))
            {
                if (result.Count > 0)
                    e = result[0] as Employee;
                else
                    throw new ProviderException(string.Format(MSG.ProfileProvider_Employee_Not_Exist,name));
            }
            return e;
        }

        private ProfileInfoCollection QueryProfileInfos(IObjectScope objScope, string oqlQuery, object oqlValue, int pageIndex, int pageSize, out int totalRecords)
        {
            ProfileInfoCollection pic = new ProfileInfoCollection();
            IQuery query = objScope.GetOqlQuery(oqlQuery);

            IQueryResult result;
            if (oqlValue != null)
                result = query.Execute(oqlValue);
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
                        EmployeeProfile ep = res as EmployeeProfile;
                        pic.Add(new ProfileInfo(ep.Employee.Name, false, ep.Employee.LastActivityDate, ep.LastUpdatedDate, 0));
                    }
                    if (counter >= endIndex)
                        break;
                    counter += 1;
                }
            }
            return pic;
        }
        #endregion
    }
}
