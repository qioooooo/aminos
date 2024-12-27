using System;
using System.Collections;
using System.Reflection;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200070E RID: 1806
	internal class ErrorMessage : IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004138 RID: 16696 RVA: 0x000DF3B9 File Offset: 0x000DE3B9
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004139 RID: 16697 RVA: 0x000DF3BC File Offset: 0x000DE3BC
		public string Uri
		{
			get
			{
				return this.m_URI;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x0600413A RID: 16698 RVA: 0x000DF3C4 File Offset: 0x000DE3C4
		public string MethodName
		{
			get
			{
				return this.m_MethodName;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600413B RID: 16699 RVA: 0x000DF3CC File Offset: 0x000DE3CC
		public string TypeName
		{
			get
			{
				return this.m_TypeName;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600413C RID: 16700 RVA: 0x000DF3D4 File Offset: 0x000DE3D4
		public object MethodSignature
		{
			get
			{
				return this.m_MethodSignature;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x0600413D RID: 16701 RVA: 0x000DF3DC File Offset: 0x000DE3DC
		public MethodBase MethodBase
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600413E RID: 16702 RVA: 0x000DF3DF File Offset: 0x000DE3DF
		public int ArgCount
		{
			get
			{
				return this.m_ArgCount;
			}
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x000DF3E7 File Offset: 0x000DE3E7
		public string GetArgName(int index)
		{
			return this.m_ArgName;
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x000DF3EF File Offset: 0x000DE3EF
		public object GetArg(int argNum)
		{
			return null;
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06004141 RID: 16705 RVA: 0x000DF3F2 File Offset: 0x000DE3F2
		public object[] Args
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06004142 RID: 16706 RVA: 0x000DF3F5 File Offset: 0x000DE3F5
		public bool HasVarArgs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004143 RID: 16707 RVA: 0x000DF3F8 File Offset: 0x000DE3F8
		public int InArgCount
		{
			get
			{
				return this.m_ArgCount;
			}
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x000DF400 File Offset: 0x000DE400
		public string GetInArgName(int index)
		{
			return null;
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x000DF403 File Offset: 0x000DE403
		public object GetInArg(int argNum)
		{
			return null;
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004146 RID: 16710 RVA: 0x000DF406 File Offset: 0x000DE406
		public object[] InArgs
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004147 RID: 16711 RVA: 0x000DF409 File Offset: 0x000DE409
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400209F RID: 8351
		private string m_URI = "Exception";

		// Token: 0x040020A0 RID: 8352
		private string m_MethodName = "Unknown";

		// Token: 0x040020A1 RID: 8353
		private string m_TypeName = "Unknown";

		// Token: 0x040020A2 RID: 8354
		private object m_MethodSignature;

		// Token: 0x040020A3 RID: 8355
		private int m_ArgCount;

		// Token: 0x040020A4 RID: 8356
		private string m_ArgName = "Unknown";
	}
}
