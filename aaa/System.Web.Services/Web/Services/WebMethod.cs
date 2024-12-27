using System;
using System.Reflection;

namespace System.Web.Services
{
	// Token: 0x02000013 RID: 19
	internal class WebMethod
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00002985 File Offset: 0x00001985
		internal WebMethod(MethodInfo declaration, WebServiceBindingAttribute binding, WebMethodAttribute attribute)
		{
			this.declaration = declaration;
			this.binding = binding;
			this.attribute = attribute;
		}

		// Token: 0x04000214 RID: 532
		internal MethodInfo declaration;

		// Token: 0x04000215 RID: 533
		internal WebServiceBindingAttribute binding;

		// Token: 0x04000216 RID: 534
		internal WebMethodAttribute attribute;
	}
}
