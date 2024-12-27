using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;
using System.Security.Permissions;
using System.Text;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200002B RID: 43
	internal sealed class ObjectWriter
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x0000C2A0 File Offset: 0x0000B2A0
		internal ObjectWriter(Stream stream, ISurrogateSelector selector, StreamingContext context, InternalFE formatterEnums)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream", SoapUtil.GetResourceString("ArgumentNull_Stream"));
			}
			this.m_stream = stream;
			this.m_surrogates = selector;
			this.m_context = context;
			this.formatterEnums = formatterEnums;
			this.m_objectManager = new SerializationObjectManager(context);
			this.m_formatterConverter = new FormatterConverter();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000C334 File Offset: 0x0000B334
		internal void Serialize(object graph, Header[] inHeaders, SoapWriter serWriter)
		{
			ObjectWriter.serializationPermission.Demand();
			if (graph == null)
			{
				throw new ArgumentNullException("graph", SoapUtil.GetResourceString("ArgumentNull_Graph"));
			}
			if (serWriter == null)
			{
				throw new ArgumentNullException("serWriter", string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("ArgumentNull_WithParamName"), new object[] { "serWriter" }));
			}
			this.serObjectInfoInit = new SerObjectInfoInit();
			this.serWriter = serWriter;
			this.headers = inHeaders;
			if (graph is IMethodMessage)
			{
				this.bRemoting = true;
				MethodBase methodBase = ((IMethodMessage)graph).MethodBase;
				if (methodBase != null)
				{
					serWriter.WriteXsdVersion(this.ProcessTypeAttributes(methodBase.ReflectedType));
				}
				else
				{
					serWriter.WriteXsdVersion(XsdVersion.V2001);
				}
			}
			else
			{
				serWriter.WriteXsdVersion(XsdVersion.V2001);
			}
			this.m_idGenerator = new ObjectIDGenerator();
			this.m_objectQueue = new Queue();
			if (graph is ISoapMessage)
			{
				this.bRemoting = true;
				ISoapMessage soapMessage = (ISoapMessage)graph;
				graph = new InternalSoapMessage(soapMessage.MethodName, soapMessage.XmlNameSpace, soapMessage.ParamNames, soapMessage.ParamValues, soapMessage.ParamTypes);
				this.headers = soapMessage.Headers;
			}
			this.m_serializedTypeTable = new Hashtable();
			serWriter.WriteBegin();
			bool flag;
			this.topId = this.m_idGenerator.GetId(graph, out flag);
			long num;
			if (this.headers != null)
			{
				num = this.m_idGenerator.GetId(this.headers, out flag);
			}
			else
			{
				num = -1L;
			}
			this.WriteSerializedStreamHeader(this.topId, num);
			if (this.headers != null && this.headers.Length != 0)
			{
				this.ProcessHeaders(num);
			}
			this.m_objectQueue.Enqueue(graph);
			long num2;
			object next;
			while ((next = this.GetNext(out num2)) != null)
			{
				WriteObjectInfo writeObjectInfo;
				if (next is WriteObjectInfo)
				{
					writeObjectInfo = (WriteObjectInfo)next;
				}
				else
				{
					writeObjectInfo = WriteObjectInfo.Serialize(next, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, null, this);
					writeObjectInfo.assemId = this.GetAssemblyId(writeObjectInfo);
				}
				writeObjectInfo.objectId = num2;
				NameInfo nameInfo = this.TypeToNameInfo(writeObjectInfo);
				nameInfo.NIisTopLevelObject = true;
				if (this.bRemoting && next == graph)
				{
					nameInfo.NIisRemoteRecord = true;
				}
				this.Write(writeObjectInfo, nameInfo, nameInfo);
				this.PutNameInfo(nameInfo);
				writeObjectInfo.ObjectEnd();
			}
			serWriter.WriteSerializationHeaderEnd();
			serWriter.WriteEnd();
			this.m_idGenerator = new ObjectIDGenerator();
			this.m_serializedTypeTable = new Hashtable();
			this.m_objectManager.RaiseOnSerializedEvent();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000C59C File Offset: 0x0000B59C
		private XsdVersion ProcessTypeAttributes(Type type)
		{
			SoapTypeAttribute soapTypeAttribute = InternalRemotingServices.GetCachedSoapAttribute(type) as SoapTypeAttribute;
			XsdVersion xsdVersion = XsdVersion.V2001;
			if (soapTypeAttribute != null)
			{
				SoapOption soapOption = soapTypeAttribute.SoapOptions;
				if ((soapOption &= SoapOption.Option1) == SoapOption.Option1)
				{
					xsdVersion = XsdVersion.V1999;
				}
				else if ((soapOption & SoapOption.Option1) == SoapOption.Option2)
				{
					xsdVersion = XsdVersion.V2000;
				}
			}
			return xsdVersion;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000C5DC File Offset: 0x0000B5DC
		private void ProcessHeaders(long headerId)
		{
			this.serWriter.WriteHeader((int)headerId, this.headers.Length);
			for (int i = 0; i < this.headers.Length; i++)
			{
				Type type = null;
				if (this.headers[i].Value != null)
				{
					type = this.GetType(this.headers[i].Value);
				}
				if (type != null && type == Converter.typeofString)
				{
					NameInfo nameInfo = this.GetNameInfo();
					nameInfo.NInameSpaceEnum = InternalNameSpaceE.UserNameSpace;
					nameInfo.NIname = this.headers[i].Name;
					nameInfo.NIisMustUnderstand = this.headers[i].MustUnderstand;
					nameInfo.NIobjectId = -1L;
					this.HeaderNamespace(this.headers[i], nameInfo);
					this.serWriter.WriteHeaderString(nameInfo, this.headers[i].Value.ToString());
					this.PutNameInfo(nameInfo);
				}
				else if (this.headers[i].Name.Equals("__MethodSignature"))
				{
					if (!(this.headers[i].Value is Type[]))
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_MethodSignature"), new object[] { type }));
					}
					Type[] array = (Type[])this.headers[i].Value;
					NameInfo[] array2 = new NameInfo[array.Length];
					WriteObjectInfo[] array3 = new WriteObjectInfo[array.Length];
					for (int j = 0; j < array.Length; j++)
					{
						array3[j] = WriteObjectInfo.Serialize(array[j], this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, null);
						array3[j].objectId = -1L;
						array3[j].assemId = this.GetAssemblyId(array3[j]);
						array2[j] = this.TypeToNameInfo(array3[j]);
					}
					NameInfo nameInfo2 = this.MemberToNameInfo(this.headers[i].Name);
					nameInfo2.NIisMustUnderstand = this.headers[i].MustUnderstand;
					nameInfo2.NItransmitTypeOnMember = true;
					nameInfo2.NIisNestedObject = true;
					nameInfo2.NIisHeader = true;
					this.HeaderNamespace(this.headers[i], nameInfo2);
					this.serWriter.WriteHeaderMethodSignature(nameInfo2, array2);
					for (int k = 0; k < array.Length; k++)
					{
						this.PutNameInfo(array2[k]);
						array3[k].ObjectEnd();
					}
					this.PutNameInfo(nameInfo2);
				}
				else
				{
					InternalPrimitiveTypeE internalPrimitiveTypeE = InternalPrimitiveTypeE.Invalid;
					if (type != null)
					{
						internalPrimitiveTypeE = Converter.ToCode(type);
					}
					if (type != null && internalPrimitiveTypeE == InternalPrimitiveTypeE.Invalid)
					{
						long num = this.Schedule(this.headers[i].Value, type);
						if (num == -1L)
						{
							WriteObjectInfo writeObjectInfo = WriteObjectInfo.Serialize(this.headers[i].Value, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, null, this);
							writeObjectInfo.objectId = -1L;
							writeObjectInfo.assemId = this.GetAssemblyId(writeObjectInfo);
							NameInfo nameInfo3 = this.TypeToNameInfo(writeObjectInfo);
							NameInfo nameInfo4 = this.MemberToNameInfo(this.headers[i].Name);
							nameInfo4.NIisMustUnderstand = this.headers[i].MustUnderstand;
							nameInfo4.NItransmitTypeOnMember = true;
							nameInfo4.NIisNestedObject = true;
							nameInfo4.NIisHeader = true;
							this.HeaderNamespace(this.headers[i], nameInfo4);
							this.Write(writeObjectInfo, nameInfo4, nameInfo3);
							this.PutNameInfo(nameInfo3);
							this.PutNameInfo(nameInfo4);
							writeObjectInfo.ObjectEnd();
						}
						else
						{
							NameInfo nameInfo5 = this.MemberToNameInfo(this.headers[i].Name);
							nameInfo5.NIisMustUnderstand = this.headers[i].MustUnderstand;
							nameInfo5.NIobjectId = num;
							nameInfo5.NItransmitTypeOnMember = true;
							nameInfo5.NIisNestedObject = true;
							this.HeaderNamespace(this.headers[i], nameInfo5);
							this.serWriter.WriteHeaderObjectRef(nameInfo5);
							this.PutNameInfo(nameInfo5);
						}
					}
					else
					{
						NameInfo nameInfo6 = this.GetNameInfo();
						nameInfo6.NInameSpaceEnum = InternalNameSpaceE.UserNameSpace;
						nameInfo6.NIname = this.headers[i].Name;
						nameInfo6.NIisMustUnderstand = this.headers[i].MustUnderstand;
						nameInfo6.NIprimitiveTypeEnum = internalPrimitiveTypeE;
						this.HeaderNamespace(this.headers[i], nameInfo6);
						NameInfo nameInfo7 = null;
						if (type != null)
						{
							nameInfo7 = this.TypeToNameInfo(type);
							nameInfo7.NItransmitTypeOnMember = true;
						}
						this.serWriter.WriteHeaderEntry(nameInfo6, nameInfo7, this.headers[i].Value);
						this.PutNameInfo(nameInfo6);
						if (type != null)
						{
							this.PutNameInfo(nameInfo7);
						}
					}
				}
			}
			this.serWriter.WriteHeaderArrayEnd();
			long num2;
			object next;
			while ((next = this.GetNext(out num2)) != null)
			{
				WriteObjectInfo writeObjectInfo2;
				if (next is WriteObjectInfo)
				{
					writeObjectInfo2 = (WriteObjectInfo)next;
				}
				else
				{
					writeObjectInfo2 = WriteObjectInfo.Serialize(next, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, null, this);
					writeObjectInfo2.assemId = this.GetAssemblyId(writeObjectInfo2);
				}
				writeObjectInfo2.objectId = num2;
				NameInfo nameInfo8 = this.TypeToNameInfo(writeObjectInfo2);
				this.Write(writeObjectInfo2, nameInfo8, nameInfo8);
				this.PutNameInfo(nameInfo8);
				writeObjectInfo2.ObjectEnd();
			}
			this.serWriter.WriteHeaderSectionEnd();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000CAE8 File Offset: 0x0000BAE8
		private void HeaderNamespace(Header header, NameInfo nameInfo)
		{
			if (header.HeaderNamespace == null)
			{
				nameInfo.NInamespace = this.headerNamespace;
			}
			else
			{
				nameInfo.NInamespace = header.HeaderNamespace;
			}
			bool flag = false;
			nameInfo.NIheaderPrefix = "h" + this.InternalGetId(nameInfo.NInamespace, Converter.typeofString, out flag);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000CB41 File Offset: 0x0000BB41
		internal SerializationObjectManager ObjectManager
		{
			get
			{
				return this.m_objectManager;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000CB4C File Offset: 0x0000BB4C
		private void Write(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo)
		{
			object obj = objectInfo.obj;
			if (obj == null)
			{
				throw new ArgumentNullException("objectInfo.obj", string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ArgumentNull_Obj"), new object[] { objectInfo.objectType }));
			}
			if (objectInfo.objectType.IsGenericType)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_SoapNoGenericsSupport"), new object[] { objectInfo.objectType }));
			}
			Type objectType = objectInfo.objectType;
			long objectId = objectInfo.objectId;
			if (objectType == Converter.typeofString)
			{
				memberNameInfo.NIobjectId = objectId;
				this.serWriter.WriteObjectString(memberNameInfo, obj.ToString());
				return;
			}
			if (objectType == Converter.typeofTimeSpan)
			{
				this.serWriter.WriteTopPrimitive(memberNameInfo, obj);
				return;
			}
			if (objectType.IsArray)
			{
				this.WriteArray(objectInfo, null, null);
			}
			else
			{
				string[] array;
				Type[] array2;
				object[] array3;
				SoapAttributeInfo[] array4;
				objectInfo.GetMemberInfo(out array, out array2, out array3, out array4);
				if (this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.TypesAlways))
				{
					memberNameInfo.NItransmitTypeOnObject = true;
					memberNameInfo.NIisParentTypeOnObject = true;
					typeNameInfo.NItransmitTypeOnObject = true;
					typeNameInfo.NIisParentTypeOnObject = true;
				}
				WriteObjectInfo[] array5 = new WriteObjectInfo[array.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					if (Nullable.GetUnderlyingType(array2[i]) != null)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_SoapNoGenericsSupport"), new object[] { array2[i] }));
					}
					Type type;
					if (array3[i] != null)
					{
						type = this.GetType(array3[i]);
					}
					else
					{
						type = typeof(object);
					}
					if ((Converter.ToCode(type) == InternalPrimitiveTypeE.Invalid && type != Converter.typeofString) || (objectInfo.cache.memberAttributeInfos != null && objectInfo.cache.memberAttributeInfos[i] != null && (objectInfo.cache.memberAttributeInfos[i].IsXmlAttribute() || objectInfo.cache.memberAttributeInfos[i].IsXmlElement())))
					{
						if (array3[i] != null)
						{
							array5[i] = WriteObjectInfo.Serialize(array3[i], this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, (array4 == null) ? null : array4[i], this);
							array5[i].assemId = this.GetAssemblyId(array5[i]);
						}
						else
						{
							array5[i] = WriteObjectInfo.Serialize(array2[i], this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, (array4 == null) ? null : array4[i]);
							array5[i].assemId = this.GetAssemblyId(array5[i]);
						}
					}
				}
				this.Write(objectInfo, memberNameInfo, typeNameInfo, array, array2, array3, array5);
			}
			if (!this.m_serializedTypeTable.ContainsKey(objectType))
			{
				this.m_serializedTypeTable.Add(objectType, objectType);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000CE18 File Offset: 0x0000BE18
		private void Write(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo, string[] memberNames, Type[] memberTypes, object[] memberData, WriteObjectInfo[] memberObjectInfos)
		{
			int num = memberNames.Length;
			NameInfo nameInfo = null;
			if (objectInfo.cache.memberAttributeInfos != null)
			{
				for (int i = 0; i < objectInfo.cache.memberAttributeInfos.Length; i++)
				{
					if (objectInfo.cache.memberAttributeInfos[i] != null && objectInfo.cache.memberAttributeInfos[i].IsXmlAttribute())
					{
						this.WriteMemberSetup(objectInfo, memberNameInfo, typeNameInfo, memberNames[i], memberTypes[i], memberData[i], memberObjectInfos[i], true);
					}
				}
			}
			if (memberNameInfo != null)
			{
				memberNameInfo.NIobjectId = objectInfo.objectId;
				this.serWriter.WriteObject(memberNameInfo, typeNameInfo, num, memberNames, memberTypes, memberObjectInfos);
			}
			else if (objectInfo.objectId == this.topId && this.topName != null)
			{
				nameInfo = this.MemberToNameInfo(this.topName);
				nameInfo.NIobjectId = objectInfo.objectId;
				this.serWriter.WriteObject(nameInfo, typeNameInfo, num, memberNames, memberTypes, memberObjectInfos);
			}
			else if (objectInfo.objectType != Converter.typeofString)
			{
				typeNameInfo.NIobjectId = objectInfo.objectId;
				this.serWriter.WriteObject(typeNameInfo, null, num, memberNames, memberTypes, memberObjectInfos);
			}
			if (memberNameInfo.NIisParentTypeOnObject)
			{
				memberNameInfo.NItransmitTypeOnObject = true;
				memberNameInfo.NIisParentTypeOnObject = false;
			}
			else
			{
				memberNameInfo.NItransmitTypeOnObject = false;
			}
			for (int j = 0; j < num; j++)
			{
				if (objectInfo.cache.memberAttributeInfos == null || objectInfo.cache.memberAttributeInfos[j] == null || !objectInfo.cache.memberAttributeInfos[j].IsXmlAttribute())
				{
					this.WriteMemberSetup(objectInfo, memberNameInfo, typeNameInfo, memberNames[j], memberTypes[j], memberData[j], memberObjectInfos[j], false);
				}
			}
			if (memberNameInfo != null)
			{
				memberNameInfo.NIobjectId = objectInfo.objectId;
				this.serWriter.WriteObjectEnd(memberNameInfo, typeNameInfo);
				return;
			}
			if (objectInfo.objectId == this.topId && this.topName != null)
			{
				this.serWriter.WriteObjectEnd(nameInfo, typeNameInfo);
				this.PutNameInfo(nameInfo);
				return;
			}
			if (objectInfo.objectType != Converter.typeofString)
			{
				objectInfo.GetTypeFullName();
				this.serWriter.WriteObjectEnd(typeNameInfo, typeNameInfo);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000D004 File Offset: 0x0000C004
		private void WriteMemberSetup(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo, string memberName, Type memberType, object memberData, WriteObjectInfo memberObjectInfo, bool isAttribute)
		{
			NameInfo nameInfo = this.MemberToNameInfo(memberName);
			if (memberObjectInfo != null)
			{
				nameInfo.NIassemId = memberObjectInfo.assemId;
			}
			nameInfo.NItype = memberType;
			NameInfo nameInfo2;
			if (memberObjectInfo == null)
			{
				nameInfo2 = this.TypeToNameInfo(memberType);
			}
			else
			{
				nameInfo2 = this.TypeToNameInfo(memberObjectInfo);
			}
			nameInfo.NIisRemoteRecord = typeNameInfo.NIisRemoteRecord;
			nameInfo.NItransmitTypeOnObject = memberNameInfo.NItransmitTypeOnObject;
			nameInfo.NIisParentTypeOnObject = memberNameInfo.NIisParentTypeOnObject;
			this.WriteMembers(nameInfo, nameInfo2, memberData, objectInfo, typeNameInfo, memberObjectInfo, isAttribute);
			this.PutNameInfo(nameInfo);
			this.PutNameInfo(nameInfo2);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000D090 File Offset: 0x0000C090
		private void WriteMembers(NameInfo memberNameInfo, NameInfo memberTypeNameInfo, object memberData, WriteObjectInfo objectInfo, NameInfo typeNameInfo, WriteObjectInfo memberObjectInfo, bool isAttribute)
		{
			Type type = memberNameInfo.NItype;
			if (type == Converter.typeofObject || (type.IsValueType && objectInfo.isSi && Converter.IsSiTransmitType(memberTypeNameInfo.NIprimitiveTypeEnum)))
			{
				memberTypeNameInfo.NItransmitTypeOnMember = true;
				memberNameInfo.NItransmitTypeOnMember = true;
			}
			if (this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.TypesAlways))
			{
				memberTypeNameInfo.NItransmitTypeOnObject = true;
				memberNameInfo.NItransmitTypeOnObject = true;
				memberNameInfo.NIisParentTypeOnObject = true;
			}
			if (this.CheckForNull(objectInfo, memberNameInfo, memberTypeNameInfo, memberData))
			{
				return;
			}
			Type type2 = null;
			if (memberTypeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.Invalid)
			{
				if (RemotingServices.IsTransparentProxy(memberData))
				{
					type2 = Converter.typeofMarshalByRefObject;
				}
				else
				{
					type2 = this.GetType(memberData);
					if (type != type2)
					{
						memberTypeNameInfo.NItransmitTypeOnMember = true;
						memberNameInfo.NItransmitTypeOnMember = true;
					}
				}
			}
			if (type == Converter.typeofObject)
			{
				type = this.GetType(memberData);
				if (memberObjectInfo == null)
				{
					this.TypeToNameInfo(type, memberTypeNameInfo);
				}
				else
				{
					this.TypeToNameInfo(memberObjectInfo, memberTypeNameInfo);
				}
			}
			if (memberObjectInfo == null || !memberObjectInfo.isArray)
			{
				if (!this.WriteKnownValueClass(memberNameInfo, memberTypeNameInfo, memberData, isAttribute))
				{
					if (memberTypeNameInfo.NItype.IsEnum)
					{
						this.WriteEnum(memberNameInfo, memberTypeNameInfo, memberData, isAttribute);
						return;
					}
					if (isAttribute)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_NonPrimitive_XmlAttribute"), new object[] { memberNameInfo.NIname }));
					}
					if (type.IsValueType || objectInfo.IsEmbeddedAttribute(memberNameInfo.NIname) || this.IsEmbeddedAttribute(type2))
					{
						this.serWriter.WriteMemberNested(memberNameInfo);
						memberObjectInfo.objectId = -1L;
						NameInfo nameInfo = this.TypeToNameInfo(memberObjectInfo);
						nameInfo.NIobjectId = -1L;
						memberNameInfo.NIisNestedObject = true;
						if (objectInfo.isSi)
						{
							memberTypeNameInfo.NItransmitTypeOnMember = true;
							memberNameInfo.NItransmitTypeOnMember = true;
						}
						this.Write(memberObjectInfo, memberNameInfo, nameInfo);
						this.PutNameInfo(nameInfo);
						memberObjectInfo.ObjectEnd();
						return;
					}
					long num = this.Schedule(memberData, type2, memberObjectInfo);
					if (num < 0L)
					{
						this.serWriter.WriteMemberNested(memberNameInfo);
						memberObjectInfo.objectId = -1L;
						NameInfo nameInfo2 = this.TypeToNameInfo(memberObjectInfo);
						nameInfo2.NIobjectId = -1L;
						memberNameInfo.NIisNestedObject = true;
						this.Write(memberObjectInfo, memberNameInfo, nameInfo2);
						this.PutNameInfo(nameInfo2);
						memberObjectInfo.ObjectEnd();
						return;
					}
					memberNameInfo.NIobjectId = num;
					this.WriteObjectRef(memberNameInfo, memberTypeNameInfo, num);
				}
				return;
			}
			long num2 = 0L;
			if (!objectInfo.IsEmbeddedAttribute(memberNameInfo.NIname) && !this.IsEmbeddedAttribute(type))
			{
				num2 = this.Schedule(memberData, type2, memberObjectInfo);
			}
			if (num2 > 0L)
			{
				memberNameInfo.NIobjectId = num2;
				this.WriteObjectRef(memberNameInfo, memberTypeNameInfo, num2);
				return;
			}
			this.serWriter.WriteMemberNested(memberNameInfo);
			memberObjectInfo.objectId = num2;
			memberNameInfo.NIobjectId = num2;
			memberNameInfo.NIisNestedObject = true;
			this.WriteArray(memberObjectInfo, memberNameInfo, memberObjectInfo);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000D334 File Offset: 0x0000C334
		private void WriteArray(WriteObjectInfo objectInfo, NameInfo memberNameInfo, WriteObjectInfo memberObjectInfo)
		{
			bool flag = false;
			if (memberNameInfo == null)
			{
				memberNameInfo = this.TypeToNameInfo(objectInfo);
				memberNameInfo.NIisTopLevelObject = true;
				flag = true;
			}
			memberNameInfo.NIisArray = true;
			long objectId = objectInfo.objectId;
			memberNameInfo.NIobjectId = objectInfo.objectId;
			Array array = (Array)objectInfo.obj;
			Type objectType = objectInfo.objectType;
			Type elementType = objectType.GetElementType();
			if (Nullable.GetUnderlyingType(elementType) != null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_SoapNoGenericsSupport"), new object[] { elementType }));
			}
			WriteObjectInfo writeObjectInfo = WriteObjectInfo.Serialize(elementType, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, (memberObjectInfo == null) ? null : memberObjectInfo.typeAttributeInfo);
			writeObjectInfo.assemId = this.GetAssemblyId(writeObjectInfo);
			NameInfo nameInfo = null;
			NameInfo nameInfo2 = this.ArrayTypeToNameInfo(objectInfo, out nameInfo);
			nameInfo2.NIobjectId = objectId;
			nameInfo2.NIisArray = true;
			nameInfo.NIobjectId = objectId;
			nameInfo.NItransmitTypeOnMember = memberNameInfo.NItransmitTypeOnMember;
			nameInfo.NItransmitTypeOnObject = memberNameInfo.NItransmitTypeOnObject;
			nameInfo.NIisParentTypeOnObject = memberNameInfo.NIisParentTypeOnObject;
			int rank = array.Rank;
			int[] array2 = new int[rank];
			int[] array3 = new int[rank];
			int[] array4 = new int[rank];
			for (int i = 0; i < rank; i++)
			{
				array2[i] = array.GetLength(i);
				array3[i] = array.GetLowerBound(i);
				array4[i] = array.GetUpperBound(i);
			}
			InternalArrayTypeE internalArrayTypeE;
			if (elementType.IsArray)
			{
				if (rank == 1)
				{
					internalArrayTypeE = InternalArrayTypeE.Jagged;
				}
				else
				{
					internalArrayTypeE = InternalArrayTypeE.Rectangular;
				}
			}
			else if (rank == 1)
			{
				internalArrayTypeE = InternalArrayTypeE.Single;
			}
			else
			{
				internalArrayTypeE = InternalArrayTypeE.Rectangular;
			}
			if (elementType == Converter.typeofByte && rank == 1 && array3[0] == 0)
			{
				this.serWriter.WriteObjectByteArray(memberNameInfo, nameInfo2, writeObjectInfo, nameInfo, array2[0], array3[0], (byte[])array);
				return;
			}
			if (elementType == Converter.typeofObject)
			{
				memberNameInfo.NItransmitTypeOnMember = true;
				nameInfo.NItransmitTypeOnMember = true;
			}
			if (this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.TypesAlways))
			{
				memberNameInfo.NItransmitTypeOnObject = true;
				nameInfo.NItransmitTypeOnObject = true;
			}
			if (internalArrayTypeE == InternalArrayTypeE.Single)
			{
				nameInfo2.NIname = string.Concat(new object[]
				{
					nameInfo.NIname,
					"[",
					array2[0],
					"]"
				});
				this.serWriter.WriteSingleArray(memberNameInfo, nameInfo2, writeObjectInfo, nameInfo, array2[0], array3[0], array);
				if (Converter.IsWriteAsByteArray(nameInfo.NIprimitiveTypeEnum) && array3[0] == 0)
				{
					nameInfo.NIobjectId = 0L;
					if (this.primitiveArray == null)
					{
						this.primitiveArray = new PrimitiveArray(nameInfo.NIprimitiveTypeEnum, array);
					}
					else
					{
						this.primitiveArray.Init(nameInfo.NIprimitiveTypeEnum, array);
					}
					int num = array4[0] + 1;
					for (int j = array3[0]; j < num; j++)
					{
						this.serWriter.WriteItemString(nameInfo, nameInfo, this.primitiveArray.GetValue(j));
					}
				}
				else
				{
					object[] array5 = null;
					if (!elementType.IsValueType)
					{
						array5 = (object[])array;
					}
					int num2 = array4[0] + 1;
					if (array5 != null)
					{
						int num3 = array3[0] - 1;
						for (int k = array3[0]; k < num2; k++)
						{
							if (array5[k] != null)
							{
								num3 = k;
							}
						}
						num2 = num3 + 1;
					}
					for (int l = array3[0]; l < num2; l++)
					{
						if (array5 == null)
						{
							this.WriteArrayMember(objectInfo, nameInfo, array.GetValue(l));
						}
						else
						{
							this.WriteArrayMember(objectInfo, nameInfo, array5[l]);
						}
					}
				}
			}
			else if (internalArrayTypeE == InternalArrayTypeE.Jagged)
			{
				int num4 = nameInfo2.NIname.IndexOf('[');
				if (num4 < 0)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Dimensions"), new object[] { nameInfo.NIname }));
				}
				nameInfo2.NIname.Substring(num4);
				nameInfo2.NIname = string.Concat(new object[]
				{
					nameInfo.NIname,
					"[",
					array2[0],
					"]"
				});
				nameInfo2.NIobjectId = objectId;
				this.serWriter.WriteJaggedArray(memberNameInfo, nameInfo2, writeObjectInfo, nameInfo, array2[0], array3[0]);
				object[] array6 = (object[])array;
				for (int m = array3[0]; m < array4[0] + 1; m++)
				{
					this.WriteArrayMember(objectInfo, nameInfo, array6[m]);
				}
			}
			else
			{
				nameInfo2.NIname.IndexOf('[');
				StringBuilder stringBuilder = new StringBuilder(10);
				stringBuilder.Append(nameInfo.NIname);
				stringBuilder.Append('[');
				for (int n = 0; n < rank; n++)
				{
					stringBuilder.Append(array2[n]);
					if (n < rank - 1)
					{
						stringBuilder.Append(',');
					}
				}
				stringBuilder.Append(']');
				nameInfo2.NIname = stringBuilder.ToString();
				nameInfo2.NIobjectId = objectId;
				this.serWriter.WriteRectangleArray(memberNameInfo, nameInfo2, writeObjectInfo, nameInfo, rank, array2, array3);
				bool flag2 = false;
				for (int num5 = 0; num5 < rank; num5++)
				{
					if (array2[num5] == 0)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					this.WriteRectangle(objectInfo, rank, array2, array, nameInfo, array3);
				}
			}
			this.serWriter.WriteObjectEnd(memberNameInfo, nameInfo2);
			this.PutNameInfo(nameInfo);
			this.PutNameInfo(nameInfo2);
			if (flag)
			{
				this.PutNameInfo(memberNameInfo);
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000D898 File Offset: 0x0000C898
		private void WriteArrayMember(WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, object data)
		{
			arrayElemTypeNameInfo.NIisArrayItem = true;
			if (this.CheckForNull(objectInfo, arrayElemTypeNameInfo, arrayElemTypeNameInfo, data))
			{
				return;
			}
			Type type = null;
			bool flag = false;
			if (arrayElemTypeNameInfo.NItransmitTypeOnMember)
			{
				flag = true;
			}
			if (!flag && !arrayElemTypeNameInfo.NIisSealed)
			{
				type = this.GetType(data);
				if (arrayElemTypeNameInfo.NItype != type)
				{
					flag = true;
				}
			}
			NameInfo nameInfo;
			if (flag)
			{
				if (type == null)
				{
					type = this.GetType(data);
				}
				nameInfo = this.TypeToNameInfo(type);
				nameInfo.NItransmitTypeOnMember = true;
				nameInfo.NIobjectId = arrayElemTypeNameInfo.NIobjectId;
				nameInfo.NIassemId = arrayElemTypeNameInfo.NIassemId;
				nameInfo.NIisArrayItem = true;
				nameInfo.NIitemName = arrayElemTypeNameInfo.NIitemName;
			}
			else
			{
				nameInfo = arrayElemTypeNameInfo;
				nameInfo.NIisArrayItem = true;
			}
			if (!this.WriteKnownValueClass(arrayElemTypeNameInfo, nameInfo, data, false))
			{
				if (nameInfo.NItype.IsEnum)
				{
					WriteObjectInfo writeObjectInfo = WriteObjectInfo.Serialize(data, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, null, this);
					nameInfo.NIassemId = this.GetAssemblyId(writeObjectInfo);
					this.WriteEnum(arrayElemTypeNameInfo, nameInfo, data, false);
				}
				else
				{
					long num = this.Schedule(data, nameInfo.NItype);
					arrayElemTypeNameInfo.NIobjectId = num;
					nameInfo.NIobjectId = num;
					if (num < 1L)
					{
						WriteObjectInfo writeObjectInfo2 = WriteObjectInfo.Serialize(data, this.m_surrogates, this.m_context, this.serObjectInfoInit, this.m_formatterConverter, null, this);
						writeObjectInfo2.objectId = num;
						writeObjectInfo2.assemId = this.GetAssemblyId(writeObjectInfo2);
						if (type == null)
						{
							type = this.GetType(data);
						}
						if (data != null && type.IsArray)
						{
							this.WriteArray(writeObjectInfo2, nameInfo, null);
						}
						else
						{
							nameInfo.NIisNestedObject = true;
							NameInfo nameInfo2 = this.TypeToNameInfo(writeObjectInfo2);
							nameInfo2.NIobjectId = num;
							writeObjectInfo2.objectId = num;
							this.Write(writeObjectInfo2, nameInfo, nameInfo2);
						}
						writeObjectInfo2.ObjectEnd();
					}
					else
					{
						this.serWriter.WriteItemObjectRef(arrayElemTypeNameInfo, (int)num);
					}
				}
			}
			if (arrayElemTypeNameInfo.NItransmitTypeOnMember)
			{
				this.PutNameInfo(nameInfo);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000DA70 File Offset: 0x0000CA70
		private void WriteRectangle(WriteObjectInfo objectInfo, int rank, int[] maxA, Array array, NameInfo arrayElemNameTypeInfo, int[] lowerBoundA)
		{
			int[] array2 = new int[rank];
			int[] array3 = null;
			bool flag = false;
			if (lowerBoundA != null)
			{
				for (int i = 0; i < rank; i++)
				{
					if (lowerBoundA[i] != 0)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				array3 = new int[rank];
			}
			bool flag2 = true;
			while (flag2)
			{
				flag2 = false;
				if (flag)
				{
					for (int j = 0; j < rank; j++)
					{
						array3[j] = array2[j] + lowerBoundA[j];
					}
					this.WriteArrayMember(objectInfo, arrayElemNameTypeInfo, array.GetValue(array3));
				}
				else
				{
					this.WriteArrayMember(objectInfo, arrayElemNameTypeInfo, array.GetValue(array2));
				}
				for (int k = rank - 1; k > -1; k--)
				{
					if (array2[k] < maxA[k] - 1)
					{
						array2[k]++;
						if (k < rank - 1)
						{
							for (int l = k + 1; l < rank; l++)
							{
								array2[l] = 0;
							}
						}
						flag2 = true;
						break;
					}
				}
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000DB58 File Offset: 0x0000CB58
		[Conditional("SER_LOGGING")]
		private void IndexTraceMessage(string message, int[] index)
		{
			StringBuilder stringBuilder = new StringBuilder(10);
			stringBuilder.Append("[");
			for (int i = 0; i < index.Length; i++)
			{
				stringBuilder.Append(index[i]);
				if (i != index.Length - 1)
				{
					stringBuilder.Append(",");
				}
			}
			stringBuilder.Append("]");
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000DBB4 File Offset: 0x0000CBB4
		private object GetNext(out long objID)
		{
			if (this.m_objectQueue.Count == 0)
			{
				objID = 0L;
				return null;
			}
			object obj = this.m_objectQueue.Dequeue();
			object obj2;
			if (obj is WriteObjectInfo)
			{
				obj2 = ((WriteObjectInfo)obj).obj;
			}
			else
			{
				obj2 = obj;
			}
			bool flag;
			objID = this.m_idGenerator.HasId(obj2, out flag);
			if (flag)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ObjNoID"), new object[] { obj }));
			}
			return obj;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000DC34 File Offset: 0x0000CC34
		private long InternalGetId(object obj, Type type, out bool isNew)
		{
			if (obj == this.previousObj)
			{
				isNew = false;
				return this.previousId;
			}
			if (type.IsValueType)
			{
				isNew = false;
				this.previousObj = obj;
				this.previousId = -1L;
				return -1L;
			}
			long id = this.m_idGenerator.GetId(obj, out isNew);
			this.previousObj = obj;
			this.previousId = id;
			return id;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000DC8E File Offset: 0x0000CC8E
		private long Schedule(object obj, Type type)
		{
			return this.Schedule(obj, type, null);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000DC9C File Offset: 0x0000CC9C
		private long Schedule(object obj, Type type, WriteObjectInfo objectInfo)
		{
			if (obj == null)
			{
				return 0L;
			}
			bool flag;
			long num = this.InternalGetId(obj, type, out flag);
			if (flag)
			{
				if (objectInfo == null)
				{
					this.m_objectQueue.Enqueue(obj);
				}
				else
				{
					this.m_objectQueue.Enqueue(objectInfo);
				}
			}
			return num;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000DCDC File Offset: 0x0000CCDC
		private bool WriteKnownValueClass(NameInfo memberNameInfo, NameInfo typeNameInfo, object data, bool isAttribute)
		{
			if (typeNameInfo.NItype == Converter.typeofString)
			{
				if (isAttribute)
				{
					this.serWriter.WriteAttributeValue(memberNameInfo, typeNameInfo, (string)data);
				}
				else
				{
					this.WriteString(memberNameInfo, typeNameInfo, data);
				}
			}
			else
			{
				if (typeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.Invalid)
				{
					return false;
				}
				if (typeNameInfo.NIisArray)
				{
					this.serWriter.WriteItem(memberNameInfo, typeNameInfo, data);
				}
				else if (isAttribute)
				{
					this.serWriter.WriteAttributeValue(memberNameInfo, typeNameInfo, data);
				}
				else
				{
					this.serWriter.WriteMember(memberNameInfo, typeNameInfo, data);
				}
			}
			return true;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000DD5F File Offset: 0x0000CD5F
		private void WriteObjectRef(NameInfo nameInfo, NameInfo typeNameInfo, long objectId)
		{
			this.serWriter.WriteMemberObjectRef(nameInfo, typeNameInfo, (int)objectId);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000DD70 File Offset: 0x0000CD70
		private void WriteString(NameInfo memberNameInfo, NameInfo typeNameInfo, object stringObject)
		{
			bool flag = true;
			long num = -1L;
			if (!this.CheckTypeFormat(this.formatterEnums.FEtypeFormat, FormatterTypeStyle.XsdString))
			{
				num = this.InternalGetId(stringObject, typeNameInfo.NItype, out flag);
			}
			typeNameInfo.NIobjectId = num;
			if (!flag && num >= 0L)
			{
				this.WriteObjectRef(memberNameInfo, typeNameInfo, num);
				return;
			}
			if (typeNameInfo.NIisArray)
			{
				this.serWriter.WriteItemString(memberNameInfo, typeNameInfo, (string)stringObject);
				return;
			}
			this.serWriter.WriteMemberString(memberNameInfo, typeNameInfo, (string)stringObject);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000DDF0 File Offset: 0x0000CDF0
		private bool CheckForNull(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo, object data)
		{
			bool flag = false;
			if (data == null)
			{
				flag = true;
			}
			if (flag)
			{
				if (typeNameInfo.NItype.IsArray)
				{
					this.ArrayNameToDisplayName(objectInfo, typeNameInfo);
				}
				if (typeNameInfo.NIisArrayItem)
				{
					this.serWriter.WriteNullItem(memberNameInfo, typeNameInfo);
				}
				else
				{
					this.serWriter.WriteNullMember(memberNameInfo, typeNameInfo);
				}
			}
			return flag;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000DE42 File Offset: 0x0000CE42
		private void WriteSerializedStreamHeader(long topId, long headerId)
		{
			this.serWriter.WriteSerializationHeader((int)topId, (int)headerId, 1, 0);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000DE58 File Offset: 0x0000CE58
		private NameInfo TypeToNameInfo(Type type, WriteObjectInfo objectInfo, InternalPrimitiveTypeE code, NameInfo nameInfo)
		{
			if (nameInfo == null)
			{
				nameInfo = this.GetNameInfo();
			}
			else
			{
				nameInfo.Init();
			}
			nameInfo.NIisSealed = type.IsSealed;
			string text = null;
			nameInfo.NInameSpaceEnum = Converter.GetNameSpaceEnum(code, type, objectInfo, out text);
			nameInfo.NIprimitiveTypeEnum = code;
			nameInfo.NItype = type;
			nameInfo.NIname = text;
			if (objectInfo != null)
			{
				nameInfo.NIattributeInfo = objectInfo.typeAttributeInfo;
				nameInfo.NIassemId = objectInfo.assemId;
			}
			switch (nameInfo.NInameSpaceEnum)
			{
			case InternalNameSpaceE.XdrString:
				nameInfo.NIname = "string";
				break;
			case InternalNameSpaceE.UrtUser:
				if (type.Module.Assembly != Converter.urtAssembly)
				{
				}
				break;
			}
			return nameInfo;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000DF15 File Offset: 0x0000CF15
		private NameInfo TypeToNameInfo(Type type)
		{
			return this.TypeToNameInfo(type, null, Converter.ToCode(type), null);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000DF26 File Offset: 0x0000CF26
		private NameInfo TypeToNameInfo(WriteObjectInfo objectInfo)
		{
			return this.TypeToNameInfo(objectInfo.objectType, objectInfo, Converter.ToCode(objectInfo.objectType), null);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000DF41 File Offset: 0x0000CF41
		private NameInfo TypeToNameInfo(WriteObjectInfo objectInfo, NameInfo nameInfo)
		{
			return this.TypeToNameInfo(objectInfo.objectType, objectInfo, Converter.ToCode(objectInfo.objectType), nameInfo);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000DF5C File Offset: 0x0000CF5C
		private void TypeToNameInfo(Type type, NameInfo nameInfo)
		{
			this.TypeToNameInfo(type, null, Converter.ToCode(type), nameInfo);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000DF70 File Offset: 0x0000CF70
		private NameInfo ArrayTypeToNameInfo(WriteObjectInfo objectInfo, out NameInfo arrayElemTypeNameInfo)
		{
			NameInfo nameInfo = this.TypeToNameInfo(objectInfo);
			arrayElemTypeNameInfo = this.TypeToNameInfo(objectInfo.arrayElemObjectInfo);
			this.ArrayNameToDisplayName(objectInfo, arrayElemTypeNameInfo);
			nameInfo.NInameSpaceEnum = arrayElemTypeNameInfo.NInameSpaceEnum;
			arrayElemTypeNameInfo.NIisArray = arrayElemTypeNameInfo.NItype.IsArray;
			return nameInfo;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000DFC0 File Offset: 0x0000CFC0
		private NameInfo MemberToNameInfo(string name)
		{
			NameInfo nameInfo = this.GetNameInfo();
			nameInfo.NInameSpaceEnum = InternalNameSpaceE.MemberName;
			nameInfo.NIname = name;
			return nameInfo;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000DFE4 File Offset: 0x0000CFE4
		private void ArrayNameToDisplayName(WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo)
		{
			string niname = arrayElemTypeNameInfo.NIname;
			int num = niname.IndexOf('[');
			if (num > 0)
			{
				string text = niname.Substring(0, num);
				InternalPrimitiveTypeE internalPrimitiveTypeE = Converter.ToCode(text);
				bool flag = false;
				string text2;
				if (internalPrimitiveTypeE != InternalPrimitiveTypeE.Invalid)
				{
					if (internalPrimitiveTypeE == InternalPrimitiveTypeE.Char)
					{
						text2 = text;
						arrayElemTypeNameInfo.NInameSpaceEnum = InternalNameSpaceE.UrtSystem;
					}
					else
					{
						flag = true;
						text2 = Converter.ToXmlDataType(internalPrimitiveTypeE);
						string text3 = null;
						arrayElemTypeNameInfo.NInameSpaceEnum = Converter.GetNameSpaceEnum(internalPrimitiveTypeE, null, objectInfo, out text3);
					}
				}
				else if (text.Equals("String") || text.Equals("System.String"))
				{
					flag = true;
					text2 = "string";
					arrayElemTypeNameInfo.NInameSpaceEnum = InternalNameSpaceE.XdrString;
				}
				else if (text.Equals("System.Object"))
				{
					flag = true;
					text2 = "anyType";
					arrayElemTypeNameInfo.NInameSpaceEnum = InternalNameSpaceE.XdrPrimitive;
				}
				else
				{
					text2 = text;
				}
				if (flag)
				{
					arrayElemTypeNameInfo.NIname = text2 + niname.Substring(num);
					return;
				}
			}
			else if (niname.Equals("System.Object"))
			{
				arrayElemTypeNameInfo.NIname = "anyType";
				arrayElemTypeNameInfo.NInameSpaceEnum = InternalNameSpaceE.XdrPrimitive;
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000E0DC File Offset: 0x0000D0DC
		private long GetAssemblyId(WriteObjectInfo objectInfo)
		{
			bool flag = false;
			string assemblyString = objectInfo.GetAssemblyString();
			string text = assemblyString;
			long num;
			if (assemblyString.Length == 0)
			{
				num = 0L;
			}
			else if (assemblyString.Equals(Converter.urtAssemblyString))
			{
				num = 0L;
				flag = false;
				this.serWriter.WriteAssembly(objectInfo.GetTypeFullName(), objectInfo.objectType, null, (int)num, flag, objectInfo.IsAttributeNameSpace());
			}
			else
			{
				if (this.assemblyToIdTable.ContainsKey(assemblyString))
				{
					num = (long)this.assemblyToIdTable[assemblyString];
					flag = false;
				}
				else
				{
					num = this.m_idGenerator.GetId("___AssemblyString___" + assemblyString, out flag);
					this.assemblyToIdTable[assemblyString] = num;
				}
				if (assemblyString != null && !objectInfo.IsInteropNameSpace() && this.formatterEnums.FEassemblyFormat == FormatterAssemblyStyle.Simple)
				{
					int num2 = assemblyString.IndexOf(',');
					if (num2 > 0)
					{
						text = assemblyString.Substring(0, num2);
					}
				}
				this.serWriter.WriteAssembly(objectInfo.GetTypeFullName(), objectInfo.objectType, text, (int)num, flag, objectInfo.IsInteropNameSpace());
			}
			return num;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000E1E4 File Offset: 0x0000D1E4
		private bool IsEmbeddedAttribute(Type type)
		{
			bool flag;
			if (type.IsValueType)
			{
				flag = true;
			}
			else
			{
				SoapTypeAttribute soapTypeAttribute = (SoapTypeAttribute)InternalRemotingServices.GetCachedSoapAttribute(type);
				flag = soapTypeAttribute.Embedded;
			}
			return flag;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000E213 File Offset: 0x0000D213
		private void WriteEnum(NameInfo memberNameInfo, NameInfo typeNameInfo, object data, bool isAttribute)
		{
			if (isAttribute)
			{
				this.serWriter.WriteAttributeValue(memberNameInfo, typeNameInfo, ((Enum)data).ToString());
				return;
			}
			this.serWriter.WriteMember(memberNameInfo, typeNameInfo, ((Enum)data).ToString());
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000E24C File Offset: 0x0000D24C
		private Type GetType(object obj)
		{
			Type type;
			if (RemotingServices.IsTransparentProxy(obj))
			{
				type = Converter.typeofMarshalByRefObject;
			}
			else
			{
				type = obj.GetType();
			}
			return type;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000E274 File Offset: 0x0000D274
		private NameInfo GetNameInfo()
		{
			NameInfo nameInfo;
			if (!this.niPool.IsEmpty())
			{
				nameInfo = (NameInfo)this.niPool.Pop();
				nameInfo.Init();
			}
			else
			{
				nameInfo = new NameInfo();
			}
			return nameInfo;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000E2B0 File Offset: 0x0000D2B0
		private bool CheckTypeFormat(FormatterTypeStyle test, FormatterTypeStyle want)
		{
			return (test & want) == want;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000E2B8 File Offset: 0x0000D2B8
		private void PutNameInfo(NameInfo nameInfo)
		{
			this.niPool.Push(nameInfo);
		}

		// Token: 0x040001CA RID: 458
		private Queue m_objectQueue;

		// Token: 0x040001CB RID: 459
		private ObjectIDGenerator m_idGenerator;

		// Token: 0x040001CC RID: 460
		private Stream m_stream;

		// Token: 0x040001CD RID: 461
		private ISurrogateSelector m_surrogates;

		// Token: 0x040001CE RID: 462
		private StreamingContext m_context;

		// Token: 0x040001CF RID: 463
		private SoapWriter serWriter;

		// Token: 0x040001D0 RID: 464
		private SerializationObjectManager m_objectManager;

		// Token: 0x040001D1 RID: 465
		private Hashtable m_serializedTypeTable;

		// Token: 0x040001D2 RID: 466
		private long topId;

		// Token: 0x040001D3 RID: 467
		private string topName;

		// Token: 0x040001D4 RID: 468
		private Header[] headers;

		// Token: 0x040001D5 RID: 469
		private InternalFE formatterEnums;

		// Token: 0x040001D6 RID: 470
		private SerObjectInfoInit serObjectInfoInit;

		// Token: 0x040001D7 RID: 471
		private IFormatterConverter m_formatterConverter;

		// Token: 0x040001D8 RID: 472
		private string headerNamespace = "http://schemas.microsoft.com/clr/soap";

		// Token: 0x040001D9 RID: 473
		private bool bRemoting;

		// Token: 0x040001DA RID: 474
		internal static SecurityPermission serializationPermission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);

		// Token: 0x040001DB RID: 475
		private PrimitiveArray primitiveArray;

		// Token: 0x040001DC RID: 476
		private object previousObj;

		// Token: 0x040001DD RID: 477
		private long previousId;

		// Token: 0x040001DE RID: 478
		private Hashtable assemblyToIdTable = new Hashtable(20);

		// Token: 0x040001DF RID: 479
		private StringBuilder sburi = new StringBuilder(50);

		// Token: 0x040001E0 RID: 480
		private SerStack niPool = new SerStack("NameInfo Pool");
	}
}
