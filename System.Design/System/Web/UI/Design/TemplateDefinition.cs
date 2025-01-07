using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	public class TemplateDefinition : DesignerObject
	{
		public TemplateDefinition(ControlDesigner designer, string name, object templatedObject, string templatePropertyName)
			: this(designer, name, templatedObject, templatePropertyName, false)
		{
		}

		public TemplateDefinition(ControlDesigner designer, string name, object templatedObject, string templatePropertyName, Style style)
			: this(designer, name, templatedObject, templatePropertyName, style, false)
		{
		}

		public TemplateDefinition(ControlDesigner designer, string name, object templatedObject, string templatePropertyName, bool serverControlsOnly)
			: this(designer, name, templatedObject, templatePropertyName, null, serverControlsOnly)
		{
		}

		public TemplateDefinition(ControlDesigner designer, string name, object templatedObject, string templatePropertyName, Style style, bool serverControlsOnly)
			: base(designer, name)
		{
			if (templatePropertyName == null || templatePropertyName.Length == 0)
			{
				throw new ArgumentNullException("templatePropertyName");
			}
			if (templatedObject == null)
			{
				throw new ArgumentNullException("templatedObject");
			}
			this._serverControlsOnly = serverControlsOnly;
			this._style = style;
			this._templatedObject = templatedObject;
			this._templatePropertyName = templatePropertyName;
		}

		public virtual bool AllowEditing
		{
			get
			{
				return true;
			}
		}

		public virtual string Content
		{
			get
			{
				ITemplate template = (ITemplate)this.TemplateProperty.GetValue(this.TemplatedObject);
				IDesignerHost designerHost = (IDesignerHost)base.GetService(typeof(IDesignerHost));
				return ControlPersister.PersistTemplate(template, designerHost);
			}
			set
			{
				IDesignerHost designerHost = (IDesignerHost)base.GetService(typeof(IDesignerHost));
				ITemplate template = ControlParser.ParseTemplate(designerHost, value);
				this.TemplateProperty.SetValue(this.TemplatedObject, template);
			}
		}

		public bool ServerControlsOnly
		{
			get
			{
				return this._serverControlsOnly;
			}
		}

		public bool SupportsDataBinding
		{
			get
			{
				return this._supportsDataBinding;
			}
			set
			{
				this._supportsDataBinding = value;
			}
		}

		public Style Style
		{
			get
			{
				return this._style;
			}
		}

		public object TemplatedObject
		{
			get
			{
				return this._templatedObject;
			}
		}

		private PropertyDescriptor TemplateProperty
		{
			get
			{
				if (this._templateProperty == null)
				{
					this._templateProperty = TypeDescriptor.GetProperties(this.TemplatedObject)[this.TemplatePropertyName];
					if (this._templateProperty == null || !typeof(ITemplate).IsAssignableFrom(this._templateProperty.PropertyType))
					{
						throw new InvalidOperationException(SR.GetString("TemplateDefinition_InvalidTemplateProperty", new object[] { this.TemplatePropertyName }));
					}
				}
				return this._templateProperty;
			}
		}

		public string TemplatePropertyName
		{
			get
			{
				return this._templatePropertyName;
			}
		}

		private Style _style;

		private string _templatePropertyName;

		private object _templatedObject;

		private PropertyDescriptor _templateProperty;

		private bool _serverControlsOnly;

		private bool _supportsDataBinding;
	}
}
