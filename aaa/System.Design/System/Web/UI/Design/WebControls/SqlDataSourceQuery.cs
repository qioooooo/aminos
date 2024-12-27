using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004D6 RID: 1238
	internal sealed class SqlDataSourceQuery
	{
		// Token: 0x06002C84 RID: 11396 RVA: 0x000FAB56 File Offset: 0x000F9B56
		public SqlDataSourceQuery(string command, SqlDataSourceCommandType commandType, ICollection parameters)
		{
			this._command = command;
			this._commandType = commandType;
			this._parameters = parameters;
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x000FAB73 File Offset: 0x000F9B73
		public string Command
		{
			get
			{
				return this._command;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002C86 RID: 11398 RVA: 0x000FAB7B File Offset: 0x000F9B7B
		public SqlDataSourceCommandType CommandType
		{
			get
			{
				return this._commandType;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002C87 RID: 11399 RVA: 0x000FAB83 File Offset: 0x000F9B83
		public ICollection Parameters
		{
			get
			{
				return this._parameters;
			}
		}

		// Token: 0x04001E5E RID: 7774
		private string _command;

		// Token: 0x04001E5F RID: 7775
		private SqlDataSourceCommandType _commandType;

		// Token: 0x04001E60 RID: 7776
		private ICollection _parameters;
	}
}
