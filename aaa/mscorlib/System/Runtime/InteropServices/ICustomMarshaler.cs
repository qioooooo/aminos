using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FB RID: 1275
	[ComVisible(true)]
	public interface ICustomMarshaler
	{
		// Token: 0x0600318F RID: 12687
		object MarshalNativeToManaged(IntPtr pNativeData);

		// Token: 0x06003190 RID: 12688
		IntPtr MarshalManagedToNative(object ManagedObj);

		// Token: 0x06003191 RID: 12689
		void CleanUpNativeData(IntPtr pNativeData);

		// Token: 0x06003192 RID: 12690
		void CleanUpManagedData(object ManagedObj);

		// Token: 0x06003193 RID: 12691
		int GetNativeDataSize();
	}
}
