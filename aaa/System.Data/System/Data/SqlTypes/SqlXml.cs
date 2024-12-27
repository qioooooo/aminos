using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000378 RID: 888
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public sealed class SqlXml : INullable, IXmlSerializable
	{
		// Token: 0x06002F54 RID: 12116 RVA: 0x002B00A8 File Offset: 0x002AF4A8
		public SqlXml()
		{
			this.SetNull();
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x002B00C4 File Offset: 0x002AF4C4
		private SqlXml(bool fNull)
		{
			this.SetNull();
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x002B00E0 File Offset: 0x002AF4E0
		public SqlXml(XmlReader value)
		{
			if (value == null)
			{
				this.SetNull();
				return;
			}
			this.m_fNotNull = true;
			this.firstCreateReader = true;
			this.m_stream = this.CreateMemoryStreamFromXmlReader(value);
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x002B0118 File Offset: 0x002AF518
		public SqlXml(Stream value)
		{
			if (value == null)
			{
				this.SetNull();
				return;
			}
			this.firstCreateReader = true;
			this.m_fNotNull = true;
			this.m_stream = value;
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x002B014C File Offset: 0x002AF54C
		public XmlReader CreateReader()
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
			SqlXmlStreamWrapper sqlXmlStreamWrapper = new SqlXmlStreamWrapper(this.m_stream);
			if ((!this.firstCreateReader || sqlXmlStreamWrapper.CanSeek) && sqlXmlStreamWrapper.Position != 0L)
			{
				sqlXmlStreamWrapper.Seek(0L, SeekOrigin.Begin);
			}
			XmlReader xmlReader = null;
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
			if (this.createSqlReaderMethodInfo == null)
			{
				this.createSqlReaderMethodInfo = typeof(XmlReader).GetMethod("CreateSqlReader", BindingFlags.Static | BindingFlags.NonPublic);
			}
			object[] array = new object[3];
			array[0] = sqlXmlStreamWrapper;
			array[1] = xmlReaderSettings;
			object[] array2 = array;
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
			try
			{
				xmlReader = (XmlReader)this.createSqlReaderMethodInfo.Invoke(null, array2);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this.firstCreateReader = false;
			return xmlReader;
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002F59 RID: 12121 RVA: 0x002B0228 File Offset: 0x002AF628
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002F5A RID: 12122 RVA: 0x002B0240 File Offset: 0x002AF640
		public string Value
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				StringWriter stringWriter = new StringWriter(null);
				XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
				{
					CloseOutput = false,
					ConformanceLevel = ConformanceLevel.Fragment
				});
				XmlReader xmlReader = this.CreateReader();
				if (xmlReader.ReadState == ReadState.Initial)
				{
					xmlReader.Read();
				}
				while (!xmlReader.EOF)
				{
					xmlWriter.WriteNode(xmlReader, true);
				}
				xmlWriter.Flush();
				return stringWriter.ToString();
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x002B02B4 File Offset: 0x002AF6B4
		public static SqlXml Null
		{
			get
			{
				return new SqlXml(true);
			}
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x002B02C8 File Offset: 0x002AF6C8
		private void SetNull()
		{
			this.m_fNotNull = false;
			this.m_stream = null;
			this.firstCreateReader = true;
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x002B02EC File Offset: 0x002AF6EC
		private Stream CreateMemoryStreamFromXmlReader(XmlReader reader)
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.CloseOutput = false;
			xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
			xmlWriterSettings.Encoding = Encoding.GetEncoding("utf-16");
			xmlWriterSettings.OmitXmlDeclaration = true;
			MemoryStream memoryStream = new MemoryStream();
			XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
			if (reader.ReadState == ReadState.Closed)
			{
				throw new InvalidOperationException(SQLResource.ClosedXmlReaderMessage);
			}
			if (reader.ReadState == ReadState.Initial)
			{
				reader.Read();
			}
			while (!reader.EOF)
			{
				xmlWriter.WriteNode(reader, true);
			}
			xmlWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream;
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x002B0378 File Offset: 0x002AF778
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x002B0388 File Offset: 0x002AF788
		void IXmlSerializable.ReadXml(XmlReader r)
		{
			string attribute = r.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.SetNull();
				return;
			}
			this.m_fNotNull = true;
			this.firstCreateReader = true;
			this.m_stream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(this.m_stream);
			streamWriter.Write(r.ReadInnerXml());
			streamWriter.Flush();
			if (this.m_stream.CanSeek)
			{
				this.m_stream.Seek(0L, SeekOrigin.Begin);
			}
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x002B040C File Offset: 0x002AF80C
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
			}
			else
			{
				SqlXmlStreamWrapper sqlXmlStreamWrapper = new SqlXmlStreamWrapper(this.m_stream);
				if (sqlXmlStreamWrapper.CanSeek && sqlXmlStreamWrapper.Position != 0L)
				{
					sqlXmlStreamWrapper.Seek(0L, SeekOrigin.Begin);
				}
				StreamReader streamReader = new StreamReader(sqlXmlStreamWrapper);
				char[] array = new char[4096];
				for (int i = streamReader.Read(array, 0, 4096); i > 0; i = streamReader.Read(array, 0, 4096))
				{
					writer.WriteRaw(array, 0, i);
				}
			}
			writer.Flush();
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x002B04A8 File Offset: 0x002AF8A8
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001D57 RID: 7511
		private bool m_fNotNull;

		// Token: 0x04001D58 RID: 7512
		private Stream m_stream;

		// Token: 0x04001D59 RID: 7513
		private bool firstCreateReader;

		// Token: 0x04001D5A RID: 7514
		private MethodInfo createSqlReaderMethodInfo;
	}
}
