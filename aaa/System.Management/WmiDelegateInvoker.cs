using System;

namespace System.Management
{
	// Token: 0x02000029 RID: 41
	internal class WmiDelegateInvoker
	{
		// Token: 0x0600014B RID: 331 RVA: 0x000082CD File Offset: 0x000072CD
		internal WmiDelegateInvoker(object sender)
		{
			this.sender = sender;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000082DC File Offset: 0x000072DC
		internal void FireEventToDelegates(MulticastDelegate md, ManagementEventArgs args)
		{
			try
			{
				if (md != null)
				{
					foreach (Delegate @delegate in md.GetInvocationList())
					{
						try
						{
							@delegate.DynamicInvoke(new object[] { this.sender, args });
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000123 RID: 291
		internal object sender;
	}
}
