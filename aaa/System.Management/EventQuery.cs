using System;

namespace System.Management
{
	// Token: 0x02000039 RID: 57
	public class EventQuery : ManagementQuery
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x00009F16 File Offset: 0x00008F16
		public EventQuery()
		{
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009F1E File Offset: 0x00008F1E
		public EventQuery(string query)
			: base(query)
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009F27 File Offset: 0x00008F27
		public EventQuery(string language, string query)
			: base(language, query)
		{
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009F31 File Offset: 0x00008F31
		public override object Clone()
		{
			return new EventQuery(this.QueryLanguage, this.QueryString);
		}
	}
}
