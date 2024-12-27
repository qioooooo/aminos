using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000F6 RID: 246
	public sealed class OperationMessageCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x06000690 RID: 1680 RVA: 0x0001E0EF File Offset: 0x0001D0EF
		internal OperationMessageCollection(Operation operation)
			: base(operation)
		{
		}

		// Token: 0x170001EB RID: 491
		public OperationMessage this[int index]
		{
			get
			{
				return (OperationMessage)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001E11A File Offset: 0x0001D11A
		public int Add(OperationMessage operationMessage)
		{
			return base.List.Add(operationMessage);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001E128 File Offset: 0x0001D128
		public void Insert(int index, OperationMessage operationMessage)
		{
			base.List.Insert(index, operationMessage);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001E137 File Offset: 0x0001D137
		public int IndexOf(OperationMessage operationMessage)
		{
			return base.List.IndexOf(operationMessage);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001E145 File Offset: 0x0001D145
		public bool Contains(OperationMessage operationMessage)
		{
			return base.List.Contains(operationMessage);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001E153 File Offset: 0x0001D153
		public void Remove(OperationMessage operationMessage)
		{
			base.List.Remove(operationMessage);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001E161 File Offset: 0x0001D161
		public void CopyTo(OperationMessage[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001E170 File Offset: 0x0001D170
		public OperationInput Input
		{
			get
			{
				for (int i = 0; i < base.List.Count; i++)
				{
					OperationInput operationInput = base.List[i] as OperationInput;
					if (operationInput != null)
					{
						return operationInput;
					}
				}
				return null;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001E1AC File Offset: 0x0001D1AC
		public OperationOutput Output
		{
			get
			{
				for (int i = 0; i < base.List.Count; i++)
				{
					OperationOutput operationOutput = base.List[i] as OperationOutput;
					if (operationOutput != null)
					{
						return operationOutput;
					}
				}
				return null;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001E1E8 File Offset: 0x0001D1E8
		public OperationFlow Flow
		{
			get
			{
				if (base.List.Count == 0)
				{
					return OperationFlow.None;
				}
				if (base.List.Count == 1)
				{
					if (base.List[0] is OperationInput)
					{
						return OperationFlow.OneWay;
					}
					return OperationFlow.Notification;
				}
				else
				{
					if (base.List[0] is OperationInput)
					{
						return OperationFlow.RequestResponse;
					}
					return OperationFlow.SolicitResponse;
				}
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001E23F File Offset: 0x0001D23F
		protected override void SetParent(object value, object parent)
		{
			((OperationMessage)value).SetParent((Operation)parent);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001E254 File Offset: 0x0001D254
		protected override void OnInsert(int index, object value)
		{
			if (base.Count > 1 || (base.Count == 1 && value.GetType() == base.List[0].GetType()))
			{
				throw new InvalidOperationException(Res.GetString("WebDescriptionTooManyMessages"));
			}
			base.OnInsert(index, value);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001E2A4 File Offset: 0x0001D2A4
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			if (oldValue.GetType() != newValue.GetType())
			{
				throw new InvalidOperationException(Res.GetString("WebDescriptionTooManyMessages"));
			}
			base.OnSet(index, oldValue, newValue);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001E2CD File Offset: 0x0001D2CD
		protected override void OnValidate(object value)
		{
			if (!(value is OperationInput) && !(value is OperationOutput))
			{
				throw new ArgumentException(Res.GetString("OnlyOperationInputOrOperationOutputTypes"), "value");
			}
			base.OnValidate(value);
		}
	}
}
