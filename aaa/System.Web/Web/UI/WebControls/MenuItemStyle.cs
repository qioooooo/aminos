using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005EA RID: 1514
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class MenuItemStyle : Style
	{
		// Token: 0x06004AF3 RID: 19187 RVA: 0x00132092 File Offset: 0x00131092
		public MenuItemStyle()
		{
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x0013209A File Offset: 0x0013109A
		public MenuItemStyle(StateBag bag)
			: base(bag)
		{
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06004AF5 RID: 19189 RVA: 0x001320A3 File Offset: 0x001310A3
		// (set) Token: 0x06004AF6 RID: 19190 RVA: 0x001320D0 File Offset: 0x001310D0
		[DefaultValue(typeof(Unit), "")]
		[WebSysDescription("MenuItemStyle_HorizontalPadding")]
		[WebCategory("Layout")]
		[NotifyParentProperty(true)]
		public Unit HorizontalPadding
		{
			get
			{
				if (base.IsSet(131072))
				{
					return (Unit)base.ViewState["HorizontalPadding"];
				}
				return Unit.Empty;
			}
			set
			{
				if (value.Type == UnitType.Percentage || value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["HorizontalPadding"] = value;
				this.SetBit(131072);
			}
		}

		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06004AF7 RID: 19191 RVA: 0x00132125 File Offset: 0x00131125
		internal HyperLinkStyle HyperLinkStyle
		{
			get
			{
				if (this._hyperLinkStyle == null)
				{
					this._hyperLinkStyle = new HyperLinkStyle(this);
				}
				return this._hyperLinkStyle;
			}
		}

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06004AF8 RID: 19192 RVA: 0x00132141 File Offset: 0x00131141
		// (set) Token: 0x06004AF9 RID: 19193 RVA: 0x0013216C File Offset: 0x0013116C
		[WebCategory("Layout")]
		[DefaultValue(typeof(Unit), "")]
		[WebSysDescription("MenuItemStyle_ItemSpacing")]
		[NotifyParentProperty(true)]
		public Unit ItemSpacing
		{
			get
			{
				if (base.IsSet(262144))
				{
					return (Unit)base.ViewState["ItemSpacing"];
				}
				return Unit.Empty;
			}
			set
			{
				if (value.Type == UnitType.Percentage || value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["ItemSpacing"] = value;
				this.SetBit(262144);
			}
		}

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06004AFA RID: 19194 RVA: 0x001321C1 File Offset: 0x001311C1
		// (set) Token: 0x06004AFB RID: 19195 RVA: 0x001321EC File Offset: 0x001311EC
		[DefaultValue(typeof(Unit), "")]
		[WebSysDescription("MenuItemStyle_VerticalPadding")]
		[WebCategory("Layout")]
		[NotifyParentProperty(true)]
		public Unit VerticalPadding
		{
			get
			{
				if (base.IsSet(65536))
				{
					return (Unit)base.ViewState["VerticalPadding"];
				}
				return Unit.Empty;
			}
			set
			{
				if (value.Type == UnitType.Percentage || value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["VerticalPadding"] = value;
				this.SetBit(65536);
			}
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x00132244 File Offset: 0x00131244
		public override void CopyFrom(Style s)
		{
			/*
An exception occurred when decompiling this method (06004AFC)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.WebControls.MenuItemStyle::CopyFrom(System.Web.UI.WebControls.Style)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.List`1.set_Capacity(Int32 value)
   at System.Collections.Generic.List`1.AddRange(IEnumerable`1 collection)
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 959
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x00132320 File Offset: 0x00131320
		protected override void FillStyleAttributes(CssStyleCollection attributes, IUrlResolutionService urlResolver)
		{
			/*
An exception occurred when decompiling this method (06004AFD)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.WebControls.MenuItemStyle::FillStyleAttributes(System.Web.UI.CssStyleCollection,System.Web.UI.IUrlResolutionService)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 254
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 293
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 293
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 150
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x00132524 File Offset: 0x00131524
		public override void MergeWith(Style s)
		{
			if (s != null)
			{
				if (this.IsEmpty)
				{
					this.CopyFrom(s);
					return;
				}
				base.MergeWith(s);
				MenuItemStyle menuItemStyle = s as MenuItemStyle;
				if (menuItemStyle != null && !menuItemStyle.IsEmpty)
				{
					if (s.RegisteredCssClass.Length == 0)
					{
						if (menuItemStyle.IsSet(65536) && !base.IsSet(65536))
						{
							this.VerticalPadding = menuItemStyle.VerticalPadding;
						}
						if (menuItemStyle.IsSet(131072) && !base.IsSet(131072))
						{
							this.HorizontalPadding = menuItemStyle.HorizontalPadding;
						}
					}
					if (menuItemStyle.IsSet(262144) && !base.IsSet(262144))
					{
						this.ItemSpacing = menuItemStyle.ItemSpacing;
					}
				}
			}
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x001325E4 File Offset: 0x001315E4
		public override void Reset()
		{
			if (base.IsSet(65536))
			{
				base.ViewState.Remove("VerticalPadding");
			}
			if (base.IsSet(131072))
			{
				base.ViewState.Remove("HorizontalPadding");
			}
			if (base.IsSet(262144))
			{
				base.ViewState.Remove("ItemSpacing");
			}
			this.ResetCachedStyles();
			base.Reset();
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x00132654 File Offset: 0x00131654
		internal void ResetCachedStyles()
		{
			this._hyperLinkStyle = null;
		}

		// Token: 0x04002B93 RID: 11155
		private const int PROP_VPADDING = 65536;

		// Token: 0x04002B94 RID: 11156
		private const int PROP_HPADDING = 131072;

		// Token: 0x04002B95 RID: 11157
		private const int PROP_ITEMSPACING = 262144;

		// Token: 0x04002B96 RID: 11158
		private HyperLinkStyle _hyperLinkStyle;
	}
}
