using System;
using System.Diagnostics;

namespace System.Configuration
{
	// Token: 0x02000068 RID: 104
	[DebuggerDisplay("FactoryId {ConfigKey}")]
	internal class FactoryId
	{
		// Token: 0x060003E3 RID: 995 RVA: 0x000137B7 File Offset: 0x000127B7
		internal FactoryId(string configKey, string group, string name)
		{
			this._configKey = configKey;
			this._group = group;
			this._name = name;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x000137D4 File Offset: 0x000127D4
		internal string ConfigKey
		{
			get
			{
				return this._configKey;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x000137DC File Offset: 0x000127DC
		internal string Group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x000137E4 File Offset: 0x000127E4
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x040002FE RID: 766
		private string _configKey;

		// Token: 0x040002FF RID: 767
		private string _group;

		// Token: 0x04000300 RID: 768
		private string _name;
	}
}
