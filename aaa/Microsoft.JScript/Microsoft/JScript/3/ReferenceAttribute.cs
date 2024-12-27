using System;

namespace Microsoft.JScript
{
	// Token: 0x02000103 RID: 259
	[AttributeUsage(AttributeTargets.Assembly)]
	public class ReferenceAttribute : Attribute
	{
		// Token: 0x06000AFE RID: 2814 RVA: 0x00054801 File Offset: 0x00053801
		public ReferenceAttribute(string reference)
		{
			this.reference = reference;
		}

		// Token: 0x040006AF RID: 1711
		public string reference;
	}
}
