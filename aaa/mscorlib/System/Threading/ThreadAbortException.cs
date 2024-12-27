using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000157 RID: 343
	[ComVisible(true)]
	[Serializable]
	public sealed class ThreadAbortException : SystemException
	{
		// Token: 0x0600134F RID: 4943 RVA: 0x000350F6 File Offset: 0x000340F6
		private ThreadAbortException()
			: base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.ThreadAbort))
		{
			base.SetErrorCode(-2146233040);
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0003510F File Offset: 0x0003410F
		internal ThreadAbortException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x00035119 File Offset: 0x00034119
		public object ExceptionState
		{
			get
			{
				return Thread.CurrentThread.AbortReason;
			}
		}
	}
}
