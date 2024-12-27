using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200035F RID: 863
	[Serializable]
	public class UriFormatException : FormatException, ISerializable
	{
		// Token: 0x06001B8A RID: 7050 RVA: 0x00067682 File Offset: 0x00066682
		public UriFormatException()
		{
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0006768A File Offset: 0x0006668A
		public UriFormatException(string textString)
			: base(textString)
		{
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x00067693 File Offset: 0x00066693
		protected UriFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0006769D File Offset: 0x0006669D
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
