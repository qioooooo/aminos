using System;
using System.ComponentModel;
using System.IO;

namespace System.Net
{
	// Token: 0x0200048B RID: 1163
	public class OpenReadCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023D1 RID: 9169 RVA: 0x0008D0C6 File Offset: 0x0008C0C6
		internal OpenReadCompletedEventArgs(Stream result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x0008D0D9 File Offset: 0x0008C0D9
		public Stream Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002459 RID: 9305
		private Stream m_Result;
	}
}
