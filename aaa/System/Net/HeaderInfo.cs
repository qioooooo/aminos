using System;

namespace System.Net
{
	// Token: 0x020004E0 RID: 1248
	internal class HeaderInfo
	{
		// Token: 0x060026DC RID: 9948 RVA: 0x0009FD7A File Offset: 0x0009ED7A
		internal HeaderInfo(string name, bool requestRestricted, bool responseRestricted, bool multi, HeaderParser p)
		{
			this.HeaderName = name;
			this.IsRequestRestricted = requestRestricted;
			this.IsResponseRestricted = responseRestricted;
			this.Parser = p;
			this.AllowMultiValues = multi;
		}

		// Token: 0x04002659 RID: 9817
		internal readonly bool IsRequestRestricted;

		// Token: 0x0400265A RID: 9818
		internal readonly bool IsResponseRestricted;

		// Token: 0x0400265B RID: 9819
		internal readonly HeaderParser Parser;

		// Token: 0x0400265C RID: 9820
		internal readonly string HeaderName;

		// Token: 0x0400265D RID: 9821
		internal readonly bool AllowMultiValues;
	}
}
