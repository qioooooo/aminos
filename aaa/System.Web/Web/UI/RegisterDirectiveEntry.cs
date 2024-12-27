using System;

namespace System.Web.UI
{
	// Token: 0x02000389 RID: 905
	internal abstract class RegisterDirectiveEntry : SourceLineInfo
	{
		// Token: 0x06002C3F RID: 11327 RVA: 0x000C5CAA File Offset: 0x000C4CAA
		internal RegisterDirectiveEntry(string tagPrefix)
		{
			this._tagPrefix = tagPrefix;
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06002C40 RID: 11328 RVA: 0x000C5CB9 File Offset: 0x000C4CB9
		internal string TagPrefix
		{
			get
			{
				return this._tagPrefix;
			}
		}

		// Token: 0x04002085 RID: 8325
		private string _tagPrefix;
	}
}
