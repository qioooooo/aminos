using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D2 RID: 1234
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class ImportedFromTypeLibAttribute : Attribute
	{
		// Token: 0x06003102 RID: 12546 RVA: 0x000A8CC8 File Offset: 0x000A7CC8
		public ImportedFromTypeLibAttribute(string tlbFile)
		{
			this._val = tlbFile;
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x000A8CD7 File Offset: 0x000A7CD7
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018B0 RID: 6320
		internal string _val;
	}
}
