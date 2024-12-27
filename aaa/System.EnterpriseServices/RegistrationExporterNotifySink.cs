using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.EnterpriseServices
{
	// Token: 0x02000083 RID: 131
	internal class RegistrationExporterNotifySink : ITypeLibExporterNotifySink
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x00007E3C File Offset: 0x00006E3C
		internal RegistrationExporterNotifySink(string tlb, Report report)
		{
			this._tlb = tlb;
			this._report = report;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00007E52 File Offset: 0x00006E52
		public void ReportEvent(ExporterEventKind EventKind, int EventCode, string EventMsg)
		{
			if (EventKind != ExporterEventKind.NOTIF_TYPECONVERTED && this._report != null)
			{
				this._report(EventMsg);
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00007E6C File Offset: 0x00006E6C
		public object ResolveRef(Assembly asm)
		{
			string directoryName = Path.GetDirectoryName(asm.Location);
			string text = Path.Combine(directoryName, asm.GetName().Name) + ".tlb";
			if (this._report != null)
			{
				this._report(Resource.FormatString("Reg_AutoExportMsg", asm.FullName, text));
			}
			return (ITypeLib)RegistrationDriver.GenerateTypeLibrary(asm, text, this._report);
		}

		// Token: 0x04000131 RID: 305
		private string _tlb;

		// Token: 0x04000132 RID: 306
		private Report _report;
	}
}
