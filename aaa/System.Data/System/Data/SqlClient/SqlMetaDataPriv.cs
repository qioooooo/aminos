using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x02000328 RID: 808
	internal class SqlMetaDataPriv
	{
		// Token: 0x06002A6F RID: 10863 RVA: 0x0029CCE8 File Offset: 0x0029C0E8
		internal SqlMetaDataPriv()
		{
		}

		// Token: 0x04001BAE RID: 7086
		internal SqlDbType type;

		// Token: 0x04001BAF RID: 7087
		internal byte tdsType;

		// Token: 0x04001BB0 RID: 7088
		internal byte precision = byte.MaxValue;

		// Token: 0x04001BB1 RID: 7089
		internal byte scale = byte.MaxValue;

		// Token: 0x04001BB2 RID: 7090
		internal int length;

		// Token: 0x04001BB3 RID: 7091
		internal SqlCollation collation;

		// Token: 0x04001BB4 RID: 7092
		internal int codePage;

		// Token: 0x04001BB5 RID: 7093
		internal Encoding encoding;

		// Token: 0x04001BB6 RID: 7094
		internal bool isNullable;

		// Token: 0x04001BB7 RID: 7095
		internal bool isMultiValued;

		// Token: 0x04001BB8 RID: 7096
		internal string udtDatabaseName;

		// Token: 0x04001BB9 RID: 7097
		internal string udtSchemaName;

		// Token: 0x04001BBA RID: 7098
		internal string udtTypeName;

		// Token: 0x04001BBB RID: 7099
		internal string udtAssemblyQualifiedName;

		// Token: 0x04001BBC RID: 7100
		internal Type udtType;

		// Token: 0x04001BBD RID: 7101
		internal string xmlSchemaCollectionDatabase;

		// Token: 0x04001BBE RID: 7102
		internal string xmlSchemaCollectionOwningSchema;

		// Token: 0x04001BBF RID: 7103
		internal string xmlSchemaCollectionName;

		// Token: 0x04001BC0 RID: 7104
		internal MetaType metaType;

		// Token: 0x04001BC1 RID: 7105
		internal string structuredTypeDatabaseName;

		// Token: 0x04001BC2 RID: 7106
		internal string structuredTypeSchemaName;

		// Token: 0x04001BC3 RID: 7107
		internal string structuredTypeName;

		// Token: 0x04001BC4 RID: 7108
		internal IList<SmiMetaData> structuredFields;
	}
}
