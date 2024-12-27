using System;

namespace System.Management
{
	// Token: 0x0200003A RID: 58
	public class WqlObjectQuery : ObjectQuery
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00009F44 File Offset: 0x00008F44
		public WqlObjectQuery()
			: base(null)
		{
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009F4D File Offset: 0x00008F4D
		public WqlObjectQuery(string query)
			: base(query)
		{
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00009F56 File Offset: 0x00008F56
		public override string QueryLanguage
		{
			get
			{
				return base.QueryLanguage;
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009F5E File Offset: 0x00008F5E
		public override object Clone()
		{
			return new WqlObjectQuery(this.QueryString);
		}
	}
}
