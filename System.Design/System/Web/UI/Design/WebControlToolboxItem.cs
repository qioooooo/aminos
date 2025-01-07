using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[Serializable]
	public class WebControlToolboxItem : ToolboxItem
	{
		public WebControlToolboxItem()
		{
		}

		public WebControlToolboxItem(Type type)
			: base(type)
		{
			this.BuildMetadataCache(type);
		}

		protected WebControlToolboxItem(SerializationInfo info, StreamingContext context)
		{
			this.Deserialize(info, context);
		}

		private void BuildMetadataCache(Type type)
		{
			this.toolData = WebControlToolboxItem.ExtractToolboxData(type);
			this.persistChildren = WebControlToolboxItem.ExtractPersistChildrenAttribute(type);
		}

		protected override IComponent[] CreateComponentsCore(IDesignerHost host)
		{
			throw new Exception(SR.GetString("Toolbox_OnWebformsPage"));
		}

		protected override void Deserialize(SerializationInfo info, StreamingContext context)
		{
			base.Deserialize(info, context);
			this.toolData = info.GetString("ToolData");
			this.persistChildren = info.GetInt32("PersistChildren");
		}

		private static int ExtractPersistChildrenAttribute(Type type)
		{
			if (type != null)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(PersistChildrenAttribute), true);
				if (customAttributes != null && customAttributes.Length == 1)
				{
					PersistChildrenAttribute persistChildrenAttribute = (PersistChildrenAttribute)customAttributes[0];
					if (!persistChildrenAttribute.Persist)
					{
						return 0;
					}
					return 1;
				}
			}
			if (!PersistChildrenAttribute.Default.Persist)
			{
				return 0;
			}
			return 1;
		}

		private static string ExtractToolboxData(Type type)
		{
			string text = string.Empty;
			if (type != null)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(ToolboxDataAttribute), false);
				if (customAttributes != null && customAttributes.Length == 1)
				{
					ToolboxDataAttribute toolboxDataAttribute = (ToolboxDataAttribute)customAttributes[0];
					text = toolboxDataAttribute.Data;
				}
				else
				{
					string name = type.Name;
					text = string.Concat(new string[] { "<{0}:", name, " runat=\"server\"></{0}:", name, ">" });
				}
			}
			return text;
		}

		public object GetToolAttributeValue(IDesignerHost host, Type attributeType)
		{
			if (attributeType == typeof(PersistChildrenAttribute))
			{
				if (this.persistChildren == -1)
				{
					Type toolType = this.GetToolType(host);
					this.persistChildren = WebControlToolboxItem.ExtractPersistChildrenAttribute(toolType);
				}
				return this.persistChildren == 1;
			}
			throw new ArgumentException(SR.GetString("Toolbox_BadAttributeType"));
		}

		public string GetToolHtml(IDesignerHost host)
		{
			if (this.toolData != null)
			{
				return this.toolData;
			}
			Type toolType = this.GetToolType(host);
			this.toolData = WebControlToolboxItem.ExtractToolboxData(toolType);
			return this.toolData;
		}

		public Type GetToolType(IDesignerHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			return this.GetType(host, base.AssemblyName, base.TypeName, true);
		}

		public override void Initialize(Type type)
		{
			base.Initialize(type);
			this.BuildMetadataCache(type);
		}

		protected override void Serialize(SerializationInfo info, StreamingContext context)
		{
			base.Serialize(info, context);
			info.AddValue("ToolData", this.toolData);
			info.AddValue("PersistChildren", this.persistChildren);
		}

		private string toolData;

		private int persistChildren = -1;
	}
}
