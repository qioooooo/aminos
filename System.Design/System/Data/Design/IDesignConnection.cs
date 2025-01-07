using System;
using System.CodeDom;
using System.Collections;

namespace System.Data.Design
{
	internal interface IDesignConnection : IDataSourceNamedObject, INamedObject, ICloneable, IDataSourceInitAfterLoading, IDataSourceXmlSpecialOwner
	{
		ConnectionString ConnectionStringObject { get; set; }

		string ConnectionString { get; set; }

		string Provider { get; set; }

		bool IsAppSettingsProperty { get; set; }

		string AppSettingsObjectName { get; set; }

		CodePropertyReferenceExpression PropertyReference { get; set; }

		IDictionary Properties { get; }

		IDbConnection CreateEmptyDbConnection();
	}
}
