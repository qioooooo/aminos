using System;
using System.Collections;

namespace System.Web.Services.Description
{
	// Token: 0x020000CB RID: 203
	public sealed class MimePartCollection : CollectionBase
	{
		// Token: 0x17000189 RID: 393
		public MimePart this[int index]
		{
			get
			{
				return (MimePart)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001B289 File Offset: 0x0001A289
		public int Add(MimePart mimePart)
		{
			return base.List.Add(mimePart);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001B297 File Offset: 0x0001A297
		public void Insert(int index, MimePart mimePart)
		{
			base.List.Insert(index, mimePart);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001B2A6 File Offset: 0x0001A2A6
		public int IndexOf(MimePart mimePart)
		{
			return base.List.IndexOf(mimePart);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001B2B4 File Offset: 0x0001A2B4
		public bool Contains(MimePart mimePart)
		{
			return base.List.Contains(mimePart);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001B2C2 File Offset: 0x0001A2C2
		public void Remove(MimePart mimePart)
		{
			base.List.Remove(mimePart);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001B2D0 File Offset: 0x0001A2D0
		public void CopyTo(MimePart[] array, int index)
		{
			base.List.CopyTo(array, index);
		}
	}
}
