using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200030E RID: 782
	[Serializable]
	internal static class MetadataArgs
	{
		// Token: 0x04000CBE RID: 3262
		public static MetadataArgs.SkipAddresses Skip = default(MetadataArgs.SkipAddresses);

		// Token: 0x0200030F RID: 783
		[ComVisible(true)]
		[Serializable]
		[StructLayout(LayoutKind.Auto)]
		public struct SkipAddresses
		{
			// Token: 0x04000CBF RID: 3263
			public string String;

			// Token: 0x04000CC0 RID: 3264
			public int[] Int32Array;

			// Token: 0x04000CC1 RID: 3265
			public byte[] ByteArray;

			// Token: 0x04000CC2 RID: 3266
			public MetadataFieldOffset[] MetadataFieldOffsetArray;

			// Token: 0x04000CC3 RID: 3267
			public int Int32;

			// Token: 0x04000CC4 RID: 3268
			public TypeAttributes TypeAttributes;

			// Token: 0x04000CC5 RID: 3269
			public MethodAttributes MethodAttributes;

			// Token: 0x04000CC6 RID: 3270
			public PropertyAttributes PropertyAttributes;

			// Token: 0x04000CC7 RID: 3271
			public MethodImplAttributes MethodImplAttributes;

			// Token: 0x04000CC8 RID: 3272
			public ParameterAttributes ParameterAttributes;

			// Token: 0x04000CC9 RID: 3273
			public FieldAttributes FieldAttributes;

			// Token: 0x04000CCA RID: 3274
			public EventAttributes EventAttributes;

			// Token: 0x04000CCB RID: 3275
			public MetadataColumnType MetadataColumnType;

			// Token: 0x04000CCC RID: 3276
			public PInvokeAttributes PInvokeAttributes;

			// Token: 0x04000CCD RID: 3277
			public MethodSemanticsAttributes MethodSemanticsAttributes;

			// Token: 0x04000CCE RID: 3278
			public DeclSecurityAttributes DeclSecurityAttributes;

			// Token: 0x04000CCF RID: 3279
			public CorElementType CorElementType;

			// Token: 0x04000CD0 RID: 3280
			public ConstArray ConstArray;

			// Token: 0x04000CD1 RID: 3281
			public Guid Guid;
		}
	}
}
