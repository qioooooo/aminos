using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C4 RID: 452
	internal class Datatype_year : Datatype_dateTimeBase
	{
		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x0006353C File Offset: 0x0006253C
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.GYear;
			}
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00063540 File Offset: 0x00062540
		internal Datatype_year()
			: base(XsdDateTimeFlags.GYear)
		{
		}
	}
}
