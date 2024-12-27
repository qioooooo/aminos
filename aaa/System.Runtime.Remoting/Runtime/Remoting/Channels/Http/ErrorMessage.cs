using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x0200002E RID: 46
	internal class ErrorMessage : IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00008281 File Offset: 0x00007281
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00008284 File Offset: 0x00007284
		public string Uri
		{
			get
			{
				return this.m_URI;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000828C File Offset: 0x0000728C
		public string MethodName
		{
			get
			{
				return this.m_MethodName;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00008294 File Offset: 0x00007294
		public string TypeName
		{
			get
			{
				return this.m_TypeName;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000829C File Offset: 0x0000729C
		public object MethodSignature
		{
			get
			{
				return this.m_MethodSignature;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000082A4 File Offset: 0x000072A4
		public MethodBase MethodBase
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000082A7 File Offset: 0x000072A7
		public int ArgCount
		{
			get
			{
				return this.m_ArgCount;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000082AF File Offset: 0x000072AF
		public string GetArgName(int index)
		{
			return this.m_ArgName;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000082B7 File Offset: 0x000072B7
		public object GetArg(int argNum)
		{
			return null;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000184 RID: 388 RVA: 0x000082BA File Offset: 0x000072BA
		public object[] Args
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000185 RID: 389 RVA: 0x000082BD File Offset: 0x000072BD
		public bool HasVarArgs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000082C0 File Offset: 0x000072C0
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000187 RID: 391 RVA: 0x000082C3 File Offset: 0x000072C3
		public int InArgCount
		{
			get
			{
				return this.m_ArgCount;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000082CB File Offset: 0x000072CB
		public string GetInArgName(int index)
		{
			return null;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000082CE File Offset: 0x000072CE
		public object GetInArg(int argNum)
		{
			return null;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000082D1 File Offset: 0x000072D1
		public object[] InArgs
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400012E RID: 302
		private string m_URI = "Exception";

		// Token: 0x0400012F RID: 303
		private string m_MethodName = "Unknown";

		// Token: 0x04000130 RID: 304
		private string m_TypeName = "Unknown";

		// Token: 0x04000131 RID: 305
		private object m_MethodSignature;

		// Token: 0x04000132 RID: 306
		private int m_ArgCount;

		// Token: 0x04000133 RID: 307
		private string m_ArgName = "Unknown";
	}
}
