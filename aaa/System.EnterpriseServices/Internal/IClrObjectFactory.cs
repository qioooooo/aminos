using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000DA RID: 218
	[Guid("ecabafd2-7f19-11d2-978e-0000f8757e2a")]
	public interface IClrObjectFactory
	{
		// Token: 0x06000508 RID: 1288
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object CreateFromAssembly(string assembly, string type, string mode);

		// Token: 0x06000509 RID: 1289
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object CreateFromVroot(string VrootUrl, string Mode);

		// Token: 0x0600050A RID: 1290
		[DispId(3)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object CreateFromWsdl(string WsdlUrl, string Mode);

		// Token: 0x0600050B RID: 1291
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object CreateFromMailbox(string Mailbox, string Mode);
	}
}
