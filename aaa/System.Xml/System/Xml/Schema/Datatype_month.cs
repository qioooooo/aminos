using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C7 RID: 455
	internal class Datatype_month : Datatype_dateTimeBase
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060016A8 RID: 5800 RVA: 0x00063566 File Offset: 0x00062566
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.GMonth;
			}
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x0006356A File Offset: 0x0006256A
		internal Datatype_month()
			: base(XsdDateTimeFlags.GMonth)
		{
		}
	}
}
