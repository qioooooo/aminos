using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020003C5 RID: 965
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ControlPropertyNameConverter : StringConverter
	{
		// Token: 0x06002F41 RID: 12097 RVA: 0x000D2C6C File Offset: 0x000D1C6C
		private string[] GetPropertyNames(Control control)
		{
			ArrayList arrayList = new ArrayList();
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(control.GetType());
			foreach (object obj in properties)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				arrayList.Add(propertyDescriptor.Name);
			}
			arrayList.Sort(Comparer.Default);
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000D2CFC File Offset: 0x000D1CFC
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (context == null)
			{
				return null;
			}
			ControlParameter controlParameter = (ControlParameter)context.Instance;
			string controlID = controlParameter.ControlID;
			if (string.IsNullOrEmpty(controlID))
			{
				return null;
			}
			IDesignerHost designerHost = (IDesignerHost)context.GetService(typeof(IDesignerHost));
			if (designerHost == null)
			{
				return null;
			}
			ComponentCollection components = designerHost.Container.Components;
			Control control = components[controlID] as Control;
			if (control == null)
			{
				return null;
			}
			string[] propertyNames = this.GetPropertyNames(control);
			return new TypeConverter.StandardValuesCollection(propertyNames);
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000D2D77 File Offset: 0x000D1D77
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000D2D7A File Offset: 0x000D1D7A
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return context != null;
		}
	}
}
