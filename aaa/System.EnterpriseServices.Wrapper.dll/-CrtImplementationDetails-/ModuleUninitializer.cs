using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace <CrtImplementationDetails>
{
	// Token: 0x020000AE RID: 174
	internal class ModuleUninitializer : Stack
	{
		// Token: 0x0600011C RID: 284 RVA: 0x00006308 File Offset: 0x00005708
		internal void AddHandler(EventHandler handler)
		{
			RuntimeHelpers.PrepareDelegate(handler);
			this.Push(handler);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000068C0 File Offset: 0x00005CC0
		private ModuleUninitializer()
		{
			EventHandler eventHandler = new EventHandler(this.SingletonDomainUnload);
			AppDomain.CurrentDomain.DomainUnload += eventHandler;
			AppDomain.CurrentDomain.ProcessExit += eventHandler;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006324 File Offset: 0x00005724
		[PrePrepareMethod]
		private void SingletonDomainUnload(object source, EventArgs arguments)
		{
			using (IEnumerator enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					((EventHandler)enumerator.Current)(source, arguments);
				}
			}
		}

		// Token: 0x040000C0 RID: 192
		private static object @lock = new object();

		// Token: 0x040000C1 RID: 193
		internal static ModuleUninitializer _ModuleUninitializer = new ModuleUninitializer();
	}
}
