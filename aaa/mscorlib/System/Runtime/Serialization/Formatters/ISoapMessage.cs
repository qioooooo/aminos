using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007A1 RID: 1953
	[ComVisible(true)]
	public interface ISoapMessage
	{
		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06004615 RID: 17941
		// (set) Token: 0x06004616 RID: 17942
		string[] ParamNames { get; set; }

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06004617 RID: 17943
		// (set) Token: 0x06004618 RID: 17944
		object[] ParamValues { get; set; }

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06004619 RID: 17945
		// (set) Token: 0x0600461A RID: 17946
		Type[] ParamTypes { get; set; }

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x0600461B RID: 17947
		// (set) Token: 0x0600461C RID: 17948
		string MethodName { get; set; }

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x0600461D RID: 17949
		// (set) Token: 0x0600461E RID: 17950
		string XmlNameSpace { get; set; }

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x0600461F RID: 17951
		// (set) Token: 0x06004620 RID: 17952
		Header[] Headers { get; set; }
	}
}
