using System;

namespace System.Globalization
{
	// Token: 0x020003CC RID: 972
	internal struct HebrewNumberParsingContext
	{
		// Token: 0x060028A5 RID: 10405 RVA: 0x0007E2C5 File Offset: 0x0007D2C5
		public HebrewNumberParsingContext(int result)
		{
			this.state = HebrewNumber.HS.Start;
			this.result = 0;
		}

		// Token: 0x04001375 RID: 4981
		internal HebrewNumber.HS state;

		// Token: 0x04001376 RID: 4982
		internal int result;
	}
}
