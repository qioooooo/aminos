using System;

namespace System.Xml.Schema
{
	// Token: 0x0200020C RID: 524
	internal abstract class SchemaBuilder
	{
		// Token: 0x060018DE RID: 6366
		internal abstract bool ProcessElement(string prefix, string name, string ns);

		// Token: 0x060018DF RID: 6367
		internal abstract void ProcessAttribute(string prefix, string name, string ns, string value);

		// Token: 0x060018E0 RID: 6368
		internal abstract bool IsContentParsed();

		// Token: 0x060018E1 RID: 6369
		internal abstract void ProcessMarkup(XmlNode[] markup);

		// Token: 0x060018E2 RID: 6370
		internal abstract void ProcessCData(string value);

		// Token: 0x060018E3 RID: 6371
		internal abstract void StartChildren();

		// Token: 0x060018E4 RID: 6372
		internal abstract void EndChildren();
	}
}
