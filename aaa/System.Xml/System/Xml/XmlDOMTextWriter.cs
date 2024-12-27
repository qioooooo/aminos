using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	// Token: 0x020000D8 RID: 216
	internal class XmlDOMTextWriter : XmlTextWriter
	{
		// Token: 0x06000D3E RID: 3390 RVA: 0x0003B3EC File Offset: 0x0003A3EC
		public XmlDOMTextWriter(Stream w, Encoding encoding)
			: base(w, encoding)
		{
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0003B3F6 File Offset: 0x0003A3F6
		public XmlDOMTextWriter(string filename, Encoding encoding)
			: base(filename, encoding)
		{
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0003B400 File Offset: 0x0003A400
		public XmlDOMTextWriter(TextWriter w)
			: base(w)
		{
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003B409 File Offset: 0x0003A409
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (ns.Length == 0 && prefix.Length != 0)
			{
				prefix = "";
			}
			base.WriteStartElement(prefix, localName, ns);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0003B42B File Offset: 0x0003A42B
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (ns.Length == 0 && prefix.Length != 0)
			{
				prefix = "";
			}
			base.WriteStartAttribute(prefix, localName, ns);
		}
	}
}
