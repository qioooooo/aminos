using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text
{
	// Token: 0x020003FC RID: 1020
	[Serializable]
	internal sealed class MLangCodePageEncoding : ISerializable, IObjectReference
	{
		// Token: 0x06002A58 RID: 10840 RVA: 0x0008787C File Offset: 0x0008687C
		internal MLangCodePageEncoding(SerializationInfo info, StreamingContext context)
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

		// Token: 0x06002A59 RID: 10841 RVA: 0x00087940 File Offset: 0x00086940
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

		// Token: 0x06002A5A RID: 10842 RVA: 0x000879AC File Offset: 0x000869AC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
		}

		// Token: 0x04001486 RID: 5254
		[NonSerialized]
		private int m_codePage;

		// Token: 0x04001487 RID: 5255
		[NonSerialized]
		private bool m_isReadOnly;

		// Token: 0x04001488 RID: 5256
		[NonSerialized]
		private bool m_deserializedFromEverett;

		// Token: 0x04001489 RID: 5257
		[NonSerialized]
		private EncoderFallback encoderFallback;

		// Token: 0x0400148A RID: 5258
		[NonSerialized]
		private DecoderFallback decoderFallback;

		// Token: 0x0400148B RID: 5259
		[NonSerialized]
		private Encoding realEncoding;

		// Token: 0x020003FD RID: 1021
		[Serializable]
		internal sealed class MLangEncoder : ISerializable, IObjectReference
		{
			// Token: 0x06002A5B RID: 10843 RVA: 0x000879BD File Offset: 0x000869BD
			internal MLangEncoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.realEncoding = (Encoding)info.GetValue("m_encoding", typeof(Encoding));
			}

			// Token: 0x06002A5C RID: 10844 RVA: 0x000879F3 File Offset: 0x000869F3
			public object GetRealObject(StreamingContext context)
			{
				return this.realEncoding.GetEncoder();
			}

			// Token: 0x06002A5D RID: 10845 RVA: 0x00087A00 File Offset: 0x00086A00
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
			}

			// Token: 0x0400148C RID: 5260
			[NonSerialized]
			private Encoding realEncoding;
		}

		// Token: 0x020003FE RID: 1022
		[Serializable]
		internal sealed class MLangDecoder : ISerializable, IObjectReference
		{
			// Token: 0x06002A5E RID: 10846 RVA: 0x00087A11 File Offset: 0x00086A11
			internal MLangDecoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.realEncoding = (Encoding)info.GetValue("m_encoding", typeof(Encoding));
			}

			// Token: 0x06002A5F RID: 10847 RVA: 0x00087A47 File Offset: 0x00086A47
			public object GetRealObject(StreamingContext context)
			{
				return this.realEncoding.GetDecoder();
			}

			// Token: 0x06002A60 RID: 10848 RVA: 0x00087A54 File Offset: 0x00086A54
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ExecutionEngineException"));
			}

			// Token: 0x0400148D RID: 5261
			[NonSerialized]
			private Encoding realEncoding;
		}
	}
}
