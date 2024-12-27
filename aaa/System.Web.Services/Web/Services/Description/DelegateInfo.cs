using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000DB RID: 219
	internal class DelegateInfo
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x0001C2DC File Offset: 0x0001B2DC
		internal DelegateInfo(string handlerType, string handlerArgs)
		{
			this.handlerType = handlerType;
			this.handlerArgs = handlerArgs;
		}

		// Token: 0x04000432 RID: 1074
		internal string handlerType;

		// Token: 0x04000433 RID: 1075
		internal string handlerArgs;
	}
}
