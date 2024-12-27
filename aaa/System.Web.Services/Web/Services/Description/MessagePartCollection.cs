using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000FD RID: 253
	public sealed class MessagePartCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x0001E713 File Offset: 0x0001D713
		internal MessagePartCollection(Message message)
			: base(message)
		{
		}

		// Token: 0x170001FA RID: 506
		public MessagePart this[int index]
		{
			get
			{
				return (MessagePart)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001E73E File Offset: 0x0001D73E
		public int Add(MessagePart messagePart)
		{
			return base.List.Add(messagePart);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001E74C File Offset: 0x0001D74C
		public void Insert(int index, MessagePart messagePart)
		{
			base.List.Insert(index, messagePart);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001E75B File Offset: 0x0001D75B
		public int IndexOf(MessagePart messagePart)
		{
			return base.List.IndexOf(messagePart);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001E769 File Offset: 0x0001D769
		public bool Contains(MessagePart messagePart)
		{
			return base.List.Contains(messagePart);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001E777 File Offset: 0x0001D777
		public void Remove(MessagePart messagePart)
		{
			base.List.Remove(messagePart);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001E785 File Offset: 0x0001D785
		public void CopyTo(MessagePart[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x170001FB RID: 507
		public MessagePart this[string name]
		{
			get
			{
				return (MessagePart)this.Table[name];
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001E7A7 File Offset: 0x0001D7A7
		protected override string GetKey(object value)
		{
			return ((MessagePart)value).Name;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001E7B4 File Offset: 0x0001D7B4
		protected override void SetParent(object value, object parent)
		{
			((MessagePart)value).SetParent((Message)parent);
		}
	}
}
