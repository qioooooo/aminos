using System;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000500 RID: 1280
	internal class ImporterCallback : ITypeLibImporterNotifySink
	{
		// Token: 0x0600325A RID: 12890 RVA: 0x000AB008 File Offset: 0x000AA008
		public void ReportEvent(ImporterEventKind EventKind, int EventCode, string EventMsg)
		{
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x000AB00C File Offset: 0x000AA00C
		public Assembly ResolveRef(object TypeLib)
		{
			Assembly assembly;
			try
			{
				ITypeLibConverter typeLibConverter = new TypeLibConverter();
				assembly = typeLibConverter.ConvertTypeLibToAssembly(TypeLib, Marshal.GetTypeLibName((ITypeLib)TypeLib) + ".dll", TypeLibImporterFlags.None, new ImporterCallback(), null, null, null, null);
			}
			catch (Exception)
			{
				assembly = null;
			}
			return assembly;
		}
	}
}
