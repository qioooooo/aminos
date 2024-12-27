using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003BB RID: 955
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[Serializable]
	public class WebControlToolboxItem : ToolboxItem
	{
		// Token: 0x0600232E RID: 9006 RVA: 0x000BE47C File Offset: 0x000BD47C
		public WebControlToolboxItem()
		{
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x000BE48B File Offset: 0x000BD48B
		public WebControlToolboxItem(Type type)
			: base(type)
		{
			this.BuildMetadataCache(type);
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x000BE4A2 File Offset: 0x000BD4A2
		protected WebControlToolboxItem(SerializationInfo info, StreamingContext context)
		{
			this.Deserialize(info, context);
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x000BE4B9 File Offset: 0x000BD4B9
		private void BuildMetadataCache(Type type)
		{
			this.toolData = WebControlToolboxItem.ExtractToolboxData(type);
			this.persistChildren = WebControlToolboxItem.ExtractPersistChildrenAttribute(type);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x000BE4D3 File Offset: 0x000BD4D3
		protected override IComponent[] CreateComponentsCore(IDesignerHost host)
		{
			throw new Exception(SR.GetString("Toolbox_OnWebformsPage"));
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x000BE4E4 File Offset: 0x000BD4E4
		protected override void Deserialize(SerializationInfo info, StreamingContext context)
		{
			base.Deserialize(info, context);
			this.toolData = info.GetString("ToolData");
			this.persistChildren = info.GetInt32("PersistChildren");
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x000BE510 File Offset: 0x000BD510
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

		// Token: 0x06002335 RID: 9013 RVA: 0x000BE560 File Offset: 0x000BD560
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

		// Token: 0x06002336 RID: 9014 RVA: 0x000BE5E0 File Offset: 0x000BD5E0
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

		// Token: 0x06002337 RID: 9015 RVA: 0x000BE63C File Offset: 0x000BD63C
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

		// Token: 0x06002338 RID: 9016 RVA: 0x000BE672 File Offset: 0x000BD672
		public Type GetToolType(IDesignerHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			return this.GetType(host, base.AssemblyName, base.TypeName, true);
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000BE696 File Offset: 0x000BD696
		public override void Initialize(Type type)
		{
			base.Initialize(type);
			this.BuildMetadataCache(type);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000BE6A6 File Offset: 0x000BD6A6
		protected override void Serialize(SerializationInfo info, StreamingContext context)
		{
			base.Serialize(info, context);
			info.AddValue("ToolData", this.toolData);
			info.AddValue("PersistChildren", this.persistChildren);
		}

		// Token: 0x04001891 RID: 6289
		private string toolData;

		// Token: 0x04001892 RID: 6290
		private int persistChildren = -1;
	}
}
