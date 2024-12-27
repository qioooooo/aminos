using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200047D RID: 1149
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TraceContextRecord
	{
		// Token: 0x060035FA RID: 13818 RVA: 0x000E98A1 File Offset: 0x000E88A1
		public TraceContextRecord(string category, string msg, bool isWarning, Exception errorInfo)
		{
			this._category = category;
			this._message = msg;
			this._isWarning = isWarning;
			this._errorInfo = errorInfo;
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x060035FB RID: 13819 RVA: 0x000E98C6 File Offset: 0x000E88C6
		public string Category
		{
			get
			{
				return this._category;
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x060035FC RID: 13820 RVA: 0x000E98CE File Offset: 0x000E88CE
		public Exception ErrorInfo
		{
			get
			{
				return this._errorInfo;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x060035FD RID: 13821 RVA: 0x000E98D6 File Offset: 0x000E88D6
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x060035FE RID: 13822 RVA: 0x000E98DE File Offset: 0x000E88DE
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
		}

		// Token: 0x0400256E RID: 9582
		private string _category;

		// Token: 0x0400256F RID: 9583
		private string _message;

		// Token: 0x04002570 RID: 9584
		private Exception _errorInfo;

		// Token: 0x04002571 RID: 9585
		private bool _isWarning;
	}
}
