using System;
using System.Diagnostics;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000CF RID: 207
	public class ComSoapPublishError
	{
		// Token: 0x060004C1 RID: 1217 RVA: 0x0000DF54 File Offset: 0x0000CF54
		public static void Report(string s)
		{
			try
			{
				new EventLog
				{
					Source = "COM+ SOAP Services"
				}.WriteEntry(s, EventLogEntryType.Warning);
			}
			catch
			{
			}
		}
	}
}
