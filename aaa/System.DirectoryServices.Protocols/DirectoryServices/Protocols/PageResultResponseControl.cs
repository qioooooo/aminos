using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000028 RID: 40
	public class PageResultResponseControl : DirectoryControl
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00004828 File Offset: 0x00003828
		internal PageResultResponseControl(int count, byte[] cookie, bool criticality, byte[] controlValue)
			: base("1.2.840.113556.1.4.319", controlValue, criticality, true)
		{
			this.count = count;
			this.pageCookie = cookie;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004848 File Offset: 0x00003848
		public byte[] Cookie
		{
			get
			{
				if (this.pageCookie == null)
				{
					return new byte[0];
				}
				byte[] array = new byte[this.pageCookie.Length];
				for (int i = 0; i < this.pageCookie.Length; i++)
				{
					array[i] = this.pageCookie[i];
				}
				return array;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004891 File Offset: 0x00003891
		public int TotalCount
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x040000F1 RID: 241
		private byte[] pageCookie;

		// Token: 0x040000F2 RID: 242
		private int count;
	}
}
