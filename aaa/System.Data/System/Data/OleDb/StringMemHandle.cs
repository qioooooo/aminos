using System;
using System.Data.ProviderBase;

namespace System.Data.OleDb
{
	// Token: 0x02000261 RID: 609
	internal sealed class StringMemHandle : DbBuffer
	{
		// Token: 0x060020D9 RID: 8409 RVA: 0x00264D7C File Offset: 0x0026417C
		internal StringMemHandle(string value)
			: base((value != null) ? checked(2 + 2 * value.Length) : 0)
		{
			if (value != null)
			{
				base.WriteCharArray(0, value.ToCharArray(), 0, value.Length);
			}
		}
	}
}
