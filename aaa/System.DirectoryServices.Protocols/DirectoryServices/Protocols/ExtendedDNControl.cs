using System;
using System.ComponentModel;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200001C RID: 28
	public class ExtendedDNControl : DirectoryControl
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000042E7 File Offset: 0x000032E7
		public ExtendedDNControl()
			: base("1.2.840.113556.1.4.529", null, true, true)
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000042F7 File Offset: 0x000032F7
		public ExtendedDNControl(ExtendedDNFlag flag)
			: this()
		{
			this.Flag = flag;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00004306 File Offset: 0x00003306
		// (set) Token: 0x06000082 RID: 130 RVA: 0x0000430E File Offset: 0x0000330E
		public ExtendedDNFlag Flag
		{
			get
			{
				return this.format;
			}
			set
			{
				if (value < ExtendedDNFlag.HexString || value > ExtendedDNFlag.StandardString)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ExtendedDNFlag));
				}
				this.format = value;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004338 File Offset: 0x00003338
		public override byte[] GetValue()
		{
			this.directoryControlValue = BerConverter.Encode("{i}", new object[] { (int)this.format });
			return base.GetValue();
		}

		// Token: 0x040000E4 RID: 228
		private ExtendedDNFlag format;
	}
}
