using System;

namespace System.Web.Services.Description
{
	// Token: 0x02000101 RID: 257
	public sealed class OperationFaultCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x06000712 RID: 1810 RVA: 0x0001E9A3 File Offset: 0x0001D9A3
		internal OperationFaultCollection(Operation operation)
			: base(operation)
		{
		}

		// Token: 0x17000200 RID: 512
		public OperationFault this[int index]
		{
			get
			{
				return (OperationFault)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001E9CE File Offset: 0x0001D9CE
		public int Add(OperationFault operationFaultMessage)
		{
			return base.List.Add(operationFaultMessage);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001E9DC File Offset: 0x0001D9DC
		public void Insert(int index, OperationFault operationFaultMessage)
		{
			base.List.Insert(index, operationFaultMessage);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001E9EB File Offset: 0x0001D9EB
		public int IndexOf(OperationFault operationFaultMessage)
		{
			return base.List.IndexOf(operationFaultMessage);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001E9F9 File Offset: 0x0001D9F9
		public bool Contains(OperationFault operationFaultMessage)
		{
			return base.List.Contains(operationFaultMessage);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001EA07 File Offset: 0x0001DA07
		public void Remove(OperationFault operationFaultMessage)
		{
			base.List.Remove(operationFaultMessage);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001EA15 File Offset: 0x0001DA15
		public void CopyTo(OperationFault[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x17000201 RID: 513
		public OperationFault this[string name]
		{
			get
			{
				return (OperationFault)this.Table[name];
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001EA37 File Offset: 0x0001DA37
		protected override string GetKey(object value)
		{
			return ((OperationFault)value).Name;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001EA44 File Offset: 0x0001DA44
		protected override void SetParent(object value, object parent)
		{
			((OperationFault)value).SetParent((Operation)parent);
		}
	}
}
