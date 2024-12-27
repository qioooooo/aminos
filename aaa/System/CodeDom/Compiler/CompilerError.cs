using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001EF RID: 495
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CompilerError
	{
		// Token: 0x060010A6 RID: 4262 RVA: 0x00036F52 File Offset: 0x00035F52
		public CompilerError()
		{
			this.line = 0;
			this.column = 0;
			this.errorNumber = string.Empty;
			this.errorText = string.Empty;
			this.fileName = string.Empty;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00036F89 File Offset: 0x00035F89
		public CompilerError(string fileName, int line, int column, string errorNumber, string errorText)
		{
			this.line = line;
			this.column = column;
			this.errorNumber = errorNumber;
			this.errorText = errorText;
			this.fileName = fileName;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00036FB6 File Offset: 0x00035FB6
		// (set) Token: 0x060010A9 RID: 4265 RVA: 0x00036FBE File Offset: 0x00035FBE
		public int Line
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x00036FC7 File Offset: 0x00035FC7
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x00036FCF File Offset: 0x00035FCF
		public int Column
		{
			get
			{
				return this.column;
			}
			set
			{
				this.column = value;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x00036FD8 File Offset: 0x00035FD8
		// (set) Token: 0x060010AD RID: 4269 RVA: 0x00036FE0 File Offset: 0x00035FE0
		public string ErrorNumber
		{
			get
			{
				return this.errorNumber;
			}
			set
			{
				this.errorNumber = value;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x00036FE9 File Offset: 0x00035FE9
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x00036FF1 File Offset: 0x00035FF1
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
			set
			{
				this.errorText = value;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00036FFA File Offset: 0x00035FFA
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x00037002 File Offset: 0x00036002
		public bool IsWarning
		{
			get
			{
				return this.warning;
			}
			set
			{
				this.warning = value;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x0003700B File Offset: 0x0003600B
		// (set) Token: 0x060010B3 RID: 4275 RVA: 0x00037013 File Offset: 0x00036013
		public string FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0003701C File Offset: 0x0003601C
		public override string ToString()
		{
			if (this.FileName.Length > 0)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}({1},{2}) : {3} {4}: {5}", new object[]
				{
					this.FileName,
					this.Line,
					this.Column,
					this.IsWarning ? "warning" : "error",
					this.ErrorNumber,
					this.ErrorText
				});
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} {1}: {2}", new object[]
			{
				this.IsWarning ? "warning" : "error",
				this.ErrorNumber,
				this.ErrorText
			});
		}

		// Token: 0x04000F5D RID: 3933
		private int line;

		// Token: 0x04000F5E RID: 3934
		private int column;

		// Token: 0x04000F5F RID: 3935
		private string errorNumber;

		// Token: 0x04000F60 RID: 3936
		private bool warning;

		// Token: 0x04000F61 RID: 3937
		private string errorText;

		// Token: 0x04000F62 RID: 3938
		private string fileName;
	}
}
