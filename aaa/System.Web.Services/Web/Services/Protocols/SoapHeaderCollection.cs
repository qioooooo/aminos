using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000072 RID: 114
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class SoapHeaderCollection : CollectionBase
	{
		// Token: 0x170000E2 RID: 226
		public SoapHeader this[int index]
		{
			get
			{
				return (SoapHeader)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000E4B3 File Offset: 0x0000D4B3
		public int Add(SoapHeader header)
		{
			return base.List.Add(header);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000E4C1 File Offset: 0x0000D4C1
		public void Insert(int index, SoapHeader header)
		{
			base.List.Insert(index, header);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000E4D0 File Offset: 0x0000D4D0
		public int IndexOf(SoapHeader header)
		{
			return base.List.IndexOf(header);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000E4DE File Offset: 0x0000D4DE
		public bool Contains(SoapHeader header)
		{
			return base.List.Contains(header);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000E4EC File Offset: 0x0000D4EC
		public void Remove(SoapHeader header)
		{
			base.List.Remove(header);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000E4FA File Offset: 0x0000D4FA
		public void CopyTo(SoapHeader[] array, int index)
		{
			base.List.CopyTo(array, index);
		}
	}
}
