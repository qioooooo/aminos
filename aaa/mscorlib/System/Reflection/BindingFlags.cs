using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E0 RID: 736
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum BindingFlags
	{
		// Token: 0x04000AA5 RID: 2725
		Default = 0,
		// Token: 0x04000AA6 RID: 2726
		IgnoreCase = 1,
		// Token: 0x04000AA7 RID: 2727
		DeclaredOnly = 2,
		// Token: 0x04000AA8 RID: 2728
		Instance = 4,
		// Token: 0x04000AA9 RID: 2729
		Static = 8,
		// Token: 0x04000AAA RID: 2730
		Public = 16,
		// Token: 0x04000AAB RID: 2731
		NonPublic = 32,
		// Token: 0x04000AAC RID: 2732
		FlattenHierarchy = 64,
		// Token: 0x04000AAD RID: 2733
		InvokeMethod = 256,
		// Token: 0x04000AAE RID: 2734
		CreateInstance = 512,
		// Token: 0x04000AAF RID: 2735
		GetField = 1024,
		// Token: 0x04000AB0 RID: 2736
		SetField = 2048,
		// Token: 0x04000AB1 RID: 2737
		GetProperty = 4096,
		// Token: 0x04000AB2 RID: 2738
		SetProperty = 8192,
		// Token: 0x04000AB3 RID: 2739
		PutDispProperty = 16384,
		// Token: 0x04000AB4 RID: 2740
		PutRefDispProperty = 32768,
		// Token: 0x04000AB5 RID: 2741
		ExactBinding = 65536,
		// Token: 0x04000AB6 RID: 2742
		SuppressChangeType = 131072,
		// Token: 0x04000AB7 RID: 2743
		OptionalParamBinding = 262144,
		// Token: 0x04000AB8 RID: 2744
		IgnoreReturn = 16777216
	}
}
