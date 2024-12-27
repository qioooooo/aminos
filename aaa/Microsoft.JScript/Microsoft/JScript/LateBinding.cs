using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000C9 RID: 201
	public sealed class LateBinding
	{
		// Token: 0x0600091D RID: 2333 RVA: 0x00045E5C File Offset: 0x00044E5C
		public LateBinding(string name)
			: this(name, null, false)
		{
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00045E67 File Offset: 0x00044E67
		public LateBinding(string name, object obj)
			: this(name, obj, false)
		{
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00045E72 File Offset: 0x00044E72
		internal LateBinding(string name, object obj, bool checkForDebugger)
		{
			this.last_ir = null;
			this.last_member = null;
			this.last_members = null;
			this.last_object = null;
			this.name = name;
			this.obj = obj;
			this.checkForDebugger = checkForDebugger;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00045EAC File Offset: 0x00044EAC
		internal MemberInfo BindToMember()
		{
			if (this.obj == this.last_object && this.last_member != null)
			{
				return this.last_member;
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
			object obj = this.obj;
			Type type = obj.GetType();
			TypeReflector typeReflectorFor = TypeReflector.GetTypeReflectorFor(type);
			IReflect reflect;
			if (typeReflectorFor.Is__ComObject())
			{
				if (!this.checkForDebugger)
				{
					return null;
				}
				IDebuggerObject debuggerObject = obj as IDebuggerObject;
				if (debuggerObject == null)
				{
					return null;
				}
				if (debuggerObject.IsCOMObject())
				{
					return null;
				}
				reflect = (IReflect)obj;
			}
			else if (typeReflectorFor.ImplementsIReflect())
			{
				reflect = obj as ScriptObject;
				if (reflect != null)
				{
					if (obj is ClassScope)
					{
						bindingFlags = BindingFlags.Static | BindingFlags.Public;
					}
				}
				else
				{
					reflect = obj as Type;
					if (reflect != null)
					{
						bindingFlags = BindingFlags.Static | BindingFlags.Public;
					}
					else
					{
						reflect = (IReflect)obj;
					}
				}
			}
			else
			{
				reflect = typeReflectorFor;
			}
			this.last_object = this.obj;
			this.last_ir = reflect;
			MemberInfo[] array = (this.last_members = reflect.GetMember(this.name, bindingFlags));
			this.last_member = LateBinding.SelectMember(array);
			if (this.obj is Type)
			{
				MemberInfo[] member = typeof(Type).GetMember(this.name, BindingFlags.Instance | BindingFlags.Public);
				int num;
				if (member != null && (num = member.Length) > 0)
				{
					int num2;
					if (array == null || (num2 = array.Length) == 0)
					{
						this.last_member = LateBinding.SelectMember(this.last_members = member);
					}
					else
					{
						MemberInfo[] array2 = new MemberInfo[num + num2];
						ArrayObject.Copy(array, 0, array2, 0, num2);
						ArrayObject.Copy(member, 0, array2, num2, num);
						this.last_member = LateBinding.SelectMember(this.last_members = array2);
					}
				}
			}
			return this.last_member;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0004604C File Offset: 0x0004504C
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object Call(object[] arguments, bool construct, bool brackets, VsaEngine engine)
		{
			object obj;
			try
			{
				if (this.name == null)
				{
					obj = LateBinding.CallValue(this.obj, arguments, construct, brackets, engine, ((IActivationObject)engine.ScriptObjectStackTop()).GetDefaultThisObject(), JSBinder.ob, null, null);
				}
				else
				{
					obj = this.Call(JSBinder.ob, arguments, null, null, null, construct, brackets, engine);
				}
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return obj;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x000460BC File Offset: 0x000450BC
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal object Call(Binder binder, object[] arguments, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters, bool construct, bool brackets, VsaEngine engine)
		{
			MemberInfo memberInfo = this.BindToMember();
			if (this.obj is ScriptObject || this.obj is GlobalObject)
			{
				if (this.obj is WithObject)
				{
					object contained_object = ((WithObject)this.obj).contained_object;
					if (!(contained_object is ScriptObject))
					{
						IReflect irforObjectThatRequiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(contained_object, VsaEngine.executeForJSEE);
						if (irforObjectThatRequiresInvokeMember != null)
						{
							return LateBinding.CallCOMObject(irforObjectThatRequiresInvokeMember, this.name, contained_object, binder, arguments, modifiers, culture, namedParameters, construct, brackets, engine);
						}
					}
				}
				if (memberInfo is FieldInfo)
				{
					return LateBinding.CallValue(((FieldInfo)memberInfo).GetValue(this.obj), arguments, construct, brackets, engine, this.obj, JSBinder.ob, null, null);
				}
				if (memberInfo is PropertyInfo && !(memberInfo is JSProperty))
				{
					if (!brackets)
					{
						JSWrappedPropertyAndMethod jswrappedPropertyAndMethod = memberInfo as JSWrappedPropertyAndMethod;
						if (jswrappedPropertyAndMethod != null)
						{
							BindingFlags bindingFlags = ((arguments == null || arguments.Length == 0) ? BindingFlags.InvokeMethod : (BindingFlags.InvokeMethod | BindingFlags.GetProperty));
							return jswrappedPropertyAndMethod.Invoke(this.obj, bindingFlags, JSBinder.ob, arguments, null);
						}
					}
					return LateBinding.CallValue(JSProperty.GetValue((PropertyInfo)memberInfo, this.obj, null), arguments, construct, brackets, engine, this.obj, JSBinder.ob, null, null);
				}
				if (memberInfo is MethodInfo)
				{
					if (memberInfo is JSMethod)
					{
						if (construct)
						{
							return ((JSMethod)memberInfo).Construct(arguments);
						}
						return ((JSMethod)memberInfo).Invoke(this.obj, this.obj, BindingFlags.Default, JSBinder.ob, arguments, null);
					}
					else
					{
						Type declaringType = memberInfo.DeclaringType;
						if (declaringType == typeof(object))
						{
							return LateBinding.CallMethod((MethodInfo)memberInfo, arguments, this.obj, binder, culture, namedParameters);
						}
						if (declaringType == typeof(string))
						{
							return LateBinding.CallMethod((MethodInfo)memberInfo, arguments, Convert.ToString(this.obj), binder, culture, namedParameters);
						}
						if (Convert.IsPrimitiveNumericType(declaringType))
						{
							return LateBinding.CallMethod((MethodInfo)memberInfo, arguments, Convert.CoerceT(this.obj, declaringType), binder, culture, namedParameters);
						}
						if (declaringType == typeof(bool))
						{
							return LateBinding.CallMethod((MethodInfo)memberInfo, arguments, Convert.ToBoolean(this.obj), binder, culture, namedParameters);
						}
						if (declaringType == typeof(StringObject) || declaringType == typeof(BooleanObject) || declaringType == typeof(NumberObject) || brackets)
						{
							return LateBinding.CallMethod((MethodInfo)memberInfo, arguments, Convert.ToObject(this.obj, engine), binder, culture, namedParameters);
						}
						if (declaringType == typeof(GlobalObject) && ((MethodInfo)memberInfo).IsSpecialName)
						{
							return LateBinding.CallValue(((MethodInfo)memberInfo).Invoke(this.obj, null), arguments, construct, false, engine, this.obj, JSBinder.ob, null, null);
						}
						if (!(this.obj is ClassScope))
						{
							if (CustomAttribute.IsDefined(memberInfo, typeof(JSFunctionAttribute), false))
							{
								FieldInfo fieldInfo = LateBinding.SelectMember(this.last_members) as FieldInfo;
								if (fieldInfo != null)
								{
									object value = this.obj;
									if (!(value is Closure))
									{
										value = fieldInfo.GetValue(this.obj);
									}
									return LateBinding.CallValue(value, arguments, construct, brackets, engine, this.obj, JSBinder.ob, null, null);
								}
							}
							return LateBinding.CallValue(new BuiltinFunction(this.obj, (MethodInfo)memberInfo), arguments, construct, false, engine, this.obj, JSBinder.ob, null, null);
						}
					}
				}
			}
			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null)
			{
				return LateBinding.CallMethod(methodInfo, arguments, this.obj, binder, culture, namedParameters);
			}
			JSConstructor jsconstructor = memberInfo as JSConstructor;
			if (jsconstructor != null)
			{
				return LateBinding.CallValue(jsconstructor.cons, arguments, construct, brackets, engine, this.obj, JSBinder.ob, null, null);
			}
			if (memberInfo is Type)
			{
				return LateBinding.CallValue(memberInfo, arguments, construct, brackets, engine, this.obj, JSBinder.ob, null, null);
			}
			if (memberInfo is ConstructorInfo)
			{
				return LateBinding.CallOneOfTheMembers(new MemberInfo[] { this.last_member }, arguments, true, this.obj, binder, culture, namedParameters, engine);
			}
			if (!construct && memberInfo is PropertyInfo)
			{
				if (memberInfo is COMPropertyInfo)
				{
					return ((PropertyInfo)memberInfo).GetValue(this.obj, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.OptionalParamBinding, binder, arguments, culture);
				}
				if (((PropertyInfo)memberInfo).GetIndexParameters().Length == 0)
				{
					Type propertyType = ((PropertyInfo)memberInfo).PropertyType;
					if (propertyType == typeof(object))
					{
						MethodInfo getMethod = JSProperty.GetGetMethod((PropertyInfo)memberInfo, false);
						if (getMethod != null)
						{
							object obj = getMethod.Invoke(this.obj, null);
							return LateBinding.CallValue(obj, arguments, construct, brackets, engine, this.obj, JSBinder.ob, null, null);
						}
					}
					MemberInfo[] defaultMembers = TypeReflector.GetTypeReflectorFor(propertyType).GetDefaultMembers();
					if (defaultMembers != null && defaultMembers.Length > 0)
					{
						MethodInfo getMethod2 = JSProperty.GetGetMethod((PropertyInfo)memberInfo, false);
						if (getMethod2 != null)
						{
							object obj2 = getMethod2.Invoke(this.obj, null);
							return LateBinding.CallOneOfTheMembers(defaultMembers, arguments, false, obj2, binder, culture, namedParameters, engine);
						}
					}
				}
			}
			if (this.last_members != null && this.last_members.Length > 0)
			{
				bool flag;
				object obj3 = LateBinding.CallOneOfTheMembers(this.last_members, arguments, construct, this.obj, binder, culture, namedParameters, engine, out flag);
				if (flag)
				{
					return obj3;
				}
			}
			IReflect irforObjectThatRequiresInvokeMember2 = LateBinding.GetIRForObjectThatRequiresInvokeMember(this.obj, VsaEngine.executeForJSEE);
			if (irforObjectThatRequiresInvokeMember2 != null)
			{
				return LateBinding.CallCOMObject(irforObjectThatRequiresInvokeMember2, this.name, this.obj, binder, arguments, modifiers, culture, namedParameters, construct, brackets, engine);
			}
			object memberValue = LateBinding.GetMemberValue(this.obj, this.name, this.last_member, this.last_members);
			if (!(memberValue is Missing))
			{
				return LateBinding.CallValue(memberValue, arguments, construct, brackets, engine, this.obj, JSBinder.ob, null, null);
			}
			if (!brackets)
			{
				throw new JScriptException(JSError.FunctionExpected);
			}
			if (this.obj is IActivationObject)
			{
				throw new JScriptException(JSError.ObjectExpected);
			}
			throw new JScriptException(JSError.OLENoPropOrMethod);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00046684 File Offset: 0x00045684
		[DebuggerHidden]
		[DebuggerStepThrough]
		private static object CallCOMObject(IReflect ir, string name, object ob, Binder binder, object[] arguments, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters, bool construct, bool brackets, VsaEngine engine)
		{
			object obj;
			try
			{
				try
				{
					LateBinding.Change64bitIntegersToDouble(arguments);
					BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.OptionalParamBinding;
					if (construct)
					{
						obj = ir.InvokeMember(name, bindingFlags | BindingFlags.CreateInstance, binder, ob, arguments, modifiers, culture, namedParameters);
					}
					else
					{
						if (brackets)
						{
							try
							{
								return ir.InvokeMember(name, bindingFlags | BindingFlags.GetProperty | BindingFlags.GetField, binder, ob, arguments, modifiers, culture, namedParameters);
							}
							catch (TargetInvocationException)
							{
								object obj2 = ir.InvokeMember(name, bindingFlags | BindingFlags.GetProperty | BindingFlags.GetField, binder, ob, new object[0], modifiers, culture, new string[0]);
								return LateBinding.CallValue(obj2, arguments, construct, brackets, engine, obj2, binder, culture, namedParameters);
							}
						}
						int num = ((arguments == null) ? 0 : arguments.Length);
						if (namedParameters != null && namedParameters.Length > 0 && (namedParameters[0].Equals("[DISPID=-613]") || namedParameters[0].Equals("this")))
						{
							num--;
						}
						bindingFlags |= ((num > 0) ? (BindingFlags.InvokeMethod | BindingFlags.GetProperty) : BindingFlags.InvokeMethod);
						obj = ir.InvokeMember(name, bindingFlags, binder, ob, arguments, modifiers, culture, namedParameters);
					}
				}
				catch (MissingMemberException)
				{
					if (!brackets)
					{
						throw new JScriptException(JSError.FunctionExpected);
					}
					obj = null;
				}
				catch (COMException ex)
				{
					int errorCode = ex.ErrorCode;
					if (errorCode != -2147352570 && errorCode != -2147352573)
					{
						if (((long)errorCode & (long)((ulong)(-65536))) == (long)((ulong)(-2146828288)))
						{
							string source = ex.Source;
							if (source != null && source.IndexOf("JScript") != -1)
							{
								throw new JScriptException(ex, null);
							}
						}
						throw ex;
					}
					if (!brackets)
					{
						throw new JScriptException(JSError.FunctionExpected);
					}
					obj = null;
				}
			}
			catch (JScriptException ex2)
			{
				if ((ex2.Number & 65535) == 5002)
				{
					MemberInfo[] member = typeof(object).GetMember(name, BindingFlags.Instance | BindingFlags.Public);
					if (member != null && member.Length > 0)
					{
						return LateBinding.CallOneOfTheMembers(member, arguments, construct, ob, binder, culture, namedParameters, engine);
					}
				}
				throw ex2;
			}
			return obj;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x000468CC File Offset: 0x000458CC
		[DebuggerHidden]
		[DebuggerStepThrough]
		private static object CallMethod(MethodInfo method, object[] arguments, object thisob, Binder binder, CultureInfo culture, string[] namedParameters)
		{
			if (namedParameters != null && namedParameters.Length > 0)
			{
				if (arguments.Length < namedParameters.Length)
				{
					throw new JScriptException(JSError.MoreNamedParametersThanArguments);
				}
				arguments = JSBinder.ArrangeNamedArguments(method, arguments, namedParameters);
			}
			object[] array = LateBinding.LickArgumentsIntoShape(method.GetParameters(), arguments, binder, culture);
			object obj2;
			try
			{
				object obj = method.Invoke(thisob, BindingFlags.SuppressChangeType, null, array, null);
				if (array != arguments && array != null && arguments != null)
				{
					int num = arguments.Length;
					int num2 = array.Length;
					if (num2 < num)
					{
						num = num2;
					}
					for (int i = 0; i < num; i++)
					{
						arguments[i] = array[i];
					}
				}
				obj2 = obj;
			}
			catch (TargetException ex)
			{
				ClassScope classScope = thisob as ClassScope;
				if (classScope == null)
				{
					throw;
				}
				obj2 = classScope.FakeCallToTypeMethod(method, array, ex);
			}
			return obj2;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0004698C File Offset: 0x0004598C
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static object CallOneOfTheMembers(MemberInfo[] members, object[] arguments, bool construct, object thisob, Binder binder, CultureInfo culture, string[] namedParameters, VsaEngine engine)
		{
			bool flag;
			object obj = LateBinding.CallOneOfTheMembers(members, arguments, construct, thisob, binder, culture, namedParameters, engine, out flag);
			if (!flag)
			{
				throw new MissingMemberException();
			}
			return obj;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x000469B8 File Offset: 0x000459B8
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static object CallOneOfTheMembers(MemberInfo[] members, object[] arguments, bool construct, object thisob, Binder binder, CultureInfo culture, string[] namedParameters, VsaEngine engine, out bool memberCalled)
		{
			memberCalled = true;
			if (construct)
			{
				ConstructorInfo constructorInfo = JSBinder.SelectConstructor(Runtime.TypeRefs, members, ref arguments, namedParameters);
				if (constructorInfo != null)
				{
					if (CustomAttribute.IsDefined(constructorInfo, typeof(JSFunctionAttribute), false))
					{
						if (thisob is StackFrame)
						{
							thisob = ((StackFrame)thisob).closureInstance;
						}
						int num = arguments.Length;
						object[] array = new object[num + 1];
						ArrayObject.Copy(arguments, 0, array, 0, num);
						array[num] = thisob;
						arguments = array;
					}
					JSConstructor jsconstructor = constructorInfo as JSConstructor;
					object obj;
					if (jsconstructor != null)
					{
						obj = jsconstructor.Construct(thisob, LateBinding.LickArgumentsIntoShape(constructorInfo.GetParameters(), arguments, JSBinder.ob, culture));
					}
					else
					{
						obj = constructorInfo.Invoke(BindingFlags.SuppressChangeType, null, LateBinding.LickArgumentsIntoShape(constructorInfo.GetParameters(), arguments, JSBinder.ob, culture), null);
					}
					if (obj is INeedEngine)
					{
						((INeedEngine)obj).SetEngine(engine);
					}
					return obj;
				}
			}
			else
			{
				object[] array2 = arguments;
				MethodInfo methodInfo = JSBinder.SelectMethod(Runtime.TypeRefs, members, ref arguments, namedParameters);
				if (methodInfo != null)
				{
					if (methodInfo is JSMethod)
					{
						return ((JSMethod)methodInfo).Invoke(thisob, thisob, BindingFlags.Default, JSBinder.ob, arguments, null);
					}
					if (CustomAttribute.IsDefined(methodInfo, typeof(JSFunctionAttribute), false))
					{
						if (!construct)
						{
							JSBuiltin builtinFunction = ((JSFunctionAttribute)CustomAttribute.GetCustomAttributes(methodInfo, typeof(JSFunctionAttribute), false)[0]).builtinFunction;
							if (builtinFunction != JSBuiltin.None)
							{
								IActivationObject activationObject = thisob as IActivationObject;
								if (activationObject != null)
								{
									thisob = activationObject.GetDefaultThisObject();
								}
								return BuiltinFunction.QuickCall(arguments, thisob, builtinFunction, null, engine);
							}
						}
						return LateBinding.CallValue(new BuiltinFunction(thisob, methodInfo), arguments, construct, false, engine, thisob, JSBinder.ob, null, null);
					}
					object[] array3 = LateBinding.LickArgumentsIntoShape(methodInfo.GetParameters(), arguments, JSBinder.ob, culture);
					if (thisob != null && !methodInfo.DeclaringType.IsAssignableFrom(thisob.GetType()))
					{
						if (thisob is StringObject)
						{
							return methodInfo.Invoke(((StringObject)thisob).value, BindingFlags.SuppressChangeType, null, array3, null);
						}
						if (thisob is NumberObject)
						{
							return methodInfo.Invoke(((NumberObject)thisob).value, BindingFlags.SuppressChangeType, null, array3, null);
						}
						if (thisob is BooleanObject)
						{
							return methodInfo.Invoke(((BooleanObject)thisob).value, BindingFlags.SuppressChangeType, null, array3, null);
						}
						if (thisob is ArrayWrapper)
						{
							return methodInfo.Invoke(((ArrayWrapper)thisob).value, BindingFlags.SuppressChangeType, null, array3, null);
						}
					}
					object obj2 = methodInfo.Invoke(thisob, BindingFlags.SuppressChangeType, null, array3, null);
					if (array3 != array2 && arguments == array2 && array3 != null && arguments != null)
					{
						int num2 = arguments.Length;
						int num3 = array3.Length;
						if (num3 < num2)
						{
							num2 = num3;
						}
						for (int i = 0; i < num2; i++)
						{
							arguments[i] = array3[i];
						}
					}
					return obj2;
				}
			}
			memberCalled = false;
			return null;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00046C68 File Offset: 0x00045C68
		[DebuggerHidden]
		[DebuggerStepThrough]
		public static object CallValue(object thisob, object val, object[] arguments, bool construct, bool brackets, VsaEngine engine)
		{
			object obj;
			try
			{
				obj = LateBinding.CallValue(val, arguments, construct, brackets, engine, thisob, JSBinder.ob, null, null);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return obj;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00046CA8 File Offset: 0x00045CA8
		[DebuggerHidden]
		[DebuggerStepThrough]
		public static object CallValue2(object val, object thisob, object[] arguments, bool construct, bool brackets, VsaEngine engine)
		{
			object obj;
			try
			{
				obj = LateBinding.CallValue(val, arguments, construct, brackets, engine, thisob, JSBinder.ob, null, null);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return obj;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00046CE8 File Offset: 0x00045CE8
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static object CallValue(object val, object[] arguments, bool construct, bool brackets, VsaEngine engine, object thisob, Binder binder, CultureInfo culture, string[] namedParameters)
		{
			if (construct)
			{
				if (val is ScriptFunction)
				{
					ScriptFunction scriptFunction = (ScriptFunction)val;
					if (brackets)
					{
						object obj = scriptFunction[arguments];
						if (obj != null)
						{
							return LateBinding.CallValue(obj, new object[0], true, false, engine, thisob, binder, culture, namedParameters);
						}
						Type predefinedType = Runtime.TypeRefs.GetPredefinedType(scriptFunction.name);
						if (predefinedType != null)
						{
							int num = arguments.Length;
							int[] array = new int[num];
							num = 0;
							foreach (object obj2 in arguments)
							{
								if (obj2 is int)
								{
									array[num++] = (int)obj2;
								}
								else
								{
									IConvertible iconvertible = Convert.GetIConvertible(obj2);
									if (iconvertible == null || !Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
									{
										goto IL_00E8;
									}
									double num2 = iconvertible.ToDouble(null);
									int num3 = (int)num2;
									if (num2 != (double)num3)
									{
										goto IL_00E8;
									}
									array[num++] = num3;
								}
							}
							return Array.CreateInstance(predefinedType, array);
						}
					}
					IL_00E8:
					FunctionObject functionObject = scriptFunction as FunctionObject;
					if (functionObject != null)
					{
						return functionObject.Construct(thisob as JSObject, (arguments == null) ? new object[0] : arguments);
					}
					object obj3 = scriptFunction.Construct((arguments == null) ? new object[0] : arguments);
					JSObject jsobject = obj3 as JSObject;
					if (jsobject != null)
					{
						jsobject.outer_class_instance = thisob as JSObject;
					}
					return obj3;
				}
				else if (val is ClassScope)
				{
					if (brackets)
					{
						return Array.CreateInstance(typeof(object), LateBinding.ToIndices(arguments));
					}
					JSObject jsobject2 = (JSObject)LateBinding.CallOneOfTheMembers(((ClassScope)val).constructors, arguments, construct, thisob, binder, culture, namedParameters, engine);
					jsobject2.noExpando = ((ClassScope)val).noExpando;
					return jsobject2;
				}
				else if (val is Type)
				{
					Type type = (Type)val;
					if (type.IsInterface && type.IsImport)
					{
						type = JSBinder.HandleCoClassAttribute(type);
					}
					if (brackets)
					{
						return Array.CreateInstance(type, LateBinding.ToIndices(arguments));
					}
					ConstructorInfo[] constructors = type.GetConstructors();
					object obj4;
					if (constructors == null || constructors.Length == 0)
					{
						obj4 = Activator.CreateInstance(type, BindingFlags.Default, JSBinder.ob, arguments, null);
					}
					else
					{
						obj4 = LateBinding.CallOneOfTheMembers(constructors, arguments, construct, thisob, binder, culture, namedParameters, engine);
					}
					if (obj4 is INeedEngine)
					{
						((INeedEngine)obj4).SetEngine(engine);
					}
					return obj4;
				}
				else
				{
					if (val is TypedArray && brackets)
					{
						return Array.CreateInstance(typeof(object), LateBinding.ToIndices(arguments));
					}
					if (VsaEngine.executeForJSEE && val is IDebuggerObject)
					{
						IReflect reflect = val as IReflect;
						if (reflect == null)
						{
							throw new JScriptException(JSError.FunctionExpected);
						}
						return reflect.InvokeMember(string.Empty, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.OptionalParamBinding, binder, thisob, arguments, null, culture, namedParameters);
					}
				}
			}
			if (brackets)
			{
				ScriptObject scriptObject = val as ScriptObject;
				if (scriptObject != null)
				{
					object obj5 = scriptObject[arguments];
					if (construct)
					{
						return LateBinding.CallValue(thisob, obj5, new object[0], true, false, engine);
					}
					return obj5;
				}
			}
			else
			{
				if (val is ScriptFunction)
				{
					if (thisob is IActivationObject)
					{
						thisob = ((IActivationObject)thisob).GetDefaultThisObject();
					}
					return ((ScriptFunction)val).Call((arguments == null) ? new object[0] : arguments, thisob, binder, culture);
				}
				if (val is Delegate)
				{
					return LateBinding.CallMethod(((Delegate)val).Method, arguments, thisob, binder, culture, namedParameters);
				}
				if (val is MethodInfo)
				{
					return LateBinding.CallMethod((MethodInfo)val, arguments, thisob, binder, culture, namedParameters);
				}
				if (val is Type && arguments.Length == 1)
				{
					return Convert.CoerceT(arguments[0], (Type)val, true);
				}
				if (VsaEngine.executeForJSEE && val is IDebuggerObject)
				{
					IReflect reflect2 = val as IReflect;
					if (reflect2 == null)
					{
						throw new JScriptException(JSError.FunctionExpected);
					}
					object[] array3 = new object[((arguments != null) ? arguments.Length : 0) + 1];
					array3[0] = thisob;
					if (arguments != null)
					{
						ArrayObject.Copy(arguments, 0, array3, 1, arguments.Length);
					}
					string[] array4 = new string[((namedParameters != null) ? namedParameters.Length : 0) + 1];
					array4[0] = "this";
					if (namedParameters != null)
					{
						ArrayObject.Copy(namedParameters, 0, array4, 1, namedParameters.Length);
					}
					return LateBinding.CallCOMObject(reflect2, string.Empty, val, binder, array3, null, culture, array4, false, false, engine);
				}
				else if (val is ClassScope)
				{
					if (arguments == null || arguments.Length != 1)
					{
						throw new JScriptException(JSError.FunctionExpected);
					}
					if (((ClassScope)val).HasInstance(arguments[0]))
					{
						return arguments[0];
					}
					throw new InvalidCastException(null);
				}
				else
				{
					if (val is TypedArray && arguments.Length == 1)
					{
						return Convert.Coerce(arguments[0], val, true);
					}
					if (val is ScriptObject)
					{
						throw new JScriptException(JSError.FunctionExpected);
					}
					if (val is MemberInfo[])
					{
						return LateBinding.CallOneOfTheMembers((MemberInfo[])val, arguments, construct, thisob, binder, culture, namedParameters, engine);
					}
				}
			}
			if (val != null)
			{
				Array array5 = val as Array;
				if (array5 != null)
				{
					if (arguments.Length != array5.Rank)
					{
						throw new JScriptException(JSError.IncorrectNumberOfIndices);
					}
					return array5.GetValue(LateBinding.ToIndices(arguments));
				}
				else
				{
					val = Convert.ToObject(val, engine);
					ScriptObject scriptObject2 = val as ScriptObject;
					if (scriptObject2 != null)
					{
						if (brackets)
						{
							return scriptObject2[arguments];
						}
						ScriptFunction scriptFunction2 = scriptObject2 as ScriptFunction;
						if (scriptFunction2 != null)
						{
							IActivationObject activationObject = thisob as IActivationObject;
							if (activationObject != null)
							{
								thisob = activationObject.GetDefaultThisObject();
							}
							return scriptFunction2.Call((arguments == null) ? new object[0] : arguments, thisob, binder, culture);
						}
						throw new JScriptException(JSError.InvalidCall);
					}
					else
					{
						IReflect irforObjectThatRequiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(val, VsaEngine.executeForJSEE);
						if (irforObjectThatRequiresInvokeMember != null)
						{
							if (brackets)
							{
								string text = string.Empty;
								int num4 = arguments.Length;
								if (num4 > 0)
								{
									text = Convert.ToString(arguments[num4 - 1]);
								}
								return LateBinding.CallCOMObject(irforObjectThatRequiresInvokeMember, text, val, binder, null, null, culture, namedParameters, false, true, engine);
							}
							if (!(val is IReflect))
							{
								return LateBinding.CallCOMObject(irforObjectThatRequiresInvokeMember, string.Empty, val, binder, arguments, null, culture, namedParameters, false, brackets, engine);
							}
							object[] array6 = new object[((arguments != null) ? arguments.Length : 0) + 1];
							array6[0] = thisob;
							if (arguments != null)
							{
								ArrayObject.Copy(arguments, 0, array6, 1, arguments.Length);
							}
							string[] array7 = new string[((namedParameters != null) ? namedParameters.Length : 0) + 1];
							array7[0] = "[DISPID=-613]";
							if (namedParameters != null)
							{
								ArrayObject.Copy(namedParameters, 0, array7, 1, namedParameters.Length);
							}
							return LateBinding.CallCOMObject(irforObjectThatRequiresInvokeMember, "[DISPID=0]", val, binder, array6, null, culture, array7, false, brackets, engine);
						}
						else
						{
							if (VsaEngine.executeForJSEE && val is IDebuggerObject && val is IReflect)
							{
								return LateBinding.CallCOMObject((IReflect)val, string.Empty, val, binder, arguments, null, culture, namedParameters, false, brackets, engine);
							}
							MemberInfo[] defaultMembers = TypeReflector.GetTypeReflectorFor(val.GetType()).GetDefaultMembers();
							if (defaultMembers != null && defaultMembers.Length > 0)
							{
								MethodInfo methodInfo = JSBinder.SelectMethod(Runtime.TypeRefs, defaultMembers, ref arguments, namedParameters);
								if (methodInfo != null)
								{
									return LateBinding.CallMethod(methodInfo, arguments, val, binder, culture, namedParameters);
								}
							}
						}
					}
				}
			}
			throw new JScriptException(JSError.FunctionExpected);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0004737C File Offset: 0x0004637C
		private static void Change64bitIntegersToDouble(object[] arguments)
		{
			if (arguments == null)
			{
				return;
			}
			int i = 0;
			int num = arguments.Length;
			while (i < num)
			{
				object obj = arguments[i];
				IConvertible iconvertible = Convert.GetIConvertible(obj);
				switch (Convert.GetTypeCode(obj, iconvertible))
				{
				case TypeCode.Int64:
				case TypeCode.UInt64:
					arguments[i] = iconvertible.ToDouble(null);
					break;
				}
				i++;
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000473D8 File Offset: 0x000463D8
		public bool Delete()
		{
			return LateBinding.DeleteMember(this.obj, this.name);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x000473EC File Offset: 0x000463EC
		public static bool DeleteMember(object obj, string name)
		{
			if (name == null || obj == null)
			{
				return false;
			}
			if (obj is ScriptObject)
			{
				return ((ScriptObject)obj).DeleteMember(name);
			}
			if (obj is IExpando)
			{
				try
				{
					IExpando expando = (IExpando)obj;
					MemberInfo[] member = expando.GetMember(name, BindingFlags.Instance | BindingFlags.Public);
					MemberInfo memberInfo = LateBinding.SelectMember(member);
					if (memberInfo != null)
					{
						expando.RemoveMember(memberInfo);
						return true;
					}
					return false;
				}
				catch
				{
					return false;
				}
			}
			if (!(obj is IDictionary))
			{
				Type type = obj.GetType();
				MethodInfo method = TypeReflector.GetTypeReflectorFor(type).GetMethod("op_Delete", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[]
				{
					type,
					typeof(object[])
				}, null);
				return method != null && (method.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope && method.ReturnType == typeof(bool) && (bool)method.Invoke(null, new object[]
				{
					obj,
					new object[] { name }
				});
			}
			IDictionary dictionary = (IDictionary)obj;
			if (dictionary.Contains(name))
			{
				dictionary.Remove(name);
				return true;
			}
			return false;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00047528 File Offset: 0x00046528
		internal static bool DeleteValueAtIndex(object obj, ulong index)
		{
			if (obj is ArrayObject && index < (ulong)(-1))
			{
				return ((ArrayObject)obj).DeleteValueAtIndex((uint)index);
			}
			return LateBinding.DeleteMember(obj, index.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00047558 File Offset: 0x00046558
		private static IReflect GetIRForObjectThatRequiresInvokeMember(object obj, bool checkForDebugger)
		{
			Type type = obj.GetType();
			TypeReflector typeReflectorFor = TypeReflector.GetTypeReflectorFor(type);
			if (!typeReflectorFor.Is__ComObject())
			{
				return null;
			}
			if (checkForDebugger)
			{
				IDebuggerObject debuggerObject = obj as IDebuggerObject;
				if (debuggerObject != null)
				{
					if (!debuggerObject.IsCOMObject())
					{
						return null;
					}
					return (IReflect)obj;
				}
			}
			return type;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0004759C File Offset: 0x0004659C
		private static IReflect GetIRForObjectThatRequiresInvokeMember(object obj, bool checkForDebugger, TypeCode tcode)
		{
			if (tcode == TypeCode.Object)
			{
				return LateBinding.GetIRForObjectThatRequiresInvokeMember(obj, checkForDebugger);
			}
			return null;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x000475AC File Offset: 0x000465AC
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static object GetMemberValue(object obj, string name)
		{
			if (obj is ScriptObject)
			{
				return ((ScriptObject)obj).GetMemberValue(name);
			}
			LateBinding lateBinding = new LateBinding(name, obj);
			return lateBinding.GetNonMissingValue();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x000475DC File Offset: 0x000465DC
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static object GetMemberValue2(object obj, string name)
		{
			if (obj is ScriptObject)
			{
				return ((ScriptObject)obj).GetMemberValue(name);
			}
			LateBinding lateBinding = new LateBinding(name, obj);
			return lateBinding.GetValue();
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0004760C File Offset: 0x0004660C
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static object GetMemberValue(object obj, string name, MemberInfo member, MemberInfo[] members)
		{
			if (member != null)
			{
				try
				{
					MemberTypes memberType = member.MemberType;
					switch (memberType)
					{
					case MemberTypes.Event:
						return null;
					case MemberTypes.Constructor | MemberTypes.Event:
						break;
					case MemberTypes.Field:
					{
						object obj2 = ((FieldInfo)member).GetValue(obj);
						Type type = obj as Type;
						if (type != null && type.IsEnum)
						{
							try
							{
								obj2 = Enum.ToObject(type, ((IConvertible)obj2).ToUInt64(null));
							}
							catch
							{
							}
						}
						return obj2;
					}
					default:
						if (memberType == MemberTypes.Property)
						{
							PropertyInfo propertyInfo = (PropertyInfo)member;
							if (propertyInfo.DeclaringType == typeof(ArrayObject))
							{
								ArrayObject arrayObject = obj as ArrayObject;
								if (arrayObject != null)
								{
									return arrayObject.length;
								}
							}
							else if (propertyInfo.DeclaringType == typeof(StringObject))
							{
								StringObject stringObject = obj as StringObject;
								if (stringObject != null)
								{
									return stringObject.length;
								}
							}
							return JSProperty.GetValue(propertyInfo, obj, null);
						}
						if (memberType == MemberTypes.NestedType)
						{
							return member;
						}
						break;
					}
				}
				catch
				{
					if (obj is StringObject)
					{
						return LateBinding.GetMemberValue(((StringObject)obj).value, name, member, members);
					}
					if (obj is NumberObject)
					{
						return LateBinding.GetMemberValue(((NumberObject)obj).value, name, member, members);
					}
					if (obj is BooleanObject)
					{
						return LateBinding.GetMemberValue(((BooleanObject)obj).value, name, member, members);
					}
					if (obj is ArrayWrapper)
					{
						return LateBinding.GetMemberValue(((ArrayWrapper)obj).value, name, member, members);
					}
					throw;
				}
			}
			if (members != null && members.Length > 0)
			{
				if (members.Length == 1 && members[0].MemberType == MemberTypes.Method)
				{
					MethodInfo methodInfo = (MethodInfo)members[0];
					Type declaringType = methodInfo.DeclaringType;
					if (declaringType == typeof(GlobalObject) || (declaringType != null && declaringType != typeof(StringObject) && declaringType != typeof(NumberObject) && declaringType != typeof(BooleanObject) && declaringType.IsSubclassOf(typeof(JSObject))))
					{
						return Globals.BuiltinFunctionFor(obj, methodInfo);
					}
				}
				return new FunctionWrapper(name, obj, members);
			}
			if (obj is ScriptObject)
			{
				return ((ScriptObject)obj).GetMemberValue(name);
			}
			if (!(obj is Namespace))
			{
				IReflect irforObjectThatRequiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(obj, true);
				if (irforObjectThatRequiresInvokeMember != null)
				{
					try
					{
						return irforObjectThatRequiresInvokeMember.InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.OptionalParamBinding, JSBinder.ob, obj, null, null, null, null);
					}
					catch (MissingMemberException)
					{
					}
					catch (COMException ex)
					{
						int errorCode = ex.ErrorCode;
						if (errorCode != -2147352570 && errorCode != -2147352573)
						{
							throw ex;
						}
					}
				}
				return Missing.Value;
			}
			Namespace @namespace = (Namespace)obj;
			string text = @namespace.Name + "." + name;
			Type type2 = @namespace.GetType(text);
			if (type2 != null)
			{
				return type2;
			}
			return Namespace.GetNamespace(text, @namespace.engine);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00047920 File Offset: 0x00046920
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object GetNonMissingValue()
		{
			object value = this.GetValue();
			if (value is Missing)
			{
				return null;
			}
			return value;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0004793F File Offset: 0x0004693F
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal object GetValue()
		{
			this.BindToMember();
			return LateBinding.GetMemberValue(this.obj, this.name, this.last_member, this.last_members);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00047968 File Offset: 0x00046968
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object GetValue2()
		{
			object value = this.GetValue();
			if (value == Missing.Value)
			{
				throw new JScriptException(JSError.UndefinedIdentifier, new Context(new DocumentContext("", null), this.name));
			}
			return value;
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x000479A8 File Offset: 0x000469A8
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static object GetValueAtIndex(object obj, ulong index)
		{
			if (!(obj is ScriptObject))
			{
				while (!(obj is IList))
				{
					if (obj is Array)
					{
						return ((Array)obj).GetValue(checked((int)index));
					}
					Type type = obj.GetType();
					if (type.IsCOMObject || obj is IReflect || index > 2147483647UL)
					{
						return LateBinding.GetMemberValue(obj, index.ToString(CultureInfo.InvariantCulture));
					}
					MethodInfo defaultPropertyForArrayIndex = JSBinder.GetDefaultPropertyForArrayIndex(type, (int)index, null, false);
					if (defaultPropertyForArrayIndex == null)
					{
						return Missing.Value;
					}
					ParameterInfo[] parameters = defaultPropertyForArrayIndex.GetParameters();
					if (parameters != null && parameters.Length != 0)
					{
						return defaultPropertyForArrayIndex.Invoke(obj, BindingFlags.Default, JSBinder.ob, new object[] { (int)index }, null);
					}
					obj = defaultPropertyForArrayIndex.Invoke(obj, BindingFlags.SuppressChangeType, null, null, null);
				}
				return ((IList)obj)[checked((int)index)];
			}
			if (index < (ulong)(-1))
			{
				return ((ScriptObject)obj).GetValueAtIndex((uint)index);
			}
			return ((ScriptObject)obj).GetMemberValue(index.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00047AA0 File Offset: 0x00046AA0
		private static object[] LickArgumentsIntoShape(ParameterInfo[] pars, object[] arguments, Binder binder, CultureInfo culture)
		{
			if (arguments == null)
			{
				return null;
			}
			int num = pars.Length;
			if (num == 0)
			{
				return null;
			}
			object[] array = arguments;
			int num2 = arguments.Length;
			if (num2 != num)
			{
				array = new object[num];
			}
			int num3 = num - 1;
			int num4 = ((num2 < num3) ? num2 : num3);
			for (int i = 0; i < num4; i++)
			{
				object obj = arguments[i];
				if (obj is DBNull)
				{
					array[i] = null;
				}
				else
				{
					array[i] = binder.ChangeType(arguments[i], pars[i].ParameterType, culture);
				}
			}
			for (int j = num4; j < num3; j++)
			{
				object obj2 = TypeReferences.GetDefaultParameterValue(pars[j]);
				if (obj2 == Convert.DBNull)
				{
					obj2 = binder.ChangeType(null, pars[j].ParameterType, culture);
				}
				array[j] = obj2;
			}
			if (CustomAttribute.IsDefined(pars[num3], typeof(ParamArrayAttribute), false))
			{
				int num5 = num2 - num3;
				if (num5 < 0)
				{
					num5 = 0;
				}
				Type elementType = pars[num3].ParameterType.GetElementType();
				Array array2 = Array.CreateInstance(elementType, num5);
				for (int k = 0; k < num5; k++)
				{
					array2.SetValue(binder.ChangeType(arguments[k + num3], elementType, culture), k);
				}
				array[num3] = array2;
			}
			else if (num2 < num)
			{
				object obj3 = TypeReferences.GetDefaultParameterValue(pars[num3]);
				if (obj3 == Convert.DBNull)
				{
					obj3 = binder.ChangeType(null, pars[num3].ParameterType, culture);
				}
				array[num3] = obj3;
			}
			else
			{
				array[num3] = binder.ChangeType(arguments[num3], pars[num3].ParameterType, culture);
			}
			return array;
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00047C0C File Offset: 0x00046C0C
		internal static MemberInfo SelectMember(MemberInfo[] mems)
		{
			if (mems == null)
			{
				return null;
			}
			MemberInfo memberInfo = null;
			foreach (MemberInfo memberInfo2 in mems)
			{
				MemberTypes memberType = memberInfo2.MemberType;
				if (memberType <= MemberTypes.Property)
				{
					if (memberType != MemberTypes.Field)
					{
						if (memberType == MemberTypes.Property)
						{
							if (memberInfo == null || (memberInfo.MemberType != MemberTypes.Field && memberInfo.MemberType != MemberTypes.Property))
							{
								ParameterInfo[] indexParameters = ((PropertyInfo)memberInfo2).GetIndexParameters();
								if (indexParameters != null && indexParameters.Length == 0)
								{
									memberInfo = memberInfo2;
								}
							}
						}
					}
					else if (memberInfo == null || memberInfo.MemberType != MemberTypes.Field)
					{
						memberInfo = memberInfo2;
					}
				}
				else if (memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType)
				{
					if (memberInfo == null)
					{
						memberInfo = memberInfo2;
					}
				}
			}
			return memberInfo;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00047CAC File Offset: 0x00046CAC
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal void SetIndexedDefaultPropertyValue(object ob, object[] arguments, object value)
		{
			ScriptObject scriptObject = ob as ScriptObject;
			if (scriptObject != null)
			{
				scriptObject[arguments] = value;
				return;
			}
			Array array = ob as Array;
			if (array != null)
			{
				if (arguments.Length != array.Rank)
				{
					throw new JScriptException(JSError.IncorrectNumberOfIndices);
				}
				array.SetValue(value, LateBinding.ToIndices(arguments));
				return;
			}
			else
			{
				TypeCode typeCode = Convert.GetTypeCode(ob);
				if (Convert.NeedsWrapper(typeCode))
				{
					return;
				}
				IReflect reflect = LateBinding.GetIRForObjectThatRequiresInvokeMember(ob, this.checkForDebugger, typeCode);
				if (reflect == null && this.checkForDebugger && ob is IDebuggerObject && ob is IReflect)
				{
					reflect = (IReflect)ob;
				}
				if (reflect != null)
				{
					try
					{
						int num = arguments.Length + 1;
						object[] array2 = new object[num];
						ArrayObject.Copy(arguments, 0, array2, 0, num - 1);
						array2[num - 1] = value;
						reflect.InvokeMember(string.Empty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.OptionalParamBinding, JSBinder.ob, ob, array2, null, null, null);
						return;
					}
					catch (MissingMemberException)
					{
						throw new JScriptException(JSError.OLENoPropOrMethod);
					}
				}
				MemberInfo[] defaultMembers = TypeReflector.GetTypeReflectorFor(ob.GetType()).GetDefaultMembers();
				if (defaultMembers != null && defaultMembers.Length > 0)
				{
					PropertyInfo propertyInfo = JSBinder.SelectProperty(Runtime.TypeRefs, defaultMembers, arguments);
					if (propertyInfo != null)
					{
						MethodInfo setMethod = JSProperty.GetSetMethod(propertyInfo, false);
						if (setMethod != null)
						{
							arguments = LateBinding.LickArgumentsIntoShape(propertyInfo.GetIndexParameters(), arguments, JSBinder.ob, null);
							value = Convert.CoerceT(value, propertyInfo.PropertyType);
							int num2 = arguments.Length + 1;
							object[] array3 = new object[num2];
							ArrayObject.Copy(arguments, 0, array3, 0, num2 - 1);
							array3[num2 - 1] = value;
							setMethod.Invoke(ob, array3);
							return;
						}
					}
				}
				throw new JScriptException(JSError.OLENoPropOrMethod);
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00047E40 File Offset: 0x00046E40
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal void SetIndexedPropertyValue(object[] arguments, object value)
		{
			if (this.obj == null)
			{
				throw new JScriptException(JSError.ObjectExpected);
			}
			if (this.name == null)
			{
				this.SetIndexedDefaultPropertyValue(this.obj, arguments, value);
				return;
			}
			this.BindToMember();
			if (this.last_members != null && this.last_members.Length > 0)
			{
				PropertyInfo propertyInfo = JSBinder.SelectProperty(Runtime.TypeRefs, this.last_members, arguments);
				if (propertyInfo != null)
				{
					if (arguments.Length > 0 && propertyInfo.GetIndexParameters().Length == 0 && !(propertyInfo is COMPropertyInfo))
					{
						MethodInfo getMethod = JSProperty.GetGetMethod(propertyInfo, false);
						if (getMethod != null)
						{
							LateBinding.SetIndexedPropertyValueStatic(getMethod.Invoke(this.obj, null), arguments, value);
							return;
						}
					}
					arguments = LateBinding.LickArgumentsIntoShape(propertyInfo.GetIndexParameters(), arguments, JSBinder.ob, null);
					value = Convert.CoerceT(value, propertyInfo.PropertyType);
					JSProperty.SetValue(propertyInfo, this.obj, value, arguments);
					return;
				}
			}
			TypeCode typeCode = Convert.GetTypeCode(this.obj);
			if (Convert.NeedsWrapper(typeCode))
			{
				return;
			}
			IReflect irforObjectThatRequiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(this.obj, this.checkForDebugger, typeCode);
			if (irforObjectThatRequiresInvokeMember != null)
			{
				int num = arguments.Length + 1;
				object[] array = new object[num];
				ArrayObject.Copy(arguments, 0, array, 0, num - 1);
				array[num - 1] = value;
				irforObjectThatRequiresInvokeMember.InvokeMember(this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.OptionalParamBinding, JSBinder.ob, this.obj, array, null, null, null);
				return;
			}
			object value2 = this.GetValue();
			if (value2 != null && !(value2 is Missing))
			{
				this.SetIndexedDefaultPropertyValue(value2, arguments, value);
				return;
			}
			throw new JScriptException(JSError.OLENoPropOrMethod);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00047FB0 File Offset: 0x00046FB0
		[DebuggerHidden]
		[DebuggerStepThrough]
		public static void SetIndexedPropertyValueStatic(object obj, object[] arguments, object value)
		{
			if (obj == null)
			{
				throw new JScriptException(JSError.ObjectExpected);
			}
			ScriptObject scriptObject = obj as ScriptObject;
			if (scriptObject != null)
			{
				scriptObject[arguments] = value;
				return;
			}
			Array array = obj as Array;
			if (array != null)
			{
				if (arguments.Length != array.Rank)
				{
					throw new JScriptException(JSError.IncorrectNumberOfIndices);
				}
				array.SetValue(value, LateBinding.ToIndices(arguments));
				return;
			}
			else
			{
				TypeCode typeCode = Convert.GetTypeCode(obj);
				if (Convert.NeedsWrapper(typeCode))
				{
					return;
				}
				IReflect irforObjectThatRequiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(obj, true, typeCode);
				if (irforObjectThatRequiresInvokeMember != null)
				{
					string text = string.Empty;
					int num = arguments.Length;
					if (num > 0)
					{
						text = Convert.ToString(arguments[num - 1]);
					}
					irforObjectThatRequiresInvokeMember.InvokeMember(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.OptionalParamBinding, JSBinder.ob, obj, new object[] { value }, null, null, null);
					return;
				}
				MemberInfo[] defaultMembers = TypeReflector.GetTypeReflectorFor(obj.GetType()).GetDefaultMembers();
				if (defaultMembers != null && defaultMembers.Length > 0)
				{
					PropertyInfo propertyInfo = JSBinder.SelectProperty(Runtime.TypeRefs, defaultMembers, arguments);
					if (propertyInfo != null)
					{
						MethodInfo setMethod = JSProperty.GetSetMethod(propertyInfo, false);
						if (setMethod != null)
						{
							arguments = LateBinding.LickArgumentsIntoShape(propertyInfo.GetIndexParameters(), arguments, JSBinder.ob, null);
							value = Convert.CoerceT(value, propertyInfo.PropertyType);
							int num2 = arguments.Length + 1;
							object[] array2 = new object[num2];
							ArrayObject.Copy(arguments, 0, array2, 0, num2 - 1);
							array2[num2 - 1] = value;
							setMethod.Invoke(obj, array2);
							return;
						}
					}
				}
				throw new JScriptException(JSError.OLENoPropOrMethod);
			}
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0004810C File Offset: 0x0004710C
		[DebuggerHidden]
		[DebuggerStepThrough]
		private static void SetMember(object obj, object value, MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType == MemberTypes.Field)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				if (!fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
				{
					if (fieldInfo is JSField)
					{
						fieldInfo.SetValue(obj, value);
						return;
					}
					fieldInfo.SetValue(obj, Convert.CoerceT(value, fieldInfo.FieldType), BindingFlags.SuppressChangeType, null, null);
				}
				return;
			}
			if (memberType != MemberTypes.Property)
			{
				return;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			if (propertyInfo is JSProperty || propertyInfo is JSWrappedProperty)
			{
				propertyInfo.SetValue(obj, value, null);
				return;
			}
			MethodInfo setMethod = JSProperty.GetSetMethod(propertyInfo, false);
			if (setMethod != null)
			{
				try
				{
					setMethod.Invoke(obj, BindingFlags.SuppressChangeType, null, new object[] { Convert.CoerceT(value, propertyInfo.PropertyType) }, null);
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x000481E0 File Offset: 0x000471E0
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static void SetMemberValue(object obj, string name, object value)
		{
			if (obj is ScriptObject)
			{
				((ScriptObject)obj).SetMemberValue(name, value);
				return;
			}
			LateBinding lateBinding = new LateBinding(name, obj);
			lateBinding.SetValue(value);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00048214 File Offset: 0x00047214
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static void SetMemberValue(object obj, string name, object value, MemberInfo member, MemberInfo[] members)
		{
			if (member != null)
			{
				LateBinding.SetMember(obj, value, member);
				return;
			}
			if (obj is ScriptObject)
			{
				((ScriptObject)obj).SetMemberValue(name, value);
				return;
			}
			TypeCode typeCode = Convert.GetTypeCode(obj);
			if (Convert.NeedsWrapper(typeCode))
			{
				return;
			}
			IReflect irforObjectThatRequiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(obj, true, typeCode);
			if (irforObjectThatRequiresInvokeMember != null)
			{
				try
				{
					object[] array = new object[] { value };
					BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.OptionalParamBinding;
					irforObjectThatRequiresInvokeMember.InvokeMember(name, bindingFlags, JSBinder.ob, obj, array, null, null, null);
					return;
				}
				catch (MissingMemberException)
				{
				}
				catch (COMException ex)
				{
					int errorCode = ex.ErrorCode;
					if (errorCode != -2147352570 && errorCode != -2147352573)
					{
						throw ex;
					}
				}
			}
			if (obj is IExpando)
			{
				PropertyInfo propertyInfo = ((IExpando)obj).AddProperty(name);
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(obj, value, null);
					return;
				}
				FieldInfo fieldInfo = ((IExpando)obj).AddField(name);
				if (fieldInfo != null)
				{
					fieldInfo.SetValue(obj, value);
				}
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0004830C File Offset: 0x0004730C
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static void SetValueAtIndex(object obj, ulong index, object value)
		{
			if (obj is ScriptObject)
			{
				if (index < (ulong)(-1))
				{
					((ScriptObject)obj).SetValueAtIndex((uint)index, value);
					return;
				}
				((ScriptObject)obj).SetMemberValue(index.ToString(CultureInfo.InvariantCulture), value);
				return;
			}
			else
			{
				while (!(obj is IList))
				{
					if (obj is Array)
					{
						((Array)obj).SetValue(Convert.CoerceT(value, obj.GetType().GetElementType()), checked((int)index));
						return;
					}
					Type type = obj.GetType();
					if (type.IsCOMObject || obj is IReflect || index > 2147483647UL)
					{
						LateBinding.SetMemberValue(obj, index.ToString(CultureInfo.InvariantCulture), value);
						return;
					}
					MethodInfo defaultPropertyForArrayIndex = JSBinder.GetDefaultPropertyForArrayIndex(type, (int)index, null, true);
					if (defaultPropertyForArrayIndex != null)
					{
						ParameterInfo[] parameters = defaultPropertyForArrayIndex.GetParameters();
						if (parameters == null || parameters.Length == 0)
						{
							obj = defaultPropertyForArrayIndex.Invoke(obj, BindingFlags.SuppressChangeType, null, null, null);
							continue;
						}
						defaultPropertyForArrayIndex.Invoke(obj, BindingFlags.Default, JSBinder.ob, new object[]
						{
							(int)index,
							value
						}, null);
					}
					return;
				}
				IList list = (IList)obj;
				if (index < (ulong)((long)list.Count))
				{
					list[(int)index] = value;
					return;
				}
				list.Insert((int)index, value);
				return;
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00048432 File Offset: 0x00047432
		[DebuggerStepThrough]
		[DebuggerHidden]
		public void SetValue(object value)
		{
			this.BindToMember();
			LateBinding.SetMemberValue(this.obj, this.name, value, this.last_member, this.last_members);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0004845C File Offset: 0x0004745C
		internal static void SwapValues(object obj, uint left, uint right)
		{
			if (obj is JSObject)
			{
				((JSObject)obj).SwapValues(left, right);
				return;
			}
			if (obj is IList)
			{
				IList list = (IList)obj;
				object obj2 = list[(int)left];
				list[(int)left] = list[(int)right];
				list[(int)right] = obj2;
				return;
			}
			if (obj is Array)
			{
				Array array = (Array)obj;
				object value = array.GetValue((int)left);
				array.SetValue(array.GetValue((int)right), (int)left);
				array.SetValue(value, (int)right);
				return;
			}
			if (obj is IExpando)
			{
				string text = Convert.ToString(left, CultureInfo.InvariantCulture);
				string text2 = Convert.ToString(right, CultureInfo.InvariantCulture);
				IExpando expando = (IExpando)obj;
				FieldInfo fieldInfo = expando.GetField(text, BindingFlags.Instance | BindingFlags.Public);
				FieldInfo fieldInfo2 = expando.GetField(text2, BindingFlags.Instance | BindingFlags.Public);
				if (fieldInfo == null)
				{
					if (fieldInfo2 == null)
					{
						return;
					}
					try
					{
						fieldInfo = expando.AddField(text);
						fieldInfo.SetValue(obj, fieldInfo2.GetValue(obj));
						expando.RemoveMember(fieldInfo2);
						goto IL_012A;
					}
					catch
					{
						throw new JScriptException(JSError.ActionNotSupported);
					}
				}
				if (fieldInfo2 == null)
				{
					try
					{
						fieldInfo2 = expando.AddField(text2);
						fieldInfo2.SetValue(obj, fieldInfo.GetValue(obj));
						expando.RemoveMember(fieldInfo);
					}
					catch
					{
						throw new JScriptException(JSError.ActionNotSupported);
					}
				}
				IL_012A:
				object value2 = fieldInfo.GetValue(obj);
				fieldInfo.SetValue(obj, fieldInfo2.GetValue(obj));
				fieldInfo2.SetValue(obj, value2);
				return;
			}
			object valueAtIndex = LateBinding.GetValueAtIndex(obj, (ulong)left);
			object valueAtIndex2 = LateBinding.GetValueAtIndex(obj, (ulong)right);
			if (valueAtIndex is Missing)
			{
				LateBinding.DeleteValueAtIndex(obj, (ulong)right);
			}
			else
			{
				LateBinding.SetValueAtIndex(obj, (ulong)right, valueAtIndex);
			}
			if (valueAtIndex2 is Missing)
			{
				LateBinding.DeleteValueAtIndex(obj, (ulong)left);
				return;
			}
			LateBinding.SetValueAtIndex(obj, (ulong)left, valueAtIndex2);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00048624 File Offset: 0x00047624
		private static int[] ToIndices(object[] arguments)
		{
			int num = arguments.Length;
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = Convert.ToInt32(arguments[i]);
			}
			return array;
		}

		// Token: 0x0400055E RID: 1374
		private object last_ir;

		// Token: 0x0400055F RID: 1375
		internal MemberInfo last_member;

		// Token: 0x04000560 RID: 1376
		internal MemberInfo[] last_members;

		// Token: 0x04000561 RID: 1377
		internal object last_object;

		// Token: 0x04000562 RID: 1378
		private string name;

		// Token: 0x04000563 RID: 1379
		public object obj;

		// Token: 0x04000564 RID: 1380
		private bool checkForDebugger;
	}
}
