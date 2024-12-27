using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x0200032E RID: 814
	public abstract class XmlSerializationWriter : XmlSerializationGeneratedCode
	{
		// Token: 0x06002750 RID: 10064 RVA: 0x000C7669 File Offset: 0x000C6669
		internal void Init(XmlWriter w, XmlSerializerNamespaces namespaces, string encodingStyle, string idBase, TempAssembly tempAssembly)
		{
			this.w = w;
			this.namespaces = namespaces;
			this.soap12 = encodingStyle == "http://www.w3.org/2003/05/soap-encoding";
			this.idBase = idBase;
			base.Init(tempAssembly);
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x000C769A File Offset: 0x000C669A
		// (set) Token: 0x06002752 RID: 10066 RVA: 0x000C76A2 File Offset: 0x000C66A2
		protected bool EscapeName
		{
			get
			{
				return this.escapeName;
			}
			set
			{
				this.escapeName = value;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06002753 RID: 10067 RVA: 0x000C76AB File Offset: 0x000C66AB
		// (set) Token: 0x06002754 RID: 10068 RVA: 0x000C76B3 File Offset: 0x000C66B3
		protected XmlWriter Writer
		{
			get
			{
				return this.w;
			}
			set
			{
				this.w = value;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x000C76BC File Offset: 0x000C66BC
		// (set) Token: 0x06002756 RID: 10070 RVA: 0x000C76D4 File Offset: 0x000C66D4
		protected ArrayList Namespaces
		{
			get
			{
				if (this.namespaces != null)
				{
					return this.namespaces.NamespaceList;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.namespaces = null;
					return;
				}
				XmlQualifiedName[] array = (XmlQualifiedName[])value.ToArray(typeof(XmlQualifiedName));
				this.namespaces = new XmlSerializerNamespaces(array);
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000C770E File Offset: 0x000C670E
		protected static byte[] FromByteArrayBase64(byte[] value)
		{
			return value;
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000C7711 File Offset: 0x000C6711
		protected static Assembly ResolveDynamicAssembly(string assemblyFullName)
		{
			return DynamicAssemblies.Get(assemblyFullName);
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000C7719 File Offset: 0x000C6719
		protected static string FromByteArrayHex(byte[] value)
		{
			return XmlCustomFormatter.FromByteArrayHex(value);
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000C7721 File Offset: 0x000C6721
		protected static string FromDateTime(DateTime value)
		{
			return XmlCustomFormatter.FromDateTime(value);
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000C7729 File Offset: 0x000C6729
		protected static string FromDate(DateTime value)
		{
			return XmlCustomFormatter.FromDate(value);
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000C7731 File Offset: 0x000C6731
		protected static string FromTime(DateTime value)
		{
			return XmlCustomFormatter.FromTime(value);
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000C7739 File Offset: 0x000C6739
		protected static string FromChar(char value)
		{
			return XmlCustomFormatter.FromChar(value);
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000C7741 File Offset: 0x000C6741
		protected static string FromEnum(long value, string[] values, long[] ids)
		{
			return XmlCustomFormatter.FromEnum(value, values, ids, null);
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000C774C File Offset: 0x000C674C
		protected static string FromEnum(long value, string[] values, long[] ids, string typeName)
		{
			return XmlCustomFormatter.FromEnum(value, values, ids, typeName);
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000C7757 File Offset: 0x000C6757
		protected static string FromXmlName(string name)
		{
			return XmlCustomFormatter.FromXmlName(name);
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000C775F File Offset: 0x000C675F
		protected static string FromXmlNCName(string ncName)
		{
			return XmlCustomFormatter.FromXmlNCName(ncName);
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000C7767 File Offset: 0x000C6767
		protected static string FromXmlNmToken(string nmToken)
		{
			return XmlCustomFormatter.FromXmlNmToken(nmToken);
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000C776F File Offset: 0x000C676F
		protected static string FromXmlNmTokens(string nmTokens)
		{
			return XmlCustomFormatter.FromXmlNmTokens(nmTokens);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000C7777 File Offset: 0x000C6777
		protected void WriteXsiType(string name, string ns)
		{
			this.WriteAttribute("type", "http://www.w3.org/2001/XMLSchema-instance", this.GetQualifiedName(name, ns));
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000C7791 File Offset: 0x000C6791
		private XmlQualifiedName GetPrimitiveTypeName(Type type)
		{
			return this.GetPrimitiveTypeName(type, true);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000C779C File Offset: 0x000C679C
		private XmlQualifiedName GetPrimitiveTypeName(Type type, bool throwIfUnknown)
		{
			XmlQualifiedName primitiveTypeNameInternal = XmlSerializationWriter.GetPrimitiveTypeNameInternal(type);
			if (throwIfUnknown && primitiveTypeNameInternal == null)
			{
				throw this.CreateUnknownTypeException(type);
			}
			return primitiveTypeNameInternal;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000C77C8 File Offset: 0x000C67C8
		internal static XmlQualifiedName GetPrimitiveTypeNameInternal(Type type)
		{
			string text = "http://www.w3.org/2001/XMLSchema";
			string text2;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				text2 = "boolean";
				goto IL_0155;
			case TypeCode.Char:
				text2 = "char";
				text = "http://microsoft.com/wsdl/types/";
				goto IL_0155;
			case TypeCode.SByte:
				text2 = "byte";
				goto IL_0155;
			case TypeCode.Byte:
				text2 = "unsignedByte";
				goto IL_0155;
			case TypeCode.Int16:
				text2 = "short";
				goto IL_0155;
			case TypeCode.UInt16:
				text2 = "unsignedShort";
				goto IL_0155;
			case TypeCode.Int32:
				text2 = "int";
				goto IL_0155;
			case TypeCode.UInt32:
				text2 = "unsignedInt";
				goto IL_0155;
			case TypeCode.Int64:
				text2 = "long";
				goto IL_0155;
			case TypeCode.UInt64:
				text2 = "unsignedLong";
				goto IL_0155;
			case TypeCode.Single:
				text2 = "float";
				goto IL_0155;
			case TypeCode.Double:
				text2 = "double";
				goto IL_0155;
			case TypeCode.Decimal:
				text2 = "decimal";
				goto IL_0155;
			case TypeCode.DateTime:
				text2 = "dateTime";
				goto IL_0155;
			case TypeCode.String:
				text2 = "string";
				goto IL_0155;
			}
			if (type == typeof(XmlQualifiedName))
			{
				text2 = "QName";
			}
			else if (type == typeof(byte[]))
			{
				text2 = "base64Binary";
			}
			else if (type == typeof(Guid))
			{
				text2 = "guid";
				text = "http://microsoft.com/wsdl/types/";
			}
			else
			{
				if (type != typeof(XmlNode[]))
				{
					return null;
				}
				text2 = "anyType";
			}
			IL_0155:
			return new XmlQualifiedName(text2, text);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000C7934 File Offset: 0x000C6934
		protected void WriteTypedPrimitive(string name, string ns, object o, bool xsiType)
		{
			string text = "http://www.w3.org/2001/XMLSchema";
			bool flag = true;
			bool flag2 = false;
			Type type = o.GetType();
			bool flag3 = false;
			string text2;
			string text3;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				text2 = XmlConvert.ToString((bool)o);
				text3 = "boolean";
				goto IL_02E2;
			case TypeCode.Char:
				text2 = XmlSerializationWriter.FromChar((char)o);
				text3 = "char";
				text = "http://microsoft.com/wsdl/types/";
				goto IL_02E2;
			case TypeCode.SByte:
				text2 = XmlConvert.ToString((sbyte)o);
				text3 = "byte";
				goto IL_02E2;
			case TypeCode.Byte:
				text2 = XmlConvert.ToString((byte)o);
				text3 = "unsignedByte";
				goto IL_02E2;
			case TypeCode.Int16:
				text2 = XmlConvert.ToString((short)o);
				text3 = "short";
				goto IL_02E2;
			case TypeCode.UInt16:
				text2 = XmlConvert.ToString((ushort)o);
				text3 = "unsignedShort";
				goto IL_02E2;
			case TypeCode.Int32:
				text2 = XmlConvert.ToString((int)o);
				text3 = "int";
				goto IL_02E2;
			case TypeCode.UInt32:
				text2 = XmlConvert.ToString((uint)o);
				text3 = "unsignedInt";
				goto IL_02E2;
			case TypeCode.Int64:
				text2 = XmlConvert.ToString((long)o);
				text3 = "long";
				goto IL_02E2;
			case TypeCode.UInt64:
				text2 = XmlConvert.ToString((ulong)o);
				text3 = "unsignedLong";
				goto IL_02E2;
			case TypeCode.Single:
				text2 = XmlConvert.ToString((float)o);
				text3 = "float";
				goto IL_02E2;
			case TypeCode.Double:
				text2 = XmlConvert.ToString((double)o);
				text3 = "double";
				goto IL_02E2;
			case TypeCode.Decimal:
				text2 = XmlConvert.ToString((decimal)o);
				text3 = "decimal";
				goto IL_02E2;
			case TypeCode.DateTime:
				text2 = XmlSerializationWriter.FromDateTime((DateTime)o);
				text3 = "dateTime";
				goto IL_02E2;
			case TypeCode.String:
				text2 = (string)o;
				text3 = "string";
				flag = false;
				goto IL_02E2;
			}
			if (type == typeof(XmlQualifiedName))
			{
				text3 = "QName";
				flag3 = true;
				if (name == null)
				{
					this.w.WriteStartElement(text3, text);
				}
				else
				{
					this.w.WriteStartElement(name, ns);
				}
				text2 = this.FromXmlQualifiedName((XmlQualifiedName)o, false);
			}
			else if (type == typeof(byte[]))
			{
				text2 = string.Empty;
				flag2 = true;
				text3 = "base64Binary";
			}
			else if (type == typeof(Guid))
			{
				text2 = XmlConvert.ToString((Guid)o);
				text3 = "guid";
				text = "http://microsoft.com/wsdl/types/";
			}
			else
			{
				if (typeof(XmlNode[]).IsAssignableFrom(type))
				{
					if (name == null)
					{
						this.w.WriteStartElement("anyType", "http://www.w3.org/2001/XMLSchema");
					}
					else
					{
						this.w.WriteStartElement(name, ns);
					}
					XmlNode[] array = (XmlNode[])o;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							array[i].WriteTo(this.w);
						}
					}
					this.w.WriteEndElement();
					return;
				}
				throw this.CreateUnknownTypeException(type);
			}
			IL_02E2:
			if (!flag3)
			{
				if (name == null)
				{
					this.w.WriteStartElement(text3, text);
				}
				else
				{
					this.w.WriteStartElement(name, ns);
				}
			}
			if (xsiType)
			{
				this.WriteXsiType(text3, text);
			}
			if (text2 == null)
			{
				this.w.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
			}
			else if (flag2)
			{
				XmlCustomFormatter.WriteArrayBase64(this.w, (byte[])o, 0, ((byte[])o).Length);
			}
			else if (flag)
			{
				this.w.WriteRaw(text2);
			}
			else
			{
				this.w.WriteString(text2);
			}
			this.w.WriteEndElement();
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000C7CBC File Offset: 0x000C6CBC
		private string GetQualifiedName(string name, string ns)
		{
			if (ns == null || ns.Length == 0)
			{
				return name;
			}
			string text = this.w.LookupPrefix(ns);
			if (text == null)
			{
				if (ns == "http://www.w3.org/XML/1998/namespace")
				{
					text = "xml";
				}
				else
				{
					text = this.NextPrefix();
					this.WriteAttribute("xmlns", text, null, ns);
				}
			}
			else if (text.Length == 0)
			{
				return name;
			}
			return text + ":" + name;
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000C7D28 File Offset: 0x000C6D28
		protected string FromXmlQualifiedName(XmlQualifiedName xmlQualifiedName)
		{
			return this.FromXmlQualifiedName(xmlQualifiedName, true);
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000C7D34 File Offset: 0x000C6D34
		protected string FromXmlQualifiedName(XmlQualifiedName xmlQualifiedName, bool ignoreEmpty)
		{
			if (xmlQualifiedName == null)
			{
				return null;
			}
			if (xmlQualifiedName.IsEmpty && ignoreEmpty)
			{
				return null;
			}
			return this.GetQualifiedName(this.EscapeName ? XmlConvert.EncodeLocalName(xmlQualifiedName.Name) : xmlQualifiedName.Name, xmlQualifiedName.Namespace);
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000C7D80 File Offset: 0x000C6D80
		protected void WriteStartElement(string name)
		{
			this.WriteStartElement(name, null, null, false, null);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000C7D8D File Offset: 0x000C6D8D
		protected void WriteStartElement(string name, string ns)
		{
			this.WriteStartElement(name, ns, null, false, null);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000C7D9A File Offset: 0x000C6D9A
		protected void WriteStartElement(string name, string ns, bool writePrefixed)
		{
			this.WriteStartElement(name, ns, null, writePrefixed, null);
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000C7DA7 File Offset: 0x000C6DA7
		protected void WriteStartElement(string name, string ns, object o)
		{
			this.WriteStartElement(name, ns, o, false, null);
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000C7DB4 File Offset: 0x000C6DB4
		protected void WriteStartElement(string name, string ns, object o, bool writePrefixed)
		{
			this.WriteStartElement(name, ns, o, writePrefixed, null);
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000C7DC4 File Offset: 0x000C6DC4
		protected void WriteStartElement(string name, string ns, object o, bool writePrefixed, XmlSerializerNamespaces xmlns)
		{
			if (o != null && this.objectsInUse != null)
			{
				if (this.objectsInUse.ContainsKey(o))
				{
					throw new InvalidOperationException(Res.GetString("XmlCircularReference", new object[] { o.GetType().FullName }));
				}
				this.objectsInUse.Add(o, o);
			}
			string text = null;
			bool flag = false;
			if (this.namespaces != null)
			{
				foreach (object obj in this.namespaces.Namespaces.Keys)
				{
					string text2 = (string)obj;
					string text3 = (string)this.namespaces.Namespaces[text2];
					if (text2.Length > 0 && text3 == ns)
					{
						text = text2;
					}
					if (text2.Length == 0)
					{
						if (text3 == null || text3.Length == 0)
						{
							flag = true;
						}
						if (ns != text3)
						{
							writePrefixed = true;
						}
					}
				}
				this.usedPrefixes = this.ListUsedPrefixes(this.namespaces.Namespaces, this.aliasBase);
			}
			if (writePrefixed && text == null && ns != null && ns.Length > 0)
			{
				text = this.w.LookupPrefix(ns);
				if (text == null || text.Length == 0)
				{
					text = this.NextPrefix();
				}
			}
			if (text == null && xmlns != null)
			{
				text = xmlns.LookupPrefix(ns);
			}
			if (flag && text == null && ns != null && ns.Length != 0)
			{
				text = this.NextPrefix();
			}
			this.w.WriteStartElement(text, name, ns);
			if (this.namespaces != null)
			{
				foreach (object obj2 in this.namespaces.Namespaces.Keys)
				{
					string text4 = (string)obj2;
					string text5 = (string)this.namespaces.Namespaces[text4];
					if (text4.Length != 0 || (text5 != null && text5.Length != 0))
					{
						if (text5 == null || text5.Length == 0)
						{
							if (text4.Length > 0)
							{
								throw new InvalidOperationException(Res.GetString("XmlInvalidXmlns", new object[] { text4 }));
							}
							this.WriteAttribute("xmlns", text4, null, text5);
						}
						else if (this.w.LookupPrefix(text5) == null)
						{
							if (text == null && text4.Length == 0)
							{
								break;
							}
							this.WriteAttribute("xmlns", text4, null, text5);
						}
					}
				}
			}
			this.WriteNamespaceDeclarations(xmlns);
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000C8068 File Offset: 0x000C7068
		private Hashtable ListUsedPrefixes(Hashtable nsList, string prefix)
		{
			Hashtable hashtable = new Hashtable();
			int length = prefix.Length;
			foreach (object obj in this.namespaces.Namespaces.Keys)
			{
				string text = (string)obj;
				if (text.Length > length)
				{
					string text2 = text;
					int length2 = text2.Length;
					if (text2.Length > length && text2.Length <= length + "2147483647".Length && text2.StartsWith(prefix, StringComparison.Ordinal))
					{
						bool flag = true;
						for (int i = length; i < text2.Length; i++)
						{
							if (!char.IsDigit(text2, i))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							long num = long.Parse(text2.Substring(length), CultureInfo.InvariantCulture);
							if (num <= 2147483647L)
							{
								int num2 = (int)num;
								if (!hashtable.ContainsKey(num2))
								{
									hashtable.Add(num2, num2);
								}
							}
						}
					}
				}
			}
			if (hashtable.Count > 0)
			{
				return hashtable;
			}
			return null;
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000C819C File Offset: 0x000C719C
		protected void WriteNullTagEncoded(string name)
		{
			this.WriteNullTagEncoded(name, null);
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000C81A6 File Offset: 0x000C71A6
		protected void WriteNullTagEncoded(string name, string ns)
		{
			if (name == null || name.Length == 0)
			{
				return;
			}
			this.WriteStartElement(name, ns, null, true);
			this.w.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
			this.w.WriteEndElement();
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000C81E3 File Offset: 0x000C71E3
		protected void WriteNullTagLiteral(string name)
		{
			this.WriteNullTagLiteral(name, null);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000C81ED File Offset: 0x000C71ED
		protected void WriteNullTagLiteral(string name, string ns)
		{
			if (name == null || name.Length == 0)
			{
				return;
			}
			this.WriteStartElement(name, ns, null, false);
			this.w.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
			this.w.WriteEndElement();
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000C822A File Offset: 0x000C722A
		protected void WriteEmptyTag(string name)
		{
			this.WriteEmptyTag(name, null);
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000C8234 File Offset: 0x000C7234
		protected void WriteEmptyTag(string name, string ns)
		{
			if (name == null || name.Length == 0)
			{
				return;
			}
			this.WriteStartElement(name, ns, null, false);
			this.w.WriteEndElement();
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000C8257 File Offset: 0x000C7257
		protected void WriteEndElement()
		{
			this.w.WriteEndElement();
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000C8264 File Offset: 0x000C7264
		protected void WriteEndElement(object o)
		{
			this.w.WriteEndElement();
			if (o != null && this.objectsInUse != null)
			{
				this.objectsInUse.Remove(o);
			}
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000C8288 File Offset: 0x000C7288
		protected void WriteSerializable(IXmlSerializable serializable, string name, string ns, bool isNullable)
		{
			this.WriteSerializable(serializable, name, ns, isNullable, true);
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000C8296 File Offset: 0x000C7296
		protected void WriteSerializable(IXmlSerializable serializable, string name, string ns, bool isNullable, bool wrapped)
		{
			if (serializable == null)
			{
				if (isNullable)
				{
					this.WriteNullTagLiteral(name, ns);
				}
				return;
			}
			if (wrapped)
			{
				this.w.WriteStartElement(name, ns);
			}
			serializable.WriteXml(this.w);
			if (wrapped)
			{
				this.w.WriteEndElement();
			}
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000C82D4 File Offset: 0x000C72D4
		protected void WriteNullableStringEncoded(string name, string ns, string value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				this.WriteNullTagEncoded(name, ns);
				return;
			}
			this.WriteElementString(name, ns, value, xsiType);
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000C82ED File Offset: 0x000C72ED
		protected void WriteNullableStringLiteral(string name, string ns, string value)
		{
			if (value == null)
			{
				this.WriteNullTagLiteral(name, ns);
				return;
			}
			this.WriteElementString(name, ns, value, null);
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000C8305 File Offset: 0x000C7305
		protected void WriteNullableStringEncodedRaw(string name, string ns, string value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				this.WriteNullTagEncoded(name, ns);
				return;
			}
			this.WriteElementStringRaw(name, ns, value, xsiType);
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000C831E File Offset: 0x000C731E
		protected void WriteNullableStringEncodedRaw(string name, string ns, byte[] value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				this.WriteNullTagEncoded(name, ns);
				return;
			}
			this.WriteElementStringRaw(name, ns, value, xsiType);
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000C8337 File Offset: 0x000C7337
		protected void WriteNullableStringLiteralRaw(string name, string ns, string value)
		{
			if (value == null)
			{
				this.WriteNullTagLiteral(name, ns);
				return;
			}
			this.WriteElementStringRaw(name, ns, value, null);
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000C834F File Offset: 0x000C734F
		protected void WriteNullableStringLiteralRaw(string name, string ns, byte[] value)
		{
			if (value == null)
			{
				this.WriteNullTagLiteral(name, ns);
				return;
			}
			this.WriteElementStringRaw(name, ns, value, null);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000C8367 File Offset: 0x000C7367
		protected void WriteNullableQualifiedNameEncoded(string name, string ns, XmlQualifiedName value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				this.WriteNullTagEncoded(name, ns);
				return;
			}
			this.WriteElementQualifiedName(name, ns, value, xsiType);
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000C8386 File Offset: 0x000C7386
		protected void WriteNullableQualifiedNameLiteral(string name, string ns, XmlQualifiedName value)
		{
			if (value == null)
			{
				this.WriteNullTagLiteral(name, ns);
				return;
			}
			this.WriteElementQualifiedName(name, ns, value, null);
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000C83A4 File Offset: 0x000C73A4
		protected void WriteElementEncoded(XmlNode node, string name, string ns, bool isNullable, bool any)
		{
			if (node == null)
			{
				if (isNullable)
				{
					this.WriteNullTagEncoded(name, ns);
				}
				return;
			}
			this.WriteElement(node, name, ns, isNullable, any);
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000C83C3 File Offset: 0x000C73C3
		protected void WriteElementLiteral(XmlNode node, string name, string ns, bool isNullable, bool any)
		{
			if (node == null)
			{
				if (isNullable)
				{
					this.WriteNullTagLiteral(name, ns);
				}
				return;
			}
			this.WriteElement(node, name, ns, isNullable, any);
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000C83E4 File Offset: 0x000C73E4
		private void WriteElement(XmlNode node, string name, string ns, bool isNullable, bool any)
		{
			if (typeof(XmlAttribute).IsAssignableFrom(node.GetType()))
			{
				throw new InvalidOperationException(Res.GetString("XmlNoAttributeHere"));
			}
			if (node is XmlDocument)
			{
				node = ((XmlDocument)node).DocumentElement;
				if (node == null)
				{
					if (isNullable)
					{
						this.WriteNullTagEncoded(name, ns);
					}
					return;
				}
			}
			if (any)
			{
				if (node is XmlElement && name != null && name.Length > 0 && (node.LocalName != name || node.NamespaceURI != ns))
				{
					throw new InvalidOperationException(Res.GetString("XmlElementNameMismatch", new object[] { node.LocalName, node.NamespaceURI, name, ns }));
				}
			}
			else
			{
				this.w.WriteStartElement(name, ns);
			}
			node.WriteTo(this.w);
			if (!any)
			{
				this.w.WriteEndElement();
			}
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000C84CB File Offset: 0x000C74CB
		protected Exception CreateUnknownTypeException(object o)
		{
			return this.CreateUnknownTypeException(o.GetType());
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000C84DC File Offset: 0x000C74DC
		protected Exception CreateUnknownTypeException(Type type)
		{
			if (typeof(IXmlSerializable).IsAssignableFrom(type))
			{
				return new InvalidOperationException(Res.GetString("XmlInvalidSerializable", new object[] { type.FullName }));
			}
			TypeDesc typeDesc = new TypeScope().GetTypeDesc(type);
			if (!typeDesc.IsStructLike)
			{
				return new InvalidOperationException(Res.GetString("XmlInvalidUseOfType", new object[] { type.FullName }));
			}
			return new InvalidOperationException(Res.GetString("XmlUnxpectedType", new object[] { type.FullName }));
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000C8574 File Offset: 0x000C7574
		protected Exception CreateMismatchChoiceException(string value, string elementName, string enumValue)
		{
			return new InvalidOperationException(Res.GetString("XmlChoiceMismatchChoiceException", new object[] { elementName, value, enumValue }));
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000C85A4 File Offset: 0x000C75A4
		protected Exception CreateUnknownAnyElementException(string name, string ns)
		{
			return new InvalidOperationException(Res.GetString("XmlUnknownAnyElement", new object[] { name, ns }));
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000C85D0 File Offset: 0x000C75D0
		protected Exception CreateInvalidChoiceIdentifierValueException(string type, string identifier)
		{
			return new InvalidOperationException(Res.GetString("XmlInvalidChoiceIdentifierValue", new object[] { type, identifier }));
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000C85FC File Offset: 0x000C75FC
		protected Exception CreateChoiceIdentifierValueException(string value, string identifier, string name, string ns)
		{
			return new InvalidOperationException(Res.GetString("XmlChoiceIdentifierMismatch", new object[] { value, identifier, name, ns }));
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000C8634 File Offset: 0x000C7634
		protected Exception CreateInvalidEnumValueException(object value, string typeName)
		{
			return new InvalidOperationException(Res.GetString("XmlUnknownConstant", new object[] { value, typeName }));
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000C8660 File Offset: 0x000C7660
		protected Exception CreateInvalidAnyTypeException(object o)
		{
			return this.CreateInvalidAnyTypeException(o.GetType());
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000C8670 File Offset: 0x000C7670
		protected Exception CreateInvalidAnyTypeException(Type type)
		{
			return new InvalidOperationException(Res.GetString("XmlIllegalAnyElement", new object[] { type.FullName }));
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000C869D File Offset: 0x000C769D
		protected void WriteReferencingElement(string n, string ns, object o)
		{
			this.WriteReferencingElement(n, ns, o, false);
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000C86AC File Offset: 0x000C76AC
		protected void WriteReferencingElement(string n, string ns, object o, bool isNullable)
		{
			if (o == null)
			{
				if (isNullable)
				{
					this.WriteNullTagEncoded(n, ns);
				}
				return;
			}
			this.WriteStartElement(n, ns, null, true);
			if (this.soap12)
			{
				this.w.WriteAttributeString("ref", "http://www.w3.org/2003/05/soap-encoding", this.GetId(o, true));
			}
			else
			{
				this.w.WriteAttributeString("href", "#" + this.GetId(o, true));
			}
			this.w.WriteEndElement();
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000C8727 File Offset: 0x000C7727
		private bool IsIdDefined(object o)
		{
			return this.references != null && this.references.Contains(o);
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000C8740 File Offset: 0x000C7740
		private string GetId(object o, bool addToReferencesList)
		{
			if (this.references == null)
			{
				this.references = new Hashtable();
				this.referencesToWrite = new ArrayList();
			}
			string text = (string)this.references[o];
			if (text == null)
			{
				string text2 = this.idBase;
				string text3 = "id";
				int num = ++this.nextId;
				text = text2 + text3 + num.ToString(CultureInfo.InvariantCulture);
				this.references.Add(o, text);
				if (addToReferencesList)
				{
					this.referencesToWrite.Add(o);
				}
			}
			return text;
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000C87CD File Offset: 0x000C77CD
		protected void WriteId(object o)
		{
			this.WriteId(o, true);
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000C87D7 File Offset: 0x000C77D7
		private void WriteId(object o, bool addToReferencesList)
		{
			if (this.soap12)
			{
				this.w.WriteAttributeString("id", "http://www.w3.org/2003/05/soap-encoding", this.GetId(o, addToReferencesList));
				return;
			}
			this.w.WriteAttributeString("id", this.GetId(o, addToReferencesList));
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000C8817 File Offset: 0x000C7817
		protected void WriteXmlAttribute(XmlNode node)
		{
			this.WriteXmlAttribute(node, null);
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000C8824 File Offset: 0x000C7824
		protected void WriteXmlAttribute(XmlNode node, object container)
		{
			XmlAttribute xmlAttribute = node as XmlAttribute;
			if (xmlAttribute == null)
			{
				throw new InvalidOperationException(Res.GetString("XmlNeedAttributeHere"));
			}
			if (xmlAttribute.Value != null)
			{
				if (xmlAttribute.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && xmlAttribute.LocalName == "arrayType")
				{
					string text;
					XmlQualifiedName xmlQualifiedName = TypeScope.ParseWsdlArrayType(xmlAttribute.Value, out text, (container is XmlSchemaObject) ? ((XmlSchemaObject)container) : null);
					string text2 = this.FromXmlQualifiedName(xmlQualifiedName, true) + text;
					this.WriteAttribute("arrayType", "http://schemas.xmlsoap.org/wsdl/", text2);
					return;
				}
				this.WriteAttribute(xmlAttribute.Name, xmlAttribute.NamespaceURI, xmlAttribute.Value);
			}
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000C88D0 File Offset: 0x000C78D0
		protected void WriteAttribute(string localName, string ns, string value)
		{
			if (value == null)
			{
				return;
			}
			if (!(localName == "xmlns"))
			{
				if (localName.StartsWith("xmlns:", StringComparison.Ordinal))
				{
					return;
				}
				int num = localName.IndexOf(':');
				if (num < 0)
				{
					if (ns == "http://www.w3.org/XML/1998/namespace")
					{
						string text = this.w.LookupPrefix(ns);
						if (text == null || text.Length == 0)
						{
							text = "xml";
						}
						this.w.WriteAttributeString(text, localName, ns, value);
						return;
					}
					this.w.WriteAttributeString(localName, ns, value);
					return;
				}
				else
				{
					string text2 = localName.Substring(0, num);
					this.w.WriteAttributeString(text2, localName.Substring(num + 1), ns, value);
				}
			}
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000C8978 File Offset: 0x000C7978
		protected void WriteAttribute(string localName, string ns, byte[] value)
		{
			if (value == null)
			{
				return;
			}
			if (!(localName == "xmlns"))
			{
				if (localName.StartsWith("xmlns:", StringComparison.Ordinal))
				{
					return;
				}
				int num = localName.IndexOf(':');
				if (num < 0)
				{
					if (ns == "http://www.w3.org/XML/1998/namespace")
					{
						string text = this.w.LookupPrefix(ns);
						if (text == null || text.Length == 0)
						{
						}
						this.w.WriteStartAttribute("xml", localName, ns);
					}
					else
					{
						this.w.WriteStartAttribute(null, localName, ns);
					}
				}
				else
				{
					string text2 = localName.Substring(0, num);
					text2 = this.w.LookupPrefix(ns);
					this.w.WriteStartAttribute(text2, localName.Substring(num + 1), ns);
				}
				XmlCustomFormatter.WriteArrayBase64(this.w, value, 0, value.Length);
				this.w.WriteEndAttribute();
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000C8A4B File Offset: 0x000C7A4B
		protected void WriteAttribute(string localName, string value)
		{
			if (value == null)
			{
				return;
			}
			this.w.WriteAttributeString(localName, null, value);
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000C8A5F File Offset: 0x000C7A5F
		protected void WriteAttribute(string localName, byte[] value)
		{
			if (value == null)
			{
				return;
			}
			this.w.WriteStartAttribute(null, localName, null);
			XmlCustomFormatter.WriteArrayBase64(this.w, value, 0, value.Length);
			this.w.WriteEndAttribute();
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000C8A8E File Offset: 0x000C7A8E
		protected void WriteAttribute(string prefix, string localName, string ns, string value)
		{
			if (value == null)
			{
				return;
			}
			this.w.WriteAttributeString(prefix, localName, null, value);
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000C8AA5 File Offset: 0x000C7AA5
		protected void WriteValue(string value)
		{
			if (value == null)
			{
				return;
			}
			this.w.WriteString(value);
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000C8AB7 File Offset: 0x000C7AB7
		protected void WriteValue(byte[] value)
		{
			if (value == null)
			{
				return;
			}
			XmlCustomFormatter.WriteArrayBase64(this.w, value, 0, value.Length);
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000C8ACD File Offset: 0x000C7ACD
		protected void WriteStartDocument()
		{
			if (this.w.WriteState == WriteState.Start)
			{
				this.w.WriteStartDocument();
			}
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000C8AE7 File Offset: 0x000C7AE7
		protected void WriteElementString(string localName, string value)
		{
			this.WriteElementString(localName, null, value, null);
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000C8AF3 File Offset: 0x000C7AF3
		protected void WriteElementString(string localName, string ns, string value)
		{
			this.WriteElementString(localName, ns, value, null);
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000C8AFF File Offset: 0x000C7AFF
		protected void WriteElementString(string localName, string value, XmlQualifiedName xsiType)
		{
			this.WriteElementString(localName, null, value, xsiType);
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000C8B0C File Offset: 0x000C7B0C
		protected void WriteElementString(string localName, string ns, string value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				return;
			}
			if (xsiType == null)
			{
				this.w.WriteElementString(localName, ns, value);
				return;
			}
			this.w.WriteStartElement(localName, ns);
			this.WriteXsiType(xsiType.Name, xsiType.Namespace);
			this.w.WriteString(value);
			this.w.WriteEndElement();
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x000C8B6E File Offset: 0x000C7B6E
		protected void WriteElementStringRaw(string localName, string value)
		{
			this.WriteElementStringRaw(localName, null, value, null);
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x000C8B7A File Offset: 0x000C7B7A
		protected void WriteElementStringRaw(string localName, byte[] value)
		{
			this.WriteElementStringRaw(localName, null, value, null);
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000C8B86 File Offset: 0x000C7B86
		protected void WriteElementStringRaw(string localName, string ns, string value)
		{
			this.WriteElementStringRaw(localName, ns, value, null);
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000C8B92 File Offset: 0x000C7B92
		protected void WriteElementStringRaw(string localName, string ns, byte[] value)
		{
			this.WriteElementStringRaw(localName, ns, value, null);
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000C8B9E File Offset: 0x000C7B9E
		protected void WriteElementStringRaw(string localName, string value, XmlQualifiedName xsiType)
		{
			this.WriteElementStringRaw(localName, null, value, xsiType);
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000C8BAA File Offset: 0x000C7BAA
		protected void WriteElementStringRaw(string localName, byte[] value, XmlQualifiedName xsiType)
		{
			this.WriteElementStringRaw(localName, null, value, xsiType);
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000C8BB8 File Offset: 0x000C7BB8
		protected void WriteElementStringRaw(string localName, string ns, string value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				return;
			}
			this.w.WriteStartElement(localName, ns);
			if (xsiType != null)
			{
				this.WriteXsiType(xsiType.Name, xsiType.Namespace);
			}
			this.w.WriteRaw(value);
			this.w.WriteEndElement();
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000C8C0C File Offset: 0x000C7C0C
		protected void WriteElementStringRaw(string localName, string ns, byte[] value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				return;
			}
			this.w.WriteStartElement(localName, ns);
			if (xsiType != null)
			{
				this.WriteXsiType(xsiType.Name, xsiType.Namespace);
			}
			XmlCustomFormatter.WriteArrayBase64(this.w, value, 0, value.Length);
			this.w.WriteEndElement();
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000C8C63 File Offset: 0x000C7C63
		protected void WriteRpcResult(string name, string ns)
		{
			if (!this.soap12)
			{
				return;
			}
			this.WriteElementQualifiedName("result", "http://www.w3.org/2003/05/soap-rpc", new XmlQualifiedName(name, ns), null);
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000C8C86 File Offset: 0x000C7C86
		protected void WriteElementQualifiedName(string localName, XmlQualifiedName value)
		{
			this.WriteElementQualifiedName(localName, null, value, null);
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000C8C92 File Offset: 0x000C7C92
		protected void WriteElementQualifiedName(string localName, XmlQualifiedName value, XmlQualifiedName xsiType)
		{
			this.WriteElementQualifiedName(localName, null, value, xsiType);
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000C8C9E File Offset: 0x000C7C9E
		protected void WriteElementQualifiedName(string localName, string ns, XmlQualifiedName value)
		{
			this.WriteElementQualifiedName(localName, ns, value, null);
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000C8CAC File Offset: 0x000C7CAC
		protected void WriteElementQualifiedName(string localName, string ns, XmlQualifiedName value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				return;
			}
			if (value.Namespace == null || value.Namespace.Length == 0)
			{
				this.WriteStartElement(localName, ns, null, true);
				this.WriteAttribute("xmlns", "");
			}
			else
			{
				this.w.WriteStartElement(localName, ns);
			}
			if (xsiType != null)
			{
				this.WriteXsiType(xsiType.Name, xsiType.Namespace);
			}
			this.w.WriteString(this.FromXmlQualifiedName(value, false));
			this.w.WriteEndElement();
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000C8D40 File Offset: 0x000C7D40
		protected void AddWriteCallback(Type type, string typeName, string typeNs, XmlSerializationWriteCallback callback)
		{
			XmlSerializationWriter.TypeEntry typeEntry = new XmlSerializationWriter.TypeEntry();
			typeEntry.typeName = typeName;
			typeEntry.typeNs = typeNs;
			typeEntry.type = type;
			typeEntry.callback = callback;
			this.typeEntries[type] = typeEntry;
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000C8D80 File Offset: 0x000C7D80
		private void WriteArray(string name, string ns, object o, Type type)
		{
			Type type2 = TypeScope.GetArrayElementType(type, null);
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.soap12)
			{
				while ((type2.IsArray || typeof(IEnumerable).IsAssignableFrom(type2)) && this.GetPrimitiveTypeName(type2, false) == null)
				{
					type2 = TypeScope.GetArrayElementType(type2, null);
					stringBuilder.Append("[]");
				}
			}
			string text;
			string text2;
			if (type2 == typeof(object))
			{
				text = "anyType";
				text2 = "http://www.w3.org/2001/XMLSchema";
			}
			else
			{
				XmlSerializationWriter.TypeEntry typeEntry = this.GetTypeEntry(type2);
				if (typeEntry != null)
				{
					text = typeEntry.typeName;
					text2 = typeEntry.typeNs;
				}
				else if (this.soap12)
				{
					XmlQualifiedName primitiveTypeName = this.GetPrimitiveTypeName(type2, false);
					if (primitiveTypeName != null)
					{
						text = primitiveTypeName.Name;
						text2 = primitiveTypeName.Namespace;
					}
					else
					{
						for (Type type3 = type2.BaseType; type3 != null; type3 = type3.BaseType)
						{
							typeEntry = this.GetTypeEntry(type3);
							if (typeEntry != null)
							{
								break;
							}
						}
						if (typeEntry != null)
						{
							text = typeEntry.typeName;
							text2 = typeEntry.typeNs;
						}
						else
						{
							text = "anyType";
							text2 = "http://www.w3.org/2001/XMLSchema";
						}
					}
				}
				else
				{
					XmlQualifiedName primitiveTypeName2 = this.GetPrimitiveTypeName(type2);
					text = primitiveTypeName2.Name;
					text2 = primitiveTypeName2.Namespace;
				}
			}
			if (stringBuilder.Length > 0)
			{
				text += stringBuilder.ToString();
			}
			if (this.soap12 && name != null && name.Length > 0)
			{
				this.WriteStartElement(name, ns, null, false);
			}
			else
			{
				this.WriteStartElement("Array", "http://schemas.xmlsoap.org/soap/encoding/", null, true);
			}
			this.WriteId(o, false);
			if (type.IsArray)
			{
				Array array = (Array)o;
				int length = array.Length;
				if (this.soap12)
				{
					this.w.WriteAttributeString("itemType", "http://www.w3.org/2003/05/soap-encoding", this.GetQualifiedName(text, text2));
					this.w.WriteAttributeString("arraySize", "http://www.w3.org/2003/05/soap-encoding", length.ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					this.w.WriteAttributeString("arrayType", "http://schemas.xmlsoap.org/soap/encoding/", this.GetQualifiedName(text, text2) + "[" + length.ToString(CultureInfo.InvariantCulture) + "]");
				}
				for (int i = 0; i < length; i++)
				{
					this.WritePotentiallyReferencingElement("Item", "", array.GetValue(i), type2, false, true);
				}
			}
			else
			{
				int num = (typeof(ICollection).IsAssignableFrom(type) ? ((ICollection)o).Count : (-1));
				if (this.soap12)
				{
					this.w.WriteAttributeString("itemType", "http://www.w3.org/2003/05/soap-encoding", this.GetQualifiedName(text, text2));
					if (num >= 0)
					{
						this.w.WriteAttributeString("arraySize", "http://www.w3.org/2003/05/soap-encoding", num.ToString(CultureInfo.InvariantCulture));
					}
				}
				else
				{
					string text3 = ((num >= 0) ? ("[" + num + "]") : "[]");
					this.w.WriteAttributeString("arrayType", "http://schemas.xmlsoap.org/soap/encoding/", this.GetQualifiedName(text, text2) + text3);
				}
				IEnumerator enumerator = ((IEnumerable)o).GetEnumerator();
				if (enumerator != null)
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						this.WritePotentiallyReferencingElement("Item", "", obj, type2, false, true);
					}
				}
			}
			this.w.WriteEndElement();
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000C90CC File Offset: 0x000C80CC
		protected void WritePotentiallyReferencingElement(string n, string ns, object o)
		{
			this.WritePotentiallyReferencingElement(n, ns, o, null, false, false);
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000C90DA File Offset: 0x000C80DA
		protected void WritePotentiallyReferencingElement(string n, string ns, object o, Type ambientType)
		{
			this.WritePotentiallyReferencingElement(n, ns, o, ambientType, false, false);
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000C90E9 File Offset: 0x000C80E9
		protected void WritePotentiallyReferencingElement(string n, string ns, object o, Type ambientType, bool suppressReference)
		{
			this.WritePotentiallyReferencingElement(n, ns, o, ambientType, suppressReference, false);
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000C90FC File Offset: 0x000C80FC
		protected void WritePotentiallyReferencingElement(string n, string ns, object o, Type ambientType, bool suppressReference, bool isNullable)
		{
			if (o == null)
			{
				if (isNullable)
				{
					this.WriteNullTagEncoded(n, ns);
				}
				return;
			}
			Type type = o.GetType();
			if (Convert.GetTypeCode(o) == TypeCode.Object && !(o is Guid) && type != typeof(XmlQualifiedName) && !(o is XmlNode[]) && type != typeof(byte[]))
			{
				if ((suppressReference || this.soap12) && !this.IsIdDefined(o))
				{
					this.WriteReferencedElement(n, ns, o, ambientType);
					return;
				}
				if (n == null)
				{
					XmlSerializationWriter.TypeEntry typeEntry = this.GetTypeEntry(type);
					this.WriteReferencingElement(typeEntry.typeName, typeEntry.typeNs, o, isNullable);
					return;
				}
				this.WriteReferencingElement(n, ns, o, isNullable);
				return;
			}
			else
			{
				bool flag = type != ambientType && !type.IsEnum;
				XmlSerializationWriter.TypeEntry typeEntry2 = this.GetTypeEntry(type);
				if (typeEntry2 != null)
				{
					if (n == null)
					{
						this.WriteStartElement(typeEntry2.typeName, typeEntry2.typeNs, null, true);
					}
					else
					{
						this.WriteStartElement(n, ns, null, true);
					}
					if (flag)
					{
						this.WriteXsiType(typeEntry2.typeName, typeEntry2.typeNs);
					}
					typeEntry2.callback(o);
					this.w.WriteEndElement();
					return;
				}
				this.WriteTypedPrimitive(n, ns, o, flag);
				return;
			}
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000C9218 File Offset: 0x000C8218
		private void WriteReferencedElement(object o, Type ambientType)
		{
			this.WriteReferencedElement(null, null, o, ambientType);
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000C9224 File Offset: 0x000C8224
		private void WriteReferencedElement(string name, string ns, object o, Type ambientType)
		{
			if (name == null)
			{
				name = string.Empty;
			}
			Type type = o.GetType();
			if (type.IsArray || typeof(IEnumerable).IsAssignableFrom(type))
			{
				this.WriteArray(name, ns, o, type);
				return;
			}
			XmlSerializationWriter.TypeEntry typeEntry = this.GetTypeEntry(type);
			if (typeEntry == null)
			{
				throw this.CreateUnknownTypeException(type);
			}
			this.WriteStartElement((name.Length == 0) ? typeEntry.typeName : name, (ns == null) ? typeEntry.typeNs : ns, null, true);
			this.WriteId(o, false);
			if (ambientType != type)
			{
				this.WriteXsiType(typeEntry.typeName, typeEntry.typeNs);
			}
			typeEntry.callback(o);
			this.w.WriteEndElement();
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000C92D5 File Offset: 0x000C82D5
		private XmlSerializationWriter.TypeEntry GetTypeEntry(Type t)
		{
			if (this.typeEntries == null)
			{
				this.typeEntries = new Hashtable();
				this.InitCallbacks();
			}
			return (XmlSerializationWriter.TypeEntry)this.typeEntries[t];
		}

		// Token: 0x060027BB RID: 10171
		protected abstract void InitCallbacks();

		// Token: 0x060027BC RID: 10172 RVA: 0x000C9304 File Offset: 0x000C8304
		protected void WriteReferencedElements()
		{
			if (this.referencesToWrite == null)
			{
				return;
			}
			for (int i = 0; i < this.referencesToWrite.Count; i++)
			{
				this.WriteReferencedElement(this.referencesToWrite[i], null);
			}
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000C9343 File Offset: 0x000C8343
		protected void TopLevelElement()
		{
			this.objectsInUse = new Hashtable();
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000C9350 File Offset: 0x000C8350
		protected void WriteNamespaceDeclarations(XmlSerializerNamespaces xmlns)
		{
			if (xmlns != null)
			{
				foreach (object obj in xmlns.Namespaces)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text = (string)dictionaryEntry.Key;
					string text2 = (string)dictionaryEntry.Value;
					if (this.namespaces != null)
					{
						string text3 = this.namespaces.Namespaces[text] as string;
						if (text3 != null && text3 != text2)
						{
							throw new InvalidOperationException(Res.GetString("XmlDuplicateNs", new object[] { text, text2 }));
						}
					}
					string text4 = ((text2 == null || text2.Length == 0) ? null : this.Writer.LookupPrefix(text2));
					if (text4 == null || text4 != text)
					{
						this.WriteAttribute("xmlns", text, null, text2);
					}
				}
			}
			this.namespaces = null;
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000C9460 File Offset: 0x000C8460
		private string NextPrefix()
		{
			if (this.usedPrefixes == null)
			{
				return this.aliasBase + ++this.tempNamespacePrefix;
			}
			while (this.usedPrefixes.ContainsKey(++this.tempNamespacePrefix))
			{
			}
			return this.aliasBase + this.tempNamespacePrefix;
		}

		// Token: 0x04001654 RID: 5716
		private XmlWriter w;

		// Token: 0x04001655 RID: 5717
		private XmlSerializerNamespaces namespaces;

		// Token: 0x04001656 RID: 5718
		private int tempNamespacePrefix;

		// Token: 0x04001657 RID: 5719
		private Hashtable usedPrefixes;

		// Token: 0x04001658 RID: 5720
		private Hashtable references;

		// Token: 0x04001659 RID: 5721
		private string idBase;

		// Token: 0x0400165A RID: 5722
		private int nextId;

		// Token: 0x0400165B RID: 5723
		private Hashtable typeEntries;

		// Token: 0x0400165C RID: 5724
		private ArrayList referencesToWrite;

		// Token: 0x0400165D RID: 5725
		private Hashtable objectsInUse;

		// Token: 0x0400165E RID: 5726
		private string aliasBase = "q";

		// Token: 0x0400165F RID: 5727
		private bool soap12;

		// Token: 0x04001660 RID: 5728
		private bool escapeName = true;

		// Token: 0x0200032F RID: 815
		internal class TypeEntry
		{
			// Token: 0x04001661 RID: 5729
			internal XmlSerializationWriteCallback callback;

			// Token: 0x04001662 RID: 5730
			internal string typeNs;

			// Token: 0x04001663 RID: 5731
			internal string typeName;

			// Token: 0x04001664 RID: 5732
			internal Type type;
		}
	}
}
