using System;
using UO_Model.Execution;

namespace UO_Service.Inquery
{
    public class QueryContainer : Inquery
    {
        public string[] GetAllContainerNames(string parcialContainerName)
        {
            const string queryContainers = 
@"SELECT o.ContainerName FROM ContainerExtent AS o WHERE o.ContainerName LIKE $1 ORDER BY o.ContainerName";
            return getObjectNamesViaQuery(parcialContainerName, queryContainers);
        }

        public string[] GetActiveContainerNames(string parcialContainerName)
        {
            const string queryContainers =
@"SELECT o.ContainerName FROM ContainerExtent AS o WHERE o.ContainerName LIKE $1 AND o.CurrentHoldCount = 0 ORDER BY o.ContainerName";
            return getObjectNamesViaQuery(parcialContainerName, queryContainers);
        }

        public string[] GetHoldContainerNames(string parcialContainerName)
        {
            const string queryContainers =
@"SELECT o.ContainerName FROM ContainerExtent AS o WHERE o.ContainerName LIKE $1 AND o.CurrentHoldCount > 0 ORDER BY o.ContainerName";
            return getObjectNamesViaQuery(parcialContainerName, queryContainers);
        }

        public Container GetContainer(string containerName)
        {
            return ResolveContainer(containerName);
        }
    }
}
