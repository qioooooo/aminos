using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000A9 RID: 169
	public sealed class JSFieldInfo : FieldInfo
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x000359F3 File Offset: 0x000349F3
		internal JSFieldInfo(FieldInfo field)
		{
			this.field = field;
			this.attributes = field.Attributes;
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x00035A0E File Offset: 0x00034A0E
		public override FieldAttributes Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00035A18 File Offset: 0x00034A18
		public override Type DeclaringType
		{
			get
			{
				Type type = this.declaringType;
				if (type == null)
				{
					type = (this.declaringType = this.field.DeclaringType);
				}
				return type;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x00035A43 File Offset: 0x00034A43
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				return this.field.FieldHandle;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00035A50 File Offset: 0x00034A50
		public override Type FieldType
		{
			get
			{
				Type type = this.fieldType;
				if (type == null)
				{
					type = (this.fieldType = this.field.FieldType);
				}
				return type;
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00035A7B File Offset: 0x00034A7B
		public override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00035A83 File Offset: 0x00034A83
		public override object[] GetCustomAttributes(bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00035A8C File Offset: 0x00034A8C
		public override object GetValue(object obj)
		{
			FieldAccessor fieldAccessor = this.fieldAccessor;
			if (fieldAccessor == null)
			{
				fieldAccessor = (this.fieldAccessor = FieldAccessor.GetAccessorFor(TypeReferences.ToExecutionContext(this.field)));
			}
			return fieldAccessor.GetValue(obj);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00035AC2 File Offset: 0x00034AC2
		public override bool IsDefined(Type type, bool inherit)
		{
			return false;
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00035AC5 File Offset: 0x00034AC5
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00035AC8 File Offset: 0x00034AC8
		public override string Name
		{
			get
			{
				return this.field.Name;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x00035AD5 File Offset: 0x00034AD5
		public override Type ReflectedType
		{
			get
			{
				return this.field.ReflectedType;
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00035AE2 File Offset: 0x00034AE2
		public new void SetValue(object obj, object value)
		{
			if ((this.attributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope)
			{
				throw new JScriptException(JSError.AssignmentToReadOnly);
			}
			this.SetValue(obj, value, BindingFlags.SetField, null, null);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00035B0C File Offset: 0x00034B0C
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			FieldAccessor fieldAccessor = this.fieldAccessor;
			if (fieldAccessor == null)
			{
				fieldAccessor = (this.fieldAccessor = FieldAccessor.GetAccessorFor(this.field));
			}
			fieldAccessor.SetValue(obj, value);
		}

		// Token: 0x0400042C RID: 1068
		internal FieldInfo field;

		// Token: 0x0400042D RID: 1069
		private FieldAttributes attributes;

		// Token: 0x0400042E RID: 1070
		private Type declaringType;

		// Token: 0x0400042F RID: 1071
		private Type fieldType;

		// Token: 0x04000430 RID: 1072
		private FieldAccessor fieldAccessor;
	}
}
