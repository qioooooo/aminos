using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000026 RID: 38
	internal sealed class SoapWriter
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00007ECC File Offset: 0x00006ECC
		static SoapWriter()
		{
			SoapWriter.encodingTable.Add('&', "&#38;");
			SoapWriter.encodingTable.Add('"', "&#34;");
			SoapWriter.encodingTable.Add('\'', "&#39;");
			SoapWriter.encodingTable.Add('<', "&#60;");
			SoapWriter.encodingTable.Add('>', "&#62;");
			SoapWriter.encodingTable.Add('\0', "&#0;");
			SoapWriter.encodingTable.Add('\v', "&#xB;");
			SoapWriter.encodingTable.Add('\f', "&#xC;");
			for (int i = 1; i < 9; i++)
			{
				SoapWriter.encodingTable.Add(((IConvertible)i).ToChar(NumberFormatInfo.InvariantInfo), "&#x" + i.ToString("X", CultureInfo.InvariantCulture) + ";");
			}
			for (int j = 14; j < 32; j++)
			{
				SoapWriter.encodingTable.Add(((IConvertible)j).ToChar(NumberFormatInfo.InvariantInfo), "&#x" + j.ToString("X", CultureInfo.InvariantCulture) + ";");
			}
			for (int k = 127; k < 133; k++)
			{
				SoapWriter.encodingTable.Add(((IConvertible)k).ToChar(NumberFormatInfo.InvariantInfo), "&#x" + k.ToString("X", CultureInfo.InvariantCulture) + ";");
			}
			for (int l = 134; l < 160; l++)
			{
				SoapWriter.encodingTable.Add(((IConvertible)l).ToChar(NumberFormatInfo.InvariantInfo), "&#x" + l.ToString("X", CultureInfo.InvariantCulture) + ";");
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000080EC File Offset: 0x000070EC
		internal SoapWriter(Stream stream)
		{
			this.stream = stream;
			UTF8Encoding utf8Encoding = new UTF8Encoding(false, true);
			this.writer = new StreamWriter(stream, utf8Encoding, 1024);
			this.typeNameToDottedInfoTable = new Hashtable(20);
			this.dottedAssemToAssemIdTable = new Hashtable(20);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00008200 File Offset: 0x00007200
		[Conditional("_DEBUG")]
		private void EmitIndent(int count)
		{
			while (--count >= 0)
			{
				for (int i = 0; i < this.lineIndent; i++)
				{
					this.writer.Write(' ');
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00008236 File Offset: 0x00007236
		[Conditional("_DEBUG")]
		private void EmitLine(int indent, string value)
		{
			this.writer.Write(value);
			this.EmitLine();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000824A File Offset: 0x0000724A
		private void EmitLine()
		{
			this.writer.Write("\r\n");
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000825C File Offset: 0x0000725C
		private string Escape(string value)
		{
			this.stringBuffer.Length = 0;
			foreach (char c in value)
			{
				if (SoapWriter.encodingTable.ContainsKey(c))
				{
					this.stringBuffer.Append(SoapWriter.encodingTable[c]);
				}
				else
				{
					this.stringBuffer.Append(c);
				}
			}
			string text;
			if (this.stringBuffer.Length > 0)
			{
				text = this.stringBuffer.ToString();
			}
			else
			{
				text = value;
			}
			return text;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000082E4 File Offset: 0x000072E4
		private string NameEscape(string name)
		{
			string text = (string)this.nameCache.GetCachedValue(name);
			if (text == null)
			{
				text = XmlConvert.EncodeName(name);
				this.nameCache.SetCachedValue(text);
			}
			return text;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000831A File Offset: 0x0000731A
		internal void Reset()
		{
			this.writer = null;
			this.stringBuffer = null;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000832A File Offset: 0x0000732A
		internal void InternalWrite(string s)
		{
			this.writer.Write(s);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00008338 File Offset: 0x00007338
		[Conditional("_LOGGING")]
		internal void TraceSoap(string s)
		{
			if (this.traceBuffer == null)
			{
				this.traceBuffer = new StringBuilder();
			}
			this.traceBuffer.Append(s);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000835A File Offset: 0x0000735A
		[Conditional("_LOGGING")]
		internal void WriteTraceSoap()
		{
			this.traceBuffer.Length = 0;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00008368 File Offset: 0x00007368
		internal void Write(InternalElementTypeE use, string name, AttributeList attrList, string value, bool isNameEscape, bool isValueEscape)
		{
			string text = name;
			if (isNameEscape)
			{
				text = this.NameEscape(name);
			}
			if (use == InternalElementTypeE.ObjectEnd)
			{
				this.instanceIndent--;
			}
			this.InternalWrite("<");
			if (use == InternalElementTypeE.ObjectEnd)
			{
				this.InternalWrite("/");
			}
			this.InternalWrite(text);
			this.WriteAttributeList(attrList);
			switch (use)
			{
			case InternalElementTypeE.ObjectBegin:
				this.InternalWrite(">");
				this.instanceIndent++;
				break;
			case InternalElementTypeE.ObjectEnd:
				this.InternalWrite(">");
				break;
			case InternalElementTypeE.Member:
				if (value == null)
				{
					this.InternalWrite("/>");
				}
				else
				{
					this.InternalWrite(">");
					if (isValueEscape)
					{
						this.InternalWrite(this.Escape(value));
					}
					else
					{
						this.InternalWrite(value);
					}
					this.InternalWrite("</");
					this.InternalWrite(text);
					this.InternalWrite(">");
				}
				break;
			default:
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_UseCode"), new object[] { use.ToString() }));
			}
			this.EmitLine();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00008494 File Offset: 0x00007494
		private void WriteAttributeList(AttributeList attrList)
		{
			for (int i = 0; i < attrList.Count; i++)
			{
				string text;
				string text2;
				attrList.Get(i, out text, out text2);
				this.InternalWrite(" ");
				this.InternalWrite(text);
				this.InternalWrite("=");
				this.InternalWrite("\"");
				this.InternalWrite(text2);
				this.InternalWrite("\"");
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000084F7 File Offset: 0x000074F7
		internal void WriteBegin()
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000084F9 File Offset: 0x000074F9
		internal void WriteEnd()
		{
			this.writer.Flush();
			this.Reset();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000850C File Offset: 0x0000750C
		internal void WriteXsdVersion(XsdVersion xsdVersion)
		{
			this.xsdVersion = xsdVersion;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00008515 File Offset: 0x00007515
		internal void WriteObjectEnd(NameInfo memberNameInfo, NameInfo typeNameInfo)
		{
			this.attrList.Clear();
			this.Write(InternalElementTypeE.ObjectEnd, this.MemberElementName(memberNameInfo, typeNameInfo), this.attrList, null, true, false);
			this.assemblyInfoUsed.Clear();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00008548 File Offset: 0x00007548
		internal void WriteSerializationHeaderEnd()
		{
			this.attrList.Clear();
			this.Write(InternalElementTypeE.ObjectEnd, "SOAP-ENV:Body", this.attrList, null, false, false);
			this.Write(InternalElementTypeE.ObjectEnd, "SOAP-ENV:Envelope", this.attrList, null, false, false);
			this.writer.Flush();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00008595 File Offset: 0x00007595
		internal void WriteHeaderArrayEnd()
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00008597 File Offset: 0x00007597
		internal void WriteHeaderSectionEnd()
		{
			this.attrList.Clear();
			this.Write(InternalElementTypeE.ObjectEnd, "SOAP-ENV:Header", this.attrList, null, false, false);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000085BC File Offset: 0x000075BC
		internal void WriteSerializationHeader(int topId, int headerId, int minorVersion, int majorVersion)
		{
			this.topId = topId;
			this.headerId = headerId;
			switch (this.xsdVersion)
			{
			case XsdVersion.V1999:
				this.stream.Write(SoapWriter._soapStart1999, 0, SoapWriter._soapStart1999.Length);
				break;
			case XsdVersion.V2000:
				this.stream.Write(SoapWriter._soapStart1999, 0, SoapWriter._soapStart2000.Length);
				break;
			case XsdVersion.V2001:
				this.stream.Write(SoapWriter._soapStart, 0, SoapWriter._soapStart.Length);
				break;
			}
			this.writer.Write(">\r\n");
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00008650 File Offset: 0x00007650
		internal void WriteObject(NameInfo nameInfo, NameInfo typeNameInfo, int numMembers, string[] memberNames, Type[] memberTypes, WriteObjectInfo[] objectInfos)
		{
			int num = (int)nameInfo.NIobjectId;
			this.attrList.Clear();
			if (num == this.topId)
			{
				this.Write(InternalElementTypeE.ObjectBegin, "SOAP-ENV:Body", this.attrList, null, false, false);
			}
			if (num > 0)
			{
				this.attrList.Put("id", this.IdToString((int)nameInfo.NIobjectId));
			}
			if ((nameInfo.NItransmitTypeOnObject || nameInfo.NItransmitTypeOnMember) && (nameInfo.NIisNestedObject || nameInfo.NIisArrayItem))
			{
				this.attrList.Put("xsi:type", this.TypeNameTagResolver(typeNameInfo, true));
			}
			if (nameInfo.NIisMustUnderstand)
			{
				this.attrList.Put("SOAP-ENV:mustUnderstand", "1");
				this.isUsedEnc = true;
			}
			if (nameInfo.NIisHeader)
			{
				this.attrList.Put("xmlns:" + nameInfo.NIheaderPrefix, nameInfo.NInamespace);
				this.attrList.Put("SOAP-ENC:root", "1");
			}
			if (this.attrValueList.Count > 0)
			{
				for (int i = 0; i < this.attrValueList.Count; i++)
				{
					string text;
					string text2;
					this.attrValueList.Get(i, out text, out text2);
					this.attrList.Put(text, text2);
				}
				this.attrValueList.Clear();
			}
			string text3 = this.MemberElementName(nameInfo, typeNameInfo);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.ObjectBegin, text3, this.attrList, null, true, false);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000087B8 File Offset: 0x000077B8
		internal void WriteAttributeValue(NameInfo memberNameInfo, NameInfo typeNameInfo, object value)
		{
			string text;
			if (value is string)
			{
				text = (string)value;
			}
			else
			{
				text = Converter.SoapToString(value, typeNameInfo.NIprimitiveTypeEnum);
			}
			this.attrValueList.Put(this.MemberElementName(memberNameInfo, typeNameInfo), text);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000087FC File Offset: 0x000077FC
		internal void WriteObjectString(NameInfo nameInfo, string value)
		{
			this.attrList.Clear();
			if (nameInfo.NIobjectId == (long)this.topId)
			{
				this.Write(InternalElementTypeE.ObjectBegin, "SOAP-ENV:Body", this.attrList, null, false, false);
			}
			if (nameInfo.NIobjectId > 0L)
			{
				this.attrList.Put("id", this.IdToString((int)nameInfo.NIobjectId));
			}
			string text;
			if (nameInfo.NIobjectId > 0L)
			{
				text = "SOAP-ENC:string";
				this.isUsedEnc = true;
			}
			else
			{
				text = "xsd:string";
			}
			this.Write(InternalElementTypeE.Member, text, this.attrList, value, false, Converter.IsEscaped(nameInfo.NIprimitiveTypeEnum));
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000889C File Offset: 0x0000789C
		internal void WriteTopPrimitive(NameInfo nameInfo, object value)
		{
			this.attrList.Clear();
			this.Write(InternalElementTypeE.ObjectBegin, "SOAP-ENV:Body", this.attrList, null, false, false);
			if (nameInfo.NIobjectId > 0L)
			{
				this.attrList.Put("id", this.IdToString((int)nameInfo.NIobjectId));
			}
			string text;
			if (value is string)
			{
				text = (string)value;
			}
			else
			{
				text = Converter.SoapToString(value, nameInfo.NIprimitiveTypeEnum);
			}
			this.Write(InternalElementTypeE.Member, "xsd:" + Converter.ToXmlDataType(nameInfo.NIprimitiveTypeEnum), this.attrList, text, true, false);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00008938 File Offset: 0x00007938
		internal void WriteObjectByteArray(NameInfo memberNameInfo, NameInfo arrayNameInfo, WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, int length, int lowerBound, byte[] byteA)
		{
			string text = Convert.ToBase64String(byteA);
			this.attrList.Clear();
			if (memberNameInfo.NIobjectId == (long)this.topId)
			{
				this.Write(InternalElementTypeE.ObjectBegin, "SOAP-ENV:Body", this.attrList, null, false, false);
			}
			if (arrayNameInfo.NIobjectId > 1L)
			{
				this.attrList.Put("id", this.IdToString((int)arrayNameInfo.NIobjectId));
			}
			this.attrList.Put("xsi:type", "SOAP-ENC:base64");
			this.isUsedEnc = true;
			string text2 = this.MemberElementName(memberNameInfo, null);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, text2, this.attrList, text, true, false);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000089E0 File Offset: 0x000079E0
		internal void WriteMember(NameInfo memberNameInfo, NameInfo typeNameInfo, object value)
		{
			this.attrList.Clear();
			if (typeNameInfo.NItype != null && (memberNameInfo.NItransmitTypeOnMember || (memberNameInfo.NItransmitTypeOnObject && !memberNameInfo.NIisArrayItem)))
			{
				this.attrList.Put("xsi:type", this.TypeNameTagResolver(typeNameInfo, true));
			}
			string text = null;
			if (value != null)
			{
				if (typeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.QName)
				{
					SoapQName soapQName = (SoapQName)value;
					if (soapQName.Key == null || soapQName.Key.Length == 0)
					{
						this.attrList.Put("xmlns", "");
					}
					else
					{
						this.attrList.Put("xmlns:" + soapQName.Key, soapQName.Namespace);
					}
					text = soapQName.ToString();
				}
				else if (value is string)
				{
					text = (string)value;
				}
				else
				{
					text = Converter.SoapToString(value, typeNameInfo.NIprimitiveTypeEnum);
				}
			}
			NameInfo nameInfo = null;
			if (typeNameInfo.NInameSpaceEnum == InternalNameSpaceE.Interop)
			{
				nameInfo = typeNameInfo;
			}
			string text2 = this.MemberElementName(memberNameInfo, nameInfo);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, text2, this.attrList, text, true, Converter.IsEscaped(typeNameInfo.NIprimitiveTypeEnum));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00008AF4 File Offset: 0x00007AF4
		internal void WriteNullMember(NameInfo memberNameInfo, NameInfo typeNameInfo)
		{
			this.attrList.Clear();
			if (typeNameInfo.NItype != null && (memberNameInfo.NItransmitTypeOnMember || (memberNameInfo.NItransmitTypeOnObject && !memberNameInfo.NIisArrayItem)))
			{
				this.attrList.Put("xsi:type", this.TypeNameTagResolver(typeNameInfo, true));
			}
			this.attrList.Put("xsi:null", "1");
			string text = this.MemberElementName(memberNameInfo, null);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, text, this.attrList, null, true, false);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008B7C File Offset: 0x00007B7C
		internal void WriteMemberObjectRef(NameInfo memberNameInfo, NameInfo typeNameInfo, int idRef)
		{
			this.attrList.Clear();
			this.attrList.Put("href", this.RefToString(idRef));
			NameInfo nameInfo = null;
			if (typeNameInfo.NInameSpaceEnum == InternalNameSpaceE.Interop)
			{
				nameInfo = typeNameInfo;
			}
			string text = this.MemberElementName(memberNameInfo, nameInfo);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, text, this.attrList, null, true, false);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00008BD8 File Offset: 0x00007BD8
		internal void WriteMemberNested(NameInfo memberNameInfo)
		{
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008BDC File Offset: 0x00007BDC
		internal void WriteMemberString(NameInfo memberNameInfo, NameInfo typeNameInfo, string value)
		{
			int num = (int)typeNameInfo.NIobjectId;
			this.attrList.Clear();
			if (num > 0)
			{
				this.attrList.Put("id", this.IdToString((int)typeNameInfo.NIobjectId));
			}
			if (typeNameInfo.NItype != null && (memberNameInfo.NItransmitTypeOnMember || (memberNameInfo.NItransmitTypeOnObject && !memberNameInfo.NIisArrayItem)))
			{
				if (typeNameInfo.NIobjectId > 0L)
				{
					this.attrList.Put("xsi:type", "SOAP-ENC:string");
					this.isUsedEnc = true;
				}
				else
				{
					this.attrList.Put("xsi:type", "xsd:string");
				}
			}
			NameInfo nameInfo = null;
			if (typeNameInfo.NInameSpaceEnum == InternalNameSpaceE.Interop)
			{
				nameInfo = typeNameInfo;
			}
			string text = this.MemberElementName(memberNameInfo, nameInfo);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, text, this.attrList, value, true, Converter.IsEscaped(typeNameInfo.NIprimitiveTypeEnum));
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00008CB4 File Offset: 0x00007CB4
		internal void WriteSingleArray(NameInfo memberNameInfo, NameInfo arrayNameInfo, WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, int length, int lowerBound, Array array)
		{
			this.attrList.Clear();
			if (memberNameInfo.NIobjectId == (long)this.topId)
			{
				this.Write(InternalElementTypeE.ObjectBegin, "SOAP-ENV:Body", this.attrList, null, false, false);
			}
			if (arrayNameInfo.NIobjectId > 1L)
			{
				this.attrList.Put("id", this.IdToString((int)arrayNameInfo.NIobjectId));
			}
			arrayElemTypeNameInfo.NIitemName = this.NameTagResolver(arrayElemTypeNameInfo, true);
			this.attrList.Put("SOAP-ENC:arrayType", this.NameTagResolver(arrayNameInfo, true, memberNameInfo.NIname));
			this.isUsedEnc = true;
			if (lowerBound != 0)
			{
				this.attrList.Put("SOAP-ENC:offset", "[" + lowerBound + "]");
			}
			string text = this.MemberElementName(memberNameInfo, null);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.ObjectBegin, text, this.attrList, null, false, false);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00008D98 File Offset: 0x00007D98
		internal void WriteJaggedArray(NameInfo memberNameInfo, NameInfo arrayNameInfo, WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, int length, int lowerBound)
		{
			this.attrList.Clear();
			if (memberNameInfo.NIobjectId == (long)this.topId)
			{
				this.Write(InternalElementTypeE.ObjectBegin, "SOAP-ENV:Body", this.attrList, null, false, false);
			}
			if (arrayNameInfo.NIobjectId > 1L)
			{
				this.attrList.Put("id", this.IdToString((int)arrayNameInfo.NIobjectId));
			}
			arrayElemTypeNameInfo.NIitemName = "SOAP-ENC:Array";
			this.isUsedEnc = true;
			this.attrList.Put("SOAP-ENC:arrayType", this.TypeArrayNameTagResolver(memberNameInfo, arrayNameInfo, true));
			if (lowerBound != 0)
			{
				this.attrList.Put("SOAP-ENC:offset", "[" + lowerBound + "]");
			}
			string text = this.MemberElementName(memberNameInfo, null);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.ObjectBegin, text, this.attrList, null, false, false);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00008E74 File Offset: 0x00007E74
		internal void WriteRectangleArray(NameInfo memberNameInfo, NameInfo arrayNameInfo, WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, int rank, int[] lengthA, int[] lowerBoundA)
		{
			this.sbOffset.Length = 0;
			this.sbOffset.Append("[");
			bool flag = true;
			for (int i = 0; i < rank; i++)
			{
				if (lowerBoundA[i] != 0)
				{
					flag = false;
				}
				if (i > 0)
				{
					this.sbOffset.Append(",");
				}
				this.sbOffset.Append(lowerBoundA[i]);
			}
			this.sbOffset.Append("]");
			this.attrList.Clear();
			if (memberNameInfo.NIobjectId == (long)this.topId)
			{
				this.Write(InternalElementTypeE.ObjectBegin, "SOAP-ENV:Body", this.attrList, null, false, false);
			}
			if (arrayNameInfo.NIobjectId > 1L)
			{
				this.attrList.Put("id", this.IdToString((int)arrayNameInfo.NIobjectId));
			}
			arrayElemTypeNameInfo.NIitemName = this.NameTagResolver(arrayElemTypeNameInfo, true);
			this.attrList.Put("SOAP-ENC:arrayType", this.TypeArrayNameTagResolver(memberNameInfo, arrayNameInfo, true));
			this.isUsedEnc = true;
			if (!flag)
			{
				this.attrList.Put("SOAP-ENC:offset", this.sbOffset.ToString());
			}
			string text = this.MemberElementName(memberNameInfo, null);
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.ObjectBegin, text, this.attrList, null, false, false);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00008FB0 File Offset: 0x00007FB0
		internal void WriteItem(NameInfo itemNameInfo, NameInfo typeNameInfo, object value)
		{
			this.attrList.Clear();
			if (itemNameInfo.NItransmitTypeOnMember)
			{
				this.attrList.Put("xsi:type", this.TypeNameTagResolver(typeNameInfo, true));
			}
			string text = null;
			if (typeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.QName)
			{
				if (value != null)
				{
					SoapQName soapQName = (SoapQName)value;
					if (soapQName.Key == null || soapQName.Key.Length == 0)
					{
						this.attrList.Put("xmlns", "");
					}
					else
					{
						this.attrList.Put("xmlns:" + soapQName.Key, soapQName.Namespace);
					}
					text = soapQName.ToString();
				}
			}
			else
			{
				text = Converter.SoapToString(value, typeNameInfo.NIprimitiveTypeEnum);
			}
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, "item", this.attrList, text, false, typeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.Invalid);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00009084 File Offset: 0x00008084
		internal void WriteNullItem(NameInfo memberNameInfo, NameInfo typeNameInfo)
		{
			string niname = typeNameInfo.NIname;
			this.attrList.Clear();
			if (typeNameInfo.NItransmitTypeOnMember && !niname.Equals("System.Object") && !niname.Equals("Object") && !niname.Equals("System.Empty") && !niname.Equals("ur-type") && !niname.Equals("anyType"))
			{
				this.attrList.Put("xsi:type", this.TypeNameTagResolver(typeNameInfo, true));
			}
			this.attrList.Put("xsi:null", "1");
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, "item", this.attrList, null, false, false);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00009134 File Offset: 0x00008134
		internal void WriteItemObjectRef(NameInfo itemNameInfo, int arrayId)
		{
			this.attrList.Clear();
			this.attrList.Put("href", this.RefToString(arrayId));
			this.Write(InternalElementTypeE.Member, "item", this.attrList, null, false, false);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00009170 File Offset: 0x00008170
		internal void WriteItemString(NameInfo itemNameInfo, NameInfo typeNameInfo, string value)
		{
			this.attrList.Clear();
			if (typeNameInfo.NIobjectId > 0L)
			{
				this.attrList.Put("id", this.IdToString((int)typeNameInfo.NIobjectId));
			}
			if (itemNameInfo.NItransmitTypeOnMember)
			{
				if (typeNameInfo.NItype == SoapUtil.typeofString)
				{
					if (typeNameInfo.NIobjectId > 0L)
					{
						this.attrList.Put("xsi:type", "SOAP-ENC:string");
						this.isUsedEnc = true;
					}
					else
					{
						this.attrList.Put("xsi:type", "xsd:string");
					}
				}
				else
				{
					this.attrList.Put("xsi:type", this.TypeNameTagResolver(typeNameInfo, true));
				}
			}
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, "item", this.attrList, value, false, Converter.IsEscaped(typeNameInfo.NIprimitiveTypeEnum));
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00009240 File Offset: 0x00008240
		internal void WriteHeader(int objectId, int numMembers)
		{
			this.attrList.Clear();
			this.Write(InternalElementTypeE.ObjectBegin, "SOAP-ENV:Header", this.attrList, null, false, false);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00009264 File Offset: 0x00008264
		internal void WriteHeaderEntry(NameInfo nameInfo, NameInfo typeNameInfo, object value)
		{
			this.attrList.Clear();
			if (value == null)
			{
				this.attrList.Put("xsi:null", "1");
			}
			else
			{
				this.attrList.Put("xsi:type", this.TypeNameTagResolver(typeNameInfo, true));
			}
			if (nameInfo.NIisMustUnderstand)
			{
				this.attrList.Put("SOAP-ENV:mustUnderstand", "1");
				this.isUsedEnc = true;
			}
			this.attrList.Put("xmlns:" + nameInfo.NIheaderPrefix, nameInfo.NInamespace);
			this.attrList.Put("SOAP-ENC:root", "1");
			string text = null;
			if (value != null)
			{
				if (typeNameInfo != null && typeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.QName)
				{
					SoapQName soapQName = (SoapQName)value;
					if (soapQName.Key == null || soapQName.Key.Length == 0)
					{
						this.attrList.Put("xmlns", "");
					}
					else
					{
						this.attrList.Put("xmlns:" + soapQName.Key, soapQName.Namespace);
					}
					text = soapQName.ToString();
				}
				else
				{
					text = Converter.SoapToString(value, typeNameInfo.NIprimitiveTypeEnum);
				}
			}
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, nameInfo.NIheaderPrefix + ":" + nameInfo.NIname, this.attrList, text, true, true);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000093B4 File Offset: 0x000083B4
		internal void WriteHeaderObjectRef(NameInfo nameInfo)
		{
			this.attrList.Clear();
			this.attrList.Put("href", this.RefToString((int)nameInfo.NIobjectId));
			if (nameInfo.NIisMustUnderstand)
			{
				this.attrList.Put("SOAP-ENV:mustUnderstand", "1");
				this.isUsedEnc = true;
			}
			this.attrList.Put("xmlns:" + nameInfo.NIheaderPrefix, nameInfo.NInamespace);
			this.attrList.Put("SOAP-ENC:root", "1");
			this.Write(InternalElementTypeE.Member, nameInfo.NIheaderPrefix + ":" + nameInfo.NIname, this.attrList, null, true, true);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000946C File Offset: 0x0000846C
		internal void WriteHeaderString(NameInfo nameInfo, string value)
		{
			this.attrList.Clear();
			this.attrList.Put("xsi:type", "SOAP-ENC:string");
			this.isUsedEnc = true;
			if (nameInfo.NIisMustUnderstand)
			{
				this.attrList.Put("SOAP-ENV:mustUnderstand", "1");
			}
			this.attrList.Put("xmlns:" + nameInfo.NIheaderPrefix, nameInfo.NInamespace);
			this.attrList.Put("SOAP-ENC:root", "1");
			this.Write(InternalElementTypeE.Member, nameInfo.NIheaderPrefix + ":" + nameInfo.NIname, this.attrList, value, true, true);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000951C File Offset: 0x0000851C
		internal void WriteHeaderMethodSignature(NameInfo nameInfo, NameInfo[] typeNameInfos)
		{
			this.attrList.Clear();
			this.attrList.Put("xsi:type", "SOAP-ENC:methodSignature");
			this.isUsedEnc = true;
			if (nameInfo.NIisMustUnderstand)
			{
				this.attrList.Put("SOAP-ENV:mustUnderstand", "1");
			}
			this.attrList.Put("xmlns:" + nameInfo.NIheaderPrefix, nameInfo.NInamespace);
			this.attrList.Put("SOAP-ENC:root", "1");
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < typeNameInfos.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(this.NameTagResolver(typeNameInfos[i], true));
			}
			this.NamespaceAttribute();
			this.Write(InternalElementTypeE.Member, nameInfo.NIheaderPrefix + ":" + nameInfo.NIname, this.attrList, stringBuilder.ToString(), true, true);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00009606 File Offset: 0x00008606
		internal void WriteAssembly(string typeFullName, Type type, string assemName, int assemId, bool isNew, bool isInteropType)
		{
			if (isNew && isInteropType)
			{
				this.assemblyInfos[this.InteropAssemIdToString(assemId)] = new SoapWriter.AssemblyInfo(assemId, assemName, isInteropType);
			}
			if (!isInteropType)
			{
				this.ParseAssemblyName(typeFullName, assemName);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000963C File Offset: 0x0000863C
		private SoapWriter.DottedInfo ParseAssemblyName(string typeFullName, string assemName)
		{
			SoapWriter.DottedInfo dottedInfo;
			if (this.typeNameToDottedInfoTable.ContainsKey(typeFullName))
			{
				dottedInfo = (SoapWriter.DottedInfo)this.typeNameToDottedInfoTable[typeFullName];
			}
			else
			{
				int num = typeFullName.LastIndexOf('.');
				string text;
				if (num > 0)
				{
					text = typeFullName.Substring(0, num);
				}
				else
				{
					text = "";
				}
				string text2 = SoapServices.CodeXmlNamespaceForClrTypeNamespace(text, assemName);
				string text3 = typeFullName.Substring(num + 1);
				int num2;
				if (this.dottedAssemToAssemIdTable.ContainsKey(text2))
				{
					num2 = (int)this.dottedAssemToAssemIdTable[text2];
				}
				else
				{
					num2 = this.dottedAssemId++;
					this.assemblyInfos[this.AssemIdToString(num2)] = new SoapWriter.AssemblyInfo(num2, text2, false);
					this.dottedAssemToAssemIdTable[text2] = num2;
				}
				dottedInfo = default(SoapWriter.DottedInfo);
				dottedInfo.dottedAssemblyName = text2;
				dottedInfo.name = text3;
				dottedInfo.nameSpace = text;
				dottedInfo.assemId = num2;
				this.typeNameToDottedInfoTable[typeFullName] = dottedInfo;
			}
			return dottedInfo;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000974B File Offset: 0x0000874B
		private string IdToString(int objectId)
		{
			this.sb1.Length = 4;
			this.sb1.Append(objectId);
			return this.sb1.ToString();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00009771 File Offset: 0x00008771
		private string AssemIdToString(int assemId)
		{
			this.sb2.Length = 1;
			this.sb2.Append(assemId);
			return this.sb2.ToString();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00009797 File Offset: 0x00008797
		private string InteropAssemIdToString(int assemId)
		{
			this.sb3.Length = 1;
			this.sb3.Append(assemId);
			return this.sb3.ToString();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000097BD File Offset: 0x000087BD
		private string RefToString(int objectId)
		{
			this.sb4.Length = 5;
			this.sb4.Append(objectId);
			return this.sb4.ToString();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000097E4 File Offset: 0x000087E4
		private string MemberElementName(NameInfo memberNameInfo, NameInfo typeNameInfo)
		{
			string text = memberNameInfo.NIname;
			if (memberNameInfo.NIisHeader)
			{
				text = memberNameInfo.NIheaderPrefix + ":" + memberNameInfo.NIname;
			}
			else if (typeNameInfo != null && typeNameInfo.NItype == SoapUtil.typeofSoapFault)
			{
				text = "SOAP-ENV:Fault";
			}
			else if (memberNameInfo.NIisArray && !memberNameInfo.NIisNestedObject)
			{
				text = "SOAP-ENC:Array";
				this.isUsedEnc = true;
			}
			else if (memberNameInfo.NIisArrayItem)
			{
				text = "item";
			}
			else if (!memberNameInfo.NIisNestedObject && (!memberNameInfo.NIisRemoteRecord || memberNameInfo.NIisTopLevelObject) && typeNameInfo != null)
			{
				text = this.NameTagResolver(typeNameInfo, true);
			}
			return text;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00009888 File Offset: 0x00008888
		private string TypeNameTagResolver(NameInfo typeNameInfo, bool isXsiAppended)
		{
			string text2;
			if (typeNameInfo.NIassemId > 0L && typeNameInfo.NIattributeInfo != null && typeNameInfo.NIattributeInfo.AttributeTypeName != null)
			{
				string text = this.InteropAssemIdToString((int)typeNameInfo.NIassemId);
				text2 = text + ":" + typeNameInfo.NIattributeInfo.AttributeTypeName;
				SoapWriter.AssemblyInfo assemblyInfo = (SoapWriter.AssemblyInfo)this.assemblyInfos[text];
				assemblyInfo.isUsed = true;
				assemblyInfo.prefix = text;
				this.assemblyInfoUsed[assemblyInfo] = 1;
			}
			else
			{
				text2 = this.NameTagResolver(typeNameInfo, isXsiAppended);
			}
			return text2;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00009919 File Offset: 0x00008919
		private string NameTagResolver(NameInfo typeNameInfo, bool isXsiAppended)
		{
			return this.NameTagResolver(typeNameInfo, isXsiAppended, null);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009924 File Offset: 0x00008924
		private string NameTagResolver(NameInfo typeNameInfo, bool isXsiAppended, string arrayItemName)
		{
			string text = typeNameInfo.NIname;
			switch (typeNameInfo.NInameSpaceEnum)
			{
			case InternalNameSpaceE.None:
			case InternalNameSpaceE.UserNameSpace:
			case InternalNameSpaceE.MemberName:
				break;
			case InternalNameSpaceE.Soap:
				text = "SOAP-ENC:" + typeNameInfo.NIname;
				this.isUsedEnc = true;
				break;
			case InternalNameSpaceE.XdrPrimitive:
				if (isXsiAppended)
				{
					text = "xsd:" + typeNameInfo.NIname;
				}
				break;
			case InternalNameSpaceE.XdrString:
				if (isXsiAppended)
				{
					text = "xsd:" + typeNameInfo.NIname;
				}
				break;
			case InternalNameSpaceE.UrtSystem:
				if (typeNameInfo.NItype == SoapUtil.typeofObject)
				{
					text = "xsd:anyType";
				}
				else if (arrayItemName == null)
				{
					SoapWriter.DottedInfo dottedInfo;
					if (this.typeNameToDottedInfoTable.ContainsKey(typeNameInfo.NIname))
					{
						dottedInfo = (SoapWriter.DottedInfo)this.typeNameToDottedInfoTable[typeNameInfo.NIname];
					}
					else
					{
						dottedInfo = this.ParseAssemblyName(typeNameInfo.NIname, null);
					}
					string text2 = this.AssemIdToString(dottedInfo.assemId);
					text = text2 + ":" + dottedInfo.name;
					SoapWriter.AssemblyInfo assemblyInfo = (SoapWriter.AssemblyInfo)this.assemblyInfos[text2];
					assemblyInfo.isUsed = true;
					assemblyInfo.prefix = text2;
					this.assemblyInfoUsed[assemblyInfo] = 1;
				}
				else
				{
					SoapWriter.DottedInfo dottedInfo2;
					if (this.typeNameToDottedInfoTable.ContainsKey(arrayItemName))
					{
						dottedInfo2 = (SoapWriter.DottedInfo)this.typeNameToDottedInfoTable[arrayItemName];
					}
					else
					{
						dottedInfo2 = this.ParseAssemblyName(arrayItemName, null);
					}
					string text3 = this.AssemIdToString(dottedInfo2.assemId);
					text = text3 + ":" + this.DottedDimensionName(dottedInfo2.name, typeNameInfo.NIname);
					SoapWriter.AssemblyInfo assemblyInfo2 = (SoapWriter.AssemblyInfo)this.assemblyInfos[text3];
					assemblyInfo2.isUsed = true;
					assemblyInfo2.prefix = text3;
					this.assemblyInfoUsed[assemblyInfo2] = 1;
				}
				break;
			case InternalNameSpaceE.UrtUser:
				if (typeNameInfo.NIassemId > 0L)
				{
					if (arrayItemName == null)
					{
						if (!this.typeNameToDottedInfoTable.ContainsKey(typeNameInfo.NIname))
						{
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Assembly"), new object[] { typeNameInfo.NIname }));
						}
						SoapWriter.DottedInfo dottedInfo3 = (SoapWriter.DottedInfo)this.typeNameToDottedInfoTable[typeNameInfo.NIname];
						string text4 = this.AssemIdToString(dottedInfo3.assemId);
						text = text4 + ":" + dottedInfo3.name;
						SoapWriter.AssemblyInfo assemblyInfo3 = (SoapWriter.AssemblyInfo)this.assemblyInfos[text4];
						assemblyInfo3.isUsed = true;
						assemblyInfo3.prefix = text4;
						this.assemblyInfoUsed[assemblyInfo3] = 1;
					}
					else
					{
						if (!this.typeNameToDottedInfoTable.ContainsKey(arrayItemName))
						{
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Assembly"), new object[] { typeNameInfo.NIname }));
						}
						SoapWriter.DottedInfo dottedInfo4 = (SoapWriter.DottedInfo)this.typeNameToDottedInfoTable[arrayItemName];
						string text5 = this.AssemIdToString(dottedInfo4.assemId);
						text = text5 + ":" + this.DottedDimensionName(dottedInfo4.name, typeNameInfo.NIname);
						SoapWriter.AssemblyInfo assemblyInfo4 = (SoapWriter.AssemblyInfo)this.assemblyInfos[text5];
						assemblyInfo4.isUsed = true;
						assemblyInfo4.prefix = text5;
						this.assemblyInfoUsed[assemblyInfo4] = 1;
					}
				}
				break;
			case InternalNameSpaceE.Interop:
				if (typeNameInfo.NIattributeInfo != null && typeNameInfo.NIattributeInfo.AttributeElementName != null)
				{
					if (typeNameInfo.NIassemId > 0L)
					{
						string text6 = this.InteropAssemIdToString((int)typeNameInfo.NIassemId);
						text = text6 + ":" + typeNameInfo.NIattributeInfo.AttributeElementName;
						if (arrayItemName != null)
						{
							int num = typeNameInfo.NIname.IndexOf("[");
							text += typeNameInfo.NIname.Substring(num);
						}
						SoapWriter.AssemblyInfo assemblyInfo5 = (SoapWriter.AssemblyInfo)this.assemblyInfos[text6];
						assemblyInfo5.isUsed = true;
						assemblyInfo5.prefix = text6;
						this.assemblyInfoUsed[assemblyInfo5] = 1;
					}
					else
					{
						text = typeNameInfo.NIattributeInfo.AttributeElementName;
					}
				}
				break;
			case InternalNameSpaceE.CallElement:
				if (typeNameInfo.NIassemId > 0L)
				{
					string text7 = this.InteropAssemIdToString((int)typeNameInfo.NIassemId);
					SoapWriter.AssemblyInfo assemblyInfo6 = (SoapWriter.AssemblyInfo)this.assemblyInfos[text7];
					if (assemblyInfo6 == null)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_NameSpaceEnum"), new object[] { typeNameInfo.NInameSpaceEnum }));
					}
					text = text7 + ":" + typeNameInfo.NIname;
					assemblyInfo6.isUsed = true;
					assemblyInfo6.prefix = text7;
					this.assemblyInfoUsed[assemblyInfo6] = 1;
				}
				break;
			default:
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_NameSpaceEnum"), new object[] { typeNameInfo.NInameSpaceEnum }));
			}
			return text;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00009E34 File Offset: 0x00008E34
		private string TypeArrayNameTagResolver(NameInfo memberNameInfo, NameInfo typeNameInfo, bool isXsiAppended)
		{
			string text;
			if (typeNameInfo.NIassemId > 0L && typeNameInfo.NIattributeInfo != null && typeNameInfo.NIattributeInfo.AttributeTypeName != null)
			{
				text = this.InteropAssemIdToString((int)typeNameInfo.NIassemId) + ":" + typeNameInfo.NIattributeInfo.AttributeTypeName;
			}
			else
			{
				text = this.NameTagResolver(typeNameInfo, isXsiAppended, memberNameInfo.NIname);
			}
			return text;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00009E98 File Offset: 0x00008E98
		private void NamespaceAttribute()
		{
			IDictionaryEnumerator enumerator = this.assemblyInfoUsed.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoapWriter.AssemblyInfo assemblyInfo = (SoapWriter.AssemblyInfo)enumerator.Key;
				this.attrList.Put("xmlns:" + assemblyInfo.prefix, assemblyInfo.name);
			}
			this.assemblyInfoUsed.Clear();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009EF4 File Offset: 0x00008EF4
		private string DottedDimensionName(string dottedName, string dimensionName)
		{
			int num = dottedName.IndexOf('[');
			int num2 = dimensionName.IndexOf('[');
			return dottedName.Substring(0, num) + dimensionName.Substring(num2);
		}

		// Token: 0x0400017A RID: 378
		private const int StringBuilderSize = 1024;

		// Token: 0x0400017B RID: 379
		private static string _soapStartStr = "<SOAP-ENV:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:clr=\"http://schemas.microsoft.com/soap/encoding/clr/1.0\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"";

		// Token: 0x0400017C RID: 380
		private static string _soapStart1999Str = "<SOAP-ENV:Envelope xmlns:xsi=\"http://www.w3.org/1999/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/1999/XMLSchema\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"";

		// Token: 0x0400017D RID: 381
		private static string _soapStart2000Str = "<SOAP-ENV:Envelope xmlns:xsi=\"http://www.w3.org/2000/10/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2000/10/XMLSchema\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"";

		// Token: 0x0400017E RID: 382
		private static byte[] _soapStart = Encoding.UTF8.GetBytes(SoapWriter._soapStartStr);

		// Token: 0x0400017F RID: 383
		private static byte[] _soapStart1999 = Encoding.UTF8.GetBytes(SoapWriter._soapStart1999Str);

		// Token: 0x04000180 RID: 384
		private static byte[] _soapStart2000 = Encoding.UTF8.GetBytes(SoapWriter._soapStart2000Str);

		// Token: 0x04000181 RID: 385
		public static Dictionary<char, string> encodingTable = new Dictionary<char, string>();

		// Token: 0x04000182 RID: 386
		private AttributeList attrList = new AttributeList();

		// Token: 0x04000183 RID: 387
		private AttributeList attrValueList = new AttributeList();

		// Token: 0x04000184 RID: 388
		private int lineIndent = 4;

		// Token: 0x04000185 RID: 389
		private int instanceIndent = 1;

		// Token: 0x04000186 RID: 390
		private StringBuilder stringBuffer = new StringBuilder(120);

		// Token: 0x04000187 RID: 391
		private StringBuilder sb = new StringBuilder(120);

		// Token: 0x04000188 RID: 392
		private int topId;

		// Token: 0x04000189 RID: 393
		private int headerId;

		// Token: 0x0400018A RID: 394
		private Hashtable assemblyInfos = new Hashtable(10);

		// Token: 0x0400018B RID: 395
		private StreamWriter writer;

		// Token: 0x0400018C RID: 396
		private Stream stream;

		// Token: 0x0400018D RID: 397
		private Hashtable typeNameToDottedInfoTable;

		// Token: 0x0400018E RID: 398
		private Hashtable dottedAssemToAssemIdTable;

		// Token: 0x0400018F RID: 399
		private Hashtable assemblyInfoUsed = new Hashtable(10);

		// Token: 0x04000190 RID: 400
		private int dottedAssemId = 1;

		// Token: 0x04000191 RID: 401
		internal bool isUsedEnc;

		// Token: 0x04000192 RID: 402
		private XsdVersion xsdVersion = XsdVersion.V2001;

		// Token: 0x04000193 RID: 403
		private NameCache nameCache = new NameCache();

		// Token: 0x04000194 RID: 404
		private StringBuilder traceBuffer;

		// Token: 0x04000195 RID: 405
		private StringBuilder sbOffset = new StringBuilder(10);

		// Token: 0x04000196 RID: 406
		private StringBuilder sb1 = new StringBuilder("ref-", 15);

		// Token: 0x04000197 RID: 407
		private StringBuilder sb2 = new StringBuilder("a-", 15);

		// Token: 0x04000198 RID: 408
		private StringBuilder sb3 = new StringBuilder("i-", 15);

		// Token: 0x04000199 RID: 409
		private StringBuilder sb4 = new StringBuilder("#ref-", 16);

		// Token: 0x02000027 RID: 39
		internal struct DottedInfo
		{
			// Token: 0x0400019A RID: 410
			internal string dottedAssemblyName;

			// Token: 0x0400019B RID: 411
			internal string name;

			// Token: 0x0400019C RID: 412
			internal string nameSpace;

			// Token: 0x0400019D RID: 413
			internal int assemId;
		}

		// Token: 0x02000028 RID: 40
		internal sealed class AssemblyInfo
		{
			// Token: 0x060000D4 RID: 212 RVA: 0x00009F2B File Offset: 0x00008F2B
			internal AssemblyInfo(int id, string name, bool isInteropType)
			{
				this.id = id;
				this.name = name;
				this.isInteropType = isInteropType;
				this.isUsed = false;
			}

			// Token: 0x0400019E RID: 414
			internal int id;

			// Token: 0x0400019F RID: 415
			internal string name;

			// Token: 0x040001A0 RID: 416
			internal string prefix;

			// Token: 0x040001A1 RID: 417
			internal bool isInteropType;

			// Token: 0x040001A2 RID: 418
			internal bool isUsed;
		}
	}
}
