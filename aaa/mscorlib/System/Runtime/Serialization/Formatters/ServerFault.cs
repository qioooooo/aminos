using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007A7 RID: 1959
	[SoapType(Embedded = true)]
	[ComVisible(true)]
	[Serializable]
	public sealed class ServerFault
	{
		// Token: 0x06004646 RID: 17990 RVA: 0x000F06A5 File Offset: 0x000EF6A5
		internal ServerFault(Exception exception)
		{
			this.exception = exception;
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x000F06B4 File Offset: 0x000EF6B4
		public ServerFault(string exceptionType, string message, string stackTrace)
		{
			this.exceptionType = exceptionType;
			this.message = message;
			this.stackTrace = stackTrace;
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004648 RID: 17992 RVA: 0x000F06D1 File Offset: 0x000EF6D1
		// (set) Token: 0x06004649 RID: 17993 RVA: 0x000F06D9 File Offset: 0x000EF6D9
		public string ExceptionType
		{
			get
			{
				return this.exceptionType;
			}
			set
			{
				this.exceptionType = value;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x0600464A RID: 17994 RVA: 0x000F06E2 File Offset: 0x000EF6E2
		// (set) Token: 0x0600464B RID: 17995 RVA: 0x000F06EA File Offset: 0x000EF6EA
		public string ExceptionMessage
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x0600464C RID: 17996 RVA: 0x000F06F3 File Offset: 0x000EF6F3
		// (set) Token: 0x0600464D RID: 17997 RVA: 0x000F06FB File Offset: 0x000EF6FB
		public string StackTrace
		{
			get
			{
				return this.stackTrace;
			}
			set
			{
				this.stackTrace = value;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x000F0704 File Offset: 0x000EF704
		internal Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x040022C8 RID: 8904
		private string exceptionType;

		// Token: 0x040022C9 RID: 8905
		private string message;

		// Token: 0x040022CA RID: 8906
		private string stackTrace;

		// Token: 0x040022CB RID: 8907
		private Exception exception;
	}
}
