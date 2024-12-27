using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C5 RID: 453
	internal class Datatype_monthDay : Datatype_dateTimeBase
	{
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x0006354A File Offset: 0x0006254A
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.GMonthDay;
			}
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x0006354E File Offset: 0x0006254E
		internal Datatype_monthDay()
			: base(XsdDateTimeFlags.GMonthDay)
		{
		}
	}
}
