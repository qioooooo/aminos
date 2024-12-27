using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200000C RID: 12
	internal sealed class CachedCodeEntry
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003128 File Offset: 0x00002128
		internal CachedCodeEntry(string key, Hashtable capnames, string[] capslist, RegexCode code, Hashtable caps, int capsize, ExclusiveReference runner, SharedReference repl)
		{
			this._key = key;
			this._capnames = capnames;
			this._capslist = capslist;
			this._code = code;
			this._caps = caps;
			this._capsize = capsize;
			this._runnerref = runner;
			this._replref = repl;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003178 File Offset: 0x00002178
		internal void AddCompiled(RegexRunnerFactory factory)
		{
			this._factory = factory;
			this._code = null;
		}

		// Token: 0x04000634 RID: 1588
		internal string _key;

		// Token: 0x04000635 RID: 1589
		internal RegexCode _code;

		// Token: 0x04000636 RID: 1590
		internal Hashtable _caps;

		// Token: 0x04000637 RID: 1591
		internal Hashtable _capnames;

		// Token: 0x04000638 RID: 1592
		internal string[] _capslist;

		// Token: 0x04000639 RID: 1593
		internal int _capsize;

		// Token: 0x0400063A RID: 1594
		internal RegexRunnerFactory _factory;

		// Token: 0x0400063B RID: 1595
		internal ExclusiveReference _runnerref;

		// Token: 0x0400063C RID: 1596
		internal SharedReference _replref;
	}
}
