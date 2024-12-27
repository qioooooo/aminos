using System;
using System.Collections;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl
{
	// Token: 0x0200000D RID: 13
	internal class XmlILCommand : XmlCommand
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002F05 File Offset: 0x00001F05
		public XmlILCommand(ExecuteDelegate delExec, XmlQueryStaticData staticData)
		{
			this.delExec = delExec;
			this.staticData = staticData;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002F1B File Offset: 0x00001F1B
		public ExecuteDelegate ExecuteDelegate
		{
			get
			{
				return this.delExec;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002F23 File Offset: 0x00001F23
		public XmlQueryStaticData StaticData
		{
			get
			{
				return this.staticData;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002F2B File Offset: 0x00001F2B
		public override void Execute(IXPathNavigable contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			if (contextDocument != null)
			{
				this.Execute(contextDocument.CreateNavigator(), dataSources, argumentList, results, false);
				return;
			}
			this.Execute(null, dataSources, argumentList, results, false);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002F5D File Offset: 0x00001F5D
		public override void Execute(IXPathNavigable contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, TextWriter results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			this.Execute(contextDocument, dataSources, argumentList, XmlWriter.Create(results, this.staticData.DefaultWriterSettings));
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002F89 File Offset: 0x00001F89
		public override void Execute(IXPathNavigable contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, Stream results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			this.Execute(contextDocument, dataSources, argumentList, XmlWriter.Create(results, this.staticData.DefaultWriterSettings));
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002FB5 File Offset: 0x00001FB5
		public void Execute(string contextDocumentUri, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			this.Execute(contextDocumentUri, dataSources, argumentList, results, false);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002FD4 File Offset: 0x00001FD4
		public IList Evaluate(string contextDocumentUri, XmlResolver dataSources, XsltArgumentList argumentList)
		{
			XmlCachedSequenceWriter xmlCachedSequenceWriter = new XmlCachedSequenceWriter();
			this.Execute(contextDocumentUri, dataSources, argumentList, xmlCachedSequenceWriter);
			return xmlCachedSequenceWriter.ResultSequence;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002FF7 File Offset: 0x00001FF7
		public override void Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			this.Execute(contextDocument, dataSources, argumentList, results, false);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003014 File Offset: 0x00002014
		public override void Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, TextWriter results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			this.Execute(contextDocument, dataSources, argumentList, XmlWriter.Create(results, this.staticData.DefaultWriterSettings), true);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003041 File Offset: 0x00002041
		public override void Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, Stream results)
		{
			if (results == null)
			{
				throw new ArgumentNullException("results");
			}
			this.Execute(contextDocument, dataSources, argumentList, XmlWriter.Create(results, this.staticData.DefaultWriterSettings), true);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003070 File Offset: 0x00002070
		public override IList Evaluate(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList)
		{
			XmlCachedSequenceWriter xmlCachedSequenceWriter = new XmlCachedSequenceWriter();
			this.Execute(contextDocument, dataSources, argumentList, xmlCachedSequenceWriter);
			return xmlCachedSequenceWriter.ResultSequence;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003094 File Offset: 0x00002094
		private void Execute(object defaultDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter writer, bool closeWriter)
		{
			try
			{
				XmlWellFormedWriter xmlWellFormedWriter = writer as XmlWellFormedWriter;
				if (xmlWellFormedWriter != null && xmlWellFormedWriter.WriteState == WriteState.Start && xmlWellFormedWriter.Settings.ConformanceLevel != ConformanceLevel.Document)
				{
					this.Execute(defaultDocument, dataSources, argumentList, new XmlMergeSequenceWriter(xmlWellFormedWriter.RawWriter));
				}
				else
				{
					this.Execute(defaultDocument, dataSources, argumentList, new XmlMergeSequenceWriter(new XmlRawWriterWrapper(writer)));
				}
			}
			finally
			{
				if (closeWriter)
				{
					writer.Close();
				}
				else
				{
					writer.Flush();
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003114 File Offset: 0x00002114
		private void Execute(object defaultDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlSequenceWriter results)
		{
			if (dataSources == null)
			{
				dataSources = XmlNullResolver.Singleton;
			}
			this.delExec(new XmlQueryRuntime(this.staticData, defaultDocument, dataSources, argumentList, results));
		}

		// Token: 0x040000C3 RID: 195
		private ExecuteDelegate delExec;

		// Token: 0x040000C4 RID: 196
		private XmlQueryStaticData staticData;
	}
}
