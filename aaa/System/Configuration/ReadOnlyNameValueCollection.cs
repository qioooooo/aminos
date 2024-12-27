using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x02000702 RID: 1794
	internal class ReadOnlyNameValueCollection : NameValueCollection
	{
		// Token: 0x0600373A RID: 14138 RVA: 0x000EAF0E File Offset: 0x000E9F0E
		internal ReadOnlyNameValueCollection(IEqualityComparer equalityComparer)
			: base(equalityComparer)
		{
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000EAF17 File Offset: 0x000E9F17
		internal ReadOnlyNameValueCollection(ReadOnlyNameValueCollection value)
			: base(value)
		{
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x000EAF20 File Offset: 0x000E9F20
		internal void SetReadOnly()
		{
			base.IsReadOnly = true;
		}
	}
}
