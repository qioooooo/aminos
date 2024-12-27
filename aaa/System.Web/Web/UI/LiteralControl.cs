using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000420 RID: 1056
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class LiteralControl : Control, ITextControl
	{
		// Token: 0x060032E2 RID: 13026 RVA: 0x000DD864 File Offset: 0x000DC864
		public LiteralControl()
		{
			base.PreventAutoID();
			base.SetEnableViewStateInternal(false);
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000DD879 File Offset: 0x000DC879
		public LiteralControl(string text)
			: this()
		{
			this._text = ((text != null) ? text : string.Empty);
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x060032E4 RID: 13028 RVA: 0x000DD892 File Offset: 0x000DC892
		// (set) Token: 0x060032E5 RID: 13029 RVA: 0x000DD89A File Offset: 0x000DC89A
		public virtual string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = ((value != null) ? value : string.Empty);
			}
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x000DD8AD File Offset: 0x000DC8AD
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection(this);
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x000DD8B5 File Offset: 0x000DC8B5
		protected internal override void Render(HtmlTextWriter output)
		{
			output.Write(this._text);
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x000DD8C3 File Offset: 0x000DC8C3
		internal override void InitRecursive(Control namingContainer)
		{
			this.ResolveAdapter();
			if (this._adapter != null)
			{
				this._adapter.OnInit(EventArgs.Empty);
				return;
			}
			this.OnInit(EventArgs.Empty);
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x000DD8F0 File Offset: 0x000DC8F0
		internal override void LoadRecursive()
		{
			if (this._adapter != null)
			{
				this._adapter.OnLoad(EventArgs.Empty);
				return;
			}
			this.OnLoad(EventArgs.Empty);
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x000DD916 File Offset: 0x000DC916
		internal override void PreRenderRecursiveInternal()
		{
			if (this._adapter != null)
			{
				this._adapter.OnPreRender(EventArgs.Empty);
				return;
			}
			this.OnPreRender(EventArgs.Empty);
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x000DD93C File Offset: 0x000DC93C
		internal override void UnloadRecursive(bool dispose)
		{
			if (this._adapter != null)
			{
				this._adapter.OnUnload(EventArgs.Empty);
			}
			else
			{
				this.OnUnload(EventArgs.Empty);
			}
			if (dispose)
			{
				this.Dispose();
			}
		}

		// Token: 0x040023D5 RID: 9173
		internal string _text;
	}
}
