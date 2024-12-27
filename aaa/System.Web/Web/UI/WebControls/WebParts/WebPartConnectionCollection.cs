using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000711 RID: 1809
	[Editor("System.ComponentModel.Design.CollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartConnectionCollection : CollectionBase
	{
		// Token: 0x06005817 RID: 22551 RVA: 0x0016317D File Offset: 0x0016217D
		internal WebPartConnectionCollection(WebPartManager webPartManager)
		{
			this._webPartManager = webPartManager;
		}

		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06005818 RID: 22552 RVA: 0x0016318C File Offset: 0x0016218C
		public bool IsReadOnly
		{
			get
			{
				return this._readOnly;
			}
		}

		// Token: 0x170016C5 RID: 5829
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

		// Token: 0x170016C6 RID: 5830
		public WebPartConnection this[string id]
		{
			get
			{
				foreach (object obj in base.List)
				{
					WebPartConnection webPartConnection = (WebPartConnection)obj;
					if (string.Equals(webPartConnection.ID, id, StringComparison.OrdinalIgnoreCase))
					{
						return webPartConnection;
					}
				}
				return null;
			}
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x00163220 File Offset: 0x00162220
		public int Add(WebPartConnection value)
		{
			return base.List.Add(value);
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x0016322E File Offset: 0x0016222E
		private void CheckReadOnly()
		{
			if (this._readOnly)
			{
				throw new InvalidOperationException(SR.GetString(this._readOnlyExceptionMessage));
			}
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x00163249 File Offset: 0x00162249
		public bool Contains(WebPartConnection value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x00163258 File Offset: 0x00162258
		internal bool ContainsProvider(WebPart provider)
		{
			foreach (object obj in base.List)
			{
				WebPartConnection webPartConnection = (WebPartConnection)obj;
				if (webPartConnection.Provider == provider)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x001632BC File Offset: 0x001622BC
		public void CopyTo(WebPartConnection[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x001632CB File Offset: 0x001622CB
		public int IndexOf(WebPartConnection value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x001632D9 File Offset: 0x001622D9
		public void Insert(int index, WebPartConnection value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x001632E8 File Offset: 0x001622E8
		protected override void OnClear()
		{
			this.CheckReadOnly();
			base.OnClear();
		}

		// Token: 0x06005824 RID: 22564 RVA: 0x001632F6 File Offset: 0x001622F6
		protected override void OnInsert(int index, object value)
		{
			this.CheckReadOnly();
			((WebPartConnection)value).SetWebPartManager(this._webPartManager);
			base.OnInsert(index, value);
		}

		// Token: 0x06005825 RID: 22565 RVA: 0x00163317 File Offset: 0x00162317
		protected override void OnRemove(int index, object value)
		{
			this.CheckReadOnly();
			((WebPartConnection)value).SetWebPartManager(null);
			base.OnRemove(index, value);
		}

		// Token: 0x06005826 RID: 22566 RVA: 0x00163333 File Offset: 0x00162333
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.CheckReadOnly();
			((WebPartConnection)oldValue).SetWebPartManager(null);
			((WebPartConnection)newValue).SetWebPartManager(this._webPartManager);
			base.OnSet(index, oldValue, newValue);
		}

		// Token: 0x06005827 RID: 22567 RVA: 0x00163364 File Offset: 0x00162364
		protected override void OnValidate(object value)
		{
			base.OnValidate(value);
			if (value == null)
			{
				throw new ArgumentNullException("value", SR.GetString("Collection_CantAddNull"));
			}
			if (!(value is WebPartConnection))
			{
				throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "WebPartConnection" }), "value");
			}
		}

		// Token: 0x06005828 RID: 22568 RVA: 0x001633BD File Offset: 0x001623BD
		public void Remove(WebPartConnection value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06005829 RID: 22569 RVA: 0x001633CB File Offset: 0x001623CB
		internal void SetReadOnly(string exceptionMessage)
		{
			this._readOnlyExceptionMessage = exceptionMessage;
			this._readOnly = true;
		}

		// Token: 0x04002FD0 RID: 12240
		private bool _readOnly;

		// Token: 0x04002FD1 RID: 12241
		private string _readOnlyExceptionMessage;

		// Token: 0x04002FD2 RID: 12242
		private WebPartManager _webPartManager;
	}
}
