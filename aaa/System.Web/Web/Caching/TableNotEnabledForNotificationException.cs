using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web.Caching
{
	// Token: 0x02000111 RID: 273
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class TableNotEnabledForNotificationException : SystemException
	{
		// Token: 0x06000C9F RID: 3231 RVA: 0x00032355 File Offset: 0x00031355
		public TableNotEnabledForNotificationException()
		{
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0003235D File Offset: 0x0003135D
		public TableNotEnabledForNotificationException(string message)
			: base(message)
		{
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00032366 File Offset: 0x00031366
		public TableNotEnabledForNotificationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00032370 File Offset: 0x00031370
		internal TableNotEnabledForNotificationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
