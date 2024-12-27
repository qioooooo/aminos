using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200020D RID: 525
	internal class DesignBindingPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x060013C8 RID: 5064 RVA: 0x00064DCC File Offset: 0x00063DCC
		internal DesignBindingPropertyDescriptor(PropertyDescriptor property, Attribute[] attrs, bool readOnly)
			: base(property.Name, attrs)
		{
			this.property = property;
			this.readOnly = readOnly;
			if (base.AttributeArray != null && base.AttributeArray.Length > 0)
			{
				Attribute[] array = new Attribute[this.AttributeArray.Length + 2];
				this.AttributeArray.CopyTo(array, 0);
				array[this.AttributeArray.Length - 1] = NotifyParentPropertyAttribute.Yes;
				array[this.AttributeArray.Length] = RefreshPropertiesAttribute.Repaint;
				base.AttributeArray = array;
				return;
			}
			base.AttributeArray = new Attribute[]
			{
				NotifyParentPropertyAttribute.Yes,
				RefreshPropertiesAttribute.Repaint
			};
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x00064E6A File Offset: 0x00063E6A
		public override Type ComponentType
		{
			get
			{
				return typeof(ControlBindingsCollection);
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x00064E76 File Offset: 0x00063E76
		public override TypeConverter Converter
		{
			get
			{
				return DesignBindingPropertyDescriptor.designBindingConverter;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x00064E7D File Offset: 0x00063E7D
		public override bool IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x00064E85 File Offset: 0x00063E85
		public override Type PropertyType
		{
			get
			{
				return typeof(DesignBinding);
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00064E91 File Offset: 0x00063E91
		public override bool CanResetValue(object component)
		{
			return !DesignBindingPropertyDescriptor.GetBinding((ControlBindingsCollection)component, this.property).IsNull;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00064EAC File Offset: 0x00063EAC
		public override object GetValue(object component)
		{
			return DesignBindingPropertyDescriptor.GetBinding((ControlBindingsCollection)component, this.property);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00064EBF File Offset: 0x00063EBF
		public override void ResetValue(object component)
		{
			DesignBindingPropertyDescriptor.SetBinding((ControlBindingsCollection)component, this.property, DesignBinding.Null);
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00064ED7 File Offset: 0x00063ED7
		public override void SetValue(object component, object value)
		{
			DesignBindingPropertyDescriptor.SetBinding((ControlBindingsCollection)component, this.property, (DesignBinding)value);
			this.OnValueChanged(component, EventArgs.Empty);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00064EFC File Offset: 0x00063EFC
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00064F00 File Offset: 0x00063F00
		private static void SetBinding(ControlBindingsCollection bindings, PropertyDescriptor property, DesignBinding designBinding)
		{
			if (designBinding == null)
			{
				return;
			}
			Binding binding = bindings[property.Name];
			if (binding != null)
			{
				bindings.Remove(binding);
			}
			if (!designBinding.IsNull)
			{
				bindings.Add(property.Name, designBinding.DataSource, designBinding.DataMember);
			}
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00064F4C File Offset: 0x00063F4C
		private static DesignBinding GetBinding(ControlBindingsCollection bindings, PropertyDescriptor property)
		{
			Binding binding = bindings[property.Name];
			if (binding == null)
			{
				return DesignBinding.Null;
			}
			return new DesignBinding(binding.DataSource, binding.BindingMemberInfo.BindingMember);
		}

		// Token: 0x040011CC RID: 4556
		private static TypeConverter designBindingConverter = new DesignBindingConverter();

		// Token: 0x040011CD RID: 4557
		private PropertyDescriptor property;

		// Token: 0x040011CE RID: 4558
		private bool readOnly;
	}
}
