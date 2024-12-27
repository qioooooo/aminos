using System;
using System.Reflection;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006A6 RID: 1702
	internal static class AsyncMessageHelper
	{
		// Token: 0x06003DC0 RID: 15808 RVA: 0x000D3F5C File Offset: 0x000D2F5C
		internal static void GetOutArgs(ParameterInfo[] syncParams, object[] syncArgs, object[] endArgs)
		{
			int num = 0;
			for (int i = 0; i < syncParams.Length; i++)
			{
				if (syncParams[i].IsOut || syncParams[i].ParameterType.IsByRef)
				{
					endArgs[num++] = syncArgs[i];
				}
			}
		}
	}
}
