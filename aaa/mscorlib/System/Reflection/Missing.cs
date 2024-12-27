using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000319 RID: 793
	[ComVisible(true)]
	[Serializable]
	public sealed class Missing : ISerializable
	{
		// Token: 0x06001EBE RID: 7870 RVA: 0x0004DF63 File Offset: 0x0004CF63
		private Missing()
		{
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0004DF6B File Offset: 0x0004CF6B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, this);
		}

		// Token: 0x04000D1B RID: 3355
		public static readonly Missing Value = new Missing();
	}
}
