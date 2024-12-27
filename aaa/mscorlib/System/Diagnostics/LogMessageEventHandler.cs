using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020002AA RID: 682
	// (Invoke) Token: 0x06001AF2 RID: 6898
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	[Serializable]
	internal delegate void LogMessageEventHandler(LoggingLevels level, LogSwitch category, string message, StackTrace location);
}
