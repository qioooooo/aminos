using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007B9 RID: 1977
	[ComVisible(true)]
	public sealed class BinaryFormatter : IRemotingFormatter, IFormatter
	{
		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600464F RID: 17999 RVA: 0x000F070C File Offset: 0x000EF70C
		// (set) Token: 0x06004650 RID: 18000 RVA: 0x000F0714 File Offset: 0x000EF714
		public FormatterTypeStyle TypeFormat
		{
			get
			{
				return this.m_typeFormat;
			}
			set
			{
				this.m_typeFormat = value;
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06004651 RID: 18001 RVA: 0x000F071D File Offset: 0x000EF71D
		// (set) Token: 0x06004652 RID: 18002 RVA: 0x000F0725 File Offset: 0x000EF725
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

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x000F072E File Offset: 0x000EF72E
		// (set) Token: 0x06004654 RID: 18004 RVA: 0x000F0736 File Offset: 0x000EF736
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

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06004655 RID: 18005 RVA: 0x000F073F File Offset: 0x000EF73F
		// (set) Token: 0x06004656 RID: 18006 RVA: 0x000F0747 File Offset: 0x000EF747
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

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x000F0750 File Offset: 0x000EF750
		// (set) Token: 0x06004658 RID: 18008 RVA: 0x000F0758 File Offset: 0x000EF758
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

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06004659 RID: 18009 RVA: 0x000F0761 File Offset: 0x000EF761
		// (set) Token: 0x0600465A RID: 18010 RVA: 0x000F0769 File Offset: 0x000EF769
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

		// Token: 0x0600465B RID: 18011 RVA: 0x000F0772 File Offset: 0x000EF772
		public BinaryFormatter()
		{
			this.m_surrogates = null;
			this.m_context = new StreamingContext(StreamingContextStates.All);
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x000F079F File Offset: 0x000EF79F
		public BinaryFormatter(ISurrogateSelector selector, StreamingContext context)
		{
			this.m_surrogates = selector;
			this.m_context = context;
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x000F07C3 File Offset: 0x000EF7C3
		public object Deserialize(Stream serializationStream)
		{
			return this.Deserialize(serializationStream, null);
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x000F07CD File Offset: 0x000EF7CD
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck)
		{
			return this.Deserialize(serializationStream, null, fCheck, null);
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x000F07D9 File Offset: 0x000EF7D9
		public object Deserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.Deserialize(serializationStream, handler, true, null);
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x000F07E5 File Offset: 0x000EF7E5
		public object DeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, true, methodCallMessage);
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x000F07F1 File Offset: 0x000EF7F1
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public object UnsafeDeserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.Deserialize(serializationStream, handler, false, null);
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x000F07FD File Offset: 0x000EF7FD
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public object UnsafeDeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, false, methodCallMessage);
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x000F0809 File Offset: 0x000EF809
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, fCheck, false, methodCallMessage);
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x000F0818 File Offset: 0x000EF818
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, bool isCrossAppDomain, IMethodCallMessage methodCallMessage)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_WithParamName"), new object[] { serializationStream }));
			}
			if (serializationStream.CanSeek && serializationStream.Length == 0L)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_Stream"));
			}
			InternalFE internalFE = new InternalFE();
			internalFE.FEtypeFormat = this.m_typeFormat;
			internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Binary;
			internalFE.FEassemblyFormat = this.m_assemblyFormat;
			internalFE.FEsecurityLevel = this.m_securityLevel;
			ObjectReader objectReader = new ObjectReader(serializationStream, this.m_surrogates, this.m_context, internalFE, this.m_binder);
			objectReader.crossAppDomainArray = this.m_crossAppDomainArray;
			return objectReader.Deserialize(handler, new __BinaryParser(serializationStream, objectReader), fCheck, isCrossAppDomain, methodCallMessage);
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x000F08DF File Offset: 0x000EF8DF
		public void Serialize(Stream serializationStream, object graph)
		{
			this.Serialize(serializationStream, graph, null);
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x000F08EA File Offset: 0x000EF8EA
		public void Serialize(Stream serializationStream, object graph, Header[] headers)
		{
			this.Serialize(serializationStream, graph, headers, true);
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x000F08F8 File Offset: 0x000EF8F8
		internal void Serialize(Stream serializationStream, object graph, Header[] headers, bool fCheck)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_WithParamName"), new object[] { serializationStream }));
			}
			InternalFE internalFE = new InternalFE();
			internalFE.FEtypeFormat = this.m_typeFormat;
			internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Binary;
			internalFE.FEassemblyFormat = this.m_assemblyFormat;
			ObjectWriter objectWriter = new ObjectWriter(this.m_surrogates, this.m_context, internalFE);
			__BinaryWriter _BinaryWriter = new __BinaryWriter(serializationStream, objectWriter, this.m_typeFormat);
			objectWriter.Serialize(graph, headers, _BinaryWriter, fCheck);
			this.m_crossAppDomainArray = objectWriter.crossAppDomainArray;
		}

		// Token: 0x04002362 RID: 9058
		internal ISurrogateSelector m_surrogates;

		// Token: 0x04002363 RID: 9059
		internal StreamingContext m_context;

		// Token: 0x04002364 RID: 9060
		internal SerializationBinder m_binder;

		// Token: 0x04002365 RID: 9061
		internal FormatterTypeStyle m_typeFormat = FormatterTypeStyle.TypesAlways;

		// Token: 0x04002366 RID: 9062
		internal FormatterAssemblyStyle m_assemblyFormat;

		// Token: 0x04002367 RID: 9063
		internal TypeFilterLevel m_securityLevel = TypeFilterLevel.Full;

		// Token: 0x04002368 RID: 9064
		internal object[] m_crossAppDomainArray;
	}
}
