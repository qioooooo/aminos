using System;

namespace System.Data.Design
{
	internal class SchemaName
	{
		private SchemaName()
		{
		}

		internal const string DataSourcePrefix = "msdatasource";

		internal const string DataSourceNamespace = "urn:schemas-microsoft-com:xml-msdatasource";

		internal const string DataSourceTempTargetNamespace = "";

		internal const string SchemaNodeName = "schema";

		internal const string AnnotationNodeName = "annotation";

		internal const string AppInfoNodeName = "appinfo";

		internal const string AppInfoSourceName = "source";

		internal const string DataSourceRoot = "DataSource";

		internal const string DbSource = "DbSource";

		internal const string Connection = "Connection";

		internal const string RadTable = "TableAdapter";

		internal const string OldRadTable = "DbTable";

		internal const string DbCommand = "DbCommand";

		internal const string Parameter = "Parameter";
	}
}
