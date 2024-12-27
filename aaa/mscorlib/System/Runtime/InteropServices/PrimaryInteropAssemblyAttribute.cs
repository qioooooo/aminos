using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EB RID: 1259
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
	public sealed class PrimaryInteropAssemblyAttribute : Attribute
	{
		// Token: 0x06003144 RID: 12612 RVA: 0x000A945C File Offset: 0x000A845C
		public PrimaryInteropAssemblyAttribute(int major, int minor)
		{
			this._major = major;
			this._minor = minor;
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x000A9472 File Offset: 0x000A8472
		public int MajorVersion
		{
			get
			{
				return this._major;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x000A947A File Offset: 0x000A847A
		public int MinorVersion
		{
			get
			{
				return this._minor;
			}
		}

		// Token: 0x04001952 RID: 6482
		internal int _major;

		// Token: 0x04001953 RID: 6483
		internal int _minor;
	}
}
