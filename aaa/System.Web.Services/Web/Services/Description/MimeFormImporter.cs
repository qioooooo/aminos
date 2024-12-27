using System;
using System.Web.Services.Protocols;

namespace System.Web.Services.Description
{
	// Token: 0x020000CF RID: 207
	internal class MimeFormImporter : MimeImporter
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x0001B52C File Offset: 0x0001A52C
		internal override MimeParameterCollection ImportParameters()
		{
			MimeContentBinding mimeContentBinding = (MimeContentBinding)base.ImportContext.OperationBinding.Input.Extensions.Find(typeof(MimeContentBinding));
			if (mimeContentBinding == null)
			{
				return null;
			}
			if (string.Compare(mimeContentBinding.Type, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) != 0)
			{
				return null;
			}
			MimeParameterCollection mimeParameterCollection = base.ImportContext.ImportStringParametersMessage();
			if (mimeParameterCollection == null)
			{
				return null;
			}
			mimeParameterCollection.WriterType = typeof(HtmlFormParameterWriter);
			return mimeParameterCollection;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001B59F File Offset: 0x0001A59F
		internal override MimeReturn ImportReturn()
		{
			return null;
		}
	}
}
