using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace <CrtImplementationDetails>
{
	// Token: 0x02000016 RID: 22
	internal class ModuleUninitializer : Stack
	{
		// Token: 0x06000094 RID: 148 RVA: 0x001C5680 File Offset: 0x001C4A80
		internal void AddHandler(EventHandler handler)
		{
			RuntimeHelpers.PrepareDelegate(handler);
			this.Push(handler);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x001C5B54 File Offset: 0x001C4F54
		private ModuleUninitializer()
		{
			EventHandler eventHandler = new EventHandler(this.SingletonDomainUnload);
			AppDomain.CurrentDomain.DomainUnload += eventHandler;
			AppDomain.CurrentDomain.ProcessExit += eventHandler;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x001C569C File Offset: 0x001C4A9C
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

		// Token: 0x0400006E RID: 110
		private static object @lock = new object();

		// Token: 0x0400006F RID: 111
		internal static ModuleUninitializer _ModuleUninitializer = new ModuleUninitializer();
	}
}
