using System;
using UO_Model.Physical;

namespace UO_Service.Inquery
{
    public class QueryResource : Inquery
    {
        public string[] GetAllResourceNames(string parcialResourceName)
        {
            const string queryResources =
@"SELECT o.Name FROM ResourceExtent AS o WHERE o.Name LIKE $1 ORDER BY o.Name";
            return getObjectNamesViaQuery(parcialResourceName, queryResources);
        }

        public Resource GetResource(string resourceName)
        {
            return ResolveCDO("Resource", resourceName) as Resource;
        }
    }
}
