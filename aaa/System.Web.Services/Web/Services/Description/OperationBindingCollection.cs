using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000FE RID: 254
	public sealed class OperationBindingCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006F2 RID: 1778 RVA: 0x0001E7C7 File Offset: 0x0001D7C7
		internal OperationBindingCollection(Binding binding)
			: base(binding)
		{
		}

		// Token: 0x170001FC RID: 508
		public OperationBinding this[int index]
		{
			get
			{
				return (OperationBinding)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001E7F2 File Offset: 0x0001D7F2
		public int Add(OperationBinding bindingOperation)
		{
			return base.List.Add(bindingOperation);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001E800 File Offset: 0x0001D800
		public void Insert(int index, OperationBinding bindingOperation)
		{
			base.List.Insert(index, bindingOperation);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001E80F File Offset: 0x0001D80F
		public int IndexOf(OperationBinding bindingOperation)
		{
			return base.List.IndexOf(bindingOperation);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001E81D File Offset: 0x0001D81D
		public bool Contains(OperationBinding bindingOperation)
		{
			return base.List.Contains(bindingOperation);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001E82B File Offset: 0x0001D82B
		public void Remove(OperationBinding bindingOperation)
		{
			base.List.Remove(bindingOperation);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001E839 File Offset: 0x0001D839
		public void CopyTo(OperationBinding[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001E848 File Offset: 0x0001D848
		protected override void SetParent(object value, object parent)
		{
			((OperationBinding)value).SetParent((Binding)parent);
		}
	}
}
