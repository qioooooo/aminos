using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C6 RID: 454
	internal class Datatype_day : Datatype_dateTimeBase
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060016A6 RID: 5798 RVA: 0x00063558 File Offset: 0x00062558
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.GDay;
			}
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x0006355C File Offset: 0x0006255C
		internal Datatype_day()
			: base(XsdDateTimeFlags.GDay)
		{
		}
	}
}
