using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200010B RID: 267
	internal struct StoreOperationMetadataProperty
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x000082DE File Offset: 0x000072DE
		public StoreOperationMetadataProperty(Guid PropertySet, string Name)
		{
			this = new StoreOperationMetadataProperty(PropertySet, Name, null);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000082E9 File Offset: 0x000072E9
		public StoreOperationMetadataProperty(Guid PropertySet, string Name, string Value)
		{
			this.GuidPropertySet = PropertySet;
			this.Name = Name;
			this.Value = Value;
			this.ValueSize = ((Value != null) ? new IntPtr((Value.Length + 1) * 2) : IntPtr.Zero);
		}

		// Token: 0x04000DF4 RID: 3572
		public Guid GuidPropertySet;

		// Token: 0x04000DF5 RID: 3573
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000DF6 RID: 3574
		[MarshalAs(UnmanagedType.SysUInt)]
		public IntPtr ValueSize;

		// Token: 0x04000DF7 RID: 3575
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
