using System;

namespace System.Web.UI
{
	// Token: 0x02000475 RID: 1141
	internal class NamespaceEntry : SourceLineInfo
	{
		// Token: 0x060035B7 RID: 13751 RVA: 0x000E7F3F File Offset: 0x000E6F3F
		internal NamespaceEntry()
		{
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x060035B8 RID: 13752 RVA: 0x000E7F47 File Offset: 0x000E6F47
		// (set) Token: 0x060035B9 RID: 13753 RVA: 0x000E7F4F File Offset: 0x000E6F4F
		internal string Namespace
		{
			get
			{
				return this._namespace;
			}
			set
			{
				this._namespace = value;
			}
		}

		// Token: 0x04002551 RID: 9553
		private string _namespace;
	}
}
