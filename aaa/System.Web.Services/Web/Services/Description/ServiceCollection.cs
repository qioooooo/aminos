using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000FC RID: 252
	public sealed class ServiceCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006DA RID: 1754 RVA: 0x0001E65F File Offset: 0x0001D65F
		internal ServiceCollection(ServiceDescription serviceDescription)
			: base(serviceDescription)
		{
		}

		// Token: 0x170001F8 RID: 504
		public Service this[int index]
		{
			get
			{
				return (Service)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001E68A File Offset: 0x0001D68A
		public int Add(Service service)
		{
			return base.List.Add(service);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001E698 File Offset: 0x0001D698
		public void Insert(int index, Service service)
		{
			base.List.Insert(index, service);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001E6A7 File Offset: 0x0001D6A7
		public int IndexOf(Service service)
		{
			return base.List.IndexOf(service);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001E6B5 File Offset: 0x0001D6B5
		public bool Contains(Service service)
		{
			return base.List.Contains(service);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001E6C3 File Offset: 0x0001D6C3
		public void Remove(Service service)
		{
			base.List.Remove(service);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001E6D1 File Offset: 0x0001D6D1
		public void CopyTo(Service[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x170001F9 RID: 505
		public Service this[string name]
		{
			get
			{
				return (Service)this.Table[name];
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001E6F3 File Offset: 0x0001D6F3
		protected override string GetKey(object value)
		{
			return ((Service)value).Name;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001E700 File Offset: 0x0001D700
		protected override void SetParent(object value, object parent)
		{
			((Service)value).SetParent((ServiceDescription)parent);
		}
	}
}
