using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	// Token: 0x02000747 RID: 1863
	[ComVisible(true)]
	public class TypeEntry
	{
		// Token: 0x060042CA RID: 17098 RVA: 0x000E52A2 File Offset: 0x000E42A2
		protected TypeEntry()
		{
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x060042CB RID: 17099 RVA: 0x000E52AA File Offset: 0x000E42AA
		// (set) Token: 0x060042CC RID: 17100 RVA: 0x000E52B2 File Offset: 0x000E42B2
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
			set
			{
				this._typeName = value;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x060042CD RID: 17101 RVA: 0x000E52BB File Offset: 0x000E42BB
		// (set) Token: 0x060042CE RID: 17102 RVA: 0x000E52C3 File Offset: 0x000E42C3
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			set
			{
				this._assemblyName = value;
			}
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x000E52CC File Offset: 0x000E42CC
		internal void CacheRemoteAppEntry(RemoteAppEntry entry)
		{
			this._cachedRemoteAppEntry = entry;
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x000E52D5 File Offset: 0x000E42D5
		internal RemoteAppEntry GetRemoteAppEntry()
		{
			return this._cachedRemoteAppEntry;
		}

		// Token: 0x04002173 RID: 8563
		private string _typeName;

		// Token: 0x04002174 RID: 8564
		private string _assemblyName;

		// Token: 0x04002175 RID: 8565
		private RemoteAppEntry _cachedRemoteAppEntry;
	}
}
