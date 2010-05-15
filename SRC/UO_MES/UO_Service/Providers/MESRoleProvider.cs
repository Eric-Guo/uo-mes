using System;
using System.Web.Security;
using System.Configuration.Provider;
using UO_Model.Physical;
using Telerik.OpenAccess;
using UO_Service.Base;

namespace UO_Service
{
    public class MESRoleProvider : RoleProvider
    {
        #region Class Variables
        private string applicationName;
        #endregion

        #region Properties
        /// <summary>
        /// Name of the application.
        /// </summary>
        /// <returns>String</returns>
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
        #endregion

        #region Initialization
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
                name = "MESRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "MES Role provider");
            }

            base.Initialize(name, config);

            if ((config["applicationName"] == null) || String.IsNullOrEmpty(config["applicationName"]))
                applicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            else
                applicationName = config["applicationName"];
        }
        #endregion

        #region Implemented Abstract Methods from RoleProvider
        /// <summary>
        /// Gets a list of the roles that a specified user is in for the
        /// configured <see cref="ApplicationName"/>.
        /// </summary>
        /// <remarks>
        /// This method is called by the ASP.NET runtime when using 
        /// UrlAuthorizationModule (the <c>authorization</c> element of web.config)
        /// to secure your website, or when calling IsInRole() on the current
        /// IPrincipal (ex: Page.User.IsInRole("Admin"))
        /// </remarks>
        /// <param name="username">The user to return a list of roles for.</param>
        /// <returns>A string array containing the names of all the roles that the
        /// specified user is in for the configured <see cref="ApplicationName"/>
        /// </returns>
        public override string[] GetRolesForUser(string username)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryRolesForUser =
                @"SELECT o.Name FROM RoleExtent AS o, o.Employees AS e WHERE e.Name = $1";
            using (IQueryResult result = objScope.GetOqlQuery(queryRolesForUser).Execute(username))
            {
                string[] roles = new string[result.Count];
                result.CopyTo(roles, 0);
                return roles;
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryUserInRoleExist =
                @"SELECT o.Name FROM RoleExtent AS o WHERE EXISTS e IN o.Employees : (e.Name = $1) AND o.Name = $2";
            using (IQueryResult result = objScope.GetOqlQuery(queryUserInRoleExist).Execute(username, roleName))
            {
                if (result.Count > 0)
                    return true;
                return false;
            }
        }

        public override bool RoleExists(string roleName)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            return (null != ResolveRoleByName(objScope, roleName));
        }

        public override string[] GetUsersInRole(string roleName)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            Role role = ResolveRoleByName(objScope, roleName);
            if (null == role)
                throw new ProviderException(String.Format("Role: {0} is not exist", roleName));

            const string queryUsersInRole =
                @"SELECT o.Name FROM EmployeeExtent AS o WHERE EXISTS r IN o.Roles : (r = $1)";
            using (IQueryResult result = objScope.GetOqlQuery(queryUsersInRole).Execute(role))
            {
                string[] users = new string[result.Count];
                result.CopyTo(users, 0);
                return users;
            }
        }

        public override string[] GetAllRoles()
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            const string queryAllRoles = @"SELECT o.Name FROM RoleExtent AS o";
            using (IQueryResult result = objScope.GetOqlQuery(queryAllRoles).Execute())
            {
                string[] roles = new string[result.Count];
                result.CopyTo(roles, 0);
                return roles;
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            Role role = ResolveRoleByName(objScope, roleName);
            if (null == role)
                throw new ProviderException(String.Format("Role: {0} is not exist", roleName));

            const string queryFindUsersInRole =
                @"SELECT o.Name FROM EmployeeExtent AS o WHERE EXISTS r IN o.Roles : (r = $1) AND o.Name like $2";
            using (IQueryResult result = objScope.GetOqlQuery(queryFindUsersInRole).Execute(role, usernameToMatch + '*'))
            {
                string[] users = new string[result.Count];
                result.CopyTo(users, 0);
                return users;
            }
        }

        public override void CreateRole(string roleName)
        {
            if (roleName.IndexOf(',') >=0 )
                throw new ProviderException(String.Format("Role: {0} had comma inside", roleName));

            IObjectScope objScope = ORM.GetNewObjectScope();
            Role role = ResolveRoleByName(objScope, roleName);
            if (null != role)
                throw new ProviderException(String.Format("Role: {0} is already exist", roleName));

            role = new Role();
            role.Name = roleName;
            objScope.Transaction.Begin();
            objScope.Add(role);
            objScope.Transaction.Commit();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            Role role = ResolveRoleByName(objScope, roleName);
            if (null == role)
                throw new ProviderException(String.Format("Role: {0} is not exist", roleName));

            if (role.Employees.Count > 0 && true == throwOnPopulatedRole)
                throw new ProviderException(String.Format("Role: {0} still has employee assigned", roleName));
            else
            {
                objScope.Transaction.Begin();
                objScope.Remove(role);
                objScope.Transaction.Commit();
                return true;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            foreach(string u in usernames)
            {
                Employee emp = ResolveEmployeeByName(objScope, u);
                if (null == emp)
                    throw new ArgumentNullException(String.Format("Employee: {0} is not exist", u));
                foreach (string r in roleNames)
                {
                    Role role = ResolveRoleByName(objScope, r);
                    if (null == role)
                        throw new ArgumentNullException(String.Format("Role: {0} is not exist", r));
                    objScope.Transaction.Begin();
                    role.Employees.Add(emp);
                    objScope.Transaction.Commit();
                }
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            IObjectScope objScope = ORM.GetNewObjectScope();
            foreach (string u in usernames)
            {
                Employee emp = ResolveEmployeeByName(objScope, u);
                if (null == emp)
                    throw new ArgumentNullException(String.Format("Employee: {0} is not exist", u));
                foreach (string r in roleNames)
                {
                    Role role = ResolveRoleByName(objScope, r);
                    if (null == role)
                        throw new ArgumentNullException(String.Format("Role: {0} is not exist", r));
                    objScope.Transaction.Begin();
                    role.Employees.Remove(emp);
                    objScope.Transaction.Commit();
                }
            }
        }
        #endregion

        #region Utility Functions
        private Role ResolveRoleByName(IObjectScope objScope, string roleName)
        {
            if (null == roleName) return null;
            Role r = null;
            const string queryRole = @"ELEMENT (SELECT * FROM RoleExtent AS o WHERE o.Name = $1)";
            IQuery oqlQuery = objScope.GetOqlQuery(queryRole);
            using (IQueryResult result = oqlQuery.Execute(roleName))
            {
                if (result.Count > 0)
                    r = result[0] as Role;
            }
            return r;
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
        #endregion
    }
}