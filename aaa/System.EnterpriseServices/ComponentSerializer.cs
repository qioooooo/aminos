using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.EnterpriseServices
{
	// Token: 0x02000036 RID: 54
	internal sealed class ComponentSerializer
	{
		// Token: 0x060000FF RID: 255 RVA: 0x00004BA8 File Offset: 0x00003BA8
		public ComponentSerializer()
		{
			this._stream = new MemoryStream(0);
			this._selector = new ComSurrogateSelector();
			this._formatter = new BinaryFormatter();
			this._streamingCtx = new StreamingContext(StreamingContextStates.Other);
			this._formatter.Context = this._streamingCtx;
			this._headerhandler = new HeaderHandler(this.TPHeaderHandler);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004C0D File Offset: 0x00003C0D
		internal void SetStream(byte[] b)
		{
			this._stream.SetLength(0L);
			if (b != null)
			{
				this._stream.Write(b, 0, b.Length);
				this._stream.Position = 0L;
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004C3C File Offset: 0x00003C3C
		internal byte[] MarshalToBuffer(object o, out long numBytes)
		{
			this.SetStream(null);
			this._formatter.SurrogateSelector = this._selector;
			this._formatter.AssemblyFormat = FormatterAssemblyStyle.Full;
			this._formatter.Serialize(this._stream, o, null);
			numBytes = this._stream.Position;
			if (numBytes % 2L != 0L)
			{
				this._stream.WriteByte(0);
				numBytes += 1L;
			}
			return this._stream.GetBuffer();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004CB4 File Offset: 0x00003CB4
		public object TPHeaderHandler(Header[] Headers)
		{
			return this._tp;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004CBC File Offset: 0x00003CBC
		internal object UnmarshalFromBuffer(byte[] b, object tp)
		{
			object obj = null;
			this.SetStream(b);
			this._tp = tp;
			try
			{
				this._formatter.SurrogateSelector = null;
				this._formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
				obj = this._formatter.Deserialize(this._stream, this._headerhandler);
			}
			finally
			{
				this._tp = null;
			}
			return obj;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004D24 File Offset: 0x00003D24
		internal object UnmarshalReturnMessageFromBuffer(byte[] b, IMethodCallMessage msg)
		{
			this.SetStream(b);
			this._formatter.SurrogateSelector = null;
			this._formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			return this._formatter.DeserializeMethodResponse(this._stream, null, msg);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004D58 File Offset: 0x00003D58
		internal static ComponentSerializer Get()
		{
			ComponentSerializer componentSerializer = (ComponentSerializer)ComponentSerializer._stack.Pop();
			if (componentSerializer == null)
			{
				componentSerializer = new ComponentSerializer();
			}
			return componentSerializer;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004D7F File Offset: 0x00003D7F
		internal void Release()
		{
			if (ComponentSerializer._stack.Count < ComponentSerializer.MaxBuffersCached && this._stream.Capacity < ComponentSerializer.MaxCachedBufferLength)
			{
				ComponentSerializer._stack.Push(this);
			}
		}

		// Token: 0x04000071 RID: 113
		private static readonly int MaxBuffersCached = 40;

		// Token: 0x04000072 RID: 114
		private static readonly int MaxCachedBufferLength = 262144;

		// Token: 0x04000073 RID: 115
		private static InterlockedStack _stack = new InterlockedStack();

		// Token: 0x04000074 RID: 116
		private MemoryStream _stream;

		// Token: 0x04000075 RID: 117
		private ISurrogateSelector _selector;

		// Token: 0x04000076 RID: 118
		private BinaryFormatter _formatter;

		// Token: 0x04000077 RID: 119
		private StreamingContext _streamingCtx;

		// Token: 0x04000078 RID: 120
		private HeaderHandler _headerhandler;

		// Token: 0x04000079 RID: 121
		private object _tp;
	}
}
