using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000576 RID: 1398
	internal sealed class ErrorStyle : Style, ICustomTypeDescriptor
	{
		// Token: 0x0600449A RID: 17562 RVA: 0x0011A18E File Offset: 0x0011918E
		public ErrorStyle()
		{
			base.ForeColor = Color.Red;
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x0011A1A1 File Offset: 0x001191A1
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x0011A1AA File Offset: 0x001191AA
		string ICustomTypeDescriptor.GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x0011A1B3 File Offset: 0x001191B3
		string ICustomTypeDescriptor.GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0011A1BC File Offset: 0x001191BC
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x0011A1C5 File Offset: 0x001191C5
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x0011A1CE File Offset: 0x001191CE
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x0011A1D7 File Offset: 0x001191D7
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x0011A1E1 File Offset: 0x001191E1
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x0011A1EA File Offset: 0x001191EA
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x0011A1F4 File Offset: 0x001191F4
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x0011A200 File Offset: 0x00119200
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.GetType(), attributes);
			PropertyDescriptor[] array = new PropertyDescriptor[properties.Count];
			PropertyDescriptor propertyDescriptor = properties["ForeColor"];
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor2 = properties[i];
				if (propertyDescriptor2 == propertyDescriptor)
				{
					array[i] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor2, new Attribute[]
					{
						new DefaultValueAttribute(typeof(Color), "Red")
					});
				}
				else
				{
					array[i] = propertyDescriptor2;
				}
			}
			return new PropertyDescriptorCollection(array, true);
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x0011A291 File Offset: 0x00119291
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}
	}
}
