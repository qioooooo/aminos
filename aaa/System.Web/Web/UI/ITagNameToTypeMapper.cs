using System;
using System.Collections;

namespace System.Web.UI
{
	// Token: 0x02000401 RID: 1025
	internal interface ITagNameToTypeMapper
	{
		// Token: 0x06003294 RID: 12948
		Type GetControlType(string tagName, IDictionary attribs);
	}
}
