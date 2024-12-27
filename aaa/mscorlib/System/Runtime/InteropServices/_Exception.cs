using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200002F RID: 47
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("b36b5c63-42ef-38bc-a07e-0b34c98f164a")]
	[ComVisible(true)]
	public interface _Exception
	{
		// Token: 0x06000269 RID: 617
		string ToString();

		// Token: 0x0600026A RID: 618
		bool Equals(object obj);

		// Token: 0x0600026B RID: 619
		int GetHashCode();

		// Token: 0x0600026C RID: 620
		Type GetType();

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600026D RID: 621
		string Message { get; }

		// Token: 0x0600026E RID: 622
		Exception GetBaseException();

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600026F RID: 623
		string StackTrace { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000270 RID: 624
		// (set) Token: 0x06000271 RID: 625
		string HelpLink { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000272 RID: 626
		// (set) Token: 0x06000273 RID: 627
		string Source { get; set; }

		// Token: 0x06000274 RID: 628
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void GetObjectData(SerializationInfo info, StreamingContext context);

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000275 RID: 629
		Exception InnerException { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000276 RID: 630
		MethodBase TargetSite { get; }
	}
}
