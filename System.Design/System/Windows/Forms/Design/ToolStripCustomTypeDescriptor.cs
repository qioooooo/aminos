using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class ToolStripCustomTypeDescriptor : CustomTypeDescriptor
	{
		public ToolStripCustomTypeDescriptor(ToolStrip instance)
		{
			this.instance = instance;
		}

		public override object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this.instance;
		}

		public override PropertyDescriptorCollection GetProperties()
		{
			if (this.instance != null && this.collection == null)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.instance);
				PropertyDescriptor[] array = new PropertyDescriptor[properties.Count];
				properties.CopyTo(array, 0);
				this.collection = new PropertyDescriptorCollection(array, false);
			}
			if (this.collection.Count > 0)
			{
				this.propItems = this.collection["Items"];
				if (this.propItems != null)
				{
					this.collection.Remove(this.propItems);
				}
			}
			return this.collection;
		}

		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			if (this.instance != null && this.collection == null)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.instance);
				PropertyDescriptor[] array = new PropertyDescriptor[properties.Count];
				properties.CopyTo(array, 0);
				this.collection = new PropertyDescriptorCollection(array, false);
			}
			if (this.collection.Count > 0)
			{
				this.propItems = this.collection["Items"];
				if (this.propItems != null)
				{
					this.collection.Remove(this.propItems);
				}
			}
			return this.collection;
		}

		private ToolStrip instance;

		private PropertyDescriptor propItems;

		private PropertyDescriptorCollection collection;
	}
}
