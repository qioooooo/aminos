using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000366 RID: 870
	[ComVisible(true)]
	[Serializable]
	public struct StreamingContext
	{
		// Token: 0x060022A3 RID: 8867 RVA: 0x000580D2 File Offset: 0x000570D2
		public StreamingContext(StreamingContextStates state)
		{
			this = new StreamingContext(state, null);
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x000580DC File Offset: 0x000570DC
		public StreamingContext(StreamingContextStates state, object additional)
		{
			this.m_state = state;
			this.m_additionalContext = additional;
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060022A5 RID: 8869 RVA: 0x000580EC File Offset: 0x000570EC
		public object Context
		{
			get
			{
				return this.m_additionalContext;
			}
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000580F4 File Offset: 0x000570F4
		public override bool Equals(object obj)
		{
			return obj is StreamingContext && (((StreamingContext)obj).m_additionalContext == this.m_additionalContext && ((StreamingContext)obj).m_state == this.m_state);
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x00058129 File Offset: 0x00057129
		public override int GetHashCode()
		{
			return (int)this.m_state;
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x00058131 File Offset: 0x00057131
		public StreamingContextStates State
		{
			get
			{
				return this.m_state;
			}
		}

		// Token: 0x04000E5E RID: 3678
		internal object m_additionalContext;

		// Token: 0x04000E5F RID: 3679
		internal StreamingContextStates m_state;
	}
}
