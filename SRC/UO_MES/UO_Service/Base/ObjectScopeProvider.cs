// usage example:
//
// // Get ObjectScope from ObjectScopeProvider
// IObjectScope scope = ObjectScopeProvider1.ObjectScope();
// // start transaction
// scope.Transaction.Begin();
// // create new persistent object person and add to scope
// Person p = new Person();
// scope.Add(p);
// // commit transction
// scope.Transaction.Commit();
//

using Telerik.OpenAccess;
using Telerik.OpenAccess.Util;

namespace UO_Service.Base
{
	/// <summary>
	/// This class provides an object context for connected database access.
	/// </summary>
	/// <remarks>
	/// This class can be used to obtain an IObjectScope instance required for a connected database
	/// access.
	/// </remarks>
	internal class ORM : IObjectScopeProvider
	{
		private Database mesDatabase;
		private IObjectScope mesScope;

		static private ORM theObjectScopeProvider;

		private ORM()
		{
		}

        /// <summary>
		/// Adjusts for dynamic loading when no entry assembly is available/configurable.
		/// </summary>
		/// <remarks>
        /// When dynamic loading is used, the configuration path from the
        /// applications entry assembly to the connection setting might be broken.
        /// This method makes up the necessary configuration entries.
        /// </remarks>
        static public void AdjustForDynamicLoad()
        {
            if( theObjectScopeProvider == null )
                theObjectScopeProvider = new ORM();

            if( theObjectScopeProvider.mesDatabase == null )
            {
                string assumedInitialConfiguration =
                           "<openaccess>" +
                               "<references>" +
                                   "<reference assemblyname='PLACEHOLDER' configrequired='True'/>" +
                               "</references>" +
                           "</openaccess>";
                System.Reflection.Assembly dll = theObjectScopeProvider.GetType().Assembly;
                assumedInitialConfiguration = assumedInitialConfiguration.Replace(
                                                    "PLACEHOLDER", dll.GetName().Name);
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(assumedInitialConfiguration);
                Database db = Telerik.OpenAccess.Database.Get("MESDatabaseConnection", 
                                            xmlDoc.DocumentElement,
                                            new System.Reflection.Assembly[] { dll } );

                theObjectScopeProvider.mesDatabase = db;
            }
        }

		/// <summary>
		/// Returns the instance of Database for the connectionId 
		/// specified in the Enable Project Wizard.
		/// </summary>
		/// <returns>Instance of Database.</returns>
		/// <remarks></remarks>
		static public Database Database()
		{
			if( theObjectScopeProvider == null )
				theObjectScopeProvider = new ORM();

			if( theObjectScopeProvider.mesDatabase == null )
				theObjectScopeProvider.mesDatabase = Telerik.OpenAccess.Database.Get( "MESDatabaseConnection" );

			return theObjectScopeProvider.mesDatabase;
		}

		/// <summary>
		/// Returns the instance of ObjectScope for the application.
		/// </summary>
		/// <returns>Instance of IObjectScope.</returns>
		/// <remarks></remarks>
		static public IObjectScope ObjectScope()
		{
			Database();

			if( theObjectScopeProvider.mesScope == null )
				theObjectScopeProvider.mesScope = GetNewObjectScope();

			return theObjectScopeProvider.mesScope;
		}

		/// <summary>
		/// Returns the new instance of ObjectScope for the application.
		/// </summary>
		/// <returns>Instance of IObjectScope.</returns>
		/// <remarks></remarks>
		static public IObjectScope GetNewObjectScope()
		{
			Database db = Database();

			IObjectScope newScope = db.GetObjectScope();
			return newScope;
		}
	}
}
