using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002BB RID: 699
	internal class ToolStripCustomTypeDescriptor : CustomTypeDescriptor
	{
		// Token: 0x06001A41 RID: 6721 RVA: 0x0008E79F File Offset: 0x0008D79F
		public ToolStripCustomTypeDescriptor(ToolStrip instance)
		{
			this.instance = instance;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0008E7AE File Offset: 0x0008D7AE
		public override object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this.instance;
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0008E7B8 File Offset: 0x0008D7B8
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

		// Token: 0x06001A44 RID: 6724 RVA: 0x0008E848 File Offset: 0x0008D848
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

		// Token: 0x04001501 RID: 5377
		private ToolStrip instance;

		// Token: 0x04001502 RID: 5378
		private PropertyDescriptor propItems;

		// Token: 0x04001503 RID: 5379
		private PropertyDescriptorCollection collection;
	}
}
