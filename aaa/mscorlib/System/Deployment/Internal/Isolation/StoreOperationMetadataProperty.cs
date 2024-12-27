using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000210 RID: 528
	internal struct StoreOperationMetadataProperty
	{
		// Token: 0x06001567 RID: 5479 RVA: 0x000370AA File Offset: 0x000360AA
		public StoreOperationMetadataProperty(Guid PropertySet, string Name)
		{
			this = new StoreOperationMetadataProperty(PropertySet, Name, null);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x000370B5 File Offset: 0x000360B5
		public StoreOperationMetadataProperty(Guid PropertySet, string Name, string Value)
		{
			this.GuidPropertySet = PropertySet;
			this.Name = Name;
			this.Value = Value;
			this.ValueSize = ((Value != null) ? new IntPtr((Value.Length + 1) * 2) : IntPtr.Zero);
		}

		// Token: 0x04000880 RID: 2176
		public Guid GuidPropertySet;

		// Token: 0x04000881 RID: 2177
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000882 RID: 2178
		[MarshalAs(UnmanagedType.SysUInt)]
		public IntPtr ValueSize;

		// Token: 0x04000883 RID: 2179
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
