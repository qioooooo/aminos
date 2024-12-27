using System;
using System.Security.Permissions;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000021 RID: 33
	public abstract class MimeFormatter
	{
		// Token: 0x06000075 RID: 117
		public abstract object GetInitializer(LogicalMethodInfo methodInfo);

		// Token: 0x06000076 RID: 118
		public abstract void Initialize(object initializer);

		// Token: 0x06000077 RID: 119 RVA: 0x00002E9C File Offset: 0x00001E9C
		public virtual object[] GetInitializers(LogicalMethodInfo[] methodInfos)
		{
			object[] array = new object[methodInfos.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.GetInitializer(methodInfos[i]);
			}
			return array;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002ECD File Offset: 0x00001ECD
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static object GetInitializer(Type type, LogicalMethodInfo methodInfo)
		{
			return ((MimeFormatter)Activator.CreateInstance(type)).GetInitializer(methodInfo);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002EE0 File Offset: 0x00001EE0
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static object[] GetInitializers(Type type, LogicalMethodInfo[] methodInfos)
		{
			return ((MimeFormatter)Activator.CreateInstance(type)).GetInitializers(methodInfos);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002EF4 File Offset: 0x00001EF4
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static MimeFormatter CreateInstance(Type type, object initializer)
		{
			MimeFormatter mimeFormatter = (MimeFormatter)Activator.CreateInstance(type);
			mimeFormatter.Initialize(initializer);
			return mimeFormatter;
		}
	}
}
