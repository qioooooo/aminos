﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class UpDownBaseDesigner : ControlDesigner
	{
		public UpDownBaseDesigner()
		{
			base.AutoResizeHandles = true;
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				return selectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
			}
		}

		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				int num = DesignerUtils.GetTextBaseline(this.Control, ContentAlignment.TopLeft);
				BorderStyle borderStyle = BorderStyle.Fixed3D;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["BorderStyle"];
				if (propertyDescriptor != null)
				{
					borderStyle = (BorderStyle)propertyDescriptor.GetValue(base.Component);
				}
				if (borderStyle == BorderStyle.None)
				{
					num--;
				}
				else
				{
					num += 2;
				}
				arrayList.Add(new SnapLine(SnapLineType.Baseline, num, SnapLinePriority.Medium));
				return arrayList;
			}
		}
	}
}