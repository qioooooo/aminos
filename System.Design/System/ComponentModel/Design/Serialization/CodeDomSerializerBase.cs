using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace System.ComponentModel.Design.Serialization
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class CodeDomSerializerBase
	{
		internal CodeDomSerializerBase()
		{
		}

		protected virtual object DeserializeInstance(IDesignerSerializationManager manager, Type type, object[] parameters, string name, bool addToContainer)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return manager.CreateInstance(type, parameters, name, addToContainer);
		}

		internal static string GetTypeNameFromCodeTypeReference(IDesignerSerializationManager manager, CodeTypeReference typeref)
		{
			if (typeref.TypeArguments == null || typeref.TypeArguments.Count == 0)
			{
				return typeref.BaseType;
			}
			return CodeDomSerializerBase.GetTypeNameFromCodeTypeReferenceHelper(manager, typeref);
		}

		private static string GetTypeNameFromCodeTypeReferenceHelper(IDesignerSerializationManager manager, CodeTypeReference typeref)
		{
			if (typeref.TypeArguments != null && typeref.TypeArguments.Count != 0)
			{
				StringBuilder stringBuilder = new StringBuilder(typeref.BaseType);
				if (!typeref.BaseType.Contains("`"))
				{
					stringBuilder.Append("`");
					stringBuilder.Append(typeref.TypeArguments.Count);
				}
				stringBuilder.Append("[");
				bool flag = true;
				foreach (object obj in typeref.TypeArguments)
				{
					CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
					if (!flag)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append("[");
					stringBuilder.Append(CodeDomSerializerBase.GetTypeNameFromCodeTypeReferenceHelper(manager, codeTypeReference));
					stringBuilder.Append("]");
					flag = false;
				}
				stringBuilder.Append("]");
				return stringBuilder.ToString();
			}
			Type type = manager.GetType(typeref.BaseType);
			if (type != null)
			{
				return type.AssemblyQualifiedName;
			}
			return typeref.BaseType;
		}

		private object DeserializePropertyReferenceExpression(IDesignerSerializationManager manager, CodePropertyReferenceExpression propertyReferenceEx, bool reportError)
		{
			object obj = propertyReferenceEx;
			object obj2 = this.DeserializeExpression(manager, null, propertyReferenceEx.TargetObject);
			if (obj2 != null && !(obj2 is CodeExpression))
			{
				if (!(obj2 is Type))
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj2)[propertyReferenceEx.PropertyName];
					if (propertyDescriptor != null)
					{
						obj = propertyDescriptor.GetValue(obj2);
					}
					else if (this.GetExpression(manager, obj2) is CodeThisReferenceExpression)
					{
						PropertyInfo property = TypeDescriptor.GetReflectionType(obj2).GetProperty(propertyReferenceEx.PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
						if (property != null)
						{
							obj = property.GetValue(obj2, null);
						}
					}
				}
				else
				{
					PropertyInfo property2 = TypeDescriptor.GetReflectionType((Type)obj2).GetProperty(propertyReferenceEx.PropertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
					if (property2 != null)
					{
						obj = property2.GetValue(null, null);
					}
				}
				if (obj == propertyReferenceEx && reportError)
				{
					string text = ((obj2 is Type) ? ((Type)obj2).FullName : TypeDescriptor.GetReflectionType(obj2).FullName);
					this.Error(manager, SR.GetString("SerializerNoSuchProperty", new object[] { text, propertyReferenceEx.PropertyName }), "SerializerNoSuchProperty");
				}
			}
			return obj;
		}

		protected object DeserializeExpression(IDesignerSerializationManager manager, string name, CodeExpression expression)
		{
			object obj = expression;
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::DeserializeExpression"))
			{
				if (obj != null)
				{
					CodePrimitiveExpression codePrimitiveExpression;
					CodePropertyReferenceExpression codePropertyReferenceExpression;
					CodeTypeReferenceExpression codeTypeReferenceExpression;
					CodeObjectCreateExpression codeObjectCreateExpression;
					CodeArgumentReferenceExpression codeArgumentReferenceExpression;
					CodeFieldReferenceExpression codeFieldReferenceExpression;
					CodeMethodInvokeExpression codeMethodInvokeExpression;
					CodeVariableReferenceExpression codeVariableReferenceExpression;
					CodeCastExpression codeCastExpression2;
					CodeArrayCreateExpression codeArrayCreateExpression;
					CodeArrayIndexerExpression codeArrayIndexerExpression;
					CodeBinaryOperatorExpression codeBinaryOperatorExpression;
					CodeDelegateInvokeExpression codeDelegateInvokeExpression;
					CodeDirectionExpression codeDirectionExpression;
					CodeIndexerExpression codeIndexerExpression;
					CodeParameterDeclarationExpression codeParameterDeclarationExpression;
					CodeTypeOfExpression codeTypeOfExpression;
					if ((codePrimitiveExpression = obj as CodePrimitiveExpression) != null)
					{
						obj = codePrimitiveExpression.Value;
					}
					else if ((codePropertyReferenceExpression = obj as CodePropertyReferenceExpression) != null)
					{
						obj = this.DeserializePropertyReferenceExpression(manager, codePropertyReferenceExpression, true);
					}
					else if (obj is CodeThisReferenceExpression)
					{
						RootContext rootContext = (RootContext)manager.Context[typeof(RootContext)];
						if (rootContext != null)
						{
							obj = rootContext.Value;
						}
						else
						{
							IDesignerHost designerHost = manager.GetService(typeof(IDesignerHost)) as IDesignerHost;
							if (designerHost != null)
							{
								obj = designerHost.RootComponent;
							}
						}
						if (obj == null)
						{
							this.Error(manager, SR.GetString("SerializerNoRootExpression"), "SerializerNoRootExpression");
						}
					}
					else if ((codeTypeReferenceExpression = obj as CodeTypeReferenceExpression) != null)
					{
						obj = manager.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeTypeReferenceExpression.Type));
					}
					else if ((codeObjectCreateExpression = obj as CodeObjectCreateExpression) != null)
					{
						obj = null;
						Type type = manager.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeObjectCreateExpression.CreateType));
						if (type != null)
						{
							object[] array = new object[codeObjectCreateExpression.Parameters.Count];
							bool flag = true;
							for (int i = 0; i < array.Length; i++)
							{
								array[i] = this.DeserializeExpression(manager, null, codeObjectCreateExpression.Parameters[i]);
								if (array[i] is CodeExpression)
								{
									if (typeof(Delegate).IsAssignableFrom(type) && array.Length == 1 && array[i] is CodeMethodReferenceExpression)
									{
										CodeMethodReferenceExpression codeMethodReferenceExpression = (CodeMethodReferenceExpression)array[i];
										if (!(codeMethodReferenceExpression.TargetObject is CodeThisReferenceExpression))
										{
											object obj2 = this.DeserializeExpression(manager, null, codeMethodReferenceExpression.TargetObject);
											if (!(obj2 is CodeExpression))
											{
												MethodInfo method = type.GetMethod("Invoke");
												if (method != null)
												{
													ParameterInfo[] parameters = method.GetParameters();
													Type[] array2 = new Type[parameters.Length];
													for (int j = 0; j < array2.Length; j++)
													{
														array2[j] = parameters[i].ParameterType;
													}
													MethodInfo method2 = TypeDescriptor.GetReflectionType(obj2).GetMethod(codeMethodReferenceExpression.MethodName, array2);
													if (method2 != null)
													{
														obj = Activator.CreateInstance(type, new object[]
														{
															obj2,
															method2.MethodHandle.GetFunctionPointer()
														});
													}
												}
											}
										}
									}
									flag = false;
									break;
								}
							}
							if (flag)
							{
								obj = this.DeserializeInstance(manager, type, array, name, name != null);
							}
						}
						else
						{
							this.Error(manager, SR.GetString("SerializerTypeNotFound", new object[] { codeObjectCreateExpression.CreateType.BaseType }), "SerializerTypeNotFound");
						}
					}
					else if ((codeArgumentReferenceExpression = obj as CodeArgumentReferenceExpression) != null)
					{
						obj = manager.GetInstance(codeArgumentReferenceExpression.ParameterName);
						if (obj == null)
						{
							this.Error(manager, SR.GetString("SerializerUndeclaredName", new object[] { codeArgumentReferenceExpression.ParameterName }), "SerializerUndeclaredName");
						}
					}
					else if ((codeFieldReferenceExpression = obj as CodeFieldReferenceExpression) != null)
					{
						object obj3 = this.DeserializeExpression(manager, null, codeFieldReferenceExpression.TargetObject);
						if (obj3 != null && !(obj3 is CodeExpression))
						{
							RootContext rootContext2 = (RootContext)manager.Context[typeof(RootContext)];
							if (rootContext2 != null && rootContext2.Value == obj3)
							{
								object instance = manager.GetInstance(codeFieldReferenceExpression.FieldName);
								if (instance != null)
								{
									obj = instance;
								}
								else
								{
									this.Error(manager, SR.GetString("SerializerUndeclaredName", new object[] { codeFieldReferenceExpression.FieldName }), "SerializerUndeclaredName");
								}
							}
							else
							{
								Type type2 = obj3 as Type;
								object obj4;
								FieldInfo fieldInfo;
								if (type2 != null)
								{
									obj4 = null;
									fieldInfo = TypeDescriptor.GetReflectionType(type2).GetField(codeFieldReferenceExpression.FieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
								}
								else
								{
									obj4 = obj3;
									fieldInfo = TypeDescriptor.GetReflectionType(obj3).GetField(codeFieldReferenceExpression.FieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
								}
								if (fieldInfo != null)
								{
									obj = fieldInfo.GetValue(obj4);
								}
								else
								{
									obj = this.DeserializePropertyReferenceExpression(manager, new CodePropertyReferenceExpression
									{
										TargetObject = codeFieldReferenceExpression.TargetObject,
										PropertyName = codeFieldReferenceExpression.FieldName
									}, false);
									if (obj == codeFieldReferenceExpression)
									{
										this.Error(manager, SR.GetString("SerializerUndeclaredName", new object[] { codeFieldReferenceExpression.FieldName }), "SerializerUndeclaredName");
									}
								}
							}
						}
						else
						{
							this.Error(manager, SR.GetString("SerializerFieldTargetEvalFailed", new object[] { codeFieldReferenceExpression.FieldName }), "SerializerFieldTargetEvalFailed");
						}
					}
					else if ((codeMethodInvokeExpression = obj as CodeMethodInvokeExpression) != null)
					{
						object obj5 = this.DeserializeExpression(manager, null, codeMethodInvokeExpression.Method.TargetObject);
						if (obj5 != null)
						{
							object[] array3 = new object[codeMethodInvokeExpression.Parameters.Count];
							bool flag2 = true;
							for (int k = 0; k < array3.Length; k++)
							{
								array3[k] = this.DeserializeExpression(manager, null, codeMethodInvokeExpression.Parameters[k]);
								if (array3[k] is CodeExpression)
								{
									flag2 = false;
									break;
								}
							}
							if (flag2)
							{
								IComponentChangeService componentChangeService = (IComponentChangeService)manager.GetService(typeof(IComponentChangeService));
								Type type3 = obj5 as Type;
								if (type3 != null)
								{
									obj = TypeDescriptor.GetReflectionType(type3).InvokeMember(codeMethodInvokeExpression.Method.MethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, array3, null, null, null);
								}
								else
								{
									if (componentChangeService != null)
									{
										componentChangeService.OnComponentChanging(obj5, null);
									}
									try
									{
										obj = TypeDescriptor.GetReflectionType(obj5).InvokeMember(codeMethodInvokeExpression.Method.MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, obj5, array3, null, null, null);
									}
									catch (MissingMethodException)
									{
										CodeCastExpression codeCastExpression = codeMethodInvokeExpression.Method.TargetObject as CodeCastExpression;
										if (codeCastExpression == null)
										{
											throw;
										}
										Type type4 = manager.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeCastExpression.TargetType));
										if (type4 == null || !type4.IsInterface)
										{
											throw;
										}
										obj = TypeDescriptor.GetReflectionType(type4).InvokeMember(codeMethodInvokeExpression.Method.MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, obj5, array3, null, null, null);
									}
									if (componentChangeService != null)
									{
										componentChangeService.OnComponentChanged(obj5, null, null, null);
									}
								}
							}
							else if (array3.Length == 1 && array3[0] is CodeDelegateCreateExpression)
							{
								string text = codeMethodInvokeExpression.Method.MethodName;
								if (text.StartsWith("add_"))
								{
									text = text.Substring(4);
									this.DeserializeAttachEventStatement(manager, new CodeAttachEventStatement(codeMethodInvokeExpression.Method.TargetObject, text, (CodeExpression)array3[0]));
									obj = null;
								}
							}
						}
					}
					else if ((codeVariableReferenceExpression = obj as CodeVariableReferenceExpression) != null)
					{
						obj = manager.GetInstance(codeVariableReferenceExpression.VariableName);
						if (obj == null)
						{
							this.Error(manager, SR.GetString("SerializerUndeclaredName", new object[] { codeVariableReferenceExpression.VariableName }), "SerializerUndeclaredName");
						}
					}
					else if ((codeCastExpression2 = obj as CodeCastExpression) != null)
					{
						obj = this.DeserializeExpression(manager, name, codeCastExpression2.Expression);
						IConvertible convertible = obj as IConvertible;
						if (convertible != null)
						{
							Type type5 = manager.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeCastExpression2.TargetType));
							if (type5 != null)
							{
								obj = convertible.ToType(type5, null);
							}
						}
					}
					else if (obj is CodeBaseReferenceExpression)
					{
						RootContext rootContext3 = (RootContext)manager.Context[typeof(RootContext)];
						if (rootContext3 != null)
						{
							obj = rootContext3.Value;
						}
						else
						{
							obj = null;
						}
					}
					else if ((codeArrayCreateExpression = obj as CodeArrayCreateExpression) != null)
					{
						Type type6 = manager.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeArrayCreateExpression.CreateType));
						Array array4 = null;
						if (type6 != null)
						{
							if (codeArrayCreateExpression.Initializers.Count > 0)
							{
								ArrayList arrayList = new ArrayList(codeArrayCreateExpression.Initializers.Count);
								foreach (object obj6 in codeArrayCreateExpression.Initializers)
								{
									CodeExpression codeExpression = (CodeExpression)obj6;
									try
									{
										object obj7 = this.DeserializeExpression(manager, null, codeExpression);
										if (!(obj7 is CodeExpression))
										{
											if (!type6.IsInstanceOfType(obj7))
											{
												obj7 = Convert.ChangeType(obj7, type6, CultureInfo.InvariantCulture);
											}
											arrayList.Add(obj7);
										}
									}
									catch (Exception ex)
									{
										manager.ReportError(ex);
									}
								}
								array4 = Array.CreateInstance(type6, arrayList.Count);
								arrayList.CopyTo(array4, 0);
							}
							else if (codeArrayCreateExpression.SizeExpression != null)
							{
								object obj8 = this.DeserializeExpression(manager, name, codeArrayCreateExpression.SizeExpression);
								IConvertible convertible2 = obj8 as IConvertible;
								if (convertible2 != null)
								{
									int num = convertible2.ToInt32(null);
									array4 = Array.CreateInstance(type6, num);
								}
							}
							else
							{
								array4 = Array.CreateInstance(type6, codeArrayCreateExpression.Size);
							}
						}
						else
						{
							this.Error(manager, SR.GetString("SerializerTypeNotFound", new object[] { codeArrayCreateExpression.CreateType.BaseType }), "SerializerTypeNotFound");
						}
						obj = array4;
						if (obj != null && name != null)
						{
							manager.SetName(obj, name);
						}
					}
					else if ((codeArrayIndexerExpression = obj as CodeArrayIndexerExpression) != null)
					{
						obj = null;
						Array array5 = this.DeserializeExpression(manager, name, codeArrayIndexerExpression.TargetObject) as Array;
						if (array5 != null)
						{
							int[] array6 = new int[codeArrayIndexerExpression.Indices.Count];
							bool flag3 = true;
							for (int l = 0; l < array6.Length; l++)
							{
								IConvertible convertible3 = this.DeserializeExpression(manager, name, codeArrayIndexerExpression.Indices[l]) as IConvertible;
								if (convertible3 == null)
								{
									flag3 = false;
									break;
								}
								array6[l] = convertible3.ToInt32(null);
							}
							if (flag3)
							{
								obj = array5.GetValue(array6);
							}
						}
					}
					else if ((codeBinaryOperatorExpression = obj as CodeBinaryOperatorExpression) != null)
					{
						object obj9 = this.DeserializeExpression(manager, null, codeBinaryOperatorExpression.Left);
						object obj10 = this.DeserializeExpression(manager, null, codeBinaryOperatorExpression.Right);
						obj = obj9;
						IConvertible convertible4 = obj9 as IConvertible;
						IConvertible convertible5 = obj10 as IConvertible;
						if (convertible4 != null && convertible5 != null)
						{
							obj = this.ExecuteBinaryExpression(convertible4, convertible5, codeBinaryOperatorExpression.Operator);
						}
					}
					else if ((codeDelegateInvokeExpression = obj as CodeDelegateInvokeExpression) != null)
					{
						object obj11 = this.DeserializeExpression(manager, null, codeDelegateInvokeExpression.TargetObject);
						Delegate @delegate = obj11 as Delegate;
						if (@delegate != null)
						{
							object[] array7 = new object[codeDelegateInvokeExpression.Parameters.Count];
							bool flag4 = true;
							for (int m = 0; m < array7.Length; m++)
							{
								array7[m] = this.DeserializeExpression(manager, null, codeDelegateInvokeExpression.Parameters[m]);
								if (array7[m] is CodeExpression)
								{
									flag4 = false;
									break;
								}
							}
							if (flag4)
							{
								@delegate.DynamicInvoke(array7);
							}
						}
					}
					else if ((codeDirectionExpression = obj as CodeDirectionExpression) != null)
					{
						obj = this.DeserializeExpression(manager, name, codeDirectionExpression.Expression);
					}
					else if ((codeIndexerExpression = obj as CodeIndexerExpression) != null)
					{
						obj = null;
						object obj12 = this.DeserializeExpression(manager, null, codeIndexerExpression.TargetObject);
						if (obj12 != null)
						{
							object[] array8 = new object[codeIndexerExpression.Indices.Count];
							bool flag5 = true;
							for (int n = 0; n < array8.Length; n++)
							{
								array8[n] = this.DeserializeExpression(manager, null, codeIndexerExpression.Indices[n]);
								if (array8[n] is CodeExpression)
								{
									flag5 = false;
									break;
								}
							}
							if (flag5)
							{
								obj = TypeDescriptor.GetReflectionType(obj12).InvokeMember("Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, obj12, array8, null, null, null);
							}
						}
					}
					else if (obj is CodeSnippetExpression)
					{
						obj = null;
					}
					else if ((codeParameterDeclarationExpression = obj as CodeParameterDeclarationExpression) != null)
					{
						obj = manager.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeParameterDeclarationExpression.Type));
					}
					else if ((codeTypeOfExpression = obj as CodeTypeOfExpression) != null)
					{
						string text2 = CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeTypeOfExpression.Type);
						for (int num2 = 0; num2 < codeTypeOfExpression.Type.ArrayRank; num2++)
						{
							text2 += "[]";
						}
						obj = manager.GetType(text2);
						if (obj == null)
						{
							this.Error(manager, SR.GetString("SerializerTypeNotFound", new object[] { text2 }), "SerializerTypeNotFound");
						}
					}
					else if (!(obj is CodeEventReferenceExpression) && !(obj is CodeMethodReferenceExpression) && obj is CodeDelegateCreateExpression)
					{
					}
				}
			}
			return obj;
		}

		protected void DeserializePropertiesFromResources(IDesignerSerializationManager manager, object value, Attribute[] filter)
		{
			using (CodeDomSerializerBase.TraceScope("ComponentCodeDomSerializerBase::DeserializePropertiesFromResources"))
			{
				IDictionaryEnumerator dictionaryEnumerator = ResourceCodeDomSerializer.Default.GetMetadataEnumerator(manager);
				if (dictionaryEnumerator == null)
				{
					dictionaryEnumerator = ResourceCodeDomSerializer.Default.GetEnumerator(manager, CultureInfo.InvariantCulture);
				}
				if (dictionaryEnumerator != null)
				{
					RootContext rootContext = manager.Context[typeof(RootContext)] as RootContext;
					string text;
					if (rootContext != null && rootContext.Value == value)
					{
						text = "$this";
					}
					else
					{
						text = manager.GetName(value);
					}
					PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
					while (dictionaryEnumerator.MoveNext())
					{
						object obj = dictionaryEnumerator.Current;
						string text2 = dictionaryEnumerator.Key as string;
						int num = text2.IndexOf('.');
						if (num != -1)
						{
							string text3 = text2.Substring(0, num);
							if (text3.Equals(text))
							{
								string text4 = text2.Substring(num + 1);
								PropertyDescriptor propertyDescriptor = properties[text4];
								if (propertyDescriptor != null)
								{
									bool flag = true;
									if (filter != null)
									{
										AttributeCollection attributes = propertyDescriptor.Attributes;
										foreach (Attribute attribute in filter)
										{
											if (!attributes.Contains(attribute))
											{
												flag = false;
												break;
											}
										}
									}
									if (flag)
									{
										object value2 = dictionaryEnumerator.Value;
										try
										{
											propertyDescriptor.SetValue(value, value2);
										}
										catch (Exception ex)
										{
											manager.ReportError(ex);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		protected void DeserializeStatement(IDesignerSerializationManager manager, CodeStatement statement)
		{
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::DeserializeStatement"))
			{
				manager.Context.Push(statement);
				try
				{
					CodeAssignStatement codeAssignStatement = statement as CodeAssignStatement;
					if (codeAssignStatement != null)
					{
						this.DeserializeAssignStatement(manager, codeAssignStatement);
					}
					else
					{
						CodeVariableDeclarationStatement codeVariableDeclarationStatement = statement as CodeVariableDeclarationStatement;
						if (codeVariableDeclarationStatement != null)
						{
							this.DeserializeVariableDeclarationStatement(manager, codeVariableDeclarationStatement);
						}
						else if (!(statement is CodeCommentStatement))
						{
							CodeExpressionStatement codeExpressionStatement = statement as CodeExpressionStatement;
							if (codeExpressionStatement != null)
							{
								this.DeserializeExpression(manager, null, codeExpressionStatement.Expression);
							}
							else
							{
								CodeMethodReturnStatement codeMethodReturnStatement = statement as CodeMethodReturnStatement;
								if (codeMethodReturnStatement != null)
								{
									this.DeserializeExpression(manager, null, codeExpressionStatement.Expression);
								}
								else
								{
									CodeAttachEventStatement codeAttachEventStatement = statement as CodeAttachEventStatement;
									if (codeAttachEventStatement != null)
									{
										this.DeserializeAttachEventStatement(manager, codeAttachEventStatement);
									}
									else
									{
										CodeRemoveEventStatement codeRemoveEventStatement = statement as CodeRemoveEventStatement;
										if (codeRemoveEventStatement != null)
										{
											this.DeserializeDetachEventStatement(manager, codeRemoveEventStatement);
										}
										else
										{
											CodeLabeledStatement codeLabeledStatement = statement as CodeLabeledStatement;
											if (codeLabeledStatement != null)
											{
												this.DeserializeStatement(manager, codeLabeledStatement.Statement);
											}
										}
									}
								}
							}
						}
					}
				}
				catch (CheckoutException)
				{
					throw;
				}
				catch (Exception ex)
				{
					if (ex is TargetInvocationException)
					{
						ex = ex.InnerException;
					}
					if (!(ex is CodeDomSerializerException) && statement.LinePragma != null)
					{
						ex = new CodeDomSerializerException(ex, statement.LinePragma);
					}
					manager.ReportError(ex);
				}
				finally
				{
					manager.Context.Pop();
				}
			}
		}

		private bool DeserializePropertyAssignStatement(IDesignerSerializationManager manager, CodeAssignStatement statement, CodePropertyReferenceExpression propertyReferenceEx, bool reportError)
		{
			object obj = this.DeserializeExpression(manager, null, propertyReferenceEx.TargetObject);
			if (obj != null && !(obj is CodeExpression))
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj, CodeDomSerializerBase.runTimeProperties)[propertyReferenceEx.PropertyName];
				if (propertyDescriptor != null)
				{
					object obj2 = this.DeserializeExpression(manager, null, statement.Right);
					if (obj2 is CodeExpression)
					{
						return false;
					}
					IConvertible convertible = obj2 as IConvertible;
					if (convertible != null && propertyDescriptor.PropertyType != obj2.GetType())
					{
						try
						{
							obj2 = convertible.ToType(propertyDescriptor.PropertyType, null);
						}
						catch
						{
						}
					}
					Type type = obj2 as Type;
					if (type != null && type.UnderlyingSystemType != null)
					{
						obj2 = type.UnderlyingSystemType;
					}
					MemberRelationship memberRelationship = MemberRelationship.Empty;
					MemberRelationshipService memberRelationshipService = null;
					if (statement.Right is CodePropertyReferenceExpression)
					{
						memberRelationshipService = manager.GetService(typeof(MemberRelationshipService)) as MemberRelationshipService;
						if (memberRelationshipService != null)
						{
							CodePropertyReferenceExpression codePropertyReferenceExpression = (CodePropertyReferenceExpression)statement.Right;
							object obj3 = this.DeserializeExpression(manager, null, codePropertyReferenceExpression.TargetObject);
							PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(obj3)[codePropertyReferenceExpression.PropertyName];
							if (propertyDescriptor2 != null)
							{
								MemberRelationship memberRelationship2 = new MemberRelationship(obj, propertyDescriptor);
								MemberRelationship memberRelationship3 = new MemberRelationship(obj3, propertyDescriptor2);
								memberRelationship = memberRelationshipService[memberRelationship2];
								if (memberRelationshipService.SupportsRelationship(memberRelationship2, memberRelationship3))
								{
									memberRelationshipService[memberRelationship2] = memberRelationship3;
								}
							}
						}
					}
					else
					{
						memberRelationshipService = manager.GetService(typeof(MemberRelationshipService)) as MemberRelationshipService;
						if (memberRelationshipService != null)
						{
							memberRelationship = memberRelationshipService[obj, propertyDescriptor];
							memberRelationshipService[obj, propertyDescriptor] = MemberRelationship.Empty;
						}
					}
					try
					{
						propertyDescriptor.SetValue(obj, obj2);
					}
					catch
					{
						if (memberRelationshipService != null)
						{
							memberRelationshipService[obj, propertyDescriptor] = memberRelationship;
						}
						throw;
					}
					return true;
				}
				else if (reportError)
				{
					this.Error(manager, SR.GetString("SerializerNoSuchProperty", new object[]
					{
						obj.GetType().FullName,
						propertyReferenceEx.PropertyName
					}), "SerializerNoSuchProperty");
				}
			}
			return false;
		}

		private void DeserializeAssignStatement(IDesignerSerializationManager manager, CodeAssignStatement statement)
		{
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::DeserializeAssignStatement"))
			{
				CodeExpression left = statement.Left;
				CodePropertyReferenceExpression codePropertyReferenceExpression;
				CodeFieldReferenceExpression codeFieldReferenceExpression;
				CodeVariableReferenceExpression codeVariableReferenceExpression;
				CodeArrayIndexerExpression codeArrayIndexerExpression;
				if ((codePropertyReferenceExpression = left as CodePropertyReferenceExpression) != null)
				{
					this.DeserializePropertyAssignStatement(manager, statement, codePropertyReferenceExpression, true);
				}
				else if ((codeFieldReferenceExpression = left as CodeFieldReferenceExpression) != null)
				{
					object obj = this.DeserializeExpression(manager, codeFieldReferenceExpression.FieldName, codeFieldReferenceExpression.TargetObject);
					if (obj != null)
					{
						RootContext rootContext = (RootContext)manager.Context[typeof(RootContext)];
						if (rootContext != null && rootContext.Value == obj)
						{
							object obj2 = this.DeserializeExpression(manager, codeFieldReferenceExpression.FieldName, statement.Right);
							if (obj2 is CodeExpression)
							{
							}
						}
						else
						{
							Type type = obj as Type;
							object obj3;
							FieldInfo fieldInfo;
							if (type != null)
							{
								obj3 = null;
								fieldInfo = TypeDescriptor.GetReflectionType(type).GetField(codeFieldReferenceExpression.FieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
							}
							else
							{
								obj3 = obj;
								fieldInfo = TypeDescriptor.GetReflectionType(obj).GetField(codeFieldReferenceExpression.FieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
							}
							if (fieldInfo != null)
							{
								object obj4 = this.DeserializeExpression(manager, codeFieldReferenceExpression.FieldName, statement.Right);
								if (!(obj4 is CodeExpression))
								{
									IConvertible convertible = obj4 as IConvertible;
									if (convertible != null && fieldInfo.FieldType != obj4.GetType())
									{
										try
										{
											obj4 = convertible.ToType(fieldInfo.FieldType, null);
										}
										catch
										{
										}
									}
									fieldInfo.SetValue(obj3, obj4);
								}
							}
							else if (!this.DeserializePropertyAssignStatement(manager, statement, new CodePropertyReferenceExpression
							{
								TargetObject = codeFieldReferenceExpression.TargetObject,
								PropertyName = codeFieldReferenceExpression.FieldName
							}, false))
							{
								this.Error(manager, SR.GetString("SerializerNoSuchField", new object[]
								{
									obj.GetType().FullName,
									codeFieldReferenceExpression.FieldName
								}), "SerializerNoSuchField");
							}
						}
					}
				}
				else if ((codeVariableReferenceExpression = left as CodeVariableReferenceExpression) != null)
				{
					object obj5 = this.DeserializeExpression(manager, codeVariableReferenceExpression.VariableName, statement.Right);
					if (!(obj5 is CodeExpression))
					{
						manager.SetName(obj5, codeVariableReferenceExpression.VariableName);
					}
				}
				else if ((codeArrayIndexerExpression = left as CodeArrayIndexerExpression) != null)
				{
					int[] array = new int[codeArrayIndexerExpression.Indices.Count];
					object obj6 = this.DeserializeExpression(manager, null, codeArrayIndexerExpression.TargetObject);
					bool flag = true;
					for (int i = 0; i < array.Length; i++)
					{
						object obj7 = this.DeserializeExpression(manager, null, codeArrayIndexerExpression.Indices[i]);
						IConvertible convertible2 = obj7 as IConvertible;
						if (convertible2 == null)
						{
							flag = false;
							break;
						}
						array[i] = convertible2.ToInt32(null);
					}
					Array array2 = obj6 as Array;
					if (array2 != null && flag)
					{
						object obj8 = this.DeserializeExpression(manager, null, statement.Right);
						if (!(obj8 is CodeExpression))
						{
							array2.SetValue(obj8, array);
						}
					}
				}
			}
		}

		private void DeserializeAttachEventStatement(IDesignerSerializationManager manager, CodeAttachEventStatement statement)
		{
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::DeserializeAttachEventStatement"))
			{
				string text = null;
				object obj = null;
				object obj2 = this.DeserializeExpression(manager, null, statement.Event.TargetObject);
				string eventName = statement.Event.EventName;
				if (eventName != null && obj2 != null)
				{
					CodeObjectCreateExpression codeObjectCreateExpression = statement.Listener as CodeObjectCreateExpression;
					if (codeObjectCreateExpression != null)
					{
						if (codeObjectCreateExpression.Parameters.Count == 1)
						{
							CodeMethodReferenceExpression codeMethodReferenceExpression = codeObjectCreateExpression.Parameters[0] as CodeMethodReferenceExpression;
							if (codeMethodReferenceExpression != null)
							{
								text = codeMethodReferenceExpression.MethodName;
								obj = this.DeserializeExpression(manager, null, codeMethodReferenceExpression.TargetObject);
							}
						}
					}
					else
					{
						object obj3 = this.DeserializeExpression(manager, null, statement.Listener);
						CodeDelegateCreateExpression codeDelegateCreateExpression = obj3 as CodeDelegateCreateExpression;
						if (codeDelegateCreateExpression != null)
						{
							obj = this.DeserializeExpression(manager, null, codeDelegateCreateExpression.TargetObject);
							text = codeDelegateCreateExpression.MethodName;
						}
					}
					RootContext rootContext = (RootContext)manager.Context[typeof(RootContext)];
					bool flag = rootContext == null || (rootContext != null && rootContext.Value == obj);
					if (text != null && flag && !(obj2 is CodeExpression))
					{
						EventDescriptor eventDescriptor = TypeDescriptor.GetEvents(obj2)[eventName];
						if (eventDescriptor != null)
						{
							IEventBindingService eventBindingService = (IEventBindingService)manager.GetService(typeof(IEventBindingService));
							if (eventBindingService != null)
							{
								PropertyDescriptor eventProperty = eventBindingService.GetEventProperty(eventDescriptor);
								eventProperty.SetValue(obj2, text);
							}
						}
						else
						{
							this.Error(manager, SR.GetString("SerializerNoSuchEvent", new object[]
							{
								obj2.GetType().FullName,
								eventName
							}), "SerializerNoSuchEvent");
						}
					}
				}
			}
		}

		private void DeserializeDetachEventStatement(IDesignerSerializationManager manager, CodeRemoveEventStatement statement)
		{
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::DeserializeDetachEventStatement"))
			{
				object obj = this.DeserializeExpression(manager, null, statement.Listener);
				CodeDelegateCreateExpression codeDelegateCreateExpression = obj as CodeDelegateCreateExpression;
				if (codeDelegateCreateExpression != null)
				{
					object obj2 = this.DeserializeExpression(manager, null, codeDelegateCreateExpression.TargetObject);
					RootContext rootContext = (RootContext)manager.Context[typeof(RootContext)];
					bool flag = rootContext == null || (rootContext != null && rootContext.Value == obj2);
					if (flag)
					{
						object obj3 = this.DeserializeExpression(manager, null, statement.Event.TargetObject);
						if (!(obj3 is CodeExpression))
						{
							EventDescriptor eventDescriptor = TypeDescriptor.GetEvents(obj3)[statement.Event.EventName];
							if (eventDescriptor != null)
							{
								IEventBindingService eventBindingService = (IEventBindingService)manager.GetService(typeof(IEventBindingService));
								if (eventBindingService != null)
								{
									PropertyDescriptor eventProperty = eventBindingService.GetEventProperty(eventDescriptor);
									eventProperty.SetValue(obj3, null);
								}
							}
							else
							{
								this.Error(manager, SR.GetString("SerializerNoSuchEvent", new object[]
								{
									obj3.GetType().FullName,
									statement.Event.EventName
								}), "SerializerNoSuchEvent");
							}
						}
					}
				}
			}
		}

		private void DeserializeVariableDeclarationStatement(IDesignerSerializationManager manager, CodeVariableDeclarationStatement statement)
		{
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::DeserializeVariableDeclarationStatement"))
			{
				if (statement.InitExpression != null)
				{
					this.DeserializeExpression(manager, statement.Name, statement.InitExpression);
				}
			}
		}

		internal void Error(IDesignerSerializationManager manager, string exceptionText, string helpLink)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (exceptionText == null)
			{
				throw new ArgumentNullException("exceptionText");
			}
			CodeStatement codeStatement = (CodeStatement)manager.Context[typeof(CodeStatement)];
			CodeLinePragma codeLinePragma = null;
			if (codeStatement != null)
			{
				codeLinePragma = codeStatement.LinePragma;
			}
			throw new CodeDomSerializerException(exceptionText, codeLinePragma)
			{
				HelpLink = helpLink
			};
		}

		private object ExecuteBinaryExpression(IConvertible left, IConvertible right, CodeBinaryOperatorType op)
		{
			CodeBinaryOperatorType[] array = new CodeBinaryOperatorType[]
			{
				CodeBinaryOperatorType.IdentityInequality,
				CodeBinaryOperatorType.IdentityEquality,
				CodeBinaryOperatorType.ValueEquality,
				CodeBinaryOperatorType.BooleanOr,
				CodeBinaryOperatorType.BooleanAnd,
				CodeBinaryOperatorType.LessThan,
				CodeBinaryOperatorType.LessThanOrEqual,
				CodeBinaryOperatorType.GreaterThan,
				CodeBinaryOperatorType.GreaterThanOrEqual
			};
			CodeBinaryOperatorType[] array2 = new CodeBinaryOperatorType[]
			{
				CodeBinaryOperatorType.Add,
				CodeBinaryOperatorType.Subtract,
				CodeBinaryOperatorType.Multiply,
				CodeBinaryOperatorType.Divide,
				CodeBinaryOperatorType.Modulus
			};
			CodeBinaryOperatorType[] array3 = new CodeBinaryOperatorType[]
			{
				CodeBinaryOperatorType.BitwiseOr,
				CodeBinaryOperatorType.BitwiseAnd
			};
			for (int i = 0; i < array3.Length; i++)
			{
				if (op == array3[i])
				{
					return this.ExecuteBinaryOperator(left, right, op);
				}
			}
			for (int j = 0; j < array2.Length; j++)
			{
				if (op == array2[j])
				{
					return this.ExecuteMathOperator(left, right, op);
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (op == array[k])
				{
					return this.ExecuteBooleanOperator(left, right, op);
				}
			}
			return left;
		}

		private object ExecuteBinaryOperator(IConvertible left, IConvertible right, CodeBinaryOperatorType op)
		{
			TypeCode typeCode = left.GetTypeCode();
			TypeCode typeCode2 = right.GetTypeCode();
			TypeCode[] array = new TypeCode[]
			{
				TypeCode.Byte,
				TypeCode.Char,
				TypeCode.Int16,
				TypeCode.UInt16,
				TypeCode.Int32,
				TypeCode.UInt32,
				TypeCode.Int64,
				TypeCode.UInt64
			};
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (typeCode == array[i])
				{
					num = i;
				}
				if (typeCode2 == array[i])
				{
					num2 = i;
				}
				if (num != -1 && num2 != -1)
				{
					break;
				}
			}
			if (num == -1 || num2 == -1)
			{
				return left;
			}
			int num3 = Math.Max(num, num2);
			object obj = left;
			switch (array[num3])
			{
			case TypeCode.Char:
			{
				char c = left.ToChar(null);
				char c2 = right.ToChar(null);
				if (op == CodeBinaryOperatorType.BitwiseOr)
				{
					obj = (int)(c | c2);
				}
				else
				{
					obj = (int)(c & c2);
				}
				break;
			}
			case TypeCode.Byte:
			{
				byte b = left.ToByte(null);
				byte b2 = right.ToByte(null);
				if (op == CodeBinaryOperatorType.BitwiseOr)
				{
					obj = (int)(b | b2);
				}
				else
				{
					obj = (int)(b & b2);
				}
				break;
			}
			case TypeCode.Int16:
			{
				short num4 = left.ToInt16(null);
				short num5 = right.ToInt16(null);
				if (op == CodeBinaryOperatorType.BitwiseOr)
				{
					obj = (short)((ushort)num4 | (ushort)num5);
				}
				else
				{
					obj = (int)(num4 & num5);
				}
				break;
			}
			case TypeCode.UInt16:
			{
				ushort num6 = left.ToUInt16(null);
				ushort num7 = right.ToUInt16(null);
				if (op == CodeBinaryOperatorType.BitwiseOr)
				{
					obj = (int)(num6 | num7);
				}
				else
				{
					obj = (int)(num6 & num7);
				}
				break;
			}
			case TypeCode.Int32:
			{
				int num8 = left.ToInt32(null);
				int num9 = right.ToInt32(null);
				if (op == CodeBinaryOperatorType.BitwiseOr)
				{
					obj = num8 | num9;
				}
				else
				{
					obj = num8 & num9;
				}
				break;
			}
			case TypeCode.UInt32:
			{
				uint num10 = left.ToUInt32(null);
				uint num11 = right.ToUInt32(null);
				if (op == CodeBinaryOperatorType.BitwiseOr)
				{
					obj = num10 | num11;
				}
				else
				{
					obj = num10 & num11;
				}
				break;
			}
			case TypeCode.Int64:
			{
				long num12 = left.ToInt64(null);
				long num13 = right.ToInt64(null);
				if (op == CodeBinaryOperatorType.BitwiseOr)
				{
					obj = num12 | num13;
				}
				else
				{
					obj = num12 & num13;
				}
				break;
			}
			case TypeCode.UInt64:
			{
				ulong num14 = left.ToUInt64(null);
				ulong num15 = right.ToUInt64(null);
				if (op == CodeBinaryOperatorType.BitwiseOr)
				{
					obj = num14 | num15;
				}
				else
				{
					obj = num14 & num15;
				}
				break;
			}
			}
			if (obj != left && left is Enum)
			{
				obj = Enum.ToObject(left.GetType(), obj);
			}
			return obj;
		}

		private object ExecuteBooleanOperator(IConvertible left, IConvertible right, CodeBinaryOperatorType op)
		{
			bool flag = false;
			switch (op)
			{
			case CodeBinaryOperatorType.IdentityInequality:
				flag = left != right;
				break;
			case CodeBinaryOperatorType.IdentityEquality:
				flag = left == right;
				break;
			case CodeBinaryOperatorType.ValueEquality:
				flag = left.Equals(right);
				break;
			case CodeBinaryOperatorType.BooleanOr:
				flag = left.ToBoolean(null) || right.ToBoolean(null);
				break;
			case CodeBinaryOperatorType.BooleanAnd:
				flag = left.ToBoolean(null) && right.ToBoolean(null);
				break;
			}
			return flag;
		}

		private object ExecuteMathOperator(IConvertible left, IConvertible right, CodeBinaryOperatorType op)
		{
			if (op != CodeBinaryOperatorType.Add)
			{
				return left;
			}
			string text = left as string;
			string text2 = right as string;
			if (text == null && left is char)
			{
				text = left.ToString();
			}
			if (text2 == null && right is char)
			{
				text2 = right.ToString();
			}
			if (text != null && text2 != null)
			{
				return text + text2;
			}
			return left;
		}

		protected CodeExpression GetExpression(IDesignerSerializationManager manager, object value)
		{
			CodeExpression codeExpression = null;
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ExpressionTable expressionTable = manager.Context[typeof(ExpressionTable)] as ExpressionTable;
			if (expressionTable != null)
			{
				codeExpression = expressionTable.GetExpression(value);
			}
			if (codeExpression == null)
			{
				RootContext rootContext = manager.Context[typeof(RootContext)] as RootContext;
				if (rootContext != null && object.ReferenceEquals(rootContext.Value, value))
				{
					codeExpression = rootContext.Expression;
				}
			}
			if (codeExpression == null)
			{
				string text = manager.GetName(value);
				if (text == null || text.IndexOf('.') != -1)
				{
					IReferenceService referenceService = manager.GetService(typeof(IReferenceService)) as IReferenceService;
					if (referenceService != null)
					{
						text = referenceService.GetName(value);
						if (text != null && text.IndexOf('.') != -1)
						{
							string[] array = text.Split(new char[] { '.' });
							object instance = manager.GetInstance(array[0]);
							if (instance != null)
							{
								CodeExpression codeExpression2 = this.SerializeToExpression(manager, instance);
								if (codeExpression2 != null)
								{
									for (int i = 1; i < array.Length; i++)
									{
										codeExpression2 = new CodePropertyReferenceExpression(codeExpression2, array[i]);
									}
									codeExpression = codeExpression2;
								}
							}
						}
					}
				}
			}
			if (codeExpression == null)
			{
				ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
				if (expressionContext != null && object.ReferenceEquals(expressionContext.PresetValue, value))
				{
					codeExpression = expressionContext.Expression;
				}
			}
			if (codeExpression != null)
			{
				ComponentCache.Entry entry = (ComponentCache.Entry)manager.Context[typeof(ComponentCache.Entry)];
				ComponentCache componentCache = (ComponentCache)manager.Context[typeof(ComponentCache)];
				if (entry != null && entry.Component != value && componentCache != null)
				{
					ComponentCache.Entry entryAll = componentCache.GetEntryAll(value);
					if (entryAll != null && entry.Component != null)
					{
						entryAll.AddDependency(entry.Component);
					}
				}
			}
			return codeExpression;
		}

		private PropertyDescriptorCollection GetFilteredProperties(IDesignerSerializationManager manager, object value, Attribute[] filter)
		{
			IComponent component = value as IComponent;
			PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(value, filter);
			if (component != null)
			{
				if (((IDictionary)propertyDescriptorCollection).IsReadOnly)
				{
					PropertyDescriptor[] array = new PropertyDescriptor[propertyDescriptorCollection.Count];
					propertyDescriptorCollection.CopyTo(array, 0);
					propertyDescriptorCollection = new PropertyDescriptorCollection(array);
				}
				PropertyDescriptor propertyDescriptor = manager.Properties["FilteredProperties"];
				if (propertyDescriptor != null)
				{
					ITypeDescriptorFilterService typeDescriptorFilterService = propertyDescriptor.GetValue(manager) as ITypeDescriptorFilterService;
					if (typeDescriptorFilterService != null)
					{
						typeDescriptorFilterService.FilterProperties(component, propertyDescriptorCollection);
					}
				}
			}
			return propertyDescriptorCollection;
		}

		private CodeExpression GetLegacyExpression(IDesignerSerializationManager manager, object value)
		{
			CodeDomSerializerBase.LegacyExpressionTable legacyExpressionTable = manager.Context[typeof(CodeDomSerializerBase.LegacyExpressionTable)] as CodeDomSerializerBase.LegacyExpressionTable;
			CodeExpression codeExpression = null;
			if (legacyExpressionTable != null)
			{
				object obj = legacyExpressionTable[value];
				if (obj == value)
				{
					string text = manager.GetName(value);
					bool flag = false;
					if (text == null)
					{
						IReferenceService referenceService = (IReferenceService)manager.GetService(typeof(IReferenceService));
						if (referenceService != null)
						{
							text = referenceService.GetName(value);
							flag = text != null;
						}
					}
					if (text != null)
					{
						RootContext rootContext = (RootContext)manager.Context[typeof(RootContext)];
						if (rootContext != null)
						{
							if (rootContext.Value == value)
							{
								codeExpression = rootContext.Expression;
							}
							else if (flag && text.IndexOf('.') != -1)
							{
								int num = text.IndexOf('.');
								codeExpression = new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(rootContext.Expression, text.Substring(0, num)), text.Substring(num + 1));
							}
							else
							{
								codeExpression = new CodeFieldReferenceExpression(rootContext.Expression, text);
							}
						}
						else if (flag && text.IndexOf('.') != -1)
						{
							int num2 = text.IndexOf('.');
							codeExpression = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(text.Substring(0, num2)), text.Substring(num2 + 1));
						}
						else
						{
							codeExpression = new CodeVariableReferenceExpression(text);
						}
					}
					legacyExpressionTable[value] = codeExpression;
				}
				else
				{
					codeExpression = obj as CodeExpression;
				}
			}
			return codeExpression;
		}

		protected CodeDomSerializer GetSerializer(IDesignerSerializationManager manager, object value)
		{
			if (value != null)
			{
				AttributeCollection attributes = TypeDescriptor.GetAttributes(value);
				AttributeCollection attributes2 = TypeDescriptor.GetAttributes(value.GetType());
				if (attributes.Count != attributes2.Count)
				{
					string text = null;
					Type typeFromHandle = typeof(CodeDomSerializer);
					foreach (object obj in attributes)
					{
						Attribute attribute = (Attribute)obj;
						DesignerSerializerAttribute designerSerializerAttribute = attribute as DesignerSerializerAttribute;
						if (designerSerializerAttribute != null)
						{
							Type type = manager.GetType(designerSerializerAttribute.SerializerBaseTypeName);
							if (type == typeFromHandle)
							{
								text = designerSerializerAttribute.SerializerTypeName;
								break;
							}
						}
					}
					if (text != null)
					{
						foreach (object obj2 in attributes2)
						{
							Attribute attribute2 = (Attribute)obj2;
							DesignerSerializerAttribute designerSerializerAttribute2 = attribute2 as DesignerSerializerAttribute;
							if (designerSerializerAttribute2 != null)
							{
								Type type2 = manager.GetType(designerSerializerAttribute2.SerializerBaseTypeName);
								if (type2 == typeFromHandle)
								{
									if (text.Equals(designerSerializerAttribute2.SerializerTypeName))
									{
										text = null;
										break;
									}
									break;
								}
							}
						}
					}
					if (text != null)
					{
						Type type3 = manager.GetType(text);
						if (type3 != null && typeFromHandle.IsAssignableFrom(type3))
						{
							return (CodeDomSerializer)Activator.CreateInstance(type3);
						}
					}
				}
			}
			Type type4 = null;
			if (value != null)
			{
				type4 = value.GetType();
			}
			return (CodeDomSerializer)manager.GetSerializer(type4, typeof(CodeDomSerializer));
		}

		protected CodeDomSerializer GetSerializer(IDesignerSerializationManager manager, Type valueType)
		{
			return manager.GetSerializer(valueType, typeof(CodeDomSerializer)) as CodeDomSerializer;
		}

		protected bool IsSerialized(IDesignerSerializationManager manager, object value)
		{
			return this.IsSerialized(manager, value, false);
		}

		protected bool IsSerialized(IDesignerSerializationManager manager, object value, bool honorPreset)
		{
			bool flag = false;
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ExpressionTable expressionTable = manager.Context[typeof(ExpressionTable)] as ExpressionTable;
			if (expressionTable != null && expressionTable.GetExpression(value) != null && (!honorPreset || !expressionTable.ContainsPresetExpression(value)))
			{
				flag = true;
			}
			return flag;
		}

		protected CodeExpression SerializeCreationExpression(IDesignerSerializationManager manager, object value, out bool isComplete)
		{
			isComplete = false;
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TypeConverter converter = TypeDescriptor.GetConverter(value);
			ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
			if (expressionContext != null && object.ReferenceEquals(expressionContext.PresetValue, value))
			{
				CodeExpression expression = expressionContext.Expression;
				if (converter.CanConvertTo(typeof(InstanceDescriptor)))
				{
					InstanceDescriptor instanceDescriptor = converter.ConvertTo(value, typeof(InstanceDescriptor)) as InstanceDescriptor;
					if (instanceDescriptor != null && instanceDescriptor.MemberInfo != null)
					{
						isComplete = instanceDescriptor.IsComplete;
					}
				}
				return expression;
			}
			if (converter.CanConvertTo(typeof(InstanceDescriptor)))
			{
				InstanceDescriptor instanceDescriptor2 = converter.ConvertTo(value, typeof(InstanceDescriptor)) as InstanceDescriptor;
				if (instanceDescriptor2 != null && instanceDescriptor2.MemberInfo != null)
				{
					isComplete = instanceDescriptor2.IsComplete;
					return this.SerializeInstanceDescriptor(manager, value, instanceDescriptor2);
				}
			}
			if (TypeDescriptor.GetReflectionType(value).IsSerializable && (!(value is IComponent) || ((IComponent)value).Site == null))
			{
				CodeExpression codeExpression = this.SerializeToResourceExpression(manager, value);
				if (codeExpression != null)
				{
					isComplete = true;
					return codeExpression;
				}
			}
			Type reflectionType = TypeDescriptor.GetReflectionType(value);
			ConstructorInfo constructor = reflectionType.GetConstructor(new Type[0]);
			if (constructor != null)
			{
				isComplete = false;
				return new CodeObjectCreateExpression(TypeDescriptor.GetClassName(value), new CodeExpression[0]);
			}
			return null;
		}

		protected string GetUniqueName(IDesignerSerializationManager manager, object value)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			string text = manager.GetName(value);
			if (text == null)
			{
				Type reflectionType = TypeDescriptor.GetReflectionType(value);
				INameCreationService nameCreationService = manager.GetService(typeof(INameCreationService)) as INameCreationService;
				string text2;
				if (nameCreationService != null)
				{
					text2 = nameCreationService.CreateName(null, reflectionType);
				}
				else
				{
					text2 = reflectionType.Name.ToLower(CultureInfo.InvariantCulture);
				}
				int num = 1;
				ComponentCache componentCache = manager.Context[typeof(ComponentCache)] as ComponentCache;
				for (;;)
				{
					text = string.Format(CultureInfo.CurrentCulture, "{0}{1}", new object[] { text2, num });
					if (manager.GetInstance(text) == null && (componentCache == null || !componentCache.ContainsLocalName(text)))
					{
						break;
					}
					num++;
				}
				manager.SetName(value, text);
				ComponentCache.Entry entry = manager.Context[typeof(ComponentCache.Entry)] as ComponentCache.Entry;
				if (entry != null)
				{
					entry.AddLocalName(text);
				}
			}
			return text;
		}

		protected void SerializeEvent(IDesignerSerializationManager manager, CodeStatementCollection statements, object value, EventDescriptor descriptor)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (statements == null)
			{
				throw new ArgumentNullException("statements");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (descriptor == null)
			{
				throw new ArgumentNullException("descriptor");
			}
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::SerializeEvent"))
			{
				manager.Context.Push(statements);
				manager.Context.Push(descriptor);
				try
				{
					MemberCodeDomSerializer memberCodeDomSerializer = (MemberCodeDomSerializer)manager.GetSerializer(descriptor.GetType(), typeof(MemberCodeDomSerializer));
					if (memberCodeDomSerializer != null && memberCodeDomSerializer.ShouldSerialize(manager, value, descriptor))
					{
						memberCodeDomSerializer.Serialize(manager, value, descriptor, statements);
					}
				}
				finally
				{
					manager.Context.Pop();
					manager.Context.Pop();
				}
			}
		}

		protected void SerializeEvents(IDesignerSerializationManager manager, CodeStatementCollection statements, object value, params Attribute[] filter)
		{
			EventDescriptorCollection eventDescriptorCollection = TypeDescriptor.GetEvents(value, filter).Sort();
			foreach (object obj in eventDescriptorCollection)
			{
				EventDescriptor eventDescriptor = (EventDescriptor)obj;
				this.SerializeEvent(manager, statements, value, eventDescriptor);
			}
		}

		private CodeExpression SerializeInstanceDescriptor(IDesignerSerializationManager manager, object value, InstanceDescriptor descriptor)
		{
			CodeExpression codeExpression = null;
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::SerializeInstanceDescriptor"))
			{
				CodeExpression[] array = new CodeExpression[descriptor.Arguments.Count];
				object[] array2 = new object[array.Length];
				ParameterInfo[] array3 = null;
				if (array.Length > 0)
				{
					descriptor.Arguments.CopyTo(array2, 0);
					MethodBase methodBase = descriptor.MemberInfo as MethodBase;
					if (methodBase != null)
					{
						array3 = methodBase.GetParameters();
					}
				}
				bool flag = true;
				for (int i = 0; i < array.Length; i++)
				{
					object obj = array2[i];
					CodeExpression codeExpression2 = null;
					ExpressionContext expressionContext = null;
					ExpressionContext expressionContext2 = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
					if (expressionContext2 != null)
					{
						expressionContext = new ExpressionContext(expressionContext2.Expression, array3[i].ParameterType, expressionContext2.Owner);
						manager.Context.Push(expressionContext);
					}
					try
					{
						codeExpression2 = this.SerializeToExpression(manager, obj);
					}
					finally
					{
						if (expressionContext != null)
						{
							manager.Context.Pop();
						}
					}
					if (codeExpression2 == null)
					{
						flag = false;
						break;
					}
					if (obj != null && !array3[i].ParameterType.IsAssignableFrom(obj.GetType()))
					{
						codeExpression2 = new CodeCastExpression(array3[i].ParameterType, codeExpression2);
					}
					array[i] = codeExpression2;
				}
				if (flag)
				{
					Type type = descriptor.MemberInfo.DeclaringType;
					CodeTypeReference codeTypeReference = new CodeTypeReference(type);
					if (descriptor.MemberInfo is ConstructorInfo)
					{
						codeExpression = new CodeObjectCreateExpression(codeTypeReference, array);
					}
					else if (descriptor.MemberInfo is MethodInfo)
					{
						CodeTypeReferenceExpression codeTypeReferenceExpression = new CodeTypeReferenceExpression(codeTypeReference);
						CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(codeTypeReferenceExpression, descriptor.MemberInfo.Name);
						codeExpression = new CodeMethodInvokeExpression(codeMethodReferenceExpression, array);
						type = ((MethodInfo)descriptor.MemberInfo).ReturnType;
					}
					else if (descriptor.MemberInfo is PropertyInfo)
					{
						CodeTypeReferenceExpression codeTypeReferenceExpression2 = new CodeTypeReferenceExpression(codeTypeReference);
						CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(codeTypeReferenceExpression2, descriptor.MemberInfo.Name);
						codeExpression = codePropertyReferenceExpression;
						type = ((PropertyInfo)descriptor.MemberInfo).PropertyType;
					}
					else if (descriptor.MemberInfo is FieldInfo)
					{
						CodeTypeReferenceExpression codeTypeReferenceExpression3 = new CodeTypeReferenceExpression(codeTypeReference);
						codeExpression = new CodeFieldReferenceExpression(codeTypeReferenceExpression3, descriptor.MemberInfo.Name);
						type = ((FieldInfo)descriptor.MemberInfo).FieldType;
					}
					Type type2 = value.GetType();
					while (!type2.IsPublic)
					{
						type2 = type2.BaseType;
					}
					if (!type2.IsAssignableFrom(type))
					{
						codeExpression = new CodeCastExpression(type2, codeExpression);
					}
				}
			}
			return codeExpression;
		}

		protected void SerializeProperties(IDesignerSerializationManager manager, CodeStatementCollection statements, object value, Attribute[] filter)
		{
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::SerializeProperties"))
			{
				PropertyDescriptorCollection propertyDescriptorCollection = this.GetFilteredProperties(manager, value, filter).Sort();
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(value)[typeof(InheritanceAttribute)];
				if (inheritanceAttribute == null)
				{
					inheritanceAttribute = InheritanceAttribute.NotInherited;
				}
				manager.Context.Push(inheritanceAttribute);
				try
				{
					foreach (object obj in propertyDescriptorCollection)
					{
						PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
						if (!propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden))
						{
							this.SerializeProperty(manager, statements, value, propertyDescriptor);
						}
					}
				}
				finally
				{
					manager.Context.Pop();
				}
			}
		}

		protected void SerializePropertiesToResources(IDesignerSerializationManager manager, CodeStatementCollection statements, object value, Attribute[] filter)
		{
			using (CodeDomSerializerBase.TraceScope("ComponentCodeDomSerializerBase::SerializePropertiesToResources"))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, filter);
				manager.Context.Push(statements);
				try
				{
					CodeExpression codeExpression = this.SerializeToExpression(manager, value);
					if (codeExpression != null)
					{
						CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(codeExpression, string.Empty);
						foreach (object obj in properties)
						{
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
							ExpressionContext expressionContext = new ExpressionContext(codePropertyReferenceExpression, propertyDescriptor.PropertyType, value);
							manager.Context.Push(expressionContext);
							try
							{
								if (propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Visible))
								{
									codePropertyReferenceExpression.PropertyName = propertyDescriptor.Name;
									string text;
									if (codeExpression is CodeThisReferenceExpression)
									{
										text = "$this";
									}
									else
									{
										text = manager.GetName(value);
									}
									text = string.Format(CultureInfo.CurrentCulture, "{0}.{1}", new object[] { text, propertyDescriptor.Name });
									ResourceCodeDomSerializer.Default.SerializeMetadata(manager, text, propertyDescriptor.GetValue(value), propertyDescriptor.ShouldSerializeValue(value));
								}
							}
							finally
							{
								manager.Context.Pop();
							}
						}
					}
				}
				finally
				{
					manager.Context.Pop();
				}
			}
		}

		protected void SerializeProperty(IDesignerSerializationManager manager, CodeStatementCollection statements, object value, PropertyDescriptor propertyToSerialize)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (propertyToSerialize == null)
			{
				throw new ArgumentNullException("propertyToSerialize");
			}
			if (statements == null)
			{
				throw new ArgumentNullException("statements");
			}
			manager.Context.Push(statements);
			manager.Context.Push(propertyToSerialize);
			try
			{
				MemberCodeDomSerializer memberCodeDomSerializer = (MemberCodeDomSerializer)manager.GetSerializer(propertyToSerialize.GetType(), typeof(MemberCodeDomSerializer));
				if (memberCodeDomSerializer != null && memberCodeDomSerializer.ShouldSerialize(manager, value, propertyToSerialize))
				{
					memberCodeDomSerializer.Serialize(manager, value, propertyToSerialize, statements);
				}
			}
			finally
			{
				manager.Context.Pop();
				manager.Context.Pop();
			}
		}

		protected void SerializeResource(IDesignerSerializationManager manager, string resourceName, object value)
		{
			ResourceCodeDomSerializer.Default.WriteResource(manager, resourceName, value);
		}

		protected void SerializeResourceInvariant(IDesignerSerializationManager manager, string resourceName, object value)
		{
			ResourceCodeDomSerializer.Default.WriteResourceInvariant(manager, resourceName, value);
		}

		protected CodeExpression SerializeToExpression(IDesignerSerializationManager manager, object value)
		{
			CodeExpression codeExpression = null;
			using (CodeDomSerializerBase.TraceScope("SerializeToExpression"))
			{
				if (value != null)
				{
					if (this.IsSerialized(manager, value))
					{
						codeExpression = this.GetExpression(manager, value);
					}
					else
					{
						codeExpression = this.GetLegacyExpression(manager, value);
						if (codeExpression != null)
						{
							this.SetExpression(manager, value, codeExpression);
						}
					}
				}
				if (codeExpression == null)
				{
					CodeDomSerializer serializer = this.GetSerializer(manager, value);
					if (serializer != null)
					{
						CodeStatementCollection codeStatementCollection = null;
						if (value != null)
						{
							this.SetLegacyExpression(manager, value);
							StatementContext statementContext = manager.Context[typeof(StatementContext)] as StatementContext;
							if (statementContext != null)
							{
								codeStatementCollection = statementContext.StatementCollection[value];
							}
							if (codeStatementCollection != null)
							{
								manager.Context.Push(codeStatementCollection);
							}
						}
						object obj = null;
						try
						{
							obj = serializer.Serialize(manager, value);
						}
						finally
						{
							if (codeStatementCollection != null)
							{
								manager.Context.Pop();
							}
						}
						codeExpression = obj as CodeExpression;
						if (codeExpression == null && value != null)
						{
							codeExpression = this.GetExpression(manager, value);
						}
						CodeStatementCollection codeStatementCollection2 = obj as CodeStatementCollection;
						if (codeStatementCollection2 == null)
						{
							CodeStatement codeStatement = obj as CodeStatement;
							if (codeStatement != null)
							{
								codeStatementCollection2 = new CodeStatementCollection();
								codeStatementCollection2.Add(codeStatement);
							}
						}
						if (codeStatementCollection2 != null)
						{
							if (codeStatementCollection == null)
							{
								codeStatementCollection = manager.Context[typeof(CodeStatementCollection)] as CodeStatementCollection;
							}
							if (codeStatementCollection != null)
							{
								codeStatementCollection.AddRange(codeStatementCollection2);
							}
							else
							{
								string text = "(null)";
								if (value != null)
								{
									text = manager.GetName(value);
									if (text == null)
									{
										text = value.GetType().Name;
									}
								}
								manager.ReportError(SR.GetString("SerializerLostStatements", new object[] { text }));
							}
						}
					}
					else
					{
						manager.ReportError(SR.GetString("SerializerNoSerializerForComponent", new object[] { value.GetType().FullName }));
					}
				}
			}
			return codeExpression;
		}

		protected CodeExpression SerializeToResourceExpression(IDesignerSerializationManager manager, object value)
		{
			return this.SerializeToResourceExpression(manager, value, true);
		}

		protected CodeExpression SerializeToResourceExpression(IDesignerSerializationManager manager, object value, bool ensureInvariant)
		{
			CodeExpression codeExpression = null;
			if (value == null || value.GetType().IsSerializable)
			{
				CodeStatementCollection codeStatementCollection = null;
				if (value != null)
				{
					StatementContext statementContext = manager.Context[typeof(StatementContext)] as StatementContext;
					if (statementContext != null)
					{
						codeStatementCollection = statementContext.StatementCollection[value];
					}
					if (codeStatementCollection != null)
					{
						manager.Context.Push(codeStatementCollection);
					}
				}
				try
				{
					codeExpression = ResourceCodeDomSerializer.Default.Serialize(manager, value, false, ensureInvariant) as CodeExpression;
				}
				finally
				{
					if (codeStatementCollection != null)
					{
						manager.Context.Pop();
					}
				}
			}
			return codeExpression;
		}

		protected void SetExpression(IDesignerSerializationManager manager, object value, CodeExpression expression)
		{
			this.SetExpression(manager, value, expression, false);
		}

		protected void SetExpression(IDesignerSerializationManager manager, object value, CodeExpression expression, bool isPreset)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			ExpressionTable expressionTable = (ExpressionTable)manager.Context[typeof(ExpressionTable)];
			if (expressionTable == null)
			{
				expressionTable = new ExpressionTable();
				manager.Context.Append(expressionTable);
			}
			expressionTable.SetExpression(value, expression, isPreset);
		}

		private void SetLegacyExpression(IDesignerSerializationManager manager, object value)
		{
			if (value is IComponent)
			{
				CodeDomSerializerBase.LegacyExpressionTable legacyExpressionTable = (CodeDomSerializerBase.LegacyExpressionTable)manager.Context[typeof(CodeDomSerializerBase.LegacyExpressionTable)];
				if (legacyExpressionTable == null)
				{
					legacyExpressionTable = new CodeDomSerializerBase.LegacyExpressionTable();
					manager.Context.Append(legacyExpressionTable);
				}
				legacyExpressionTable[value] = value;
			}
		}

		[Conditional("DEBUG")]
		internal static void Trace(string message, params object[] values)
		{
			if (CodeDomSerializerBase.traceSerialization.TraceVerbose)
			{
				int num = 0;
				int indentLevel = Debug.IndentLevel;
				if (CodeDomSerializerBase.traceScope != null)
				{
					num = CodeDomSerializerBase.traceScope.Count;
				}
				try
				{
					Debug.IndentLevel = num;
				}
				finally
				{
					Debug.IndentLevel = indentLevel;
				}
			}
		}

		[Conditional("DEBUG")]
		internal static void Trace(CodeTypeDeclaration typeDecl)
		{
			if (CodeDomSerializerBase.traceSerialization.TraceInfo)
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				new CSharpCodeProvider().GenerateCodeFromType(typeDecl, stringWriter, new CodeGeneratorOptions());
			}
		}

		[Conditional("DEBUG")]
		internal static void TraceError(string message, params object[] values)
		{
			if (CodeDomSerializerBase.traceSerialization.TraceError)
			{
				string text = string.Empty;
				if (CodeDomSerializerBase.traceScope != null)
				{
					foreach (object obj in CodeDomSerializerBase.traceScope)
					{
						string text2 = (string)obj;
						if (text.Length > 0)
						{
							text = "/" + text;
						}
						text = text2 + text;
					}
				}
			}
		}

		[Conditional("DEBUG")]
		internal static void TraceErrorIf(bool condition, string message, params object[] values)
		{
		}

		[Conditional("DEBUG")]
		internal static void TraceIf(bool condition, string message, params object[] values)
		{
		}

		internal static IDisposable TraceScope(string name)
		{
			return default(CodeDomSerializerBase.TracingScope);
		}

		[Conditional("DEBUG")]
		internal static void TraceWarning(string message, params object[] values)
		{
			if (CodeDomSerializerBase.traceSerialization.TraceWarning)
			{
				string text = string.Empty;
				if (CodeDomSerializerBase.traceScope != null)
				{
					foreach (object obj in CodeDomSerializerBase.traceScope)
					{
						string text2 = (string)obj;
						if (text.Length > 0)
						{
							text = "/" + text;
						}
						text = text2 + text;
					}
				}
			}
		}

		[Conditional("DEBUG")]
		internal static void TraceWarningIf(bool condition, string message, params object[] values)
		{
		}

		private static void AddStatement(IDictionary table, string name, CodeStatement statement)
		{
			CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection;
			if (table.Contains(name))
			{
				orderedCodeStatementCollection = (CodeDomSerializerBase.OrderedCodeStatementCollection)table[name];
			}
			else
			{
				orderedCodeStatementCollection = new CodeDomSerializerBase.OrderedCodeStatementCollection();
				orderedCodeStatementCollection.Order = table.Count;
				orderedCodeStatementCollection.Name = name;
				table[name] = orderedCodeStatementCollection;
			}
			orderedCodeStatementCollection.Add(statement);
		}

		internal static Type GetType(ITypeResolutionService trs, string name, Dictionary<string, string> names)
		{
			Type type = null;
			if (names != null && names.ContainsKey(name))
			{
				string text = names[name];
				if (trs != null && !string.IsNullOrEmpty(text))
				{
					type = trs.GetType(text, false);
				}
			}
			return type;
		}

		internal static void FillStatementTable(IDesignerSerializationManager manager, IDictionary table, CodeStatementCollection statements)
		{
			CodeDomSerializerBase.FillStatementTable(manager, table, null, statements, null);
		}

		internal static void FillStatementTable(IDesignerSerializationManager manager, IDictionary table, Dictionary<string, string> names, CodeStatementCollection statements, string className)
		{
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializerBase::FillStatementTable"))
			{
				ITypeResolutionService typeResolutionService = manager.GetService(typeof(ITypeResolutionService)) as ITypeResolutionService;
				foreach (object obj in statements)
				{
					CodeStatement codeStatement = (CodeStatement)obj;
					CodeExpression codeExpression = null;
					CodeAssignStatement codeAssignStatement;
					CodeAttachEventStatement codeAttachEventStatement;
					CodeRemoveEventStatement codeRemoveEventStatement;
					CodeExpressionStatement codeExpressionStatement;
					CodeVariableDeclarationStatement codeVariableDeclarationStatement;
					if ((codeAssignStatement = codeStatement as CodeAssignStatement) != null)
					{
						codeExpression = codeAssignStatement.Left;
					}
					else if ((codeAttachEventStatement = codeStatement as CodeAttachEventStatement) != null)
					{
						codeExpression = codeAttachEventStatement.Event;
					}
					else if ((codeRemoveEventStatement = codeStatement as CodeRemoveEventStatement) != null)
					{
						codeExpression = codeRemoveEventStatement.Event;
					}
					else if ((codeExpressionStatement = codeStatement as CodeExpressionStatement) != null)
					{
						codeExpression = codeExpressionStatement.Expression;
					}
					else if ((codeVariableDeclarationStatement = codeStatement as CodeVariableDeclarationStatement) != null)
					{
						CodeDomSerializerBase.AddStatement(table, codeVariableDeclarationStatement.Name, codeVariableDeclarationStatement);
						if (names != null && codeVariableDeclarationStatement.Type != null && !string.IsNullOrEmpty(codeVariableDeclarationStatement.Type.BaseType))
						{
							names[codeVariableDeclarationStatement.Name] = CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeVariableDeclarationStatement.Type);
						}
						codeExpression = null;
					}
					if (codeExpression != null)
					{
						CodeFieldReferenceExpression codeFieldReferenceExpression;
						bool flag;
						CodePropertyReferenceExpression codePropertyReferenceExpression;
						for (;;)
						{
							CodeCastExpression codeCastExpression;
							CodeDelegateCreateExpression codeDelegateCreateExpression;
							CodeDelegateInvokeExpression codeDelegateInvokeExpression;
							CodeDirectionExpression codeDirectionExpression;
							CodeEventReferenceExpression codeEventReferenceExpression;
							CodeMethodInvokeExpression codeMethodInvokeExpression;
							CodeMethodReferenceExpression codeMethodReferenceExpression;
							CodeArrayIndexerExpression codeArrayIndexerExpression;
							if ((codeCastExpression = codeExpression as CodeCastExpression) != null)
							{
								codeExpression = codeCastExpression.Expression;
							}
							else if ((codeDelegateCreateExpression = codeExpression as CodeDelegateCreateExpression) != null)
							{
								codeExpression = codeDelegateCreateExpression.TargetObject;
							}
							else if ((codeDelegateInvokeExpression = codeExpression as CodeDelegateInvokeExpression) != null)
							{
								codeExpression = codeDelegateInvokeExpression.TargetObject;
							}
							else if ((codeDirectionExpression = codeExpression as CodeDirectionExpression) != null)
							{
								codeExpression = codeDirectionExpression.Expression;
							}
							else if ((codeEventReferenceExpression = codeExpression as CodeEventReferenceExpression) != null)
							{
								codeExpression = codeEventReferenceExpression.TargetObject;
							}
							else if ((codeMethodInvokeExpression = codeExpression as CodeMethodInvokeExpression) != null)
							{
								codeExpression = codeMethodInvokeExpression.Method;
							}
							else if ((codeMethodReferenceExpression = codeExpression as CodeMethodReferenceExpression) != null)
							{
								codeExpression = codeMethodReferenceExpression.TargetObject;
							}
							else if ((codeArrayIndexerExpression = codeExpression as CodeArrayIndexerExpression) != null)
							{
								codeExpression = codeArrayIndexerExpression.TargetObject;
							}
							else if ((codeFieldReferenceExpression = codeExpression as CodeFieldReferenceExpression) != null)
							{
								flag = false;
								if (codeFieldReferenceExpression.TargetObject is CodeThisReferenceExpression)
								{
									break;
								}
								codeExpression = codeFieldReferenceExpression.TargetObject;
							}
							else
							{
								if ((codePropertyReferenceExpression = codeExpression as CodePropertyReferenceExpression) == null)
								{
									goto IL_02AA;
								}
								if (codePropertyReferenceExpression.TargetObject is CodeThisReferenceExpression && (names == null || names.ContainsKey(codePropertyReferenceExpression.PropertyName)))
								{
									goto IL_0288;
								}
								codeExpression = codePropertyReferenceExpression.TargetObject;
							}
						}
						Type type = CodeDomSerializerBase.GetType(typeResolutionService, codeFieldReferenceExpression.FieldName, names);
						if (type != null)
						{
							CodeDomSerializer codeDomSerializer = manager.GetSerializer(type, typeof(CodeDomSerializer)) as CodeDomSerializer;
							if (codeDomSerializer != null)
							{
								string targetComponentName = codeDomSerializer.GetTargetComponentName(codeStatement, codeExpression, type);
								if (!string.IsNullOrEmpty(targetComponentName))
								{
									CodeDomSerializerBase.AddStatement(table, targetComponentName, codeStatement);
									flag = true;
								}
							}
						}
						if (!flag)
						{
							CodeDomSerializerBase.AddStatement(table, codeFieldReferenceExpression.FieldName, codeStatement);
							continue;
						}
						continue;
						IL_0288:
						CodeDomSerializerBase.AddStatement(table, codePropertyReferenceExpression.PropertyName, codeStatement);
						continue;
						IL_02AA:
						CodeVariableReferenceExpression codeVariableReferenceExpression;
						if ((codeVariableReferenceExpression = codeExpression as CodeVariableReferenceExpression) != null)
						{
							bool flag2 = false;
							if (names != null)
							{
								Type type2 = CodeDomSerializerBase.GetType(typeResolutionService, codeVariableReferenceExpression.VariableName, names);
								if (type2 != null)
								{
									CodeDomSerializer codeDomSerializer2 = manager.GetSerializer(type2, typeof(CodeDomSerializer)) as CodeDomSerializer;
									if (codeDomSerializer2 != null)
									{
										string targetComponentName2 = codeDomSerializer2.GetTargetComponentName(codeStatement, codeExpression, type2);
										if (!string.IsNullOrEmpty(targetComponentName2))
										{
											CodeDomSerializerBase.AddStatement(table, targetComponentName2, codeStatement);
											flag2 = true;
										}
									}
								}
							}
							else
							{
								CodeDomSerializerBase.AddStatement(table, codeVariableReferenceExpression.VariableName, codeStatement);
								flag2 = true;
							}
							if (!flag2)
							{
								manager.ReportError(new CodeDomSerializerException(SR.GetString("SerializerUndeclaredName", new object[] { codeVariableReferenceExpression.VariableName }), manager));
							}
						}
						else if ((codeExpression is CodeThisReferenceExpression || codeExpression is CodeBaseReferenceExpression) && className != null)
						{
							CodeDomSerializerBase.AddStatement(table, className, codeStatement);
						}
					}
				}
			}
		}

		private static readonly Attribute[] runTimeProperties = new Attribute[] { DesignOnlyAttribute.No };

		private static readonly CodeThisReferenceExpression thisRef = new CodeThisReferenceExpression();

		private static TraceSwitch traceSerialization = new TraceSwitch("DesignerSerialization", "Trace design time serialization");

		private static Stack traceScope;

		private class LegacyExpressionTable : Hashtable
		{
		}

		private struct TracingScope : IDisposable
		{
			public void Dispose()
			{
				if (CodeDomSerializerBase.traceScope != null)
				{
					CodeDomSerializerBase.traceScope.Pop();
				}
			}
		}

		internal class OrderedCodeStatementCollection : CodeStatementCollection
		{
			public int Order;

			public string Name;
		}
	}
}
