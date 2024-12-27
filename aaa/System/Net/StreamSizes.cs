using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000542 RID: 1346
	[StructLayout(LayoutKind.Sequential)]
	internal class StreamSizes
	{
		// Token: 0x06002917 RID: 10519 RVA: 0x000AB748 File Offset: 0x000AA748
		internal unsafe StreamSizes(byte[] memory)
		{
			checked
			{
				fixed (void* ptr = memory)
				{
					IntPtr intPtr = new IntPtr(ptr);
					try
					{
						this.header = (int)((uint)Marshal.ReadInt32(intPtr));
						this.trailer = (int)((uint)Marshal.ReadInt32(intPtr, 4));
						this.maximumMessage = (int)((uint)Marshal.ReadInt32(intPtr, 8));
						this.buffersCount = (int)((uint)Marshal.ReadInt32(intPtr, 12));
						this.blockSize = (int)((uint)Marshal.ReadInt32(intPtr, 16));
					}
					catch (OverflowException)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x040027D8 RID: 10200
		public int header;

		// Token: 0x040027D9 RID: 10201
		public int trailer;

		// Token: 0x040027DA RID: 10202
		public int maximumMessage;

		// Token: 0x040027DB RID: 10203
		public int buffersCount;

		// Token: 0x040027DC RID: 10204
		public int blockSize;

		// Token: 0x040027DD RID: 10205
		public static readonly int SizeOf = Marshal.SizeOf(typeof(StreamSizes));
	}
}
