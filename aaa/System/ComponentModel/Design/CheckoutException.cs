using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200015A RID: 346
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class CheckoutException : ExternalException
	{
		// Token: 0x06000B55 RID: 2901 RVA: 0x00028004 File Offset: 0x00027004
		public CheckoutException()
		{
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0002800C File Offset: 0x0002700C
		public CheckoutException(string message)
			: base(message)
		{
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00028015 File Offset: 0x00027015
		public CheckoutException(string message, int errorCode)
			: base(message, errorCode)
		{
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002801F File Offset: 0x0002701F
		protected CheckoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00028029 File Offset: 0x00027029
		public CheckoutException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x04000AA1 RID: 2721
		public static readonly CheckoutException Canceled = new CheckoutException(SR.GetString("CHECKOUTCanceled"), -2147467260);
	}
}
