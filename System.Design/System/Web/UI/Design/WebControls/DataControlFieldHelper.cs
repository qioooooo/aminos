using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal static class DataControlFieldHelper
	{
		internal static IDictionary<Type, DataControlFieldDesigner> GetCustomFieldDesigners(DesignerForm designerForm, DataBoundControl control)
		{
			Dictionary<Type, DataControlFieldDesigner> dictionary = new Dictionary<Type, DataControlFieldDesigner>();
			ITypeDiscoveryService typeDiscoveryService = (ITypeDiscoveryService)control.Site.GetService(typeof(ITypeDiscoveryService));
			if (typeDiscoveryService != null)
			{
				ICollection types = typeDiscoveryService.GetTypes(typeof(DataControlField), false);
				foreach (object obj in types)
				{
					Type type = (Type)obj;
					DesignerAttribute designerAttribute = (DesignerAttribute)Attribute.GetCustomAttribute(type, typeof(DesignerAttribute));
					if (designerAttribute != null)
					{
						Type type2 = Type.GetType(designerAttribute.DesignerTypeName, false, true);
						if (type2 != null && type2.IsSubclassOf(typeof(DataControlFieldDesigner)))
						{
							try
							{
								DataControlFieldDesigner dataControlFieldDesigner = (DataControlFieldDesigner)Activator.CreateInstance(type2);
								if (dataControlFieldDesigner.IsEnabled(control))
								{
									dataControlFieldDesigner.DesignerForm = designerForm;
									dictionary.Add(type, dataControlFieldDesigner);
								}
							}
							catch
							{
							}
						}
					}
				}
			}
			return dictionary;
		}

		internal static ITemplate GetTemplate(DataBoundControl control, string templateContent)
		{
			ITemplate template;
			try
			{
				ISite site = control.Site;
				IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
				if (templateContent != null && templateContent.Length > 0)
				{
					template = ControlParser.ParseTemplate(designerHost, templateContent, null);
				}
				else
				{
					template = null;
				}
			}
			catch (Exception)
			{
				template = null;
			}
			return template;
		}

		internal static TemplateField GetTemplateField(DataControlField dataControlField, DataBoundControl dataBoundControl)
		{
			TemplateField templateField = new TemplateField();
			templateField.HeaderText = dataControlField.HeaderText;
			templateField.HeaderImageUrl = dataControlField.HeaderImageUrl;
			templateField.AccessibleHeaderText = dataControlField.AccessibleHeaderText;
			templateField.FooterText = dataControlField.FooterText;
			templateField.SortExpression = dataControlField.SortExpression;
			templateField.Visible = dataControlField.Visible;
			templateField.InsertVisible = dataControlField.InsertVisible;
			templateField.ShowHeader = dataControlField.ShowHeader;
			templateField.ControlStyle.CopyFrom(dataControlField.ControlStyle);
			templateField.FooterStyle.CopyFrom(dataControlField.FooterStyle);
			templateField.HeaderStyle.CopyFrom(dataControlField.HeaderStyle);
			templateField.ItemStyle.CopyFrom(dataControlField.ItemStyle);
			return templateField;
		}
	}
}
