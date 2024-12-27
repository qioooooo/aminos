using System;

namespace System.Xml.Schema
{
	// Token: 0x0200018D RID: 397
	internal class KSStruct
	{
		// Token: 0x06001515 RID: 5397 RVA: 0x0005E23F File Offset: 0x0005D23F
		public KSStruct(KeySequence ks, int dim)
		{
			this.ks = ks;
			this.fields = new LocatedActiveAxis[dim];
		}

		// Token: 0x04000CA9 RID: 3241
		public int depth;

		// Token: 0x04000CAA RID: 3242
		public KeySequence ks;

		// Token: 0x04000CAB RID: 3243
		public LocatedActiveAxis[] fields;
	}
}
