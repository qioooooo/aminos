using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200025A RID: 602
	internal class LabelDesigner : ControlDesigner
	{
		// Token: 0x060016E7 RID: 5863 RVA: 0x00075FFE File Offset: 0x00074FFE
		public LabelDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x00076010 File Offset: 0x00075010
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				ContentAlignment contentAlignment = ContentAlignment.TopLeft;
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				PropertyDescriptor propertyDescriptor;
				if ((propertyDescriptor = properties["TextAlign"]) != null)
				{
					contentAlignment = (ContentAlignment)propertyDescriptor.GetValue(base.Component);
				}
				int num = DesignerUtils.GetTextBaseline(this.Control, contentAlignment);
				if ((propertyDescriptor = properties["AutoSize"]) != null && !(bool)propertyDescriptor.GetValue(base.Component))
				{
					BorderStyle borderStyle = BorderStyle.None;
					if ((propertyDescriptor = properties["BorderStyle"]) != null)
					{
						borderStyle = (BorderStyle)propertyDescriptor.GetValue(base.Component);
					}
					num += this.LabelBaselineOffset(contentAlignment, borderStyle);
				}
				arrayList.Add(new SnapLine(SnapLineType.Baseline, num, SnapLinePriority.Medium));
				Label label = this.Control as Label;
				if (label != null && label.BorderStyle == BorderStyle.None)
				{
					Type type = Type.GetType("System.Windows.Forms.Label");
					if (type != null)
					{
						MethodInfo method = type.GetMethod("GetLeadingTextPaddingFromTextFormatFlags", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method != null)
						{
							int num2 = (int)method.Invoke(base.Component, null);
							bool flag = label.RightToLeft == RightToLeft.Yes;
							for (int i = 0; i < arrayList.Count; i++)
							{
								SnapLine snapLine = arrayList[i] as SnapLine;
								if (snapLine != null && snapLine.SnapLineType == (flag ? SnapLineType.Right : SnapLineType.Left))
								{
									snapLine.AdjustOffset(flag ? (-num2) : num2);
									break;
								}
							}
						}
					}
				}
				return arrayList;
			}
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00076181 File Offset: 0x00075181
		private int LabelBaselineOffset(ContentAlignment alignment, BorderStyle borderStyle)
		{
			if ((alignment & DesignerUtils.anyMiddleAlignment) != (ContentAlignment)0 || (alignment & DesignerUtils.anyTopAlignment) != (ContentAlignment)0)
			{
				if (borderStyle == BorderStyle.None)
				{
					return 0;
				}
				if (borderStyle == BorderStyle.FixedSingle || borderStyle == BorderStyle.Fixed3D)
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (borderStyle == BorderStyle.None)
				{
					return -1;
				}
				if (borderStyle == BorderStyle.FixedSingle || borderStyle == BorderStyle.Fixed3D)
				{
					return 0;
				}
				return 0;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060016EA RID: 5866 RVA: 0x000761B8 File Offset: 0x000751B8
		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				object component = base.Component;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["AutoSize"];
				if (propertyDescriptor != null)
				{
					bool flag = (bool)propertyDescriptor.GetValue(component);
					if (flag)
					{
						selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable);
					}
				}
				return selectionRules;
			}
		}
	}
}
