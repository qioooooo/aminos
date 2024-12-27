using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000068 RID: 104
	public abstract class SoapExtensionAttribute : Attribute
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002BB RID: 699
		public abstract Type ExtensionType { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002BC RID: 700
		// (set) Token: 0x060002BD RID: 701
		public abstract int Priority { get; set; }
	}
}
