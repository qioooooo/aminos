using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x020000BF RID: 191
	internal class JSProperty : PropertyInfo
	{
		// Token: 0x0600088E RID: 2190 RVA: 0x000412A0 File Offset: 0x000402A0
		internal JSProperty(string name)
		{
			this.name = name;
			this.formal_parameters = null;
			this.getter = null;
			this.setter = null;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x000412C4 File Offset: 0x000402C4
		public override PropertyAttributes Attributes
		{
			get
			{
				return PropertyAttributes.None;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x000412C7 File Offset: 0x000402C7
		public override bool CanRead
		{
			get
			{
				return JSProperty.GetGetMethod(this, true) != null;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x000412D6 File Offset: 0x000402D6
		public override bool CanWrite
		{
			get
			{
				return JSProperty.GetSetMethod(this, true) != null;
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x000412E5 File Offset: 0x000402E5
		internal virtual string GetClassFullName()
		{
			if (this.getter != null)
			{
				return this.getter.GetClassFullName();
			}
			return this.setter.GetClassFullName();
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00041308 File Offset: 0x00040308
		internal bool GetterAndSetterAreConsistent()
		{
			if (this.getter == null || this.setter == null)
			{
				return true;
			}
			((JSFieldMethod)this.getter).func.PartiallyEvaluate();
			((JSFieldMethod)this.setter).func.PartiallyEvaluate();
			ParameterInfo[] parameters = this.getter.GetParameters();
			ParameterInfo[] parameters2 = this.setter.GetParameters();
			int num = parameters.Length;
			int num2 = parameters2.Length;
			if (num != num2 - 1)
			{
				return false;
			}
			if (!((JSFieldMethod)this.getter).func.ReturnType(null).Equals(((ParameterDeclaration)parameters2[num]).type.ToIReflect()))
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (((ParameterDeclaration)parameters[i]).type.ToIReflect() != ((ParameterDeclaration)parameters2[i]).type.ToIReflect())
				{
					return false;
				}
			}
			return (this.getter.Attributes & ~MethodAttributes.Abstract) == (this.setter.Attributes & ~MethodAttributes.Abstract);
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00041408 File Offset: 0x00040408
		public override Type DeclaringType
		{
			get
			{
				if (this.getter != null)
				{
					return this.getter.DeclaringType;
				}
				return this.setter.DeclaringType;
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00041429 File Offset: 0x00040429
		public sealed override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return new object[0];
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00041431 File Offset: 0x00040431
		public sealed override object[] GetCustomAttributes(bool inherit)
		{
			if (this.getter != null)
			{
				return this.getter.GetCustomAttributes(true);
			}
			if (this.setter != null)
			{
				return this.setter.GetCustomAttributes(true);
			}
			return new object[0];
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00041464 File Offset: 0x00040464
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static object GetValue(PropertyInfo prop, object obj, object[] index)
		{
			JSProperty jsproperty = prop as JSProperty;
			if (jsproperty != null)
			{
				return jsproperty.GetValue(obj, BindingFlags.ExactBinding, null, index, null);
			}
			JSWrappedProperty jswrappedProperty = prop as JSWrappedProperty;
			if (jswrappedProperty != null)
			{
				return jswrappedProperty.GetValue(obj, BindingFlags.ExactBinding, null, index, null);
			}
			MethodInfo getMethod = JSProperty.GetGetMethod(prop, false);
			if (getMethod != null)
			{
				try
				{
					return getMethod.Invoke(obj, BindingFlags.ExactBinding, null, index, null);
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
			}
			throw new MissingMethodException();
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x000414E4 File Offset: 0x000404E4
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			MethodInfo methodInfo = this.getter;
			JSObject jsobject = obj as JSObject;
			if (methodInfo == null && jsobject != null)
			{
				methodInfo = jsobject.GetMethod("get_" + this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				JSWrappedMethod jswrappedMethod = methodInfo as JSWrappedMethod;
				if (jswrappedMethod != null)
				{
					methodInfo = jswrappedMethod.method;
				}
			}
			if (methodInfo == null)
			{
				methodInfo = this.GetGetMethod(false);
			}
			if (methodInfo != null)
			{
				try
				{
					return methodInfo.Invoke(obj, invokeAttr, binder, (index == null) ? new object[0] : index, culture);
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
			}
			return Missing.Value;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0004157C File Offset: 0x0004057C
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			if (this.getter != null && (nonPublic || this.getter.IsPublic))
			{
				if (this.setter != null && (nonPublic || this.setter.IsPublic))
				{
					return new MethodInfo[] { this.getter, this.setter };
				}
				return new MethodInfo[] { this.getter };
			}
			else
			{
				if (this.setter != null && (nonPublic || this.setter.IsPublic))
				{
					return new MethodInfo[] { this.setter };
				}
				return new MethodInfo[0];
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00041618 File Offset: 0x00040618
		internal static MethodInfo GetGetMethod(PropertyInfo prop, bool nonPublic)
		{
			if (prop == null)
			{
				return null;
			}
			JSProperty jsproperty = prop as JSProperty;
			if (jsproperty != null)
			{
				return jsproperty.GetGetMethod(nonPublic);
			}
			MethodInfo methodInfo = prop.GetGetMethod(nonPublic);
			if (methodInfo != null)
			{
				return methodInfo;
			}
			Type declaringType = prop.DeclaringType;
			if (declaringType == null)
			{
				return null;
			}
			Type baseType = declaringType.BaseType;
			if (baseType == null)
			{
				return null;
			}
			methodInfo = prop.GetGetMethod(nonPublic);
			if (methodInfo == null)
			{
				return null;
			}
			BindingFlags bindingFlags = BindingFlags.Public;
			if (methodInfo.IsStatic)
			{
				bindingFlags |= BindingFlags.Static | BindingFlags.FlattenHierarchy;
			}
			else
			{
				bindingFlags |= BindingFlags.Instance;
			}
			if (nonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			string text = prop.Name;
			prop = null;
			try
			{
				prop = baseType.GetProperty(text, bindingFlags, null, null, new Type[0], null);
			}
			catch (AmbiguousMatchException)
			{
			}
			if (prop != null)
			{
				return JSProperty.GetGetMethod(prop, nonPublic);
			}
			return null;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x000416D4 File Offset: 0x000406D4
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			if (this.getter == null)
			{
				try
				{
					IReflect superType = ((ClassScope)this.setter.obj).GetSuperType();
					BindingFlags bindingFlags = BindingFlags.Public;
					if (this.setter.IsStatic)
					{
						bindingFlags |= BindingFlags.Static | BindingFlags.FlattenHierarchy;
					}
					else
					{
						bindingFlags |= BindingFlags.Instance;
					}
					if (nonPublic)
					{
						bindingFlags |= BindingFlags.NonPublic;
					}
					PropertyInfo property = superType.GetProperty(this.name, bindingFlags, null, null, new Type[0], null);
					if (property is JSProperty)
					{
						return property.GetGetMethod(nonPublic);
					}
					return JSProperty.GetGetMethod(property, nonPublic);
				}
				catch (AmbiguousMatchException)
				{
				}
			}
			if (nonPublic || this.getter.IsPublic)
			{
				return this.getter;
			}
			return null;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00041784 File Offset: 0x00040784
		public override ParameterInfo[] GetIndexParameters()
		{
			if (this.formal_parameters == null)
			{
				if (this.getter != null)
				{
					this.formal_parameters = this.getter.GetParameters();
				}
				else
				{
					ParameterInfo[] parameters = this.setter.GetParameters();
					int num = parameters.Length;
					if (num <= 1)
					{
						num = 1;
					}
					this.formal_parameters = new ParameterInfo[num - 1];
					for (int i = 0; i < num - 1; i++)
					{
						this.formal_parameters[i] = parameters[i];
					}
				}
			}
			return this.formal_parameters;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x000417F8 File Offset: 0x000407F8
		internal static MethodInfo GetSetMethod(PropertyInfo prop, bool nonPublic)
		{
			if (prop == null)
			{
				return null;
			}
			JSProperty jsproperty = prop as JSProperty;
			if (jsproperty != null)
			{
				return jsproperty.GetSetMethod(nonPublic);
			}
			MethodInfo methodInfo = prop.GetSetMethod(nonPublic);
			if (methodInfo != null)
			{
				return methodInfo;
			}
			Type declaringType = prop.DeclaringType;
			if (declaringType == null)
			{
				return null;
			}
			Type baseType = declaringType.BaseType;
			if (baseType == null)
			{
				return null;
			}
			methodInfo = prop.GetGetMethod(nonPublic);
			if (methodInfo == null)
			{
				return null;
			}
			BindingFlags bindingFlags = BindingFlags.Public;
			if (methodInfo.IsStatic)
			{
				bindingFlags |= BindingFlags.Static | BindingFlags.FlattenHierarchy;
			}
			else
			{
				bindingFlags |= BindingFlags.Instance;
			}
			if (nonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			string text = prop.Name;
			prop = null;
			try
			{
				prop = baseType.GetProperty(text, bindingFlags, null, null, new Type[0], null);
			}
			catch (AmbiguousMatchException)
			{
			}
			if (prop != null)
			{
				return JSProperty.GetSetMethod(prop, nonPublic);
			}
			return null;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x000418B4 File Offset: 0x000408B4
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			if (this.setter == null)
			{
				try
				{
					IReflect superType = ((ClassScope)this.getter.obj).GetSuperType();
					BindingFlags bindingFlags = BindingFlags.Public;
					if (this.getter.IsStatic)
					{
						bindingFlags |= BindingFlags.Static | BindingFlags.FlattenHierarchy;
					}
					else
					{
						bindingFlags |= BindingFlags.Instance;
					}
					if (nonPublic)
					{
						bindingFlags |= BindingFlags.NonPublic;
					}
					PropertyInfo property = superType.GetProperty(this.name, bindingFlags, null, null, new Type[0], null);
					if (property is JSProperty)
					{
						return property.GetSetMethod(nonPublic);
					}
					return JSProperty.GetSetMethod(property, nonPublic);
				}
				catch (AmbiguousMatchException)
				{
				}
			}
			if (nonPublic || this.setter.IsPublic)
			{
				return this.setter;
			}
			return null;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00041964 File Offset: 0x00040964
		public sealed override bool IsDefined(Type type, bool inherit)
		{
			return false;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00041967 File Offset: 0x00040967
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0004196B File Offset: 0x0004096B
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00041974 File Offset: 0x00040974
		internal IReflect PropertyIR()
		{
			if (this.getter is JSFieldMethod)
			{
				return ((JSFieldMethod)this.getter).ReturnIR();
			}
			if (this.setter != null)
			{
				ParameterInfo[] parameters = this.setter.GetParameters();
				if (parameters.Length > 0)
				{
					ParameterInfo parameterInfo = parameters[parameters.Length - 1];
					if (parameterInfo is ParameterDeclaration)
					{
						return ((ParameterDeclaration)parameterInfo).ParameterIReflect;
					}
					return parameterInfo.ParameterType;
				}
			}
			return Typeob.Void;
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x000419E4 File Offset: 0x000409E4
		public override Type PropertyType
		{
			get
			{
				if (this.getter != null)
				{
					return this.getter.ReturnType;
				}
				if (this.setter != null)
				{
					ParameterInfo[] parameters = this.setter.GetParameters();
					if (parameters.Length > 0)
					{
						return parameters[parameters.Length - 1].ParameterType;
					}
				}
				return Typeob.Void;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00041A31 File Offset: 0x00040A31
		public override Type ReflectedType
		{
			get
			{
				if (this.getter != null)
				{
					return this.getter.ReflectedType;
				}
				return this.setter.ReflectedType;
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00041A54 File Offset: 0x00040A54
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static void SetValue(PropertyInfo prop, object obj, object value, object[] index)
		{
			JSProperty jsproperty = prop as JSProperty;
			if (jsproperty != null)
			{
				jsproperty.SetValue(obj, value, BindingFlags.ExactBinding, null, index, null);
				return;
			}
			MethodInfo setMethod = JSProperty.GetSetMethod(prop, false);
			if (setMethod != null)
			{
				int num = ((index == null) ? 0 : index.Length);
				object[] array = new object[num + 1];
				if (num > 0)
				{
					ArrayObject.Copy(index, 0, array, 0, num);
				}
				array[num] = value;
				setMethod.Invoke(obj, BindingFlags.ExactBinding, null, array, null);
				return;
			}
			throw new MissingMethodException();
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00041AC4 File Offset: 0x00040AC4
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			MethodInfo methodInfo = this.setter;
			JSObject jsobject = obj as JSObject;
			if (methodInfo == null && jsobject != null)
			{
				methodInfo = jsobject.GetMethod("set_" + this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				JSWrappedMethod jswrappedMethod = methodInfo as JSWrappedMethod;
				if (jswrappedMethod != null)
				{
					methodInfo = jswrappedMethod.method;
				}
			}
			if (methodInfo == null)
			{
				methodInfo = this.GetSetMethod(false);
			}
			if (methodInfo != null)
			{
				if (index == null || index.Length == 0)
				{
					methodInfo.Invoke(obj, invokeAttr, binder, new object[] { value }, culture);
					return;
				}
				int num = index.Length;
				object[] array = new object[num + 1];
				ArrayObject.Copy(index, 0, array, 0, num);
				array[num] = value;
				methodInfo.Invoke(obj, invokeAttr, binder, array, culture);
			}
		}

		// Token: 0x040004A3 RID: 1187
		private string name;

		// Token: 0x040004A4 RID: 1188
		private ParameterInfo[] formal_parameters;

		// Token: 0x040004A5 RID: 1189
		internal PropertyBuilder metaData;

		// Token: 0x040004A6 RID: 1190
		internal JSMethod getter;

		// Token: 0x040004A7 RID: 1191
		internal JSMethod setter;
	}
}
