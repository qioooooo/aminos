using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000743 RID: 1859
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartTransformerCollection : CollectionBase
	{
		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x06005A31 RID: 23089 RVA: 0x0016C2B6 File Offset: 0x0016B2B6
		public bool IsReadOnly
		{
			get
			{
				return this._readOnly;
			}
		}

		// Token: 0x17001750 RID: 5968
		public WebPartTransformer this[int index]
		{
			get
			{
				return (WebPartTransformer)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x0016C2E0 File Offset: 0x0016B2E0
		public int Add(WebPartTransformer transformer)
		{
			return base.List.Add(transformer);
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x0016C2EE File Offset: 0x0016B2EE
		private void CheckReadOnly()
		{
			if (this._readOnly)
			{
				throw new InvalidOperationException(SR.GetString("WebPartTransformerCollection_ReadOnly"));
			}
		}

		// Token: 0x06005A36 RID: 23094 RVA: 0x0016C308 File Offset: 0x0016B308
		public bool Contains(WebPartTransformer transformer)
		{
			return base.List.Contains(transformer);
		}

		// Token: 0x06005A37 RID: 23095 RVA: 0x0016C316 File Offset: 0x0016B316
		public void CopyTo(WebPartTransformer[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x0016C325 File Offset: 0x0016B325
		public int IndexOf(WebPartTransformer transformer)
		{
			return base.List.IndexOf(transformer);
		}

		// Token: 0x06005A39 RID: 23097 RVA: 0x0016C333 File Offset: 0x0016B333
		public void Insert(int index, WebPartTransformer transformer)
		{
			base.List.Insert(index, transformer);
		}

		// Token: 0x06005A3A RID: 23098 RVA: 0x0016C342 File Offset: 0x0016B342
		protected override void OnClear()
		{
			this.CheckReadOnly();
			base.OnClear();
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x0016C350 File Offset: 0x0016B350
		protected override void OnInsert(int index, object value)
		{
			this.CheckReadOnly();
			if (base.List.Count > 0)
			{
				throw new InvalidOperationException(SR.GetString("WebPartTransformerCollection_NotEmpty"));
			}
			base.OnInsert(index, value);
		}

		// Token: 0x06005A3C RID: 23100 RVA: 0x0016C37E File Offset: 0x0016B37E
		protected override void OnRemove(int index, object value)
		{
			this.CheckReadOnly();
			base.OnRemove(index, value);
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x0016C38E File Offset: 0x0016B38E
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.CheckReadOnly();
			base.OnSet(index, oldValue, newValue);
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x0016C3A0 File Offset: 0x0016B3A0
		protected override void OnValidate(object value)
		{
			base.OnValidate(value);
			if (value == null)
			{
				throw new ArgumentNullException("value", SR.GetString("Collection_CantAddNull"));
			}
			if (!(value is WebPartTransformer))
			{
				throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "WebPartTransformer" }), "value");
			}
		}

		// Token: 0x06005A3F RID: 23103 RVA: 0x0016C3F9 File Offset: 0x0016B3F9
		public void Remove(WebPartTransformer transformer)
		{
			base.List.Remove(transformer);
		}

		// Token: 0x06005A40 RID: 23104 RVA: 0x0016C407 File Offset: 0x0016B407
		internal void SetReadOnly()
		{
			this._readOnly = true;
		}

		// Token: 0x04003083 RID: 12419
		private bool _readOnly;
	}
}
