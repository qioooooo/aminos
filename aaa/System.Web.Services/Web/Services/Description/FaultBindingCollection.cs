using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000FF RID: 255
	public sealed class FaultBindingCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006FC RID: 1788 RVA: 0x0001E85B File Offset: 0x0001D85B
		internal FaultBindingCollection(OperationBinding operationBinding)
			: base(operationBinding)
		{
		}

		// Token: 0x170001FD RID: 509
		public FaultBinding this[int index]
		{
			get
			{
				return (FaultBinding)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001E886 File Offset: 0x0001D886
		public int Add(FaultBinding bindingOperationFault)
		{
			return base.List.Add(bindingOperationFault);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001E894 File Offset: 0x0001D894
		public void Insert(int index, FaultBinding bindingOperationFault)
		{
			base.List.Insert(index, bindingOperationFault);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001E8A3 File Offset: 0x0001D8A3
		public int IndexOf(FaultBinding bindingOperationFault)
		{
			return base.List.IndexOf(bindingOperationFault);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001E8B1 File Offset: 0x0001D8B1
		public bool Contains(FaultBinding bindingOperationFault)
		{
			return base.List.Contains(bindingOperationFault);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001E8BF File Offset: 0x0001D8BF
		public void Remove(FaultBinding bindingOperationFault)
		{
			base.List.Remove(bindingOperationFault);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001E8CD File Offset: 0x0001D8CD
		public void CopyTo(FaultBinding[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x170001FE RID: 510
		public FaultBinding this[string name]
		{
			get
			{
				return (FaultBinding)this.Table[name];
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001E8EF File Offset: 0x0001D8EF
		protected override string GetKey(object value)
		{
			return ((FaultBinding)value).Name;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001E8FC File Offset: 0x0001D8FC
		protected override void SetParent(object value, object parent)
		{
			((FaultBinding)value).SetParent((OperationBinding)parent);
		}
	}
}
