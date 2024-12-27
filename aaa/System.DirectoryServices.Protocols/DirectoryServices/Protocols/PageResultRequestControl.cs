using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000027 RID: 39
	public class PageResultRequestControl : DirectoryControl
	{
		// Token: 0x060000AA RID: 170 RVA: 0x0000472C File Offset: 0x0000372C
		public PageResultRequestControl()
			: base("1.2.840.113556.1.4.319", null, true, true)
		{
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004747 File Offset: 0x00003747
		public PageResultRequestControl(int pageSize)
			: this()
		{
			this.PageSize = pageSize;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004756 File Offset: 0x00003756
		public PageResultRequestControl(byte[] cookie)
			: this()
		{
			this.pageCookie = cookie;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004765 File Offset: 0x00003765
		// (set) Token: 0x060000AE RID: 174 RVA: 0x0000476D File Offset: 0x0000376D
		public int PageSize
		{
			get
			{
				return this.size;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ValidValue"), "value");
				}
				this.size = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004790 File Offset: 0x00003790
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000047D9 File Offset: 0x000037D9
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
			set
			{
				this.pageCookie = value;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000047E4 File Offset: 0x000037E4
		public override byte[] GetValue()
		{
			object[] array = new object[] { this.size, this.pageCookie };
			this.directoryControlValue = BerConverter.Encode("{io}", array);
			return base.GetValue();
		}

		// Token: 0x040000EF RID: 239
		private int size = 512;

		// Token: 0x040000F0 RID: 240
		private byte[] pageCookie;
	}
}
