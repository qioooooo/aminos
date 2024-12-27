using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Xml;

namespace System.Resources
{
	// Token: 0x02000148 RID: 328
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class ResXResourceReader : IResourceReader, IEnumerable, IDisposable
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x0000CBAC File Offset: 0x0000BBAC
		private ResXResourceReader(ITypeResolutionService typeResolver)
		{
			this.typeResolver = typeResolver;
			this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0000CBC6 File Offset: 0x0000BBC6
		private ResXResourceReader(AssemblyName[] assemblyNames)
		{
			this.assemblyNames = assemblyNames;
			this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0000CBE0 File Offset: 0x0000BBE0
		public ResXResourceReader(string fileName)
			: this(fileName, null, null)
		{
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0000CBEB File Offset: 0x0000BBEB
		public ResXResourceReader(string fileName, ITypeResolutionService typeResolver)
			: this(fileName, typeResolver, null)
		{
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000CBF6 File Offset: 0x0000BBF6
		internal ResXResourceReader(string fileName, ITypeResolutionService typeResolver, IAliasResolver aliasResolver)
		{
			this.fileName = fileName;
			this.typeResolver = typeResolver;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000CC26 File Offset: 0x0000BC26
		public ResXResourceReader(TextReader reader)
			: this(reader, null, null)
		{
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000CC31 File Offset: 0x0000BC31
		public ResXResourceReader(TextReader reader, ITypeResolutionService typeResolver)
			: this(reader, typeResolver, null)
		{
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000CC3C File Offset: 0x0000BC3C
		internal ResXResourceReader(TextReader reader, ITypeResolutionService typeResolver, IAliasResolver aliasResolver)
		{
			this.reader = reader;
			this.typeResolver = typeResolver;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0000CC6C File Offset: 0x0000BC6C
		public ResXResourceReader(Stream stream)
			: this(stream, null, null)
		{
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0000CC77 File Offset: 0x0000BC77
		public ResXResourceReader(Stream stream, ITypeResolutionService typeResolver)
			: this(stream, typeResolver, null)
		{
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000CC82 File Offset: 0x0000BC82
		internal ResXResourceReader(Stream stream, ITypeResolutionService typeResolver, IAliasResolver aliasResolver)
		{
			this.stream = stream;
			this.typeResolver = typeResolver;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0000CCB2 File Offset: 0x0000BCB2
		public ResXResourceReader(Stream stream, AssemblyName[] assemblyNames)
			: this(stream, assemblyNames, null)
		{
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0000CCBD File Offset: 0x0000BCBD
		internal ResXResourceReader(Stream stream, AssemblyName[] assemblyNames, IAliasResolver aliasResolver)
		{
			this.stream = stream;
			this.assemblyNames = assemblyNames;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0000CCED File Offset: 0x0000BCED
		public ResXResourceReader(TextReader reader, AssemblyName[] assemblyNames)
			: this(reader, assemblyNames, null)
		{
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000CCF8 File Offset: 0x0000BCF8
		internal ResXResourceReader(TextReader reader, AssemblyName[] assemblyNames, IAliasResolver aliasResolver)
		{
			this.reader = reader;
			this.assemblyNames = assemblyNames;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000CD28 File Offset: 0x0000BD28
		public ResXResourceReader(string fileName, AssemblyName[] assemblyNames)
			: this(fileName, assemblyNames, null)
		{
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0000CD33 File Offset: 0x0000BD33
		internal ResXResourceReader(string fileName, AssemblyName[] assemblyNames, IAliasResolver aliasResolver)
		{
			this.fileName = fileName;
			this.assemblyNames = assemblyNames;
			this.aliasResolver = aliasResolver;
			if (this.aliasResolver == null)
			{
				this.aliasResolver = new ResXResourceReader.ReaderAliasResolver();
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000CD64 File Offset: 0x0000BD64
		~ResXResourceReader()
		{
			this.Dispose(false);
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0000CD94 File Offset: 0x0000BD94
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x0000CD9C File Offset: 0x0000BD9C
		public string BasePath
		{
			get
			{
				return this.basePath;
			}
			set
			{
				if (this.isReaderDirty)
				{
					throw new InvalidOperationException(SR.GetString("InvalidResXBasePathOperation"));
				}
				this.basePath = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0000CDBD File Offset: 0x0000BDBD
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x0000CDC5 File Offset: 0x0000BDC5
		public bool UseResXDataNodes
		{
			get
			{
				return this.useResXDataNodes;
			}
			set
			{
				if (this.isReaderDirty)
				{
					throw new InvalidOperationException(SR.GetString("InvalidResXBasePathOperation"));
				}
				this.useResXDataNodes = value;
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000CDE6 File Offset: 0x0000BDE6
		public void Close()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0000CDEE File Offset: 0x0000BDEE
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose(true);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000CE00 File Offset: 0x0000BE00
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.fileName != null && this.stream != null)
				{
					this.stream.Close();
					this.stream = null;
				}
				if (this.reader != null)
				{
					this.reader.Close();
					this.reader = null;
				}
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000CE4C File Offset: 0x0000BE4C
		private void SetupNameTable(XmlReader reader)
		{
			reader.NameTable.Add("type");
			reader.NameTable.Add("name");
			reader.NameTable.Add("data");
			reader.NameTable.Add("metadata");
			reader.NameTable.Add("mimetype");
			reader.NameTable.Add("value");
			reader.NameTable.Add("resheader");
			reader.NameTable.Add("version");
			reader.NameTable.Add("resmimetype");
			reader.NameTable.Add("reader");
			reader.NameTable.Add("writer");
			reader.NameTable.Add(ResXResourceWriter.BinSerializedObjectMimeType);
			reader.NameTable.Add(ResXResourceWriter.SoapSerializedObjectMimeType);
			reader.NameTable.Add("assembly");
			reader.NameTable.Add("alias");
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000CF58 File Offset: 0x0000BF58
		private void EnsureResData()
		{
			if (this.resData == null)
			{
				this.resData = new ListDictionary();
				this.resMetadata = new ListDictionary();
				XmlTextReader xmlTextReader = null;
				try
				{
					if (this.fileContents != null)
					{
						xmlTextReader = new XmlTextReader(new StringReader(this.fileContents));
					}
					else if (this.reader != null)
					{
						xmlTextReader = new XmlTextReader(this.reader);
					}
					else if (this.fileName != null || this.stream != null)
					{
						if (this.stream == null)
						{
							this.stream = new FileStream(this.fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
						}
						xmlTextReader = new XmlTextReader(this.stream);
					}
					this.SetupNameTable(xmlTextReader);
					xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
					this.ParseXml(xmlTextReader);
				}
				finally
				{
					if (this.fileName != null && this.stream != null)
					{
						this.stream.Close();
						this.stream = null;
					}
				}
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000D03C File Offset: 0x0000C03C
		public static ResXResourceReader FromFileContents(string fileContents)
		{
			return ResXResourceReader.FromFileContents(fileContents, null);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000D048 File Offset: 0x0000C048
		public static ResXResourceReader FromFileContents(string fileContents, ITypeResolutionService typeResolver)
		{
			return new ResXResourceReader(typeResolver)
			{
				fileContents = fileContents
			};
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0000D064 File Offset: 0x0000C064
		public static ResXResourceReader FromFileContents(string fileContents, AssemblyName[] assemblyNames)
		{
			return new ResXResourceReader(assemblyNames)
			{
				fileContents = fileContents
			};
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000D080 File Offset: 0x0000C080
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000D088 File Offset: 0x0000C088
		public IDictionaryEnumerator GetEnumerator()
		{
			this.isReaderDirty = true;
			this.EnsureResData();
			return this.resData.GetEnumerator();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000D0A2 File Offset: 0x0000C0A2
		public IDictionaryEnumerator GetMetadataEnumerator()
		{
			this.EnsureResData();
			return this.resMetadata.GetEnumerator();
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000D0B8 File Offset: 0x0000C0B8
		private Point GetPosition(XmlReader reader)
		{
			Point point = new Point(0, 0);
			IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
			if (xmlLineInfo != null)
			{
				point.Y = xmlLineInfo.LineNumber;
				point.X = xmlLineInfo.LinePosition;
			}
			return point;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000D0F4 File Offset: 0x0000C0F4
		private void ParseXml(XmlTextReader reader)
		{
			bool flag = false;
			try
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						string localName = reader.LocalName;
						if (reader.LocalName.Equals("assembly"))
						{
							this.ParseAssemblyNode(reader, false);
						}
						else if (reader.LocalName.Equals("data"))
						{
							this.ParseDataNode(reader, false);
						}
						else if (reader.LocalName.Equals("resheader"))
						{
							this.ParseResHeaderNode(reader);
						}
						else if (reader.LocalName.Equals("metadata"))
						{
							this.ParseDataNode(reader, true);
						}
					}
				}
				flag = true;
			}
			catch (SerializationException ex)
			{
				Point position = this.GetPosition(reader);
				string @string = SR.GetString("SerializationException", new object[]
				{
					reader["type"],
					position.Y,
					position.X,
					ex.Message
				});
				XmlException ex2 = new XmlException(@string, ex, position.Y, position.X);
				SerializationException ex3 = new SerializationException(@string, ex2);
				throw ex3;
			}
			catch (TargetInvocationException ex4)
			{
				Point position2 = this.GetPosition(reader);
				string string2 = SR.GetString("InvocationException", new object[]
				{
					reader["type"],
					position2.Y,
					position2.X,
					ex4.InnerException.Message
				});
				XmlException ex5 = new XmlException(string2, ex4.InnerException, position2.Y, position2.X);
				TargetInvocationException ex6 = new TargetInvocationException(string2, ex5);
				throw ex6;
			}
			catch (XmlException ex7)
			{
				throw new ArgumentException(SR.GetString("InvalidResXFile", new object[] { ex7.Message }), ex7);
			}
			catch (Exception ex8)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex8))
				{
					throw;
				}
				Point position3 = this.GetPosition(reader);
				XmlException ex9 = new XmlException(ex8.Message, ex8, position3.Y, position3.X);
				throw new ArgumentException(SR.GetString("InvalidResXFile", new object[] { ex9.Message }), ex9);
			}
			finally
			{
				if (!flag)
				{
					this.resData = null;
					this.resMetadata = null;
				}
			}
			bool flag2 = false;
			if (object.Equals(this.resHeaderMimeType, ResXResourceWriter.ResMimeType))
			{
				Type typeFromHandle = typeof(ResXResourceReader);
				Type typeFromHandle2 = typeof(ResXResourceWriter);
				string text = this.resHeaderReaderType;
				string text2 = this.resHeaderWriterType;
				if (text != null && text.IndexOf(',') != -1)
				{
					text = text.Split(new char[] { ',' })[0].Trim();
				}
				if (text2 != null && text2.IndexOf(',') != -1)
				{
					text2 = text2.Split(new char[] { ',' })[0].Trim();
				}
				if (text != null && text2 != null && text.Equals(typeFromHandle.FullName) && text2.Equals(typeFromHandle2.FullName))
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				this.resData = null;
				this.resMetadata = null;
				throw new ArgumentException(SR.GetString("InvalidResXFileReaderWriterTypes"));
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000D49C File Offset: 0x0000C49C
		private void ParseResHeaderNode(XmlReader reader)
		{
			string text = reader["name"];
			if (text != null)
			{
				reader.ReadStartElement();
				string text2;
				if (object.Equals(text, "version"))
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						this.resHeaderVersion = reader.ReadElementString();
						return;
					}
					this.resHeaderVersion = reader.Value.Trim();
					return;
				}
				else if (object.Equals(text, "resmimetype"))
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						this.resHeaderMimeType = reader.ReadElementString();
						return;
					}
					this.resHeaderMimeType = reader.Value.Trim();
					return;
				}
				else if (object.Equals(text, "reader"))
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						this.resHeaderReaderType = reader.ReadElementString();
						return;
					}
					this.resHeaderReaderType = reader.Value.Trim();
					return;
				}
				else if (object.Equals(text, "writer"))
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						this.resHeaderWriterType = reader.ReadElementString();
						return;
					}
					this.resHeaderWriterType = reader.Value.Trim();
					return;
				}
				else if ((text2 = text.ToLower(CultureInfo.InvariantCulture)) != null)
				{
					if (!(text2 == "version"))
					{
						if (!(text2 == "resmimetype"))
						{
							if (!(text2 == "reader"))
							{
								if (!(text2 == "writer"))
								{
									return;
								}
								if (reader.NodeType == XmlNodeType.Element)
								{
									this.resHeaderWriterType = reader.ReadElementString();
									return;
								}
								this.resHeaderWriterType = reader.Value.Trim();
							}
							else
							{
								if (reader.NodeType == XmlNodeType.Element)
								{
									this.resHeaderReaderType = reader.ReadElementString();
									return;
								}
								this.resHeaderReaderType = reader.Value.Trim();
								return;
							}
						}
						else
						{
							if (reader.NodeType == XmlNodeType.Element)
							{
								this.resHeaderMimeType = reader.ReadElementString();
								return;
							}
							this.resHeaderMimeType = reader.Value.Trim();
							return;
						}
					}
					else
					{
						if (reader.NodeType == XmlNodeType.Element)
						{
							this.resHeaderVersion = reader.ReadElementString();
							return;
						}
						this.resHeaderVersion = reader.Value.Trim();
						return;
					}
				}
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000D67C File Offset: 0x0000C67C
		private void ParseAssemblyNode(XmlReader reader, bool isMetaData)
		{
			string text = reader["alias"];
			string text2 = reader["name"];
			AssemblyName assemblyName = new AssemblyName(text2);
			if (string.IsNullOrEmpty(text))
			{
				text = assemblyName.Name;
			}
			this.aliasResolver.PushAlias(text, assemblyName);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000D6C4 File Offset: 0x0000C6C4
		private void ParseDataNode(XmlTextReader reader, bool isMetaData)
		{
			DataNodeInfo dataNodeInfo = new DataNodeInfo();
			dataNodeInfo.Name = reader["name"];
			string text = reader["type"];
			string text2 = null;
			AssemblyName assemblyName = null;
			if (!string.IsNullOrEmpty(text))
			{
				text2 = this.GetAliasFromTypeName(text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				assemblyName = this.aliasResolver.ResolveAlias(text2);
			}
			if (assemblyName != null)
			{
				dataNodeInfo.TypeName = this.GetTypeFromTypeName(text) + ", " + assemblyName.FullName;
			}
			else
			{
				dataNodeInfo.TypeName = reader["type"];
			}
			dataNodeInfo.MimeType = reader["mimetype"];
			bool flag = false;
			dataNodeInfo.ReaderPosition = this.GetPosition(reader);
			while (!flag && reader.Read())
			{
				if (reader.NodeType == XmlNodeType.EndElement && (reader.LocalName.Equals("data") || reader.LocalName.Equals("metadata")))
				{
					flag = true;
				}
				else if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.Name.Equals("value"))
					{
						WhitespaceHandling whitespaceHandling = reader.WhitespaceHandling;
						try
						{
							reader.WhitespaceHandling = WhitespaceHandling.Significant;
							dataNodeInfo.ValueData = reader.ReadString();
							continue;
						}
						finally
						{
							reader.WhitespaceHandling = whitespaceHandling;
						}
					}
					if (reader.Name.Equals("comment"))
					{
						dataNodeInfo.Comment = reader.ReadString();
					}
				}
				else
				{
					dataNodeInfo.ValueData = reader.Value.Trim();
				}
			}
			if (dataNodeInfo.Name == null)
			{
				throw new ArgumentException(SR.GetString("InvalidResXResourceNoName", new object[] { dataNodeInfo.ValueData }));
			}
			ResXDataNode resXDataNode = new ResXDataNode(dataNodeInfo, this.BasePath);
			if (this.UseResXDataNodes)
			{
				this.resData[dataNodeInfo.Name] = resXDataNode;
				return;
			}
			IDictionary dictionary = (isMetaData ? this.resMetadata : this.resData);
			if (this.assemblyNames == null)
			{
				dictionary[dataNodeInfo.Name] = resXDataNode.GetValue(this.typeResolver);
				return;
			}
			dictionary[dataNodeInfo.Name] = resXDataNode.GetValue(this.assemblyNames);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000D8E0 File Offset: 0x0000C8E0
		private string GetAliasFromTypeName(string typeName)
		{
			int num = typeName.IndexOf(",");
			return typeName.Substring(num + 2);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000D904 File Offset: 0x0000C904
		private string GetTypeFromTypeName(string typeName)
		{
			int num = typeName.IndexOf(",");
			return typeName.Substring(0, num);
		}

		// Token: 0x04000EF8 RID: 3832
		private string fileName;

		// Token: 0x04000EF9 RID: 3833
		private TextReader reader;

		// Token: 0x04000EFA RID: 3834
		private Stream stream;

		// Token: 0x04000EFB RID: 3835
		private string fileContents;

		// Token: 0x04000EFC RID: 3836
		private AssemblyName[] assemblyNames;

		// Token: 0x04000EFD RID: 3837
		private string basePath;

		// Token: 0x04000EFE RID: 3838
		private bool isReaderDirty;

		// Token: 0x04000EFF RID: 3839
		private ITypeResolutionService typeResolver;

		// Token: 0x04000F00 RID: 3840
		private IAliasResolver aliasResolver;

		// Token: 0x04000F01 RID: 3841
		private ListDictionary resData;

		// Token: 0x04000F02 RID: 3842
		private ListDictionary resMetadata;

		// Token: 0x04000F03 RID: 3843
		private string resHeaderVersion;

		// Token: 0x04000F04 RID: 3844
		private string resHeaderMimeType;

		// Token: 0x04000F05 RID: 3845
		private string resHeaderReaderType;

		// Token: 0x04000F06 RID: 3846
		private string resHeaderWriterType;

		// Token: 0x04000F07 RID: 3847
		private bool useResXDataNodes;

		// Token: 0x0200014A RID: 330
		private sealed class ReaderAliasResolver : IAliasResolver
		{
			// Token: 0x06000529 RID: 1321 RVA: 0x0000D925 File Offset: 0x0000C925
			internal ReaderAliasResolver()
			{
				this.cachedAliases = new Hashtable();
			}

			// Token: 0x0600052A RID: 1322 RVA: 0x0000D938 File Offset: 0x0000C938
			public AssemblyName ResolveAlias(string alias)
			{
				AssemblyName assemblyName = null;
				if (this.cachedAliases != null)
				{
					assemblyName = (AssemblyName)this.cachedAliases[alias];
				}
				return assemblyName;
			}

			// Token: 0x0600052B RID: 1323 RVA: 0x0000D962 File Offset: 0x0000C962
			public void PushAlias(string alias, AssemblyName name)
			{
				if (this.cachedAliases != null && !string.IsNullOrEmpty(alias))
				{
					this.cachedAliases[alias] = name;
				}
			}

			// Token: 0x04000F08 RID: 3848
			private Hashtable cachedAliases;
		}
	}
}
