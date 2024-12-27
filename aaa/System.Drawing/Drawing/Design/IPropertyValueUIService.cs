using System;
using System.ComponentModel;

namespace System.Drawing.Design
{
	// Token: 0x020000F4 RID: 244
	public interface IPropertyValueUIService
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000D83 RID: 3459
		// (remove) Token: 0x06000D84 RID: 3460
		event EventHandler PropertyUIValueItemsChanged;

		// Token: 0x06000D85 RID: 3461
		void AddPropertyValueUIHandler(PropertyValueUIHandler newHandler);

		// Token: 0x06000D86 RID: 3462
		PropertyValueUIItem[] GetPropertyUIValueItems(ITypeDescriptorContext context, PropertyDescriptor propDesc);

		// Token: 0x06000D87 RID: 3463
		void NotifyPropertyValueUIItemsChanged();

		// Token: 0x06000D88 RID: 3464
		void RemovePropertyValueUIHandler(PropertyValueUIHandler newHandler);
	}
}
