using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000F7 RID: 247
	public sealed class ImportCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006A0 RID: 1696 RVA: 0x0001E2FB File Offset: 0x0001D2FB
		internal ImportCollection(ServiceDescription serviceDescription)
			: base(serviceDescription)
		{
		}

		// Token: 0x170001EF RID: 495
		public Import this[int index]
		{
			get
			{
				return (Import)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001E326 File Offset: 0x0001D326
		public int Add(Import import)
		{
			return base.List.Add(import);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001E334 File Offset: 0x0001D334
		public void Insert(int index, Import import)
		{
			base.List.Insert(index, import);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001E343 File Offset: 0x0001D343
		public int IndexOf(Import import)
		{
			return base.List.IndexOf(import);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001E351 File Offset: 0x0001D351
		public bool Contains(Import import)
		{
			return base.List.Contains(import);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001E35F File Offset: 0x0001D35F
		public void Remove(Import import)
		{
			base.List.Remove(import);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001E36D File Offset: 0x0001D36D
		public void CopyTo(Import[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001E37C File Offset: 0x0001D37C
		protected override void SetParent(object value, object parent)
		{
			((Import)value).SetParent((ServiceDescription)parent);
		}
	}
}
