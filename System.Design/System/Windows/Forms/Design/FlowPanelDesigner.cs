using System;
using System.Collections;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class FlowPanelDesigner : PanelDesigner
	{
		public override bool ParticipatesWithSnapLines
		{
			get
			{
				return false;
			}
		}

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

		internal override void AddChildControl(Control newChild)
		{
			this.Control.Controls.Add(newChild);
		}

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
