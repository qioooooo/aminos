using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000543 RID: 1347
	[StructLayout(LayoutKind.Sequential)]
	internal class SecSizes
	{
		// Token: 0x06002919 RID: 10521 RVA: 0x000AB7F4 File Offset: 0x000AA7F4
		internal unsafe SecSizes(byte[] memory)
		{
			checked
			{
				fixed (void* ptr = memory)
				{
					IntPtr intPtr = new IntPtr(ptr);
					try
					{
						this.MaxToken = (int)((uint)Marshal.ReadInt32(intPtr));
						this.MaxSignature = (int)((uint)Marshal.ReadInt32(intPtr, 4));
						this.BlockSize = (int)((uint)Marshal.ReadInt32(intPtr, 8));
						this.SecurityTrailer = (int)((uint)Marshal.ReadInt32(intPtr, 12));
					}
					catch (OverflowException)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x040027DE RID: 10206
		public readonly int MaxToken;

		// Token: 0x040027DF RID: 10207
		public readonly int MaxSignature;

		// Token: 0x040027E0 RID: 10208
		public readonly int BlockSize;

		// Token: 0x040027E1 RID: 10209
		public readonly int SecurityTrailer;

		// Token: 0x040027E2 RID: 10210
		public static readonly int SizeOf = Marshal.SizeOf(typeof(SecSizes));
	}
}
