using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003DD RID: 989
	[Bindable(false)]
	[Designer("System.Web.UI.Design.DataSourceDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[NonVisualControl]
	[ControlBuilder(typeof(DataSourceControlBuilder))]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class DataSourceControl : Control, IDataSource, IListSource
	{
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x000D4B9B File Offset: 0x000D3B9B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string ClientID
		{
			get
			{
				return base.ClientID;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x000D4BA3 File Offset: 0x000D3BA3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x000D4BAB File Offset: 0x000D3BAB
		// (set) Token: 0x06003008 RID: 12296 RVA: 0x000D4BB0 File Offset: 0x000D3BB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(false)]
		[Browsable(false)]
		public override bool EnableTheming
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NoThemingSupport", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x000D4BE2 File Offset: 0x000D3BE2
		// (set) Token: 0x0600300A RID: 12298 RVA: 0x000D4BEC File Offset: 0x000D3BEC
		[Browsable(false)]
		[DefaultValue("")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string SkinID
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NoThemingSupport", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x000D4C1E File Offset: 0x000D3C1E
		// (set) Token: 0x0600300C RID: 12300 RVA: 0x000D4C24 File Offset: 0x000D3C24
		[DefaultValue(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Visible
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("ControlNonVisual", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x0600300D RID: 12301 RVA: 0x000D4C56 File Offset: 0x000D3C56
		// (remove) Token: 0x0600300E RID: 12302 RVA: 0x000D4C69 File Offset: 0x000D3C69
		internal event EventHandler DataSourceChangedInternal
		{
			add
			{
				base.Events.AddHandler(DataSourceControl.EventDataSourceChangedInternal, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataSourceControl.EventDataSourceChangedInternal, value);
			}
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000D4C7C File Offset: 0x000D3C7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ApplyStyleSheetSkin(Page page)
		{
			base.ApplyStyleSheetSkin(page);
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000D4C85 File Offset: 0x000D3C85
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection(this);
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000D4C8D File Offset: 0x000D3C8D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Control FindControl(string id)
		{
			return base.FindControl(id);
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x000D4C98 File Offset: 0x000D3C98
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Focus()
		{
			throw new NotSupportedException(SR.GetString("NoFocusSupport", new object[] { base.GetType().Name }));
		}

		// Token: 0x06003013 RID: 12307
		protected abstract DataSourceView GetView(string viewName);

		// Token: 0x06003014 RID: 12308 RVA: 0x000D4CCA File Offset: 0x000D3CCA
		protected virtual ICollection GetViewNames()
		{
			return null;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x000D4CCD File Offset: 0x000D3CCD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool HasControls()
		{
			return base.HasControls();
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000D4CD8 File Offset: 0x000D3CD8
		private void OnDataSourceChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[DataSourceControl.EventDataSourceChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000D4D08 File Offset: 0x000D3D08
		private void OnDataSourceChangedInternal(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[DataSourceControl.EventDataSourceChangedInternal];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000D4D36 File Offset: 0x000D3D36
		protected virtual void RaiseDataSourceChangedEvent(EventArgs e)
		{
			this.OnDataSourceChangedInternal(e);
			this.OnDataSourceChanged(e);
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x000D4D46 File Offset: 0x000D3D46
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void RenderControl(HtmlTextWriter writer)
		{
			base.RenderControl(writer);
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x0600301A RID: 12314 RVA: 0x000D4D4F File Offset: 0x000D3D4F
		// (remove) Token: 0x0600301B RID: 12315 RVA: 0x000D4D62 File Offset: 0x000D3D62
		event EventHandler IDataSource.DataSourceChanged
		{
			add
			{
				base.Events.AddHandler(DataSourceControl.EventDataSourceChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataSourceControl.EventDataSourceChanged, value);
			}
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x000D4D75 File Offset: 0x000D3D75
		DataSourceView IDataSource.GetView(string viewName)
		{
			return this.GetView(viewName);
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x000D4D7E File Offset: 0x000D3D7E
		ICollection IDataSource.GetViewNames()
		{
			return this.GetViewNames();
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x000D4D86 File Offset: 0x000D3D86
		bool IListSource.ContainsListCollection
		{
			get
			{
				return !base.DesignMode && ListSourceHelper.ContainsListCollection(this);
			}
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x000D4D98 File Offset: 0x000D3D98
		IList IListSource.GetList()
		{
			if (base.DesignMode)
			{
				return null;
			}
			return ListSourceHelper.GetList(this);
		}

		// Token: 0x04002204 RID: 8708
		private static readonly object EventDataSourceChanged = new object();

		// Token: 0x04002205 RID: 8709
		private static readonly object EventDataSourceChangedInternal = new object();
	}
}
