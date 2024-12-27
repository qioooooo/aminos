using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C3 RID: 451
	internal class Datatype_yearMonth : Datatype_dateTimeBase
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060016A0 RID: 5792 RVA: 0x0006352F File Offset: 0x0006252F
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.GYearMonth;
			}
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00063533 File Offset: 0x00062533
		internal Datatype_yearMonth()
			: base(XsdDateTimeFlags.GYearMonth)
		{
		}
	}
}
