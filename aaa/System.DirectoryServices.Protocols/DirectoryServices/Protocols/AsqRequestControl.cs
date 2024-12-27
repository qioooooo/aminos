using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000018 RID: 24
	public class AsqRequestControl : DirectoryControl
	{
		// Token: 0x06000072 RID: 114 RVA: 0x000041CB File Offset: 0x000031CB
		public AsqRequestControl()
			: base("1.2.840.113556.1.4.1504", null, true, true)
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000041DB File Offset: 0x000031DB
		public AsqRequestControl(string attributeName)
			: this()
		{
			this.name = attributeName;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000041EA File Offset: 0x000031EA
		// (set) Token: 0x06000075 RID: 117 RVA: 0x000041F2 File Offset: 0x000031F2
		public string AttributeName
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000041FC File Offset: 0x000031FC
		public override byte[] GetValue()
		{
			this.directoryControlValue = BerConverter.Encode("{s}", new object[] { this.name });
			return base.GetValue();
		}

		// Token: 0x040000E1 RID: 225
		private string name;
	}
}
