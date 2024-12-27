using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000235 RID: 565
	internal class FlowLayoutPanelDesigner : FlowPanelDesigner
	{
		// Token: 0x06001578 RID: 5496 RVA: 0x0006F818 File Offset: 0x0006E818
		public FlowLayoutPanelDesigner()
		{
			this.commonSizes = new ArrayList();
			this.oldP1 = (this.oldP2 = Point.Empty);
			this.insertIndex = FlowLayoutPanelDesigner.InvalidIndex;
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001579 RID: 5497 RVA: 0x0006F862 File Offset: 0x0006E862
		protected override bool AllowGenericDragBox
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x0006F865 File Offset: 0x0006E865
		protected internal override bool AllowSetChildIndexOnDrop
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x0006F868 File Offset: 0x0006E868
		private new FlowLayoutPanel Control
		{
			get
			{
				return base.Control as FlowLayoutPanel;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x0006F875 File Offset: 0x0006E875
		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (base.InheritanceAttribute == InheritanceAttribute.Inherited || base.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
				{
					return InheritanceAttribute.InheritedReadOnly;
				}
				return base.InheritanceAttribute;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600157D RID: 5501 RVA: 0x0006F89D File Offset: 0x0006E89D
		// (set) Token: 0x0600157E RID: 5502 RVA: 0x0006F8AA File Offset: 0x0006E8AA
		private FlowDirection FlowDirection
		{
			get
			{
				return this.Control.FlowDirection;
			}
			set
			{
				if (value != this.Control.FlowDirection)
				{
					base.BehaviorService.Invalidate(base.BehaviorService.ControlRectInAdornerWindow(this.Control));
					this.Control.FlowDirection = value;
				}
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x0600157F RID: 5503 RVA: 0x0006F8E2 File Offset: 0x0006E8E2
		private bool HorizontalFlow
		{
			get
			{
				return this.Control.FlowDirection == FlowDirection.RightToLeft || this.Control.FlowDirection == FlowDirection.LeftToRight;
			}
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0006F904 File Offset: 0x0006E904
		private FlowDirection RTLTranslateFlowDirection(FlowDirection direction)
		{
			if (this.Control.RightToLeft == RightToLeft.No)
			{
				return direction;
			}
			switch (direction)
			{
			case FlowDirection.LeftToRight:
				return FlowDirection.RightToLeft;
			case FlowDirection.TopDown:
			case FlowDirection.BottomUp:
				return direction;
			case FlowDirection.RightToLeft:
				return FlowDirection.LeftToRight;
			default:
				return direction;
			}
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0006F944 File Offset: 0x0006E944
		private Rectangle GetMarginBounds(Control control)
		{
			return new Rectangle(control.Bounds.Left - ((this.Control.RightToLeft == RightToLeft.No) ? control.Margin.Left : control.Margin.Right), control.Bounds.Top - control.Margin.Top, control.Bounds.Width + control.Margin.Horizontal, control.Bounds.Height + control.Margin.Vertical);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0006F9EC File Offset: 0x0006E9EC
		private void CreateMarginBoundsList()
		{
			this.commonSizes.Clear();
			if (this.Control.Controls.Count == 0)
			{
				this.childInfo = new FlowLayoutPanelDesigner.ChildInfo[0];
				return;
			}
			this.childInfo = new FlowLayoutPanelDesigner.ChildInfo[this.Control.Controls.Count];
			Point point = this.Control.PointToScreen(Point.Empty);
			FlowDirection flowDirection = this.RTLTranslateFlowDirection(this.Control.FlowDirection);
			bool horizontalFlow = this.HorizontalFlow;
			int num = int.MaxValue;
			int num2 = -1;
			int num3 = -1;
			if ((horizontalFlow && flowDirection == FlowDirection.RightToLeft) || (!horizontalFlow && flowDirection == FlowDirection.BottomUp))
			{
				num3 = int.MaxValue;
			}
			bool flag = this.Control.RightToLeft == RightToLeft.Yes;
			int i;
			for (i = 0; i < this.Control.Controls.Count; i++)
			{
				Control control = this.Control.Controls[i];
				Rectangle marginBounds = this.GetMarginBounds(control);
				Rectangle bounds = control.Bounds;
				if (horizontalFlow)
				{
					bounds.X -= ((!flag) ? control.Margin.Left : control.Margin.Right);
					bounds.Width += control.Margin.Horizontal;
					bounds.Height--;
				}
				else
				{
					bounds.Y -= control.Margin.Top;
					bounds.Height += control.Margin.Vertical;
					bounds.Width--;
				}
				marginBounds.Offset(point.X, point.Y);
				bounds.Offset(point.X, point.Y);
				this.childInfo[i].marginBounds = marginBounds;
				this.childInfo[i].controlBounds = bounds;
				this.childInfo[i].inSelectionColl = false;
				if (this.dragControls != null && this.dragControls.Contains(control))
				{
					this.childInfo[i].inSelectionColl = true;
				}
				if (horizontalFlow)
				{
					if (((flowDirection == FlowDirection.LeftToRight) ? (marginBounds.X < num3) : (marginBounds.X > num3)) && num > 0 && num2 > 0)
					{
						this.commonSizes.Add(new Rectangle(num, num2, num2 - num, i));
						num = int.MaxValue;
						num2 = -1;
					}
					num3 = marginBounds.X;
					if (marginBounds.Top < num)
					{
						num = marginBounds.Top;
					}
					if (marginBounds.Bottom > num2)
					{
						num2 = marginBounds.Bottom;
					}
				}
				else
				{
					if (((flowDirection == FlowDirection.TopDown) ? (marginBounds.Y < num3) : (marginBounds.Y > num3)) && num > 0 && num2 > 0)
					{
						this.commonSizes.Add(new Rectangle(num, num2, num2 - num, i));
						num = int.MaxValue;
						num2 = -1;
					}
					num3 = marginBounds.Y;
					if (marginBounds.Left < num)
					{
						num = marginBounds.Left;
					}
					if (marginBounds.Right > num2)
					{
						num2 = marginBounds.Right;
					}
				}
			}
			if (num > 0 && num2 > 0)
			{
				this.commonSizes.Add(new Rectangle(num, num2, num2 - num, i));
			}
			int j = 0;
			for (i = 0; i < this.commonSizes.Count; i++)
			{
				while (j < ((Rectangle)this.commonSizes[i]).Height)
				{
					if (horizontalFlow)
					{
						this.childInfo[j].marginBounds.Y = ((Rectangle)this.commonSizes[i]).X;
						this.childInfo[j].marginBounds.Height = ((Rectangle)this.commonSizes[i]).Width;
					}
					else
					{
						this.childInfo[j].marginBounds.X = ((Rectangle)this.commonSizes[i]).X;
						this.childInfo[j].marginBounds.Width = ((Rectangle)this.commonSizes[i]).Width;
					}
					j++;
				}
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0006FE6C File Offset: 0x0006EE6C
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			if (this.InheritanceAttribute == InheritanceAttribute.InheritedReadOnly)
			{
				for (int i = 0; i < this.Control.Controls.Count; i++)
				{
					TypeDescriptor.AddAttributes(this.Control.Controls[i], new Attribute[] { InheritanceAttribute.InheritedReadOnly });
				}
			}
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0006FED0 File Offset: 0x0006EED0
		private void OnChildControlAdded(object sender, ControlEventArgs e)
		{
			/*
An exception occurred when decompiling this method (06001584)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.FlowLayoutPanelDesigner::OnChildControlAdded(System.Object,System.Windows.Forms.ControlEventArgs)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 307
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0006FF80 File Offset: 0x0006EF80
		protected override void OnDragDrop(DragEventArgs de)
		{
			/*
An exception occurred when decompiling this method (06001585)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.FlowLayoutPanelDesigner::OnDragDrop(System.Windows.Forms.DragEventArgs)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 314
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x000704CC File Offset: 0x0006F4CC
		protected override void OnDragLeave(EventArgs e)
		{
			this.EraseIBar();
			this.insertIndex = FlowLayoutPanelDesigner.InvalidIndex;
			this.primaryDragControl = null;
			if (this.dragControls != null)
			{
				this.dragControls.Clear();
			}
			base.OnDragLeave(e);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00070500 File Offset: 0x0006F500
		protected override void OnDragEnter(DragEventArgs de)
		{
			base.OnDragEnter(de);
			this.insertIndex = FlowLayoutPanelDesigner.InvalidIndex;
			this.lastMouseLoc = Point.Empty;
			this.primaryDragControl = null;
			DropSourceBehavior.BehaviorDataObject behaviorDataObject = de.Data as DropSourceBehavior.BehaviorDataObject;
			if (behaviorDataObject != null)
			{
				int num = -1;
				this.dragControls = behaviorDataObject.GetSortedDragControls(ref num);
				this.primaryDragControl = this.dragControls[num] as Control;
			}
			this.CreateMarginBoundsList();
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00070570 File Offset: 0x0006F570
		protected override void OnDragOver(DragEventArgs de)
		{
			/*
An exception occurred when decompiling this method (06001588)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.FlowLayoutPanelDesigner::OnDragOver(System.Windows.Forms.DragEventArgs)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.RemoveRedundantCodeCore(ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 110
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.RemoveRedundantCode(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 67
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 420
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x000709CA File Offset: 0x0006F9CA
		private void EraseIBar()
		{
			this.ReDrawIBar(Point.Empty, Point.Empty);
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x000709DC File Offset: 0x0006F9DC
		private void ReDrawIBar(Point p1, Point p2)
		{
			/*
An exception occurred when decompiling this method (0600158A)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.Design.FlowLayoutPanelDesigner::ReDrawIBar(System.Drawing.Point,System.Drawing.Point)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.BuildGraph(List`1 nodes, ILLabel entryLabel) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 83
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindLoops(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 53
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 343
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x00070E5C File Offset: 0x0006FE5C
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "FlowDirection" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(FlowLayoutPanelDesigner), propertyDescriptor, array2);
				}
			}
		}

		// Token: 0x04001289 RID: 4745
		private const int iBarHalfSize = 2;

		// Token: 0x0400128A RID: 4746
		private const int minIBar = 10;

		// Token: 0x0400128B RID: 4747
		private const int iBarHatHeight = 3;

		// Token: 0x0400128C RID: 4748
		private const int iBarSpace = 2;

		// Token: 0x0400128D RID: 4749
		private const int iBarLineOffset = 5;

		// Token: 0x0400128E RID: 4750
		private const int iBarHatWidth = 5;

		// Token: 0x0400128F RID: 4751
		private FlowLayoutPanelDesigner.ChildInfo[] childInfo;

		// Token: 0x04001290 RID: 4752
		private ArrayList dragControls;

		// Token: 0x04001291 RID: 4753
		private Control primaryDragControl;

		// Token: 0x04001292 RID: 4754
		private ArrayList commonSizes;

		// Token: 0x04001293 RID: 4755
		private int insertIndex;

		// Token: 0x04001294 RID: 4756
		private Point lastMouseLoc;

		// Token: 0x04001295 RID: 4757
		private Point oldP1;

		// Token: 0x04001296 RID: 4758
		private Point oldP2;

		// Token: 0x04001297 RID: 4759
		private static readonly int InvalidIndex = -1;

		// Token: 0x04001298 RID: 4760
		private int maxIBarWidth = Math.Max(2, 2);

		// Token: 0x02000236 RID: 566
		private struct ChildInfo
		{
			// Token: 0x04001299 RID: 4761
			public Rectangle marginBounds;

			// Token: 0x0400129A RID: 4762
			public Rectangle controlBounds;

			// Token: 0x0400129B RID: 4763
			public bool 