using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web.Caching
{
	// Token: 0x02000110 RID: 272
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class DatabaseNotEnabledForNotificationException : SystemException
	{
		// Token: 0x06000C9B RID: 3227 RVA: 0x00032330 File Offset: 0x00031330
		public DatabaseNotEnabledForNotificationException()
		{
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00032338 File Offset: 0x00031338
		public DatabaseNotEnabledForNotificationException(string message)
			: base(message)
		{
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00032341 File Offset: 0x00031341
		public DatabaseNotEnabledForNotificationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0003234B File Offset: 0x0003134B
		internal DatabaseNotEnabledForNotificationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
