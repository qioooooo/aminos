using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000BC RID: 188
	internal sealed class XmlRawWriterWrapper : XmlRawWriter
	{
		// Token: 0x06000923 RID: 2339 RVA: 0x0002BC72 File Offset: 0x0002AC72
		public XmlRawWriterWrapper(XmlWriter writer)
		{
			this.wrapped = writer;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0002BC81 File Offset: 0x0002AC81
		public override XmlWriterSettings Settings
		{
			get
			{
				return this.wrapped.Settings;
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002BC8E File Offset: 0x0002AC8E
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this.wrapped.WriteDocType(name, pubid, sysid, subset);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002BCA0 File Offset: 0x0002ACA0
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.wrapped.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0002BCB0 File Offset: 0x0002ACB0
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.wrapped.WriteStartAttribute(prefix, localName, ns);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002BCC0 File Offset: 0x0002ACC0
		public override void WriteEndAttribute()
		{
			this.wrapped.WriteEndAttribute();
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0002BCCD File Offset: 0x0002ACCD
		public override void WriteCData(string text)
		{
			this.wrapped.WriteCData(text);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0002BCDB File Offset: 0x0002ACDB
		public override void WriteComment(string text)
		{
			this.wrapped.WriteComment(text);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0002BCE9 File Offset: 0x0002ACE9
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.wrapped.WriteProcessingInstruction(name, text);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0002BCF8 File Offset: 0x0002ACF8
		public override void WriteWhitespace(string ws)
		{
			this.wrapped.WriteWhitespace(ws);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002BD06 File Offset: 0x0002AD06
		public override void WriteString(string text)
		{
			this.wrapped.WriteString(text);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002BD14 File Offset: 0x0002AD14
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.wrapped.WriteChars(buffer, index, count);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002BD24 File Offset: 0x0002AD24
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.wrapped.WriteRaw(buffer, index, count);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002BD34 File Offset: 0x0002AD34
		public override void WriteRaw(string data)
		{
			this.wrapped.WriteRaw(data);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002BD42 File Offset: 0x0002AD42
		public override void WriteEntityRef(string name)
		{
			this.wrapped.WriteEntityRef(name);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002BD50 File Offset: 0x0002AD50
		public override void WriteCharEntity(char ch)
		{
			this.wrapped.WriteCharEntity(ch);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002BD5E File Offset: 0x0002AD5E
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.wrapped.WriteSurrogateCharEntity(lowChar, highChar);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002BD6D File Offset: 0x0002AD6D
		public override void Close()
		{
			this.wrapped.Close();
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002BD7A File Offset: 0x0002AD7A
		public override void Flush()
		{
			this.wrapped.Flush();
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002BD87 File Offset: 0x0002AD87
		public override void WriteValue(object value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002BD95 File Offset: 0x0002AD95
		public override void WriteValue(string value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002BDA3 File Offset: 0x0002ADA3
		public override void WriteValue(bool value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0002BDB1 File Offset: 0x0002ADB1
		public override void WriteValue(DateTime value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002BDBF File Offset: 0x0002ADBF
		public override void WriteValue(float value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002BDCD File Offset: 0x0002ADCD
		public override void WriteValue(decimal value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002BDDB File Offset: 0x0002ADDB
		public override void WriteValue(double value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002BDE9 File Offset: 0x0002ADE9
		public override void WriteValue(int value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002BDF7 File Offset: 0x0002ADF7
		public override void WriteValue(long value)
		{
			this.wrapped.WriteValue(value);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002BE08 File Offset: 0x0002AE08
		protected override void Dispose(bool disposing)
		{
			try
			{
				((IDisposable)this.wrapped).Dispose();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002BE3C File Offset: 0x0002AE3C
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002BE3E File Offset: 0x0002AE3E
		internal override void WriteXmlDeclaration(string xmldecl)
		{
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002BE40 File Offset: 0x0002AE40
		internal override void StartElementContent()
		{
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002BE42 File Offset: 0x0002AE42
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			this.wrapped.WriteEndElement();
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002BE4F File Offset: 0x0002AE4F
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.wrapped.WriteFullEndElement();
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002BE5C File Offset: 0x0002AE5C
		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
			if (prefix.Length == 0)
			{
				this.wrapped.WriteAttributeString(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", ns);
				return;
			}
			this.wrapped.WriteAttributeString("xmlns", prefix, "http://www.w3.org/2000/xmlns/", ns);
		}

		// Token: 0x040005B9 RID: 1465
		private XmlWriter wrapped;
	}
}
