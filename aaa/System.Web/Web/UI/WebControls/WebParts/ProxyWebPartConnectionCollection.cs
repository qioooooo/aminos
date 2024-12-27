using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006F0 RID: 1776
	[Editor("System.ComponentModel.Design.CollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProxyWebPartConnectionCollection : CollectionBase
	{
		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x060056DC RID: 22236 RVA: 0x0015E550 File Offset: 0x0015D550
		public bool IsReadOnly
		{
			get
			{
				return this._webPartManager != null && this._webPartManager.StaticConnections.IsReadOnly;
			}
		}

		// Token: 0x1700166E RID: 5742
		public WebPartConnection this[int index]
		{
			get
			{
				return (WebPartConnection)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x1700166F RID: 5743
		public WebPartConnection this[string id]
		{
			get
			{
				foreach (object obj in base.List)
				{
					WebPartConnection webPartConnection = (WebPartConnection)obj;
					if (webPartConnection != null && string.Equals(webPartConnection.ID, id, StringComparison.OrdinalIgnoreCase))
					{
						return webPartConnection;
					}
				}
				return null;
			}
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x0015E5FC File Offset: 0x0015D5FC
		public int Add(WebPartConnection value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060056E1 RID: 22241 RVA: 0x0015E60A File Offset: 0x0015D60A
		private void CheckReadOnly()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("ProxyWebPartConnectionCollection_ReadOnly"));
			}
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x0015E624 File Offset: 0x0015D624
		public bool Contains(WebPartConnection value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060056E3 RID: 22243 RVA: 0x0015E632 File Offset: 0x0015D632
		public void CopyTo(WebPartConnection[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x0015E641 File Offset: 0x0015D641
		public int IndexOf(WebPartConnection value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060056E5 RID: 22245 RVA: 0x0015E64F File Offset: 0x0015D64F
		public void Insert(int index, WebPartConnection value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x0015E660 File Offset: 0x0015D660
		protected override void OnClear()
		{
			this.CheckReadOnly();
			if (this._webPartManager != null)
			{
				foreach (object obj in this)
				{
					WebPartConnection webPartConnection = (WebPartConnection)obj;
					this._webPartManager.StaticConnections.Remove(webPartConnection);
				}
			}
			base.OnClear();
		}

		// Token: 0x060056E7 RID: 22247 RVA: 0x0015E6D4 File Offset: 0x0015D6D4
		protected override void OnInsert(int index, object value)
		{
			this.CheckReadOnly();
			if (this._webPartManager != null)
			{
				this._webPartManager.StaticConnections.Insert(index, (WebPartConnection)value);
			}
			base.OnInsert(index, value);
		}

		// Token: 0x060056E8 RID: 22248 RVA: 0x0015E703 File Offset: 0x0015D703
		protected override void OnRemove(int index, object value)
		{
			this.CheckReadOnly();
			if (this._webPartManager != null)
			{
				this._webPartManager.StaticConnections.Remove((WebPartConnection)value);
			}
			base.OnRemove(index, value);
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x0015E734 File Offset: 0x0015D734
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.CheckReadOnly();
			if (this._webPartManager != null)
			{
				int num = this._webPartManager.StaticConnections.IndexOf((WebPartConnection)oldValue);
				this._webPartManager.StaticConnections[num] = (WebPartConnection)newValue;
			}
			base.OnSet(index, oldValue, newValue);
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x0015E788 File Offset: 0x0015D788
		protected override void OnValidate(object value)
		{
			base.OnValidate(value);
			if (value == null)
			{
				throw new ArgumentNullException("value", SR.GetString("Collection_CantAddNull"));
			}
			if (!(value is WebPartConnection))
			{
				throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "WebPartConnection" }));
			}
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x0015E7DC File Offset: 0x0015D7DC
		public void Remove(WebPartConnection value)
		{
			base.List.Remove(value);
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x0015E7EC File Offset: 0x0015D7EC
		internal void SetWebPartManager(WebPartManager webPartManager)
		{
			this._webPartManager = webPartManager;
			foreach (object obj in this)
			{
				WebPartConnection webPartConnection = (WebPartConnection)obj;
				this._webPartManager.StaticConnections.Add(webPartConnection);
			}
		}

		// Token: 0x04002F79 RID: 12153
		private WebPartManager _webPartManager;
	}
}
