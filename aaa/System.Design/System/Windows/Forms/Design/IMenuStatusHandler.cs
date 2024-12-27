using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000227 RID: 551
	internal interface IMenuStatusHandler
	{
		// Token: 0x060014F3 RID: 5363
		bool OverrideInvoke(MenuCommand cmd);

		// Token: 0x060014F4 RID: 5364
		bool OverrideStatus(MenuCommand cmd);
	}
}
