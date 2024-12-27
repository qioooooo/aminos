using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000F8 RID: 248
	public sealed class MessageCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x060006AA RID: 1706 RVA: 0x0001E38F File Offset: 0x0001D38F
		internal MessageCollection(ServiceDescription serviceDescription)
			: base(serviceDescription)
		{
		}

		// Token: 0x170001F0 RID: 496
		public Message this[int index]
		{
			get
			{
				return (Message)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001E3BA File Offset: 0x0001D3BA
		public int Add(Message message)
		{
			return base.List.Add(message);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001E3C8 File Offset: 0x0001D3C8
		public void Insert(int index, Message message)
		{
			base.List.Insert(index, message);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001E3D7 File Offset: 0x0001D3D7
		public int IndexOf(Message message)
		{
			return base.List.IndexOf(message);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001E3E5 File Offset: 0x0001D3E5
		public bool Contains(Message message)
		{
			return base.List.Contains(message);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001E3F3 File Offset: 0x0001D3F3
		public void Remove(Message message)
		{
			base.List.Remove(message);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001E401 File Offset: 0x0001D401
		public void CopyTo(Message[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x170001F1 RID: 497
		public Message this[string name]
		{
			get
			{
				return (Message)this.Table[name];
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001E423 File Offset: 0x0001D423
		protected override string GetKey(object value)
		{
			return ((Message)value).Name;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001E430 File Offset: 0x0001D430
		protected override void SetParent(object value, object parent)
		{
			((Message)value).SetParent((ServiceDescription)parent);
		}
	}
}
