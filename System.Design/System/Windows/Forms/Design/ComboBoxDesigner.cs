using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class ComboBoxDesigner : ControlDesigner
	{
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

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.propChanged != null)
			{
				((ComboBox)this.Control).StyleChanged -= this.propChanged;
			}
			base.Dispose(disposing);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			this.propChanged = new EventHandler(this.OnControlPropertyChanged);
			((ComboBox)this.Control).StyleChanged += this.propChanged;
		}

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

		private void OnControlPropertyChanged(object sender, EventArgs e)
		{
			if (base.BehaviorService != null)
			{
				base.BehaviorService.SyncSelection();
			}
		}

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

		private EventHandler propChanged;

		private DesignerActionListCollection _actionLists;
	}
}
