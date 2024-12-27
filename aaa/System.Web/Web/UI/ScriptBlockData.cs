using System;

namespace System.Web.UI
{
	// Token: 0x02000476 RID: 1142
	internal class ScriptBlockData : SourceLineInfo
	{
		// Token: 0x060035BA RID: 13754 RVA: 0x000E7F58 File Offset: 0x000E6F58
		internal ScriptBlockData(int line, int column, string virtualPath)
		{
			base.Line = line;
			this.Column = column;
			base.VirtualPath = virtualPath;
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x060035BB RID: 13755 RVA: 0x000E7F75 File Offset: 0x000E6F75
		// (set) Token: 0x060035BC RID: 13756 RVA: 0x000E7F7D File Offset: 0x000E6F7D
		internal int Column
		{
			get
			{
				return this._column;
			}
			set
			{
				this._column = value;
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x060035BD RID: 13757 RVA: 0x000E7F86 File Offset: 0x000E6F86
		// (set) Token: 0x060035BE RID: 13758 RVA: 0x000E7F8E File Offset: 0x000E6F8E
		internal string Script
		{
			get
			{
				return this._script;
			}
			set
			{
				this._script = value;
			}
		}

		// Token: 0x04002552 RID: 9554
		protected string _script;

		// Token: 0x04002553 RID: 9555
		private int _column;
	}
}
