using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200002E RID: 46
	internal sealed class RegexTree
	{
		// Token: 0x06000232 RID: 562 RVA: 0x00011755 File Offset: 0x00010755
		internal RegexTree(RegexNode root, Hashtable caps, object[] capnumlist, int captop, Hashtable capnames, string[] capslist, RegexOptions opts)
		{
			this._root = root;
			this._caps = caps;
			this._capnumlist = capnumlist;
			this._capnames = capnames;
			this._capslist = capslist;
			this._captop = captop;
			this._options = opts;
		}

		// Token: 0x040007BB RID: 1979
		internal RegexNode _root;

		// Token: 0x040007BC RID: 1980
		internal Hashtable _caps;

		// Token: 0x040007BD RID: 1981
		internal object[] _capnumlist;

		// Token: 0x040007BE RID: 1982
		internal Hashtable _capnames;

		// Token: 0x040007BF RID: 1983
		internal string[] _capslist;

		// Token: 0x040007C0 RID: 1984
		internal RegexOptions _options;

		// Token: 0x040007C1 RID: 1985
		internal int _captop;
	}
}
