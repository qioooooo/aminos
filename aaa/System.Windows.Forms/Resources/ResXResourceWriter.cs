using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace System.Resources
{
	// Token: 0x0200014C RID: 332
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class ResXResourceWriter : IResourceWriter, IDisposable
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0000D9E3 File Offset: 0x0000C9E3
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0000D9EB File Offset: 0x0000C9EB
		public string BasePath
		{
			get
			{
				return this.basePath;
			}
			set
			{
				this.basePath = value;
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000D9F4 File Offset: 0x0000C9F4
		public ResXResourceWriter(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000DA0E File Offset: 0x0000CA0E
		public ResXResourceWriter(Stream stream)
		{
			this.stream = stream;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000DA28 File Offset: 0x0000CA28
		public ResXResourceWriter(TextWriter textWriter)
		{
			this.textWriter = textWriter;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000DA44 File Offset: 0x0000CA44
		~ResXResourceWriter()
		{
			this.Dispose(false);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000DA74 File Offset: 0x0000CA74
		private void InitializeWriter()
		{
			if (this.xmlTextWriter == null)
			{
				bool flag = false;
				if (this.textWriter != null)
				{
					this.textWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					flag = true;
					this.xmlTextWriter = new XmlTextWriter(this.textWriter);
				}
				else if (this.stream != null)
				{
					this.xmlTextWriter = new XmlTextWriter(this.stream, Encoding.UTF8);
				}
				else
				{
					this.xmlTextWriter = new XmlTextWriter(this.fileName, Encoding.UTF8);
				}
				this.xmlTextWriter.Formatting = Formatting.Indented;
				this.xmlTextWriter.Indentation = 2;
				if (!flag)
				{
					this.xmlTextWriter.WriteStartDocument();
				}
			}
			else
			{
				this.xmlTextWriter.WriteStartDocument();
			}
			this.xmlTextWriter.WriteStartElement("root");
			XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(ResXResourceWriter.ResourceSchema));
			xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
			this.xmlTextWriter.WriteNode(xmlTextReader, true);
			this.xmlTextWriter.WriteStartElement("resheader");
			this.xmlTextWriter.WriteAttributeString("name", "resmimetype");
			this.xmlTextWriter.WriteStartElement("value");
			this.xmlTextWriter.WriteString(ResXResourceWriter.ResMimeType);
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteStartElement("resheader");
			this.xmlTextWriter.WriteAttributeString("name", "version");
			this.xmlTextWriter.WriteStartElement("value");
			this.xmlTextWriter.WriteString(ResXResourceWriter.Version);
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteStartElement("resheader");
			this.xmlTextWriter.WriteAttributeString("name", "reader");
			this.xmlTextWriter.WriteStartElement("value");
			this.xmlTextWriter.WriteString(typeof(ResXResourceReader).AssemblyQualifiedName);
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteStartElement("resheader");
			this.xmlTextWriter.WriteAttributeString("name", "writer");
			this.xmlTextWriter.WriteStartElement("value");
			this.xmlTextWriter.WriteString(typeof(ResXResourceWriter).AssemblyQualifiedName);
			this.xmlTextWriter.WriteEndElement();
			this.xmlTextWriter.WriteEndElement();
			this.initialized = true;
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0000DCDF File Offset: 0x0000CCDF
		private XmlWriter Writer
		{
			get
			{
				if (!this.initialized)
				{
					this.InitializeWriter();
				}
				return this.xmlTextWriter;
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0000DCF5 File Offset: 0x0000CCF5
		public virtual void AddAlias(string aliasName, AssemblyName assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (this.cachedAliases == null)
			{
				this.cachedAliases = new Hashtable();
			}
			this.cachedAliases[assemblyName.FullName] = aliasName;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000DD2A File Offset: 0x0000CD2A
		public void AddMetadata(string name, byte[] value)
		{
			this.AddDataRow("metadata", name, value);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000DD39 File Offset: 0x0000CD39
		public void AddMetadata(string name, string value)
		{
			this.AddDataRow("metadata", name, value);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000DD48 File Offset: 0x0000CD48
		public void AddMetadata(string name, object value)
		{
			this.AddDataRow("metadata", name, value);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000DD57 File Offset: 0x0000CD57
		public void AddResource(string name, byte[] value)
		{
			this.AddDataRow("data", name, value);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000DD66 File Offset: 0x0000CD66
		public void AddResource(string name, object value)
		{
			if (value is ResXDataNode)
			{
				this.AddResource((ResXDataNode)value);
				return;
			}
			this.AddDataRow("data", name, value);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000DD8A File Offset: 0x0000CD8A
		public void AddResource(string name, string value)
		{
			this.AddDataRow("data", name, value);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0000DD9C File Offset: 0x0000CD9C
		public void AddResource(ResXDataNode node)
		{
			ResXDataNode resXDataNode = node.DeepClone();
			ResXFileRef fileRef = resXDataNode.FileRef;
			string text = this.BasePath;
			if (!string.IsNullOrEmpty(text))
			{
				if (!text.EndsWith("\\"))
				{
					text += "\\";
				}
				if (fileRef != null)
				{
					fileRef.MakeFilePathRelative(text);
				}
			}
			DataNodeInfo dataNodeInfo = resXDataNode.GetDataNodeInfo();
			this.AddDataRow("data", dataNodeInfo.Name, dataNodeInfo.ValueData, dataNodeInfo.TypeName, dataNodeInfo.MimeType, dataNodeInfo.Comment);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000DE19 File Offset: 0x0000CE19
		private void AddDataRow(string elementName, string name, byte[] value)
		{
			this.AddDataRow(elementName, name, ResXResourceWriter.ToBase64WrappedString(value), this.TypeNameWithAssembly(typeof(byte[])), null, null);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000DE3C File Offset: 0x0000CE3C
		private void AddDataRow(string elementName, string name, object value)
		{
			if (value is string)
			{
				this.AddDataRow(elementName, name, (string)value);
				return;
			}
			if (value is byte[])
			{
				this.AddDataRow(elementName, name, (byte[])value);
				return;
			}
			if (value is ResXFileRef)
			{
				ResXFileRef resXFileRef = (ResXFileRef)value;
				ResXDataNode resXDataNode = new ResXDataNode(name, resXFileRef);
				if (resXFileRef != null)
				{
					resXFileRef.MakeFilePathRelative(this.BasePath);
				}
				DataNodeInfo dataNodeInfo = resXDataNode.GetDataNodeInfo();
				this.AddDataRow(elementName, dataNodeInfo.Name, dataNodeInfo.ValueData, dataNodeInfo.TypeName, dataNodeInfo.MimeType, dataNodeInfo.Comment);
				return;
			}
			ResXDataNode resXDataNode2 = new ResXDataNode(name, value);
			DataNodeInfo dataNodeInfo2 = resXDataNode2.GetDataNodeInfo();
			this.AddDataRow(elementName, dataNodeInfo2.Name, dataNodeInfo2.ValueData, dataNodeInfo2.TypeName, dataNodeInfo2.MimeType, dataNodeInfo2.Comment);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000DF04 File Offset: 0x0000CF04
		private void AddDataRow(string elementName, string name, string value)
		{
			if (value == null)
			{
				this.AddDataRow(elementName, name, value, typeof(ResXNullRef).AssemblyQualifiedName, null, null);
				return;
			}
			this.AddDataRow(elementName, name, value, null, null, null);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0000DF30 File Offset: 0x0000CF30
		private void AddDataRow(string elementName, string name, string value, string type, string mimeType, string comment)
		{
			if (this.hasBeenSaved)
			{
				throw new InvalidOperationException(SR.GetString("ResXResourceWriterSaved"));
			}
			string text = null;
			if (!string.IsNullOrEmpty(type) && elementName == "data")
			{
				string text2 = this.GetFullName(type);
				if (string.IsNullOrEmpty(text2))
				{
					try
					{
						Type type2 = Type.GetType(type);
						if (type2 == typeof(string))
						{
							type = null;
						}
						else if (type2 != null)
						{
							text2 = this.GetFullName(type2.AssemblyQualifiedName);
							text = this.GetAliasFromName(new AssemblyName(text2));
						}
						goto IL_0091;
					}
					catch
					{
						goto IL_0091;
					}
				}
				text = this.GetAliasFromName(new AssemblyName(this.GetFullName(type)));
			}
			IL_0091:
			this.Writer.WriteStartElement(elementName);
			this.Writer.WriteAttributeString("name", name);
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(type) && elementName == "data")
			{
				string typeName = this.GetTypeName(type);
				string text3 = typeName + ", " + text;
				this.Writer.WriteAttributeString("type", text3);
			}
			else if (type != null)
			{
				this.Writer.WriteAttributeString("type", type);
			}
			if (mimeType != null)
			{
				this.Writer.WriteAttributeString("mimetype", mimeType);
			}
			if ((type == null && mimeType == null) || (type != null && type.StartsWith("System.Char", StringComparison.Ordinal)))
			{
				this.Writer.WriteAttributeString("xml", "space", null, "preserve");
			}
			this.Writer.WriteStartElement("value");
			if (!string.IsNullOrEmpty(value))
			{
				this.Writer.WriteString(value);
			}
			this.Writer.WriteEndElement();
			if (!string.IsNullOrEmpty(comment))
			{
				this.Writer.WriteStartElement("comment");
				this.Writer.WriteString(comment);
				this.Writer.WriteEndElement();
			}
			this.Writer.WriteEndElement();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0000E114 File Offset: 0x0000D114
		private void AddAssemblyRow(string elementName, string alias, string name)
		{
			this.Writer.WriteStartElement(elementName);
			if (!string.IsNullOrEmpty(alias))
			{
				this.Writer.WriteAttributeString("alias", alias);
			}
			if (!string.IsNullOrEmpty(name))
			{
				this.Writer.WriteAttributeString("name", name);
			}
			this.Writer.WriteEndElement();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0000E16C File Offset: 0x0000D16C
		private string GetAliasFromName(AssemblyName assemblyName)
		{
			if (this.cachedAliases == null)
			{
				this.cachedAliases = new Hashtable();
			}
			string text = (string)this.cachedAliases[assemblyName.FullName];
			if (string.IsNullOrEmpty(text))
			{
				text = assemblyName.Name;
				this.AddAlias(text, assemblyName);
				this.AddAssemblyRow("assembly", text, assemblyName.FullName);
			}
			return text;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000E1CD File Offset: 0x0000D1CD
		public void Close()
		{
			this.Dispose();
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000E1D5 File Offset: 0x0000D1D5
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000E1E4 File Offset: 0x0000D1E4
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!this.hasBeenSaved)
				{
					this.Generate();
				}
				if (this.xmlTextWriter != null)
				{
					this.xmlTextWriter.Close();
					this.xmlTextWriter = null;
				}
				if (this.stream != null)
				{
					this.stream.Close();
					this.stream = null;
				}
				if (this.textWriter != null)
				{
					this.textWriter.Close();
					this.textWriter = null;
				}
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0000E250 File Offset: 0x0000D250
		private string GetTypeName(string typeName)
		{
			int num = typeName.IndexOf(",");
			if (num != -1)
			{
				return typeName.Substring(0, num);
			}
			return typeName;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000E278 File Offset: 0x0000D278
		private string GetFullName(string typeName)
		{
			int num = typeName.IndexOf(",");
			if (num == -1)
			{
				return null;
			}
			return typeName.Substring(num + 2);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0000E2A0 File Offset: 0x0000D2A0
		private static string ToBase64WrappedString(byte[] data)
		{
			string text = Convert.ToBase64String(data);
			if (text.Length > 80)
			{
				StringBuilder stringBuilder = new StringBuilder(text.Length + text.Length / 80 * 3);
				int i;
				for (i = 0; i < text.Length - 80; i += 80)
				{
					stringBuilder.Append("\r\n");
					stringBuilder.Append("        ");
					stringBuilder.Append(text, i, 80);
				}
				stringBuilder.Append("\r\n");
				stringBuilder.Append("        ");
				stringBuilder.Append(text, i, text.Length - i);
				stringBuilder.Append("\r\n");
				return stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0000E350 File Offset: 0x0000D350
		private string TypeNameWithAssembly(Type type)
		{
			return type.AssemblyQualifiedName;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0000E365 File Offset: 0x0000D365
		public void Generate()
		{
			if (this.hasBeenSaved)
			{
				throw new InvalidOperationException(SR.GetString("ResXResourceWriterSaved"));
			}
			this.hasBeenSaved = true;
			this.Writer.WriteEndElement();
			this.Writer.Flush();
		}

		// Token: 0x04000F09 RID: 3849
		internal const string TypeStr = "type";

		// Token: 0x04000F0A RID: 3850
		internal const string NameStr = "name";

		// Token: 0x04000F0B RID: 3851
		internal const string DataStr = "data";

		// Token: 0x04000F0C RID: 3852
		internal const string MetadataStr = "metadata";

		// Token: 0x04000F0D RID: 3853
		internal const string MimeTypeStr = "mimetype";

		// Token: 0x04000F0E RID: 3854
		internal const string ValueStr = "value";

		// Token: 0x04000F0F RID: 3855
		internal const string ResHeaderStr = "resheader";

		// Token: 0x04000F10 RID: 3856
		internal const string VersionStr = "version";

		// Token: 0x04000F11 RID: 3857
		internal const string ResMimeTypeStr = "resmimetype";

		// Token: 0x04000F12 RID: 3858
		internal const string ReaderStr = "reader";

		// Token: 0x04000F13 RID: 3859
		internal const string WriterStr = "writer";

		// Token: 0x04000F14 RID: 3860
		internal const string CommentStr = "comment";

		// Token: 0x04000F15 RID: 3861
		internal const string AssemblyStr = "assembly";

		// Token: 0x04000F16 RID: 3862
		internal const string AliasStr = "alias";

		// Token: 0x04000F17 RID: 3863
		private Hashtable cachedAliases;

		// Token: 0x04000F18 RID: 3864
		private static TraceSwitch ResValueProviderSwitch = new TraceSwitch("ResX", "Debug the resource value provider");

		// Token: 0x04000F19 RID: 3865
		internal static readonly string Beta2CompatSerializedObjectMimeType = "text/microsoft-urt/psuedoml-serialized/base64";

		// Token: 0x04000F1A RID: 3866
		internal static readonly string CompatBinSerializedObjectMimeType = "text/microsoft-urt/binary-serialized/base64";

		// Token: 0x04000F1B RID: 3867
		internal static readonly string CompatSoapSerializedObjectMimeType = "text/microsoft-urt/soap-serialized/base64";

		// Token: 0x04000F1C RID: 3868
		public static readonly string BinSerializedObjectMimeType = "application/x-microsoft.net.object.binary.base64";

		// Token: 0x04000F1D RID: 3869
		public static readonly string SoapSerializedObjectMimeType = "application/x-microsoft.net.object.soap.base64";

		// Token: 0x04000F1E RID: 3870
		public static readonly string DefaultSerializedObjectMimeType = ResXResourceWriter.BinSerializedObjectMimeType;

		// Token: 0x04000F1F RID: 3871
		public static readonly string ByteArraySerializedObjectMimeType = "application/x-microsoft.net.object.bytearray.base64";

		// Token: 0x04000F20 RID: 3872
		public static readonly string ResMimeType = "text/microsoft-resx";

		// Token: 0x04000F21 RID: 3873
		public static readonly string Version = "2.0";

		// Token: 0x04000F22 RID: 3874
		public static readonly string ResourceSchema = string.Concat(new string[]
		{
			"\r\n    <!-- \r\n    Microsoft ResX Schema \r\n    \r\n    Version ",
			ResXResourceWriter.Version,
			"\r\n    \r\n    The primary goals of this format is to allow a simple XML format \r\n    that is mostly human readable. The generation and parsing of the \r\n    various data types are done through the TypeConverter classes \r\n    associated with the data types.\r\n    \r\n    Example:\r\n    \r\n    ... ado.net/XML headers & schema ...\r\n    <resheader name=\"resmimetype\">text/microsoft-resx</resheader>\r\n    <resheader name=\"version\">",
			ResXResourceWriter.Version,
			"</resheader>\r\n    <resheader name=\"reader\">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>\r\n    <resheader name=\"writer\">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>\r\n    <data name=\"Name1\"><value>this is my long string</value><comment>this is a comment</comment></data>\r\n    <data name=\"Color1\" type=\"System.Drawing.Color, System.Drawing\">Blue</data>\r\n    <data name=\"Bitmap1\" mimetype=\"",
			ResXResourceWriter.BinSerializedObjectMimeType,
			"\">\r\n        <value>[base64 mime encoded serialized .NET Framework object]</value>\r\n    </data>\r\n    <data name=\"Icon1\" type=\"System.Drawing.Icon, System.Drawing\" mimetype=\"",
			ResXResourceWriter.ByteArraySerializedObjectMimeType,
			"\">\r\n        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>\r\n        <comment>This is a comment</comment>\r\n    </data>\r\n                \r\n    There are any number of \"resheader\" rows that contain simple \r\n    name/value pairs.\r\n    \r\n    Each data row contains a name, and value. The row also contains a \r\n    type or mimetype. Type corresponds to a .NET class that support \r\n    text/value conversion through the TypeConverter architecture. \r\n    Classes that don't support this are serialized and stored with the \r\n    mimetype set.\r\n    \r\n    The mimetype is used for serialized objects, and tells the \r\n    ResXResourceReader how to depersist the object. This is currently not \r\n    extensible. For a given mimetype the value must be set accordingly:\r\n    \r\n    Note - ",
			ResXResourceWriter.BinSerializedObjectMimeType,
			" is the format \r\n    that the ResXResourceWriter will generate, however the reader can \r\n    read any of the formats listed below.\r\n    \r\n    mimetype: ",
			ResXResourceWriter.BinSerializedObjectMimeType,
			"\r\n    value   : The object must be serialized with \r\n            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter\r\n            : and then encoded with base64 encoding.\r\n    \r\n    mimetype: ",
			ResXResourceWriter.SoapSerializedObjectMimeType,
			"\r\n    value   : The object must be serialized with \r\n            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter\r\n            : and then encoded with base64 encoding.\r\n\r\n    mimetype: ",
			ResXResourceWriter.ByteArraySerializedObjectMimeType,
			"\r\n    value   : The object must be serialized into a byte array \r\n            : using a System.ComponentModel.TypeConverter\r\n            : and then encoded with base64 encoding.\r\n    -->\r\n    <xsd:schema id=\"root\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\r\n        <xsd:import namespace=\"http://www.w3.org/XML/1998/namespace\"/>\r\n        <xsd:element name=\"root\" msdata:IsDataSet=\"true\">\r\n            <xsd:complexType>\r\n                <xsd:choice maxOccurs=\"unbounded\">\r\n                    <xsd:element name=\"metadata\">\r\n                        <xsd:complexType>\r\n                            <xsd:sequence>\r\n                            <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\"/>\r\n                            </xsd:sequence>\r\n                            <xsd:attribute name=\"name\" use=\"required\" type=\"xsd:string\"/>\r\n                            <xsd:attribute name=\"type\" type=\"xsd:string\"/>\r\n                            <xsd:attribute name=\"mimetype\" type=\"xsd:string\"/>\r\n                            <xsd:attribute ref=\"xml:space\"/>                            \r\n                        </xsd:complexType>\r\n                    </xsd:element>\r\n                    <xsd:element name=\"assembly\">\r\n                      <xsd:complexType>\r\n                        <xsd:attribute name=\"alias\" type=\"xsd:string\"/>\r\n                        <xsd:attribute name=\"name\" type=\"xsd:string\"/>\r\n                      </xsd:complexType>\r\n                    </xsd:element>\r\n                    <xsd:element name=\"data\">\r\n                        <xsd:complexType>\r\n                            <xsd:sequence>\r\n                                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />\r\n                                <xsd:element name=\"comment\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"2\" />\r\n                            </xsd:sequence>\r\n                            <xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" msdata:Ordinal=\"1\" />\r\n                            <xsd:attribute name=\"type\" type=\"xsd:string\" msdata:Ordinal=\"3\" />\r\n                            <xsd:attribute name=\"mimetype\" type=\"xsd:string\" msdata:Ordinal=\"4\" />\r\n                            <xsd:attribute ref=\"xml:space\"/>\r\n                        </xsd:complexType>\r\n                    </xsd:element>\r\n                    <xsd:element name=\"resheader\">\r\n                        <xsd:complexType>\r\n                            <xsd:sequence>\r\n                                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />\r\n                            </xsd:sequence>\r\n                            <xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" />\r\n                        </xsd:complexType>\r\n                    </xsd:element>\r\n                </xsd:choice>\r\n            </xsd:complexType>\r\n        </xsd:element>\r\n        </xsd:schema>\r\n        "
		});

		// Token: 0x04000F23 RID: 3875
		private IFormatter binaryFormatter = new BinaryFormatter();

		// Token: 0x04000F24 RID: 3876
		private string fileName;

		// Token: 0x04000F25 RID: 3877
		private Stream stream;

		// Token: 0x04000F26 RID: 3878
		private TextWriter textWriter;

		// Token: 0x04000F27 RID: 3879
		private XmlTextWriter xmlTextWriter;

		// Token: 0x04000F28 RID: 3880
		private string basePath;

		// Token: 0x04000F29 RID: 3881
		private bool hasBeenSaved;

		// Token: 0x04000F2A RID: 3882
		private bool initialized;
	}
}
