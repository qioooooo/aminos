using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000005 RID: 5
	internal sealed class SoapHandler
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002684 File Offset: 0x00001684
		internal SoapHandler(SoapParser soapParser)
		{
			this.soapParser = soapParser;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002738 File Offset: 0x00001738
		internal void Init(ObjectReader objectReader)
		{
			this.objectReader = objectReader;
			objectReader.soapHandler = this;
			this.isEnvelope = false;
			this.isBody = false;
			this.isTopFound = false;
			this.attributeValues.Clear();
			this.assemKeyToAssemblyTable = new Hashtable(10);
			this.assemKeyToAssemblyTable[this.urtKey] = new SoapAssemblyInfo(SoapUtil.urtAssemblyString, SoapUtil.urtAssembly);
			this.assemKeyToNameSpaceTable = new Hashtable(10);
			this.assemKeyToInteropAssemblyTable = new Hashtable(10);
			this.nameSpaceToKey = new Hashtable(5);
			this.keyToNamespaceTable = new Hashtable(10);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027D3 File Offset: 0x000017D3
		private string NextPrefix()
		{
			this.nextPrefix++;
			return "_P" + this.nextPrefix;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027F8 File Offset: 0x000017F8
		private ParseRecord GetPr()
		{
			ParseRecord parseRecord;
			if (!this.prPool.IsEmpty())
			{
				parseRecord = (ParseRecord)this.prPool.Pop();
				parseRecord.Init();
			}
			else
			{
				parseRecord = new ParseRecord();
			}
			return parseRecord;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002834 File Offset: 0x00001834
		private void PutPr(ParseRecord pr)
		{
			this.prPool.Push(pr);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002844 File Offset: 0x00001844
		private static string SerTraceString(string handler, ParseRecord pr, string value, InternalParseStateE currentState, SoapHandler.HeaderStateEnum headerState)
		{
			string text = "";
			if (value != null)
			{
				text = value;
			}
			string text2 = "";
			if (pr != null)
			{
				text2 = pr.PRparseStateEnum.ToString();
			}
			return string.Concat(new string[]
			{
				handler,
				" - ",
				text,
				", State ",
				currentState.ToString(),
				", PushState ",
				text2
			});
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028B8 File Offset: 0x000018B8
		private void MarshalError(string handler, ParseRecord pr, string value, InternalParseStateE currentState)
		{
			string text = SoapHandler.SerTraceString(handler, pr, value, currentState, this.headerState);
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Syntax"), new object[] { text }));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000028FB File Offset: 0x000018FB
		internal void Start(XmlTextReader p)
		{
			this.currentState = InternalParseStateE.Object;
			this.xmlTextReader = p;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000290C File Offset: 0x0000190C
		internal void StartElement(string prefix, string name, string urn)
		{
			string text = this.NameFilter(name);
			string text2 = prefix;
			if (urn != null && urn.Length != 0 && (prefix == null || prefix.Length == 0))
			{
				if (this.nameSpaceToKey.ContainsKey(urn))
				{
					text2 = (string)this.nameSpaceToKey[urn];
				}
				else
				{
					text2 = this.NextPrefix();
					this.nameSpaceToKey[urn] = text2;
				}
			}
			switch (this.currentState)
			{
			case InternalParseStateE.Object:
			{
				ParseRecord parseRecord = this.GetPr();
				parseRecord.PRname = text;
				parseRecord.PRnameXmlKey = text2;
				parseRecord.PRxmlNameSpace = urn;
				parseRecord.PRparseStateEnum = InternalParseStateE.Object;
				if (string.Compare(name, "Array", StringComparison.OrdinalIgnoreCase) == 0 && text2.Equals(this.soapKey))
				{
					parseRecord.PRparseTypeEnum = InternalParseTypeE.Object;
				}
				else if ((string.Compare(name, "anyType", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(name, "ur-type", StringComparison.OrdinalIgnoreCase) == 0) && text2.Equals(this.xsdKey))
				{
					parseRecord.PRname = "System.Object";
					parseRecord.PRnameXmlKey = this.urtKey;
					parseRecord.PRxmlNameSpace = urn;
					parseRecord.PRparseTypeEnum = InternalParseTypeE.Object;
				}
				else if (string.Compare(urn, "http://schemas.xmlsoap.org/soap/envelope/", StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (string.Compare(name, "Envelope", StringComparison.OrdinalIgnoreCase) == 0)
					{
						if (this.isEnvelope)
						{
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Parser_Envelope"), new object[] { prefix + ":" + name }));
						}
						this.isEnvelope = true;
						parseRecord.PRparseTypeEnum = InternalParseTypeE.Envelope;
					}
					else if (string.Compare(name, "Body", StringComparison.OrdinalIgnoreCase) == 0)
					{
						if (!this.isEnvelope)
						{
							throw new SerializationException(SoapUtil.GetResourceString("Serialization_Parser_BodyChild"));
						}
						if (this.isBody)
						{
							throw new SerializationException(SoapUtil.GetResourceString("Serialization_Parser_BodyOnce"));
						}
						this.isBody = true;
						this.headerState = SoapHandler.HeaderStateEnum.None;
						this.isTopFound = false;
						parseRecord.PRparseTypeEnum = InternalParseTypeE.Body;
					}
					else if (string.Compare(name, "Header", StringComparison.OrdinalIgnoreCase) == 0)
					{
						if (!this.isEnvelope)
						{
							throw new SerializationException(SoapUtil.GetResourceString("Serialization_Parser_Header"));
						}
						parseRecord.PRparseTypeEnum = InternalParseTypeE.Headers;
						this.headerState = SoapHandler.HeaderStateEnum.FirstHeaderRecord;
					}
					else
					{
						parseRecord.PRparseTypeEnum = InternalParseTypeE.Object;
					}
				}
				else
				{
					parseRecord.PRparseTypeEnum = InternalParseTypeE.Object;
				}
				this.stack.Push(parseRecord);
				return;
			}
			case InternalParseStateE.Member:
			{
				ParseRecord parseRecord = this.GetPr();
				ParseRecord parseRecord2 = (ParseRecord)this.stack.Peek();
				parseRecord.PRname = text;
				parseRecord.PRnameXmlKey = text2;
				parseRecord.PRxmlNameSpace = urn;
				parseRecord.PRparseTypeEnum = InternalParseTypeE.Member;
				parseRecord.PRparseStateEnum = InternalParseStateE.Member;
				this.stack.Push(parseRecord);
				return;
			}
			case InternalParseStateE.MemberChild:
			{
				ParseRecord parseRecord2 = (ParseRecord)this.stack.PeekPeek();
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				parseRecord.PRmemberValueEnum = InternalMemberValueE.Nested;
				this.ProcessAttributes(parseRecord, parseRecord2);
				switch (this.headerState)
				{
				case SoapHandler.HeaderStateEnum.None:
				case SoapHandler.HeaderStateEnum.TopLevelObject:
					this.objectReader.Parse(parseRecord);
					parseRecord.PRisParsed = true;
					break;
				case SoapHandler.HeaderStateEnum.HeaderRecord:
				case SoapHandler.HeaderStateEnum.NestedObject:
					this.ProcessHeaderMember(parseRecord);
					break;
				}
				ParseRecord pr = this.GetPr();
				pr.PRparseTypeEnum = InternalParseTypeE.Member;
				pr.PRparseStateEnum = InternalParseStateE.Member;
				pr.PRname = text;
				pr.PRnameXmlKey = text2;
				parseRecord.PRxmlNameSpace = urn;
				this.currentState = InternalParseStateE.Member;
				this.stack.Push(pr);
				return;
			}
			default:
				this.MarshalError("StartElement", (ParseRecord)this.stack.Peek(), text, this.currentState);
				return;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002C74 File Offset: 0x00001C74
		internal void EndElement(string prefix, string name, string urn)
		{
			string text = this.NameFilter(name);
			switch (this.currentState)
			{
			case InternalParseStateE.Object:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Pop();
				if (parseRecord.PRparseTypeEnum == InternalParseTypeE.Envelope)
				{
					parseRecord.PRparseTypeEnum = InternalParseTypeE.EnvelopeEnd;
				}
				else if (parseRecord.PRparseTypeEnum == InternalParseTypeE.Body)
				{
					parseRecord.PRparseTypeEnum = InternalParseTypeE.BodyEnd;
				}
				else if (parseRecord.PRparseTypeEnum == InternalParseTypeE.Headers)
				{
					parseRecord.PRparseTypeEnum = InternalParseTypeE.HeadersEnd;
					this.headerState = SoapHandler.HeaderStateEnum.HeaderRecord;
				}
				else if (parseRecord.PRarrayTypeEnum != InternalArrayTypeE.Base64)
				{
					ParseRecord parseRecord2 = (ParseRecord)this.stack.Peek();
					if (!this.isTopFound && parseRecord2 != null && parseRecord2.PRparseTypeEnum == InternalParseTypeE.Body)
					{
						parseRecord.PRobjectPositionEnum = InternalObjectPositionE.Top;
						this.isTopFound = true;
					}
					if (!parseRecord.PRisParsed)
					{
						if (!parseRecord.PRisProcessAttributes && (parseRecord.PRobjectPositionEnum != InternalObjectPositionE.Top || !this.objectReader.IsFakeTopObject))
						{
							this.ProcessAttributes(parseRecord, parseRecord2);
						}
						this.objectReader.Parse(parseRecord);
						parseRecord.PRisParsed = true;
					}
					parseRecord.PRparseTypeEnum = InternalParseTypeE.ObjectEnd;
				}
				switch (this.headerState)
				{
				case SoapHandler.HeaderStateEnum.None:
				case SoapHandler.HeaderStateEnum.TopLevelObject:
					this.objectReader.Parse(parseRecord);
					break;
				case SoapHandler.HeaderStateEnum.HeaderRecord:
				case SoapHandler.HeaderStateEnum.NestedObject:
					this.ProcessHeaderEnd(parseRecord);
					break;
				}
				if (parseRecord.PRparseTypeEnum == InternalParseTypeE.EnvelopeEnd)
				{
					this.soapParser.Stop();
				}
				this.PutPr(parseRecord);
				return;
			}
			case InternalParseStateE.Member:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				ParseRecord parseRecord2 = (ParseRecord)this.stack.PeekPeek();
				this.ProcessAttributes(parseRecord, parseRecord2);
				ArrayList arrayList = this.xmlAttributeList;
				if (this.xmlAttributeList != null && this.xmlAttributeList.Count > 0)
				{
					for (int i = 0; i < this.xmlAttributeList.Count; i++)
					{
						this.objectReader.Parse((ParseRecord)this.xmlAttributeList[i]);
					}
					this.xmlAttributeList.Clear();
				}
				parseRecord = (ParseRecord)this.stack.Pop();
				if (this.headerState == SoapHandler.HeaderStateEnum.TopLevelObject && parseRecord.PRarrayTypeEnum == InternalArrayTypeE.Base64)
				{
					this.objectReader.Parse(parseRecord);
					parseRecord.PRisParsed = true;
				}
				else if (parseRecord.PRmemberValueEnum != InternalMemberValueE.Nested)
				{
					if (parseRecord.PRobjectTypeEnum == InternalObjectTypeE.Array && parseRecord.PRmemberValueEnum != InternalMemberValueE.Null)
					{
						parseRecord.PRmemberValueEnum = InternalMemberValueE.Nested;
						this.objectReader.Parse(parseRecord);
						parseRecord.PRisParsed = true;
						parseRecord.PRparseTypeEnum = InternalParseTypeE.MemberEnd;
					}
					else if (parseRecord.PRidRef > 0L)
					{
						parseRecord.PRmemberValueEnum = InternalMemberValueE.Reference;
					}
					else if (parseRecord.PRmemberValueEnum != InternalMemberValueE.Null)
					{
						parseRecord.PRmemberValueEnum = InternalMemberValueE.InlineValue;
					}
					switch (this.headerState)
					{
					case SoapHandler.HeaderStateEnum.None:
					case SoapHandler.HeaderStateEnum.TopLevelObject:
						if (parseRecord.PRparseTypeEnum == InternalParseTypeE.Object)
						{
							if (!parseRecord.PRisParsed)
							{
								this.objectReader.Parse(parseRecord);
							}
							parseRecord.PRparseTypeEnum = InternalParseTypeE.ObjectEnd;
						}
						this.objectReader.Parse(parseRecord);
						parseRecord.PRisParsed = true;
						break;
					case SoapHandler.HeaderStateEnum.HeaderRecord:
					case SoapHandler.HeaderStateEnum.NestedObject:
						this.ProcessHeaderMember(parseRecord);
						break;
					}
				}
				else
				{
					parseRecord.PRparseTypeEnum = InternalParseTypeE.MemberEnd;
					switch (this.headerState)
					{
					case SoapHandler.HeaderStateEnum.None:
					case SoapHandler.HeaderStateEnum.TopLevelObject:
						this.objectReader.Parse(parseRecord);
						parseRecord.PRisParsed = true;
						break;
					case SoapHandler.HeaderStateEnum.HeaderRecord:
					case SoapHandler.HeaderStateEnum.NestedObject:
						this.ProcessHeaderMemberEnd(parseRecord);
						break;
					}
				}
				this.PutPr(parseRecord);
				return;
			}
			case InternalParseStateE.MemberChild:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				if (parseRecord.PRmemberValueEnum != InternalMemberValueE.Null)
				{
					this.MarshalError("EndElement", (ParseRecord)this.stack.Peek(), text, this.currentState);
					return;
				}
				break;
			}
			default:
				this.MarshalError("EndElement", (ParseRecord)this.stack.Peek(), text, this.currentState);
				break;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003024 File Offset: 0x00002024
		internal void StartChildren()
		{
			switch (this.currentState)
			{
			case InternalParseStateE.Object:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				ParseRecord parseRecord2 = (ParseRecord)this.stack.PeekPeek();
				this.ProcessAttributes(parseRecord, parseRecord2);
				if (parseRecord.PRarrayTypeEnum == InternalArrayTypeE.Base64)
				{
					return;
				}
				if (parseRecord.PRparseTypeEnum != InternalParseTypeE.Envelope && parseRecord.PRparseTypeEnum != InternalParseTypeE.Body)
				{
					this.currentState = InternalParseStateE.Member;
				}
				switch (this.headerState)
				{
				case SoapHandler.HeaderStateEnum.None:
				case SoapHandler.HeaderStateEnum.TopLevelObject:
					if (!this.isTopFound && parseRecord2 != null && parseRecord2.PRparseTypeEnum == InternalParseTypeE.Body)
					{
						parseRecord.PRobjectPositionEnum = InternalObjectPositionE.Top;
						this.isTopFound = true;
					}
					this.objectReader.Parse(parseRecord);
					parseRecord.PRisParsed = true;
					return;
				case SoapHandler.HeaderStateEnum.FirstHeaderRecord:
				case SoapHandler.HeaderStateEnum.HeaderRecord:
				case SoapHandler.HeaderStateEnum.NestedObject:
					this.ProcessHeader(parseRecord);
					return;
				default:
					return;
				}
				break;
			}
			case InternalParseStateE.Member:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				this.currentState = InternalParseStateE.MemberChild;
				return;
			}
			}
			this.MarshalError("StartChildren", (ParseRecord)this.stack.Peek(), null, this.currentState);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003140 File Offset: 0x00002140
		internal void FinishChildren(string prefix, string name, string urn)
		{
			switch (this.currentState)
			{
			case InternalParseStateE.Object:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				if (parseRecord.PRarrayTypeEnum == InternalArrayTypeE.Base64)
				{
					parseRecord.PRvalue = this.textValue;
					this.textValue = "";
					return;
				}
				break;
			}
			case InternalParseStateE.Member:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				this.currentState = parseRecord.PRparseStateEnum;
				parseRecord.PRvalue = this.textValue;
				this.textValue = "";
				return;
			}
			case InternalParseStateE.MemberChild:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				this.currentState = parseRecord.PRparseStateEnum;
				ParseRecord parseRecord2 = (ParseRecord)this.stack.PeekPeek();
				parseRecord.PRvalue = this.textValue;
				this.textValue = "";
				return;
			}
			default:
				this.MarshalError("FinishChildren", (ParseRecord)this.stack.Peek(), name, this.currentState);
				break;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003240 File Offset: 0x00002240
		internal void Attribute(string prefix, string name, string urn, string value)
		{
			switch (this.currentState)
			{
			case InternalParseStateE.Object:
			case InternalParseStateE.Member:
			{
				ParseRecord parseRecord = (ParseRecord)this.stack.Peek();
				string text = name;
				if (urn != null && urn.Length != 0 && (prefix == null || prefix.Length == 0))
				{
					if (this.nameSpaceToKey.ContainsKey(urn))
					{
						text = (string)this.nameSpaceToKey[urn];
					}
					else
					{
						text = this.NextPrefix();
						this.nameSpaceToKey[urn] = text;
					}
				}
				if (prefix != null && text != null && value != null && urn != null)
				{
					this.attributeValues.Push(new SoapHandler.AttributeValueEntry(prefix, text, value, urn));
					return;
				}
				return;
			}
			}
			this.MarshalError("EndAttribute, Unknown State ", (ParseRecord)this.stack.Peek(), name, this.currentState);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003311 File Offset: 0x00002311
		internal void Text(string text)
		{
			this.textValue = text;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000331A File Offset: 0x0000231A
		internal void Comment(string body)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000331C File Offset: 0x0000231C
		private void ProcessAttributes(ParseRecord pr, ParseRecord objectPr)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			pr.PRisProcessAttributes = true;
			string text4 = "http://schemas.xmlsoap.org/soap/encoding/";
			int length = text4.Length;
			string text5 = "http://schemas.microsoft.com/clr/id";
			int length2 = text5.Length;
			string text6 = "http://schemas.xmlsoap.org/soap/envelope/";
			int length3 = text6.Length;
			string text7 = "http://www.w3.org/2001/XMLSchema-instance";
			int length4 = text7.Length;
			string text8 = "http://www.w3.org/2000/10/XMLSchema-instance";
			int length5 = text8.Length;
			string text9 = "http://www.w3.org/1999/XMLSchema-instance";
			int length6 = text9.Length;
			string text10 = "http://www.w3.org/1999/XMLSchema";
			int length7 = text10.Length;
			string text11 = "http://www.w3.org/2000/10/XMLSchema";
			int length8 = text11.Length;
			string text12 = "http://www.w3.org/2001/XMLSchema";
			int length9 = text12.Length;
			string text13 = "http://schemas.microsoft.com/soap/encoding/clr/1.0";
			int length10 = text13.Length;
			for (int i = 0; i < this.attributeValues.Count(); i++)
			{
				SoapHandler.AttributeValueEntry attributeValueEntry = (SoapHandler.AttributeValueEntry)this.attributeValues.GetItem(i);
				string prefix = attributeValueEntry.prefix;
				string text14 = attributeValueEntry.key;
				if (text14 == null || text14.Length == 0)
				{
					text14 = pr.PRnameXmlKey;
				}
				string value = attributeValueEntry.value;
				string urn = attributeValueEntry.urn;
				int length11 = text14.Length;
				int length12 = value.Length;
				if (text14 == null || length11 == 0)
				{
					this.keyToNamespaceTable[prefix] = value;
				}
				else
				{
					this.keyToNamespaceTable[prefix + ":" + text14] = value;
				}
				bool flag;
				if (length11 == 2 && string.Compare(text14, "id", StringComparison.OrdinalIgnoreCase) == 0)
				{
					pr.PRobjectId = this.objectReader.GetId(value);
				}
				else if (length11 == 8 && string.Compare(text14, "position", StringComparison.OrdinalIgnoreCase) == 0)
				{
					text = value;
				}
				else if (length11 == 6 && string.Compare(text14, "offset", StringComparison.OrdinalIgnoreCase) == 0)
				{
					text2 = value;
				}
				else if (length11 == 14 && string.Compare(text14, "MustUnderstand", StringComparison.OrdinalIgnoreCase) == 0)
				{
					text3 = value;
				}
				else if (length11 == 4 && string.Compare(text14, "null", StringComparison.OrdinalIgnoreCase) == 0)
				{
					pr.PRmemberValueEnum = InternalMemberValueE.Null;
					pr.PRvalue = null;
				}
				else if (length11 == 4 && string.Compare(text14, "root", StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (value.Equals("1"))
					{
						pr.PRisHeaderRoot = true;
					}
				}
				else if (length11 == 4 && string.Compare(text14, "href", StringComparison.OrdinalIgnoreCase) == 0)
				{
					pr.PRidRef = this.objectReader.GetId(value);
				}
				else if (length11 == 4 && string.Compare(text14, "type", StringComparison.OrdinalIgnoreCase) == 0)
				{
					string prtypeXmlKey = pr.PRtypeXmlKey;
					string prkeyDt = pr.PRkeyDt;
					Type prdtType = pr.PRdtType;
					string text15 = value;
					int num = value.IndexOf(":");
					if (num > 0)
					{
						pr.PRtypeXmlKey = value.Substring(0, num);
						text15 = value.Substring(num + 1);
					}
					else
					{
						pr.PRtypeXmlKey = prefix;
					}
					if (string.Compare(text15, "anyType", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text15, "ur-type", StringComparison.OrdinalIgnoreCase) == 0)
					{
						pr.PRkeyDt = "System.Object";
						pr.PRdtType = SoapUtil.typeofObject;
						pr.PRtypeXmlKey = this.urtKey;
					}
					if (pr.PRtypeXmlKey == this.soapKey && text15 == "Array")
					{
						pr.PRtypeXmlKey = prtypeXmlKey;
						pr.PRkeyDt = prkeyDt;
						pr.PRdtType = prdtType;
					}
					else
					{
						pr.PRkeyDt = text15;
					}
				}
				else if (length11 == 9 && string.Compare(text14, "arraytype", StringComparison.OrdinalIgnoreCase) == 0)
				{
					string text16 = value;
					int num2 = value.IndexOf(":");
					if (num2 > 0)
					{
						pr.PRtypeXmlKey = value.Substring(0, num2);
						text16 = (pr.PRkeyDt = value.Substring(num2 + 1));
					}
					if (text16.StartsWith("ur_type[", StringComparison.Ordinal))
					{
						pr.PRkeyDt = "System.Object" + text16.Substring(6);
						pr.PRtypeXmlKey = this.urtKey;
					}
				}
				else if (SoapServices.IsClrTypeNamespace(value))
				{
					if (!this.assemKeyToAssemblyTable.ContainsKey(text14))
					{
						string text17 = null;
						string text18 = null;
						SoapServices.DecodeXmlNamespaceForClrTypeNamespace(value, out text17, out text18);
						if (text18 == null)
						{
							this.assemKeyToAssemblyTable[text14] = new SoapAssemblyInfo(SoapUtil.urtAssemblyString, SoapUtil.urtAssembly);
							this.assemKeyToNameSpaceTable[text14] = text17;
						}
						else
						{
							this.assemKeyToAssemblyTable[text14] = new SoapAssemblyInfo(text18);
							if (text17 != null)
							{
								this.assemKeyToNameSpaceTable[text14] = text17;
							}
						}
					}
				}
				else if ((flag = prefix.Equals("xmlns")) && length12 == length && string.Compare(value, text4, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.soapKey = text14;
				}
				else if (flag && length12 == length2 && string.Compare(value, text5, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.urtKey = text14;
					this.assemKeyToAssemblyTable[this.urtKey] = new SoapAssemblyInfo(SoapUtil.urtAssemblyString, SoapUtil.urtAssembly);
				}
				else if (flag && length12 == length3 && string.Compare(value, text6, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.soapEnvKey = text14;
				}
				else if (!(text14 == "encodingStyle"))
				{
					if (flag && ((length12 == length4 && string.Compare(value, text7, StringComparison.OrdinalIgnoreCase) == 0) || (length12 == length6 && string.Compare(value, text9, StringComparison.OrdinalIgnoreCase) == 0) || (length12 == length5 && string.Compare(value, text8, StringComparison.OrdinalIgnoreCase) == 0)))
					{
						this.xsiKey = text14;
					}
					else if ((flag && length12 == length9 && string.Compare(value, text12, StringComparison.OrdinalIgnoreCase) == 0) || (length12 == length7 && string.Compare(value, text10, StringComparison.OrdinalIgnoreCase) == 0) || (length12 == length8 && string.Compare(value, text11, StringComparison.OrdinalIgnoreCase) == 0))
					{
						this.xsdKey = text14;
					}
					else if (flag && length12 == length10 && string.Compare(value, text13, StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.objectReader.SetVersion(1, 0);
					}
					else if (flag)
					{
						this.assemKeyToInteropAssemblyTable[text14] = value;
					}
					else if (string.Compare(prefix, this.soapKey, StringComparison.OrdinalIgnoreCase) != 0 && this.assemKeyToInteropAssemblyTable.ContainsKey(prefix) && ((string)this.assemKeyToInteropAssemblyTable[prefix]).Equals(urn))
					{
						this.ProcessXmlAttribute(prefix, text14, value, objectPr);
					}
				}
			}
			this.attributeValues.Clear();
			if (this.headerState != SoapHandler.HeaderStateEnum.None)
			{
				if (objectPr.PRparseTypeEnum == InternalParseTypeE.Headers)
				{
					if (pr.PRisHeaderRoot || this.headerState == SoapHandler.HeaderStateEnum.FirstHeaderRecord)
					{
						this.headerState = SoapHandler.HeaderStateEnum.HeaderRecord;
					}
					else
					{
						this.headerState = SoapHandler.HeaderStateEnum.TopLevelObject;
						this.currentState = InternalParseStateE.Object;
						pr.PRobjectTypeEnum = InternalObjectTypeE.Object;
						pr.PRparseTypeEnum = InternalParseTypeE.Object;
						pr.PRparseStateEnum = InternalParseStateE.Object;
						pr.PRmemberTypeEnum = InternalMemberTypeE.Empty;
						pr.PRmemberValueEnum = InternalMemberValueE.Empty;
					}
				}
				else if (objectPr.PRisHeaderRoot)
				{
					this.headerState = SoapHandler.HeaderStateEnum.NestedObject;
				}
			}
			if (!this.isTopFound && objectPr != null && objectPr.PRparseTypeEnum == InternalParseTypeE.Body)
			{
				pr.PRobjectPositionEnum = InternalObjectPositionE.Top;
				this.isTopFound = true;
			}
			else if (pr.PRobjectPositionEnum != InternalObjectPositionE.Top)
			{
				pr.PRobjectPositionEnum = InternalObjectPositionE.Child;
			}
			if (pr.PRparseTypeEnum != InternalParseTypeE.Envelope && pr.PRparseTypeEnum != InternalParseTypeE.Body && pr.PRparseTypeEnum != InternalParseTypeE.Headers && (pr.PRobjectPositionEnum != InternalObjectPositionE.Top || !this.objectReader.IsFakeTopObject || pr.PRnameXmlKey.Equals(this.soapEnvKey)))
			{
				this.ProcessType(pr, objectPr);
			}
			if (text != null)
			{
				int num3;
				string text19;
				InternalArrayTypeE internalArrayTypeE;
				pr.PRpositionA = this.ParseArrayDimensions(text, out num3, out text19, out internalArrayTypeE);
			}
			if (text2 != null)
			{
				int num4;
				string text20;
				InternalArrayTypeE internalArrayTypeE2;
				pr.PRlowerBoundA = this.ParseArrayDimensions(text2, out num4, out text20, out internalArrayTypeE2);
			}
			if (text3 != null)
			{
				if (text3.Equals("1"))
				{
					pr.PRisMustUnderstand = true;
				}
				else
				{
					if (!text3.Equals("0"))
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_MustUnderstand"), new object[] { text3 }));
					}
					pr.PRisMustUnderstand = false;
				}
			}
			if (pr.PRparseTypeEnum == InternalParseTypeE.Member)
			{
				if (objectPr.PRparseTypeEnum == InternalParseTypeE.Headers)
				{
					pr.PRmemberTypeEnum = InternalMemberTypeE.Header;
					return;
				}
				if (objectPr.PRobjectTypeEnum == InternalObjectTypeE.Array)
				{
					pr.PRmemberTypeEnum = InternalMemberTypeE.Item;
					return;
				}
				pr.PRmemberTypeEnum = InternalMemberTypeE.Field;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003B18 File Offset: 0x00002B18
		private void ProcessType(ParseRecord pr, ParseRecord objectPr)
		{
			if (pr.PRdtType != null)
			{
				return;
			}
			if (pr.PRnameXmlKey.Equals(this.soapEnvKey) && string.Compare(pr.PRname, "Fault", StringComparison.OrdinalIgnoreCase) == 0)
			{
				pr.PRdtType = SoapUtil.typeofSoapFault;
				pr.PRparseTypeEnum = InternalParseTypeE.Object;
			}
			else if (pr.PRname != null)
			{
				string text = null;
				if (pr.PRnameXmlKey != null && pr.PRnameXmlKey.Length > 0)
				{
					text = (string)this.assemKeyToInteropAssemblyTable[pr.PRnameXmlKey];
				}
				Type type = null;
				string text2 = null;
				if (objectPr != null)
				{
					if (pr.PRisXmlAttribute)
					{
						SoapServices.GetInteropFieldTypeAndNameFromXmlAttribute(objectPr.PRdtType, pr.PRname, text, out type, out text2);
					}
					else
					{
						SoapServices.GetInteropFieldTypeAndNameFromXmlElement(objectPr.PRdtType, pr.PRname, text, out type, out text2);
					}
				}
				if (type != null)
				{
					pr.PRdtType = type;
					pr.PRname = text2;
					pr.PRdtTypeCode = Converter.SoapToCode(pr.PRdtType);
				}
				else
				{
					if (text != null)
					{
						pr.PRdtType = this.objectReader.Bind(text, pr.PRname);
					}
					if (pr.PRdtType == null)
					{
						pr.PRdtType = SoapServices.GetInteropTypeFromXmlElement(pr.PRname, text);
					}
					if (pr.PRkeyDt == null && pr.PRnameXmlKey != null && pr.PRnameXmlKey.Length > 0 && objectPr.PRobjectTypeEnum == InternalObjectTypeE.Array && objectPr.PRarrayElementType == Converter.typeofObject)
					{
						pr.PRdtType = this.ProcessGetType(pr.PRname, pr.PRnameXmlKey, out pr.PRassemblyName);
						pr.PRdtTypeCode = Converter.SoapToCode(pr.PRdtType);
					}
				}
			}
			if (pr.PRdtType != null)
			{
				return;
			}
			if (pr.PRtypeXmlKey != null && pr.PRtypeXmlKey.Length > 0 && pr.PRkeyDt != null && pr.PRkeyDt.Length > 0 && this.assemKeyToInteropAssemblyTable.ContainsKey(pr.PRtypeXmlKey))
			{
				int num = pr.PRkeyDt.IndexOf("[");
				if (num > 0)
				{
					this.ProcessArray(pr, num, true);
					return;
				}
				string text3 = (string)this.assemKeyToInteropAssemblyTable[pr.PRtypeXmlKey];
				pr.PRdtType = this.objectReader.Bind(text3, pr.PRkeyDt);
				if (pr.PRdtType == null)
				{
					pr.PRdtType = SoapServices.GetInteropTypeFromXmlType(pr.PRkeyDt, text3);
					if (pr.PRdtType == null)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_TypeElement"), new object[] { pr.PRname + " " + pr.PRkeyDt }));
					}
				}
				if (pr.PRdtType == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_TypeElement"), new object[] { string.Concat(new string[] { pr.PRname, " ", pr.PRkeyDt, ", ", text3 }) }));
				}
			}
			else if (pr.PRkeyDt != null)
			{
				if (string.Compare(pr.PRkeyDt, "Base64", StringComparison.OrdinalIgnoreCase) == 0)
				{
					pr.PRobjectTypeEnum = InternalObjectTypeE.Array;
					pr.PRarrayTypeEnum = InternalArrayTypeE.Base64;
					return;
				}
				if (string.Compare(pr.PRkeyDt, "String", StringComparison.OrdinalIgnoreCase) == 0)
				{
					pr.PRdtType = SoapUtil.typeofString;
					return;
				}
				if (string.Compare(pr.PRkeyDt, "methodSignature", StringComparison.OrdinalIgnoreCase) == 0)
				{
					try
					{
						pr.PRdtType = typeof(Type[]);
						char[] array = new char[] { ' ', ':' };
						string[] array2 = null;
						if (pr.PRvalue != null)
						{
							array2 = pr.PRvalue.Split(array);
						}
						Type[] array3;
						if (array2 == null || (array2.Length == 1 && array2[0].Length == 0))
						{
							array3 = new Type[0];
						}
						else
						{
							array3 = new Type[array2.Length / 2];
							for (int i = 0; i < array2.Length; i += 2)
							{
								string text4 = array2[i];
								string text5 = array2[i + 1];
								array3[i / 2] = this.ProcessGetType(text5, text4, out pr.PRassemblyName);
							}
						}
						pr.PRvarValue = array3;
						return;
					}
					catch
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_MethodSignature"), new object[] { pr.PRvalue }));
					}
				}
				pr.PRdtTypeCode = Converter.ToCode(pr.PRkeyDt);
				if (pr.PRdtTypeCode != InternalPrimitiveTypeE.Invalid)
				{
					pr.PRdtType = Converter.SoapToType(pr.PRdtTypeCode);
					return;
				}
				int num2 = pr.PRkeyDt.IndexOf("[");
				if (num2 > 0)
				{
					this.ProcessArray(pr, num2, false);
					return;
				}
				pr.PRobjectTypeEnum = InternalObjectTypeE.Object;
				pr.PRdtType = this.ProcessGetType(pr.PRkeyDt, pr.PRtypeXmlKey, out pr.PRassemblyName);
				if (pr.PRdtType == null && pr.PRobjectPositionEnum != InternalObjectPositionE.Top)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_TypeElement"), new object[] { pr.PRname + " " + pr.PRkeyDt }));
				}
			}
			else if (pr.PRparseTypeEnum == InternalParseTypeE.Object && (!this.objectReader.IsFakeTopObject || pr.PRobjectPositionEnum != InternalObjectPositionE.Top))
			{
				if (string.Compare(pr.PRname, "Array", StringComparison.OrdinalIgnoreCase) == 0)
				{
					pr.PRdtType = this.ProcessGetType(pr.PRkeyDt, pr.PRtypeXmlKey, out pr.PRassemblyName);
					return;
				}
				pr.PRdtType = this.ProcessGetType(pr.PRname, pr.PRnameXmlKey, out pr.PRassemblyName);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000040A8 File Offset: 0x000030A8
		private Type ProcessGetType(string value, string xmlKey, out string assemblyString)
		{
			Type type = null;
			string text = null;
			assemblyString = null;
			string text2 = (string)this.keyToNamespaceTable["xmlns:" + xmlKey];
			if (text2 != null)
			{
				type = this.GetInteropType(value, text2);
				if (type != null)
				{
					return type;
				}
			}
			if ((string.Compare(value, "anyType", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(value, "ur-type", StringComparison.OrdinalIgnoreCase) == 0) && xmlKey.Equals(this.xsdKey))
			{
				type = SoapUtil.typeofObject;
			}
			else if (xmlKey.Equals(this.xsdKey) || xmlKey.Equals(this.soapKey))
			{
				if (string.Compare(value, "string", StringComparison.OrdinalIgnoreCase) == 0)
				{
					type = SoapUtil.typeofString;
				}
				else
				{
					InternalPrimitiveTypeE internalPrimitiveTypeE = Converter.ToCode(value);
					if (internalPrimitiveTypeE == InternalPrimitiveTypeE.Invalid)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Parser_xsd"), new object[] { value }));
					}
					type = Converter.SoapToType(internalPrimitiveTypeE);
				}
			}
			else
			{
				if (xmlKey == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Parser_xml"), new object[] { value }));
				}
				string text3 = (string)this.assemKeyToNameSpaceTable[xmlKey];
				text = null;
				if (text3 == null || text3.Length == 0)
				{
					text = value;
				}
				else
				{
					text = text3 + "." + value;
				}
				SoapAssemblyInfo soapAssemblyInfo = (SoapAssemblyInfo)this.assemKeyToAssemblyTable[xmlKey];
				if (soapAssemblyInfo == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Parser_xmlAssembly"), new object[] { xmlKey + " " + value }));
				}
				assemblyString = soapAssemblyInfo.assemblyString;
				if (assemblyString != null)
				{
					type = this.objectReader.Bind(assemblyString, text);
					if (type == null)
					{
						type = this.objectReader.FastBindToType(assemblyString, text);
					}
				}
				if (type == null)
				{
					Assembly assembly = null;
					try
					{
						assembly = soapAssemblyInfo.GetAssembly(this.objectReader);
					}
					catch
					{
					}
					if (assembly == null)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Parser_xmlAssembly"), new object[] { string.Concat(new string[] { xmlKey, ":", text2, " ", value }) }));
					}
					type = FormatterServices.GetTypeFromAssembly(assembly, text);
				}
			}
			if (type == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Parser_xmlType"), new object[] { string.Concat(new string[] { xmlKey, " ", text, " ", assemblyString }) }));
			}
			return type;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004358 File Offset: 0x00003358
		private Type GetInteropType(string value, string httpstring)
		{
			Type type = SoapServices.GetInteropTypeFromXmlType(value, httpstring);
			if (type == null)
			{
				int num = httpstring.IndexOf("%2C");
				if (num > 0)
				{
					string text = httpstring.Substring(0, num);
					type = SoapServices.GetInteropTypeFromXmlType(value, text);
				}
			}
			return type;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00004394 File Offset: 0x00003394
		private void ProcessArray(ParseRecord pr, int firstIndex, bool IsInterop)
		{
			string prtypeXmlKey = pr.PRtypeXmlKey;
			InternalPrimitiveTypeE internalPrimitiveTypeE = InternalPrimitiveTypeE.Invalid;
			pr.PRobjectTypeEnum = InternalObjectTypeE.Array;
			pr.PRmemberTypeEnum = InternalMemberTypeE.Item;
			pr.PRprimitiveArrayTypeString = pr.PRkeyDt.Substring(0, firstIndex);
			pr.PRkeyDt.Substring(firstIndex);
			if (IsInterop)
			{
				string text = (string)this.assemKeyToInteropAssemblyTable[pr.PRtypeXmlKey];
				pr.PRarrayElementType = this.objectReader.Bind(text, pr.PRprimitiveArrayTypeString);
				if (pr.PRarrayElementType == null)
				{
					pr.PRarrayElementType = SoapServices.GetInteropTypeFromXmlType(pr.PRprimitiveArrayTypeString, text);
				}
				if (pr.PRarrayElementType == null)
				{
					pr.PRarrayElementType = SoapServices.GetInteropTypeFromXmlElement(pr.PRprimitiveArrayTypeString, text);
				}
				if (pr.PRarrayElementType == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_TypeElement"), new object[] { pr.PRname + " " + pr.PRkeyDt }));
				}
				pr.PRprimitiveArrayTypeString = pr.PRarrayElementType.FullName;
			}
			else
			{
				internalPrimitiveTypeE = Converter.ToCode(pr.PRprimitiveArrayTypeString);
				if (internalPrimitiveTypeE != InternalPrimitiveTypeE.Invalid)
				{
					pr.PRprimitiveArrayTypeString = Converter.SoapToComType(internalPrimitiveTypeE);
					prtypeXmlKey = this.urtKey;
				}
				else if (string.Compare(pr.PRprimitiveArrayTypeString, "string", StringComparison.Ordinal) == 0)
				{
					pr.PRprimitiveArrayTypeString = "System.String";
					prtypeXmlKey = this.urtKey;
				}
				else if (string.Compare(pr.PRprimitiveArrayTypeString, "anyType", StringComparison.Ordinal) == 0 || string.Compare(pr.PRprimitiveArrayTypeString, "ur-type", StringComparison.Ordinal) == 0)
				{
					pr.PRprimitiveArrayTypeString = "System.Object";
					prtypeXmlKey = this.urtKey;
				}
			}
			int num = firstIndex;
			int num2 = pr.PRkeyDt.IndexOf(']', num + 1);
			if (num2 < 1)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ArrayDimensions"), new object[] { pr.PRkeyDt }));
			}
			int num3 = 0;
			string text2 = null;
			InternalArrayTypeE internalArrayTypeE = InternalArrayTypeE.Empty;
			int num4 = 0;
			StringBuilder stringBuilder = new StringBuilder(10);
			int[] array;
			for (;;)
			{
				num4++;
				array = this.ParseArrayDimensions(pr.PRkeyDt.Substring(num, num2 - num + 1), out num3, out text2, out internalArrayTypeE);
				if (num2 + 1 == pr.PRkeyDt.Length)
				{
					break;
				}
				stringBuilder.Append(text2);
				num = num2 + 1;
				num2 = pr.PRkeyDt.IndexOf(']', num);
			}
			pr.PRlengthA = array;
			pr.PRrank = num3;
			if (num4 == 1)
			{
				pr.PRarrayElementTypeCode = internalPrimitiveTypeE;
				pr.PRarrayTypeEnum = internalArrayTypeE;
				pr.PRarrayElementTypeString = pr.PRprimitiveArrayTypeString;
			}
			else
			{
				pr.PRarrayElementTypeCode = InternalPrimitiveTypeE.Invalid;
				pr.PRarrayTypeEnum = InternalArrayTypeE.Rectangular;
				pr.PRarrayElementTypeString = pr.PRprimitiveArrayTypeString + stringBuilder.ToString();
			}
			if (!IsInterop || num4 > 1)
			{
				pr.PRarrayElementType = this.ProcessGetType(pr.PRarrayElementTypeString, prtypeXmlKey, out pr.PRassemblyName);
				if (pr.PRarrayElementType == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ArrayType"), new object[] { pr.PRarrayElementType }));
				}
				if (pr.PRarrayElementType == SoapUtil.typeofObject)
				{
					pr.PRisArrayVariant = true;
					prtypeXmlKey = this.urtKey;
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000046A4 File Offset: 0x000036A4
		private int[] ParseArrayDimensions(string dimString, out int rank, out string dimSignature, out InternalArrayTypeE arrayTypeEnum)
		{
			char[] array = dimString.ToCharArray();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int[] array2 = new int[array.Length];
			StringBuilder stringBuilder = new StringBuilder(10);
			StringBuilder stringBuilder2 = new StringBuilder(10);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == '[')
				{
					num++;
					stringBuilder2.Append(array[i]);
				}
				else if (array[i] == ']')
				{
					if (stringBuilder.Length > 0)
					{
						array2[num3++] = int.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);
						stringBuilder.Length = 0;
					}
					stringBuilder2.Append(array[i]);
				}
				else if (array[i] == ',')
				{
					num2++;
					if (stringBuilder.Length > 0)
					{
						array2[num3++] = int.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);
						stringBuilder.Length = 0;
					}
					stringBuilder2.Append(array[i]);
				}
				else
				{
					if (array[i] != '-' && !char.IsDigit(array[i]))
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ArrayDimensions"), new object[] { dimString }));
					}
					stringBuilder.Append(array[i]);
				}
			}
			rank = num3;
			dimSignature = stringBuilder2.ToString();
			int[] array3 = new int[rank];
			for (int j = 0; j < rank; j++)
			{
				array3[j] = array2[j];
			}
			InternalArrayTypeE internalArrayTypeE;
			if (num2 > 0)
			{
				internalArrayTypeE = InternalArrayTypeE.Rectangular;
			}
			else
			{
				internalArrayTypeE = InternalArrayTypeE.Single;
			}
			arrayTypeEnum = internalArrayTypeE;
			return array3;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004828 File Offset: 0x00003828
		private string NameFilter(string name)
		{
			string text = this.nameCache.GetCachedValue(name) as string;
			if (text == null)
			{
				text = XmlConvert.DecodeName(name);
				this.nameCache.SetCachedValue(text);
			}
			return text;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004860 File Offset: 0x00003860
		private void ProcessXmlAttribute(string prefix, string key, string value, ParseRecord objectPr)
		{
			if (this.xmlAttributeList == null)
			{
				this.xmlAttributeList = new ArrayList(10);
			}
			ParseRecord pr = this.GetPr();
			pr.PRparseTypeEnum = InternalParseTypeE.Member;
			pr.PRmemberTypeEnum = InternalMemberTypeE.Field;
			pr.PRmemberValueEnum = InternalMemberValueE.InlineValue;
			pr.PRname = key;
			pr.PRvalue = value;
			pr.PRnameXmlKey = prefix;
			pr.PRisXmlAttribute = true;
			this.ProcessType(pr, objectPr);
			this.xmlAttributeList.Add(pr);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000048D0 File Offset: 0x000038D0
		private void ProcessHeader(ParseRecord pr)
		{
			if (this.headerList == null)
			{
				this.headerList = new ArrayList(10);
			}
			ParseRecord pr2 = this.GetPr();
			pr2.PRparseTypeEnum = InternalParseTypeE.Object;
			pr2.PRobjectTypeEnum = InternalObjectTypeE.Array;
			pr2.PRobjectPositionEnum = InternalObjectPositionE.Headers;
			pr2.PRarrayTypeEnum = InternalArrayTypeE.Single;
			pr2.PRarrayElementType = typeof(Header);
			pr2.PRisArrayVariant = false;
			pr2.PRarrayElementTypeCode = InternalPrimitiveTypeE.Invalid;
			pr2.PRrank = 1;
			pr2.PRlengthA = new int[1];
			this.headerList.Add(pr2);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00004954 File Offset: 0x00003954
		private void ProcessHeaderMember(ParseRecord pr)
		{
			if (this.headerState == SoapHandler.HeaderStateEnum.NestedObject)
			{
				ParseRecord parseRecord = pr.Copy();
				this.headerList.Add(parseRecord);
				return;
			}
			ParseRecord parseRecord2 = this.GetPr();
			parseRecord2.PRparseTypeEnum = InternalParseTypeE.Member;
			parseRecord2.PRmemberTypeEnum = InternalMemberTypeE.Item;
			parseRecord2.PRmemberValueEnum = InternalMemberValueE.Nested;
			parseRecord2.PRisHeaderRoot = true;
			this.headerArrayLength++;
			this.headerList.Add(parseRecord2);
			parseRecord2 = this.GetPr();
			parseRecord2.PRparseTypeEnum = InternalParseTypeE.Member;
			parseRecord2.PRmemberTypeEnum = InternalMemberTypeE.Field;
			parseRecord2.PRmemberValueEnum = InternalMemberValueE.InlineValue;
			parseRecord2.PRisHeaderRoot = true;
			parseRecord2.PRname = "Name";
			parseRecord2.PRvalue = pr.PRname;
			parseRecord2.PRdtType = SoapUtil.typeofString;
			parseRecord2.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			this.headerList.Add(parseRecord2);
			parseRecord2 = this.GetPr();
			parseRecord2.PRparseTypeEnum = InternalParseTypeE.Member;
			parseRecord2.PRmemberTypeEnum = InternalMemberTypeE.Field;
			parseRecord2.PRmemberValueEnum = InternalMemberValueE.InlineValue;
			parseRecord2.PRisHeaderRoot = true;
			parseRecord2.PRname = "HeaderNamespace";
			parseRecord2.PRvalue = pr.PRxmlNameSpace;
			parseRecord2.PRdtType = SoapUtil.typeofString;
			parseRecord2.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			this.headerList.Add(parseRecord2);
			parseRecord2 = this.GetPr();
			parseRecord2.PRparseTypeEnum = InternalParseTypeE.Member;
			parseRecord2.PRmemberTypeEnum = InternalMemberTypeE.Field;
			parseRecord2.PRmemberValueEnum = InternalMemberValueE.InlineValue;
			parseRecord2.PRisHeaderRoot = true;
			parseRecord2.PRname = "MustUnderstand";
			if (pr.PRisMustUnderstand)
			{
				parseRecord2.PRvarValue = true;
			}
			else
			{
				parseRecord2.PRvarValue = false;
			}
			parseRecord2.PRdtType = SoapUtil.typeofBoolean;
			parseRecord2.PRdtTypeCode = InternalPrimitiveTypeE.Boolean;
			this.headerList.Add(parseRecord2);
			parseRecord2 = this.GetPr();
			parseRecord2.PRparseTypeEnum = InternalParseTypeE.Member;
			parseRecord2.PRmemberTypeEnum = InternalMemberTypeE.Field;
			parseRecord2.PRmemberValueEnum = pr.PRmemberValueEnum;
			parseRecord2.PRisHeaderRoot = true;
			parseRecord2.PRname = "Value";
			switch (pr.PRmemberValueEnum)
			{
			case InternalMemberValueE.InlineValue:
				parseRecord2.PRvalue = pr.PRvalue;
				parseRecord2.PRvarValue = pr.PRvarValue;
				parseRecord2.PRdtType = pr.PRdtType;
				parseRecord2.PRdtTypeCode = pr.PRdtTypeCode;
				parseRecord2.PRkeyDt = pr.PRkeyDt;
				this.headerList.Add(parseRecord2);
				this.ProcessHeaderMemberEnd(pr);
				return;
			case InternalMemberValueE.Nested:
				parseRecord2.PRdtType = pr.PRdtType;
				parseRecord2.PRdtTypeCode = pr.PRdtTypeCode;
				parseRecord2.PRkeyDt = pr.PRkeyDt;
				this.headerList.Add(parseRecord2);
				return;
			case InternalMemberValueE.Reference:
				parseRecord2.PRidRef = pr.PRidRef;
				this.headerList.Add(parseRecord2);
				this.ProcessHeaderMemberEnd(pr);
				return;
			case InternalMemberValueE.Null:
				this.headerList.Add(parseRecord2);
				this.ProcessHeaderMemberEnd(pr);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00004BEC File Offset: 0x00003BEC
		private void ProcessHeaderMemberEnd(ParseRecord pr)
		{
			if (this.headerState == SoapHandler.HeaderStateEnum.NestedObject)
			{
				ParseRecord parseRecord = pr.Copy();
				this.headerList.Add(parseRecord);
				return;
			}
			ParseRecord parseRecord2 = this.GetPr();
			parseRecord2.PRparseTypeEnum = InternalParseTypeE.MemberEnd;
			parseRecord2.PRmemberTypeEnum = InternalMemberTypeE.Field;
			parseRecord2.PRmemberValueEnum = pr.PRmemberValueEnum;
			parseRecord2.PRisHeaderRoot = true;
			this.headerList.Add(parseRecord2);
			parseRecord2 = this.GetPr();
			parseRecord2.PRparseTypeEnum = InternalParseTypeE.MemberEnd;
			parseRecord2.PRmemberTypeEnum = InternalMemberTypeE.Item;
			parseRecord2.PRmemberValueEnum = InternalMemberValueE.Nested;
			parseRecord2.PRisHeaderRoot = true;
			this.headerList.Add(parseRecord2);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004C80 File Offset: 0x00003C80
		private void ProcessHeaderEnd(ParseRecord pr)
		{
			if (this.headerList == null)
			{
				return;
			}
			ParseRecord parseRecord = this.GetPr();
			parseRecord.PRparseTypeEnum = InternalParseTypeE.ObjectEnd;
			parseRecord.PRobjectTypeEnum = InternalObjectTypeE.Array;
			this.headerList.Add(parseRecord);
			parseRecord = (ParseRecord)this.headerList[0];
			parseRecord = (ParseRecord)this.headerList[0];
			parseRecord.PRlengthA[0] = this.headerArrayLength;
			parseRecord.PRobjectPositionEnum = InternalObjectPositionE.Headers;
			for (int i = 0; i < this.headerList.Count; i++)
			{
				this.objectReader.Parse((ParseRecord)this.headerList[i]);
			}
			for (int j = 0; j < this.headerList.Count; j++)
			{
				this.PutPr((ParseRecord)this.headerList[j]);
			}
		}

		// Token: 0x04000011 RID: 17
		private SerStack stack = new SerStack("SoapParser Stack");

		// Token: 0x04000012 RID: 18
		private XmlTextReader xmlTextReader;

		// Token: 0x04000013 RID: 19
		private SoapParser soapParser;

		// Token: 0x04000014 RID: 20
		private string textValue = "";

		// Token: 0x04000015 RID: 21
		private ObjectReader objectReader;

		// Token: 0x04000016 RID: 22
		internal Hashtable keyToNamespaceTable;

		// Token: 0x04000017 RID: 23
		private InternalParseStateE currentState;

		// Token: 0x04000018 RID: 24
		private bool isEnvelope;

		// Token: 0x04000019 RID: 25
		private bool isBody;

		// Token: 0x0400001A RID: 26
		private bool isTopFound;

		// Token: 0x0400001B RID: 27
		private SoapHandler.HeaderStateEnum headerState;

		// Token: 0x0400001C RID: 28
		private SerStack attributeValues = new SerStack("AttributePrefix");

		// Token: 0x0400001D RID: 29
		private SerStack prPool = new SerStack("prPool");

		// Token: 0x0400001E RID: 30
		private Hashtable assemKeyToAssemblyTable;

		// Token: 0x0400001F RID: 31
		private Hashtable assemKeyToNameSpaceTable;

		// Token: 0x04000020 RID: 32
		private Hashtable assemKeyToInteropAssemblyTable;

		// Token: 0x04000021 RID: 33
		private Hashtable nameSpaceToKey;

		// Token: 0x04000022 RID: 34
		private string soapKey = "SOAP-ENC";

		// Token: 0x04000023 RID: 35
		private string urtKey = "urt";

		// Token: 0x04000024 RID: 36
		private string soapEnvKey = "SOAP-ENV";

		// Token: 0x04000025 RID: 37
		private string xsiKey = "xsi";

		// Token: 0x04000026 RID: 38
		private string xsdKey = "xsd";

		// Token: 0x04000027 RID: 39
		private int nextPrefix;

		// Token: 0x04000028 RID: 40
		private StringBuilder sburi = new StringBuilder(50);

		// Token: 0x04000029 RID: 41
		private StringBuilder stringBuffer = new StringBuilder(120);

		// Token: 0x0400002A RID: 42
		private NameCache nameCache = new NameCache();

		// Token: 0x0400002B RID: 43
		private ArrayList xmlAttributeList;

		// Token: 0x0400002C RID: 44
		private ArrayList headerList;

		// Token: 0x0400002D RID: 45
		private int headerArrayLength;

		// Token: 0x02000006 RID: 6
		internal class AttributeValueEntry
		{
			// Token: 0x06000038 RID: 56 RVA: 0x00004D50 File Offset: 0x00003D50
			internal AttributeValueEntry(string prefix, string key, string value, string urn)
			{
				this.prefix = prefix;
				this.key = key;
				this.value = value;
				this.urn = urn;
			}

			// Token: 0x0400002E RID: 46
			internal string prefix;

			// Token: 0x0400002F RID: 47
			internal string key;

			// Token: 0x04000030 RID: 48
			internal string value;

			// Token: 0x04000031 RID: 49
			internal string urn;
		}

		// Token: 0x02000007 RID: 7
		[Serializable]
		private enum HeaderStateEnum
		{
			// Token: 0x04000033 RID: 51
			None,
			// Token: 0x04000034 RID: 52
			FirstHeaderRecord,
			// Token: 0x04000035 RID: 53
			HeaderRecord,
			// Token: 0x04000036 RID: 54
			NestedObject,
			// Token: 0x04000037 RID: 55
			TopLevelObject
		}
	}
}
