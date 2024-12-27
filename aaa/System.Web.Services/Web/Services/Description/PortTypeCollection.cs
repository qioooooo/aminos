using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000FA RID: 250
	public sealed class PortTypeCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006C2 RID: 1730 RVA: 0x0001E4F7 File Offset: 0x0001D4F7
		internal PortTypeCollection(ServiceDescription serviceDescription)
			: base(serviceDescription)
		{
		}

		// Token: 0x170001F4 RID: 500
		public PortType this[int index]
		{
			get
			{
				return (PortType)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001E522 File Offset: 0x0001D522
		public int Add(PortType portType)
		{
			return base.List.Add(portType);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001E530 File Offset: 0x0001D530
		public void Insert(int index, PortType portType)
		{
			base.List.Insert(index, portType);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001E53F File Offset: 0x0001D53F
		public int IndexOf(PortType portType)
		{
			return base.List.IndexOf(portType);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001E54D File Offset: 0x0001D54D
		public bool Contains(PortType portType)
		{
			return base.List.Contains(portType);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001E55B File Offset: 0x0001D55B
		public void Remove(PortType portType)
		{
			base.List.Remove(portType);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001E569 File Offset: 0x0001D569
		public void CopyTo(PortType[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x170001F5 RID: 501
		public PortType this[string name]
		{
			get
			{
				return (PortType)this.Table[name];
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001E58B File Offset: 0x0001D58B
		protected override string GetKey(object value)
		{
			return ((PortType)value).Name;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001E598 File Offset: 0x0001D598
		protected override void SetParent(object value, object parent)
		{
			((PortType)value).SetParent((ServiceDescription)parent);
		}
	}
}
