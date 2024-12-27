using System;

namespace System.Xml.Schema
{
	// Token: 0x020001C1 RID: 449
	internal class Datatype_time : Datatype_dateTimeBase
	{
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x0600169C RID: 5788 RVA: 0x00063515 File Offset: 0x00062515
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Time;
			}
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00063519 File Offset: 0x00062519
		internal Datatype_time()
			: base(XsdDateTimeFlags.Time)
		{
		}
	}
}
