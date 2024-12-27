using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EF RID: 1263
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComCompatibleVersionAttribute : Attribute
	{
		// Token: 0x0600314F RID: 12623 RVA: 0x000A94E5 File Offset: 0x000A84E5
		public ComCompatibleVersionAttribute(int major, int minor, int build, int revision)
		{
			this._major = major;
			this._minor = minor;
			this._build = build;
			this._revision = revision;
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003150 RID: 12624 RVA: 0x000A950A File Offset: 0x000A850A
		public int MajorVersion
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06003151 RID: 12625 RVA: 0x000A9512 File Offset: 0x000A8512
		public int MinorVersion
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003152 RID: 12626 RVA: 0x000A951A File Offset: 0x000A851A
		public int BuildNumber
		{
			get
			{
				return this._build;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003153 RID: 12627 RVA: 0x000A9522 File Offset: 0x000A8522
		public int RevisionNumber
		{
			get
			{
				return this._revision;
			}
		}

		// Token: 0x04001959 RID: 6489
		internal int _major;

		// Token: 0x0400195A RID: 6490
		internal int _minor;

		// Token: 0x0400195B RID: 6491
		internal int _build;

		// Token: 0x0400195C RID: 6492
		internal int _revision;
	}
}
