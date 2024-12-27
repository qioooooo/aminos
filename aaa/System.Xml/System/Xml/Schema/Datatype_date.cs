using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C2 RID: 450
	internal class Datatype_date : Datatype_dateTimeBase
	{
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x00063522 File Offset: 0x00062522
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Date;
			}
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00063526 File Offset: 0x00062526
		internal Datatype_date()
			: base(XsdDateTimeFlags.Date)
		{
		}
	}
}
