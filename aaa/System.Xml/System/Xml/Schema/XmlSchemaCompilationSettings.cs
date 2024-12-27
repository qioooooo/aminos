using System;

namespace System.Xml.Schema
{
	// Token: 0x02000274 RID: 628
	public sealed class XmlSchemaCompilationSettings
	{
		// Token: 0x06001D2A RID: 7466 RVA: 0x00085BE5 File Offset: 0x00084BE5
		public XmlSchemaCompilationSettings()
		{
			this.enableUpaCheck = true;
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06001D2B RID: 7467 RVA: 0x00085BF4 File Offset: 0x00084BF4
		// (set) Token: 0x06001D2C RID: 7468 RVA: 0x00085BFC File Offset: 0x00084BFC
		public bool EnableUpaCheck
		{
			get
			{
				return this.enableUpaCheck;
			}
			set
			{
				this.enableUpaCheck = value;
			}
		}

		// Token: 0x040011CE RID: 4558
		private bool enableUpaCheck;
	}
}
