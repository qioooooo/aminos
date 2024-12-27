using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002E0 RID: 736
	internal class WebBrowserDesigner : AxDesigner
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x0009F5BA File Offset: 0x0009E5BA
		// (set) Token: 0x06001C4F RID: 7247 RVA: 0x0009F5D1 File Offset: 0x0009E5D1
		public Uri Url
		{
			get
			{
				return (Uri)base.ShadowProperties["Url"];
			}
			set
			{
				base.ShadowProperties["Url"] = value;
			}
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x0009F5E4 File Offset: 0x0009E5E4
		public override void Initialize(IComponent c)
		{
			WebBrowser webBrowser = c as WebBrowser;
			this.Url = webBrowser.Url;
			webBrowser.Url = new Uri("about:blank");
			base.Initialize(c);
			webBrowser.Url = null;
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x0009F624 File Offset: 0x0009E624
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			WebBrowser webBrowser = (WebBrowser)base.Component;
			if (webBrowser != null)
			{
				webBrowser.MinimumSize = new Size(20, 20);
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x0009F656 File Offset: 0x0009E656
		protected override InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (base.InheritanceAttribute == InheritanceAttribute.Inherited)
				{
					return InheritanceAttribute.InheritedReadOnly;
				}
				return base.InheritanceAttribute;
			}
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x0009F674 File Offset: 0x0009E674
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "Url" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(WebBrowserDesigner), propertyDescriptor, array2);
				}
			}
		}
	}
}
