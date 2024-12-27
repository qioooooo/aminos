using System;

namespace System.Xml.Schema
{
	// Token: 0x020001BF RID: 447
	internal class Datatype_timeNoTimeZone : Datatype_dateTimeBase
	{
		// Token: 0x0600169A RID: 5786 RVA: 0x000634FF File Offset: 0x000624FF
		internal Datatype_timeNoTimeZone()
			: base(XsdDateTimeFlags.XdrTimeNoTz)
		{
		}
	}
}
