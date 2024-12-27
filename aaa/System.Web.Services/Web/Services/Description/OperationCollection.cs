using System;

namespace System.Web.Services.Description
{
	// Token: 0x02000100 RID: 256
	public sealed class OperationCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x06000708 RID: 1800 RVA: 0x0001E90F File Offset: 0x0001D90F
		internal OperationCollection(PortType portType)
			: base(portType)
		{
		}

		// Token: 0x170001FF RID: 511
		public Operation this[int index]
		{
			get
			{
				return (Operation)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001E93A File Offset: 0x0001D93A
		public int Add(Operation operation)
		{
			return base.List.Add(operation);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001E948 File Offset: 0x0001D948
		public void Insert(int index, Operation operation)
		{
			base.List.Insert(index, operation);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001E957 File Offset: 0x0001D957
		public int IndexOf(Operation operation)
		{
			return base.List.IndexOf(operation);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001E965 File Offset: 0x0001D965
		public bool Contains(Operation operation)
		{
			return base.List.Contains(operation);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001E973 File Offset: 0x0001D973
		public void Remove(Operation operation)
		{
			base.List.Remove(operation);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001E981 File Offset: 0x0001D981
		public void CopyTo(Operation[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001E990 File Offset: 0x0001D990
		protected override void SetParent(object value, object parent)
		{
			((Operation)value).SetParent((PortType)parent);
		}
	}
}
