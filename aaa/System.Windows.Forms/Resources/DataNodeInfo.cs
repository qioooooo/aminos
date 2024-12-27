using System;
using System.Drawing;

namespace System.Resources
{
	// Token: 0x02000142 RID: 322
	internal class DataNodeInfo
	{
		// Token: 0x060004E1 RID: 1249 RVA: 0x0000C39C File Offset: 0x0000B39C
		internal DataNodeInfo Clone()
		{
			return new DataNodeInfo
			{
				Name = this.Name,
				Comment = this.Comment,
				TypeName = this.TypeName,
				MimeType = this.MimeType,
				ValueData = this.ValueData,
				ReaderPosition = new Point(this.ReaderPosition.X, this.ReaderPosition.Y)
			};
		}

		// Token: 0x04000EEC RID: 3820
		internal string Name;

		// Token: 0x04000EED RID: 3821
		internal string Comment;

		// Token: 0x04000EEE RID: 3822
		internal string TypeName;

		// Token: 0x04000EEF RID: 3823
		internal string MimeType;

		// Token: 0x04000EF0 RID: 3824
		internal string ValueData;

		// Token: 0x04000EF1 RID: 3825
		internal Point ReaderPosition;
	}
}
