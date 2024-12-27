using System;
using System.Text;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000024 RID: 36
	public class VerifyNameControl : DirectoryControl
	{
		// Token: 0x06000093 RID: 147 RVA: 0x000044BD File Offset: 0x000034BD
		public VerifyNameControl()
			: base("1.2.840.113556.1.4.1338", null, true, true)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000044CD File Offset: 0x000034CD
		public VerifyNameControl(string serverName)
			: this()
		{
			if (serverName == null)
			{
				throw new ArgumentNullException("serverName");
			}
			this.name = serverName;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000044EA File Offset: 0x000034EA
		public VerifyNameControl(string serverName, int flag)
			: this(serverName)
		{
			this.flag = flag;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000044FA File Offset: 0x000034FA
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00004502 File Offset: 0x00003502
		public string ServerName
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.name = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004519 File Offset: 0x00003519
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004521 File Offset: 0x00003521
		public int Flag
		{
			get
			{
				return this.flag;
			}
			set
			{
				this.flag = value;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000452C File Offset: 0x0000352C
		public override byte[] GetValue()
		{
			byte[] array = null;
			if (this.ServerName != null)
			{
				UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
				array = unicodeEncoding.GetBytes(this.ServerName);
			}
			this.directoryControlValue = BerConverter.Encode("{io}", new object[] { this.flag, array });
			return base.GetValue();
		}

		// Token: 0x040000E7 RID: 231
		private string name;

		// Token: 0x040000E8 RID: 232
		private int flag;
	}
}
