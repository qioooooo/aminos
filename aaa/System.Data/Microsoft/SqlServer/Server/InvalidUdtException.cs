using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200027F RID: 639
	[Serializable]
	public sealed class InvalidUdtException : SystemException
	{
		// Token: 0x0600219C RID: 8604 RVA: 0x00269694 File Offset: 0x00268A94
		internal InvalidUdtException()
		{
			base.HResult = -2146232009;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x002696B4 File Offset: 0x00268AB4
		internal InvalidUdtException(string message)
			: base(message)
		{
			base.HResult = -2146232009;
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x002696D4 File Offset: 0x00268AD4
		internal InvalidUdtException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232009;
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x002696F4 File Offset: 0x00268AF4
		private InvalidUdtException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x0026970C File Offset: 0x00268B0C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			base.GetObjectData(si, context);
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00269724 File Offset: 0x00268B24
		internal static InvalidUdtException Create(Type udtType, string resourceReason)
		{
			string @string = Res.GetString(resourceReason);
			string string2 = Res.GetString("SqlUdt_InvalidUdtMessage", new object[] { udtType.FullName, @string });
			InvalidUdtException ex = new InvalidUdtException(string2);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}
	}
}
