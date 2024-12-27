using System;
using System.Security.Permissions;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000020 RID: 32
	[Serializable]
	public class Group : Capture
	{
		// Token: 0x0600014F RID: 335 RVA: 0x0000B243 File Offset: 0x0000A243
		internal Group(string text, int[] caps, int capcount)
			: base(text, (capcount == 0) ? 0 : caps[(capcount - 1) * 2], (capcount == 0) ? 0 : caps[capcount * 2 - 1])
		{
			this._caps = caps;
			this._capcount = capcount;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000150 RID: 336 RVA: 0x0000B274 File Offset: 0x0000A274
		public bool Success
		{
			get
			{
				return this._capcount != 0;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000B282 File Offset: 0x0000A282
		public CaptureCollection Captures
		{
			get
			{
				if (this._capcoll == null)
				{
					this._capcoll = new CaptureCollection(this);
				}
				return this._capcoll;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000B2A0 File Offset: 0x0000A2A0
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Group Synchronized(Group inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			CaptureCollection captures = inner.Captures;
			if (inner._capcount > 0)
			{
				Capture capture = captures[0];
			}
			return inner;
		}

		// Token: 0x04000729 RID: 1833
		internal static Group _emptygroup = new Group(string.Empty, new int[0], 0);

		// Token: 0x0400072A RID: 1834
		internal int[] _caps;

		// Token: 0x0400072B RID: 1835
		internal int _capcount;

		// Token: 0x0400072C RID: 1836
		internal CaptureCollection _capcoll;
	}
}
