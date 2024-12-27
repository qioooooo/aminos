using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x0200036E RID: 878
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ISessionStateItemCollection : ICollection, IEnumerable
	{
		// Token: 0x17000923 RID: 2339
		object this[string name] { get; set; }

		// Token: 0x17000924 RID: 2340
		object this[int index] { get; set; }

		// Token: 0x06002AA6 RID: 10918
		void Remove(string name);

		// Token: 0x06002AA7 RID: 10919
		void RemoveAt(int index);

		// Token: 0x06002AA8 RID: 10920
		void Clear();

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06002AA9 RID: 10921
		NameObjectCollectionBase.KeysCollection Keys { get; }

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06002AAA RID: 10922
		// (set) Token: 0x06002AAB RID: 10923
		bool Dirty { get; set; }
	}
}
