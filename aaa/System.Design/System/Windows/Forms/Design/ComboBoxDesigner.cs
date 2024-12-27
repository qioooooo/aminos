using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001AC RID: 428
	internal class ComboBoxDesigner : ControlDesigner
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x0004AC30 File Offset: 0x00049C30
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				int num = DesignerUtils.GetTextBaseline(this.Control, ContentAlignment.TopLeft);
				num += 3;
				arrayList.Add(new SnapLine(SnapLineType.Baseline, num, SnapLinePriority.Medium));
				return arrayList;
			}
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0004AC6A File Offset: 0x00049C6A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.propChanged != null)
			{
				((ComboBox)this.Control).StyleChanged -= this.propChanged;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0004AC94 File Offset: 0x00049C94
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			this.propChanged = new EventHandler(this.OnControlPropertyChanged);
			((ComboBox)this.Control).StyleChanged += this.propChanged;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0004ACCC File Offset: 0x00049CCC
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			((ComboBox)base.Component).FormattingEnabled = true;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
			{
				propertyDescriptor.SetValue(base.Component, "");
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0004AD3D File Offset: 0x00049D3D
		private void OnControlPropertyChanged(object sender, EventArgs e)
		{
			if (base.BehaviorService != null)
			{
				base.BehaviorService.SyncSelection();
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x0004AD54 File Offset: 0x00049D54
		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				object component = base.Component;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["DropDownStyle"];
				if (propertyDescriptor != null)
				{
					ComboBoxStyle comboBoxStyle = (ComboBoxStyle)propertyDescriptor.GetValue(component);
					if (comboBoxStyle == ComboBoxStyle.DropDown || comboBoxStyle == ComboBoxStyle.DropDownList)
					{
						selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
					}
				}
				return selectionRules;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001066 RID: 4198 RVA: 0x0004AD9E File Offset: 0x00049D9E
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					this._actionLists.Add(new ListControlBoundActionList(this));
				}
				return this._actionLists;
			}
		}

		// Token: 0x0400105C RID: 4188
		private EventHandler propChanged;

		// Token: 0x0400105D RID: 4189
		private DesignerActionListCollection _actionLists;
	}
}
