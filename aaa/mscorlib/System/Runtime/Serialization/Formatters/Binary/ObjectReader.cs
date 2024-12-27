using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007DE RID: 2014
	internal sealed class ObjectReader
	{
		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x0600477A RID: 18298 RVA: 0x000F6058 File Offset: 0x000F5058
		private SerStack ValueFixupStack
		{
			get
			{
				if (this.valueFixupStack == null)
				{
					this.valueFixupStack = new SerStack("ValueType Fixup Stack");
				}
				return this.valueFixupStack;
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x0600477B RID: 18299 RVA: 0x000F6078 File Offset: 0x000F5078
		// (set) Token: 0x0600477C RID: 18300 RVA: 0x000F6080 File Offset: 0x000F5080
		internal object TopObject
		{
			get
			{
				return this.m_topObject;
			}
			set
			{
				this.m_topObject = value;
				if (this.m_objectManager != null)
				{
					this.m_objectManager.TopObject = value;
				}
			}
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x000F609D File Offset: 0x000F509D
		internal void SetMethodCall(BinaryMethodCall binaryMethodCall)
		{
			this.bMethodCall = true;
			this.binaryMethodCall = binaryMethodCall;
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x000F60AD File Offset: 0x000F50AD
		internal void SetMethodReturn(BinaryMethodReturn binaryMethodReturn)
		{
			this.bMethodReturn = true;
			this.binaryMethodReturn = binaryMethodReturn;
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x000F60C0 File Offset: 0x000F50C0
		internal ObjectReader(Stream stream, ISurrogateSelector selector, StreamingContext context, InternalFE formatterEnums, SerializationBinder binder)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream", Environment.GetResourceString("ArgumentNull_Stream"));
			}
			this.m_stream = stream;
			this.m_surrogates = selector;
			this.m_context = context;
			this.m_binder = binder;
			if (this.m_binder != null)
			{
				ResourceReader.TypeLimitingDeserializationBinder typeLimitingDeserializationBinder = this.m_binder as ResourceReader.TypeLimitingDeserializationBinder;
				if (typeLimitingDeserializationBinder != null)
				{
					typeLimitingDeserializationBinder.ObjectReader = this;
				}
			}
			this.formatterEnums = formatterEnums;
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x000F613C File Offset: 0x000F513C
		internal object Deserialize(HeaderHandler handler, __BinaryParser serParser, bool fCheck, bool isCrossAppDomain, IMethodCallMessage methodCallMessage)
		{
			this.bFullDeserialization = false;
			this.bMethodCall = false;
			this.bMethodReturn = false;
			this.TopObject = null;
			this.topId = 0L;
			this.bIsCrossAppDomain = isCrossAppDomain;
			this.bSimpleAssembly = this.formatterEnums.FEassemblyFormat == FormatterAssemblyStyle.Simple;
			if (serParser == null)
			{
				throw new ArgumentNullException("serParser", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_WithParamName"), new object[] { serParser }));
			}
			if (fCheck)
			{
				CodeAccessPermission.DemandInternal(PermissionType.SecuritySerialization);
			}
			this.handler = handler;
			if (this.bFullDeserialization)
			{
				this.m_objectManager = new ObjectManager(this.m_surrogates, this.m_context, false, this.bIsCrossAppDomain);
				this.serObjectInfoInit = new SerObjectInfoInit();
			}
			serParser.Run();
			if (this.bFullDeserialization)
			{
				this.m_objectManager.DoFixups();
			}
			if (!this.bMethodCall && !this.bMethodReturn)
			{
				if (this.TopObject == null)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_TopObject"));
				}
				if (this.HasSurrogate(this.TopObject.GetType()) && this.topId != 0L)
				{
					this.TopObject = this.m_objectManager.GetObject(this.topId);
				}
				if (this.TopObject is IObjectReference)
				{
					this.TopObject = ((IObjectReference)this.TopObject).GetRealObject(this.m_context);
				}
			}
			if (this.bFullDeserialization)
			{
				this.m_objectManager.RaiseDeserializationEvent();
			}
			if (handler != null)
			{
				this.handlerObject = handler(this.headers);
			}
			if (this.bMethodCall)
			{
				object[] array = this.TopObject as object[];
				this.TopObject = this.binaryMethodCall.ReadArray(array, this.handlerObject);
			}
			else if (this.bMethodReturn)
			{
				object[] array2 = this.TopObject as object[];
				this.TopObject = this.binaryMethodReturn.ReadArray(array2, methodCallMessage, this.handlerObject);
			}
			return this.TopObject;
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x000F6320 File Offset: 0x000F5320
		private bool HasSurrogate(Type t)
		{
			ISurrogateSelector surrogateSelector;
			return this.m_surrogates != null && this.m_surrogates.GetSurrogate(t, this.m_context, out surrogateSelector) != null;
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x000F6354 File Offset: 0x000F5354
		private void CheckSerializable(Type t)
		{
			if (!t.IsSerializable && !this.HasSurrogate(t))
			{
				throw new SerializationException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Serialization_NonSerType"), new object[]
				{
					t.FullName,
					t.Assembly.FullName
				}));
			}
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x000F63AC File Offset: 0x000F53AC
		private void InitFullDeserialization()
		{
			this.bFullDeserialization = true;
			this.stack = new SerStack("ObjectReader Object Stack");
			this.m_objectManager = new ObjectManager(this.m_surrogates, this.m_context, false, this.bIsCrossAppDomain);
			if (this.m_formatterConverter == null)
			{
				this.m_formatterConverter = new FormatterConverter();
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x000F6401 File Offset: 0x000F5401
		internal object CrossAppDomainArray(int index)
		{
			return this.crossAppDomainArray[index];
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x000F640B File Offset: 0x000F540B
		internal ReadObjectInfo CreateReadObjectInfo(Type objectType)
		{
			return ReadObjectInfo.Create(objectType, this.m_surrogates, this.m_context, this.m_objectManager, this.serObjectInfoInit, this.m_formatterConverter, this.bSimpleAssembly);
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x000F6438 File Offset: 0x000F5438
		internal ReadObjectInfo CreateReadObjectInfo(Type objectType, string[] memberNames, Type[] memberTypes)
		{
			return ReadObjectInfo.Create(objectType, memberNames, memberTypes, this.m_surrogates, this.m_context, this.m_objectManager, this.serObjectInfoInit, this.m_formatterConverter, this.bSimpleAssembly);
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x000F6474 File Offset: 0x000F5474
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
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_XMLElement"), new object[] { pr.PRname }));
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x000F6520 File Offset: 0x000F5520
		private void ParseError(ParseRecord processing, ParseRecord onStack)
		{
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ParseError"), new object[] { string.Concat(new object[] { onStack.PRname, " ", onStack.PRparseTypeEnum, " ", processing.PRname, " ", processing.PRparseTypeEnum }) }));
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x000F65A4 File Offset: 0x000F55A4
		private void ParseSerializedStreamHeader(ParseRecord pr)
		{
			this.stack.Push(pr);
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x000F65B2 File Offset: 0x000F55B2
		private void ParseSerializedStreamHeaderEnd(ParseRecord pr)
		{
			this.stack.Pop();
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x0600478B RID: 18315 RVA: 0x000F65C0 File Offset: 0x000F55C0
		private bool IsRemoting
		{
			get
			{
				return this.bMethodCall || this.bMethodReturn;
			}
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x000F65D4 File Offset: 0x000F55D4
		internal void CheckSecurity(ParseRecord pr)
		{
			Type prdtType = pr.PRdtType;
			if (prdtType != null && this.IsRemoting)
			{
				if (typeof(MarshalByRefObject).IsAssignableFrom(prdtType))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_MBRAsMBV"), new object[] { prdtType.FullName }));
				}
				FormatterServices.CheckTypeSecurity(prdtType, this.formatterEnums.FEsecurityLevel);
			}
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x000F6644 File Offset: 0x000F5644
		private void ParseObject(ParseRecord pr)
		{
			if (!this.bFullDeserialization)
			{
				this.InitFullDeserialization();
			}
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
			if (pr.PRdtType == null)
			{
				pr.PRnewObj = new TypeLoadExceptionHolder(pr.PRkeyDt);
				return;
			}
			if (pr.PRdtType == Converter.typeofString)
			{
				if (pr.PRvalue != null)
				{
					pr.PRnewObj = pr.PRvalue;
					if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
					{
						this.TopObject = pr.PRnewObj;
						return;
					}
					this.stack.Pop();
					this.RegisterObject(pr.PRnewObj, pr, (ParseRecord)this.stack.Peek());
				}
				return;
			}
			this.CheckSerializable(pr.PRdtType);
			if (this.IsRemoting && this.formatterEnums.FEsecurityLevel != TypeFilterLevel.Full)
			{
				pr.PRnewObj = FormatterServices.GetSafeUninitializedObject(pr.PRdtType);
			}
			else
			{
				pr.PRnewObj = FormatterServices.GetUninitializedObject(pr.PRdtType);
			}
			this.m_objectManager.RaiseOnDeserializingEvent(pr.PRnewObj);
			if (pr.PRnewObj == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_TopObjectInstantiate"), new object[] { pr.PRdtType }));
			}
			if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
			{
				this.TopObject = pr.PRnewObj;
			}
			if (pr.PRobjectInfo == null)
			{
				pr.PRobjectInfo = ReadObjectInfo.Create(pr.PRdtType, this.m_surrogates, this.m_context, this.m_objectManager, this.serObjectInfoInit, this.m_formatterConverter, this.bSimpleAssembly);
			}
			this.CheckSecurity(pr);
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x000F67F8 File Offset: 0x000F57F8
		private void ParseObjectEnd(ParseRecord pr)
		{
			ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
			if (parseRecord == null)
			{
				parseRecord = pr;
			}
			if (parseRecord.PRobjectPositionEnum == InternalObjectPositionE.Top && parseRecord.PRdtType == Converter.typeofString)
			{
				parseRecord.PRnewObj = parseRecord.PRvalue;
				this.TopObject = parseRecord.PRnewObj;
				return;
			}
			this.stack.Pop();
			ParseRecord parseRecord2 = (ParseRecord)this.stack.Peek();
			if (parseRecord.PRnewObj == null)
			{
				return;
			}
			if (parseRecord.PRobjectTypeEnum == InternalObjectTypeE.Array)
			{
				if (parseRecord.PRobjectPositionEnum == InternalObjectPositionE.Top)
				{
					this.TopObject = parseRecord.PRnewObj;
				}
				this.RegisterObject(parseRecord.PRnewObj, parseRecord, parseRecord2);
				return;
			}
			parseRecord.PRobjectInfo.PopulateObjectMembers(parseRecord.PRnewObj, parseRecord.PRmemberData);
			if (!parseRecord.PRisRegistered && parseRecord.PRobjectId > 0L)
			{
				this.RegisterObject(parseRecord.PRnewObj, parseRecord, parseRecord2);
			}
			if (parseRecord.PRisValueTypeFixup)
			{
				ValueFixup valueFixup = (ValueFixup)this.ValueFixupStack.Pop();
				valueFixup.Fixup(parseRecord, parseRecord2);
			}
			if (parseRecord.PRobjectPositionEnum == InternalObjectPositionE.Top)
			{
				this.TopObject = parseRecord.PRnewObj;
			}
			parseRecord.PRobjectInfo.ObjectEnd();
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x000F6918 File Offset: 0x000F5918
		private void ParseArray(ParseRecord pr)
		{
			long probjectId = pr.PRobjectId;
			if (pr.PRarrayTypeEnum == InternalArrayTypeE.Base64)
			{
				if (pr.PRvalue.Length > 0)
				{
					pr.PRnewObj = Convert.FromBase64String(pr.PRvalue);
				}
				else
				{
					pr.PRnewObj = new byte[0];
				}
				if (this.stack.Peek() == pr)
				{
					this.stack.Pop();
				}
				if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
				{
					this.TopObject = pr.PRnewObj;
				}
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				this.RegisterObject(pr.PRnewObj, pr, parseRecord);
			}
			else if (pr.PRnewObj != null && Converter.IsWriteAsByteArray(pr.PRarrayElementTypeCode))
			{
				if (pr.PRobjectPositionEnum == InternalObjectPositionE.Top)
				{
					this.TopObject = pr.PRnewObj;
				}
				ParseRecord parseRecord2 = (ParseRecord)this.stack.Peek();
				this.RegisterObject(pr.PRnewObj, pr, parseRecord2);
			}
			else if (pr.PRarrayTypeEnum == InternalArrayTypeE.Jagged || pr.PRarrayTypeEnum == InternalArrayTypeE.Single)
			{
				bool flag = true;
				if (pr.PRlowerBoundA == null || pr.PRlowerBoundA[0] == 0)
				{
					if (pr.PRarrayElementType == Converter.typeofString)
					{
						pr.PRobjectA = new string[pr.PRlengthA[0]];
						pr.PRnewObj = pr.PRobjectA;
						flag = false;
					}
					else if (pr.PRarrayElementType == Converter.typeofObject)
					{
						pr.PRobjectA = new object[pr.PRlengthA[0]];
						pr.PRnewObj = pr.PRobjectA;
						flag = false;
					}
					else if (pr.PRarrayElementType != null)
					{
						pr.PRnewObj = Array.CreateInstance(pr.PRarrayElementType, pr.PRlengthA[0]);
					}
					pr.PRisLowerBound = false;
				}
				else
				{
					if (pr.PRarrayElementType != null)
					{
						pr.PRnewObj = Array.CreateInstance(pr.PRarrayElementType, pr.PRlengthA, pr.PRlowerBoundA);
					}
					pr.PRisLowerBound = true;
				}
				if (pr.PRarrayTypeEnum == InternalArrayTypeE.Single)
				{
					if (!pr.PRisLowerBound && Converter.IsWriteAsByteArray(pr.PRarrayElementTypeCode))
					{
						pr.PRprimitiveArray = new PrimitiveArray(pr.PRarrayElementTypeCode, (Array)pr.PRnewObj);
					}
					else if (flag && pr.PRarrayElementType != null && !pr.PRarrayElementType.IsValueType && !pr.PRisLowerBound)
					{
						pr.PRobjectA = (object[])pr.PRnewObj;
					}
				}
				if (pr.PRobjectPositionEnum == InternalObjectPositionE.Headers)
				{
					this.headers = (Header[])pr.PRnewObj;
				}
				pr.PRindexMap = new int[1];
			}
			else
			{
				if (pr.PRarrayTypeEnum != InternalArrayTypeE.Rectangular)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ArrayType"), new object[] { pr.PRarrayTypeEnum }));
				}
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
				if (pr.PRarrayElementType != null)
				{
					if (!pr.PRisLowerBound)
					{
						pr.PRnewObj = Array.CreateInstance(pr.PRarrayElementType, pr.PRlengthA);
					}
					else
					{
						pr.PRnewObj = Array.CreateInstance(pr.PRarrayElementType, pr.PRlengthA, pr.PRlowerBoundA);
					}
				}
				int num = 1;
				for (int j = 0; j < pr.PRrank; j++)
				{
					num *= pr.PRlengthA[j];
				}
				pr.PRindexMap = new int[pr.PRrank];
				pr.PRrectangularMap = new int[pr.PRrank];
				pr.PRlinearlength = num;
			}
			this.CheckSecurity(pr);
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x000F6C90 File Offset: 0x000F5C90
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

		// Token: 0x06004791 RID: 18321 RVA: 0x000F6D1C File Offset: 0x000F5D1C
		private void ParseArrayMember(ParseRecord pr)
		{
			ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
			if (parseRecord.PRarrayTypeEnum == InternalArrayTypeE.Rectangular)
			{
				if (parseRecord.PRmemberIndex > 0)
				{
					this.NextRectangleMap(parseRecord);
				}
				if (parseRecord.PRisLowerBound)
				{
					for (int i = 0; i < parseRecord.PRrank; i++)
					{
						parseRecord.PRindexMap[i] = parseRecord.PRrectangularMap[i] + parseRecord.PRlowerBoundA[i];
					}
				}
			}
			else if (!parseRecord.PRisLowerBound)
			{
				parseRecord.PRindexMap[0] = parseRecord.PRmemberIndex;
			}
			else
			{
				parseRecord.PRindexMap[0] = parseRecord.PRlowerBoundA[0] + parseRecord.PRmemberIndex;
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
				if (parseRecord.PRarrayElementType != null)
				{
					if (parseRecord.PRarrayElementType.IsValueType && pr.PRarrayElementTypeCode == InternalPrimitiveTypeE.Invalid)
					{
						pr.PRisValueTypeFixup = true;
						this.ValueFixupStack.Push(new ValueFixup((Array)parseRecord.PRnewObj, parseRecord.PRindexMap));
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
			}
			else if (pr.PRmemberValueEnum == InternalMemberValueE.InlineValue)
			{
				if (parseRecord.PRarrayElementType == Converter.typeofString || pr.PRdtType == Converter.typeofString)
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
				else if (parseRecord.PRisArrayVariant)
				{
					if (pr.PRkeyDt == null)
					{
						throw new SerializationException(Environment.GetResourceString("Serialization_ArrayTypeObject"));
					}
					object obj;
					if (pr.PRdtType == Converter.typeofString)
					{
						this.ParseString(pr, parseRecord);
						obj = pr.PRvalue;
					}
					else if (pr.PRdtTypeCode == InternalPrimitiveTypeE.Invalid)
					{
						this.CheckSerializable(pr.PRdtType);
						if (this.IsRemoting && this.formatterEnums.FEsecurityLevel != TypeFilterLevel.Full)
						{
							obj = FormatterServices.GetSafeUninitializedObject(pr.PRdtType);
						}
						else
						{
							obj = FormatterServices.GetUninitializedObject(pr.PRdtType);
						}
					}
					else if (pr.PRvarValue != null)
					{
						obj = pr.PRvarValue;
					}
					else
					{
						obj = Converter.FromString(pr.PRvalue, pr.PRdtTypeCode);
					}
					if (parseRecord.PRobjectA != null)
					{
						parseRecord.PRobjectA[parseRecord.PRindexMap[0]] = obj;
					}
					else
					{
						((Array)parseRecord.PRnewObj).SetValue(obj, parseRecord.PRindexMap);
					}
				}
				else if (parseRecord.PRprimitiveArray != null)
				{
					parseRecord.PRprimitiveArray.SetValue(pr.PRvalue, parseRecord.PRindexMap[0]);
				}
				else
				{
					object obj2;
					if (pr.PRvarValue != null)
					{
						obj2 = pr.PRvarValue;
					}
					else
					{
						obj2 = Converter.FromString(pr.PRvalue, parseRecord.PRarrayElementTypeCode);
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
			}
			else if (pr.PRmemberValueEnum == InternalMemberValueE.Null)
			{
				parseRecord.PRmemberIndex += pr.PRnullCount - 1;
			}
			else
			{
				this.ParseError(pr, parseRecord);
			}
			parseRecord.PRmemberIndex++;
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x000F7127 File Offset: 0x000F6127
		private void ParseArrayMemberEnd(ParseRecord pr)
		{
			if (pr.PRmemberValueEnum == InternalMemberValueE.Nested)
			{
				this.ParseObjectEnd(pr);
			}
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x000F713C File Offset: 0x000F613C
		private void ParseMember(ParseRecord pr)
		{
			ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
			if (parseRecord != null)
			{
				string prname = parseRecord.PRname;
			}
			switch (pr.PRmemberTypeEnum)
			{
			case InternalMemberTypeE.Item:
				this.ParseArrayMember(pr);
				return;
			}
			if (pr.PRdtType == null && parseRecord.PRobjectInfo.isTyped)
			{
				pr.PRdtType = parseRecord.PRobjectInfo.GetType(pr.PRname);
				if (pr.PRdtType != null)
				{
					pr.PRdtTypeCode = Converter.ToCode(pr.PRdtType);
				}
			}
			if (pr.PRmemberValueEnum == InternalMemberValueE.Null)
			{
				parseRecord.PRobjectInfo.AddValue(pr.PRname, null, ref parseRecord.PRsi, ref parseRecord.PRmemberData);
				return;
			}
			if (pr.PRmemberValueEnum == InternalMemberValueE.Nested)
			{
				this.ParseObject(pr);
				this.stack.Push(pr);
				if (pr.PRobjectInfo != null && pr.PRobjectInfo.objectType != null && pr.PRobjectInfo.objectType.IsValueType)
				{
					pr.PRisValueTypeFixup = true;
					this.ValueFixupStack.Push(new ValueFixup(parseRecord.PRnewObj, pr.PRname, parseRecord.PRobjectInfo));
					return;
				}
				parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRnewObj, ref parseRecord.PRsi, ref parseRecord.PRmemberData);
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
							parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRvalue, ref parseRecord.PRsi, ref parseRecord.PRmemberData);
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
							parseRecord.PRobjectInfo.AddValue(pr.PRname, obj, ref parseRecord.PRsi, ref parseRecord.PRmemberData);
							return;
						}
						if (pr.PRarrayTypeEnum == InternalArrayTypeE.Base64)
						{
							parseRecord.PRobjectInfo.AddValue(pr.PRname, Convert.FromBase64String(pr.PRvalue), ref parseRecord.PRsi, ref parseRecord.PRmemberData);
							return;
						}
						if (pr.PRdtType == Converter.typeofObject)
						{
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_TypeMissing"), new object[] { pr.PRname }));
						}
						this.ParseString(pr, parseRecord);
						if (pr.PRdtType == Converter.typeofSystemVoid)
						{
							parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRdtType, ref parseRecord.PRsi, ref parseRecord.PRmemberData);
							return;
						}
						if (parseRecord.PRobjectInfo.isSi)
						{
							parseRecord.PRobjectInfo.AddValue(pr.PRname, pr.PRvalue, ref parseRecord.PRsi, ref parseRecord.PRmemberData);
							return;
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
					parseRecord.PRobjectInfo.AddValue(pr.PRname, null, ref parseRecord.PRsi, ref parseRecord.PRmemberData);
					parseRecord.PRobjectInfo.RecordFixup(parseRecord.PRobjectId, pr.PRname, pr.PRidRef);
					return;
				}
				parseRecord.PRobjectInfo.AddValue(pr.PRname, @object, ref parseRecord.PRsi, ref parseRecord.PRmemberData);
				return;
			}
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x000F7474 File Offset: 0x000F6474
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
				this.ParseError(pr, (ParseRecord)this.stack.Peek());
				break;
			}
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x000F74CA File Offset: 0x000F64CA
		private void ParseString(ParseRecord pr, ParseRecord parentPr)
		{
			if (!pr.PRisRegistered && pr.PRobjectId > 0L)
			{
				this.RegisterObject(pr.PRvalue, pr, parentPr, true);
			}
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x000F74ED File Offset: 0x000F64ED
		private void RegisterObject(object obj, ParseRecord pr, ParseRecord objectPr)
		{
			this.RegisterObject(obj, pr, objectPr, false);
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x000F74FC File Offset: 0x000F64FC
		private void RegisterObject(object obj, ParseRecord pr, ParseRecord objectPr, bool bIsString)
		{
			if (!pr.PRisRegistered)
			{
				pr.PRisRegistered = true;
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
				SerializationInfo prsi = pr.PRsi;
				if (bIsString)
				{
					this.m_objectManager.RegisterString((string)obj, pr.PRobjectId, prsi, num, memberInfo);
					return;
				}
				this.m_objectManager.RegisterObject(obj, pr.PRobjectId, prsi, num, memberInfo, array);
			}
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x000F7598 File Offset: 0x000F6598
		internal long GetId(long objectId)
		{
			if (!this.bFullDeserialization)
			{
				this.InitFullDeserialization();
			}
			if (objectId > 0L)
			{
				return objectId;
			}
			if (this.bOldFormatDetected || objectId == -1L)
			{
				this.bOldFormatDetected = true;
				if (this.valTypeObjectIdTable == null)
				{
					this.valTypeObjectIdTable = new IntSizedArray();
				}
				long num;
				if ((num = (long)this.valTypeObjectIdTable[(int)objectId]) == 0L)
				{
					num = 2147483647L + objectId;
					this.valTypeObjectIdTable[(int)objectId] = (int)num;
				}
				return num;
			}
			return -1L * objectId;
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x000F7618 File Offset: 0x000F6618
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

		// Token: 0x0600479A RID: 18330 RVA: 0x000F7674 File Offset: 0x000F6674
		internal Type Bind(string assemblyString, string typeString)
		{
			Type type = null;
			if (this.m_binder != null)
			{
				type = this.m_binder.BindToType(assemblyString, typeString);
			}
			if (type == null)
			{
				type = this.FastBindToType(assemblyString, typeString);
			}
			return type;
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x000F76A8 File Offset: 0x000F66A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Type FastBindToType(string assemblyName, string typeName)
		{
			Type type = null;
			ObjectReader.TypeNAssembly typeNAssembly = (ObjectReader.TypeNAssembly)this.typeCache.GetCachedValue(typeName);
			if (typeNAssembly == null || typeNAssembly.assemblyName != assemblyName)
			{
				Assembly assembly = null;
				if (this.bSimpleAssembly)
				{
					try
					{
						ObjectReader.sfileIOPermission.Assert();
						try
						{
							StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMe;
							assembly = Assembly.LoadWithPartialNameInternal(assemblyName, null, ref stackCrawlMark);
							if (assembly == null && assemblyName != null)
							{
								assembly = Assembly.LoadWithPartialNameInternal(new AssemblyName(assemblyName).Name, null, ref stackCrawlMark);
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						goto IL_008D;
					}
					catch (Exception)
					{
						goto IL_008D;
					}
				}
				try
				{
					ObjectReader.sfileIOPermission.Assert();
					try
					{
						assembly = Assembly.Load(assemblyName);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				catch (Exception)
				{
				}
				IL_008D:
				if (assembly == null)
				{
					return null;
				}
				try
				{
					ObjectReader.sfileIOPermission.Assert();
					try
					{
						type = FormatterServices.GetTypeFromAssembly(assembly, typeName);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				catch (Exception)
				{
				}
				if (type == null && this.bSimpleAssembly)
				{
					try
					{
						ObjectReader.sfileIOPermission.Assert();
						try
						{
							type = TypeName.GetType(assembly, typeName);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					catch (Exception)
					{
					}
				}
				if (type == null)
				{
					return null;
				}
				ObjectReader.CheckTypeForwardedTo(assembly, type.Assembly);
				typeNAssembly = new ObjectReader.TypeNAssembly();
				typeNAssembly.type = type;
				typeNAssembly.assemblyName = assemblyName;
				this.typeCache.SetCachedValue(typeNAssembly);
			}
			return typeNAssembly.type;
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x000F782C File Offset: 0x000F682C
		internal Type GetType(BinaryAssemblyInfo assemblyInfo, string name)
		{
			Type type;
			if (this.previousName != null && this.previousName.Length == name.Length && this.previousName.Equals(name) && this.previousAssemblyString != null && this.previousAssemblyString.Length == assemblyInfo.assemblyString.Length && this.previousAssemblyString.Equals(assemblyInfo.assemblyString))
			{
				type = this.previousType;
			}
			else
			{
				type = this.Bind(assemblyInfo.assemblyString, name);
				if (type == null)
				{
					Assembly assembly = assemblyInfo.GetAssembly();
					type = FormatterServices.GetTypeFromAssembly(assembly, name);
					if (type != null)
					{
						ObjectReader.CheckTypeForwardedTo(assembly, type.Assembly);
					}
				}
				this.previousAssemblyString = assemblyInfo.assemblyString;
				this.previousName = name;
				this.previousType = type;
			}
			return type;
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x000F78EC File Offset: 0x000F68EC
		[SecuritySafeCritical]
		private static void CheckTypeForwardedTo(Assembly sourceAssembly, Assembly destAssembly)
		{
			if (!FormatterServices.UnsafeTypeForwardersIsEnabled() && sourceAssembly != destAssembly)
			{
				PermissionSet permissionSet = sourceAssembly.GetPermissionSet();
				if (!destAssembly.GetPermissionSet().IsSubsetOf(permissionSet))
				{
					throw new SecurityException
					{
						Demanded = permissionSet
					};
				}
			}
		}

		// Token: 0x04002463 RID: 9315
		private const int THRESHOLD_FOR_VALUETYPE_IDS = 2147483647;

		// Token: 0x04002464 RID: 9316
		internal Stream m_stream;

		// Token: 0x04002465 RID: 9317
		internal ISurrogateSelector m_surrogates;

		// Token: 0x04002466 RID: 9318
		internal StreamingContext m_context;

		// Token: 0x04002467 RID: 9319
		internal ObjectManager m_objectManager;

		// Token: 0x04002468 RID: 9320
		internal InternalFE formatterEnums;

		// Token: 0x04002469 RID: 9321
		internal SerializationBinder m_binder;

		// Token: 0x0400246A RID: 9322
		internal long topId;

		// Token: 0x0400246B RID: 9323
		internal bool bSimpleAssembly;

		// Token: 0x0400246C RID: 9324
		internal object handlerObject;

		// Token: 0x0400246D RID: 9325
		internal object m_topObject;

		// Token: 0x0400246E RID: 9326
		internal Header[] headers;

		// Token: 0x0400246F RID: 9327
		internal HeaderHandler handler;

		// Token: 0x04002470 RID: 9328
		internal SerObjectInfoInit serObjectInfoInit;

		// Token: 0x04002471 RID: 9329
		internal IFormatterConverter m_formatterConverter;

		// Token: 0x04002472 RID: 9330
		internal SerStack stack;

		// Token: 0x04002473 RID: 9331
		private SerStack valueFixupStack;

		// Token: 0x04002474 RID: 9332
		internal object[] crossAppDomainArray;

		// Token: 0x04002475 RID: 9333
		private bool bMethodCall;

		// Token: 0x04002476 RID: 9334
		private bool bMethodReturn;

		// Token: 0x04002477 RID: 9335
		private bool bFullDeserialization;

		// Token: 0x04002478 RID: 9336
		private BinaryMethodCall binaryMethodCall;

		// Token: 0x04002479 RID: 9337
		private BinaryMethodReturn binaryMethodReturn;

		// Token: 0x0400247A RID: 9338
		private bool bIsCrossAppDomain;

		// Token: 0x0400247B RID: 9339
		private static FileIOPermission sfileIOPermission = new FileIOPermission(PermissionState.Unrestricted);

		// Token: 0x0400247C RID: 9340
		private bool bOldFormatDetected;

		// Token: 0x0400247D RID: 9341
		private IntSizedArray valTypeObjectIdTable;

		// Token: 0x0400247E RID: 9342
		private NameCache typeCache = new NameCache();

		// Token: 0x0400247F RID: 9343
		private string previousAssemblyString;

		// Token: 0x04002480 RID: 9344
		private string previousName;

		// Token: 0x04002481 RID: 9345
		private Type previousType;

		// Token: 0x020007DF RID: 2015
		internal class TypeNAssembly
		{
			// Token: 0x04002482 RID: 9346
			public Type type;

			// Token: 0x04002483 RID: 9347
			public string assemblyName;
		}
	}
}
