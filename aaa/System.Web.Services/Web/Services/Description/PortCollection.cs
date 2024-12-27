using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000F9 RID: 249
	public sealed class PortCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x0001E443 File Offset: 0x0001D443
		internal PortCollection(Service service)
			: base(service)
		{
		}

		// Token: 0x170001F2 RID: 498
		public Port this[int index]
		{
			get
			{
				return (Port)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001E46E File Offset: 0x0001D46E
		public int Add(Port port)
		{
			return base.List.Add(port);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001E47C File Offset: 0x0001D47C
		public void Insert(int index, Port port)
		{
			base.List.Insert(index, port);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001E48B File Offset: 0x0001D48B
		public int IndexOf(Port port)
		{
			return base.List.IndexOf(port);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001E499 File Offset: 0x0001D499
		public bool Contains(Port port)
		{
			return base.List.Contains(port);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001E4A7 File Offset: 0x0001D4A7
		public void Remove(Port port)
		{
			base.List.Remove(port);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001E4B5 File Offset: 0x0001D4B5
		public void CopyTo(Port[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x170001F3 RID: 499
		public Port this[string name]
		{
			get
			{
				return (Port)this.Table[name];
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001E4D7 File Offset: 0x0001D4D7
		protected override string GetKey(object value)
		{
			return ((Port)value).Name;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001E4E4 File Offset: 0x0001D4E4
		protected override void SetParent(object value, object parent)
		{
			((Port)value).SetParent((Service)parent);
		}
	}
}
