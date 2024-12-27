using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000136 RID: 310
	internal struct StoreOperationMetadataProperty
	{
		// Token: 0x060006CF RID: 1743 RVA: 0x0001F996 File Offset: 0x0001E996
		public StoreOperationMetadataProperty(Guid PropertySet, string Name)
		{
			this = new StoreOperationMetadataProperty(PropertySet, Name, null);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001F9A1 File Offset: 0x0001E9A1
		public StoreOperationMetadataProperty(Guid PropertySet, string Name, string Value)
		{
			this.GuidPropertySet = PropertySet;
			this.Name = Name;
			this.Value = Value;
			this.ValueSize = ((Value != null) ? new IntPtr((Value.Length + 1) * 2) : IntPtr.Zero);
		}

		// Token: 0x04000568 RID: 1384
		public Guid GuidPropertySet;

		// Token: 0x04000569 RID: 1385
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x0400056A RID: 1386
		[MarshalAs(UnmanagedType.SysUInt)]
		public IntPtr ValueSize;

		// Token: 0x0400056B RID: 1387
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
