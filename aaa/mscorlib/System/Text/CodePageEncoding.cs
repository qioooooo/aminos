using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text
{
	// Token: 0x020003E1 RID: 993
	[Serializable]
	internal sealed class CodePageEncoding : ISerializable, IObjectReference
	{
		// Token: 0x06002988 RID: 10632 RVA: 0x00082500 File Offset: 0x00081500
		internal CodePageEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_codePage = (int)info.GetValue("m_codePage", typeof(int));
			try
			{
				this.m_isReadOnly = (bool)info.GetValue("m_isReadOnly", typeof(bool));
				this.encoderFallback = (EncoderFallback)info.GetValue("encoderFallback", typeof(EncoderFallback));
				this.decoderFallback = (DecoderFallback)info.GetValue("decoderFallback", typeof(DecoderFallback));
			}
			catch (SerializationException)
			{
				this.m_deserializedFromEverett = true;
				this.m_isReadOnly = true;
			}
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000825C4 File Offset: 0x000815C4
		public object GetRealObject(StreamingContext context)
		{
			this.realEncoding = Encoding.GetEncoding(this.m_codePage);
			if (!this.m_deserializedFromEverett && !this.m_isReadOnly)
			{
				this.realEncoding = (Encoding)this.realEncoding.Clone();
				this.realEncoding.EncoderFallback = this.encoderFallback;
				this.realEncoding.DecoderFallback = this.decoderFallback;
			}
			return this.realEncoding;
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x00082630 File Offset: 0x00081630
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
		}

		// Token: 0x0400141F RID: 5151
		[NonSerialized]
		private int m_codePage;

		// Token: 0x04001420 RID: 5152
		[NonSerialized]
		private bool m_isReadOnly;

		// Token: 0x04001421 RID: 5153
		[NonSerialized]
		private bool m_deserializedFromEverett;

		// Token: 0x04001422 RID: 5154
		[NonSerialized]
		private EncoderFallback encoderFallback;

		// Token: 0x04001423 RID: 5155
		[NonSerialized]
		private DecoderFallback decoderFallback;

		// Token: 0x04001424 RID: 5156
		[NonSerialized]
		private Encoding realEncoding;

		// Token: 0x020003E2 RID: 994
		[Serializable]
		internal sealed class Decoder : ISerializable, IObjectReference
		{
			// Token: 0x0600298B RID: 10635 RVA: 0x00082641 File Offset: 0x00081641
			internal Decoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.realEncoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
			}

			// Token: 0x0600298C RID: 10636 RVA: 0x00082677 File Offset: 0x00081677
			public object GetRealObject(StreamingContext context)
			{
				return this.realEncoding.GetDecoder();
			}

			// Token: 0x0600298D RID: 10637 RVA: 0x00082684 File Offset: 0x00081684
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
			}

			// Token: 0x04001425 RID: 5157
			[NonSerialized]
			private Encoding realEncoding;
		}
	}
}
