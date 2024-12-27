using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002CF RID: 719
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class SqlExecutionException : SystemException
	{
		// Token: 0x060024BF RID: 9407 RVA: 0x0009D376 File Offset: 0x0009C376
		public SqlExecutionException(string message, string server, string database, string sqlFile, string commands, SqlException sqlException)
			: base(message)
		{
			this._server = server;
			this._database = database;
			this._sqlFile = sqlFile;
			this._commands = commands;
			this._sqlException = sqlException;
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x0009D3A5 File Offset: 0x0009C3A5
		public SqlExecutionException(string message)
			: base(message)
		{
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0009D3AE File Offset: 0x0009C3AE
		public SqlExecutionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x0009D3B8 File Offset: 0x0009C3B8
		public SqlExecutionException()
		{
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x0009D3C0 File Offset: 0x0009C3C0
		private SqlExecutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._server = info.GetString("_server");
			this._database = info.GetString("_database");
			this._sqlFile = info.GetString("_sqlFile");
			this._commands = info.GetString("_commands");
			this._sqlException = (SqlException)info.GetValue("_sqlException", typeof(SqlException));
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x0009D43C File Offset: 0x0009C43C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_server", this._server);
			info.AddValue("_database", this._database);
			info.AddValue("_sqlFile", this._sqlFile);
			info.AddValue("_commands", this._commands);
			info.AddValue("_sqlException", this._sqlException);
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060024C5 RID: 9413 RVA: 0x0009D4A6 File Offset: 0x0009C4A6
		public string Server
		{
			get
			{
				return this._server;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x0009D4AE File Offset: 0x0009C4AE
		public string Database
		{
			get
			{
				return this._database;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x0009D4B6 File Offset: 0x0009C4B6
		public string SqlFile
		{
			get
			{
				return this._sqlFile;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060024C8 RID: 9416 RVA: 0x0009D4BE File Offset: 0x0009C4BE
		public string Commands
		{
			get
			{
				return this._commands;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060024C9 RID: 9417 RVA: 0x0009D4C6 File Offset: 0x0009C4C6
		public SqlException Exception
		{
			get
			{
				return this._sqlException;
			}
		}

		// Token: 0x04001C8B RID: 7307
		private string _server;

		// Token: 0x04001C8C RID: 7308
		private string _database;

		// Token: 0x04001C8D RID: 7309
		private string _sqlFile;

		// Token: 0x04001C8E RID: 7310
		private string _commands;

		// Token: 0x04001C8F RID: 7311
		private SqlException _sqlException;
	}
}
