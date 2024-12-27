using System;
using System.IO;
using System.Xml;

namespace System.Web.Util
{
	// Token: 0x0200076F RID: 1903
	internal sealed class NoEntitiesXmlReader : XmlTextReader
	{
		// Token: 0x06005C4E RID: 23630 RVA: 0x00172454 File Offset: 0x00171454
		public NoEntitiesXmlReader(string filepath)
			: base(filepath)
		{
			this.Initialize();
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x00172463 File Offset: 0x00171463
		public NoEntitiesXmlReader(Stream datastream)
			: base(datastream)
		{
			this.Initialize();
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x00172472 File Offset: 0x00171472
		public NoEntitiesXmlReader(TextReader reader)
			: base(reader)
		{
			this.Initialize();
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x00172481 File Offset: 0x00171481
		public NoEntitiesXmlReader(string baseURI, Stream contentStream)
			: base(baseURI, contentStream)
		{
			this.Initialize();
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x00172491 File Offset: 0x00171491
		private void Initialize()
		{
			base.EntityHandling = EntityHandling.ExpandCharEntities;
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x0017249A File Offset: 0x0017149A
		public override void ResolveEntity()
		{
		}
	}
}
