using System;

namespace System.Web.UI
{
	// Token: 0x02000388 RID: 904
	internal abstract class SourceLineInfo
	{
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x000C5C80 File Offset: 0x000C4C80
		// (set) Token: 0x06002C3B RID: 11323 RVA: 0x000C5C88 File Offset: 0x000C4C88
		internal string VirtualPath
		{
			get
			{
				return this._virtualPath;
			}
			set
			{
				this._virtualPath = value;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06002C3C RID: 11324 RVA: 0x000C5C91 File Offset: 0x000C4C91
		// (set) Token: 0x06002C3D RID: 11325 RVA: 0x000C5C99 File Offset: 0x000C4C99
		internal int Line
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

		// Token: 0x04002083 RID: 8323
		private string _virtualPath;

		// Token: 0x04002084 RID: 8324
		private int _line;
	}
}
