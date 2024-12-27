using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EE RID: 1262
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class TypeLibVersionAttribute : Attribute
	{
		// Token: 0x0600314C RID: 12620 RVA: 0x000A94BF File Offset: 0x000A84BF
		public TypeLibVersionAttribute(int major, int minor)
		{
			this._major = major;
			this._minor = minor;
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x000A94D5 File Offset: 0x000A84D5
		public int MajorVersion
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x0600314E RID: 12622 RVA: 0x000A94DD File Offset: 0x000A84DD
		public int MinorVersion
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x04001957 RID: 6487
		internal int _major;

		// Token: 0x04001958 RID: 6488
		internal int _minor;
	}
}
