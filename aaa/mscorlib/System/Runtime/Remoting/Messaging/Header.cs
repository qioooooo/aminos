using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006C6 RID: 1734
	[ComVisible(true)]
	[Serializable]
	public class Header
	{
		// Token: 0x06003ED1 RID: 16081 RVA: 0x000D803D File Offset: 0x000D703D
		public Header(string _Name, object _Value)
			: this(_Name, _Value, true)
		{
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x000D8048 File Offset: 0x000D7048
		public Header(string _Name, object _Value, bool _MustUnderstand)
		{
			this.Name = _Name;
			this.Value = _Value;
			this.MustUnderstand = _MustUnderstand;
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x000D8065 File Offset: 0x000D7065
		public Header(string _Name, object _Value, bool _MustUnderstand, string _HeaderNamespace)
		{
			this.Name = _Name;
			this.Value = _Value;
			this.MustUnderstand = _MustUnderstand;
			this.HeaderNamespace = _HeaderNamespace;
		}

		// Token: 0x04001FBA RID: 8122
		public string Name;

		// Token: 0x04001FBB RID: 8123
		public object Value;

		// Token: 0x04001FBC RID: 8124
		public bool MustUnderstand;

		// Token: 0x04001FBD RID: 8125
		public string HeaderNamespace;
	}
}
