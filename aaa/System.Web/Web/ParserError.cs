using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000070 RID: 112
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class ParserError
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x00014257 File Offset: 0x00013257
		public ParserError()
		{
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001425F File Offset: 0x0001325F
		public ParserError(string errorText, string virtualPath, int line)
			: this(errorText, global::System.Web.VirtualPath.CreateAllowNull(virtualPath), line)
		{
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001426F File Offset: 0x0001326F
		internal ParserError(string errorText, VirtualPath virtualPath, int line)
		{
			this._virtualPath = virtualPath;
			this._line = line;
			this._errorText = errorText;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0001428C File Offset: 0x0001328C
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x00014294 File Offset: 0x00013294
		internal Exception Exception
		{
			get
			{
				return this._exception;
			}
			set
			{
				this._exception = value;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001429D File Offset: 0x0001329D
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x000142AA File Offset: 0x000132AA
		public string VirtualPath
		{
			get
			{
				return global::System.Web.VirtualPath.GetVirtualPathString(this._virtualPath);
			}
			set
			{
				this._virtualPath = global::System.Web.VirtualPath.Create(value);
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x000142B8 File Offset: 0x000132B8
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x000142C0 File Offset: 0x000132C0
		public string ErrorText
		{
			get
			{
				return this._errorText;
			}
			set
			{
				this._errorText = value;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x000142C9 File Offset: 0x000132C9
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x000142D1 File Offset: 0x000132D1
		public int Line
		{
			get
			{
				return this._line;
			}
			set
			{
				this._line = value;
			}
		}

		// Token: 0x04001041 RID: 4161
		private int _line;

		// Token: 0x04001042 RID: 4162
		private VirtualPath _virtualPath;

		// Token: 0x04001043 RID: 4163
		private string _errorText;

		// Token: 0x04001044 RID: 4164
		private Exception _exception;
	}
}
