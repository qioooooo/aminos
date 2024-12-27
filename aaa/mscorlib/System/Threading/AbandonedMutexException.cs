using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000129 RID: 297
	[ComVisible(false)]
	[Serializable]
	public class AbandonedMutexException : SystemException
	{
		// Token: 0x0600113F RID: 4415 RVA: 0x000311B0 File Offset: 0x000301B0
		public AbandonedMutexException()
			: base(Environment.GetResourceString("Threading.AbandonedMutexException"))
		{
			base.SetErrorCode(-2146233043);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x000311D4 File Offset: 0x000301D4
		public AbandonedMutexException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233043);
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x000311EF File Offset: 0x000301EF
		public AbandonedMutexException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233043);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0003120B File Offset: 0x0003020B
		public AbandonedMutexException(int location, WaitHandle handle)
			: base(Environment.GetResourceString("Threading.AbandonedMutexException"))
		{
			base.SetErrorCode(-2146233043);
			this.SetupException(location, handle);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00031237 File Offset: 0x00030237
		public AbandonedMutexException(string message, int location, WaitHandle handle)
			: base(message)
		{
			base.SetErrorCode(-2146233043);
			this.SetupException(location, handle);
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0003125A File Offset: 0x0003025A
		public AbandonedMutexException(string message, Exception inner, int location, WaitHandle handle)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233043);
			this.SetupException(location, handle);
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0003127F File Offset: 0x0003027F
		private void SetupException(int location, WaitHandle handle)
		{
			this.m_MutexIndex = location;
			if (handle != null)
			{
				this.m_Mutex = handle as Mutex;
			}
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00031297 File Offset: 0x00030297
		protected AbandonedMutexException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x000312A8 File Offset: 0x000302A8
		public Mutex Mutex
		{
			get
			{
				return this.m_Mutex;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x000312B0 File Offset: 0x000302B0
		public int MutexIndex
		{
			get
			{
				return this.m_MutexIndex;
			}
		}

		// Token: 0x040005C1 RID: 1473
		private int m_MutexIndex = -1;

		// Token: 0x040005C2 RID: 1474
		private Mutex m_Mutex;
	}
}
