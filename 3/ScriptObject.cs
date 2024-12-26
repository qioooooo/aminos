using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000005 RID: 5
	[ComVisible(true)]
	public abstract class ScriptObject : IReflect
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002509 File Offset: 0x00001509
		internal ScriptObject(ScriptObject parent)
		{
			this.parent = parent;
			this.wrappedMemberCache = null;
			if (this.parent != null)
			{
				this.engine = parent.engine;
				return;
			}
			this.engine = null;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000253B File Offset: 0x0000153B
		internal virtual bool DeleteMember(string name)
		{
			return false;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000253E File Offset: 0x0000153E
		internal virtual object GetDefaultValue(PreferredType preferred_type)
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002548 File Offset: 0x00001548
		public FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			foreach (MemberInfo memberInfo in this.GetMember(name, bindingAttr))
			{
				if (memberInfo.MemberType == MemberTypes.Field)
				{
					return (FieldInfo)memberInfo;
				}
			}
			return null;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002588 File Offset: 0x00001588
		public virtual FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			ArrayObject arrayObject = this as ArrayObject;
			if (arrayObject != null && arrayObject.denseArrayLength > 0U)
			{
				uint num = arrayObject.denseArrayLength;
				if (num > arrayObject.len)
				{
					num = arrayObject.len;
				}
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					object obj = arrayObject.denseArray[(int)((UIntPtr)num2)];
					if (obj != Missing.Value)
					{
						arrayObject.SetMemberValue2(num2.ToString(CultureInfo.InvariantCulture), obj);
					}
				}
				arrayObject.denseArrayLength = 0U;
				arrayObject.denseArray = null;
			}
			MemberInfo[] members = this.GetMembers(bindingAttr);
			if (members == null)
			{
				return new FieldInfo[0];
			}
			int num3 = 0;
			foreach (MemberInfo memberInfo in members)
			{
				if (memberInfo.MemberType == MemberTypes.Field)
				{
					num3++;
				}
			}
			FieldInfo[] array2 = new FieldInfo[num3];
			num3 = 0;
			foreach (MemberInfo memberInfo2 in members)
			{
				if (memberInfo2.MemberType == MemberTypes.Field)
				{
					array2[num3++] = (FieldInfo)memberInfo2;
				}
			}
			return array2;
		}

		// Token: 0x06000015 RID: 21
		public abstract MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x06000016 RID: 22 RVA: 0x0000268C File Offset: 0x0000168C
		internal virtual object GetMemberValue(string name)
		{
			MemberInfo[] member = this.GetMember(name, BindingFlags.Instance | BindingFlags.Public);
			if (member.Length == 0)
			{
				return Missing.Value;
			}
			return LateBinding.GetMemberValue(this, name, LateBinding.SelectMember(member), member);
		}

		// Token: 0x06000017 RID: 23
		public abstract MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x06000018 RID: 24 RVA: 0x000026BC File Offset: 0x000016BC
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
		{
			return this.GetMethod(name, bindingAttr, JSBinder.ob, Type.EmptyTypes, null);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026D4 File Offset: 0x000016D4
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			MemberInfo[] member = this.GetMember(name, bindingAttr);
			if (member.Length == 1)
			{
				return member[0] as MethodInfo;
			}
			int num = 0;
			foreach (MemberInfo memberInfo in member)
			{
				if (memberInfo.MemberType == MemberTypes.Method)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			MethodInfo[] array2 = new MethodInfo[num];
			num = 0;
			foreach (MemberInfo memberInfo2 in member)
			{
				if (memberInfo2.MemberType == MemberTypes.Method)
				{
					array2[num++] = (MethodInfo)memberInfo2;
				}
			}
			if (binder == null)
			{
				binder = JSBinder.ob;
			}
			return (MethodInfo)binder.SelectMethod(bindingAttr, array2, types, modifiers);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002784 File Offset: 0x00001784
		public virtual MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			MemberInfo[] members = this.GetMembers(bindingAttr);
			if (members == null)
			{
				return new MethodInfo[0];
			}
			int num = 0;
			foreach (MemberInfo memberInfo in members)
			{
				if (memberInfo.MemberType == MemberTypes.Method)
				{
					num++;
				}
			}
			MethodInfo[] array2 = new MethodInfo[num];
			num = 0;
			foreach (MemberInfo memberInfo2 in members)
			{
				if (memberInfo2.MemberType == MemberTypes.Method)
				{
					array2[num++] = (MethodInfo)memberInfo2;
				}
			}
			return array2;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000280D File Offset: 0x0000180D
		public ScriptObject GetParent()
		{
			return this.parent;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002818 File Offset: 0x00001818
		internal virtual void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
		{
			MemberInfo[] members = this.GetMembers(BindingFlags.Instance | BindingFlags.Public);
			if (members.Length > 0)
			{
				enums.Add(members.GetEnumerator());
				objects.Add(this);
			}
			ScriptObject scriptObject = this.GetParent();
			if (scriptObject != null)
			{
				scriptObject.GetPropertyEnumerator(enums, objects);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000285B File Offset: 0x0000185B
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
		{
			return this.GetProperty(name, bindingAttr, JSBinder.ob, null, Type.EmptyTypes, null);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002874 File Offset: 0x00001874
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			MemberInfo[] member = this.GetMember(name, bindingAttr);
			if (member.Length == 1)
			{
				return member[0] as PropertyInfo;
			}
			int num = 0;
			foreach (MemberInfo memberInfo in member)
			{
				if (memberInfo.MemberType == MemberTypes.Property)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			PropertyInfo[] array2 = new PropertyInfo[num];
			num = 0;
			foreach (MemberInfo memberInfo2 in member)
			{
				if (memberInfo2.MemberType == MemberTypes.Property)
				{
					array2[num++] = (PropertyInfo)memberInfo2;
				}
			}
			if (binder == null)
			{
				binder = JSBinder.ob;
			}
			return binder.SelectProperty(bindingAttr, array2, returnType, types, modifiers);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002924 File Offset: 0x00001924
		public virtual PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			MemberInfo[] members = this.GetMembers(bindingAttr);
			if (members == null)
			{
				return new PropertyInfo[0];
			}
			int num = 0;
			foreach (MemberInfo memberInfo in members)
			{
				if (memberInfo.MemberType == MemberTypes.Property)
				{
					num++;
				}
			}
			PropertyInfo[] array2 = new PropertyInfo[num];
			num = 0;
			foreach (MemberInfo memberInfo2 in members)
			{
				if (memberInfo2.MemberType == MemberTypes.Property)
				{
					array2[num++] = (PropertyInfo)memberInfo2;
				}
			}
			return array2;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029AF File Offset: 0x000019AF
		internal virtual object GetValueAtIndex(uint index)
		{
			return this.GetMemberValue(index.ToString(CultureInfo.CurrentUICulture));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000029C4 File Offset: 0x000019C4
		[DebuggerStepThrough]
		[DebuggerHidden]
		public virtual object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo locale, string[] namedParameters)
		{
			if (target != this)
			{
				throw new TargetException();
			}
			bool flag = name.StartsWith("< JScript-", StringComparison.Ordinal);
			bool flag2 = name == null || name == string.Empty || name.Equals("[DISPID=0]") || flag;
			if ((invokeAttr & BindingFlags.CreateInstance) != BindingFlags.Default)
			{
				if ((invokeAttr & (BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty)) != BindingFlags.Default)
				{
					throw new ArgumentException(JScriptException.Localize("Bad binding flags", locale));
				}
				if (flag2)
				{
					throw new MissingMethodException();
				}
				LateBinding lateBinding = new LateBinding(name, this);
				return lateBinding.Call(binder, args, modifiers, locale, namedParameters, true, false, this.engine);
			}
			else
			{
				if (name == null)
				{
					throw new ArgumentException(JScriptException.Localize("Bad name", locale));
				}
				if ((invokeAttr & (BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty)) != BindingFlags.Default)
				{
					if ((invokeAttr & (BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.PutDispProperty)) != BindingFlags.Default)
					{
						throw new ArgumentException(JScriptException.Localize("Bad binding flags", locale));
					}
					if (!flag2)
					{
						if ((args == null || args.Length == 0) && (invokeAttr & (BindingFlags.GetField | BindingFlags.GetProperty)) != BindingFlags.Default)
						{
							object memberValue = this.GetMemberValue(name);
							if (memberValue != Missing.Value)
							{
								return memberValue;
							}
							if ((invokeAttr & BindingFlags.InvokeMethod) == BindingFlags.Default)
							{
								throw new MissingFieldException();
							}
						}
						LateBinding lateBinding2 = new LateBinding(name, this);
						return lateBinding2.Call(binder, args, modifiers, locale, namedParameters, false, false, this.engine);
					}
					if ((invokeAttr & (BindingFlags.GetField | BindingFlags.GetProperty)) == BindingFlags.Default)
					{
						throw new MissingMethodException();
					}
					if (args == null || args.Length == 0)
					{
						if (this is JSObject || this is GlobalScope || this is ClassScope)
						{
							PreferredType preferredType = PreferredType.Either;
							if (flag)
							{
								if (name.StartsWith("< JScript-Number", StringComparison.Ordinal))
								{
									preferredType = PreferredType.Number;
								}
								else if (name.StartsWith("< JScript-String", StringComparison.Ordinal))
								{
									preferredType = PreferredType.String;
								}
								else if (name.StartsWith("< JScript-LocaleString", StringComparison.Ordinal))
								{
									preferredType = PreferredType.LocaleString;
								}
							}
							return this.GetDefaultValue(preferredType);
						}
						throw new MissingFieldException();
					}
					else
					{
						if (args.Length > 1)
						{
							throw new ArgumentException(JScriptException.Localize("Too many arguments", locale));
						}
						object obj = args[0];
						if (obj is int)
						{
							return this[(int)obj];
						}
						IConvertible iconvertible = Convert.GetIConvertible(obj);
						if (iconvertible != null && Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
						{
							double num = iconvertible.ToDouble(null);
							if (num >= 0.0 && num <= 2147483647.0 && num == Math.Round(num))
							{
								return this[(int)num];
							}
						}
						return this[Convert.ToString(obj)];
					}
				}
				else
				{
					if ((invokeAttr & (BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.PutDispProperty)) == BindingFlags.Default)
					{
						throw new ArgumentException(JScriptException.Localize("Bad binding flags", locale));
					}
					if (flag2)
					{
						if (args == null || args.Length < 2)
						{
							throw new ArgumentException(JScriptException.Localize("Too few arguments", locale));
						}
						if (args.Length > 2)
						{
							throw new ArgumentException(JScriptException.Localize("Too many arguments", locale));
						}
						object obj2 = args[0];
						if (obj2 is int)
						{
							this[(int)obj2] = args[1];
							return null;
						}
						IConvertible iconvertible2 = Convert.GetIConvertible(obj2);
						if (iconvertible2 != null && Convert.IsPrimitiveNumericTypeCode(iconvertible2.GetTypeCode()))
						{
							double num2 = iconvertible2.ToDouble(null);
							if (num2 >= 0.0 && num2 <= 2147483647.0 && num2 == Math.Round(num2))
							{
								this[(int)num2] = args[1];
								return null;
							}
						}
						this[Convert.ToString(obj2)] = args[1];
						return null;
					}
					else
					{
						if (args == null || args.Length < 1)
						{
							throw new ArgumentException(JScriptException.Localize("Too few arguments", locale));
						}
						if (args.Length > 1)
						{
							throw new ArgumentException(JScriptException.Localize("Too many arguments", locale));
						}
						this.SetMemberValue(name, args[0]);
						return null;
					}
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002D30 File Offset: 0x00001D30
		internal virtual void SetMemberValue(string name, object value)
		{
			MemberInfo[] member = this.GetMember(name, BindingFlags.Instance | BindingFlags.Public);
			LateBinding.SetMemberValue(this, name, value, LateBinding.SelectMember(member), member);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002D56 File Offset: 0x00001D56
		internal void SetParent(ScriptObject parent)
		{
			this.parent = parent;
			if (parent != null)
			{
				this.engine = parent.engine;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002D6E File Offset: 0x00001D6E
		internal virtual void SetValueAtIndex(uint index, object value)
		{
			this.SetMemberValue(index.ToString(CultureInfo.InvariantCulture), value);
		}

		// Token: 0x17000001 RID: 1
		public object this[double index]
		{
			get
			{
				if (this == null)
				{
					throw new JScriptException(JSError.ObjectExpected);
				}
				object obj;
				if (index >= 0.0 && index <= 4294967295.0 && index == Math.Round(index))
				{
					obj = this.GetValueAtIndex((uint)index);
				}
				else
				{
					obj = this.GetMemberValue(Convert.ToString(index));
				}
				if (!(obj is Missing))
				{
					return obj;
				}
				return null;
			}
			set
			{
				if (index >= 0.0 && index <= 4294967295.0 && index == Math.Round(index))
				{
					this.SetValueAtIndex((uint)index, value);
					return;
				}
				this.SetMemberValue(Convert.ToString(index), value);
			}
		}

		// Token: 0x17000002 RID: 2
		public object this[int index]
		{
			get
			{
				if (this == null)
				{
					throw new JScriptException(JSError.ObjectExpected);
				}
				object obj;
				if (index >= 0)
				{
					obj = this.GetValueAtIndex((uint)index);
				}
				else
				{
					obj = this.GetMemberValue(Convert.ToString((double)index));
				}
				if (!(obj is Missing))
				{
					return obj;
				}
				return null;
			}
			set
			{
				if (this == null)
				{
					throw new JScriptException(JSError.ObjectExpected);
				}
				if (index >= 0)
				{
					this.SetValueAtIndex((uint)index, value);
					return;
				}
				this.SetMemberValue(Convert.ToString((double)index), value);
			}
		}

		// Token: 0x17000003 RID: 3
		public object this[string name]
		{
			get
			{
				if (this == null)
				{
					throw new JScriptException(JSError.ObjectExpected);
				}
				object memberValue = this.GetMemberValue(name);
				if (!(memberValue is Missing))
				{
					return memberValue;
				}
				return null;
			}
			set
			{
				if (this == null)
				{
					throw new JScriptException(JSError.ObjectExpected);
				}
				this.SetMemberValue(name, value);
			}
		}

		// Token: 0x17000004 RID: 4
		public object this[params object[] pars]
		{
			get
			{
				int num = pars.Length;
				if (num == 0)
				{
					if (this is ScriptFunction || this == null)
					{
						throw new JScriptException(JSError.FunctionExpected);
					}
					throw new JScriptException(JSError.TooFewParameters);
				}
				else
				{
					if (this == null)
					{
						throw new JScriptException(JSError.ObjectExpected);
					}
					object obj = pars[num - 1];
					if (obj is int)
					{
						return this[(int)obj];
					}
					IConvertible iconvertible = Convert.GetIConvertible(obj);
					if (iconvertible != null && Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
					{
						double num2 = iconvertible.ToDouble(null);
						if (num2 >= 0.0 && num2 <= 2147483647.0 && num2 == Math.Round(num2))
						{
							return this[(int)num2];
						}
					}
					return this[Convert.ToString(obj)];
				}
			}
			set
			{
				int num = pars.Length;
				if (num == 0)
				{
					if (this == null)
					{
						throw new JScriptException(JSError.FunctionExpected);
					}
					if (this is ScriptFunction)
					{
						throw new JScriptException(JSError.CannotAssignToFunctionResult);
					}
					throw new JScriptException(JSError.TooFewParameters);
				}
				else
				{
					if (this == null)
					{
						throw new JScriptException(JSError.ObjectExpected);
					}
					object obj = pars[num - 1];
					if (obj is int)
					{
						this[(int)obj] = value;
						return;
					}
					IConvertible iconvertible = Convert.GetIConvertible(obj);
					if (iconvertible != null && Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
					{
						double num2 = iconvertible.ToDouble(null);
						if (num2 >= 0.0 && num2 <= 2147483647.0 && num2 == Math.Round(num2))
						{
							this[(int)num2] = value;
							return;
						}
					}
					this[Convert.ToString(obj)] = value;
					return;
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000304D File Offset: 0x0000204D
		public virtual Type UnderlyingSystemType
		{
			get
			{
				return base.GetType();
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003058 File Offset: 0x00002058
		protected static MemberInfo[] WrapMembers(MemberInfo[] members, object obj)
		{
			if (members == null)
			{
				return null;
			}
			int num = members.Length;
			if (num == 0)
			{
				return members;
			}
			MemberInfo[] array = new MemberInfo[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ScriptObject.WrapMember(members[i], obj);
			}
			return array;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003094 File Offset: 0x00002094
		protected static MemberInfo[] WrapMembers(MemberInfo member, object obj)
		{
			return new MemberInfo[] { ScriptObject.WrapMember(member, obj) };
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000030B4 File Offset: 0x000020B4
		protected static MemberInfo[] WrapMembers(MemberInfo[] members, object obj, SimpleHashtable cache)
		{
			if (members == null)
			{
				return null;
			}
			int num = members.Length;
			if (num == 0)
			{
				return members;
			}
			MemberInfo[] array = new MemberInfo[num];
			for (int i = 0; i < num; i++)
			{
				MemberInfo memberInfo = (MemberInfo)cache[members[i]];
				if (memberInfo == null)
				{
					memberInfo = ScriptObject.WrapMember(members[i], obj);
					cache[members[i]] = memberInfo;
				}
				array[i] = memberInfo;
			}
			return array;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003110 File Offset: 0x00002110
		internal static MemberInfo WrapMember(MemberInfo member, object obj)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType != MemberTypes.Field)
			{
				if (memberType != MemberTypes.Method)
				{
					if (memberType != MemberTypes.Property)
					{
						return member;
					}
					PropertyInfo propertyInfo = (PropertyInfo)member;
					if (propertyInfo is JSWrappedProperty)
					{
						return propertyInfo;
					}
					MethodInfo getMethod = JSProperty.GetGetMethod(propertyInfo, true);
					MethodInfo setMethod = JSProperty.GetSetMethod(propertyInfo, true);
					if ((getMethod == null || getMethod.IsStatic) && (setMethod == null || setMethod.IsStatic))
					{
						return propertyInfo;
					}
					return new JSWrappedProperty(propertyInfo, obj);
				}
				else
				{
					MethodInfo methodInfo = (MethodInfo)member;
					if (methodInfo.IsStatic)
					{
						return methodInfo;
					}
					if (!(methodInfo is JSWrappedMethod))
					{
						return new JSWrappedMethod(methodInfo, obj);
					}
					return methodInfo;
				}
			}
			else
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				if (fieldInfo.IsStatic || fieldInfo.IsLiteral)
				{
					return fieldInfo;
				}
				if (!(fieldInfo is JSWrappedField))
				{
					return new JSWrappedField(fieldInfo, obj);
				}
				return fieldInfo;
			}
		}

		// Token: 0x04000010 RID: 16
		protected ScriptObject parent;

		// Token: 0x04000011 RID: 17
		internal SimpleHashtable wrappedMemberCache;

		// Token: 0x04000012 RID: 18
		public VsaEngine engine;
	}
}
