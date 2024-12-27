using System;
using System.CodeDom;

namespace System.Web.Services.Description
{
	// Token: 0x020000C5 RID: 197
	internal abstract class MimeImporter
	{
		// Token: 0x06000554 RID: 1364
		internal abstract MimeParameterCollection ImportParameters();

		// Token: 0x06000555 RID: 1365
		internal abstract MimeReturn ImportReturn();

		// Token: 0x06000556 RID: 1366 RVA: 0x0001B156 File Offset: 0x0001A156
		internal virtual void GenerateCode(MimeReturn[] importedReturns, MimeParameterCollection[] importedParameters)
		{
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001B158 File Offset: 0x0001A158
		internal virtual void AddClassMetadata(CodeTypeDeclaration codeClass)
		{
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001B15A File Offset: 0x0001A15A
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x0001B162 File Offset: 0x0001A162
		internal HttpProtocolImporter ImportContext
		{
			get
			{
				return this.protocol;
			}
			set
			{
				this.protocol = value;
			}
		}

		// Token: 0x04000414 RID: 1044
		private HttpProtocolImporter protocol;
	}
}
