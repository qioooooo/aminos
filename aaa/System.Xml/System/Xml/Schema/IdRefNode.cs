using System;

namespace System.Xml.Schema
{
	// Token: 0x02000284 RID: 644
	internal class IdRefNode
	{
		// Token: 0x06001D78 RID: 7544 RVA: 0x000860BA File Offset: 0x000850BA
		internal IdRefNode(IdRefNode next, string id, int lineNo, int linePos)
		{
			this.Id = id;
			this.LineNo = lineNo;
			this.LinePos = linePos;
			this.Next = next;
		}

		// Token: 0x040011FF RID: 4607
		internal string Id;

		// Token: 0x04001200 RID: 4608
		internal int LineNo;

		// Token: 0x04001201 RID: 4609
		internal int LinePos;

		// Token: 0x04001202 RID: 4610
		internal IdRefNode Next;
	}
}
