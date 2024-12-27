using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000572 RID: 1394
	[TypeConverter(typeof(EmbeddedMailObject.EmbeddedMailObjectTypeConverter))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class EmbeddedMailObject
	{
		// Token: 0x06004486 RID: 17542 RVA: 0x0011A007 File Offset: 0x00119007
		public EmbeddedMailObject()
		{
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0011A00F File Offset: 0x0011900F
		public EmbeddedMailObject(string name, string path)
		{
			this.Name = name;
			this.Path = path;
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x06004488 RID: 17544 RVA: 0x0011A025 File Offset: 0x00119025
		// (set) Token: 0x06004489 RID: 17545 RVA: 0x0011A03B File Offset: 0x0011903B
		[NotifyParentProperty(true)]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		[WebSysDescription("EmbeddedMailObject_Name")]
		public string Name
		{
			get
			{
				if (this._name == null)
				{
					return string.Empty;
				}
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x0600448A RID: 17546 RVA: 0x0011A044 File Offset: 0x00119044
		// (set) Token: 0x0600448B RID: 17547 RVA: 0x0011A05A File Offset: 0x0011905A
		[DefaultValue("")]
		[WebCategory("Behavior")]
		[UrlProperty]
		[NotifyParentProperty(true)]
		[Editor("System.Web.UI.Design.MailFileEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[WebSysDescription("EmbeddedMailObject_Path")]
		public string Path
		{
			get
			{
				if (this._path != null)
				{
					return this._path;
				}
				return string.Empty;
			}
			set
			{
				this._path = value;
			}
		}

		// Token: 0x040029B5 RID: 10677
		private string _path;

		// Token: 0x040029B6 RID: 10678
		private string _name;

		// Token: 0x02000573 RID: 1395
		private sealed class EmbeddedMailObjectTypeConverter : TypeConverter
		{
			// Token: 0x0600448C RID: 17548 RVA: 0x0011A063 File Offset: 0x00119063
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return "EmbeddedMailObject";
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
