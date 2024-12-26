using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000C0 RID: 192
	internal sealed class JSPropertyField : JSField
	{
		// Token: 0x060008A7 RID: 2215 RVA: 0x00041B73 File Offset: 0x00040B73
		internal JSPropertyField(PropertyInfo field, object obj)
		{
			this.wrappedProperty = field;
			this.wrappedObject = obj;
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00041B89 File Offset: 0x00040B89
		public override string Name
		{
			get
			{
				return this.wrappedProperty.Name;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x00041B96 File Offset: 0x00040B96
		public override FieldAttributes Attributes
		{
			get
			{
				return FieldAttributes.Public;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00041B99 File Offset: 0x00040B99
		public override Type DeclaringType
		{
			get
			{
				return this.wrappedProperty.DeclaringType;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x00041BA6 File Offset: 0x00040BA6
		public override Type FieldType
		{
			get
			{
				return this.wrappedProperty.PropertyType;
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00041BB3 File Offset: 0x00040BB3
		public override object GetValue(object obj)
		{
			return this.wrappedProperty.GetValue(this.wrappedObject, new object[0]);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00041BCC File Offset: 0x00040BCC
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo locale)
		{
			this.wrappedProperty.SetValue(this.wrappedObject, value, invokeAttr, binder, new object[0], locale);
		}

		// Token: 0x040004A8 RID: 1192
		internal PropertyInfo wrappedProperty;

		// Token: 0x040004A9 RID: 1193
		internal object wrappedObject;
	}
}
