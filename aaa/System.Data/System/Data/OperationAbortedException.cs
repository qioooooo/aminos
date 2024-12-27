using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x020000CB RID: 203
	[Serializable]
	public sealed class OperationAbortedException : SystemException
	{
		// Token: 0x06000CD5 RID: 3285 RVA: 0x001FC1FC File Offset: 0x001FB5FC
		private OperationAbortedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232010;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x001FC21C File Offset: 0x001FB61C
		private OperationAbortedException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x001FC234 File Offset: 0x001FB634
		internal static OperationAbortedException Aborted(Exception inner)
		{
			OperationAbortedException ex;
			if (inner == null)
			{
				ex = new OperationAbortedException(Res.GetString("ADP_OperationAborted"), null);
			}
			else
			{
				ex = new OperationAbortedException(Res.GetString("ADP_OperationAbortedExceptionMessage"), inner);
			}
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}
	}
}
