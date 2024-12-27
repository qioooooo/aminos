using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x0200009E RID: 158
	internal class JSWrappedProperty : PropertyInfo, IWrappedMember
	{
		// Token: 0x0600070D RID: 1805 RVA: 0x0003113C File Offset: 0x0003013C
		internal JSWrappedProperty(PropertyInfo property, object obj)
		{
			this.obj = obj;
			this.property = property;
			if (obj is JSObject)
			{
				Type declaringType = property.DeclaringType;
				if (declaringType == Typeob.Object || declaringType == Typeob.String || declaringType.IsPrimitive || declaringType == Typeob.Array)
				{
					if (obj is BooleanObject)
					{
						this.obj = ((BooleanObject)obj).value;
						return;
					}
					if (obj is NumberObject)
					{
						this.obj = ((NumberObject)obj).value;
						return;
					}
					if (obj is StringObject)
					{
						this.obj = ((StringObject)obj).value;
						return;
					}
					if (obj is ArrayWrapper)
					{
						this.obj = ((ArrayWrapper)obj).value;
					}
				}
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000311FB File Offset: 0x000301FB
		internal virtual string GetClassFullName()
		{
			if (this.property is JSProperty)
			{
				return ((JSProperty)this.property).GetClassFullName();
			}
			return this.property.DeclaringType.FullName;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0003122B File Offset: 0x0003022B
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00031230 File Offset: 0x00030230
		public override string Name
		{
			get
			{
				if (this.obj is LenientGlobalObject && this.property.Name.StartsWith("Slow", StringComparison.Ordinal))
				{
					return this.property.Name.Substring(4);
				}
				return this.property.Name;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0003127F File Offset: 0x0003027F
		public override Type DeclaringType
		{
			get
			{
				return this.property.DeclaringType;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x0003128C File Offset: 0x0003028C
		public override Type ReflectedType
		{
			get
			{
				return this.property.ReflectedType;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00031299 File Offset: 0x00030299
		public override PropertyAttributes Attributes
		{
			get
			{
				return this.property.Attributes;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x000312A6 File Offset: 0x000302A6
		public override bool CanRead
		{
			get
			{
				return this.property.CanRead;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x000312B3 File Offset: 0x000302B3
		public override bool CanWrite
		{
			get
			{
				return this.property.CanWrite;
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000312C0 File Offset: 0x000302C0
		public override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this.property, t, inherit);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000312CF File Offset: 0x000302CF
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.property.GetCustomAttributes(inherit);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000312DD File Offset: 0x000302DD
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			return this.property.GetValue(this.obj, invokeAttr, binder, index, culture);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x000312F6 File Offset: 0x000302F6
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			this.property.SetValue(this.obj, value, invokeAttr, binder, index, culture);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00031311 File Offset: 0x00030311
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			return this.property.GetAccessors(nonPublic);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00031320 File Offset: 0x00030320
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			MethodInfo getMethod = JSProperty.GetGetMethod(this.property, nonPublic);
			if (getMethod == null)
			{
				return null;
			}
			return new JSWrappedMethod(getMethod, this.obj);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0003134B File Offset: 0x0003034B
		public override ParameterInfo[] GetIndexParameters()
		{
			return this.property.GetIndexParameters();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00031358 File Offset: 0x00030358
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			MethodInfo setMethod = JSProperty.GetSetMethod(this.property, nonPublic);
			if (setMethod == null)
			{
				return null;
			}
			return new JSWrappedMethod(setMethod, this.obj);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00031383 File Offset: 0x00030383
		public object GetWrappedObject()
		{
			return this.obj;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0003138B File Offset: 0x0003038B
		public override Type PropertyType
		{
			get
			{
				return this.property.PropertyType;
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00031398 File Offset: 0x00030398
		public override bool IsDefined(Type type, bool inherit)
		{
			return CustomAttribute.IsDefined(this.property, type, inherit);
		}

		// Token: 0x0400031F RID: 799
		internal object obj;

		// Token: 0x04000320 RID: 800
		internal PropertyInfo property;
	}
}
