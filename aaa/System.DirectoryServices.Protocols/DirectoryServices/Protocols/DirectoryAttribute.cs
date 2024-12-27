using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200000A RID: 10
	public class DirectoryAttribute : CollectionBase
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002D84 File Offset: 0x00001D84
		public DirectoryAttribute()
		{
			Utility.CheckOSVersion();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002D9C File Offset: 0x00001D9C
		public DirectoryAttribute(string name, string value)
			: this(name, value)
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002DA6 File Offset: 0x00001DA6
		public DirectoryAttribute(string name, byte[] value)
			: this(name, value)
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002DB0 File Offset: 0x00001DB0
		public DirectoryAttribute(string name, Uri value)
			: this(name, value)
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002DBA File Offset: 0x00001DBA
		internal DirectoryAttribute(string name, object value)
			: this()
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Name = name;
			this.Add(value);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002DF0 File Offset: 0x00001DF0
		public DirectoryAttribute(string name, params object[] values)
			: this()
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			this.Name = name;
			for (int i = 0; i < values.Length; i++)
			{
				this.Add(values[i]);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002E40 File Offset: 0x00001E40
		internal DirectoryAttribute(XmlElement node)
		{
			string text = "@dsml:name";
			string text2 = "@name";
			XmlNamespaceManager dsmlNamespaceManager = NamespaceUtils.GetDsmlNamespaceManager();
			XmlAttribute xmlAttribute = (XmlAttribute)node.SelectSingleNode(text, dsmlNamespaceManager);
			if (xmlAttribute == null)
			{
				xmlAttribute = (XmlAttribute)node.SelectSingleNode(text2, dsmlNamespaceManager);
				if (xmlAttribute == null)
				{
					throw new DsmlInvalidDocumentException(Res.GetString("MissingSearchResultEntryAttributeName"));
				}
				this.attributeName = xmlAttribute.Value;
			}
			else
			{
				this.attributeName = xmlAttribute.Value;
			}
			XmlNodeList xmlNodeList = node.SelectNodes("dsml:value", dsmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlAttribute xmlAttribute2 = (XmlAttribute)xmlNode.SelectSingleNode("@xsi:type", dsmlNamespaceManager);
					if (xmlAttribute2 == null)
					{
						this.Add(xmlNode.InnerText);
					}
					else if (string.Compare(xmlAttribute2.Value, "xsd:string", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.Add(xmlNode.InnerText);
					}
					else if (string.Compare(xmlAttribute2.Value, "xsd:base64Binary", StringComparison.OrdinalIgnoreCase) == 0)
					{
						string innerText = xmlNode.InnerText;
						byte[] array;
						try
						{
							array = Convert.FromBase64String(innerText);
						}
						catch (FormatException)
						{
							throw new DsmlInvalidDocumentException(Res.GetString("BadBase64Value"));
						}
						this.Add(array);
					}
					else if (string.Compare(xmlAttribute2.Value, "xsd:anyURI", StringComparison.OrdinalIgnoreCase) == 0)
					{
						Uri uri = new Uri(xmlNode.InnerText);
						this.Add(uri);
					}
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002FF0 File Offset: 0x00001FF0
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002FF8 File Offset: 0x00001FF8
		public string Name
		{
			get
			{
				return this.attributeName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.attributeName = value;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003010 File Offset: 0x00002010
		public object[] GetValues(Type valuesType)
		{
			if (valuesType == typeof(byte[]))
			{
				int count = base.List.Count;
				byte[][] array = new byte[count][];
				for (int i = 0; i < count; i++)
				{
					if (base.List[i] is string)
					{
						array[i] = DirectoryAttribute.encoder.GetBytes((string)base.List[i]);
					}
					else
					{
						if (!(base.List[i] is byte[]))
						{
							throw new NotSupportedException(Res.GetString("DirectoryAttributeConversion"));
						}
						array[i] = (byte[])base.List[i];
					}
				}
				return array;
			}
			if (valuesType == typeof(string))
			{
				int count2 = base.List.Count;
				string[] array2 = new string[count2];
				for (int j = 0; j < count2; j++)
				{
					if (base.List[j] is string)
					{
						array2[j] = (string)base.List[j];
					}
					else
					{
						if (!(base.List[j] is byte[]))
						{
							throw new NotSupportedException(Res.GetString("DirectoryAttributeConversion"));
						}
						array2[j] = DirectoryAttribute.encoder.GetString((byte[])base.List[j]);
					}
				}
				return array2;
			}
			throw new ArgumentException(Res.GetString("ValidDirectoryAttributeType"), "valuesType");
		}

		// Token: 0x17000006 RID: 6
		public object this[int index]
		{
			get
			{
				if (!this.isSearchResult)
				{
					return base.List[index];
				}
				byte[] array = base.List[index] as byte[];
				if (array != null)
				{
					try
					{
						return DirectoryAttribute.utf8EncoderWithErrorDetection.GetString(array);
					}
					catch (ArgumentException)
					{
						return base.List[index];
					}
				}
				return base.List[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value is string || value is byte[] || value is Uri)
				{
					base.List[index] = value;
					return;
				}
				throw new ArgumentException(Res.GetString("ValidValueType"), "value");
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003245 File Offset: 0x00002245
		public int Add(byte[] value)
		{
			return this.Add(value);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000324E File Offset: 0x0000224E
		public int Add(string value)
		{
			return this.Add(value);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003257 File Offset: 0x00002257
		public int Add(Uri value)
		{
			return this.Add(value);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003260 File Offset: 0x00002260
		internal int Add(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is string) && !(value is byte[]) && !(value is Uri))
			{
				throw new ArgumentException(Res.GetString("ValidValueType"), "value");
			}
			return base.List.Add(value);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000032B4 File Offset: 0x000022B4
		public void AddRange(object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (!(values is string[]) && !(values is byte[][]) && !(values is Uri[]))
			{
				throw new ArgumentException(Res.GetString("ValidValuesType"), "values");
			}
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] == null)
				{
					throw new ArgumentException(Res.GetString("NullValueArray"), "values");
				}
			}
			base.InnerList.AddRange(values);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003330 File Offset: 0x00002330
		public bool Contains(object value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000333E File Offset: 0x0000233E
		public void CopyTo(object[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000334D File Offset: 0x0000234D
		public int IndexOf(object value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000335B File Offset: 0x0000235B
		public void Insert(int index, byte[] value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003365 File Offset: 0x00002365
		public void Insert(int index, string value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000336F File Offset: 0x0000236F
		public void Insert(int index, Uri value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003379 File Offset: 0x00002379
		private void Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			base.List.Insert(index, value);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003396 File Offset: 0x00002396
		public void Remove(object value)
		{
			base.List.Remove(value);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000033A4 File Offset: 0x000023A4
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is string) && !(value is byte[]) && !(value is Uri))
			{
				throw new ArgumentException(Res.GetString("ValidValueType"), "value");
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000033E4 File Offset: 0x000023E4
		internal void ToXmlNodeCommon(XmlElement elemBase)
		{
			XmlDocument ownerDocument = elemBase.OwnerDocument;
			XmlAttribute xmlAttribute = ownerDocument.CreateAttribute("name", null);
			xmlAttribute.InnerText = this.Name;
			elemBase.Attributes.Append(xmlAttribute);
			if (base.Count != 0)
			{
				foreach (object obj in base.InnerList)
				{
					XmlElement xmlElement = ownerDocument.CreateElement("value", "urn:oasis:names:tc:DSML:2:0:core");
					if (obj is byte[])
					{
						xmlElement.InnerText = Convert.ToBase64String((byte[])obj);
						XmlAttribute xmlAttribute2 = ownerDocument.CreateAttribute("xsi:type", "http://www.w3.org/2001/XMLSchema-instance");
						xmlAttribute2.InnerText = "xsd:base64Binary";
						xmlElement.Attributes.Append(xmlAttribute2);
					}
					else if (obj is Uri)
					{
						xmlElement.InnerText = obj.ToString();
						XmlAttribute xmlAttribute3 = ownerDocument.CreateAttribute("xsi:type", "http://www.w3.org/2001/XMLSchema-instance");
						xmlAttribute3.InnerText = "xsd:anyURI";
						xmlElement.Attributes.Append(xmlAttribute3);
					}
					else
					{
						xmlElement.InnerText = obj.ToString();
						if (xmlElement.InnerText.StartsWith(" ", StringComparison.Ordinal) || xmlElement.InnerText.EndsWith(" ", StringComparison.Ordinal))
						{
							XmlAttribute xmlAttribute4 = ownerDocument.CreateAttribute("xml:space");
							xmlAttribute4.InnerText = "preserve";
							xmlElement.Attributes.Append(xmlAttribute4);
						}
					}
					elemBase.AppendChild(xmlElement);
				}
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003580 File Offset: 0x00002580
		internal XmlElement ToXmlNode(XmlDocument doc, string elementName)
		{
			XmlElement xmlElement = doc.CreateElement(elementName, "urn:oasis:names:tc:DSML:2:0:core");
			this.ToXmlNodeCommon(xmlElement);
			return xmlElement;
		}

		// Token: 0x040000BA RID: 186
		private string attributeName = "";

		// Token: 0x040000BB RID: 187
		internal bool isSearchResult;

		// Token: 0x040000BC RID: 188
		private static UTF8Encoding utf8EncoderWithErrorDetection = new UTF8Encoding(false, true);

		// Token: 0x040000BD RID: 189
		private static UTF8Encoding encoder = new UTF8Encoding();
	}
}
