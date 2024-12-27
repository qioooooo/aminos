using System;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020002A5 RID: 677
	public abstract class LayoutEngine
	{
		// Token: 0x06002576 RID: 9590 RVA: 0x0005798C File Offset: 0x0005698C
		internal IArrangedElement CastToArrangedElement(object obj)
		{
			IArrangedElement arrangedElement = obj as IArrangedElement;
			if (obj == null)
			{
				throw new NotSupportedException(SR.GetString("LayoutEngineUnsupportedType", new object[] { obj.GetType() }));
			}
			return arrangedElement;
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000579C5 File Offset: 0x000569C5
		internal virtual Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
		{
			return Size.Empty;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000579CC File Offset: 0x000569CC
		public virtual void InitLayout(object child, BoundsSpecified specified)
		{
			this.InitLayoutCore(this.CastToArrangedElement(child), specified);
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000579DC File Offset: 0x000569DC
		internal virtual void InitLayoutCore(IArrangedElement element, BoundsSpecified bounds)
		{
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000579DE File Offset: 0x000569DE
		internal virtual void ProcessSuspendedLayoutEventArgs(IArrangedElement container, LayoutEventArgs args)
		{
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000579E0 File Offset: 0x000569E0
		public virtual bool Layout(object container, LayoutEventArgs layoutEventArgs)
		{
			return this.LayoutCore(this.CastToArrangedElement(container), layoutEventArgs);
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000579FD File Offset: 0x000569FD
		internal virtual bool LayoutCore(IArrangedElement container, LayoutEventArgs layoutEventArgs)
		{
			return false;
		}
	}
}
