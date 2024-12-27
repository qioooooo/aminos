using System;
using System.Collections;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000079 RID: 121
	internal class SoapHeaderAttributeComparer : IComparer
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0000E697 File Offset: 0x0000D697
		public int Compare(object x, object y)
		{
			return string.Compare(((SoapHeaderAttribute)x).MemberName, ((SoapHeaderAttribute)y).MemberName, StringComparison.Ordinal);
		}
	}
}
