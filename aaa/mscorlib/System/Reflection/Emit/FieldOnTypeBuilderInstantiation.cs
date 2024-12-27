using System;
using System.Collections;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x0200083D RID: 2109
	internal sealed class FieldOnTypeBuilderInstantiation : FieldInfo
	{
		// Token: 0x06004DDA RID: 19930 RVA: 0x0010F3D0 File Offset: 0x0010E3D0
		internal static FieldInfo GetField(FieldInfo Field, TypeBuilderInstantiation type)
		{
			FieldOnTypeBuilderInstantiation.Entry entry = new FieldOnTypeBuilderInstantiation.Entry(Field, type);
			if (FieldOnTypeBuilderInstantiation.m_hashtable.Contains(entry))
			{
				return FieldOnTypeBuilderInstantiation.m_hashtable[entry] as FieldInfo;
			}
			FieldInfo fieldInfo = new FieldOnTypeBuilderInstantiation(Field, type);
			FieldOnTypeBuilderInstantiation.m_hashtable[entry] = fieldInfo;
			return fieldInfo;
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x0010F428 File Offset: 0x0010E428
		internal FieldOnTypeBuilderInstantiation(FieldInfo field, TypeBuilderInstantiation type)
		{
			this.m_field = field;
			this.m_type = type;
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06004DDC RID: 19932 RVA: 0x0010F43E File Offset: 0x0010E43E
		internal FieldInfo FieldInfo
		{
			get
			{
				return this.m_field;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06004DDD RID: 19933 RVA: 0x0010F446 File Offset: 0x0010E446
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06004DDE RID: 19934 RVA: 0x0010F449 File Offset: 0x0010E449
		public override string Name
		{
			get
			{
				return this.m_field.Name;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06004DDF RID: 19935 RVA: 0x0010F456 File Offset: 0x0010E456
		public override Type DeclaringType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x0010F45E File Offset: 0x0010E45E
		public override Type ReflectedType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x0010F466 File Offset: 0x0010E466
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_field.GetCustomAttributes(inherit);
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x0010F474 File Offset: 0x0010E474
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_field.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x0010F483 File Offset: 0x0010E483
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_field.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x0010F492 File Offset: 0x0010E492
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06004DE5 RID: 19941 RVA: 0x0010F499 File Offset: 0x0010E499
		public override Module Module
		{
			get
			{
				return this.m_field.Module;
			}
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0010F4A6 File Offset: 0x0010E4A6
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0010F4AE File Offset: 0x0010E4AE
		public override Type[] GetRequiredCustomModifiers()
		{
			return this.m_field.GetRequiredCustomModifiers();
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0010F4BB File Offset: 0x0010E4BB
		public override Type[] GetOptionalCustomModifiers()
		{
			return this.m_field.GetOptionalCustomModifiers();
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0010F4C8 File Offset: 0x0010E4C8
		public override void SetValueDirect(TypedReference obj, object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x0010F4CF File Offset: 0x0010E4CF
		public override object GetValueDirect(TypedReference obj)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06004DEB RID: 19947 RVA: 0x0010F4D6 File Offset: 0x0010E4D6
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06004DEC RID: 19948 RVA: 0x0010F4DD File Offset: 0x0010E4DD
		public override Type FieldType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x0010F4E4 File Offset: 0x0010E4E4
		public override object GetValue(object obj)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x0010F4EB File Offset: 0x0010E4EB
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06004DEF RID: 19951 RVA: 0x0010F4F2 File Offset: 0x0010E4F2
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_field.Attributes;
			}
		}

		// Token: 0x04002804 RID: 10244
		private static Hashtable m_hashtable = new Hashtable();

		// Token: 0x04002805 RID: 10245
		private FieldInfo m_field;

		// Token: 0x04002806 RID: 10246
		private TypeBuilderInstantiation m_type;

		// Token: 0x0200083E RID: 2110
		private struct Entry
		{
			// Token: 0x06004DF1 RID: 19953 RVA: 0x0010F50B File Offset: 0x0010E50B
			public Entry(FieldInfo Field, TypeBuilderInstantiation type)
			{
				this.m_field = Field;
				this.m_type = type;
			}

			// Token: 0x06004DF2 RID: 19954 RVA: 0x0010F51B File Offset: 0x0010E51B
			public override int GetHashCode()
			{
				return this.m_field.GetHashCode();
			}

			// Token: 0x06004DF3 RID: 19955 RVA: 0x0010F528 File Offset: 0x0010E528
			public override bool Equals(object o)
			{
				return o is FieldOnTypeBuilderInstantiation.Entry && this.Equals((FieldOnTypeBuilderInstantiation.Entry)o);
			}

			// Token: 0x06004DF4 RID: 19956 RVA: 0x0010F540 File Offset: 0x0010E540
			public bool Equals(FieldOnTypeBuilderInstantiation.Entry obj)
			{
				return obj.m_field == this.m_field && obj.m_type == this.m_type;
			}

			// Token: 0x04002807 RID: 10247
			public FieldInfo m_field;

			// Token: 0x04002808 RID: 10248
			public TypeBuilderInstantiation m_type;
		}
	}
}
