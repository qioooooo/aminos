using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000AB RID: 171
	[Serializable]
	internal sealed class Empty : ISerializable
	{
		// Token: 0x06000A52 RID: 2642 RVA: 0x0001F925 File Offset: 0x0001E925
		private Empty()
		{
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0001F92D File Offset: 0x0001E92D
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0001F934 File Offset: 0x0001E934
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, 1, null, null);
		}

		// Token: 0x040003A1 RID: 929
		public static readonly Empty Value = new Empty();
	}
}
