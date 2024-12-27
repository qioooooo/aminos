using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x0200005B RID: 91
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class PropertyItemInternal : IDisposable
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x00017A67 File Offset: 0x00016A67
		internal PropertyItemInternal()
		{
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00017A7C File Offset: 0x00016A7C
		~PropertyItemInternal()
		{
			this.Dispose(false);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00017AAC File Offset: 0x00016AAC
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00017AB5 File Offset: 0x00016AB5
		private void Dispose(bool disposing)
		{
			if (this.value != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.value);
				this.value = IntPtr.Zero;
			}
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00017AE8 File Offset: 0x00016AE8
		internal static PropertyItemInternal ConvertFromPropertyItem(PropertyItem propItem)
		{
			PropertyItemInternal propertyItemInternal = new PropertyItemInternal();
			propertyItemInternal.id = propItem.Id;
			propertyItemInternal.len = 0;
			propertyItemInternal.type = propItem.Type;
			byte[] array = propItem.Value;
			if (array != null)
			{
				int num = array.Length;
				propertyItemInternal.len = num;
				propertyItemInternal.value = Marshal.AllocHGlobal(num);
				Marshal.Copy(array, 0, propertyItemInternal.value, num);
			}
			return propertyItemInternal;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00017B4C File Offset: 0x00016B4C
		internal static PropertyItem[] ConvertFromMemory(IntPtr propdata, int count)
		{
			PropertyItem[] array = new PropertyItem[count];
			for (int i = 0; i < count; i++)
			{
				PropertyItemInternal propertyItemInternal = null;
				try
				{
					propertyItemInternal = (PropertyItemInternal)UnsafeNativeMethods.PtrToStructure(propdata, typeof(PropertyItemInternal));
					array[i] = new PropertyItem();
					array[i].Id = propertyItemInternal.id;
					array[i].Len = propertyItemInternal.len;
					array[i].Type = propertyItemInternal.type;
					array[i].Value = propertyItemInternal.Value;
					propertyItemInternal.value = IntPtr.Zero;
				}
				finally
				{
					if (propertyItemInternal != null)
					{
						propertyItemInternal.Dispose();
					}
				}
				propdata = (IntPtr)((long)propdata + (long)Marshal.SizeOf(typeof(PropertyItemInternal)));
			}
			return array;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00017C10 File Offset: 0x00016C10
		public byte[] Value
		{
			get
			{
				if (this.len == 0)
				{
					return null;
				}
				byte[] array = new byte[this.len];
				Marshal.Copy(this.value, array, 0, this.len);
				return array;
			}
		}

		// Token: 0x0400045F RID: 1119
		public int id;

		// Token: 0x04000460 RID: 1120
		public int len;

		// Token: 0x04000461 RID: 1121
		public short type;

		// Token: 0x04000462 RID: 1122
		public IntPtr value = IntPtr.Zero;
	}
}
