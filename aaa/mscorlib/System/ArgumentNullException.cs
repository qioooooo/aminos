using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200006E RID: 110
	[ComVisible(true)]
	[Serializable]
	public class ArgumentNullException : ArgumentException
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x00015A52 File Offset: 0x00014A52
		public ArgumentNullException()
			: base(Environment.GetResourceString("ArgumentNull_Generic"))
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00015A6F File Offset: 0x00014A6F
		public ArgumentNullException(string paramName)
			: base(Environment.GetResourceString("ArgumentNull_Generic"), paramName)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00015A8D File Offset: 0x00014A8D
		public ArgumentNullException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00015AA2 File Offset: 0x00014AA2
		public ArgumentNullException(string paramName, string message)
			: base(message, paramName)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00015AB7 File Offset: 0x00014AB7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected ArgumentNullException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
