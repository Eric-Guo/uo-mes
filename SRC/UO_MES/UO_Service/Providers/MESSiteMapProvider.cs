using System;
using System.Collections.Generic;
using System.Web;
using Telerik.OpenAccess;
using UO_Service.Base;
using UO_Service.Resx;

namespace UO_Service
{
    public class MESSiteMapProvider : StaticSiteMapProvider
    {
        #region Class Variables
        private SiteMapNode rootNode = null;
        #endregion

        #region Properties
        #endregion

        #region Initialization
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if ((name == null) || (name.Length == 0))
                name = "MESSiteMapProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "MES Site Map provider");
            }

            base.Initialize(name, config);
        }
        #endregion

        #region Implemented Abstract Methods from StaticSiteMapProvider
        public override SiteMapNode BuildSiteMap()
        {
            const string queryRootNode = "ELEMENT (SELECT o FROM SiteMapExtent AS o WHERE o.ParentSiteMap = nil )";
            if (rootNode == null)
            {
                lock (this)
                {
                    UO_Model.Execution.SiteMap rt;
                    using (IQueryResult result = ORM.ObjectScope().GetOqlQuery(queryRootNode).Execute())
                    {
                        if (result.Count > 0)
                            rt = result[0] as UO_Model.Execution.SiteMap;
                        else
                            throw new ApplicationException(MSG.SiteMap_Root_Not_Fould);
                    }
                    rootNode = new SiteMapNode(this, rt.SiteMap_ID.ToString(), rt.URL, rt.Title, rt.Description);
                    AddNode(rootNode);
                    AddChildSiteMaps(rt, rootNode);
                }
            }
            return rootNode;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            if (rootNode == null)
                BuildSiteMap();
            return rootNode;
        }
        #endregion

        #region Utility Functions
        private void AddChildSiteMaps(UO_Model.Execution.SiteMap parent, SiteMapNode parentNode)
        {
            foreach (UO_Model.Execution.SiteMap m in parent.ChildSiteMaps)
            {
                SiteMapNode n = new SiteMapNode(this, m.SiteMap_ID.ToString(), m.URL, m.Title, m.Description);

                AddNode(n, parentNode);
                AddChildSiteMaps(m, n);
            }
        }
        #endregion
    }
}
