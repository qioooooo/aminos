using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000020 RID: 32
	public class SecurityDescriptorFlagControl : DirectoryControl
	{
		// Token: 0x06000087 RID: 135 RVA: 0x000043A1 File Offset: 0x000033A1
		public SecurityDescriptorFlagControl()
			: base("1.2.840.113556.1.4.801", null, true, true)
		{
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000043B1 File Offset: 0x000033B1
		public SecurityDescriptorFlagControl(SecurityMasks masks)
			: this()
		{
			this.SecurityMasks = masks;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000043C0 File Offset: 0x000033C0
		// (set) Token: 0x0600008A RID: 138 RVA: 0x000043C8 File Offset: 0x000033C8
		public SecurityMasks SecurityMasks
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

		// Token: 0x0600008B RID: 139 RVA: 0x000043D4 File Offset: 0x000033D4
		public override byte[] GetValue()
		{
			this.directoryControlValue = BerConverter.Encode("{i}", new object[] { (int)this.flag });
			return base.GetValue();
		}

		// Token: 0x040000E5 RID: 229
		private SecurityMasks flag;
	}
}
