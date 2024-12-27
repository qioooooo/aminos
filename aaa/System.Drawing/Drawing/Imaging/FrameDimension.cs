using System;

namespace System.Drawing.Imaging
{
	// Token: 0x0200008F RID: 143
	public sealed class FrameDimension
	{
		// Token: 0x0600080C RID: 2060 RVA: 0x0001EE6F File Offset: 0x0001DE6F
		public FrameDimension(Guid guid)
		{
			this.guid = guid;
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0001EE7E File Offset: 0x0001DE7E
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x0001EE86 File Offset: 0x0001DE86
		public static FrameDimension Time
		{
			get
			{
				return FrameDimension.time;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0001EE8D File Offset: 0x0001DE8D
		public static FrameDimension Resolution
		{
			get
			{
				return FrameDimension.resolution;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0001EE94 File Offset: 0x0001DE94
		public static FrameDimension Page
		{
			get
			{
				return FrameDimension.page;
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001EE9C File Offset: 0x0001DE9C
		public override bool Equals(object o)
		{
			FrameDimension frameDimension = o as FrameDimension;
			return frameDimension != null && this.guid == frameDimension.guid;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001EEC6 File Offset: 0x0001DEC6
		public override int GetHashCode()
		{
			return this.guid.GetHashCode();
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001EEDC File Offset: 0x0001DEDC
		public override string ToString()
		{
			if (this == FrameDimension.time)
			{
				return "Time";
			}
			if (this == FrameDimension.resolution)
			{
				return "Resolution";
			}
			if (this == FrameDimension.page)
			{
				return "Page";
			}
			return "[FrameDimension: " + this.guid + "]";
		}

		// Token: 0x04000650 RID: 1616
		private static FrameDimension time = new FrameDimension(new Guid("{6aedbd6d-3fb5-418a-83a6-7f45229dc872}"));

		// Token: 0x04000651 RID: 1617
		private static FrameDimension resolution = new FrameDimension(new Guid("{84236f7b-3bd3-428f-8dab-4ea1439ca315}"));

		// Token: 0x04000652 RID: 1618
		private static FrameDimension page = new FrameDimension(new Guid("{7462dc86-6180-4c7e-8e3f-ee7333a7a483}"));

		// Token: 0x04000653 RID: 1619
		private Guid guid;
	}
}
