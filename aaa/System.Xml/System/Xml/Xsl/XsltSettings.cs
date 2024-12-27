using System;
using System.CodeDom.Compiler;

namespace System.Xml.Xsl
{
	// Token: 0x0200017B RID: 379
	public sealed class XsltSettings
	{
		// Token: 0x06001431 RID: 5169 RVA: 0x00056A1B File Offset: 0x00055A1B
		public XsltSettings()
		{
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x00056A2A File Offset: 0x00055A2A
		public XsltSettings(bool enableDocumentFunction, bool enableScript)
		{
			this.enableDocumentFunction = enableDocumentFunction;
			this.enableScript = enableScript;
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x00056A47 File Offset: 0x00055A47
		public static XsltSettings Default
		{
			get
			{
				return new XsltSettings(false, false);
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x00056A50 File Offset: 0x00055A50
		public static XsltSettings TrustedXslt
		{
			get
			{
				return new XsltSettings(true, true);
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x00056A59 File Offset: 0x00055A59
		// (set) Token: 0x06001436 RID: 5174 RVA: 0x00056A61 File Offset: 0x00055A61
		public bool EnableDocumentFunction
		{
			get
			{
				return this.enableDocumentFunction;
			}
			set
			{
				this.enableDocumentFunction = value;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x00056A6A File Offset: 0x00055A6A
		// (set) Token: 0x06001438 RID: 5176 RVA: 0x00056A72 File Offset: 0x00055A72
		public bool EnableScript
		{
			get
			{
				return this.enableScript;
			}
			set
			{
				this.enableScript = value;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x00056A7B File Offset: 0x00055A7B
		// (set) Token: 0x0600143A RID: 5178 RVA: 0x00056A83 File Offset: 0x00055A83
		internal bool CheckOnly
		{
			get
			{
				return this.checkOnly;
			}
			set
			{
				this.checkOnly = value;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x00056A8C File Offset: 0x00055A8C
		// (set) Token: 0x0600143C RID: 5180 RVA: 0x00056A94 File Offset: 0x00055A94
		internal bool IncludeDebugInformation
		{
			get
			{
				return this.includeDebugInformation;
			}
			set
			{
				this.includeDebugInformation = value;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00056A9D File Offset: 0x00055A9D
		// (set) Token: 0x0600143E RID: 5182 RVA: 0x00056AA5 File Offset: 0x00055AA5
		internal int WarningLevel
		{
			get
			{
				return this.warningLevel;
			}
			set
			{
				this.warningLevel = value;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x00056AAE File Offset: 0x00055AAE
		// (set) Token: 0x06001440 RID: 5184 RVA: 0x00056AB6 File Offset: 0x00055AB6
		internal bool TreatWarningsAsErrors
		{
			get
			{
				return this.treatWarningsAsErrors;
			}
			set
			{
				this.treatWarningsAsErrors = value;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x00056ABF File Offset: 0x00055ABF
		// (set) Token: 0x06001442 RID: 5186 RVA: 0x00056AC7 File Offset: 0x00055AC7
		internal TempFileCollection TempFiles
		{
			get
			{
				return this.tempFiles;
			}
			set
			{
				this.tempFiles = value;
			}
		}

		// Token: 0x04000C4D RID: 3149
		private bool enableDocumentFunction;

		// Token: 0x04000C4E RID: 3150
		private bool enableScript;

		// Token: 0x04000C4F RID: 3151
		private bool checkOnly;

		// Token: 0x04000C50 RID: 3152
		private bool includeDebugInformation;

		// Token: 0x04000C51 RID: 3153
		private int warningLevel = -1;

		// Token: 0x04000C52 RID: 3154
		private bool treatWarningsAsErrors;

		// Token: 0x04000C53 RID: 3155
		private TempFileCollection tempFiles;
	}
}
