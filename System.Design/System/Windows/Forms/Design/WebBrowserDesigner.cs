using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class WebBrowserDesigner : AxDesigner
	{
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

		public override void Initialize(IComponent c)
		{
			WebBrowser webBrowser = c as WebBrowser;
			this.Url = webBrowser.Url;
			webBrowser.Url = new Uri("about:blank");
			base.Initialize(c);
			webBrowser.Url = null;
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			WebBrowser webBrowser = (WebBrowser)base.Component;
			if (webBrowser != null)
			{
				webBrowser.MinimumSize = new Size(20, 20);
			}
		}

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
