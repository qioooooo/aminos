using System;
using System.Globalization;

namespace System.ComponentModel
{
	// Token: 0x020001A0 RID: 416
	[AttributeUsage(AttributeTargets.All)]
	public class ToolboxItemAttribute : Attribute
	{
		// Token: 0x06000CE7 RID: 3303 RVA: 0x0002A231 File Offset: 0x00029231
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ToolboxItemAttribute.Default);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0002A23E File Offset: 0x0002923E
		public ToolboxItemAttribute(bool defaultType)
		{
			if (defaultType)
			{
				this.toolboxItemTypeName = "System.Drawing.Design.ToolboxItem, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0002A254 File Offset: 0x00029254
		public ToolboxItemAttribute(string toolboxItemTypeName)
		{
			toolboxItemTypeName.ToUpper(CultureInfo.InvariantCulture);
			this.toolboxItemTypeName = toolboxItemTypeName;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0002A26F File Offset: 0x0002926F
		public ToolboxItemAttribute(Type toolboxItemType)
		{
			this.toolboxItemType = toolboxItemType;
			this.toolboxItemTypeName = toolboxItemType.AssemblyQualifiedName;
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x0002A28C File Offset: 0x0002928C
		public Type ToolboxItemType
		{
			get
			{
				if (this.toolboxItemType == null && this.toolboxItemTypeName != null)
				{
					try
					{
						this.toolboxItemType = Type.GetType(this.toolboxItemTypeName, true);
					}
					catch (Exception ex)
					{
						throw new ArgumentException(SR.GetString("ToolboxItemAttributeFailedGetType", new object[] { this.toolboxItemTypeName }), ex);
					}
				}
				return this.toolboxItemType;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0002A2F8 File Offset: 0x000292F8
		public string ToolboxItemTypeName
		{
			get
			{
				if (this.toolboxItemTypeName == null)
				{
					return string.Empty;
				}
				return this.toolboxItemTypeName;
			}
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0002A310 File Offset: 0x00029310
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ToolboxItemAttribute toolboxItemAttribute = obj as ToolboxItemAttribute;
			return toolboxItemAttribute != null && toolboxItemAttribute.ToolboxItemTypeName == this.ToolboxItemTypeName;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0002A340 File Offset: 0x00029340
		public override int GetHashCode()
		{
			if (this.toolboxItemTypeName != null)
			{
				return this.toolboxItemTypeName.GetHashCode();
			}
			return base.GetHashCode();
		}

		// Token: 0x04000EA0 RID: 3744
		private Type toolboxItemType;

		// Token: 0x04000EA1 RID: 3745
		private string toolboxItemTypeName;

		// Token: 0x04000EA2 RID: 3746
		public static readonly ToolboxItemAttribute Default = new ToolboxItemAttribute("System.Drawing.Design.ToolboxItem, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

		// Token: 0x04000EA3 RID: 3747
		public static readonly ToolboxItemAttribute None = new ToolboxItemAttribute(false);
	}
}
