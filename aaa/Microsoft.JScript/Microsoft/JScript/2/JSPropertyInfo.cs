using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000C1 RID: 193
	internal class JSPropertyInfo : PropertyInfo
	{
		// Token: 0x060008AE RID: 2222 RVA: 0x00041BEB File Offset: 0x00040BEB
		internal JSPropertyInfo(PropertyInfo property)
		{
			this.property = property;
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00041BFA File Offset: 0x00040BFA
		public override PropertyAttributes Attributes
		{
			get
			{
				return this.property.Attributes;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00041C07 File Offset: 0x00040C07
		public override bool CanRead
		{
			get
			{
				return this.property.CanRead;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00041C14 File Offset: 0x00040C14
		public override bool CanWrite
		{
			get
			{
				return this.property.CanWrite;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00041C24 File Offset: 0x00040C24
		public override Type DeclaringType
		{
			get
			{
				Type type = this.declaringType;
				if (type == null)
				{
					type = (this.declaringType = this.property.DeclaringType);
				}
				return type;
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00041C50 File Offset: 0x00040C50
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			MethodInfo methodInfo = this.getter;
			if (methodInfo == null)
			{
				methodInfo = this.property.GetGetMethod(nonPublic);
				if (methodInfo != null)
				{
					methodInfo = new JSMethodInfo(methodInfo);
				}
				this.getter = methodInfo;
			}
			return methodInfo;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00041C88 File Offset: 0x00040C88
		public override ParameterInfo[] GetIndexParameters()
		{
			MethodInfo getMethod = this.GetGetMethod(false);
			if (getMethod != null)
			{
				return getMethod.GetParameters();
			}
			return this.property.GetIndexParameters();
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00041CB4 File Offset: 0x00040CB4
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			MethodInfo methodInfo = this.setter;
			if (methodInfo == null)
			{
				methodInfo = this.property.GetSetMethod(nonPublic);
				if (methodInfo != null)
				{
					methodInfo = new JSMethodInfo(methodInfo);
				}
				this.setter = methodInfo;
			}
			return methodInfo;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00041CEA File Offset: 0x00040CEA
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00041CF3 File Offset: 0x00040CF3
		public override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this.property, t, inherit);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00041D02 File Offset: 0x00040D02
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.property.GetCustomAttributes(inherit);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00041D10 File Offset: 0x00040D10
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			return this.GetGetMethod(false).Invoke(obj, invokeAttr, binder, (index == null) ? new object[0] : index, culture);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00041D31 File Offset: 0x00040D31
		public override bool IsDefined(Type type, bool inherit)
		{
			return CustomAttribute.IsDefined(this.property, type, inherit);
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00041D40 File Offset: 0x00040D40
		public override string Name
		{
			get
			{
				return this.property.Name;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00041D4D File Offset: 0x00040D4D
		public override Type PropertyType
		{
			get
			{
				return this.property.PropertyType;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00041D5A File Offset: 0x00040D5A
		public override Type ReflectedType
		{
			get
			{
				return this.property.ReflectedType;
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00041D68 File Offset: 0x00040D68
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			if (index == null || index.Length == 0)
			{
				this.GetSetMethod(false).Invoke(obj, invokeAttr, binder, new object[] { value }, culture);
				return;
			}
			int num = index.Length;
			object[] array = new object[num + 1];
			ArrayObject.Copy(index, 0, array, 0, num);
			array[num] = value;
			this.GetSetMethod(false).Invoke(obj, invokeAttr, binder, array, culture);
		}

		// Token: 0x040004AA RID: 1194
		private PropertyInfo property;

		// Token: 0x040004AB RID: 1195
		private Type declaringType;

		// Token: 0x040004AC RID: 1196
		internal MethodInfo getter;

		// Token: 0x040004AD RID: 1197
		internal MethodInfo setter;
	}
}
