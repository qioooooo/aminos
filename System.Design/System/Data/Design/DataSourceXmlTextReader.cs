using System;
using System.IO;
using System.Xml;

namespace System.Data.Design
{
	internal class DataSourceXmlTextReader : XmlTextReader
	{
		internal DataSourceXmlTextReader(DesignDataSource dataSource, TextReader textReader)
			: base(textReader)
		{
			this.dataSource = dataSource;
			this.readingDataSource = false;
		}

		internal DataSourceXmlTextReader(DesignDataSource dataSource, Stream stream)
			: base(stream)
		{
			this.dataSource = dataSource;
			this.readingDataSource = false;
		}

		public override bool Read()
		{
			bool flag = base.Read();
			if (flag && !this.readingDataSource && this.NodeType == XmlNodeType.Element && this.LocalName == "DataSource" && this.NamespaceURI == "urn:schemas-microsoft-com:xml-msdatasource")
			{
				this.readingDataSource = true;
				this.dataSource.ReadDataSourceExtraInformation(this);
				flag = !this.EOF;
			}
			return flag;
		}

		private DesignDataSource dataSource;

		private bool readingDataSource;
	}
}
