using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x02000079 RID: 121
	public sealed class ColorPalette
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x0001BFE1 File Offset: 0x0001AFE1
		public int Flags
		{
			get
			{
				return this.flags;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0001BFE9 File Offset: 0x0001AFE9
		public Color[] Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001BFF1 File Offset: 0x0001AFF1
		internal ColorPalette(int count)
		{
			this.entries = new Color[count];
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001C005 File Offset: 0x0001B005
		internal ColorPalette()
		{
			this.entries = new Color[1];
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001C01C File Offset: 0x0001B01C
		internal void ConvertFromMemory(IntPtr memory)
		{
			this.flags = Marshal.ReadInt32(memory);
			int num = Marshal.ReadInt32((IntPtr)((long)memory + 4L));
			this.entries = new Color[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = Marshal.ReadInt32((IntPtr)((long)memory + 8L + (long)(i * 4)));
				this.entries[i] = Color.FromArgb(num2);
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001C094 File Offset: 0x0001B094
		internal IntPtr ConvertToMemory()
		{
			int num = this.entries.Length;
			IntPtr intPtr;
			checked
			{
				intPtr = Marshal.AllocHGlobal(4 * (2 + num));
				Marshal.WriteInt32(intPtr, 0, this.flags);
				Marshal.WriteInt32((IntPtr)((long)intPtr + 4L), 0, num);
			}
			for (int i = 0; i < num; i++)
			{
				Marshal.WriteInt32((IntPtr)((long)intPtr + (long)(4 * (i + 2))), 0, this.entries[i].ToArgb());
			}
			return intPtr;
		}

		// Token: 0x040004D9 RID: 1241
		private int flags;

		// Token: 0x040004DA RID: 1242
		private Color[] entries;
	}
}
