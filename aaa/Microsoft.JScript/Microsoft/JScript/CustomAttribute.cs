using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000055 RID: 85
	internal sealed class CustomAttribute : AST
	{
		// Token: 0x06000437 RID: 1079 RVA: 0x0001FE20 File Offset: 0x0001EE20
		internal CustomAttribute(Context context, AST func, ASTList args)
			: base(context)
		{
			this.ctor = func;
			this.args = args;
			this.target = null;
			this.type = null;
			this.positionalArgValues = new ArrayList();
			this.namedArgFields = new ArrayList();
			this.namedArgFieldValues = new ArrayList();
			this.namedArgProperties = new ArrayList();
			this.namedArgPropertyValues = new ArrayList();
			this.raiseToPropertyLevel = false;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001FE90 File Offset: 0x0001EE90
		private bool CheckIfTargetOK(object caType)
		{
			if (caType == null)
			{
				return false;
			}
			Type type = caType as Type;
			AttributeTargets attributeTargets;
			if (type != null)
			{
				object[] customAttributes = CustomAttribute.GetCustomAttributes(type, typeof(AttributeUsageAttribute), true);
				attributeTargets = ((AttributeUsageAttribute)customAttributes[0]).ValidOn;
			}
			else
			{
				attributeTargets = ((ClassScope)caType).owner.validOn;
			}
			object obj = this.target;
			Class @class = obj as Class;
			if (@class != null)
			{
				if (@class.isInterface)
				{
					if ((attributeTargets & AttributeTargets.Interface) != (AttributeTargets)0)
					{
						return true;
					}
				}
				else if (@class is EnumDeclaration)
				{
					if ((attributeTargets & AttributeTargets.Enum) != (AttributeTargets)0)
					{
						return true;
					}
				}
				else
				{
					if ((attributeTargets & AttributeTargets.Class) != (AttributeTargets)0)
					{
						if (type == typeof(AttributeUsageAttribute))
						{
							if (this.positionalArgValues.Count > 0)
							{
								object obj2 = this.positionalArgValues[0];
								if (obj2 is AttributeTargets)
								{
									@class.validOn = (AttributeTargets)obj2;
								}
							}
							int i = 0;
							int count = this.namedArgProperties.Count;
							while (i < count)
							{
								PropertyInfo propertyInfo = this.namedArgProperties[i] as PropertyInfo;
								if (propertyInfo.Name == "AllowMultiple")
								{
									@class.allowMultiple = (bool)this.namedArgPropertyValues[i];
								}
								i++;
							}
						}
						return true;
					}
					if (type.FullName == "System.NonSerializedAttribute")
					{
						@class.attributes &= ~TypeAttributes.Serializable;
						return false;
					}
				}
				this.context.HandleError(JSError.InvalidCustomAttributeTarget, CustomAttribute.GetTypeName(caType));
				return false;
			}
			FunctionDeclaration functionDeclaration = obj as FunctionDeclaration;
			if (functionDeclaration != null)
			{
				if ((attributeTargets & AttributeTargets.Property) != (AttributeTargets)0 && functionDeclaration.enclosingProperty != null)
				{
					if (functionDeclaration.enclosingProperty.getter == null || ((JSFieldMethod)functionDeclaration.enclosingProperty.getter).func == functionDeclaration.func)
					{
						this.raiseToPropertyLevel = true;
						return true;
					}
					this.context.HandleError(JSError.PropertyLevelAttributesMustBeOnGetter);
					return false;
				}
				else
				{
					if ((attributeTargets & AttributeTargets.Method) != (AttributeTargets)0 && functionDeclaration.isMethod)
					{
						return true;
					}
					if ((attributeTargets & AttributeTargets.Constructor) != (AttributeTargets)0 && functionDeclaration.func.isConstructor)
					{
						return true;
					}
					this.context.HandleError(JSError.InvalidCustomAttributeTarget, CustomAttribute.GetTypeName(caType));
					return false;
				}
			}
			else if (obj is VariableDeclaration || obj is Constant)
			{
				if ((attributeTargets & AttributeTargets.Field) != (AttributeTargets)0)
				{
					return true;
				}
				this.context.HandleError(JSError.InvalidCustomAttributeTarget, CustomAttribute.GetTypeName(caType));
				return false;
			}
			else
			{
				if (obj is AssemblyCustomAttributeList && (attributeTargets & AttributeTargets.Assembly) != (AttributeTargets)0)
				{
					return true;
				}
				if (obj == null && (attributeTargets & AttributeTargets.Parameter) != (AttributeTargets)0)
				{
					return true;
				}
				this.context.HandleError(JSError.InvalidCustomAttributeTarget, CustomAttribute.GetTypeName(caType));
				return false;
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0002011C File Offset: 0x0001F11C
		private static ushort DaysSince2000()
		{
			return (ushort)(DateTime.Now - new DateTime(2000, 1, 1)).Days;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00020148 File Offset: 0x0001F148
		internal override object Evaluate()
		{
			ConstructorInfo constructorInfo = (ConstructorInfo)((Binding)this.ctor).member;
			ParameterInfo[] parameters = constructorInfo.GetParameters();
			int num = parameters.Length;
			for (int i = this.positionalArgValues.Count; i < num; i++)
			{
				this.positionalArgValues.Add(Convert.CoerceT(null, parameters[i].ParameterType));
			}
			object[] array = new object[num];
			this.positionalArgValues.CopyTo(0, array, 0, num);
			object obj = constructorInfo.Invoke(BindingFlags.ExactBinding, null, array, null);
			int j = 0;
			int count = this.namedArgProperties.Count;
			while (j < count)
			{
				JSProperty jsproperty = this.namedArgProperties[j] as JSProperty;
				if (jsproperty != null)
				{
					jsproperty.SetValue(obj, Convert.Coerce(this.namedArgPropertyValues[j], jsproperty.PropertyIR()), null);
				}
				else
				{
					((PropertyInfo)this.namedArgProperties[j]).SetValue(obj, this.namedArgPropertyValues[j], null);
				}
				j++;
			}
			int k = 0;
			int count2 = this.namedArgFields.Count;
			while (k < count2)
			{
				JSVariableField jsvariableField = this.namedArgFields[k] as JSVariableField;
				if (jsvariableField != null)
				{
					jsvariableField.SetValue(obj, Convert.Coerce(this.namedArgFieldValues[k], jsvariableField.GetInferredType(null)));
				}
				else
				{
					((FieldInfo)this.namedArgFields[k]).SetValue(obj, this.namedArgFieldValues[k]);
				}
				k++;
			}
			return obj;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000202D4 File Offset: 0x0001F2D4
		internal CLSComplianceSpec GetCLSComplianceValue()
		{
			if (!(bool)this.positionalArgValues[0])
			{
				return CLSComplianceSpec.NonCLSCompliant;
			}
			return CLSComplianceSpec.CLSCompliant;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000202EC File Offset: 0x0001F2EC
		private void ConvertClassScopesAndEnumWrappers(ArrayList vals)
		{
			int i = 0;
			int count = vals.Count;
			while (i < count)
			{
				ClassScope classScope = vals[i] as ClassScope;
				if (classScope != null)
				{
					vals[i] = classScope.GetTypeBuilder();
				}
				else
				{
					EnumWrapper enumWrapper = vals[i] as EnumWrapper;
					if (enumWrapper != null)
					{
						vals[i] = enumWrapper.ToNumericValue();
					}
				}
				i++;
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00020348 File Offset: 0x0001F348
		private void ConvertFieldAndPropertyInfos(ArrayList vals)
		{
			int i = 0;
			int count = vals.Count;
			while (i < count)
			{
				JSField jsfield = vals[i] as JSField;
				if (jsfield != null)
				{
					vals[i] = jsfield.GetMetaData();
				}
				else
				{
					JSProperty jsproperty = vals[i] as JSProperty;
					if (jsproperty != null)
					{
						vals[i] = jsproperty.metaData;
					}
				}
				i++;
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000203A4 File Offset: 0x0001F3A4
		internal CustomAttributeBuilder GetCustomAttribute()
		{
			ConstructorInfo constructorInfo = (ConstructorInfo)((Binding)this.ctor).member;
			ParameterInfo[] parameters = constructorInfo.GetParameters();
			int num = parameters.Length;
			if (constructorInfo is JSConstructor)
			{
				constructorInfo = ((JSConstructor)constructorInfo).GetConstructorInfo(base.compilerGlobals);
			}
			this.ConvertClassScopesAndEnumWrappers(this.positionalArgValues);
			this.ConvertClassScopesAndEnumWrappers(this.namedArgPropertyValues);
			this.ConvertClassScopesAndEnumWrappers(this.namedArgFieldValues);
			this.ConvertFieldAndPropertyInfos(this.namedArgProperties);
			this.ConvertFieldAndPropertyInfos(this.namedArgFields);
			for (int i = this.positionalArgValues.Count; i < num; i++)
			{
				this.positionalArgValues.Add(Convert.CoerceT(null, parameters[i].ParameterType));
			}
			object[] array = new object[num];
			this.positionalArgValues.CopyTo(0, array, 0, num);
			PropertyInfo[] array2 = new PropertyInfo[this.namedArgProperties.Count];
			this.namedArgProperties.CopyTo(array2);
			object[] array3 = new object[this.namedArgPropertyValues.Count];
			this.namedArgPropertyValues.CopyTo(array3);
			FieldInfo[] array4 = new FieldInfo[this.namedArgFields.Count];
			this.namedArgFields.CopyTo(array4);
			object[] array5 = new object[this.namedArgFieldValues.Count];
			this.namedArgFieldValues.CopyTo(array5);
			return new CustomAttributeBuilder(constructorInfo, array, array2, array3, array4, array5);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000204FC File Offset: 0x0001F4FC
		internal object GetTypeIfAttributeHasToBeUnique()
		{
			Type type = this.type as Type;
			if (type != null)
			{
				object[] customAttributes = CustomAttribute.GetCustomAttributes(type, typeof(AttributeUsageAttribute), false);
				if (customAttributes.Length > 0 && !((AttributeUsageAttribute)customAttributes[0]).AllowMultiple)
				{
					return type;
				}
				return null;
			}
			else
			{
				if (!((ClassScope)this.type).owner.allowMultiple)
				{
					return this.type;
				}
				return null;
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00020564 File Offset: 0x0001F564
		private static string GetTypeName(object t)
		{
			Type type = t as Type;
			if (type != null)
			{
				return type.FullName;
			}
			return ((ClassScope)t).GetFullName();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00020590 File Offset: 0x0001F590
		internal bool IsExpandoAttribute()
		{
			Lookup lookup = this.ctor as Lookup;
			return lookup != null && lookup.Name == "expando";
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000205C0 File Offset: 0x0001F5C0
		internal override AST PartiallyEvaluate()
		{
			this.ctor = this.ctor.PartiallyEvaluateAsCallable();
			ASTList astlist = new ASTList(this.args.context);
			ASTList astlist2 = new ASTList(this.args.context);
			int i = 0;
			int count = this.args.count;
			while (i < count)
			{
				AST ast = this.args[i];
				Assign assign = ast as Assign;
				if (assign != null)
				{
					assign.rhside = assign.rhside.PartiallyEvaluate();
					astlist2.Append(assign);
				}
				else
				{
					astlist.Append(ast.PartiallyEvaluate());
				}
				i++;
			}
			int count2 = astlist.count;
			IReflect[] array = new IReflect[count2];
			int j = 0;
			while (j < count2)
			{
				AST ast2 = astlist[j];
				if (ast2 is ConstantWrapper)
				{
					object obj = ast2.Evaluate();
					if ((array[j] = CustomAttribute.TypeOfArgument(obj)) == null)
					{
						goto IL_0120;
					}
					this.positionalArgValues.Add(obj);
				}
				else
				{
					if (!(ast2 is ArrayLiteral) || !((ArrayLiteral)ast2).IsOkToUseInCustomAttribute())
					{
						goto IL_0120;
					}
					array[j] = Typeob.ArrayObject;
					this.positionalArgValues.Add(ast2.Evaluate());
				}
				j++;
				continue;
				IL_0120:
				ast2.context.HandleError(JSError.InvalidCustomAttributeArgument);
				return null;
			}
			this.type = this.ctor.ResolveCustomAttribute(astlist, array, this.target);
			if (this.type == null)
			{
				return null;
			}
			if (Convert.IsPromotableTo((IReflect)this.type, Typeob.CodeAccessSecurityAttribute))
			{
				this.context.HandleError(JSError.CannotUseStaticSecurityAttribute);
				return null;
			}
			ConstructorInfo constructorInfo = (ConstructorInfo)((Binding)this.ctor).member;
			ParameterInfo[] parameters = constructorInfo.GetParameters();
			int num = 0;
			int count3 = this.positionalArgValues.Count;
			foreach (ParameterInfo parameterInfo in parameters)
			{
				IReflect reflect = ((parameterInfo is ParameterDeclaration) ? ((ParameterDeclaration)parameterInfo).ParameterIReflect : parameterInfo.ParameterType);
				if (num < count3)
				{
					object obj2 = this.positionalArgValues[num];
					this.positionalArgValues[num] = Convert.Coerce(obj2, reflect, obj2 is ArrayObject);
					num++;
				}
				else
				{
					object obj3;
					if (TypeReferences.GetDefaultParameterValue(parameterInfo) == Convert.DBNull)
					{
						obj3 = Convert.Coerce(null, reflect);
					}
					else
					{
						obj3 = TypeReferences.GetDefaultParameterValue(parameterInfo);
					}
					this.positionalArgValues.Add(obj3);
				}
			}
			int l = 0;
			int count4 = astlist2.count;
			while (l < count4)
			{
				Assign assign2 = (Assign)astlist2[l];
				if (assign2.lhside is Lookup && (assign2.rhside is ConstantWrapper || (assign2.rhside is ArrayLiteral && ((ArrayLiteral)assign2.rhside).IsOkToUseInCustomAttribute())))
				{
					object obj4 = assign2.rhside.Evaluate();
					IReflect reflect2;
					if (obj4 is ArrayObject || ((reflect2 = CustomAttribute.TypeOfArgument(obj4)) != null && reflect2 != Typeob.Object))
					{
						string name = ((Lookup)assign2.lhside).Name;
						MemberInfo[] member = ((IReflect)this.type).GetMember(name, BindingFlags.Instance | BindingFlags.Public);
						if (member == null || member.Length == 0)
						{
							assign2.context.HandleError(JSError.NoSuchMember);
							return null;
						}
						if (member.Length == 1)
						{
							MemberInfo memberInfo = member[0];
							if (!(memberInfo is FieldInfo))
							{
								goto IL_0401;
							}
							FieldInfo fieldInfo = (FieldInfo)memberInfo;
							if (!fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
							{
								try
								{
									IReflect reflect3 = ((fieldInfo is JSVariableField) ? ((JSVariableField)fieldInfo).GetInferredType(null) : fieldInfo.FieldType);
									obj4 = Convert.Coerce(obj4, reflect3, obj4 is ArrayObject);
									this.namedArgFields.Add(memberInfo);
									this.namedArgFieldValues.Add(obj4);
									goto IL_04C3;
								}
								catch (JScriptException)
								{
									assign2.rhside.context.HandleError(JSError.TypeMismatch);
									return null;
								}
								goto IL_0401;
							}
							goto IL_04B0;
							IL_04C3:
							l++;
							continue;
							IL_0401:
							if (!(memberInfo is PropertyInfo))
							{
								goto IL_04B0;
							}
							PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
							MethodInfo setMethod = JSProperty.GetSetMethod(propertyInfo, false);
							if (setMethod == null)
							{
								goto IL_04B0;
							}
							ParameterInfo[] parameters2 = setMethod.GetParameters();
							if (parameters2 != null && parameters2.Length == 1)
							{
								try
								{
									IReflect reflect4 = ((parameters2[0] is ParameterDeclaration) ? ((ParameterDeclaration)parameters2[0]).ParameterIReflect : parameters2[0].ParameterType);
									obj4 = Convert.Coerce(obj4, reflect4, obj4 is ArrayObject);
									this.namedArgProperties.Add(memberInfo);
									this.namedArgPropertyValues.Add(obj4);
									goto IL_04C3;
								}
								catch (JScriptException)
								{
									assign2.rhside.context.HandleError(JSError.TypeMismatch);
									return null;
								}
							}
						}
					}
				}
				IL_04B0:
				assign2.context.HandleError(JSError.InvalidCustomAttributeArgument);
				return null;
			}
			if (!this.CheckIfTargetOK(this.type))
			{
				return null;
			}
			try
			{
				Type type = this.type as Type;
				if (type != null && this.target is AssemblyCustomAttributeList)
				{
					if (type.FullName == "System.Reflection.AssemblyAlgorithmIdAttribute")
					{
						if (this.positionalArgValues.Count > 0)
						{
							base.Engine.Globals.assemblyHashAlgorithm = (AssemblyHashAlgorithm)Convert.CoerceT(this.positionalArgValues[0], typeof(AssemblyHashAlgorithm));
						}
						return null;
					}
					if (type.FullName == "System.Reflection.AssemblyCultureAttribute")
					{
						if (this.positionalArgValues.Count > 0)
						{
							string text = Convert.ToString(this.positionalArgValues[0]);
							if (base.Engine.PEFileKind != PEFileKinds.Dll && text.Length > 0)
							{
								this.context.HandleError(JSError.ExecutablesCannotBeLocalized);
								return null;
							}
							base.Engine.Globals.assemblyCulture = new CultureInfo(text);
						}
						return null;
					}
					if (type.FullName == "System.Reflection.AssemblyDelaySignAttribute")
					{
						if (this.positionalArgValues.Count > 0)
						{
							base.Engine.Globals.assemblyDelaySign = Convert.ToBoolean(this.positionalArgValues[0], false);
						}
						return null;
					}
					if (type.FullName == "System.Reflection.AssemblyFlagsAttribute")
					{
						if (this.positionalArgValues.Count > 0)
						{
							base.Engine.Globals.assemblyFlags = (AssemblyFlags)((uint)Convert.CoerceT(this.positionalArgValues[0], typeof(uint)));
						}
						return null;
					}
					if (type.FullName == "System.Reflection.AssemblyKeyFileAttribute")
					{
						if (this.positionalArgValues.Count > 0)
						{
							base.Engine.Globals.assemblyKeyFileName = Convert.ToString(this.positionalArgValues[0]);
							base.Engine.Globals.assemblyKeyFileNameContext = this.context;
							if (base.Engine.Globals.assemblyKeyFileName != null && base.Engine.Globals.assemblyKeyFileName.Length == 0)
							{
								base.Engine.Globals.assemblyKeyFileName = null;
								base.Engine.Globals.assemblyKeyFileNameContext = null;
							}
						}
						return null;
					}
					if (type.FullName == "System.Reflection.AssemblyKeyNameAttribute")
					{
						if (this.positionalArgValues.Count > 0)
						{
							base.Engine.Globals.assemblyKeyName = Convert.ToString(this.positionalArgValues[0]);
							base.Engine.Globals.assemblyKeyNameContext = this.context;
							if (base.Engine.Globals.assemblyKeyName != null && base.Engine.Globals.assemblyKeyName.Length == 0)
							{
								base.Engine.Globals.assemblyKeyName = null;
								base.Engine.Globals.assemblyKeyNameContext = null;
							}
						}
						return null;
					}
					if (type.FullName == "System.Reflection.AssemblyVersionAttribute")
					{
						if (this.positionalArgValues.Count > 0)
						{
							base.Engine.Globals.assemblyVersion = this.ParseVersion(Convert.ToString(this.positionalArgValues[0]));
						}
						return null;
					}
					if (type.FullName == "System.CLSCompliantAttribute")
					{
						base.Engine.isCLSCompliant = this.args == null || this.args.count == 0 || Convert.ToBoolean(this.positionalArgValues[0], false);
						return this;
					}
				}
			}
			catch (ArgumentException)
			{
				this.context.HandleError(JSError.InvalidCall);
			}
			return this;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00020EB0 File Offset: 0x0001FEB0
		private Version ParseVersion(string vString)
		{
			ushort num = 1;
			ushort num2 = 0;
			ushort num3 = 0;
			ushort num4 = 0;
			try
			{
				int length = vString.Length;
				int num5 = vString.IndexOf('.', 0);
				if (num5 < 0)
				{
					throw new Exception();
				}
				num = ushort.Parse(vString.Substring(0, num5), CultureInfo.InvariantCulture);
				int num6 = vString.IndexOf('.', num5 + 1);
				if (num6 < num5 + 1)
				{
					num2 = ushort.Parse(vString.Substring(num5 + 1, length - num5 - 1), CultureInfo.InvariantCulture);
				}
				else
				{
					num2 = ushort.Parse(vString.Substring(num5 + 1, num6 - num5 - 1), CultureInfo.InvariantCulture);
					if (vString[num6 + 1] == '*')
					{
						num3 = CustomAttribute.DaysSince2000();
						num4 = CustomAttribute.SecondsSinceMidnight();
					}
					else
					{
						int num7 = vString.IndexOf('.', num6 + 1);
						if (num7 < num6 + 1)
						{
							num3 = ushort.Parse(vString.Substring(num6 + 1, length - num6 - 1), CultureInfo.InvariantCulture);
						}
						else
						{
							num3 = ushort.Parse(vString.Substring(num6 + 1, num7 - num6 - 1), CultureInfo.InvariantCulture);
							if (vString[num7 + 1] == '*')
							{
								num4 = CustomAttribute.SecondsSinceMidnight();
							}
							else
							{
								num4 = ushort.Parse(vString.Substring(num7 + 1, length - num7 - 1), CultureInfo.InvariantCulture);
							}
						}
					}
				}
			}
			catch
			{
				this.args[0].context.HandleError(JSError.NotValidVersionString);
			}
			return new Version((int)num, (int)num2, (int)num3, (int)num4);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00021030 File Offset: 0x00020030
		private static ushort SecondsSinceMidnight()
		{
			TimeSpan timeSpan = DateTime.Now - DateTime.Today;
			return (ushort)((timeSpan.Hours * 60 * 60 + timeSpan.Minutes * 60 + timeSpan.Seconds) / 2);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00021070 File Offset: 0x00020070
		internal void SetTarget(AST target)
		{
			this.target = target;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00021079 File Offset: 0x00020079
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0002107B File Offset: 0x0002007B
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00021080 File Offset: 0x00020080
		internal static IReflect TypeOfArgument(object argument)
		{
			if (argument is Enum)
			{
				return argument.GetType();
			}
			if (argument is EnumWrapper)
			{
				return ((EnumWrapper)argument).classScopeOrType;
			}
			switch (Convert.GetTypeCode(argument))
			{
			case TypeCode.Empty:
			case TypeCode.DBNull:
				return Typeob.Object;
			case TypeCode.Object:
				if (argument is Type)
				{
					return Typeob.Type;
				}
				if (argument is ClassScope)
				{
					return Typeob.Type;
				}
				break;
			case TypeCode.Boolean:
				return Typeob.Boolean;
			case TypeCode.Char:
				return Typeob.Char;
			case TypeCode.SByte:
				return Typeob.SByte;
			case TypeCode.Byte:
				return Typeob.Byte;
			case TypeCode.Int16:
				return Typeob.Int16;
			case TypeCode.UInt16:
				return Typeob.UInt16;
			case TypeCode.Int32:
				return Typeob.Int32;
			case TypeCode.UInt32:
				return Typeob.UInt32;
			case TypeCode.Int64:
				return Typeob.Int64;
			case TypeCode.UInt64:
				return Typeob.UInt64;
			case TypeCode.Single:
				return Typeob.Single;
			case TypeCode.Double:
				return Typeob.Double;
			case TypeCode.String:
				return Typeob.String;
			}
			return null;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0002117C File Offset: 0x0002017C
		private static object GetCustomAttributeValue(CustomAttributeTypedArgument arg)
		{
			Type argumentType = arg.ArgumentType;
			if (argumentType.IsEnum)
			{
				return Enum.ToObject(Type.GetType(argumentType.FullName), arg.Value);
			}
			return arg.Value;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000211B8 File Offset: 0x000201B8
		internal static object[] GetCustomAttributes(Assembly target, Type caType, bool inherit)
		{
			if (!target.ReflectionOnly)
			{
				return target.GetCustomAttributes(caType, inherit);
			}
			return CustomAttribute.ExtractCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000211D7 File Offset: 0x000201D7
		internal static object[] GetCustomAttributes(Module target, Type caType, bool inherit)
		{
			if (!target.Assembly.ReflectionOnly)
			{
				return target.GetCustomAttributes(caType, inherit);
			}
			return CustomAttribute.ExtractCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000211FC File Offset: 0x000201FC
		internal static object[] GetCustomAttributes(MemberInfo target, Type caType, bool inherit)
		{
			Type type = target.GetType();
			if (type.Assembly == typeof(CustomAttribute).Assembly || !target.Module.Assembly.ReflectionOnly)
			{
				return target.GetCustomAttributes(caType, inherit);
			}
			return CustomAttribute.ExtractCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00021250 File Offset: 0x00020250
		internal static object[] GetCustomAttributes(ParameterInfo target, Type caType, bool inherit)
		{
			Type type = target.GetType();
			if (type.Assembly == typeof(CustomAttribute).Assembly || !target.Member.Module.Assembly.ReflectionOnly)
			{
				return target.GetCustomAttributes(caType, inherit);
			}
			return CustomAttribute.ExtractCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000212A8 File Offset: 0x000202A8
		private static object[] ExtractCustomAttribute(IList<CustomAttributeData> attributes, Type caType)
		{
			Type type = Globals.TypeRefs.ToReferenceContext(caType);
			foreach (CustomAttributeData customAttributeData in attributes)
			{
				if (customAttributeData.Constructor.DeclaringType == type)
				{
					ArrayList arrayList = new ArrayList();
					foreach (CustomAttributeTypedArgument customAttributeTypedArgument in customAttributeData.ConstructorArguments)
					{
						arrayList.Add(CustomAttribute.GetCustomAttributeValue(customAttributeTypedArgument));
					}
					object obj = Activator.CreateInstance(caType, arrayList.ToArray());
					foreach (CustomAttributeNamedArgument customAttributeNamedArgument in customAttributeData.NamedArguments)
					{
						caType.InvokeMember(customAttributeNamedArgument.MemberInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty, null, obj, new object[] { CustomAttribute.GetCustomAttributeValue(customAttributeNamedArgument.TypedValue) }, null, CultureInfo.InvariantCulture, null);
					}
					return new object[] { obj };
				}
			}
			return new object[0];
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00021400 File Offset: 0x00020400
		internal static bool IsDefined(MemberInfo target, Type caType, bool inherit)
		{
			Type type = target.GetType();
			if (type.Assembly == typeof(CustomAttribute).Assembly || !target.Module.Assembly.ReflectionOnly)
			{
				return target.IsDefined(caType, inherit);
			}
			return CustomAttribute.CheckForCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00021454 File Offset: 0x00020454
		internal static bool IsDefined(ParameterInfo target, Type caType, bool inherit)
		{
			Type type = target.GetType();
			if (type.Assembly == typeof(CustomAttribute).Assembly || !target.Member.Module.Assembly.ReflectionOnly)
			{
				return target.IsDefined(caType, inherit);
			}
			return CustomAttribute.CheckForCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000214AC File Offset: 0x000204AC
		private static bool CheckForCustomAttribute(IList<CustomAttributeData> attributes, Type caType)
		{
			Type type = Globals.TypeRefs.ToReferenceContext(caType);
			foreach (CustomAttributeData customAttributeData in attributes)
			{
				if (customAttributeData.Constructor.DeclaringType == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040001F6 RID: 502
		private AST ctor;

		// Token: 0x040001F7 RID: 503
		private ASTList args;

		// Token: 0x040001F8 RID: 504
		private AST target;

		// Token: 0x040001F9 RID: 505
		internal object type;

		// Token: 0x040001FA RID: 506
		private ArrayList positionalArgValues;

		// Token: 0x040001FB RID: 507
		private ArrayList namedArgFields;

		// Token: 0x040001FC RID: 508
		private ArrayList namedArgFieldValues;

		// Token: 0x040001FD RID: 509
		private ArrayList namedArgProperties;

		// Token: 0x040001FE RID: 510
		private ArrayList namedArgPropertyValues;

		// Token: 0x040001FF RID: 511
		internal bool raiseToPropertyLevel;
	}
}
