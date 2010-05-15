using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Telerik.OpenAccess;
using UO_Model.Base;

namespace UO_Model.Execution
{
    [DataContract]
    [Persistent(IdentityField = "siteMap_id")]
    public class SiteMap : NamedObject
    {
        private int siteMap_id;
        [FieldAlias("siteMap_id")]
        public int SiteMap_ID
        {
            get { return siteMap_id; }
            set { siteMap_id = value; }
        }

        // Title is just the Name
        [DataMember]
        public string Title
        {
            get { return Name; }
            set { Name = value; }
        }

        private string url;
        [DataMember]
        [FieldAlias("url")]
        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        private SiteMap parentSiteMap;
        [DataMember]
        [FieldAlias("parentSiteMap")]
        public SiteMap ParentSiteMap
        {
            get { return parentSiteMap; }
            set { parentSiteMap = value; }
        }

        private IList<SiteMap> childSiteMaps = new List<SiteMap>();  // inverse SiteMap.parentSiteMap
        [DataMember]
        [FieldAlias("childSiteMaps")]
        public IList<SiteMap> ChildSiteMaps
        {
            get { return childSiteMaps; }
        }
    }
}
