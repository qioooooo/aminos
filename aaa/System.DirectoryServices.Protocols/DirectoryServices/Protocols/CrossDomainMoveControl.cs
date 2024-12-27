using System;
using System.Text;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200001A RID: 26
	public class CrossDomainMoveControl : DirectoryControl
	{
		// Token: 0x06000079 RID: 121 RVA: 0x0000424F File Offset: 0x0000324F
		public CrossDomainMoveControl()
			: base("1.2.840.113556.1.4.521", null, true, true)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000425F File Offset: 0x0000325F
		public CrossDomainMoveControl(string targetDomainController)
			: this()
		{
			this.dcName = targetDomainController;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000426E File Offset: 0x0000326E
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00004276 File Offset: 0x00003276
		public string TargetDomainController
		{
			get
			{
				return this.dcName;
			}
			set
			{
				this.dcName = value;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004280 File Offset: 0x00003280
		public override byte[] GetValue()
		{
			if (this.dcName != null)
			{
				UTF8Encoding utf8Encoding = new UTF8Encoding();
				byte[] bytes = utf8Encoding.GetBytes(this.dcName);
				this.directoryControlValue = new byte[bytes.Length + 2];
				for (int i = 0; i < bytes.Length; i++)
				{
					this.directoryControlValue[i] = bytes[i];
				}
			}
			return base.GetValue();
		}

		// Token: 0x040000E3 RID: 227
		private string dcName;
	}
}
