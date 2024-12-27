using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200002C RID: 44
	public class VlvResponseControl : DirectoryControl
	{
		// Token: 0x060000CF RID: 207 RVA: 0x0000501C File Offset: 0x0000401C
		internal VlvResponseControl(int targetPosition, int count, byte[] context, ResultCode result, bool criticality, byte[] value)
			: base("2.16.840.1.113730.3.4.10", value, criticality, true)
		{
			this.position = targetPosition;
			this.count = count;
			this.context = context;
			this.result = result;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000504B File Offset: 0x0000404B
		public int TargetPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005053 File Offset: 0x00004053
		public int ContentCount
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000505C File Offset: 0x0000405C
		public byte[] ContextId
		{
			get
			{
				if (this.context == null)
				{
					return new byte[0];
				}
				byte[] array = new byte[this.context.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.context[i];
				}
				return array;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000050A0 File Offset: 0x000040A0
		public ResultCode Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x040000FC RID: 252
		private int position;

		// Token: 0x040000FD RID: 253
		private int count;

		// Token: 0x040000FE RID: 254
		private byte[] context;

		// Token: 0x040000FF RID: 255
		private ResultCode result;
	}
}
