using System;
using System.Diagnostics;

namespace System.Data
{
	// Token: 0x02000098 RID: 152
	internal sealed class DataSetTraceSource : TraceSource
	{
		// Token: 0x06000925 RID: 2341 RVA: 0x001E9488 File Offset: 0x001E8888
		private DataSetTraceSource()
			: base("System.Data.DataSet")
		{
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x001E94A0 File Offset: 0x001E88A0
		internal static void TraceTypeNotAllowed(Type type)
		{
			TraceEventType traceEventType = (SerializationConfig.IsAuditMode() ? TraceEventType.Warning : TraceEventType.Error);
			DataSetTraceSource dataSetTraceSource = DataSetTraceSource.s_singleton;
			if (dataSetTraceSource.Switch.ShouldTrace(traceEventType))
			{
				dataSetTraceSource.TraceEvent(traceEventType, 1, Res.GetString("Data_TypeNotAllowed", new object[] { type.AssemblyQualifiedName }));
			}
		}

		// Token: 0x040007B5 RID: 1973
		private const int DisallowedTypeSeenEventId = 1;

		// Token: 0x040007B6 RID: 1974
		private static readonly DataSetTraceSource s_singleton = new DataSetTraceSource();
	}
}
