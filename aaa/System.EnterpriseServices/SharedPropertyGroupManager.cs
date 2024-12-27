using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000020 RID: 32
	[ComVisible(false)]
	public sealed class SharedPropertyGroupManager : IEnumerable
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00002157 File Offset: 0x00001157
		public SharedPropertyGroupManager()
		{
			Platform.Assert(Platform.MTS, "SharedPropertyGroupManager");
			this._ex = (ISharedPropertyGroupManager)new xSharedPropertyGroupManager();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000217E File Offset: 0x0000117E
		public SharedPropertyGroup CreatePropertyGroup(string name, ref PropertyLockMode dwIsoMode, ref PropertyReleaseMode dwRelMode, out bool fExist)
		{
			return new SharedPropertyGroup(this._ex.CreatePropertyGroup(name, ref dwIsoMode, ref dwRelMode, out fExist));
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002195 File Offset: 0x00001195
		public SharedPropertyGroup Group(string name)
		{
			return new SharedPropertyGroup(this._ex.Group(name));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000021A8 File Offset: 0x000011A8
		public IEnumerator GetEnumerator()
		{
			IEnumerator enumerator = null;
			this._ex.GetEnumerator(out enumerator);
			return enumerator;
		}

		// Token: 0x0400001E RID: 30
		private ISharedPropertyGroupManager _ex;
	}
}
