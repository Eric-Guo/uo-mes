using System;
using System.Collections.Generic;
using System.Text;

namespace UO_Service.Base
{
    /// <summary>
    /// Group the sub entities that will be used for shopfloor services.
    /// </summary>
    public class ServiceDetail
    {
        private int serviceDetailID;
        public int ServiceDetailID
        {
            get { return serviceDetailID; }
            set { serviceDetailID = value; }
        }
    }
}
