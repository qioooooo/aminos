using System;

namespace System.Security.Policy
{
	// Token: 0x02000489 RID: 1161
	internal class CodeGroupPositionMarker
	{
		// Token: 0x06002EA7 RID: 11943 RVA: 0x0009EAE4 File Offset: 0x0009DAE4
		internal CodeGroupPositionMarker(int elementIndex, int groupIndex, SecurityElement element)
		{
			this.elementIndex = elementIndex;
			this.groupIndex = groupIndex;
			this.element = element;
		}

		// Token: 0x040017A1 RID: 6049
		internal int elementIndex;

		// Token: 0x040017A2 RID: 6050
		internal int groupIndex;

		// Token: 0x040017A3 RID: 6051
		internal SecurityElement element;
	}
}
