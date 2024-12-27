using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000029 RID: 41
	internal sealed class ObjectReader
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00009F50 File Offset: 0x00008F50
		internal ObjectReader(Stream stream, ISurrogateSelector selector, StreamingContext context, InternalFE formatterEnums, SerializationBinder binder)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream", SoapUtil.GetResourceString("ArgumentNull_Stream"));
			}
			this.m_stream = stream;
			this.m_surrogates = selector;
			this.m_context = context;
			this.m_binder = binder;
			this.formatterEnums = formatterEnums;
			if (formatterEnums.FEtopObject != null)
			{
				this.IsFakeTopObject = true;
			}
			else
			{
				this.IsFakeTopObject = false;
			}
			this.m_formatterConverter = new FormatterConverter();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000A00E File Offset: 0x0000900E
		private ObjectManager GetObjectManager()
		{
			new SecurityPermission(SecurityPermissionFlag.SerializationFormatter).Assert();
			return new ObjectManager(this.m_surrogates, this.m_context);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000A030 File Offset: 0x00009030
		internal object Deserialize(HeaderHandler handler, ISerParser serParser)
		{
			if (serParser == null)
			{
				throw new ArgumentNullException("serParser", string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("ArgumentNull_WithParamName"), new object[] { serParser }));
			}
			this.deserializationSecurityException = null;
			try
			{
				ObjectReader.serializationPermission.Demand();
			}
			catch (Exception ex)
			{
				this.deserializationSecurityException = ex;
			}
			catch
			{
				this.deserializationSecurityException = new Exception(SoapUtil.GetResourceString("Serialization_NonClsCompliantException"));
			}
			this.handler = handler;
			this.isTopObjectSecondPass = false;
			this.isHeaderHandlerCalled = false;
			if (handler != null)
			{
				this.IsFakeTopObject = true;
			}
			this.m_idGenerator = new ObjectIDGenerator();
			this.m_objectManager = this.GetObjectManager();
			this.serObjectInfoInit = new SerObjectInfoInit();
			this.objectIdTable.Clear();
			this.objectIds = 0L;
			serParser.Run();
			if (handler != null)
			{
				this.m_objectManager.DoFixups();
				if (this.handlerObject == null)
				{
					this.handlerObject = handler(this.newheaders);
				}
				if (this.soapFaultId > 0L && this.handlerObject != null)
				{
					this.topStack = new SerStack("Top ParseRecords");
					ParseRecord parseRecord = new ParseRecord();
					parseRecord.PRparseTypeEnum = InternalParseTypeE.Object;
					parseRecord.PRobjectPositionEnum = InternalObjectPositionE.Top;
					parseRecord.PRparseStateEnum = InternalParseStateE.Object;
					parseRecord.PRname = "Response";
					this.topStack.Push(parseRecord);
					parseRecord = new ParseRecord();
					parseRecord.PRparseTypeEnum = InternalParseTypeE.Member;
					parseRecord.PRobjectPositionEnum = InternalObjectPositionE.Child;
					parseRecord.PRmemberTypeEnum = InternalMemberTypeE.Field;
					parseRecord.PRmemberValueEnum = InternalMemberValueE.Reference;
					parseRecord.PRparseStateEnum = InternalParseStateE.Member;
					parseRecord.PRname = "__fault";
					parseRecord.PRidRef = this.soapFaultId;
					this.topStack.Push(parseRecord);
					parseRecord = new ParseRecord();
					parseRecord.PRparseTypeEnum = InternalParseTypeE.ObjectEnd;
					parseRecord.PRobjectPositionEnum = InternalObjectPositionE.Top;
					parseRecord.PRparseStateEnum = InternalParseStateE.Object;
					parseRecord.PRname = "Response";
					this.topStack.Push(parseRecord);
					this.isTopObjectResolved = false;
				}
			}
			if (!this.isTopObjectResolved)
			{
				this.isTopObjectSecondPass = true;
				this.topStack.Reverse();
				int num = this.topStack.Count();
				for (int i = 0; i < num; i++)
				{
					ParseRecord parseRecord2 = (ParseRecord)this.topStack.Pop();
					this.Parse(parseRecord2);
				}
			}
			this.m_objectManager.DoFixups();
			if (this.topObject == null)
			{
				throw new SerializationException(SoapUtil.GetResourceString("Serialization_TopObject"));
			}
			if (this.HasSurrogate(this.topObject.GetType()) && this.topId != 0L)
			{
				this.topObject = this.m_objectManager.GetObject(this.topId);
			}
			if (this.topObject is IObjectReference)
			{
				this.topObject = ((IObjectReference)this.topObject).GetRealObject(this.m_context);
			}
			this.m_objectManager.RaiseDeserializationEvent();
			if (this.formatterEnums.FEtopObject != null && this.topObject is InternalSoapMessage)
			{
				InternalSoapMessage internalSoapMessage = (InternalSoapMessage)this.topObject;
				ISoapMessage fetopObject = this.formatterEnums.FEtopObject;
				fetopObject.MethodName = internalSoapMessage.methodName;
				fetopObject.XmlNameSpace = internalSoapMessage.xmlNameSpace;
				fetopObject.ParamNames = internalSoapMessage.paramNames;
				fetopObject.ParamValues = internalSoapMessage.paramValues;
				fetopObject.Headers = this.headers;
				this.topObject = fetopObject;
				this.isTopObjectResolved = true;
			}
			return this.topObject;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000A38C File Offset: 0x0000938C
		private bool HasSurrogate(Type t)
		{
			ISurrogateSelector surrogateSelector;
			return this.m_surrogates != null && this.m_surrogates.GetSurrogate(t, this.m_context, out surrogateSelector) != null;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000A3C0 File Offset: 0x000093C0
		private void CheckSerializable(Type t)
		{
			if (!t.IsSerializable && !this.HasSurrogate(t))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_NonSerType"), new object[]
				{
					t.FullName,
					t.Module.Assembly.FullName
				}));
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000A41C File Offset: 0x0000941C
		internal ReadObjectInfo CreateReadObjectInfo(Type objectType, string assemblyName)
		{
			ReadObjectInfo readObjectInfo = ReadObjectInfo.Create(objectType, this.m_surrogates, this.m_context, this.m_objectManager, this.serObjectInfoInit, this.m_formatterConverter, assemblyName);
			readObjectInfo.SetVersion(this.majorVersion, this.minorVersion);
			return readObjectInfo;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000A464 File Offset: 0x00009464
		internal ReadObjectInfo CreateReadObjectInfo(Type objectType, string[] memberNames, Type[] memberTypes, string assemblyName)
		{
			ReadObjectInfo readObjectInfo = ReadObjectInfo.Create(objectType, memberNames, memberTypes, this.m_surrogates, this.m_context, this.m_objectManager, this.serObjectInfoInit, this.m_formatterConverter, assemblyName);
			readObjectInfo.SetVersion(this.majorVersion, this.minorVersion);
			return readObjectInfo;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000A4B0 File Offset: 0x000094B0
		internal void Parse(ParseRecord pr)
		{
			switch (pr.PRparseTypeEnum)
			{
			case InternalParseTypeE.SerializedStreamHeader:
				this.ParseSerializedStreamHeader(pr);
				return;
			case InternalParseTypeE.Object:
				this.ParseObject(pr);
				return;
			case InternalParseTypeE.Member:
				this.ParseMember(pr);
				return;
			case InternalParseTypeE.ObjectEnd:
				this.ParseObjectEnd(pr);
				return;
			case InternalParseTypeE.MemberEnd:
				this.ParseMemberEnd(pr);
				return;
			case InternalParseTypeE.SerializedStreamHeaderEnd:
				this.ParseSerializedStreamHeaderEnd(pr);
				return;
			case InternalParseTypeE.Envelope:
			case InternalParseTypeE.EnvelopeEnd:
			case InternalParseTypeE.Body:
			case InternalParseTypeE.BodyEnd:
				return;
			}
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_XMLElement"), new object[] { pr.PRname }));
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000A55C File Offset: 0x0000955C
		private void ParseError(ParseRecord processing, ParseRecord onStack)
		{
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ParseError"), new object[] { string.Concat(new string[]
			{
				onStack.PRname,
				" ",
				onStack.PRparseTypeEnum.ToString(),
				" ",
				processing.PRname,
				" ",
				processing.PRparseTypeEnum.ToString()
			}) }));
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000A5EA File Offset: 0x000095EA
		private void ParseSerializedStreamHeader(ParseRecord pr)
		{
			this.stack.Push(pr);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000A5F8 File Offset: 0x000095F8
		private void ParseSerializedStreamHeaderEnd(ParseRecord pr)
		{
			this.stack.Pop();
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000A606 File Offset: 0x00009606
		private bool IsRemoting
		{
			get
			{
				return this.IsFakeTopObject;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000A610 File Offset: 0x00009610
		private void CheckSecurity(ParseRecord pr)
		{
			Type prdtType = pr.PRdtType;
			if (prdtType != null && this.IsRemoting)
			{
				if (typeof(MarshalByRefObject).IsAssignableFrom(prdtType))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_MBRAsMBV"), new object[] { prdtType.FullName }));
				}
				FormatterServices.CheckTypeSecurity(prdtType, this.formatterEnums.FEsecurityLevel);
			}
			if (this.deserializationSecurityException == null)
			{
				return;
			}
			if (prdtType != null)
			{
				if (prdtType.IsPrimitive || prdtType == Converter.typeofString)
				{
					return;
				}
				if (typeof(Enum).IsAssignableFrom(prdtType))
				{
					return;
				}
				if (prdtType.IsArray)
				{
					Type elementType = prdtType.GetElementType();
					if (elementType.IsPrimitive || elementType == Converter.typeofString)
					{
						return;
					}
				}
			}
			throw this.deserializationSecurityException;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000A6D4 File Offset: 0x000096D4
		private void ParseObject(ParseRecord pr)
		{
			if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
			{
				this.topId = pr.PRobjectId;
			}
			if (pr.PRparseTypeEnum == InternalParseTypeE.Object)
			{
				this.stack.Push(pr);
			}
			if (pr.PRobjectTypeEnum == InternalObjectTypeE.Array)
			{
				this.ParseArray(pr);
				return;
			}
			if (pr.PRdtType == null && !this.IsFakeTopObject)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_TopObjectInstantiate"), new object[] { pr.PRname }));
			}
			if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top && this.IsFakeTopObject && pr.PRdtType != Converter.typeofSoapFault)
			{
				if (this.handler != null)
				{
					if (!this.isHeaderHandlerCalled)
					{
						this.newheaders = null;
						this.isHeaderHandlerCalled = true;
						if (this.headers == null)
						{
							this.newheaders = new Header[1];
						}
						else
						{
							this.newheaders = new Header[this.headers.Length + 1];
							Array.Copy(this.headers, 0, this.newheaders, 1, this.headers.Length);
						}
						Header header = new Header("__methodName", pr.PRname, false, pr.PRnameXmlKey);
						this.newheaders[0] = header;
						this.handlerObject = this.handler(this.newheaders);
					}
					if (!this.isHeaderHandlerCalled)
					{
						this.isTopObjectResolved = false;
						this.topStack = new SerStack("Top ParseRecords");
						this.topStack.Push(pr.Copy());
						return;
					}
					pr.PRnewObj = this.handlerObject;
					pr.PRdtType = this.handlerObject.GetType();
					this.CheckSecurity(pr);
					if (pr.PRnewObj is IFieldInfo)
					{
						IFieldInfo fieldInfo = (IFieldInfo)pr.PRnewObj;
						if (fieldInfo.FieldTypes != null && fieldInfo.FieldTypes.Length > 0)
						{
							pr.PRobjectInfo = this.CreateReadObjectInfo(pr.PRdtType, fieldInfo.FieldNames, fieldInfo.FieldTypes, pr.PRassemblyName);
						}
					}
				}
				else if (this.formatterEnums.FEtopObject != null)
				{
					if (!this.isTopObjectSecondPass)
					{
						this.isTopObjectResolved = false;
						this.topStack = new SerStack("Top ParseRecords");
						this.topStack.Push(pr.Copy());
						return;
					}
					pr.PRnewObj = new InternalSoapMessage();
					pr.PRdtType = typeof(InternalSoapMessage);
					this.CheckSecurity(pr);
					if (this.formatterEnums.FEtopObject != null)
					{
						ISoapMessage fetopObject = this.formatterEnums.FEtopObject;
						pr.PRobjectInfo = this.CreateReadObjectInfo(pr.PRdtType, fetopObject.ParamNames, fetopObject.ParamTypes, pr.PRassemblyName);
					}
				}
			}
			else
			{
				if (pr.PRdtType == Converter.typeofString)
				{
					if (pr.PRvalue != null)
					{
						pr.PRnewObj = pr.PRvalue;
						if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
						{
							this.isTopObjectResolved = true;
							this.topObject = pr.PRnewObj;
							return;
						}
						this.stack.Pop();
						this.RegisterObject(pr.PRnewObj, pr, (ParseRecord)this.stack.Peek());
					}
					return;
				}
				if (pr.PRdtType == null)
				{
					ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
					if (parseRecord.PRdtType == Converter.typeofSoapFault)
					{
						throw new ServerException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_SoapFault"), new object[] { this.faultString }));
					}
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_TypeElement"), new object[] { pr.PRname }));
				}
				else
				{
					this.CheckSerializable(pr.PRdtType);
					if (this.IsRemoting && this.formatterEnums.FEsecurityLevel != TypeFilterLevel.Full)
					{
						pr.PRnewObj = FormatterServices.GetSafeUninitializedObject(pr.PRdtType);
					}
					else
					{
						pr.PRnewObj = FormatterServices.GetUninitializedObject(pr.PRdtType);
					}
					this.CheckSecurity(pr);
					this.m_objectManager.RaiseOnDeserializingEvent(pr.PRnewObj);
				}
			}
			if (pr.PRnewObj == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_TopObjectInstantiate"), new object[] { pr.PRdtType }));
			}
			long probjectId = pr.PRobjectId;
			if (probjectId < 1L)
			{
				pr.PRobjectId = this.GetId("GenId-" + this.objectIds);
			}
			if (this.IsFakeTopObject && pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
			{
				this.isTopObjectResolved = true;
				this.topObject = pr.PRnewObj;
			}
			if (pr.PRobjectInfo == null)
			{
				pr.PRobjectInfo = this.CreateReadObjectInfo(pr.PRdtType, pr.PRassemblyName);
			}
			pr.PRobjectInfo.obj = pr.PRnewObj;
			if (this.IsFakeTopObject && pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
			{
				pr.PRobjectInfo.AddValue("__methodName", pr.PRname);
				pr.PRobjectInfo.AddValue("__keyToNamespaceTable", this.soapHandler.keyToNamespaceTable);
				pr.PRobjectInfo.AddValue("__paramNameList", pr.PRobjectInfo.SetFakeObject());
				if (this.formatterEnums.FEtopObject != null)
				{
					pr.PRobjectInfo.AddValue("__xmlNameSpace", pr.PRxmlNameSpace);
				}
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000AC04 File Offset: 0x00009C04
		private bool IsWhiteSpace(string value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] != ' ' && value[i] != '\n' && value[i] != '\r')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000AC48 File Offset: 0x00009C48
		private void ParseObjectEnd(ParseRecord pr)
		{
			ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
			if (parseRecord == null)
			{
				parseRecord = pr;
			}
			if (parseRecord.PRobjectPositionEnum == InternalObjectPositionE.Top)
			{
				if (parseRecord.PRdtType == Converter.typeofString)
				{
					if (parseRecord.PRvalue == null)
					{
						parseRecord.PRvalue = string.Empty;
					}
					parseRecord.PRnewObj = parseRecord.PRvalue;
					this.CheckSecurity(parseRecord);
					this.isTopObjectResolved = true;
					this.topObject = parseRecord.PRnewObj;
					return;
				}
				if (parseRecord.PRdtType != null && parseRecord.PRvalue != null && !this.IsWhiteSpace(parseRecord.PRvalue) && (parseRecord.PRdtType.IsPrimitive || parseRecord.PRdtType == Converter.typeofTimeSpan))
				{
					parseRecord.PRnewObj = Converter.FromString(parseRecord.PRvalue, Converter.ToCode(parseRecord.PRdtType));
					this.CheckSecurity(parseRecord);
					this.isTopObjectResolved = true;
					this.topObject = parseRecord.PRnewObj;
					return;
				}
				if (!this.isTopObjectResolved && parseRecord.PRdtType != Converter.typeofSoapFault)
				{
					this.topStack.Push(pr.Copy());
					if (parseRecord.PRparseRecordId == pr.PRparseRecordId)
					{
						this.stack.Pop();
					}
					return;
				}
			}
			this.stack.Pop();
			ParseRecord parseRecord2 = (ParseRecord)this.stack.Peek();
			if (parseRecord.PRobjectTypeEnum == InternalObjectTypeE.Array)
			{
				if (parseRecord.PRobjectPositionEnum == InternalObjectPositionE.Top)
				{
					this.isTopObjectResolved = true;
					this.topObject = parseRecord.PRnewObj;
				}
				this.RegisterObject(parseRecord.PRnewObj, parseRecord, parseRecord2);
				return;
			}
			if (parseRecord.PRobjectInfo != null)
			{
				parseRecord.PRobjectInfo.PopulateObjectMembers();
			}
			if (parseRecord.PRnewObj == null)
			{
				if (parseRecord.PRdtType != Converter.typeofString)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ObjectMissing"), new object[] { pr.PRname }));
				}
				if (parseRecord.PRvalue == null)
				{
					parseRecord.PRvalue = string.Empty;
				}
				parseRecord.PRnewObj = parseRecord.PRvalue;
				this.CheckSecurity(parseRecord);
			}
			if (!parseRecord.PRisRegistered && parseRecord.PRobjectId > 0L)
			{
				this.RegisterObject(parseRecord.PRnewObj, parseRecord, parseRecord2);
			}
			if (parseRecord.PRisValueTypeFixup)
			{
				ValueFixup valueFixup = (ValueFixup)this.valueFixupStack.Pop();
				valueFixup.Fixup(parseRecord, parseRecord2);
			}
			if (parseRecord.PRobjectPositionEnum == InternalObjectPositionE.Top)
			{
				this.isTopObjectResolved = true;
				this.topObject = parseRecord.PRnewObj;
			}
			if (parseRecord.PRnewObj is SoapFault)
			{
				this.soapFaultId = parseRecord.PRobjectId;
			}
			if (parseRecord.PRobjectInfo != null)
			{
				if (parseRecord.PRobjectInfo.bfake && !parseRecord.PRobjectInfo.bSoapFault)
				{
					parseRecord.PRobjectInfo.AddValue("__fault", null);
				}
				parseRecord.PRobjectInfo.ObjectEnd();
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000AEF0 File Offset: 0x00009EF0
		private void ParseArray(ParseRecord pr)
		{
			long probjectId = pr.PRobjectId;
			if (probjectId < 1L)
			{
				pr.PRobjectId = this.GetId("GenId-" + this.objectIds);
			}
			if (pr.PRarrayElementType != null && pr.PRarrayElementType.IsEnum)
			{
				pr.PRisEnum = true;
			}
			if (pr.PRarrayTypeEnum == InternalArrayTypeE.Base64)
			{
				if (pr.PRvalue == null)
				{
					pr.PRnewObj = new byte[0];
					this.CheckSecurity(pr);
				}
				else if (pr.PRdtType == Converter.typeofSoapBase64Binary)
				{
					pr.PRnewObj = SoapBase64Binary.Parse(pr.PRvalue);
					this.CheckSecurity(pr);
				}
				else if (pr.PRvalue.Length > 0)
				{
					pr.PRnewObj = Convert.FromBase64String(this.FilterBin64(pr.PRvalue));
					this.CheckSecurity(pr);
				}
				else
				{
					pr.PRnewObj = new byte[0];
					this.CheckSecurity(pr);
				}
				if (this.stack.Peek() == pr)
				{
					this.stack.Pop();
				}
				if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
				{
					this.topObject = pr.PRnewObj;
					this.isTopObjectResolved = true;
				}
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				this.RegisterObject(pr.PRnewObj, pr, parseRecord);
				return;
			}
			if (pr.PRnewObj != null && Converter.IsWriteAsByteArray(pr.PRarrayElementTypeCode))
			{
				if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
				{
					this.topObject = pr.PRnewObj;
					this.isTopObjectResolved = true;
				}
				ParseRecord parseRecord2 = (ParseRecord)this.stack.Peek();
				this.RegisterObject(pr.PRnewObj, pr, parseRecord2);
				return;
			}
			if (pr.PRarrayTypeEnum == InternalArrayTypeE.Jagged || pr.PRarrayTypeEnum == InternalArrayTypeE.Single)
			{
				if (pr.PRlowerBoundA == null || pr.PRlowerBoundA[0] == 0)
				{
					pr.PRnewObj = Array.CreateInstance(pr.PRarrayElementType, (pr.PRrank > 0) ? pr.PRlengthA[0] : 0);
					pr.PRisLowerBound = false;
				}
				else
				{
					pr.PRnewObj = Array.CreateInstance(pr.PRarrayElementType, pr.PRlengthA, pr.PRlowerBoundA);
					pr.PRisLowerBound = true;
				}
				if (pr.PRarrayTypeEnum == InternalArrayTypeE.Single)
				{
					if (!pr.PRisLowerBound && Converter.IsWriteAsByteArray(pr.PRarrayElementTypeCode))
					{
						pr.PRprimitiveArray = new PrimitiveArray(pr.PRarrayElementTypeCode, (Array)pr.PRnewObj);
					}
					else if (!pr.PRarrayElementType.IsValueType && pr.PRlowerBoundA == null)
					{
						pr.PRobjectA = (object[])pr.PRnewObj;
					}
				}
				this.CheckSecurity(pr);
				if (pr.PRobjectPositionEnum == InternalObjectPositionE.Headers)
				{
					this.headers = (Header[])pr.PRnewObj;
				}
				pr.PRindexMap = new int[1];
				return;
			}
			if (pr.PRarrayTypeEnum == InternalArrayTypeE.Rectangular)
			{
				pr.PRisLowerBound = false;
				if (pr.PRlowerBoundA != null)
				{
					for (int i = 0; i < pr.PRrank; i++)
					{
						if (pr.PRlowerBoundA[i] != 0)
						{
							pr.PRisLowerBound = true;
						}
					}
				}
				if (!pr.PRisLowerBound)
				{
					pr.PRnewObj = Array.CreateInstance(pr.PRarrayElementType, pr.PRlengthA);
				}
				else
				{
					pr.PRnewObj = Array.CreateInstance(pr.PRarrayElementType, pr.PRlengthA, pr.PRlowerBoundA);
				}
				this.CheckSecurity(pr);
				int num = 1;
				for (int j = 0; j < pr.PRrank; j++)
				{
					num *= pr.PRlengthA[j];
				}
				pr.PRindexMap = new int[pr.PRrank];
				pr.PRrectangularMap = new int[pr.PRrank];
				pr.PRlinearlength = num;
				return;
			}
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ArrayType"), new object[] { pr.PRarrayTypeEnum.ToString() }));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000B294 File Offset: 0x0000A294
		private void NextRectangleMap(ParseRecord pr)
		{
			for (int i = pr.PRrank - 1; i > -1; i--)
			{
				if (pr.PRrectangularMap[i] < pr.PRlengthA[i] - 1)
				{
					pr.PRrectangularMap[i]++;
					if (i < pr.PRrank - 1)
					{
						for (int j = i + 1; j < pr.PRrank; j++)
						{
							pr.PRrectangularMap[j] = 0;
						}
					}
					Array.Copy(pr.PRrectangularMap, pr.PRindexMap, pr.PRrank);
					return;
				}
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000B320 File Offset: 0x0000A320
		private void ParseArrayMember(ParseRecord pr)
		{
			ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
			if (parseRecord.PRarrayTypeEnum == InternalArrayTypeE.Rectangular)
			{
				if (pr.PRpositionA != null)
				{
					Array.Copy(pr.PRpositionA, parseRecord.PRindexMap, parseRecord.PRindexMap.Length);
					if (parseRecord.PRlowerBoundA == null)
					{
						Array.Copy(pr.PRpositionA, parseRecord.PRrectangularMap, parseRecord.PRrectangularMap.Length);
					}
					else
					{
						for (int i = 0; i < parseRecord.PRrectangularMap.Length; i++)
						{
							parseRecord.PRrectangularMap[i] = pr.PRpositionA[i] - parseRecord.PRlowerBoundA[i];
						}
					}
				}
				else
				{
					if (parseRecord.PRmemberIndex > 0)
					{
						this.NextRectangleMap(parseRecord);
					}
					for (int j = 0; j < parseRecord.PRrank; j++)
					{
						int num = 0;
						if (parseRecord.PRlowerBoundA != null)
						{
							num = parseRecord.PRlowerBoundA[j];
						}
						parseRecord.PRindexMap[j] = parseRecord.PRrectangularMap[j] + num;
					}
				}
			}
			else if (!parseRecord.PRisLowerBound)
			{
				if (pr.PRpositionA == null)
				{
					parseRecord.PRindexMap[0] = parseRecord.PRmemberIndex;
				}
				else
				{
					parseRecord.PRindexMap[0] = (parseRecord.PRmemberIndex = pr.PRpositionA[0]);
				}
			}
			else if (pr.PRpositionA == null)
			{
				parseRecord.PRindexMap[0] = parseRecord.PRmemberIndex + parseRecord.PRlowerBoundA[0];
			}
			else
			{
				parseRecord.PRindexMap[0] = pr.PRpositionA[0];
				parseRecord.PRmemberIndex = pr.PRpositionA[0] - parseRecord.PRlowerBoundA[0];
			}
			if (pr.PRmemberValueEnum == InternalMemberValueE.Reference)
			{
				object @object = this.m_objectManager.GetObject(pr.PRidRef);
				if (@object == null)
				{
					int[] array = new int[parseRecord.PRrank];
					Array.Copy(parseRecord.PRindexMap, 0, array, 0, parseRecord.PRrank);
					this.m_objectManager.RecordArrayElementFixup(parseRecord.PRobjectId, array, pr.PRidRef);
				}
				else if (parseRecord.PRobjectA != null)
				{
					parseRecord.PRobjectA[parseRecord.PRindexMap[0]] = @object;
				}
				else
				{
					((Array)parseRecord.PRnewObj).SetValue(@object, parseRecord.PRindexMap);
				}
			}
			else if (pr.PRmemberValueEnum == InternalMemberValueE.Nested)
			{
				if (pr.PRdtType == null)
				{
					pr.PRdtType = parseRecord.PRarrayElementType;
				}
				this.ParseObject(pr);
				this.stack.Push(pr);
				if (parseRecord.PRarrayElementType.IsValueType && pr.PRarrayElementTypeCode == InternalPrimitiveTypeE.Invalid)
				{
					pr.PRisValueTypeFixup = true;
					this.valueFixupStack.Push(new ValueFixup((Array)parseRecord.PRnewObj, parseRecord.PRindexMap));
				}
				else if (parseRecord.PRobjectA != null)
				{
					parseRecord.PRobjectA[parseRecord.PRindexMap[0]] = pr.PRnewObj;
				}
				else
				{
					((Array)parseRecord.PRnewObj).SetValue(pr.PRnewObj, parseRecord.PRindexMap);
				}
			}
			else if (pr.PRmemberValueEnum == InternalMemberValueE.InlineValue)
			{
				if (parseRecord.PRarrayElementType == Converter.typeofString)
				{
					this.ParseString(pr, parseRecord);
					if (parseRecord.PRobjectA != null)
					{
						parseRecord.PRobjectA[parseRecord.PRindexMap[0]] = pr.PRvalue;
					}
					else
					{
						((Array)parseRecord.PRnewObj).SetValue(pr.PRvalue, parseRecord.PRindexMap);
					}
				}
				else if (parseRecord.PRisEnum)
				{
					object obj = Enum.Parse(parseRecord.PRarrayElementType, pr.PRvalue);
					if (parseRecord.PRobjectA != null)
					{
						parseRecord.PRobjectA[parseRecord.PRindexMap[0]] = (Enum)obj;
					}
					else
					{
						((Array)parseRecord.PRnewObj).SetValue((Enum)obj, parseRecord.PRindexMap);
					}
				}
				else if (parseRecord.PRisArrayVariant)
				{
					if (pr.PRdtType == null && pr.PRkeyDt == null)
					{
						throw new SerializationException(SoapUtil.GetResourceString("Serialization_ArrayTypeObject"));
					}
					object obj2;
					if (pr.PRdtType == Converter.typeofString)
					{
						this.ParseString(pr, parseRecord);
						obj2 = pr.PRvalue;
					}
					else if (pr.PRdtType.IsEnum)
					{
						obj2 = Enum.Parse(pr.PRdtType, pr.PRvalue);
					}
					else if (pr.PRdtTypeCode == InternalPrimitiveTypeE.Invalid)
					{
						this.CheckSerializable(pr.PRdtType);
						if (this.IsRemoting && this.formatterEnums.FEsecurityLevel != TypeFilterLevel.Full)
						{
							obj2 = FormatterServices.GetSafeUninitializedObject(pr.PRdtType);
						}
						else
						{
							obj2 = FormatterServices.GetUninitializedObject(pr.PRdtType);
						}
					}
					else if (pr.PRvarValue != null)
					{
						obj2 = pr.PRvarValue;
					}
					else
					{
						obj2 = Converter.FromString(pr.PRvalue, pr.PRdtTypeCode);
					}
					if (parseRecord.PRobjectA != null)
					{
						parseRecord.PRobjectA[parseRecord.PRindexMap[0]] = obj2;
					}
					else
					{
						((Array)parseRecord.PRnewObj).SetValue(obj2, parseRecord.PRindexMap);
					}
				}
				else if (parseRecord.PRprimitiveArray != null)
				{
					parseRecord.PRprimitiveArray.SetValue(pr.PRvalue, parseRecord.PRindexMap[0]);
				}
				else
				{
					object obj3;
					if (pr.PRvarValue != null)
					{
						obj3 = pr.PRvarValue;
					}
					else
					{
						obj3 = Converter.FromString(pr.PRvalue, parseRecord.PRarrayElementTypeCode);
					}
					if (parseRecord.PRarrayElementTypeCode == InternalPrimitiveTypeE.QName)
					{
						SoapQName soapQName = (SoapQName)obj3;
						if (soapQName.Key.Length == 0)
						{
							soapQName.Namespace = (string)this.soapHandler.keyToNamespaceTable["xmlns"];
						}
						else
						{
							soapQName.Namespace = (string)this.soapHandler.keyToNamespaceTable["xmlns:" + soapQName.Key];
						}
					}
					if (parseRecord.PRobjectA != null)
					{
						parseRecord.PRobjectA[parseRecord.PRindexMap[0]] = obj3;
					}
					else
					{
						((Array)parseRecord.PRnewObj).SetValue(obj3, parseRecord.PRindexMap);
					}
				}
			}
			else if (pr.PRmemberValueEnum != InternalMemberValueE.Null)
			{
				this.ParseError(pr, parseRecord);
			}
			parseRecord.PRmemberIndex++;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000B8D9 File Offset: 0x0000A8D9
		private void ParseArrayMemberEnd(ParseRecord pr)
		{
			if (pr.PRmemberValueEnum == InternalMemberValueE.Nested)
			{
				this.ParseObjectEnd(pr);
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000B8EC File Offset: 0x0000A8EC
		private void ParseMember(ParseRecord pr)
		{
			ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
			if (parseRecord != null)
			{
				string prname = parseRecord.PRname;
			}
			if (parseRecord.PRdtType == Converter.typeofSoapFault && pr.PRname.ToLower(CultureInfo.InvariantCulture) == "faultstring")
			{
				this.faultString = pr.PRvalue;
			}
			if (parseRecord.PRobjectPositionEnum == InternalObjectPositionE.Top && !this.isTopObjectResolved)
			{
				if (pr.PRdtType == Converter.typeofString)
				{
					this.ParseString(pr, parseRecord);
				}
				this.topStack.Push(pr.Copy());
				return;
			}
			switch (pr.PRmemberTypeEnum)
			{
			case InternalMemberTypeE.Item:
				this.ParseArrayMember(pr);
				return;
			}
			if (parseRecord.PRobjectInfo != null)
			{
				parseRecord.PRobjectInfo.AddMemberSeen();
			}
			bool flag = this.IsFakeTopObject && parseRecord.PRobjectPositionEnum == InternalObjectPositionE.Top && parseRecord.PRobjectInfo != null && parseRecord.PRdtType != Converter.typeofSoapFault;
			if (pr.PRdtType == null && parseRecord.PRobjectInfo.isTyped)
			{
				if (flag)
				{
					pr.PRdtType = parseRecord.PRobjectInfo.GetType(this.paramPosition++);
				}
				else
				{
					pr.PRdtType = parseRecord.PRobjectInfo.GetType(pr.PRname);
				}
				if (pr.PRdtType == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_TypeResolved"), new object[] { parseRecord.PRnewObj + " " + pr.PRname }));
				}
				pr.PRdtTypeCode = Converter.ToCode(pr.PRdtType);
			}
			else if (flag)
			{
				this.paramPosition++;
			}
			if (pr.PRmemberValueEnum == InternalMemberValueE.Null)
			{
				parseRecord.PRobjectInfo.AddValue(pr.PRname, null);
				return;
			}
			if (pr.PRmemberValueEnum == InternalMemberValueE.Nested)
			{
				this.ParseObject(pr);
				this.stack.Push(pr);
				if (pr.PRobjectInfo != null && pr.PRobjectInfo.objectType.IsValueType)
				{
					if (this.IsFakeTopObject)
					{
						parseRecord.PRobjectInfo.AddParamName(pr.PRname);
					}
					pr.PRisValueTypeFixup = true;
					this.valueFixupStack.Push(new ValueFixup(parseRecord.PRnewObj, pr.PRname, parseRecord.PRobjectInfo));
					return;
				}
				parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRnewObj);
				return;
			}
			else
			{
				if (pr.PRmemberValueEnum != InternalMemberValueE.Reference)
				{
					if (pr.PRmemberValueEnum == InternalMemberValueE.InlineValue)
					{
						if (pr.PRdtType == Converter.typeofString)
						{
							this.ParseString(pr, parseRecord);
							parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRvalue);
							return;
						}
						if (pr.PRdtTypeCode != InternalPrimitiveTypeE.Invalid)
						{
							object obj;
							if (pr.PRvarValue != null)
							{
								obj = pr.PRvarValue;
							}
							else
							{
								obj = Converter.FromString(pr.PRvalue, pr.PRdtTypeCode);
							}
							if (pr.PRdtTypeCode == InternalPrimitiveTypeE.QName && obj != null)
							{
								SoapQName soapQName = (SoapQName)obj;
								if (soapQName.Key != null)
								{
									if (soapQName.Key.Length == 0)
									{
										soapQName.Namespace = (string)this.soapHandler.keyToNamespaceTable["xmlns"];
									}
									else
									{
										soapQName.Namespace = (string)this.soapHandler.keyToNamespaceTable["xmlns:" + soapQName.Key];
									}
								}
							}
							parseRecord.PRobjectInfo.AddValue(pr.PRname, obj);
							return;
						}
						if (pr.PRarrayTypeEnum == InternalArrayTypeE.Base64)
						{
							parseRecord.PRobjectInfo.AddValue(pr.PRname, Convert.FromBase64String(this.FilterBin64(pr.PRvalue)));
							return;
						}
						if (pr.PRdtType == Converter.typeofObject && pr.PRvalue != null)
						{
							if (parseRecord != null && parseRecord.PRdtType == Converter.typeofHeader)
							{
								pr.PRdtType = Converter.typeofString;
								this.ParseString(pr, parseRecord);
								parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRvalue);
								return;
							}
						}
						else
						{
							if (pr.PRdtType != null && pr.PRdtType.IsEnum)
							{
								object obj2 = Enum.Parse(pr.PRdtType, pr.PRvalue);
								parseRecord.PRobjectInfo.AddValue(pr.PRname, obj2);
								return;
							}
							if (pr.PRdtType != null && pr.PRdtType == Converter.typeofTypeArray)
							{
								parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRvarValue);
								return;
							}
							if (!pr.PRisRegistered && pr.PRobjectId > 0L)
							{
								if (pr.PRvalue == null)
								{
									pr.PRvalue = "";
								}
								this.RegisterObject(pr.PRvalue, pr, parseRecord);
							}
							if (pr.PRdtType == Converter.typeofSystemVoid)
							{
								parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRdtType);
								return;
							}
							if (parseRecord.PRobjectInfo.isSi)
							{
								parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRvalue);
								return;
							}
						}
					}
					else
					{
						this.ParseError(pr, parseRecord);
					}
					return;
				}
				object @object = this.m_objectManager.GetObject(pr.PRidRef);
				if (@object == null)
				{
					parseRecord.PRobjectInfo.AddValue(pr.PRname, null);
					parseRecord.PRobjectInfo.RecordFixup(parseRecord.PRobjectId, pr.PRname, pr.PRidRef);
					return;
				}
				parseRecord.PRobjectInfo.AddValue(pr.PRname, @object);
				return;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000BE2C File Offset: 0x0000AE2C
		private void ParseMemberEnd(ParseRecord pr)
		{
			switch (pr.PRmemberTypeEnum)
			{
			case InternalMemberTypeE.Field:
				if (pr.PRmemberValueEnum == InternalMemberValueE.Nested)
				{
					this.ParseObjectEnd(pr);
					return;
				}
				break;
			case InternalMemberTypeE.Item:
				this.ParseArrayMemberEnd(pr);
				return;
			default:
				if (pr.PRmemberValueEnum == InternalMemberValueE.Nested)
				{
					this.ParseObjectEnd(pr);
					return;
				}
				this.ParseError(pr, (ParseRecord)this.stack.Peek());
				break;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000BE93 File Offset: 0x0000AE93
		private void ParseString(ParseRecord pr, ParseRecord parentPr)
		{
			if (pr.PRvalue == null)
			{
				pr.PRvalue = "";
			}
			if (!pr.PRisRegistered && pr.PRobjectId > 0L)
			{
				this.RegisterObject(pr.PRvalue, pr, parentPr);
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000BEC8 File Offset: 0x0000AEC8
		private void RegisterObject(object obj, ParseRecord pr, ParseRecord objectPr)
		{
			if (!pr.PRisRegistered)
			{
				pr.PRisRegistered = true;
				SerializationInfo serializationInfo = null;
				long num = 0L;
				MemberInfo memberInfo = null;
				int[] array = null;
				if (objectPr != null)
				{
					array = objectPr.PRindexMap;
					num = objectPr.PRobjectId;
					if (objectPr.PRobjectInfo != null && !objectPr.PRobjectInfo.isSi)
					{
						memberInfo = objectPr.PRobjectInfo.GetMemberInfo(pr.PRname);
					}
				}
				if (pr.PRobjectInfo != null)
				{
					serializationInfo = pr.PRobjectInfo.si;
				}
				this.m_objectManager.RegisterObject(obj, pr.PRobjectId, serializationInfo, num, memberInfo, array);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000BF4F File Offset: 0x0000AF4F
		internal void SetVersion(int major, int minor)
		{
			if (this.formatterEnums.FEassemblyFormat != FormatterAssemblyStyle.Simple)
			{
				this.majorVersion = major;
				this.minorVersion = minor;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000BF6C File Offset: 0x0000AF6C
		internal long GetId(string keyId)
		{
			if (keyId == null)
			{
				throw new ArgumentNullException("keyId", string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("ArgumentNull_WithParamName"), new object[] { "keyId" }));
			}
			if (keyId != this.inKeyId)
			{
				this.inKeyId = keyId;
				string text;
				if (keyId[0] == '#')
				{
					text = keyId.Substring(1);
				}
				else
				{
					text = keyId;
				}
				object obj = this.objectIdTable[text];
				if (obj == null)
				{
					this.outKeyId = (this.objectIds += 1L);
					this.objectIdTable[text] = this.outKeyId;
				}
				else
				{
					this.outKeyId = (long)obj;
				}
			}
			return this.outKeyId;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000C02C File Offset: 0x0000B02C
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

		// Token: 0x060000F0 RID: 240 RVA: 0x0000C088 File Offset: 0x0000B088
		internal Assembly LoadAssemblyFromString(string assemblyString)
		{
			Assembly assembly = null;
			if (this.formatterEnums.FEassemblyFormat == FormatterAssemblyStyle.Simple)
			{
				try
				{
					ObjectReader.sfileIOPermission.Assert();
					try
					{
						assembly = Assembly.LoadWithPartialName(assemblyString, null);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					return assembly;
				}
				catch (Exception)
				{
					return assembly;
				}
				catch
				{
					return assembly;
				}
			}
			try
			{
				ObjectReader.sfileIOPermission.Assert();
				try
				{
					assembly = Assembly.Load(assemblyString);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			catch (Exception)
			{
			}
			catch
			{
			}
			return assembly;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000C134 File Offset: 0x0000B134
		internal Type Bind(string assemblyString, string typeString)
		{
			Type type = null;
			if (this.m_binder != null && !this.IsInternalType(assemblyString, typeString))
			{
				type = this.m_binder.BindToType(assemblyString, typeString);
			}
			return type;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000C164 File Offset: 0x0000B164
		private bool IsInternalType(string assemblyString, string typeString)
		{
			return assemblyString == Converter.urtAssemblyString && (typeString == "System.DelegateSerializationHolder" || typeString == "System.UnitySerializationHolder" || typeString == "System.MemberInfoSerializationHolder");
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000C19C File Offset: 0x0000B19C
		internal Type FastBindToType(string assemblyName, string typeName)
		{
			ObjectReader.TypeNAssembly typeNAssembly = this.typeCache.GetCachedValue(typeName) as ObjectReader.TypeNAssembly;
			if (typeNAssembly == null || typeNAssembly.assemblyName != assemblyName)
			{
				Assembly assembly = this.LoadAssemblyFromString(assemblyName);
				if (assembly == null)
				{
					return null;
				}
				Type typeFromAssembly = FormatterServices.GetTypeFromAssembly(assembly, typeName);
				if (typeFromAssembly == null)
				{
					return null;
				}
				typeNAssembly = new ObjectReader.TypeNAssembly();
				typeNAssembly.type = typeFromAssembly;
				typeNAssembly.assemblyName = assemblyName;
				this.typeCache.SetCachedValue(typeNAssembly);
			}
			return typeNAssembly.type;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000C210 File Offset: 0x0000B210
		internal string FilterBin64(string value)
		{
			this.sbf.Length = 0;
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] != ' ' && value[i] != '\n' && value[i] != '\r')
				{
					this.sbf.Append(value[i]);
				}
			}
			return this.sbf.ToString();
		}

		// Token: 0x040001A3 RID: 419
		internal ObjectIDGenerator m_idGenerator;

		// Token: 0x040001A4 RID: 420
		internal Stream m_stream;

		// Token: 0x040001A5 RID: 421
		internal ISurrogateSelector m_surrogates;

		// Token: 0x040001A6 RID: 422
		internal StreamingContext m_context;

		// Token: 0x040001A7 RID: 423
		internal ObjectManager m_objectManager;

		// Token: 0x040001A8 RID: 424
		internal InternalFE formatterEnums;

		// Token: 0x040001A9 RID: 425
		internal SerializationBinder m_binder;

		// Token: 0x040001AA RID: 426
		internal SoapHandler soapHandler;

		// Token: 0x040001AB RID: 427
		internal long topId;

		// Token: 0x040001AC RID: 428
		internal SerStack topStack;

		// Token: 0x040001AD RID: 429
		internal bool isTopObjectSecondPass;

		// Token: 0x040001AE RID: 430
		internal bool isTopObjectResolved = true;

		// Token: 0x040001AF RID: 431
		internal bool isHeaderHandlerCalled;

		// Token: 0x040001B0 RID: 432
		internal Exception deserializationSecurityException;

		// Token: 0x040001B1 RID: 433
		internal object handlerObject;

		// Token: 0x040001B2 RID: 434
		internal object topObject;

		// Token: 0x040001B3 RID: 435
		internal long soapFaultId;

		// Token: 0x040001B4 RID: 436
		internal Header[] headers;

		// Token: 0x040001B5 RID: 437
		internal Header[] newheaders;

		// Token: 0x040001B6 RID: 438
		internal bool IsFakeTopObject;

		// Token: 0x040001B7 RID: 439
		internal HeaderHandler handler;

		// Token: 0x040001B8 RID: 440
		internal SerObjectInfoInit serObjectInfoInit;

		// Token: 0x040001B9 RID: 441
		internal IFormatterConverter m_formatterConverter;

		// Token: 0x040001BA RID: 442
		internal SerStack stack = new SerStack("ObjectReader Object Stack");

		// Token: 0x040001BB RID: 443
		internal SerStack valueFixupStack = new SerStack("ValueType Fixup Stack");

		// Token: 0x040001BC RID: 444
		internal Hashtable objectIdTable = new Hashtable(25);

		// Token: 0x040001BD RID: 445
		internal long objectIds;

		// Token: 0x040001BE RID: 446
		internal int paramPosition;

		// Token: 0x040001BF RID: 447
		internal int majorVersion;

		// Token: 0x040001C0 RID: 448
		internal int minorVersion;

		// Token: 0x040001C1 RID: 449
		internal string faultString;

		// Token: 0x040001C2 RID: 450
		internal static SecurityPermission serializationPermission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);

		// Token: 0x040001C3 RID: 451
		private static FileIOPermission sfileIOPermission = new FileIOPermission(PermissionState.Unrestricted);

		// Token: 0x040001C4 RID: 452
		private string inKeyId;

		// Token: 0x040001C5 RID: 453
		private long outKeyId;

		// Token: 0x040001C6 RID: 454
		private NameCache typeCache = new NameCache();

		// Token: 0x040001C7 RID: 455
		private StringBuilder sbf = new StringBuilder();

		// Token: 0x0200002A RID: 42
		internal class TypeNAssembly
		{
			// Token: 0x040001C8 RID: 456
			public Type type;

			// Token: 0x040001C9 RID: 457
			public string assemblyName;
		}
	}
}
