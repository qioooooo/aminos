using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000002 RID: 2
	public sealed class SoapFormatter : IRemotingFormatter, IFormatter
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		// (set) Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000010D8
		public ISoapMessage TopObject
		{
			get
			{
				return this.m_topObject;
			}
			set
			{
				this.m_topObject = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020E1 File Offset: 0x000010E1
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020E9 File Offset: 0x000010E9
		public FormatterTypeStyle TypeFormat
		{
			get
			{
				return this.m_typeFormat;
			}
			set
			{
				if (value == FormatterTypeStyle.TypesWhenNeeded)
				{
					this.m_typeFormat = FormatterTypeStyle.TypesWhenNeeded;
					return;
				}
				this.m_typeFormat |= value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002104 File Offset: 0x00001104
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000210C File Offset: 0x0000110C
		public FormatterAssemblyStyle AssemblyFormat
		{
			get
			{
				return this.m_assemblyFormat;
			}
			set
			{
				this.m_assemblyFormat = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002115 File Offset: 0x00001115
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000211D File Offset: 0x0000111D
		public TypeFilterLevel FilterLevel
		{
			get
			{
				return this.m_securityLevel;
			}
			set
			{
				this.m_securityLevel = value;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002126 File Offset: 0x00001126
		public SoapFormatter()
		{
			this.m_surrogates = null;
			this.m_context = new StreamingContext(StreamingContextStates.All);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002153 File Offset: 0x00001153
		public SoapFormatter(ISurrogateSelector selector, StreamingContext context)
		{
			this.m_surrogates = selector;
			this.m_context = context;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002177 File Offset: 0x00001177
		public object Deserialize(Stream serializationStream)
		{
			return this.Deserialize(serializationStream, null);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002184 File Offset: 0x00001184
		public object Deserialize(Stream serializationStream, HeaderHandler handler)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream");
			}
			if (serializationStream.CanSeek && serializationStream.Length == 0L)
			{
				throw new SerializationException(SoapUtil.GetResourceString("Serialization_Stream"));
			}
			InternalFE internalFE = new InternalFE();
			internalFE.FEtypeFormat = this.m_typeFormat;
			internalFE.FEtopObject = this.m_topObject;
			internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Soap;
			internalFE.FEassemblyFormat = this.m_assemblyFormat;
			internalFE.FEsecurityLevel = this.m_securityLevel;
			ObjectReader objectReader = new ObjectReader(serializationStream, this.m_surrogates, this.m_context, internalFE, this.m_binder);
			if (this.soapParser == null || serializationStream != this.currentStream)
			{
				this.soapParser = new SoapParser(serializationStream);
				this.currentStream = serializationStream;
			}
			this.soapParser.Init(objectReader);
			return objectReader.Deserialize(handler, this.soapParser);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002257 File Offset: 0x00001257
		public void Serialize(Stream serializationStream, object graph)
		{
			this.Serialize(serializationStream, graph, null);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002264 File Offset: 0x00001264
		public void Serialize(Stream serializationStream, object graph, Header[] headers)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream");
			}
			InternalFE internalFE = new InternalFE();
			internalFE.FEtypeFormat = this.m_typeFormat;
			internalFE.FEtopObject = this.m_topObject;
			internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Soap;
			internalFE.FEassemblyFormat = this.m_assemblyFormat;
			ObjectWriter objectWriter = new ObjectWriter(serializationStream, this.m_surrogates, this.m_context, internalFE);
			objectWriter.Serialize(graph, headers, new SoapWriter(serializationStream));
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022D2 File Offset: 0x000012D2
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000022DA File Offset: 0x000012DA
		public ISurrogateSelector SurrogateSelector
		{
			get
			{
				return this.m_surrogates;
			}
			set
			{
				this.m_surrogates = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022E3 File Offset: 0x000012E3
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000022EB File Offset: 0x000012EB
		public SerializationBinder Binder
		{
			get
			{
				return this.m_binder;
			}
			set
			{
				this.m_binder = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000022F4 File Offset: 0x000012F4
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000022FC File Offset: 0x000012FC
		public StreamingContext Context
		{
			get
			{
				return this.m_context;
			}
			set
			{
				this.m_context = value;
			}
		}

		// Token: 0x04000001 RID: 1
		private SoapParser soapParser;

		// Token: 0x04000002 RID: 2
		private ISurrogateSelector m_surrogates;

		// Token: 0x04000003 RID: 3
		private StreamingContext m_context;

		// Token: 0x04000004 RID: 4
		private FormatterTypeStyle m_typeFormat;

		// Token: 0x04000005 RID: 5
		private ISoapMessage m_topObject;

		// Token: 0x04000006 RID: 6
		private FormatterAssemblyStyle m_assemblyFormat = FormatterAssemblyStyle.Full;

		// Token: 0x04000007 RID: 7
		private TypeFilterLevel m_securityLevel = TypeFilterLevel.Full;

		// Token: 0x04000008 RID: 8
		private SerializationBinder m_binder;

		// Token: 0x04000009 RID: 9
		private Stream currentStream;
	}
}
