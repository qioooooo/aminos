using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000268 RID: 616
	public class XmlSchemaObjectCollection : CollectionBase
	{
		// Token: 0x06001CAA RID: 7338 RVA: 0x000834F9 File Offset: 0x000824F9
		public XmlSchemaObjectCollection()
		{
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x00083501 File Offset: 0x00082501
		public XmlSchemaObjectCollection(XmlSchemaObject parent)
		{
			this.parent = parent;
		}

		// Token: 0x17000765 RID: 1893
		public virtual XmlSchemaObject this[int index]
		{
			get
			{
				return (XmlSchemaObject)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x00083532 File Offset: 0x00082532
		public new XmlSchemaObjectEnumerator GetEnumerator()
		{
			return new XmlSchemaObjectEnumerator(base.InnerList.GetEnumerator());
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x00083544 File Offset: 0x00082544
		public int Add(XmlSchemaObject item)
		{
			return base.List.Add(item);
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x00083552 File Offset: 0x00082552
		public void Insert(int index, XmlSchemaObject item)
		{
			base.List.Insert(index, item);
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x00083561 File Offset: 0x00082561
		public int IndexOf(XmlSchemaObject item)
		{
			return base.List.IndexOf(item);
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0008356F File Offset: 0x0008256F
		public bool Contains(XmlSchemaObject item)
		{
			return base.List.Contains(item);
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x0008357D File Offset: 0x0008257D
		public void Remove(XmlSchemaObject item)
		{
			base.List.Remove(item);
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0008358B File Offset: 0x0008258B
		public void CopyTo(XmlSchemaObject[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0008359A File Offset: 0x0008259A
		protected override void OnInsert(int index, object item)
		{
			if (this.parent != null)
			{
				this.parent.OnAdd(this, item);
			}
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000835B1 File Offset: 0x000825B1
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			if (this.parent != null)
			{
				this.parent.OnRemove(this, oldValue);
				this.parent.OnAdd(this, newValue);
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000835D5 File Offset: 0x000825D5
		protected override void OnClear()
		{
			if (this.parent != null)
			{
				this.parent.OnClear(this);
			}
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000835EB File Offset: 0x000825EB
		protected override void OnRemove(int index, object item)
		{
			if (this.parent != null)
			{
				this.parent.OnRemove(this, item);
			}
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x00083604 File Offset: 0x00082604
		internal XmlSchemaObjectCollection Clone()
		{
			return new XmlSchemaObjectCollection { this };
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x0008361F File Offset: 0x0008261F
		private void Add(XmlSchemaObjectCollection collToAdd)
		{
			base.InnerList.InsertRange(0, collToAdd);
		}

		// Token: 0x040011A1 RID: 4513
		private XmlSchemaObject parent;
	}
}
