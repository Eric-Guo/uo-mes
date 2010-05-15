using System;
using System.Collections.Generic;
using System.Text;

namespace UO_Service.Inquery
{
    public class QuerySpec : Inquery
    {
        public string[] GetAllSpecRevisions(string parcialSpecName)
        {
            const string querySpecs =
@"SELECT o.RevBase.Name,o.Revision FROM SpecExtent AS o WHERE o.RevBase.Name LIKE $1 ORDER BY o.RevBase.Name, o.Revision";
            return getObjectRevisionsViaQuery(parcialSpecName, querySpecs);
        }
    }
}
