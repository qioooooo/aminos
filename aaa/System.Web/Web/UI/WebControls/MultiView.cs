using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005EE RID: 1518
	[ControlBuilder(typeof(MultiViewControlBuilder))]
	[ToolboxData("<{0}:MultiView runat=\"server\"></{0}:MultiView>")]
	[Designer("System.Web.UI.Design.WebControls.MultiViewDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("ActiveViewChanged")]
	[ParseChildren(typeof(View))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class MultiView : Control
	{
		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06004B17 RID: 19223 RVA: 0x00132781 File Offset: 0x00131781
		// (set) Token: 0x06004B18 RID: 19224 RVA: 0x0013279C File Offset: 0x0013179C
		[WebSysDescription("MultiView_ActiveView")]
		[WebCategory("Behavior")]
		[DefaultValue(-1)]
		public virtual int ActiveViewIndex
		{
			get
			{
				if (this._cachedActiveViewIndex > -1)
				{
					return this._cachedActiveViewIndex;
				}
				return this._activeViewIndex;
			}
			set
			{
				/*
An exception occurred when decompiling this method (06004B18)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.WebControls.MultiView::set_ActiveViewIndex(System.Int32)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.String.Ctor(ReadOnlySpan`1 value)
   at System.Runtime.CompilerServices.DefaultInterpolatedStringHandler.ToStringAndClear()
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 519
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06004B19 RID: 19225 RVA: 0x001328EF File Offset: 0x001318EF
		// (set) Token: 0x06004B1A RID: 19226 RVA: 0x001328F7 File Offset: 0x001318F7
		[Browsable(true)]
		public override bool EnableTheming
		{
			get
			{
				return base.EnableTheming;
			}
			set
			{
				base.EnableTheming = value;
			}
		}

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06004B1B RID: 19227 RVA: 0x00132900 File Offset: 0x00131900
		private bool ShouldTriggerViewEvent
		{
			get
			{
				return this._controlStateApplied || (this.Page != null && !this.Page.IsPostBack);
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06004B1C RID: 19228 RVA: 0x00132924 File Offset: 0x00131924
		[WebSysDescription("MultiView_Views")]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		public virtual ViewCollection Views
		{
			get
			{
				return (ViewCollection)this.Controls;
			}
		}

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06004B1D RID: 19229 RVA: 0x00132931 File Offset: 0x00131931
		// (remove) Token: 0x06004B1E RID: 19230 RVA: 0x00132944 File Offset: 0x00131944
		[WebSysDescription("MultiView_ActiveViewChanged")]
		[WebCategory("Action")]
		public event EventHandler ActiveViewChanged
		{
			add
			{
				base.Events.AddHandler(MultiView._eventActiveViewChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(MultiView._eventActiveViewChanged, value);
			}
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x00132958 File Offset: 0x00131958
		protected override void AddParsedSubObject(object obj)
		{
			if (obj is View)
			{
				this.Controls.Add((Control)obj);
				return;
			}
			if (!(obj is LiteralControl))
			{
				throw new HttpException(SR.GetString("MultiView_cannot_have_children_of_type", new object[] { obj.GetType().Name }));
			}
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x001329AD File Offset: 0x001319AD
		protected override ControlCollection CreateControlCollection()
		{
			return new ViewCollection(this);
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x001329B8 File Offset: 0x001319B8
		public View GetActiveView()
		{
			int activeViewIndex = this.ActiveViewIndex;
			if (activeViewIndex >= this.Views.Count)
			{
				throw new Exception(SR.GetString("MultiView_ActiveViewIndex_out_of_range"));
			}
			if (activeViewIndex < 0)
			{
				return null;
			}
			View view = this.Views[activeViewIndex];
			if (!view.Active)
			{
				this.UpdateActiveView(activeViewIndex);
			}
			return view;
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x00132A0D File Offset: 0x00131A0D
		internal void IgnoreBubbleEvents()
		{
			this._ignoreBubbleEvents = true;
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x00132A18 File Offset: 0x00131A18
		private void UpdateActiveView(int activeViewIndex)
		{
			for (int i = 0; i < this.Views.Count; i++)
			{
				View view = this.Views[i];
				if (