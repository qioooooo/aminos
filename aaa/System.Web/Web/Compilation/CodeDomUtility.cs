using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000164 RID: 356
	internal static class CodeDomUtility
	{
		// Token: 0x0600100A RID: 4106 RVA: 0x0004717C File Offset: 0x0004617C
		internal static CodeExpression GenerateExpressionForValue(PropertyInfo propertyInfo, object value, Type valueType)
		{
			CodeExpression codeExpression = null;
			if (valueType == null)
			{
				throw new ArgumentNullException("valueType");
			}
			PropertyDescriptor propertyDescriptor = null;
			if (propertyInfo != null)
			{
				propertyDescriptor = TypeDescriptor.GetProperties(propertyInfo.ReflectedType)[propertyInfo.Name];
			}
			if (valueType == typeof(string) && value is string)
			{
				bool enabled = CodeDomUtility.WebFormsCompilation.Enabled;
				codeExpression = new CodePrimitiveExpression((string)value);
			}
			else if (valueType.IsPrimitive)
			{
				bool enabled2 = CodeDomUtility.WebFormsCompilation.Enabled;
				codeExpression = new CodePrimitiveExpression(value);
			}
			else if (propertyInfo == null && valueType == typeof(object) && (value == null || value.GetType().IsPrimitive))
			{
				bool enabled3 = CodeDomUtility.WebFormsCompilation.Enabled;
				codeExpression = new CodePrimitiveExpression(value);
			}
			else if (valueType.IsArray)
			{
				bool enabled4 = CodeDomUtility.WebFormsCompilation.Enabled;
				Array array = (Array)value;
				CodeArrayCreateExpression codeArrayCreateExpression = new CodeArrayCreateExpression();
				codeArrayCreateExpression.CreateType = new CodeTypeReference(valueType.GetElementType());
				if (array != null)
				{
					foreach (object obj in array)
					{
						codeArrayCreateExpression.Initializers.Add(CodeDomUtility.GenerateExpressionForValue(null, obj, valueType.GetElementType()));
					}
				}
				codeExpression = codeArrayCreateExpression;
			}
			else if (valueType == typeof(Type))
			{
				codeExpression = new CodeTypeOfExpression((Type)value);
			}
			else
			{
				bool enabled5 = CodeDomUtility.WebFormsCompilation.Enabled;
				TypeConverter typeConverter;
				if (propertyDescriptor != null)
				{
					typeConverter = propertyDescriptor.Converter;
				}
				else
				{
					typeConverter = TypeDescriptor.GetConverter(valueType);
				}
				bool flag = false;
				if (typeConverter != null)
				{
					InstanceDescriptor instanceDescriptor = null;
					if (typeConverter.CanConvertTo(typeof(InstanceDescriptor)))
					{
						instanceDescriptor = (InstanceDescriptor)typeConverter.ConvertTo(value, typeof(InstanceDescriptor));
					}
					if (instanceDescriptor != null)
					{
						bool enabled6 = CodeDomUtility.WebFormsCompilation.Enabled;
						if (instanceDescriptor.MemberInfo is FieldInfo)
						{
							bool enabled7 = CodeDomUtility.WebFormsCompilation.Enabled;
							CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(instanceDescriptor.MemberInfo.DeclaringType.FullName), instanceDescriptor.MemberInfo.Name);
							codeExpression = codeFieldReferenceExpression;
							flag = true;
						}
						else if (instanceDescriptor.MemberInfo is PropertyInfo)
						{
							bool enabled8 = CodeDomUtility.WebFormsCompilation.Enabled;
							CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(instanceDescriptor.MemberInfo.DeclaringType.FullName), instanceDescriptor.MemberInfo.Name);
							codeExpression = codePropertyReferenceExpression;
							flag = true;
						}
						else
						{
							object[] array2 = new object[instanceDescriptor.Arguments.Count];
							instanceDescriptor.Arguments.CopyTo(array2, 0);
							CodeExpression[] array3 = new CodeExpression[array2.Length];
							if (instanceDescriptor.MemberInfo is MethodInfo)
							{
								MethodInfo methodInfo = (MethodInfo)instanceDescriptor.MemberInfo;
								ParameterInfo[] parameters = methodInfo.GetParameters();
								for (int i = 0; i < array2.Length; i++)
								{
									array3[i] = CodeDomUtility.GenerateExpressionForValue(null, array2[i], parameters[i].ParameterType);
								}
								bool enabled9 = CodeDomUtility.WebFormsCompilation.Enabled;
								CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(instanceDescriptor.MemberInfo.DeclaringType.FullName), instanceDescriptor.MemberInfo.Name, new CodeExpression[0]);
								foreach (CodeExpression codeExpression2 in array3)
								{
									codeMethodInvokeExpression.Parameters.Add(codeExpression2);
								}
								codeExpression = new CodeCastExpression(valueType, codeMethodInvokeExpression);
								flag = true;
							}
							else if (instanceDescriptor.MemberInfo is ConstructorInfo)
							{
								ConstructorInfo constructorInfo = (ConstructorInfo)instanceDescriptor.MemberInfo;
								ParameterInfo[] parameters2 = constructorInfo.GetParameters();
								for (int k = 0; k < array2.Length; k++)
								{
									array3[k] = CodeDomUtility.GenerateExpressionForValue(null, array2[k], parameters2[k].ParameterType);
								}
								bool enabled10 = CodeDomUtility.WebFormsCompilation.Enabled;
								CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression(instanceDescriptor.MemberInfo.DeclaringType.FullName, new CodeExpression[0]);
								foreach (CodeExpression codeExpression3 in array3)
								{
									codeObjectCreateExpression.Parameters.Add(codeExpression3);
								}
								codeExpression = codeObjectCreateExpression;
								flag = true;
							}
						}
					}
				}
				if (!flag)
				{
					if (valueType.GetMethod("Parse", new Type[]
					{
						typeof(string),
						typeof(CultureInfo)
					}) != null)
					{
						CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(valueType.FullName), "Parse", new CodeExpression[0]);
						string text;
						if (typeConverter != null)
						{
							text = typeConverter.ConvertToInvariantString(value);
						}
						else
						{
							text = value.ToString();
						}
						codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(text));
						codeMethodInvokeExpression2.Parameters.Add(new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), "InvariantCulture"));
						codeExpression = codeMethodInvokeExpression2;
					}
					else
					{
						if (valueType.GetMethod("Parse", new Type[] { typeof(string) }) == null)
						{
							throw new HttpException(SR.GetString("CantGenPropertySet", new object[] { propertyInfo.Name, valueType.FullName }));
						}
						codeExpression = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(valueType.FullName), "Parse", new CodeExpression[0])
						{
							Parameters = 
							{
								new CodePrimitiveExpression(value.ToString())
							}
						};
					}
				}
			}
			return codeExpression;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x000476E8 File Offset: 0x000466E8
		internal static void CreatePropertySetStatements(CodeStatementCollection methodStatements, CodeStatementCollection statements, CodeExpression target, string targetPropertyName, Type destinationType, CodeExpression value, CodeLinePragma linePragma)
		{
			bool flag = false;
			if (destinationType == null)
			{
				flag = true;
			}
			if (flag)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
				CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(codeMethodInvokeExpression);
				codeExpressionStatement.LinePragma = linePragma;
				codeMethodInvokeExpression.Method.TargetObject = new CodeCastExpression(typeof(IAttributeAccessor), target);
				codeMethodInvokeExpression.Method.MethodName = "SetAttribute";
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(targetPropertyName));
				codeMethodInvokeExpression.Parameters.Add(CodeDomUtility.GenerateConvertToString(value));
				statements.Add(codeExpressionStatement);
				return;
			}
			if (destinationType.IsValueType)
			{
				statements.Add(new CodeAssignStatement(CodeDomUtility.BuildPropertyReferenceExpression(target, targetPropertyName), new CodeCastExpression(destinationType, value))
				{
					LinePragma = linePragma
				});
				return;
			}
			CodeExpression codeExpression;
			if (destinationType == typeof(string))
			{
				codeExpression = CodeDomUtility.GenerateConvertToString(value);
			}
			else
			{
				codeExpression = new CodeCastExpression(destinationType, value);
			}
			statements.Add(new CodeAssignStatement(CodeDomUtility.BuildPropertyReferenceExpression(target, targetPropertyName), codeExpression)
			{
				LinePragma = linePragma
			});
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x000477E4 File Offset: 0x000467E4
		internal static CodeExpression GenerateConvertToString(CodeExpression value)
		{
			return new CodeMethodInvokeExpression
			{
				Method = 
				{
					TargetObject = new CodeTypeReferenceExpression(typeof(Convert)),
					MethodName = "ToString"
				},
				Parameters = 
				{
					value,
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), "CurrentCulture")
				}
			};
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00047854 File Offset: 0x00046854
		internal static void PrependCompilerOption(CompilerParameters compilParams, string compilerOptions)
		{
			if (compilParams.CompilerOptions == null)
			{
				compilParams.CompilerOptions = compilerOptions;
				return;
			}
			compilParams.CompilerOptions = compilerOptions + " " + compilParams.CompilerOptions;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0004787D File Offset: 0x0004687D
		internal static void AppendCompilerOption(CompilerParameters compilParams, string compilerOptions)
		{
			if (compilParams.CompilerOptions == null)
			{
				compilParams.CompilerOptions = compilerOptions;
				return;
			}
			compilParams.CompilerOptions = compilParams.CompilerOptions + " " + compilerOptions;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x000478A8 File Offset: 0x000468A8
		internal static CodeExpression BuildPropertyReferenceExpression(CodeExpression objRefExpr, string propName)
		{
			string[] array = propName.Split(new char[] { '.' });
			CodeExpression codeExpression = objRefExpr;
			foreach (string text in array)
			{
				codeExpression = new CodePropertyReferenceExpression(codeExpression, text);
			}
			return codeExpression;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x000478F0 File Offset: 0x000468F0
		internal static CodeCastExpression BuildJSharpCastExpression(Type castType, CodeExpression expression)
		{
			return new CodeCastExpression(castType, expression)
			{
				UserData = { { "CastIsBoxing", true } }
			};
		}

		// Token: 0x04001639 RID: 5689
		internal static BooleanSwitch WebFormsCompilation = new BooleanSwitch("WebFormsCompilation", "Outputs information about the WebForms compilation of ASPX templates");
	}
}
