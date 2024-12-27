using System;
using System.Collections;

namespace System.Web.Services.Description
{
	// Token: 0x020000CE RID: 206
	public sealed class MimeTextMatchCollection : CollectionBase
	{
		// Token: 0x17000194 RID: 404
		public MimeTextMatch this[int index]
		{
			get
			{
				return (MimeTextMatch)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001B4CC File Offset: 0x0001A4CC
		public int Add(MimeTextMatch match)
		{
			return base.List.Add(match);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001B4DA File Offset: 0x0001A4DA
		public void Insert(int index, MimeTextMatch match)
		{
			base.List.Insert(index, match);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001B4E9 File Offset: 0x0001A4E9
		public int IndexOf(MimeTextMatch match)
		{
			return base.List.IndexOf(match);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001B4F7 File Offset: 0x0001A4F7
		public bool Contains(MimeTextMatch match)
		{
			return base.List.Contains(match);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001B505 File Offset: 0x0001A505
		public void Remove(MimeTextMatch match)
		{
			base.List.Remove(match);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001B513 File Offset: 0x0001A513
		public void CopyTo(MimeTextMatch[] array, int index)
		{
			base.List.CopyTo(array, index);
		}
	}
}
