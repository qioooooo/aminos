using System;
using System.CodeDom;

namespace System.Web.Services.Description
{
	// Token: 0x020000D4 RID: 212
	internal class MimeReturn
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0001B71B File Offset: 0x0001A71B
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x0001B731 File Offset: 0x0001A731
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001B73A File Offset: 0x0001A73A
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x0001B742 File Offset: 0x0001A742
		internal Type ReaderType
		{
			get
			{
				return this.readerType;
			}
			set
			{
				this.readerType = value;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0001B74B File Offset: 0x0001A74B
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

		// Token: 0x0400042A RID: 1066
		private string typeName;

		// Token: 0x0400042B RID: 1067
		private Type readerType;

		// Token: 0x0400042C RID: 1068
		private CodeAttributeDeclarationCollection attrs;
	}
}
