using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000691 RID: 1681
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public sealed class CallContext
	{
		// Token: 0x06003D30 RID: 15664 RVA: 0x000D2178 File Offset: 0x000D1178
		private CallContext()
		{
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x000D2180 File Offset: 0x000D1180
		internal static LogicalCallContext GetLogicalCallContext()
		{
			return Thread.CurrentThread.GetLogicalCallContext();
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x000D218C File Offset: 0x000D118C
		internal static LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			return Thread.CurrentThread.SetLogicalCallContext(callCtx);
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x000D2199 File Offset: 0x000D1199
		internal static LogicalCallContext SetLogicalCallContext(Thread currThread, LogicalCallContext callCtx)
		{
			return currThread.SetLogicalCallContext(callCtx);
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06003D34 RID: 15668 RVA: 0x000D21A2 File Offset: 0x000D11A2
		internal static CallContextSecurityData SecurityData
		{
			get
			{
				return Thread.CurrentThread.GetLogicalCallContext().SecurityData;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06003D35 RID: 15669 RVA: 0x000D21B3 File Offset: 0x000D11B3
		internal static CallContextRemotingData RemotingData
		{
			get
			{
				return Thread.CurrentThread.GetLogicalCallContext().RemotingData;
			}
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x000D21C4 File Offset: 0x000D11C4
		public static void FreeNamedDataSlot(string name)
		{
			Thread.CurrentThread.GetLogicalCallContext().FreeNamedDataSlot(name);
			Thread.CurrentThread.GetIllogicalCallContext().FreeNamedDataSlot(name);
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x000D21E8 File Offset: 0x000D11E8
		public static object LogicalGetData(string name)
		{
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			return logicalCallContext.GetData(name);
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x000D2208 File Offset: 0x000D1208
		private static object IllogicalGetData(string name)
		{
			IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
			return illogicalCallContext.GetData(name);
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06003D39 RID: 15673 RVA: 0x000D2228 File Offset: 0x000D1228
		// (set) Token: 0x06003D3A RID: 15674 RVA: 0x000D2244 File Offset: 0x000D1244
		internal static IPrincipal Principal
		{
			get
			{
				LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
				return logicalCallContext.Principal;
			}
			set
			{
				LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
				logicalCallContext.Principal = value;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06003D3B RID: 15675 RVA: 0x000D2260 File Offset: 0x000D1260
		// (set) Token: 0x06003D3C RID: 15676 RVA: 0x000D2290 File Offset: 0x000D1290
		public static object HostContext
		{
			get
			{
				IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
				object obj = illogicalCallContext.HostContext;
				if (obj == null)
				{
					LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
					obj = logicalCallContext.HostContext;
				}
				return obj;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set
			{
				if (value is ILogicalThreadAffinative)
				{
					IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
					illogicalCallContext.HostContext = null;
					LogicalCallContext logicalCallContext = CallContext.GetLogicalCallContext();
					logicalCallContext.HostContext = value;
					return;
				}
				LogicalCallContext logicalCallContext2 = CallContext.GetLogicalCallContext();
				logicalCallContext2.HostContext = null;
				IllogicalCallContext illogicalCallContext2 = Thread.CurrentThread.GetIllogicalCallContext();
				illogicalCallContext2.HostContext = value;
			}
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x000D22E4 File Offset: 0x000D12E4
		public static object GetData(string name)
		{
			object obj = CallContext.LogicalGetData(name);
			if (obj == null)
			{
				return CallContext.IllogicalGetData(name);
			}
			return obj;
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x000D2304 File Offset: 0x000D1304
		public static void SetData(string name, object data)
		{
			if (data is ILogicalThreadAffinative)
			{
				CallContext.LogicalSetData(name, data);
				return;
			}
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			logicalCallContext.FreeNamedDataSlot(name);
			IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
			illogicalCallContext.SetData(name, data);
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x000D2348 File Offset: 0x000D1348
		public static void LogicalSetData(string name, object data)
		{
			IllogicalCallContext illogicalCallContext = Thread.CurrentThread.GetIllogicalCallContext();
			illogicalCallContext.FreeNamedDataSlot(name);
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			logicalCallContext.SetData(name, data);
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x000D237C File Offset: 0x000D137C
		public static Header[] GetHeaders()
		{
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			return logicalCallContext.InternalGetHeaders();
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x000D239C File Offset: 0x000D139C
		public static void SetHeaders(Header[] headers)
		{
			LogicalCallContext logicalCallContext = Thread.CurrentThread.GetLogicalCallContext();
			logicalCallContext.InternalSetHeaders(headers);
		}
	}
}
