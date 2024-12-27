using System;
using System.IO;
using System.Xml;

namespace System.Data.Design
{
	// Token: 0x02000083 RID: 131
	internal class DataSourceXmlTextReader : XmlTextReader
	{
		// Token: 0x0600054A RID: 1354 RVA: 0x00009D60 File Offset: 0x00008D60
		internal DataSourceXmlTextReader(DesignDataSource dataSource, TextReader textReader)
			: base(textReader)
		{
			this.dataSource = dataSource;
			this.readingDataSource = false;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00009D77 File Offset: 0x00008D77
		internal DataSourceXmlTextReader(DesignDataSource dataSource, Stream stream)
			: base(stream)
		{
			this.dataSource = dataSource;
			this.readingDataSource = false;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00009D90 File Offset: 0x00008D90
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

		// Token: 0x04000AD5 RID: 2773
		private DesignDataSource dataSource;

		// Token: 0x04000AD6 RID: 2774
		private bool readingDataSource;
	}
}
