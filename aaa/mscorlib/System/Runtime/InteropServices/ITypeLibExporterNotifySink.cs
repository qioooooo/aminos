using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000515 RID: 1301
	[ComVisible(true)]
	[Guid("F1C3BF77-C3E4-11d3-88E7-00902754C43A")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ITypeLibExporterNotifySink
	{
		// Token: 0x060032A0 RID: 12960
		void ReportEvent(ExporterEventKind eventKind, int eventCode, string eventMsg);

		// Token: 0x060032A1 RID: 12961
		[return: MarshalAs(UnmanagedType.Interface)]
		object ResolveRef(Assembly assembly);
	}
}
