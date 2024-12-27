using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x02000392 RID: 914
	public class TemplateDefinition : DesignerObject
	{
		// Token: 0x060021B5 RID: 8629 RVA: 0x000BA53C File Offset: 0x000B953C
		public TemplateDefinition(ControlDesigner designer, string name, object templatedObject, string templatePropertyName)
			: this(designer, name, templatedObject, templatePropertyName, false)
		{
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000BA54A File Offset: 0x000B954A
		public TemplateDefinition(ControlDesigner designer, string name, object templatedObject, string templatePropertyName, Style style)
			: this(designer, name, templatedObject, templatePropertyName, style, false)
		{
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000BA55A File Offset: 0x000B955A
		public TemplateDefinition(ControlDesigner designer, string name, object templatedObject, string templatePropertyName, bool serverControlsOnly)
			: this(designer, name, templatedObject, templatePropertyName, null, serverControlsOnly)
		{
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000BA56C File Offset: 0x000B956C
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

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x000BA5C6 File Offset: 0x000B95C6
		public virtual bool AllowEditing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060021BA RID: 8634 RVA: 0x000BA5CC File Offset: 0x000B95CC
		// (set) Token: 0x060021BB RID: 8635 RVA: 0x000BA610 File Offset: 0x000B9610
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

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060021BC RID: 8636 RVA: 0x000BA64D File Offset: 0x000B964D
		public bool ServerControlsOnly
		{
			get
			{
				return this._serverControlsOnly;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060021BD RID: 8637 RVA: 0x000BA655 File Offset: 0x000B9655
		// (set) Token: 0x060021BE RID: 8638 RVA: 0x000BA65D File Offset: 0x000B965D
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

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x000BA666 File Offset: 0x000B9666
		public Style Style
		{
			get
			{
				return this._style;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x000BA66E File Offset: 0x000B966E
		public object TemplatedObject
		{
			get
			{
				return this._templatedObject;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x000BA678 File Offset: 0x000B9678
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

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x000BA6F4 File Offset: 0x000B96F4
		public string TemplatePropertyName
		{
			get
			{
				return this._templatePropertyName;
			}
		}

		// Token: 0x0400181D RID: 6173
		private Style _style;

		// Token: 0x0400181E RID: 6174
		private string _templatePropertyName;

		// Token: 0x0400181F RID: 6175
		private object _templatedObject;

		// Token: 0x04001820 RID: 6176
		private PropertyDescriptor _templateProperty;

		// Token: 0x04001821 RID: 6177
		private bool _serverControlsOnly;

		// Token: 0x04001822 RID: 6178
		private bool _supportsDataBinding;
	}
}
