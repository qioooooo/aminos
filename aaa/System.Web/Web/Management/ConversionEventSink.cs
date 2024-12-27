using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Web.Management
{
	// Token: 0x020002CB RID: 715
	internal class ConversionEventSink : ITypeLibExporterNotifySink
	{
		// Token: 0x060024B2 RID: 9394 RVA: 0x0009CE38 File Offset: 0x0009BE38
		public void ReportEvent(ExporterEventKind eventKind, int eventCode, string eventMsg)
		{
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x0009CE3A File Offset: 0x0009BE3A
		public object ResolveRef(Assembly assemblyReference)
		{
			return null;
		}
	}
}
