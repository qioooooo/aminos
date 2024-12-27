using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization.Configuration;

namespace System.Xml.Serialization
{
	// Token: 0x02000324 RID: 804
	public abstract class XmlSerializationReader : XmlSerializationGeneratedCode
	{
		// Token: 0x06002672 RID: 9842 RVA: 0x000BBEAC File Offset: 0x000BAEAC
		static XmlSerializationReader()
		{
			XmlSerializerSection xmlSerializerSection = ConfigurationManager.GetSection(ConfigurationStrings.XmlSerializerSectionPath) as XmlSerializerSection;
			XmlSerializationReader.checkDeserializeAdvances = xmlSerializerSection != null && xmlSerializerSection.CheckDeserializeAdvances;
		}

		// Token: 0x06002673 RID: 9843
		protected abstract void InitIDs();

		// Token: 0x06002674 RID: 9844 RVA: 0x000BBEDC File Offset: 0x000BAEDC
		internal void Init(XmlReader r, XmlDeserializationEvents events, string encodingStyle, TempAssembly tempAssembly)
		{
			this.events = events;
			if (XmlSerializationReader.checkDeserializeAdvances)
			{
				this.countingReader = new XmlCountingReader(r);
				this.r = this.countingReader;
			}
			else
			{
				this.r = r;
			}
			this.d = null;
			this.soap12 = encodingStyle == "http://www.w3.org/2003/05/soap-encoding";
			base.Init(tempAssembly);
			this.schemaNsID = r.NameTable.Add("http://www.w3.org/2001/XMLSchema");
			this.schemaNs2000ID = r.NameTable.Add("http://www.w3.org/2000/10/XMLSchema");
			this.schemaNs1999ID = r.NameTable.Add("http://www.w3.org/1999/XMLSchema");
			this.schemaNonXsdTypesNsID = r.NameTable.Add("http://microsoft.com/wsdl/types/");
			this.instanceNsID = r.NameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
			this.instanceNs2000ID = r.NameTable.Add("http://www.w3.org/2000/10/XMLSchema-instance");
			this.instanceNs1999ID = r.NameTable.Add("http://www.w3.org/1999/XMLSchema-instance");
			this.soapNsID = r.NameTable.Add("http://schemas.xmlsoap.org/soap/encoding/");
			this.soap12NsID = r.NameTable.Add("http://www.w3.org/2003/05/soap-encoding");
			this.schemaID = r.NameTable.Add("schema");
			this.wsdlNsID = r.NameTable.Add("http://schemas.xmlsoap.org/wsdl/");
			this.wsdlArrayTypeID = r.NameTable.Add("arrayType");
			this.nullID = r.NameTable.Add("null");
			this.nilID = r.NameTable.Add("nil");
			this.typeID = r.NameTable.Add("type");
			this.arrayTypeID = r.NameTable.Add("arrayType");
			this.itemTypeID = r.NameTable.Add("itemType");
			this.arraySizeID = r.NameTable.Add("arraySize");
			this.arrayID = r.NameTable.Add("Array");
			this.urTypeID = r.NameTable.Add("anyType");
			this.InitIDs();
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x000BC0F6 File Offset: 0x000BB0F6
		// (set) Token: 0x06002676 RID: 9846 RVA: 0x000BC0FE File Offset: 0x000BB0FE
		protected bool DecodeName
		{
			get
			{
				return this.decodeName;
			}
			set
			{
				this.decodeName = value;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06002677 RID: 9847 RVA: 0x000BC107 File Offset: 0x000BB107
		protected XmlReader Reader
		{
			get
			{
				return this.r;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x000BC10F File Offset: 0x000BB10F
		protected int ReaderCount
		{
			get
			{
				if (!XmlSerializationReader.checkDeserializeAdvances)
				{
					return 0;
				}
				return this.countingReader.AdvanceCount;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06002679 RID: 9849 RVA: 0x000BC125 File Offset: 0x000BB125
		protected XmlDocument Document
		{
			get
			{
				if (this.d == null)
				{
					this.d = new XmlDocument(this.r.NameTable);
					this.d.SetBaseURI(this.r.BaseURI);
				}
				return this.d;
			}
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000BC161 File Offset: 0x000BB161
		protected static Assembly ResolveDynamicAssembly(string assemblyFullName)
		{
			return DynamicAssemblies.Get(assemblyFullName);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000BC16C File Offset: 0x000BB16C
		private void InitPrimitiveIDs()
		{
			if (this.tokenID != null)
			{
				return;
			}
			this.r.NameTable.Add("http://www.w3.org/2001/XMLSchema");
			this.r.NameTable.Add("http://microsoft.com/wsdl/types/");
			this.stringID = this.r.NameTable.Add("string");
			this.intID = this.r.NameTable.Add("int");
			this.booleanID = this.r.NameTable.Add("boolean");
			this.shortID = this.r.NameTable.Add("short");
			this.longID = this.r.NameTable.Add("long");
			this.floatID = this.r.NameTable.Add("float");
			this.doubleID = this.r.NameTable.Add("double");
			this.decimalID = this.r.NameTable.Add("decimal");
			this.dateTimeID = this.r.NameTable.Add("dateTime");
			this.qnameID = this.r.NameTable.Add("QName");
			this.dateID = this.r.NameTable.Add("date");
			this.timeID = this.r.NameTable.Add("time");
			this.hexBinaryID = this.r.NameTable.Add("hexBinary");
			this.base64BinaryID = this.r.NameTable.Add("base64Binary");
			this.unsignedByteID = this.r.NameTable.Add("unsignedByte");
			this.byteID = this.r.NameTable.Add("byte");
			this.unsignedShortID = this.r.NameTable.Add("unsignedShort");
			this.unsignedIntID = this.r.NameTable.Add("unsignedInt");
			this.unsignedLongID = this.r.NameTable.Add("unsignedLong");
			this.oldDecimalID = this.r.NameTable.Add("decimal");
			this.oldTimeInstantID = this.r.NameTable.Add("timeInstant");
			this.charID = this.r.NameTable.Add("char");
			this.guidID = this.r.NameTable.Add("guid");
			this.base64ID = this.r.NameTable.Add("base64");
			this.anyURIID = this.r.NameTable.Add("anyURI");
			this.durationID = this.r.NameTable.Add("duration");
			this.ENTITYID = this.r.NameTable.Add("ENTITY");
			this.ENTITIESID = this.r.NameTable.Add("ENTITIES");
			this.gDayID = this.r.NameTable.Add("gDay");
			this.gMonthID = this.r.NameTable.Add("gMonth");
			this.gMonthDayID = this.r.NameTable.Add("gMonthDay");
			this.gYearID = this.r.NameTable.Add("gYear");
			this.gYearMonthID = this.r.NameTable.Add("gYearMonth");
			this.IDID = this.r.NameTable.Add("ID");
			this.IDREFID = this.r.NameTable.Add("IDREF");
			this.IDREFSID = this.r.NameTable.Add("IDREFS");
			this.integerID = this.r.NameTable.Add("integer");
			this.languageID = this.r.NameTable.Add("language");
			this.NameID = this.r.NameTable.Add("Name");
			this.NCNameID = this.r.NameTable.Add("NCName");
			this.NMTOKENID = this.r.NameTable.Add("NMTOKEN");
			this.NMTOKENSID = this.r.NameTable.Add("NMTOKENS");
			this.negativeIntegerID = this.r.NameTable.Add("negativeInteger");
			this.nonNegativeIntegerID = this.r.NameTable.Add("nonNegativeInteger");
			this.nonPositiveIntegerID = this.r.NameTable.Add("nonPositiveInteger");
			this.normalizedStringID = this.r.NameTable.Add("normalizedString");
			this.NOTATIONID = this.r.NameTable.Add("NOTATION");
			this.positiveIntegerID = this.r.NameTable.Add("positiveInteger");
			this.tokenID = this.r.NameTable.Add("token");
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000BC6DC File Offset: 0x000BB6DC
		protected XmlQualifiedName GetXsiType()
		{
			string text = this.r.GetAttribute(this.typeID, this.instanceNsID);
			if (text == null)
			{
				text = this.r.GetAttribute(this.typeID, this.instanceNs2000ID);
				if (text == null)
				{
					text = this.r.GetAttribute(this.typeID, this.instanceNs1999ID);
					if (text == null)
					{
						return null;
					}
				}
			}
			return this.ToXmlQualifiedName(text, false);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000BC744 File Offset: 0x000BB744
		private Type GetPrimitiveType(XmlQualifiedName typeName, bool throwOnUnknown)
		{
			this.InitPrimitiveIDs();
			if (typeName.Namespace == this.schemaNsID || typeName.Namespace == this.soapNsID || typeName.Namespace == this.soap12NsID)
			{
				if (typeName.Name == this.stringID || typeName.Name == this.anyURIID || typeName.Name == this.durationID || typeName.Name == this.ENTITYID || typeName.Name == this.ENTITIESID || typeName.Name == this.gDayID || typeName.Name == this.gMonthID || typeName.Name == this.gMonthDayID || typeName.Name == this.gYearID || typeName.Name == this.gYearMonthID || typeName.Name == this.IDID || typeName.Name == this.IDREFID || typeName.Name == this.IDREFSID || typeName.Name == this.integerID || typeName.Name == this.languageID || typeName.Name == this.NameID || typeName.Name == this.NCNameID || typeName.Name == this.NMTOKENID || typeName.Name == this.NMTOKENSID || typeName.Name == this.negativeIntegerID || typeName.Name == this.nonPositiveIntegerID || typeName.Name == this.nonNegativeIntegerID || typeName.Name == this.normalizedStringID || typeName.Name == this.NOTATIONID || typeName.Name == this.positiveIntegerID || typeName.Name == this.tokenID)
				{
					return typeof(string);
				}
				if (typeName.Name == this.intID)
				{
					return typeof(int);
				}
				if (typeName.Name == this.booleanID)
				{
					return typeof(bool);
				}
				if (typeName.Name == this.shortID)
				{
					return typeof(short);
				}
				if (typeName.Name == this.longID)
				{
					return typeof(long);
				}
				if (typeName.Name == this.floatID)
				{
					return typeof(float);
				}
				if (typeName.Name == this.doubleID)
				{
					return typeof(double);
				}
				if (typeName.Name == this.decimalID)
				{
					return typeof(decimal);
				}
				if (typeName.Name == this.dateTimeID)
				{
					return typeof(DateTime);
				}
				if (typeName.Name == this.qnameID)
				{
					return typeof(XmlQualifiedName);
				}
				if (typeName.Name == this.dateID)
				{
					return typeof(DateTime);
				}
				if (typeName.Name == this.timeID)
				{
					return typeof(DateTime);
				}
				if (typeName.Name == this.hexBinaryID)
				{
					return typeof(byte[]);
				}
				if (typeName.Name == this.base64BinaryID)
				{
					return typeof(byte[]);
				}
				if (typeName.Name == this.unsignedByteID)
				{
					return typeof(byte);
				}
				if (typeName.Name == this.byteID)
				{
					return typeof(sbyte);
				}
				if (typeName.Name == this.unsignedShortID)
				{
					return typeof(ushort);
				}
				if (typeName.Name == this.unsignedIntID)
				{
					return typeof(uint);
				}
				if (typeName.Name == this.unsignedLongID)
				{
					return typeof(ulong);
				}
				throw this.CreateUnknownTypeException(typeName);
			}
			else if (typeName.Namespace == this.schemaNs2000ID || typeName.Namespace == this.schemaNs1999ID)
			{
				if (typeName.Name == this.stringID || typeName.Name == this.anyURIID || typeName.Name == this.durationID || typeName.Name == this.ENTITYID || typeName.Name == this.ENTITIESID || typeName.Name == this.gDayID || typeName.Name == this.gMonthID || typeName.Name == this.gMonthDayID || typeName.Name == this.gYearID || typeName.Name == this.gYearMonthID || typeName.Name == this.IDID || typeName.Name == this.IDREFID || typeName.Name == this.IDREFSID || typeName.Name == this.integerID || typeName.Name == this.languageID || typeName.Name == this.NameID || typeName.Name == this.NCNameID || typeName.Name == this.NMTOKENID || typeName.Name == this.NMTOKENSID || typeName.Name == this.negativeIntegerID || typeName.Name == this.nonPositiveIntegerID || typeName.Name == this.nonNegativeIntegerID || typeName.Name == this.normalizedStringID || typeName.Name == this.NOTATIONID || typeName.Name == this.positiveIntegerID || typeName.Name == this.tokenID)
				{
					return typeof(string);
				}
				if (typeName.Name == this.intID)
				{
					return typeof(int);
				}
				if (typeName.Name == this.booleanID)
				{
					return typeof(bool);
				}
				if (typeName.Name == this.shortID)
				{
					return typeof(short);
				}
				if (typeName.Name == this.longID)
				{
					return typeof(long);
				}
				if (typeName.Name == this.floatID)
				{
					return typeof(float);
				}
				if (typeName.Name == this.doubleID)
				{
					return typeof(double);
				}
				if (typeName.Name == this.oldDecimalID)
				{
					return typeof(decimal);
				}
				if (typeName.Name == this.oldTimeInstantID)
				{
					return typeof(DateTime);
				}
				if (typeName.Name == this.qnameID)
				{
					return typeof(XmlQualifiedName);
				}
				if (typeName.Name == this.dateID)
				{
					return typeof(DateTime);
				}
				if (typeName.Name == this.timeID)
				{
					return typeof(DateTime);
				}
				if (typeName.Name == this.hexBinaryID)
				{
					return typeof(byte[]);
				}
				if (typeName.Name == this.byteID)
				{
					return typeof(sbyte);
				}
				if (typeName.Name == this.unsignedShortID)
				{
					return typeof(ushort);
				}
				if (typeName.Name == this.unsignedIntID)
				{
					return typeof(uint);
				}
				if (typeName.Name == this.unsignedLongID)
				{
					return typeof(ulong);
				}
				throw this.CreateUnknownTypeException(typeName);
			}
			else if (typeName.Namespace == this.schemaNonXsdTypesNsID)
			{
				if (typeName.Name == this.charID)
				{
					return typeof(char);
				}
				if (typeName.Name == this.guidID)
				{
					return typeof(Guid);
				}
				throw this.CreateUnknownTypeException(typeName);
			}
			else
			{
				if (throwOnUnknown)
				{
					throw this.CreateUnknownTypeException(typeName);
				}
				return null;
			}
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000BCEA7 File Offset: 0x000BBEA7
		private bool IsPrimitiveNamespace(string ns)
		{
			return ns == this.schemaNsID || ns == this.schemaNonXsdTypesNsID || ns == this.soapNsID || ns == this.soap12NsID || ns == this.schemaNs2000ID || ns == this.schemaNs1999ID;
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000BCEE4 File Offset: 0x000BBEE4
		private string ReadStringValue()
		{
			if (this.r.IsEmptyElement)
			{
				this.r.Skip();
				return string.Empty;
			}
			this.r.ReadStartElement();
			string text = this.r.ReadString();
			this.ReadEndElement();
			return text;
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000BCF30 File Offset: 0x000BBF30
		private XmlQualifiedName ReadXmlQualifiedName()
		{
			bool flag = false;
			string text;
			if (this.r.IsEmptyElement)
			{
				text = string.Empty;
				flag = true;
			}
			else
			{
				this.r.ReadStartElement();
				text = this.r.ReadString();
			}
			XmlQualifiedName xmlQualifiedName = this.ToXmlQualifiedName(text);
			if (flag)
			{
				this.r.Skip();
			}
			else
			{
				this.ReadEndElement();
			}
			return xmlQualifiedName;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000BCF8C File Offset: 0x000BBF8C
		private byte[] ReadByteArray(bool isBase64)
		{
			ArrayList arrayList = new ArrayList();
			int num = 1024;
			int num2 = -1;
			int num3 = 0;
			int num4 = 0;
			byte[] array = new byte[num];
			arrayList.Add(array);
			while (num2 != 0)
			{
				if (num3 == array.Length)
				{
					num = Math.Min(num * 2, 65536);
					array = new byte[num];
					num3 = 0;
					arrayList.Add(array);
				}
				if (isBase64)
				{
					num2 = this.r.ReadElementContentAsBase64(array, num3, array.Length - num3);
				}
				else
				{
					num2 = this.r.ReadElementContentAsBinHex(array, num3, array.Length - num3);
				}
				num3 += num2;
				num4 += num2;
			}
			byte[] array2 = new byte[num4];
			num3 = 0;
			foreach (object obj in arrayList)
			{
				byte[] array3 = (byte[])obj;
				num = Math.Min(array3.Length, num4);
				if (num > 0)
				{
					Buffer.BlockCopy(array3, 0, array2, num3, num);
					num3 += num;
					num4 -= num;
				}
			}
			arrayList.Clear();
			return array2;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000BD0A8 File Offset: 0x000BC0A8
		protected object ReadTypedPrimitive(XmlQualifiedName type)
		{
			return this.ReadTypedPrimitive(type, false);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000BD0B4 File Offset: 0x000BC0B4
		private object ReadTypedPrimitive(XmlQualifiedName type, bool elementCanBeType)
		{
			this.InitPrimitiveIDs();
			if (!this.IsPrimitiveNamespace(type.Namespace) || type.Name == this.urTypeID)
			{
				return this.ReadXmlNodes(elementCanBeType);
			}
			object obj;
			if (type.Namespace == this.schemaNsID || type.Namespace == this.soapNsID || type.Namespace == this.soap12NsID)
			{
				if (type.Name == this.stringID || type.Name == this.normalizedStringID)
				{
					obj = this.ReadStringValue();
				}
				else if (type.Name == this.anyURIID || type.Name == this.durationID || type.Name == this.ENTITYID || type.Name == this.ENTITIESID || type.Name == this.gDayID || type.Name == this.gMonthID || type.Name == this.gMonthDayID || type.Name == this.gYearID || type.Name == this.gYearMonthID || type.Name == this.IDID || type.Name == this.IDREFID || type.Name == this.IDREFSID || type.Name == this.integerID || type.Name == this.languageID || type.Name == this.NameID || type.Name == this.NCNameID || type.Name == this.NMTOKENID || type.Name == this.NMTOKENSID || type.Name == this.negativeIntegerID || type.Name == this.nonPositiveIntegerID || type.Name == this.nonNegativeIntegerID || type.Name == this.NOTATIONID || type.Name == this.positiveIntegerID || type.Name == this.tokenID)
				{
					obj = this.CollapseWhitespace(this.ReadStringValue());
				}
				else if (type.Name == this.intID)
				{
					obj = XmlConvert.ToInt32(this.ReadStringValue());
				}
				else if (type.Name == this.booleanID)
				{
					obj = XmlConvert.ToBoolean(this.ReadStringValue());
				}
				else if (type.Name == this.shortID)
				{
					obj = XmlConvert.ToInt16(this.ReadStringValue());
				}
				else if (type.Name == this.longID)
				{
					obj = XmlConvert.ToInt64(this.ReadStringValue());
				}
				else if (type.Name == this.floatID)
				{
					obj = XmlConvert.ToSingle(this.ReadStringValue());
				}
				else if (type.Name == this.doubleID)
				{
					obj = XmlConvert.ToDouble(this.ReadStringValue());
				}
				else if (type.Name == this.decimalID)
				{
					obj = XmlConvert.ToDecimal(this.ReadStringValue());
				}
				else if (type.Name == this.dateTimeID)
				{
					obj = XmlSerializationReader.ToDateTime(this.ReadStringValue());
				}
				else if (type.Name == this.qnameID)
				{
					obj = this.ReadXmlQualifiedName();
				}
				else if (type.Name == this.dateID)
				{
					obj = XmlSerializationReader.ToDate(this.ReadStringValue());
				}
				else if (type.Name == this.timeID)
				{
					obj = XmlSerializationReader.ToTime(this.ReadStringValue());
				}
				else if (type.Name == this.unsignedByteID)
				{
					obj = XmlConvert.ToByte(this.ReadStringValue());
				}
				else if (type.Name == this.byteID)
				{
					obj = XmlConvert.ToSByte(this.ReadStringValue());
				}
				else if (type.Name == this.unsignedShortID)
				{
					obj = XmlConvert.ToUInt16(this.ReadStringValue());
				}
				else if (type.Name == this.unsignedIntID)
				{
					obj = XmlConvert.ToUInt32(this.ReadStringValue());
				}
				else if (type.Name == this.unsignedLongID)
				{
					obj = XmlConvert.ToUInt64(this.ReadStringValue());
				}
				else if (type.Name == this.hexBinaryID)
				{
					obj = this.ToByteArrayHex(false);
				}
				else if (type.Name == this.base64BinaryID)
				{
					obj = this.ToByteArrayBase64(false);
				}
				else if (type.Name == this.base64ID && (type.Namespace == this.soapNsID || type.Namespace == this.soap12NsID))
				{
					obj = this.ToByteArrayBase64(false);
				}
				else
				{
					obj = this.ReadXmlNodes(elementCanBeType);
				}
			}
			else if (type.Namespace == this.schemaNs2000ID || type.Namespace == this.schemaNs1999ID)
			{
				if (type.Name == this.stringID || type.Name == this.normalizedStringID)
				{
					obj = this.ReadStringValue();
				}
				else if (type.Name == this.anyURIID || type.Name == this.anyURIID || type.Name == this.durationID || type.Name == this.ENTITYID || type.Name == this.ENTITIESID || type.Name == this.gDayID || type.Name == this.gMonthID || type.Name == this.gMonthDayID || type.Name == this.gYearID || type.Name == this.gYearMonthID || type.Name == this.IDID || type.Name == this.IDREFID || type.Name == this.IDREFSID || type.Name == this.integerID || type.Name == this.languageID || type.Name == this.NameID || type.Name == this.NCNameID || type.Name == this.NMTOKENID || type.Name == this.NMTOKENSID || type.Name == this.negativeIntegerID || type.Name == this.nonPositiveIntegerID || type.Name == this.nonNegativeIntegerID || type.Name == this.NOTATIONID || type.Name == this.positiveIntegerID || type.Name == this.tokenID)
				{
					obj = this.CollapseWhitespace(this.ReadStringValue());
				}
				else if (type.Name == this.intID)
				{
					obj = XmlConvert.ToInt32(this.ReadStringValue());
				}
				else if (type.Name == this.booleanID)
				{
					obj = XmlConvert.ToBoolean(this.ReadStringValue());
				}
				else if (type.Name == this.shortID)
				{
					obj = XmlConvert.ToInt16(this.ReadStringValue());
				}
				else if (type.Name == this.longID)
				{
					obj = XmlConvert.ToInt64(this.ReadStringValue());
				}
				else if (type.Name == this.floatID)
				{
					obj = XmlConvert.ToSingle(this.ReadStringValue());
				}
				else if (type.Name == this.doubleID)
				{
					obj = XmlConvert.ToDouble(this.ReadStringValue());
				}
				else if (type.Name == this.oldDecimalID)
				{
					obj = XmlConvert.ToDecimal(this.ReadStringValue());
				}
				else if (type.Name == this.oldTimeInstantID)
				{
					obj = XmlSerializationReader.ToDateTime(this.ReadStringValue());
				}
				else if (type.Name == this.qnameID)
				{
					obj = this.ReadXmlQualifiedName();
				}
				else if (type.Name == this.dateID)
				{
					obj = XmlSerializationReader.ToDate(this.ReadStringValue());
				}
				else if (type.Name == this.timeID)
				{
					obj = XmlSerializationReader.ToTime(this.ReadStringValue());
				}
				else if (type.Name == this.unsignedByteID)
				{
					obj = XmlConvert.ToByte(this.ReadStringValue());
				}
				else if (type.Name == this.byteID)
				{
					obj = XmlConvert.ToSByte(this.ReadStringValue());
				}
				else if (type.Name == this.unsignedShortID)
				{
					obj = XmlConvert.ToUInt16(this.ReadStringValue());
				}
				else if (type.Name == this.unsignedIntID)
				{
					obj = XmlConvert.ToUInt32(this.ReadStringValue());
				}
				else if (type.Name == this.unsignedLongID)
				{
					obj = XmlConvert.ToUInt64(this.ReadStringValue());
				}
				else
				{
					obj = this.ReadXmlNodes(elementCanBeType);
				}
			}
			else if (type.Namespace == this.schemaNonXsdTypesNsID)
			{
				if (type.Name == this.charID)
				{
					obj = XmlSerializationReader.ToChar(this.ReadStringValue());
				}
				else if (type.Name == this.guidID)
				{
					obj = new Guid(this.CollapseWhitespace(this.ReadStringValue()));
				}
				else
				{
					obj = this.ReadXmlNodes(elementCanBeType);
				}
			}
			else
			{
				obj = this.ReadXmlNodes(elementCanBeType);
			}
			return obj;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000BDA08 File Offset: 0x000BCA08
		protected object ReadTypedNull(XmlQualifiedName type)
		{
			this.InitPrimitiveIDs();
			if (!this.IsPrimitiveNamespace(type.Namespace) || type.Name == this.urTypeID)
			{
				return null;
			}
			object obj;
			if (type.Namespace == this.schemaNsID || type.Namespace == this.soapNsID || type.Namespace == this.soap12NsID)
			{
				if (type.Name == this.stringID || type.Name == this.anyURIID || type.Name == this.durationID || type.Name == this.ENTITYID || type.Name == this.ENTITIESID || type.Name == this.gDayID || type.Name == this.gMonthID || type.Name == this.gMonthDayID || type.Name == this.gYearID || type.Name == this.gYearMonthID || type.Name == this.IDID || type.Name == this.IDREFID || type.Name == this.IDREFSID || type.Name == this.integerID || type.Name == this.languageID || type.Name == this.NameID || type.Name == this.NCNameID || type.Name == this.NMTOKENID || type.Name == this.NMTOKENSID || type.Name == this.negativeIntegerID || type.Name == this.nonPositiveIntegerID || type.Name == this.nonNegativeIntegerID || type.Name == this.normalizedStringID || type.Name == this.NOTATIONID || type.Name == this.positiveIntegerID || type.Name == this.tokenID)
				{
					obj = null;
				}
				else if (type.Name == this.intID)
				{
					obj = null;
				}
				else if (type.Name == this.booleanID)
				{
					obj = null;
				}
				else if (type.Name == this.shortID)
				{
					obj = null;
				}
				else if (type.Name == this.longID)
				{
					obj = null;
				}
				else if (type.Name == this.floatID)
				{
					obj = null;
				}
				else if (type.Name == this.doubleID)
				{
					obj = null;
				}
				else if (type.Name == this.decimalID)
				{
					obj = null;
				}
				else if (type.Name == this.dateTimeID)
				{
					obj = null;
				}
				else if (type.Name == this.qnameID)
				{
					obj = null;
				}
				else if (type.Name == this.dateID)
				{
					obj = null;
				}
				else if (type.Name == this.timeID)
				{
					obj = null;
				}
				else if (type.Name == this.unsignedByteID)
				{
					obj = null;
				}
				else if (type.Name == this.byteID)
				{
					obj = null;
				}
				else if (type.Name == this.unsignedShortID)
				{
					obj = null;
				}
				else if (type.Name == this.unsignedIntID)
				{
					obj = null;
				}
				else if (type.Name == this.unsignedLongID)
				{
					obj = null;
				}
				else if (type.Name == this.hexBinaryID)
				{
					obj = null;
				}
				else if (type.Name == this.base64BinaryID)
				{
					obj = null;
				}
				else if (type.Name == this.base64ID && (type.Namespace == this.soapNsID || type.Namespace == this.soap12NsID))
				{
					obj = null;
				}
				else
				{
					obj = null;
				}
			}
			else if (type.Namespace == this.schemaNonXsdTypesNsID)
			{
				if (type.Name == this.charID)
				{
					obj = null;
				}
				else if (type.Name == this.guidID)
				{
					obj = null;
				}
				else
				{
					obj = null;
				}
			}
			else
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000BDEDB File Offset: 0x000BCEDB
		protected bool IsXmlnsAttribute(string name)
		{
			return name.StartsWith("xmlns", StringComparison.Ordinal) && (name.Length == 5 || name[5] == ':');
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000BDF04 File Offset: 0x000BCF04
		protected void ParseWsdlArrayType(XmlAttribute attr)
		{
			if (attr.LocalName == this.wsdlArrayTypeID && attr.NamespaceURI == this.wsdlNsID)
			{
				int num = attr.Value.LastIndexOf(':');
				if (num < 0)
				{
					attr.Value = this.r.LookupNamespace("") + ":" + attr.Value;
					return;
				}
				attr.Value = this.r.LookupNamespace(attr.Value.Substring(0, num)) + ":" + attr.Value.Substring(num + 1);
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06002687 RID: 9863 RVA: 0x000BDF9C File Offset: 0x000BCF9C
		// (set) Token: 0x06002688 RID: 9864 RVA: 0x000BDFB1 File Offset: 0x000BCFB1
		protected bool IsReturnValue
		{
			get
			{
				return this.isReturnValue && !this.soap12;
			}
			set
			{
				this.isReturnValue = value;
			}
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000BDFBC File Offset: 0x000BCFBC
		protected bool ReadNull()
		{
			if (!this.GetNullAttr())
			{
				return false;
			}
			if (this.r.IsEmptyElement)
			{
				this.r.Skip();
				return true;
			}
			this.r.ReadStartElement();
			int num = 0;
			int readerCount = this.ReaderCount;
			while (this.r.NodeType != XmlNodeType.EndElement)
			{
				this.UnknownNode(null);
				this.CheckReaderCount(ref num, ref readerCount);
			}
			this.ReadEndElement();
			return true;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000BE02C File Offset: 0x000BD02C
		protected bool GetNullAttr()
		{
			string text = this.r.GetAttribute(this.nilID, this.instanceNsID);
			if (text == null)
			{
				text = this.r.GetAttribute(this.nullID, this.instanceNsID);
			}
			if (text == null)
			{
				text = this.r.GetAttribute(this.nullID, this.instanceNs2000ID);
				if (text == null)
				{
					text = this.r.GetAttribute(this.nullID, this.instanceNs1999ID);
				}
			}
			return text != null && XmlConvert.ToBoolean(text);
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000BE0B0 File Offset: 0x000BD0B0
		protected string ReadNullableString()
		{
			if (this.ReadNull())
			{
				return null;
			}
			return this.r.ReadElementString();
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000BE0C7 File Offset: 0x000BD0C7
		protected XmlQualifiedName ReadNullableQualifiedName()
		{
			if (this.ReadNull())
			{
				return null;
			}
			return this.ReadElementQualifiedName();
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000BE0DC File Offset: 0x000BD0DC
		protected XmlQualifiedName ReadElementQualifiedName()
		{
			if (this.r.IsEmptyElement)
			{
				XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(string.Empty, this.r.LookupNamespace(""));
				this.r.Skip();
				return xmlQualifiedName;
			}
			XmlQualifiedName xmlQualifiedName2 = this.ToXmlQualifiedName(this.CollapseWhitespace(this.r.ReadString()));
			this.r.ReadEndElement();
			return xmlQualifiedName2;
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000BE144 File Offset: 0x000BD144
		protected XmlDocument ReadXmlDocument(bool wrapped)
		{
			XmlNode xmlNode = this.ReadXmlNode(wrapped);
			if (xmlNode == null)
			{
				return null;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.ImportNode(xmlNode, true));
			return xmlDocument;
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000BE174 File Offset: 0x000BD174
		protected string CollapseWhitespace(string value)
		{
			if (value == null)
			{
				return null;
			}
			return value.Trim();
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000BE184 File Offset: 0x000BD184
		protected XmlNode ReadXmlNode(bool wrapped)
		{
			XmlNode xmlNode = null;
			if (wrapped)
			{
				if (this.ReadNull())
				{
					return null;
				}
				this.r.ReadStartElement();
				this.r.MoveToContent();
				if (this.r.NodeType != XmlNodeType.EndElement)
				{
					xmlNode = this.Document.ReadNode(this.r);
				}
				int num = 0;
				int readerCount = this.ReaderCount;
				while (this.r.NodeType != XmlNodeType.EndElement)
				{
					this.UnknownNode(null);
					this.CheckReaderCount(ref num, ref readerCount);
				}
				this.r.ReadEndElement();
			}
			else
			{
				xmlNode = this.Document.ReadNode(this.r);
			}
			return xmlNode;
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000BE223 File Offset: 0x000BD223
		protected static byte[] ToByteArrayBase64(string value)
		{
			return XmlCustomFormatter.ToByteArrayBase64(value);
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000BE22B File Offset: 0x000BD22B
		protected byte[] ToByteArrayBase64(bool isNull)
		{
			if (isNull)
			{
				return null;
			}
			return this.ReadByteArray(true);
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000BE239 File Offset: 0x000BD239
		protected static byte[] ToByteArrayHex(string value)
		{
			return XmlCustomFormatter.ToByteArrayHex(value);
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000BE241 File Offset: 0x000BD241
		protected byte[] ToByteArrayHex(bool isNull)
		{
			if (isNull)
			{
				return null;
			}
			return this.ReadByteArray(false);
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000BE250 File Offset: 0x000BD250
		protected int GetArrayLength(string name, string ns)
		{
			if (this.GetNullAttr())
			{
				return 0;
			}
			string attribute = this.r.GetAttribute(this.arrayTypeID, this.soapNsID);
			XmlSerializationReader.SoapArrayInfo soapArrayInfo = this.ParseArrayType(attribute);
			if (soapArrayInfo.dimensions != 1)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidArrayDimentions", new object[] { this.CurrentTag() }));
			}
			XmlQualifiedName xmlQualifiedName = this.ToXmlQualifiedName(soapArrayInfo.qname, false);
			if (xmlQualifiedName.Name != name)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidArrayTypeName", new object[]
				{
					xmlQualifiedName.Name,
					name,
					this.CurrentTag()
				}));
			}
			if (xmlQualifiedName.Namespace != ns)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidArrayTypeNamespace", new object[]
				{
					xmlQualifiedName.Namespace,
					ns,
					this.CurrentTag()
				}));
			}
			return soapArrayInfo.length;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000BE34C File Offset: 0x000BD34C
		private XmlSerializationReader.SoapArrayInfo ParseArrayType(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(Res.GetString("XmlMissingArrayType", new object[] { this.CurrentTag() }));
			}
			if (value.Length == 0)
			{
				throw new ArgumentException(Res.GetString("XmlEmptyArrayType", new object[] { this.CurrentTag() }), "value");
			}
			char[] array = value.ToCharArray();
			int num = array.Length;
			XmlSerializationReader.SoapArrayInfo soapArrayInfo = default(XmlSerializationReader.SoapArrayInfo);
			int num2 = num - 1;
			if (array[num2] != ']')
			{
				throw new ArgumentException(Res.GetString("XmlInvalidArraySyntax"), "value");
			}
			num2--;
			while (num2 != -1 && array[num2] != '[')
			{
				if (array[num2] == ',')
				{
					throw new ArgumentException(Res.GetString("XmlInvalidArrayDimentions", new object[] { this.CurrentTag() }), "value");
				}
				num2--;
			}
			if (num2 == -1)
			{
				throw new ArgumentException(Res.GetString("XmlMismatchedArrayBrackets"), "value");
			}
			int num3 = num - num2 - 2;
			if (num3 > 0)
			{
				string text = new string(array, num2 + 1, num3);
				try
				{
					soapArrayInfo.length = int.Parse(text, CultureInfo.InvariantCulture);
					goto IL_018C;
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					throw new ArgumentException(Res.GetString("XmlInvalidArrayLength", new object[] { text }), "value");
				}
				catch
				{
					throw new ArgumentException(Res.GetString("XmlInvalidArrayLength", new object[] { text }), "value");
				}
			}
			soapArrayInfo.length = -1;
			IL_018C:
			num2--;
			soapArrayInfo.jaggedDimensions = 0;
			while (num2 != -1 && array[num2] == ']')
			{
				num2--;
				if (num2 < 0)
				{
					throw new ArgumentException(Res.GetString("XmlMismatchedArrayBrackets"), "value");
				}
				if (array[num2] == ',')
				{
					throw new ArgumentException(Res.GetString("XmlInvalidArrayDimentions", new object[] { this.CurrentTag() }), "value");
				}
				if (array[num2] != '[')
				{
					throw new ArgumentException(Res.GetString("XmlInvalidArraySyntax"), "value");
				}
				num2--;
				soapArrayInfo.jaggedDimensions++;
			}
			soapArrayInfo.dimensions = 1;
			soapArrayInfo.qname = new string(array, 0, num2 + 1);
			return soapArrayInfo;
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x000BE5B4 File Offset: 0x000BD5B4
		private XmlSerializationReader.SoapArrayInfo ParseSoap12ArrayType(string itemType, string arraySize)
		{
			XmlSerializationReader.SoapArrayInfo soapArrayInfo = default(XmlSerializationReader.SoapArrayInfo);
			if (itemType != null && itemType.Length > 0)
			{
				soapArrayInfo.qname = itemType;
			}
			else
			{
				soapArrayInfo.qname = "";
			}
			string[] array;
			if (arraySize != null && arraySize.Length > 0)
			{
				array = arraySize.Split(null);
			}
			else
			{
				array = new string[0];
			}
			soapArrayInfo.dimensions = 0;
			soapArrayInfo.length = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length > 0)
				{
					if (array[i] == "*")
					{
						soapArrayInfo.dimensions++;
					}
					else
					{
						try
						{
							soapArrayInfo.length = int.Parse(array[i], CultureInfo.InvariantCulture);
							soapArrayInfo.dimensions++;
						}
						catch (Exception ex)
						{
							if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
							{
								throw;
							}
							throw new ArgumentException(Res.GetString("XmlInvalidArrayLength", new object[] { array[i] }), "value");
						}
						catch
						{
							throw new ArgumentException(Res.GetString("XmlInvalidArrayLength", new object[] { array[i] }), "value");
						}
					}
				}
			}
			if (soapArrayInfo.dimensions == 0)
			{
				soapArrayInfo.dimensions = 1;
			}
			return soapArrayInfo;
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x000BE718 File Offset: 0x000BD718
		protected static DateTime ToDateTime(string value)
		{
			return XmlCustomFormatter.ToDateTime(value);
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x000BE720 File Offset: 0x000BD720
		protected static DateTime ToDate(string value)
		{
			return XmlCustomFormatter.ToDate(value);
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x000BE728 File Offset: 0x000BD728
		protected static DateTime ToTime(string value)
		{
			return XmlCustomFormatter.ToTime(value);
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000BE730 File Offset: 0x000BD730
		protected static char ToChar(string value)
		{
			return XmlCustomFormatter.ToChar(value);
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000BE738 File Offset: 0x000BD738
		protected static long ToEnum(string value, Hashtable h, string typeName)
		{
			return XmlCustomFormatter.ToEnum(value, h, typeName, true);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000BE743 File Offset: 0x000BD743
		protected static string ToXmlName(string value)
		{
			return XmlCustomFormatter.ToXmlName(value);
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000BE74B File Offset: 0x000BD74B
		protected static string ToXmlNCName(string value)
		{
			return XmlCustomFormatter.ToXmlNCName(value);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000BE753 File Offset: 0x000BD753
		protected static string ToXmlNmToken(string value)
		{
			return XmlCustomFormatter.ToXmlNmToken(value);
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000BE75B File Offset: 0x000BD75B
		protected static string ToXmlNmTokens(string value)
		{
			return XmlCustomFormatter.ToXmlNmTokens(value);
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x000BE763 File Offset: 0x000BD763
		protected XmlQualifiedName ToXmlQualifiedName(string value)
		{
			return this.ToXmlQualifiedName(value, this.DecodeName);
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x000BE774 File Offset: 0x000BD774
		internal XmlQualifiedName ToXmlQualifiedName(string value, bool decodeName)
		{
			int num = ((value == null) ? (-1) : value.LastIndexOf(':'));
			string text = ((num < 0) ? null : value.Substring(0, num));
			string text2 = value.Substring(num + 1);
			if (decodeName)
			{
				text = XmlConvert.DecodeName(text);
				text2 = XmlConvert.DecodeName(text2);
			}
			if (text == null || text.Length == 0)
			{
				return new XmlQualifiedName(this.r.NameTable.Add(value), this.r.LookupNamespace(string.Empty));
			}
			string text3 = this.r.LookupNamespace(text);
			if (text3 == null)
			{
				throw new InvalidOperationException(Res.GetString("XmlUndefinedAlias", new object[] { text }));
			}
			return new XmlQualifiedName(this.r.NameTable.Add(text2), text3);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000BE833 File Offset: 0x000BD833
		protected void UnknownAttribute(object o, XmlAttribute attr)
		{
			this.UnknownAttribute(o, attr, null);
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000BE840 File Offset: 0x000BD840
		protected void UnknownAttribute(object o, XmlAttribute attr, string qnames)
		{
			if (this.events.OnUnknownAttribute != null)
			{
				int num;
				int num2;
				this.GetCurrentPosition(out num, out num2);
				XmlAttributeEventArgs xmlAttributeEventArgs = new XmlAttributeEventArgs(attr, num, num2, o, qnames);
				this.events.OnUnknownAttribute(this.events.sender, xmlAttributeEventArgs);
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000BE88B File Offset: 0x000BD88B
		protected void UnknownElement(object o, XmlElement elem)
		{
			this.UnknownElement(o, elem, null);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000BE898 File Offset: 0x000BD898
		protected void UnknownElement(object o, XmlElement elem, string qnames)
		{
			if (this.events.OnUnknownElement != null)
			{
				int num;
				int num2;
				this.GetCurrentPosition(out num, out num2);
				XmlElementEventArgs xmlElementEventArgs = new XmlElementEventArgs(elem, num, num2, o, qnames);
				this.events.OnUnknownElement(this.events.sender, xmlElementEventArgs);
			}
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000BE8E3 File Offset: 0x000BD8E3
		protected void UnknownNode(object o)
		{
			this.UnknownNode(o, null);
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000BE8F0 File Offset: 0x000BD8F0
		protected void UnknownNode(object o, string qnames)
		{
			if (this.r.NodeType == XmlNodeType.None || this.r.NodeType == XmlNodeType.Whitespace)
			{
				this.r.Read();
				return;
			}
			if (this.r.NodeType == XmlNodeType.EndElement)
			{
				return;
			}
			if (this.events.OnUnknownNode != null)
			{
				this.UnknownNode(this.Document.ReadNode(this.r), o, qnames);
				return;
			}
			if (this.r.NodeType == XmlNodeType.Attribute && this.events.OnUnknownAttribute == null)
			{
				return;
			}
			if (this.r.NodeType == XmlNodeType.Element && this.events.OnUnknownElement == null)
			{
				this.r.Skip();
				return;
			}
			this.UnknownNode(this.Document.ReadNode(this.r), o, qnames);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000BE9BC File Offset: 0x000BD9BC
		private void UnknownNode(XmlNode unknownNode, object o, string qnames)
		{
			if (unknownNode == null)
			{
				return;
			}
			if (unknownNode.NodeType != XmlNodeType.None && unknownNode.NodeType != XmlNodeType.Whitespace && this.events.OnUnknownNode != null)
			{
				int num;
				int num2;
				this.GetCurrentPosition(out num, out num2);
				XmlNodeEventArgs xmlNodeEventArgs = new XmlNodeEventArgs(unknownNode, num, num2, o);
				this.events.OnUnknownNode(this.events.sender, xmlNodeEventArgs);
			}
			if (unknownNode.NodeType == XmlNodeType.Attribute)
			{
				this.UnknownAttribute(o, (XmlAttribute)unknownNode, qnames);
				return;
			}
			if (unknownNode.NodeType == XmlNodeType.Element)
			{
				this.UnknownElement(o, (XmlElement)unknownNode, qnames);
			}
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000BEA4C File Offset: 0x000BDA4C
		private void GetCurrentPosition(out int lineNumber, out int linePosition)
		{
			if (this.Reader is IXmlLineInfo)
			{
				IXmlLineInfo xmlLineInfo = (IXmlLineInfo)this.Reader;
				lineNumber = xmlLineInfo.LineNumber;
				linePosition = xmlLineInfo.LinePosition;
				return;
			}
			lineNumber = (linePosition = -1);
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000BEA8C File Offset: 0x000BDA8C
		protected void UnreferencedObject(string id, object o)
		{
			if (this.events.OnUnreferencedObject != null)
			{
				UnreferencedObjectEventArgs unreferencedObjectEventArgs = new UnreferencedObjectEventArgs(o, id);
				this.events.OnUnreferencedObject(this.events.sender, unreferencedObjectEventArgs);
			}
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000BEACC File Offset: 0x000BDACC
		private string CurrentTag()
		{
			XmlNodeType nodeType = this.r.NodeType;
			switch (nodeType)
			{
			case XmlNodeType.Element:
				return string.Concat(new string[]
				{
					"<",
					this.r.LocalName,
					" xmlns='",
					this.r.NamespaceURI,
					"'>"
				});
			case XmlNodeType.Attribute:
			case XmlNodeType.EntityReference:
			case XmlNodeType.Entity:
				break;
			case XmlNodeType.Text:
				return this.r.Value;
			case XmlNodeType.CDATA:
				return "CDATA";
			case XmlNodeType.ProcessingInstruction:
				return "<?";
			case XmlNodeType.Comment:
				return "<--";
			default:
				if (nodeType == XmlNodeType.EndElement)
				{
					return ">";
				}
				break;
			}
			return "(unknown)";
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x000BEB80 File Offset: 0x000BDB80
		protected Exception CreateUnknownTypeException(XmlQualifiedName type)
		{
			return new InvalidOperationException(Res.GetString("XmlUnknownType", new object[]
			{
				type.Name,
				type.Namespace,
				this.CurrentTag()
			}));
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x000BEBC0 File Offset: 0x000BDBC0
		protected Exception CreateReadOnlyCollectionException(string name)
		{
			return new InvalidOperationException(Res.GetString("XmlReadOnlyCollection", new object[] { name }));
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000BEBE8 File Offset: 0x000BDBE8
		protected Exception CreateAbstractTypeException(string name, string ns)
		{
			return new InvalidOperationException(Res.GetString("XmlAbstractType", new object[]
			{
				name,
				ns,
				this.CurrentTag()
			}));
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000BEC20 File Offset: 0x000BDC20
		protected Exception CreateInaccessibleConstructorException(string typeName)
		{
			return new InvalidOperationException(Res.GetString("XmlConstructorInaccessible", new object[] { typeName }));
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000BEC48 File Offset: 0x000BDC48
		protected Exception CreateCtorHasSecurityException(string typeName)
		{
			return new InvalidOperationException(Res.GetString("XmlConstructorHasSecurityAttributes", new object[] { typeName }));
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000BEC70 File Offset: 0x000BDC70
		protected Exception CreateUnknownNodeException()
		{
			return new InvalidOperationException(Res.GetString("XmlUnknownNode", new object[] { this.CurrentTag() }));
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000BECA0 File Offset: 0x000BDCA0
		protected Exception CreateUnknownConstantException(string value, Type enumType)
		{
			return new InvalidOperationException(Res.GetString("XmlUnknownConstant", new object[] { value, enumType.Name }));
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000BECD1 File Offset: 0x000BDCD1
		protected Exception CreateInvalidCastException(Type type, object value)
		{
			return this.CreateInvalidCastException(type, value, null);
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000BECDC File Offset: 0x000BDCDC
		protected Exception CreateInvalidCastException(Type type, object value, string id)
		{
			if (value == null)
			{
				return new InvalidCastException(Res.GetString("XmlInvalidNullCast", new object[] { type.FullName }));
			}
			if (id == null)
			{
				return new InvalidCastException(Res.GetString("XmlInvalidCast", new object[]
				{
					value.GetType().FullName,
					type.FullName
				}));
			}
			return new InvalidCastException(Res.GetString("XmlInvalidCastWithId", new object[]
			{
				value.GetType().FullName,
				type.FullName,
				id
			}));
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000BED74 File Offset: 0x000BDD74
		protected Exception CreateBadDerivationException(string xsdDerived, string nsDerived, string xsdBase, string nsBase, string clrDerived, string clrBase)
		{
			return new InvalidOperationException(Res.GetString("XmlSerializableBadDerivation", new object[] { xsdDerived, nsDerived, xsdBase, nsBase, clrDerived, clrBase }));
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000BEDB4 File Offset: 0x000BDDB4
		protected Exception CreateMissingIXmlSerializableType(string name, string ns, string clrType)
		{
			return new InvalidOperationException(Res.GetString("XmlSerializableMissingClrType", new object[]
			{
				name,
				ns,
				typeof(XmlIncludeAttribute).Name,
				clrType
			}));
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000BEDF8 File Offset: 0x000BDDF8
		protected Array EnsureArrayIndex(Array a, int index, Type elementType)
		{
			if (a == null)
			{
				return Array.CreateInstance(elementType, 32);
			}
			if (index < a.Length)
			{
				return a;
			}
			Array array = Array.CreateInstance(elementType, a.Length * 2);
			Array.Copy(a, array, index);
			return array;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000BEE34 File Offset: 0x000BDE34
		protected Array ShrinkArray(Array a, int length, Type elementType, bool isNullable)
		{
			if (a == null)
			{
				if (isNullable)
				{
					return null;
				}
				return Array.CreateInstance(elementType, 0);
			}
			else
			{
				if (a.Length == length)
				{
					return a;
				}
				Array array = Array.CreateInstance(elementType, length);
				Array.Copy(a, array, length);
				return array;
			}
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000BEE6E File Offset: 0x000BDE6E
		protected string ReadString(string value)
		{
			return this.ReadString(value, false);
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000BEE78 File Offset: 0x000BDE78
		protected string ReadString(string value, bool trim)
		{
			string text = this.r.ReadString();
			if (text != null && trim)
			{
				text = text.Trim();
			}
			if (value == null || value.Length == 0)
			{
				return text;
			}
			return value + text;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000BEEB2 File Offset: 0x000BDEB2
		protected IXmlSerializable ReadSerializable(IXmlSerializable serializable)
		{
			return this.ReadSerializable(serializable, false);
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000BEEBC File Offset: 0x000BDEBC
		protected IXmlSerializable ReadSerializable(IXmlSerializable serializable, bool wrappedAny)
		{
			string text = null;
			string text2 = null;
			if (wrappedAny)
			{
				text = this.r.LocalName;
				text2 = this.r.NamespaceURI;
				this.r.Read();
				this.r.MoveToContent();
			}
			serializable.ReadXml(this.r);
			if (wrappedAny)
			{
				while (this.r.NodeType == XmlNodeType.Whitespace)
				{
					this.r.Skip();
				}
				if (this.r.NodeType == XmlNodeType.None)
				{
					this.r.Skip();
				}
				if (this.r.NodeType == XmlNodeType.EndElement && this.r.LocalName == text && this.r.NamespaceURI == text2)
				{
					this.Reader.Read();
				}
			}
			return serializable;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000BEF88 File Offset: 0x000BDF88
		protected bool ReadReference(out string fixupReference)
		{
			string text = (this.soap12 ? this.r.GetAttribute("ref", "http://www.w3.org/2003/05/soap-encoding") : this.r.GetAttribute("href"));
			if (text == null)
			{
				fixupReference = null;
				return false;
			}
			if (!this.soap12)
			{
				if (!text.StartsWith("#", StringComparison.Ordinal))
				{
					throw new InvalidOperationException(Res.GetString("XmlMissingHref", new object[] { text }));
				}
				fixupReference = text.Substring(1);
			}
			else
			{
				fixupReference = text;
			}
			if (this.r.IsEmptyElement)
			{
				this.r.Skip();
			}
			else
			{
				this.r.ReadStartElement();
				this.ReadEndElement();
			}
			return true;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000BF03C File Offset: 0x000BE03C
		protected void AddTarget(string id, object o)
		{
			if (id == null)
			{
				if (this.targetsWithoutIds == null)
				{
					this.targetsWithoutIds = new ArrayList();
				}
				if (o != null)
				{
					this.targetsWithoutIds.Add(o);
					return;
				}
			}
			else
			{
				if (this.targets == null)
				{
					this.targets = new Hashtable();
				}
				if (!this.targets.Contains(id))
				{
					this.targets.Add(id, o);
				}
			}
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000BF09E File Offset: 0x000BE09E
		protected void AddFixup(XmlSerializationReader.Fixup fixup)
		{
			if (this.fixups == null)
			{
				this.fixups = new ArrayList();
			}
			this.fixups.Add(fixup);
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000BF0C0 File Offset: 0x000BE0C0
		protected void AddFixup(XmlSerializationReader.CollectionFixup fixup)
		{
			if (this.collectionFixups == null)
			{
				this.collectionFixups = new ArrayList();
			}
			this.collectionFixups.Add(fixup);
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x000BF0E4 File Offset: 0x000BE0E4
		protected object GetTarget(string id)
		{
			object obj = ((this.targets != null) ? this.targets[id] : null);
			if (obj == null)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidHref", new object[] { id }));
			}
			this.Referenced(obj);
			return obj;
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x000BF130 File Offset: 0x000BE130
		protected void Referenced(object o)
		{
			if (o == null)
			{
				return;
			}
			if (this.referencedTargets == null)
			{
				this.referencedTargets = new Hashtable();
			}
			this.referencedTargets[o] = o;
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x000BF158 File Offset: 0x000BE158
		private void HandleUnreferencedObjects()
		{
			if (this.targets != null)
			{
				foreach (object obj in this.targets)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (this.referencedTargets == null || !this.referencedTargets.Contains(dictionaryEntry.Value))
					{
						this.UnreferencedObject((string)dictionaryEntry.Key, dictionaryEntry.Value);
					}
				}
			}
			if (this.targetsWithoutIds != null)
			{
				foreach (object obj2 in this.targetsWithoutIds)
				{
					if (this.referencedTargets == null || !this.referencedTargets.Contains(obj2))
					{
						this.UnreferencedObject(null, obj2);
					}
				}
			}
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000BF250 File Offset: 0x000BE250
		private void DoFixups()
		{
			if (this.fixups == null)
			{
				return;
			}
			for (int i = 0; i < this.fixups.Count; i++)
			{
				XmlSerializationReader.Fixup fixup = (XmlSerializationReader.Fixup)this.fixups[i];
				fixup.Callback(fixup);
			}
			if (this.collectionFixups == null)
			{
				return;
			}
			for (int j = 0; j < this.collectionFixups.Count; j++)
			{
				XmlSerializationReader.CollectionFixup collectionFixup = (XmlSerializationReader.CollectionFixup)this.collectionFixups[j];
				collectionFixup.Callback(collectionFixup.Collection, collectionFixup.CollectionItems);
			}
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000BF2E4 File Offset: 0x000BE2E4
		protected void FixupArrayRefs(object fixup)
		{
			XmlSerializationReader.Fixup fixup2 = (XmlSerializationReader.Fixup)fixup;
			Array array = (Array)fixup2.Source;
			for (int i = 0; i < array.Length; i++)
			{
				string text = fixup2.Ids[i];
				if (text != null)
				{
					object target = this.GetTarget(text);
					try
					{
						array.SetValue(target, i);
					}
					catch (InvalidCastException)
					{
						throw new InvalidOperationException(Res.GetString("XmlInvalidArrayRef", new object[]
						{
							text,
							target.GetType().FullName,
							i.ToString(CultureInfo.InvariantCulture)
						}));
					}
				}
			}
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000BF388 File Offset: 0x000BE388
		private object ReadArray(string typeName, string typeNs)
		{
			Type type = null;
			XmlSerializationReader.SoapArrayInfo soapArrayInfo;
			if (this.soap12)
			{
				string attribute = this.r.GetAttribute(this.itemTypeID, this.soap12NsID);
				string attribute2 = this.r.GetAttribute(this.arraySizeID, this.soap12NsID);
				Type type2 = (Type)this.types[new XmlQualifiedName(typeName, typeNs)];
				if (attribute == null && attribute2 == null && (type2 == null || !type2.IsArray))
				{
					return null;
				}
				soapArrayInfo = this.ParseSoap12ArrayType(attribute, attribute2);
				if (type2 != null)
				{
					type = TypeScope.GetArrayElementType(type2, null);
				}
			}
			else
			{
				string attribute3 = this.r.GetAttribute(this.arrayTypeID, this.soapNsID);
				if (attribute3 == null)
				{
					return null;
				}
				soapArrayInfo = this.ParseArrayType(attribute3);
			}
			if (soapArrayInfo.dimensions != 1)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidArrayDimentions", new object[] { this.CurrentTag() }));
			}
			Type type3 = null;
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(this.urTypeID, this.schemaNsID);
			XmlQualifiedName xmlQualifiedName2;
			if (soapArrayInfo.qname.Length > 0)
			{
				xmlQualifiedName2 = this.ToXmlQualifiedName(soapArrayInfo.qname, false);
				type3 = (Type)this.types[xmlQualifiedName2];
			}
			else
			{
				xmlQualifiedName2 = xmlQualifiedName;
			}
			if (this.soap12 && type3 == typeof(object))
			{
				type3 = null;
			}
			bool flag;
			if (type3 == null)
			{
				if (!this.soap12)
				{
					type3 = this.GetPrimitiveType(xmlQualifiedName2, true);
					flag = true;
				}
				else
				{
					if (xmlQualifiedName2 != xmlQualifiedName)
					{
						type3 = this.GetPrimitiveType(xmlQualifiedName2, false);
					}
					if (type3 != null)
					{
						flag = true;
					}
					else if (type == null)
					{
						type3 = typeof(object);
						flag = false;
					}
					else
					{
						type3 = type;
						XmlQualifiedName xmlQualifiedName3 = (XmlQualifiedName)this.typesReverse[type3];
						if (xmlQualifiedName3 == null)
						{
							xmlQualifiedName3 = XmlSerializationWriter.GetPrimitiveTypeNameInternal(type3);
							flag = true;
						}
						else
						{
							flag = type3.IsPrimitive;
						}
						if (xmlQualifiedName3 != null)
						{
							xmlQualifiedName2 = xmlQualifiedName3;
						}
					}
				}
			}
			else
			{
				flag = type3.IsPrimitive;
			}
			if (!this.soap12 && soapArrayInfo.jaggedDimensions > 0)
			{
				for (int i = 0; i < soapArrayInfo.jaggedDimensions; i++)
				{
					type3 = type3.MakeArrayType();
				}
			}
			if (this.r.IsEmptyElement)
			{
				this.r.Skip();
				return Array.CreateInstance(type3, 0);
			}
			this.r.ReadStartElement();
			this.r.MoveToContent();
			int num = 0;
			Array array = null;
			if (type3.IsValueType)
			{
				if (!flag && !type3.IsEnum)
				{
					throw new NotSupportedException(Res.GetString("XmlRpcArrayOfValueTypes", new object[] { type3.FullName }));
				}
				int num2 = 0;
				int readerCount = this.ReaderCount;
				while (this.r.NodeType != XmlNodeType.EndElement)
				{
					array = this.EnsureArrayIndex(array, num, type3);
					array.SetValue(this.ReadReferencedElement(xmlQualifiedName2.Name, xmlQualifiedName2.Namespace), num);
					num++;
					this.r.MoveToContent();
					this.CheckReaderCount(ref num2, ref readerCount);
				}
				array = this.ShrinkArray(array, num, type3, false);
			}
			else
			{
				string[] array2 = null;
				int num3 = 0;
				int num4 = 0;
				int readerCount2 = this.ReaderCount;
				while (this.r.NodeType != XmlNodeType.EndElement)
				{
					array = this.EnsureArrayIndex(array, num, type3);
					array2 = (string[])this.EnsureArrayIndex(array2, num3, typeof(string));
					string text;
					string text2;
					if (this.r.NamespaceURI.Length != 0)
					{
						text = this.r.LocalName;
						if (this.r.NamespaceURI == this.soapNsID)
						{
							text2 = "http://www.w3.org/2001/XMLSchema";
						}
						else
						{
							text2 = this.r.NamespaceURI;
						}
					}
					else
					{
						text = xmlQualifiedName2.Name;
						text2 = xmlQualifiedName2.Namespace;
					}
					array.SetValue(this.ReadReferencingElement(text, text2, out array2[num3]), num);
					num++;
					num3++;
					this.r.MoveToContent();
					this.CheckReaderCount(ref num4, ref readerCount2);
				}
				if (this.soap12 && type3 == typeof(object))
				{
					Type type4 = null;
					for (int j = 0; j < num; j++)
					{
						object value = array.GetValue(j);
						if (value != null)
						{
							Type type5 = value.GetType();
							if (type5.IsValueType)
							{
								type4 = null;
								break;
							}
							if (type4 == null || type5.IsAssignableFrom(type4))
							{
								type4 = type5;
							}
							else if (!type4.IsAssignableFrom(type5))
							{
								type4 = null;
								break;
							}
						}
					}
					if (type4 != null)
					{
						type3 = type4;
					}
				}
				array2 = (string[])this.ShrinkArray(array2, num3, typeof(string), false);
				array = this.ShrinkArray(array, num, type3, false);
				XmlSerializationReader.Fixup fixup = new XmlSerializationReader.Fixup(array, new XmlSerializationFixupCallback(this.FixupArrayRefs), array2);
				this.AddFixup(fixup);
			}
			this.ReadEndElement();
			return array;
		}

		// Token: 0x060026C8 RID: 9928
		protected abstract void InitCallbacks();

		// Token: 0x060026C9 RID: 9929 RVA: 0x000BF858 File Offset: 0x000BE858
		protected void ReadReferencedElements()
		{
			this.r.MoveToContent();
			int num = 0;
			int readerCount = this.ReaderCount;
			while (this.r.NodeType != XmlNodeType.EndElement && this.r.NodeType != XmlNodeType.None)
			{
				string text;
				this.ReadReferencingElement(null, null, true, out text);
				this.r.MoveToContent();
				this.CheckReaderCount(ref num, ref readerCount);
			}
			this.DoFixups();
			this.HandleUnreferencedObjects();
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000BF8C6 File Offset: 0x000BE8C6
		protected object ReadReferencedElement()
		{
			return this.ReadReferencedElement(null, null);
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000BF8D0 File Offset: 0x000BE8D0
		protected object ReadReferencedElement(string name, string ns)
		{
			string text;
			return this.ReadReferencingElement(name, ns, out text);
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000BF8E7 File Offset: 0x000BE8E7
		protected object ReadReferencingElement(out string fixupReference)
		{
			return this.ReadReferencingElement(null, null, out fixupReference);
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x000BF8F2 File Offset: 0x000BE8F2
		protected object ReadReferencingElement(string name, string ns, out string fixupReference)
		{
			return this.ReadReferencingElement(name, ns, false, out fixupReference);
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x000BF900 File Offset: 0x000BE900
		protected object ReadReferencingElement(string name, string ns, bool elementCanBeType, out string fixupReference)
		{
			if (this.callbacks == null)
			{
				this.callbacks = new Hashtable();
				this.types = new Hashtable();
				XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(this.urTypeID, this.r.NameTable.Add("http://www.w3.org/2001/XMLSchema"));
				this.types.Add(xmlQualifiedName, typeof(object));
				this.typesReverse = new Hashtable();
				this.typesReverse.Add(typeof(object), xmlQualifiedName);
				this.InitCallbacks();
			}
			this.r.MoveToContent();
			if (this.ReadReference(out fixupReference))
			{
				return null;
			}
			if (this.ReadNull())
			{
				return null;
			}
			string text = (this.soap12 ? this.r.GetAttribute("id", "http://www.w3.org/2003/05/soap-encoding") : this.r.GetAttribute("id", null));
			object obj;
			if ((obj = this.ReadArray(name, ns)) == null)
			{
				XmlQualifiedName xmlQualifiedName2 = this.GetXsiType();
				if (xmlQualifiedName2 == null)
				{
					if (name == null)
					{
						xmlQualifiedName2 = new XmlQualifiedName(this.r.NameTable.Add(this.r.LocalName), this.r.NameTable.Add(this.r.NamespaceURI));
					}
					else
					{
						xmlQualifiedName2 = new XmlQualifiedName(this.r.NameTable.Add(name), this.r.NameTable.Add(ns));
					}
				}
				XmlSerializationReadCallback xmlSerializationReadCallback = (XmlSerializationReadCallback)this.callbacks[xmlQualifiedName2];
				if (xmlSerializationReadCallback != null)
				{
					obj = xmlSerializationReadCallback();
				}
				else
				{
					obj = this.ReadTypedPrimitive(xmlQualifiedName2, elementCanBeType);
				}
			}
			this.AddTarget(text, obj);
			return obj;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x000BFA9C File Offset: 0x000BEA9C
		protected void AddReadCallback(string name, string ns, Type type, XmlSerializationReadCallback read)
		{
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(this.r.NameTable.Add(name), this.r.NameTable.Add(ns));
			this.callbacks[xmlQualifiedName] = read;
			this.types[xmlQualifiedName] = type;
			this.typesReverse[type] = xmlQualifiedName;
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x000BFAFC File Offset: 0x000BEAFC
		protected void ReadEndElement()
		{
			while (this.r.NodeType == XmlNodeType.Whitespace)
			{
				this.r.Skip();
			}
			if (this.r.NodeType == XmlNodeType.None)
			{
				this.r.Skip();
				return;
			}
			this.r.ReadEndElement();
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x000BFB4C File Offset: 0x000BEB4C
		private object ReadXmlNodes(bool elementCanBeType)
		{
			ArrayList arrayList = new ArrayList();
			string localName = this.Reader.LocalName;
			string namespaceURI = this.Reader.NamespaceURI;
			string name = this.Reader.Name;
			string text = null;
			string text2 = null;
			int num = 0;
			int num2 = -1;
			int num3 = -1;
			XmlNode xmlNode;
			if (this.Reader.NodeType == XmlNodeType.Attribute)
			{
				XmlAttribute xmlAttribute = this.Document.CreateAttribute(name, namespaceURI);
				xmlAttribute.Value = this.Reader.Value;
				xmlNode = xmlAttribute;
			}
			else
			{
				xmlNode = this.Document.CreateElement(name, namespaceURI);
			}
			this.GetCurrentPosition(out num2, out num3);
			XmlElement xmlElement = xmlNode as XmlElement;
			while (this.Reader.MoveToNextAttribute())
			{
				if (this.IsXmlnsAttribute(this.Reader.Name) || (this.Reader.Name == "id" && (!this.soap12 || this.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-encoding")))
				{
					num++;
				}
				if (this.Reader.LocalName == this.typeID && (this.Reader.NamespaceURI == this.instanceNsID || this.Reader.NamespaceURI == this.instanceNs2000ID || this.Reader.NamespaceURI == this.instanceNs1999ID))
				{
					string value = this.Reader.Value;
					int num4 = value.LastIndexOf(':');
					text = ((num4 >= 0) ? value.Substring(num4 + 1) : value);
					text2 = this.Reader.LookupNamespace((num4 >= 0) ? value.Substring(0, num4) : "");
				}
				XmlAttribute xmlAttribute2 = (XmlAttribute)this.Document.ReadNode(this.r);
				arrayList.Add(xmlAttribute2);
				if (xmlElement != null)
				{
					xmlElement.SetAttributeNode(xmlAttribute2);
				}
			}
			if (elementCanBeType && text == null)
			{
				text = localName;
				text2 = namespaceURI;
				XmlAttribute xmlAttribute3 = this.Document.CreateAttribute(this.typeID, this.instanceNsID);
				xmlAttribute3.Value = name;
				arrayList.Add(xmlAttribute3);
			}
			if (text == "anyType" && (text2 == this.schemaNsID || text2 == this.schemaNs1999ID || text2 == this.schemaNs2000ID))
			{
				num++;
			}
			this.Reader.MoveToElement();
			if (this.Reader.IsEmptyElement)
			{
				this.Reader.Skip();
			}
			else
			{
				this.Reader.ReadStartElement();
				this.Reader.MoveToContent();
				int num5 = 0;
				int readerCount = this.ReaderCount;
				while (this.Reader.NodeType != XmlNodeType.EndElement)
				{
					XmlNode xmlNode2 = this.Document.ReadNode(this.r);
					arrayList.Add(xmlNode2);
					if (xmlElement != null)
					{
						xmlElement.AppendChild(xmlNode2);
					}
					this.Reader.MoveToContent();
					this.CheckReaderCount(ref num5, ref readerCount);
				}
				this.ReadEndElement();
			}
			if (arrayList.Count <= num)
			{
				return new object();
			}
			XmlNode[] array = (XmlNode[])arrayList.ToArray(typeof(XmlNode));
			this.UnknownNode(xmlNode, null, null);
			return array;
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x000BFE5A File Offset: 0x000BEE5A
		protected void CheckReaderCount(ref int whileIterations, ref int readerCount)
		{
			if (XmlSerializationReader.checkDeserializeAdvances)
			{
				whileIterations++;
				if ((whileIterations & 128) == 128)
				{
					if (readerCount == this.ReaderCount)
					{
						throw new InvalidOperationException(Res.GetString("XmlInternalErrorReaderAdvance"));
					}
					readerCount = this.ReaderCount;
				}
			}
		}

		// Token: 0x040015DF RID: 5599
		private XmlReader r;

		// Token: 0x040015E0 RID: 5600
		private XmlCountingReader countingReader;

		// Token: 0x040015E1 RID: 5601
		private XmlDocument d;

		// Token: 0x040015E2 RID: 5602
		private Hashtable callbacks;

		// Token: 0x040015E3 RID: 5603
		private Hashtable types;

		// Token: 0x040015E4 RID: 5604
		private Hashtable typesReverse;

		// Token: 0x040015E5 RID: 5605
		private XmlDeserializationEvents events;

		// Token: 0x040015E6 RID: 5606
		private Hashtable targets;

		// Token: 0x040015E7 RID: 5607
		private Hashtable referencedTargets;

		// Token: 0x040015E8 RID: 5608
		private ArrayList targetsWithoutIds;

		// Token: 0x040015E9 RID: 5609
		private ArrayList fixups;

		// Token: 0x040015EA RID: 5610
		private ArrayList collectionFixups;

		// Token: 0x040015EB RID: 5611
		private bool soap12;

		// Token: 0x040015EC RID: 5612
		private bool isReturnValue;

		// Token: 0x040015ED RID: 5613
		private bool decodeName = true;

		// Token: 0x040015EE RID: 5614
		private string schemaNsID;

		// Token: 0x040015EF RID: 5615
		private string schemaNs1999ID;

		// Token: 0x040015F0 RID: 5616
		private string schemaNs2000ID;

		// Token: 0x040015F1 RID: 5617
		private string schemaNonXsdTypesNsID;

		// Token: 0x040015F2 RID: 5618
		private string instanceNsID;

		// Token: 0x040015F3 RID: 5619
		private string instanceNs2000ID;

		// Token: 0x040015F4 RID: 5620
		private string instanceNs1999ID;

		// Token: 0x040015F5 RID: 5621
		private string soapNsID;

		// Token: 0x040015F6 RID: 5622
		private string soap12NsID;

		// Token: 0x040015F7 RID: 5623
		private string schemaID;

		// Token: 0x040015F8 RID: 5624
		private string wsdlNsID;

		// Token: 0x040015F9 RID: 5625
		private string wsdlArrayTypeID;

		// Token: 0x040015FA RID: 5626
		private string nullID;

		// Token: 0x040015FB RID: 5627
		private string nilID;

		// Token: 0x040015FC RID: 5628
		private string typeID;

		// Token: 0x040015FD RID: 5629
		private string arrayTypeID;

		// Token: 0x040015FE RID: 5630
		private string itemTypeID;

		// Token: 0x040015FF RID: 5631
		private string arraySizeID;

		// Token: 0x04001600 RID: 5632
		private string arrayID;

		// Token: 0x04001601 RID: 5633
		private string urTypeID;

		// Token: 0x04001602 RID: 5634
		private string stringID;

		// Token: 0x04001603 RID: 5635
		private string intID;

		// Token: 0x04001604 RID: 5636
		private string booleanID;

		// Token: 0x04001605 RID: 5637
		private string shortID;

		// Token: 0x04001606 RID: 5638
		private string longID;

		// Token: 0x04001607 RID: 5639
		private string floatID;

		// Token: 0x04001608 RID: 5640
		private string doubleID;

		// Token: 0x04001609 RID: 5641
		private string decimalID;

		// Token: 0x0400160A RID: 5642
		private string dateTimeID;

		// Token: 0x0400160B RID: 5643
		private string qnameID;

		// Token: 0x0400160C RID: 5644
		private string dateID;

		// Token: 0x0400160D RID: 5645
		private string timeID;

		// Token: 0x0400160E RID: 5646
		private string hexBinaryID;

		// Token: 0x0400160F RID: 5647
		private string base64BinaryID;

		// Token: 0x04001610 RID: 5648
		private string base64ID;

		// Token: 0x04001611 RID: 5649
		private string unsignedByteID;

		// Token: 0x04001612 RID: 5650
		private string byteID;

		// Token: 0x04001613 RID: 5651
		private string unsignedShortID;

		// Token: 0x04001614 RID: 5652
		private string unsignedIntID;

		// Token: 0x04001615 RID: 5653
		private string unsignedLongID;

		// Token: 0x04001616 RID: 5654
		private string oldDecimalID;

		// Token: 0x04001617 RID: 5655
		private string oldTimeInstantID;

		// Token: 0x04001618 RID: 5656
		private string anyURIID;

		// Token: 0x04001619 RID: 5657
		private string durationID;

		// Token: 0x0400161A RID: 5658
		private string ENTITYID;

		// Token: 0x0400161B RID: 5659
		private string ENTITIESID;

		// Token: 0x0400161C RID: 5660
		private string gDayID;

		// Token: 0x0400161D RID: 5661
		private string gMonthID;

		// Token: 0x0400161E RID: 5662
		private string gMonthDayID;

		// Token: 0x0400161F RID: 5663
		private string gYearID;

		// Token: 0x04001620 RID: 5664
		private string gYearMonthID;

		// Token: 0x04001621 RID: 5665
		private string IDID;

		// Token: 0x04001622 RID: 5666
		private string IDREFID;

		// Token: 0x04001623 RID: 5667
		private string IDREFSID;

		// Token: 0x04001624 RID: 5668
		private string integerID;

		// Token: 0x04001625 RID: 5669
		private string languageID;

		// Token: 0x04001626 RID: 5670
		private string NameID;

		// Token: 0x04001627 RID: 5671
		private string NCNameID;

		// Token: 0x04001628 RID: 5672
		private string NMTOKENID;

		// Token: 0x04001629 RID: 5673
		private string NMTOKENSID;

		// Token: 0x0400162A RID: 5674
		private string negativeIntegerID;

		// Token: 0x0400162B RID: 5675
		private string nonPositiveIntegerID;

		// Token: 0x0400162C RID: 5676
		private string nonNegativeIntegerID;

		// Token: 0x0400162D RID: 5677
		private string normalizedStringID;

		// Token: 0x0400162E RID: 5678
		private string NOTATIONID;

		// Token: 0x0400162F RID: 5679
		private string positiveIntegerID;

		// Token: 0x04001630 RID: 5680
		private string tokenID;

		// Token: 0x04001631 RID: 5681
		private string charID;

		// Token: 0x04001632 RID: 5682
		private string guidID;

		// Token: 0x04001633 RID: 5683
		private static bool checkDeserializeAdvances;

		// Token: 0x02000325 RID: 805
		private struct SoapArrayInfo
		{
			// Token: 0x04001634 RID: 5684
			public string qname;

			// Token: 0x04001635 RID: 5685
			public int dimensions;

			// Token: 0x04001636 RID: 5686
			public int length;

			// Token: 0x04001637 RID: 5687
			public int jaggedDimensions;
		}

		// Token: 0x02000326 RID: 806
		protected class Fixup
		{
			// Token: 0x060026D4 RID: 9940 RVA: 0x000BFEA9 File Offset: 0x000BEEA9
			public Fixup(object o, XmlSerializationFixupCallback callback, int count)
				: this(o, callback, new string[count])
			{
			}

			// Token: 0x060026D5 RID: 9941 RVA: 0x000BFEB9 File Offset: 0x000BEEB9
			public Fixup(object o, XmlSerializationFixupCallback callback, string[] ids)
			{
				this.callback = callback;
				this.Source = o;
				this.ids = ids;
			}

			// Token: 0x17000966 RID: 2406
			// (get) Token: 0x060026D6 RID: 9942 RVA: 0x000BFED6 File Offset: 0x000BEED6
			public XmlSerializationFixupCallback Callback
			{
				get
				{
					return this.callback;
				}
			}

			// Token: 0x17000967 RID: 2407
			// (get) Token: 0x060026D7 RID: 9943 RVA: 0x000BFEDE File Offset: 0x000BEEDE
			// (set) Token: 0x060026D8 RID: 9944 RVA: 0x000BFEE6 File Offset: 0x000BEEE6
			public object Source
			{
				get
				{
					return this.source;
				}
				set
				{
					this.source = value;
				}
			}

			// Token: 0x17000968 RID: 2408
			// (get) Token: 0x060026D9 RID: 9945 RVA: 0x000BFEEF File Offset: 0x000BEEEF
			public string[] Ids
			{
				get
				{
					return this.ids;
				}
			}

			// Token: 0x04001638 RID: 5688
			private XmlSerializationFixupCallback callback;

			// Token: 0x04001639 RID: 5689
			private object source;

			// Token: 0x0400163A RID: 5690
			private string[] ids;
		}

		// Token: 0x02000327 RID: 807
		protected class CollectionFixup
		{
			// Token: 0x060026DA RID: 9946 RVA: 0x000BFEF7 File Offset: 0x000BEEF7
			public CollectionFixup(object collection, XmlSerializationCollectionFixupCallback callback, object collectionItems)
			{
				this.callback = callback;
				this.collection = collection;
				this.collectionItems = collectionItems;
			}

			// Token: 0x17000969 RID: 2409
			// (get) Token: 0x060026DB RID: 9947 RVA: 0x000BFF14 File Offset: 0x000BEF14
			public XmlSerializationCollectionFixupCallback Callback
			{
				get
				{
					return this.callback;
				}
			}

			// Token: 0x1700096A RID: 2410
			// (get) Token: 0x060026DC RID: 9948 RVA: 0x000BFF1C File Offset: 0x000BEF1C
			public object Collection
			{
				get
				{
					return this.collection;
				}
			}

			// Token: 0x1700096B RID: 2411
			// (get) Token: 0x060026DD RID: 9949 RVA: 0x000BFF24 File Offset: 0x000BEF24
			public object CollectionItems
			{
				get
				{
					return this.collectionItems;
				}
			}

			// Token: 0x0400163B RID: 5691
			private XmlSerializationCollectionFixupCallback callback;

			// Token: 0x0400163C RID: 5692
			private object collection;

			// Token: 0x0400163D RID: 5693
			private object collectionItems;
		}
	}
}
