using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Design;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel.Design.Serialization
{
	public class CollectionCodeDomSerializer : CodeDomSerializer
	{
		internal new static CollectionCodeDomSerializer Default
		{
			get
			{
				if (CollectionCodeDomSerializer.defaultSerializer == null)
				{
					CollectionCodeDomSerializer.defaultSerializer = new CollectionCodeDomSerializer();
				}
				return CollectionCodeDomSerializer.defaultSerializer;
			}
		}

		private ICollection GetCollectionDelta(ICollection original, ICollection modified)
		{
			if (original == null || modified == null || original.Count == 0)
			{
				return modified;
			}
			IEnumerator enumerator = modified.GetEnumerator();
			if (enumerator == null)
			{
				return modified;
			}
			IDictionary dictionary = new HybridDictionary();
			foreach (object obj in original)
			{
				if (dictionary.Contains(obj))
				{
					int num = (int)dictionary[obj];
					dictionary[obj] = num + 1;
				}
				else
				{
					dictionary.Add(obj, 1);
				}
			}
			ArrayList arrayList = null;
			int num2 = 0;
			while (num2 < modified.Count && enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				if (dictionary.Contains(obj2))
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						enumerator.Reset();
						int num3 = 0;
						while (num3 < num2 && enumerator.MoveNext())
						{
							arrayList.Add(enumerator.Current);
							num3++;
						}
						enumerator.MoveNext();
					}
					int num4 = (int)dictionary[obj2];
					if (--num4 == 0)
					{
						dictionary.Remove(obj2);
					}
					else
					{
						dictionary[obj2] = num4;
					}
				}
				else if (arrayList != null)
				{
					arrayList.Add(obj2);
				}
				num2++;
			}
			if (arrayList != null)
			{
				return arrayList;
			}
			return modified;
		}

		protected bool MethodSupportsSerialization(MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			object[] customAttributes = method.GetCustomAttributes(typeof(DesignerSerializationVisibilityAttribute), true);
			if (customAttributes.Length > 0)
			{
				DesignerSerializationVisibilityAttribute designerSerializationVisibilityAttribute = (DesignerSerializationVisibilityAttribute)customAttributes[0];
				if (designerSerializationVisibilityAttribute != null && designerSerializationVisibilityAttribute.Visibility == DesignerSerializationVisibility.Hidden)
				{
					return false;
				}
			}
			return true;
		}

		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			object obj = null;
			using (CodeDomSerializerBase.TraceScope("CollectionCodeDomSerializer::Serialize"))
			{
				ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
				PropertyDescriptor propertyDescriptor = manager.Context[typeof(PropertyDescriptor)] as PropertyDescriptor;
				CodeExpression codeExpression;
				if (expressionContext != null && expressionContext.PresetValue == value && propertyDescriptor != null && propertyDescriptor.PropertyType == expressionContext.ExpressionType)
				{
					codeExpression = expressionContext.Expression;
				}
				else
				{
					codeExpression = null;
					expressionContext = null;
					propertyDescriptor = null;
				}
				ICollection collection = value as ICollection;
				if (collection != null)
				{
					ICollection collection2 = collection;
					InheritedPropertyDescriptor inheritedPropertyDescriptor = propertyDescriptor as InheritedPropertyDescriptor;
					Type type = ((expressionContext == null) ? collection.GetType() : expressionContext.ExpressionType);
					bool flag = typeof(Array).IsAssignableFrom(type);
					if (codeExpression == null && !flag)
					{
						bool flag2;
						codeExpression = base.SerializeCreationExpression(manager, collection, out flag2);
						if (flag2)
						{
							return codeExpression;
						}
					}
					if (codeExpression != null || flag)
					{
						if (inheritedPropertyDescriptor != null && !flag)
						{
							collection2 = this.GetCollectionDelta(inheritedPropertyDescriptor.OriginalValue as ICollection, collection);
						}
						obj = this.SerializeCollection(manager, codeExpression, type, collection, collection2);
						if (codeExpression != null && this.ShouldClearCollection(manager, collection))
						{
							CodeStatementCollection codeStatementCollection = obj as CodeStatementCollection;
							if (collection.Count > 0 && (obj == null || (codeStatementCollection != null && codeStatementCollection.Count == 0)))
							{
								return null;
							}
							if (codeStatementCollection == null)
							{
								codeStatementCollection = new CodeStatementCollection();
								CodeStatement codeStatement = obj as CodeStatement;
								if (codeStatement != null)
								{
									codeStatementCollection.Add(codeStatement);
								}
								obj = codeStatementCollection;
							}
							if (codeStatementCollection != null)
							{
								CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeExpression, "Clear", new CodeExpression[0]);
								CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(codeMethodInvokeExpression);
								codeStatementCollection.Insert(0, codeExpressionStatement);
							}
						}
					}
				}
			}
			return obj;
		}

		private static MethodInfo ChooseMethodByType(List<MethodInfo> methods, ICollection values)
		{
			MethodInfo methodInfo = null;
			Type type = null;
			foreach (object obj in values)
			{
				Type reflectionType = TypeDescriptor.GetReflectionType(obj);
				MethodInfo methodInfo2 = null;
				Type type2 = null;
				if (methodInfo == null || (type != null && !type.IsAssignableFrom(reflectionType)))
				{
					foreach (MethodInfo methodInfo3 in methods)
					{
						ParameterInfo parameterInfo = methodInfo3.GetParameters()[0];
						if (parameterInfo != null)
						{
							Type type3 = (parameterInfo.ParameterType.IsArray ? parameterInfo.ParameterType.GetElementType() : parameterInfo.ParameterType);
							if (type3 != null && type3.IsAssignableFrom(reflectionType))
							{
								if (methodInfo != null)
								{
									if (type3.IsAssignableFrom(type))
									{
										methodInfo = methodInfo3;
										type = type3;
										break;
									}
								}
								else if (methodInfo2 == null)
								{
									methodInfo2 = methodInfo3;
									type2 = type3;
								}
								else
								{
									bool flag = type2.IsAssignableFrom(type3);
									methodInfo2 = (flag ? methodInfo3 : methodInfo2);
									type2 = (flag ? type3 : type2);
								}
							}
						}
					}
				}
				if (methodInfo == null)
				{
					methodInfo = methodInfo2;
					type = type2;
				}
			}
			return methodInfo;
		}

		protected virtual object SerializeCollection(IDesignerSerializationManager manager, CodeExpression targetExpression, Type targetType, ICollection originalCollection, ICollection valuesToSerialize)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (originalCollection == null)
			{
				throw new ArgumentNullException("originalCollection");
			}
			if (valuesToSerialize == null)
			{
				throw new ArgumentNullException("valuesToSerialize");
			}
			object obj = null;
			bool flag = false;
			if (typeof(Array).IsAssignableFrom(targetType))
			{
				CodeArrayCreateExpression codeArrayCreateExpression = this.SerializeArray(manager, targetType, originalCollection, valuesToSerialize);
				if (codeArrayCreateExpression != null)
				{
					if (targetExpression != null)
					{
						obj = new CodeAssignStatement(targetExpression, codeArrayCreateExpression);
					}
					else
					{
						obj = codeArrayCreateExpression;
					}
				}
			}
			else if (valuesToSerialize.Count > 0)
			{
				MethodInfo[] methods = TypeDescriptor.GetReflectionType(originalCollection).GetMethods(BindingFlags.Instance | BindingFlags.Public);
				List<MethodInfo> list = new List<MethodInfo>();
				List<MethodInfo> list2 = new List<MethodInfo>();
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.Name.Equals("AddRange"))
					{
						ParameterInfo[] array2 = methodInfo.GetParameters();
						if (array2.Length == 1 && array2[0].ParameterType.IsArray && this.MethodSupportsSerialization(methodInfo))
						{
							list.Add(methodInfo);
						}
					}
					if (methodInfo.Name.Equals("Add"))
					{
						ParameterInfo[] array2 = methodInfo.GetParameters();
						if (array2.Length == 1 && this.MethodSupportsSerialization(methodInfo))
						{
							list2.Add(methodInfo);
						}
					}
				}
				MethodInfo methodInfo2 = CollectionCodeDomSerializer.ChooseMethodByType(list, valuesToSerialize);
				if (methodInfo2 != null)
				{
					obj = this.SerializeViaAddRange(manager, targetExpression, targetType, methodInfo2.GetParameters()[0], valuesToSerialize);
					flag = true;
				}
				else
				{
					MethodInfo methodInfo3 = CollectionCodeDomSerializer.ChooseMethodByType(list2, valuesToSerialize);
					if (methodInfo3 != null)
					{
						obj = this.SerializeViaAdd(manager, targetExpression, targetType, methodInfo3.GetParameters()[0], valuesToSerialize);
						flag = true;
					}
				}
				if (!flag && originalCollection.GetType().IsSerializable)
				{
					obj = base.SerializeToResourceExpression(manager, originalCollection, false);
				}
			}
			return obj;
		}

		private CodeArrayCreateExpression SerializeArray(IDesignerSerializationManager manager, Type targetType, ICollection array, ICollection valuesToSerialize)
		{
			CodeArrayCreateExpression codeArrayCreateExpression = null;
			using (CodeDomSerializerBase.TraceScope("CollectionCodeDomSerializer::SerializeArray"))
			{
				if (((Array)array).Rank != 1)
				{
					manager.ReportError(SR.GetString("SerializerInvalidArrayRank", new object[] { ((Array)array).Rank.ToString(CultureInfo.InvariantCulture) }));
				}
				else
				{
					Type elementType = targetType.GetElementType();
					CodeTypeReference codeTypeReference = new CodeTypeReference(elementType);
					CodeArrayCreateExpression codeArrayCreateExpression2 = new CodeArrayCreateExpression();
					codeArrayCreateExpression2.CreateType = codeTypeReference;
					bool flag = true;
					foreach (object obj in valuesToSerialize)
					{
						if (obj is IComponent && TypeDescriptor.GetAttributes(obj).Contains(InheritanceAttribute.InheritedReadOnly))
						{
							flag = false;
							break;
						}
						CodeExpression codeExpression = null;
						ExpressionContext expressionContext = null;
						ExpressionContext expressionContext2 = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
						if (expressionContext2 != null)
						{
							expressionContext = new ExpressionContext(expressionContext2.Expression, elementType, expressionContext2.Owner);
							manager.Context.Push(expressionContext);
						}
						try
						{
							codeExpression = base.SerializeToExpression(manager, obj);
						}
						finally
						{
							if (expressionContext != null)
							{
								manager.Context.Pop();
							}
						}
						if (codeExpression == null)
						{
							flag = false;
							break;
						}
						if (obj != null && obj.GetType() != elementType)
						{
							codeExpression = new CodeCastExpression(elementType, codeExpression);
						}
						codeArrayCreateExpression2.Initializers.Add(codeExpression);
					}
					if (flag)
					{
						codeArrayCreateExpression = codeArrayCreateExpression2;
					}
				}
			}
			return codeArrayCreateExpression;
		}

		private object SerializeViaAdd(IDesignerSerializationManager manager, CodeExpression targetExpression, Type targetType, ParameterInfo parameter, ICollection valuesToSerialize)
		{
			CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
			using (CodeDomSerializerBase.TraceScope("CollectionCodeDomSerializer::SerializeViaAdd"))
			{
				CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(targetExpression, "Add");
				if (valuesToSerialize.Count > 0)
				{
					ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
					foreach (object obj in valuesToSerialize)
					{
						bool flag = !(obj is IComponent);
						if (!flag)
						{
							InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(obj)[typeof(InheritanceAttribute)];
							flag = inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly;
						}
						if (flag)
						{
							CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
							codeMethodInvokeExpression.Method = codeMethodReferenceExpression;
							CodeExpression codeExpression = null;
							Type parameterType = parameter.ParameterType;
							ExpressionContext expressionContext2 = null;
							if (expressionContext != null)
							{
								expressionContext2 = new ExpressionContext(expressionContext.Expression, parameterType, expressionContext.Owner);
								manager.Context.Push(expressionContext2);
							}
							try
							{
								codeExpression = base.SerializeToExpression(manager, obj);
							}
							finally
							{
								if (expressionContext2 != null)
								{
									manager.Context.Pop();
								}
							}
							if (obj != null && !parameterType.IsAssignableFrom(obj.GetType()) && obj.GetType().IsPrimitive)
							{
								codeExpression = new CodeCastExpression(parameterType, codeExpression);
							}
							if (codeExpression != null)
							{
								codeMethodInvokeExpression.Parameters.Add(codeExpression);
								codeStatementCollection.Add(codeMethodInvokeExpression);
							}
						}
					}
				}
			}
			return codeStatementCollection;
		}

		private object SerializeViaAddRange(IDesignerSerializationManager manager, CodeExpression targetExpression, Type targetType, ParameterInfo parameter, ICollection valuesToSerialize)
		{
			CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
			using (CodeDomSerializerBase.TraceScope("CollectionCodeDomSerializer::SerializeViaAddRange"))
			{
				if (valuesToSerialize.Count > 0)
				{
					ArrayList arrayList = new ArrayList(valuesToSerialize.Count);
					ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
					Type elementType = parameter.ParameterType.GetElementType();
					foreach (object obj in valuesToSerialize)
					{
						bool flag = !(obj is IComponent);
						if (!flag)
						{
							InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(obj)[typeof(InheritanceAttribute)];
							flag = inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly;
						}
						if (flag)
						{
							CodeExpression codeExpression = null;
							ExpressionContext expressionContext2 = null;
							if (expressionContext != null)
							{
								expressionContext2 = new ExpressionContext(expressionContext.Expression, elementType, expressionContext.Owner);
								manager.Context.Push(expressionContext2);
							}
							try
							{
								codeExpression = base.SerializeToExpression(manager, obj);
							}
							finally
							{
								if (expressionContext2 != null)
								{
									manager.Context.Pop();
								}
							}
							if (codeExpression != null)
							{
								if (obj != null && !elementType.IsAssignableFrom(obj.GetType()))
								{
									codeExpression = new CodeCastExpression(elementType, codeExpression);
								}
								arrayList.Add(codeExpression);
							}
						}
					}
					if (arrayList.Count > 0)
					{
						CodeTypeReference codeTypeReference = new CodeTypeReference(elementType);
						CodeArrayCreateExpression codeArrayCreateExpression = new CodeArrayCreateExpression();
						codeArrayCreateExpression.CreateType = codeTypeReference;
						foreach (object obj2 in arrayList)
						{
							CodeExpression codeExpression2 = (CodeExpression)obj2;
							codeArrayCreateExpression.Initializers.Add(codeExpression2);
						}
						CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(targetExpression, "AddRange");
						codeStatementCollection.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression
						{
							Method = codeMethodReferenceExpression,
							Parameters = { codeArrayCreateExpression }
						}));
					}
				}
			}
			return codeStatementCollection;
		}

		private bool ShouldClearCollection(IDesignerSerializationManager manager, ICollection collection)
		{
			bool flag = false;
			PropertyDescriptor propertyDescriptor = manager.Properties["ClearCollections"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && (bool)propertyDescriptor.GetValue(manager))
			{
				flag = true;
			}
			if (!flag)
			{
				SerializeAbsoluteContext serializeAbsoluteContext = (SerializeAbsoluteContext)manager.Context[typeof(SerializeAbsoluteContext)];
				PropertyDescriptor propertyDescriptor2 = manager.Context[typeof(PropertyDescriptor)] as PropertyDescriptor;
				if (serializeAbsoluteContext != null && serializeAbsoluteContext.ShouldSerialize(propertyDescriptor2))
				{
					flag = true;
				}
			}
			if (flag)
			{
				MethodInfo method = TypeDescriptor.GetReflectionType(collection).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
				if (method == null || !this.MethodSupportsSerialization(method))
				{
					flag = false;
				}
			}
			return flag;
		}

		private static CollectionCodeDomSerializer defaultSerializer;
	}
}
