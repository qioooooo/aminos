using System;

namespace System.Management
{
	// Token: 0x02000038 RID: 56
	public class ObjectQuery : ManagementQuery
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x00009EE8 File Offset: 0x00008EE8
		public ObjectQuery()
		{
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009EF0 File Offset: 0x00008EF0
		public ObjectQuery(string query)
			: base(query)
		{
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009EF9 File Offset: 0x00008EF9
		public ObjectQuery(string language, string query)
			: base(language, query)
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009F03 File Offset: 0x00008F03
		public override object Clone()
		{
			return new ObjectQuery(this.QueryLanguage, this.QueryString);
		}
	}
}
