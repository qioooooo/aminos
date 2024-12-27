using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000511 RID: 1297
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibImporterFlags
	{
		// Token: 0x040019B0 RID: 6576
		None = 0,
		// Token: 0x040019B1 RID: 6577
		PrimaryInteropAssembly = 1,
		// Token: 0x040019B2 RID: 6578
		UnsafeInterfaces = 2,
		// Token: 0x040019B3 RID: 6579
		SafeArrayAsSystemArray = 4,
		// Token: 0x040019B4 RID: 6580
		TransformDispRetVals = 8,
		// Token: 0x040019B5 RID: 6581
		PreventClassMembers = 16,
		// Token: 0x040019B6 RID: 6582
		SerializableValueClasses = 32,
		// Token: 0x040019B7 RID: 6583
		ImportAsX86 = 256,
		// Token: 0x040019B8 RID: 6584
		ImportAsX64 = 512,
		// Token: 0x040019B9 RID: 6585
		ImportAsItanium = 1024,
		// Token: 0x040019BA RID: 6586
		ImportAsAgnostic = 2048,
		// Token: 0x040019BB RID: 6587
		ReflectionOnlyLoading = 4096
	}
}
