using System;

namespace System.Deployment.Application
{
	// Token: 0x0200007D RID: 125
	internal struct ASSEMBLYMETADATA
	{
		// Token: 0x040002B0 RID: 688
		public ushort usMajorVersion;

		// Token: 0x040002B1 RID: 689
		public ushort usMinorVersion;

		// Token: 0x040002B2 RID: 690
		public ushort usBuildNumber;

		// Token: 0x040002B3 RID: 691
		public ushort usRevisionNumber;

		// Token: 0x040002B4 RID: 692
		public IntPtr rpLocale;

		// Token: 0x040002B5 RID: 693
		public uint cchLocale;

		// Token: 0x040002B6 RID: 694
		public IntPtr rpProcessors;

		// Token: 0x040002B7 RID: 695
		public uint cProcessors;

		// Token: 0x040002B8 RID: 696
		public IntPtr rOses;

		// Token: 0x040002B9 RID: 697
		public uint cOses;
	}
}
