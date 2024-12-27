using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000C6 RID: 198
	internal sealed class JSWrappedField : JSField, IWrappedMember
	{
		// Token: 0x060008FF RID: 2303 RVA: 0x00045920 File Offset: 0x00044920
		internal JSWrappedField(FieldInfo field, object obj)
		{
			if (field is JSFieldInfo)
			{
				field = ((JSFieldInfo)field).field;
			}
			this.wrappedField = field;
			this.wrappedObject = obj;
			if (obj is JSObject && !Typeob.JSObject.IsAssignableFrom(field.DeclaringType))
			{
				if (obj is BooleanObject)
				{
					this.wrappedObject = ((BooleanObject)obj).value;
					return;
				}
				if (obj is NumberObject)
				{
					this.wrappedObject = ((NumberObject)obj).value;
					return;
				}
				if (obj is StringObject)
				{
					this.wrappedObject = ((StringObject)obj).value;
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x000459C2 File Offset: 0x000449C2
		public override string Name
		{
			get
			{
				return this.wrappedField.Name;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x000459CF File Offset: 0x000449CF
		public override FieldAttributes Attributes
		{
			get
			{
				return this.wrappedField.Attributes;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x000459DC File Offset: 0x000449DC
		public override Type DeclaringType
		{
			get
			{
				return this.wrappedField.DeclaringType;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x000459E9 File Offset: 0x000449E9
		public override Type FieldType
		{
			get
			{
				return this.wrappedField.FieldType;
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x000459F6 File Offset: 0x000449F6
		internal override string GetClassFullName()
		{
			if (this.wrappedField is JSField)
			{
				return ((JSField)this.wrappedField).GetClassFullName();
			}
			return this.wrappedField.DeclaringType.FullName;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00045A26 File Offset: 0x00044A26
		internal override object GetMetaData()
		{
			if (this.wrappedField is JSField)
			{
				return ((JSField)this.wrappedField).GetMetaData();
			}
			return this.wrappedField;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00045A4C File Offset: 0x00044A4C
		internal override PackageScope GetPackage()
		{
			if (this.wrappedField is JSField)
			{
				return ((JSField)this.wrappedField).GetPackage();
			}
			return null;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00045A6D File Offset: 0x00044A6D
		public override object GetValue(object obj)
		{
			return this.wrappedField.GetValue(this.wrappedObject);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00045A80 File Offset: 0x00044A80
		public object GetWrappedObject()
		{
			return this.wrappedObject;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00045A88 File Offset: 0x00044A88
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo locale)
		{
			this.wrappedField.SetValue(this.wrappedObject, value, invokeAttr, binder, locale);
		}

		// Token: 0x04000558 RID: 1368
		internal FieldInfo wrappedField;

		// Token: 0x04000559 RID: 1369
		internal object wrappedObject;
	}
}
