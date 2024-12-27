using System;
using System.CodeDom;

namespace System.Web.Services.Description
{
	// Token: 0x020000D2 RID: 210
	internal class MimeParameter
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0001B629 File Offset: 0x0001A629
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x0001B63F File Offset: 0x0001A63F
		internal string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0001B648 File Offset: 0x0001A648
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x0001B65E File Offset: 0x0001A65E
		internal string TypeName
		{
			get
			{
				if (this.typeName != null)
				{
					return this.typeName;
				}
				return string.Empty;
			}
			set
			{
				this.typeName = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001B667 File Offset: 0x0001A667
		internal CodeAttributeDeclarationCollection Attributes
		{
			get
			{
				if (this.attrs == null)
				{
					this.attrs = new CodeAttributeDeclarationCollection();
				}
				return this.attrs;
			}
		}

		// Token: 0x04000426 RID: 1062
		private string name;

		// Token: 0x04000427 RID: 1063
		private string typeName;

		// Token: 0x04000428 RID: 1064
		private CodeAttributeDeclarationCollection attrs;
	}
}
