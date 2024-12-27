using System;
using System.IO;
using System.Web.Services.Protocols;

namespace System.Web.Services.Description
{
	// Token: 0x020000C6 RID: 198
	internal class MimeAnyImporter : MimeImporter
	{
		// Token: 0x0600055B RID: 1371 RVA: 0x0001B173 File Offset: 0x0001A173
		internal override MimeParameterCollection ImportParameters()
		{
			return null;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001B178 File Offset: 0x0001A178
		internal override MimeReturn ImportReturn()
		{
			if (base.ImportContext.OperationBinding.Output.Extensions.Count == 0)
			{
				return null;
			}
			return new MimeReturn
			{
				TypeName = typeof(Stream).FullName,
				ReaderType = typeof(AnyReturnReader)
			};
		}
	}
}
