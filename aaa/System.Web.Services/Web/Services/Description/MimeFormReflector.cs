using System;
using System.Web.Services.Protocols;

namespace System.Web.Services.Description
{
	// Token: 0x020000D1 RID: 209
	internal class MimeFormReflector : MimeReflector
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x0001B5C4 File Offset: 0x0001A5C4
		internal override bool ReflectParameters()
		{
			if (!ValueCollectionParameterReader.IsSupported(base.ReflectionContext.Method))
			{
				return false;
			}
			base.ReflectionContext.ReflectStringParametersMessage();
			MimeContentBinding mimeContentBinding = new MimeContentBinding();
			mimeContentBinding.Type = "application/x-www-form-urlencoded";
			base.ReflectionContext.OperationBinding.Input.Extensions.Add(mimeContentBinding);
			return true;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001B61E File Offset: 0x0001A61E
		internal override bool ReflectReturn()
		{
			return false;
		}
	}
}
