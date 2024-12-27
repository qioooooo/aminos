using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000FB RID: 251
	public sealed class BindingCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006CE RID: 1742 RVA: 0x0001E5AB File Offset: 0x0001D5AB
		internal BindingCollection(ServiceDescription serviceDescription)
			: base(serviceDescription)
		{
		}

		// Token: 0x170001F6 RID: 502
		public Binding this[int index]
		{
			get
			{
				return (Binding)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001E5D6 File Offset: 0x0001D5D6
		public int Add(Binding binding)
		{
			return base.List.Add(binding);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001E5E4 File Offset: 0x0001D5E4
		public void Insert(int index, Binding binding)
		{
			base.List.Insert(index, binding);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001E5F3 File Offset: 0x0001D5F3
		public int IndexOf(Binding binding)
		{
			return base.List.IndexOf(binding);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001E601 File Offset: 0x0001D601
		public bool Contains(Binding binding)
		{
			return base.List.Contains(binding);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001E60F File Offset: 0x0001D60F
		public void Remove(Binding binding)
		{
			base.List.Remove(binding);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001E61D File Offset: 0x0001D61D
		public void CopyTo(Binding[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x170001F7 RID: 503
		public Binding this[string name]
		{
			get
			{
				return (Binding)this.Table[name];
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001E63F File Offset: 0x0001D63F
		protected override string GetKey(object value)
		{
			return ((Binding)value).Name;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001E64C File Offset: 0x0001D64C
		protected override void SetParent(object value, object parent)
		{
			((Binding)value).SetParent((ServiceDescription)parent);
		}
	}
}
