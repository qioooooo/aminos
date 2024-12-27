using System;
using System.Collections;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000234 RID: 564
	internal class FlowPanelDesigner : PanelDesigner
	{
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x0006F6EE File Offset: 0x0006E6EE
		public override bool ParticipatesWithSnapLines
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x0006F6F4 File Offset: 0x0006E6F4
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = (ArrayList)base.SnapLines;
				ArrayList arrayList2 = new ArrayList(4);
				foreach (object obj in arrayList)
				{
					SnapLine snapLine = (SnapLine)obj;
					if (snapLine.Filter != null && snapLine.Filter.Contains("Padding"))
					{
						arrayList2.Add(snapLine);
					}
				}
				foreach (object obj2 in arrayList2)
				{
					SnapLine snapLine2 = (SnapLine)obj2;
					arrayList.Remove(snapLine2);
				}
				return arrayList;
			}
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0006F7C8 File Offset: 0x0006E7C8
		internal override void AddChildControl(Control newChild)
		{
			this.Control.Controls.Add(newChild);
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0006F7DC File Offset: 0x0006E7DC
		protected override void OnDragDrop(DragEventArgs de)
		{
			base.OnDragDrop(de);
			SelectionManager selectionManager = this.GetService(typeof(SelectionManager)) as SelectionManager;
			if (selectionManager != null)
			{
				selectionManager.Refresh();
			}
		}
	}
}
