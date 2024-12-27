using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006F1 RID: 1777
	[Designer("System.Web.UI.Design.WebControls.WebParts.ProxyWebPartManagerDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PersistChildren(false)]
	[NonVisualControl]
	[ParseChildren(true)]
	[Bindable(false)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ProxyWebPartManager : Control
	{
		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x060056EE RID: 22254 RVA: 0x0015E85C File Offset: 0x0015D85C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string ClientID
		{
			get
			{
				return base.ClientID;
			}
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x060056EF RID: 22255 RVA: 0x0015E864 File Offset: 0x0015D864
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x060056F0 RID: 22256 RVA: 0x0015E86C File Offset: 0x0015D86C
		// (set) Token: 0x060056F1 RID: 22257 RVA: 0x0015E870 File Offset: 0x0015D870
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x060056F2 RID: 22258 RVA: 0x0015E8A2 File Offset: 0x0015D8A2
		// (set) Token: 0x060056F3 RID: 22259 RVA: 0x0015E8AC File Offset: 0x0015D8AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue("")]
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

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x060056F4 RID: 22260 RVA: 0x0015E8DE File Offset: 0x0015D8DE
		[MergableProperty(false)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[WebSysDescription("WebPartManager_StaticConnections")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Behavior")]
		public ProxyWebPartConnectionCollection StaticConnections
		{
			get
			{
				if (this._staticConnections == null)
				{
					this._staticConnections = new ProxyWebPartConnectionCollection();
				}
				return this._staticConnections;
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x060056F5 RID: 22261 RVA: 0x0015E8F9 File Offset: 0x0015D8F9
		// (set) Token: 0x060056F6 RID: 22262 RVA: 0x0015E8FC File Offset: 0x0015D8FC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(false)]
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

		// Token: 0x060056F7 RID: 22263 RVA: 0x0015E92E File Offset: 0x0015D92E
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection(this);
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x0015E938 File Offset: 0x0015D938
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Focus()
		{
			throw new NotSupportedException(SR.GetString("NoFocusSupport", new object[] { base.GetType().Name }));
		}

		// Token: 0x060056F9 RID: 22265 RVA: 0x0015E96C File Offset: 0x0015D96C
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Page page = this.Page;
			if (page != null && !base.DesignMode)
			{
				WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(page);
				if (currentWebPartManager == null)
				{
					throw new InvalidOperationException(SR.GetString("WebPartManagerRequired"));
				}
				this.StaticConnections.SetWebPartManager(currentWebPartManager);
			}
		}

		// Token: 0x04002F7A RID: 12154
		private ProxyWebPartConnectionCollection _staticConnections;
	}
}
