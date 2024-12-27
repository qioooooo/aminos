using System;
using System.Collections;
using System.Globalization;

namespace System.Xml.Serialization
{
	// Token: 0x020002AF RID: 687
	internal class CaseInsensitiveKeyComparer : CaseInsensitiveComparer, IEqualityComparer
	{
		// Token: 0x06002108 RID: 8456 RVA: 0x0009C5C7 File Offset: 0x0009B5C7
		public CaseInsensitiveKeyComparer()
			: base(CultureInfo.CurrentCulture)
		{
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x0009C5D4 File Offset: 0x0009B5D4
		bool IEqualityComparer.Equals(object x, object y)
		{
			return base.Compare(x, y) == 0;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x0009C5E4 File Offset: 0x0009B5E4
		int IEqualityComparer.GetHashCode(object obj)
		{
			string text = obj as string;
			if (text == null)
			{
				throw new ArgumentException(null, "obj");
			}
			return text.ToUpper(CultureInfo.CurrentCulture).GetHashCode();
		}
	}
}
