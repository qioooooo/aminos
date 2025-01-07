using System;
using System.CodeDom;
using System.Collections;
using System.Design;
using System.Reflection;

namespace System.ComponentModel.Design.Serialization
{
	internal sealed class PropertyMemberCodeDomSerializer : MemberCodeDomSerializer
	{
		internal static PropertyMemberCodeDomSerializer Default
		{
			get
			{
				if (PropertyMemberCodeDomSerializer._default == null)
				{
					PropertyMemberCodeDomSerializer._default = new PropertyMemberCodeDomSerializer();
				}
				return PropertyMemberCodeDomSerializer._default;
			}
		}

		private object GetPropertyValue(IDesignerSerializationManager manager, PropertyDescriptor property, object value, out bool validValue)
		{
			object obj = null;
			validValue = true;
			try
			{
				if (!property.ShouldSerializeValue(value))
				{
					AmbientValueAttribute ambientValueAttribute = (AmbientValueAttribute)property.Attributes[typeof(AmbientValueAttribute)];
					if (ambientValueAttribute != null)
					{
						return ambientValueAttribute.Value;
					}
					DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)property.Attributes[typeof(DefaultValueAttribute)];
					if (defaultValueAttribute != null)
					{
						return defaultValueAttribute.Value;
					}
					validValue = false;
				}
				obj = property.GetValue(value);
			}
			catch (Exception ex)
			{
				validValue = false;
				manager.ReportError(SR.GetString("SerializerPropertyGenFailed", new object[] { property.Name, ex.Message }));
			}
			return obj;
		}

		public override void Serialize(IDesignerSerializationManager manager, object value, MemberDescriptor descriptor, CodeStatementCollection statements)
		{
			PropertyDescriptor propertyDescriptor = descriptor as PropertyDescriptor;
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (propertyDescriptor == null)
			{
				throw new ArgumentNullException("descriptor");
			}
			if (statements == null)
			{
				throw new ArgumentNullException("statements");
			}
			try
			{
				ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = (ExtenderProvidedPropertyAttribute)propertyDescriptor.Attributes[typeof(ExtenderProvidedPropertyAttribute)];
				bool flag = extenderProvidedPropertyAttribute != null && extenderProvidedPropertyAttribute.Provider != null;
				bool flag2 = propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content);
				if (flag2)
				{
					this.SerializeContentProperty(manager, value, propertyDescriptor, flag, statements);
				}
				else if (flag)
				{
					this.SerializeExtenderProperty(manager, value, propertyDescriptor, statements);
				}
				else
				{
					this.SerializeNormalProperty(manager, value, propertyDescriptor, statements);
				}
			}
			catch (Exception innerException)
			{
				if (innerException is TargetInvocationException)
				{
					innerException = innerException.InnerException;
				}
				manager.ReportError(SR.GetString("SerializerPropertyGenFailed", new object[] { propertyDescriptor.Name, innerException.Message }));
			}
		}

		private void SerializeContentProperty(IDesignerSerializationManager manager, object value, PropertyDescriptor property, bool isExtender, CodeStatementCollection statements)
		{
			bool flag;
			object propertyValue = this.GetPropertyValue(manager, property, value, out flag);
			if (propertyValue == null)
			{
				string text = manager.GetName(value);
				if (text == null)
				{
					text = value.GetType().FullName;
				}
				manager.ReportError(SR.GetString("SerializerNullNestedProperty", new object[] { text, property.Name }));
				return;
			}
			CodeDomSerializer codeDomSerializer = (CodeDomSerializer)manager.GetSerializer(propertyValue.GetType(), typeof(CodeDomSerializer));
			if (codeDomSerializer != null)
			{
				CodeExpression codeExpression = base.SerializeToExpression(manager, value);
				if (codeExpression == null)
				{
					return;
				}
				CodeExpression codeExpression2 = null;
				if (isExtender)
				{
					ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = (ExtenderProvidedPropertyAttribute)property.Attributes[typeof(ExtenderProvidedPropertyAttribute)];
					CodeExpression codeExpression3 = base.SerializeToExpression(manager, extenderProvidedPropertyAttribute.Provider);
					CodeExpression codeExpression4 = base.SerializeToExpression(manager, value);
					if (codeExpression3 != null && codeExpression4 != null)
					{
						CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(codeExpression3, "Get" + property.Name);
						codeExpression2 = new CodeMethodInvokeExpression
						{
							Method = codeMethodReferenceExpression,
							Parameters = { codeExpression4 }
						};
					}
				}
				else
				{
					codeExpression2 = new CodePropertyReferenceExpression(codeExpression, property.Name);
				}
				if (codeExpression2 != null)
				{
					ExpressionContext expressionContext = new ExpressionContext(codeExpression2, property.PropertyType, value, propertyValue);
					manager.Context.Push(expressionContext);
					object obj = null;
					try
					{
						SerializeAbsoluteContext serializeAbsoluteContext = (SerializeAbsoluteContext)manager.Context[typeof(SerializeAbsoluteContext)];
						if (base.IsSerialized(manager, propertyValue, serializeAbsoluteContext != null))
						{
							obj = base.GetExpression(manager, propertyValue);
						}
						else
						{
							obj = codeDomSerializer.Serialize(manager, propertyValue);
						}
					}
					finally
					{
						manager.Context.Pop();
					}
					CodeStatementCollection codeStatementCollection = obj as CodeStatementCollection;
					if (codeStatementCollection != null)
					{
						using (IEnumerator enumerator = codeStatementCollection.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj2 = enumerator.Current;
								CodeStatement codeStatement = (CodeStatement)obj2;
								statements.Add(codeStatement);
							}
							return;
						}
					}
					CodeStatement codeStatement2 = obj as CodeStatement;
					if (codeStatement2 != null)
					{
						statements.Add(codeStatement2);
						return;
					}
				}
			}
			else
			{
				manager.ReportError(SR.GetString("SerializerNoSerializerForComponent", new object[] { property.PropertyType.FullName }));
			}
		}

		private void SerializeExtenderProperty(IDesignerSerializationManager manager, object value, PropertyDescriptor property, CodeStatementCollection statements)
		{
			AttributeCollection attributes = property.Attributes;
			using (CodeDomSerializerBase.TraceScope("PropertyMemberCodeDomSerializer::SerializeExtenderProperty"))
			{
				ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = (ExtenderProvidedPropertyAttribute)attributes[typeof(ExtenderProvidedPropertyAttribute)];
				CodeExpression codeExpression = base.SerializeToExpression(manager, extenderProvidedPropertyAttribute.Provider);
				CodeExpression codeExpression2 = base.SerializeToExpression(manager, value);
				if (codeExpression != null && codeExpression2 != null)
				{
					CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(codeExpression, "Set" + property.Name);
					bool flag;
					object propertyValue = this.GetPropertyValue(manager, property, value, out flag);
					CodeExpression codeExpression3 = null;
					if (flag)
					{
						ExpressionContext expressionContext = null;
						if (propertyValue != value)
						{
							expressionContext = new ExpressionContext(codeMethodReferenceExpression, property.PropertyType, value);
							manager.Context.Push(expressionContext);
						}
						try
						{
							codeExpression3 = base.SerializeToExpression(manager, propertyValue);
						}
						finally
						{
							if (expressionContext != null)
							{
								manager.Context.Pop();
							}
						}
					}
					if (codeExpression3 != null)
					{
						statements.Add(new CodeMethodInvokeExpression
						{
							Method = codeMethodReferenceExpression,
							Parameters = { codeExpression2, codeExpression3 }
						});
					}
				}
			}
		}

		private void SerializeNormalProperty(IDesignerSerializationManager manager, object value, PropertyDescriptor property, CodeStatementCollection statements)
		{
			AttributeCollection attributes = property.Attributes;
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializer::SerializeProperty"))
			{
				CodeExpression codeExpression = base.SerializeToExpression(manager, value);
				if (codeExpression != null)
				{
					CodeExpression codeExpression2 = new CodePropertyReferenceExpression(codeExpression, property.Name);
					CodeExpression codeExpression3 = null;
					MemberRelationshipService memberRelationshipService = manager.GetService(typeof(MemberRelationshipService)) as MemberRelationshipService;
					if (memberRelationshipService != null)
					{
						MemberRelationship memberRelationship = memberRelationshipService[value, property];
						if (memberRelationship != MemberRelationship.Empty)
						{
							CodeExpression codeExpression4 = base.SerializeToExpression(manager, memberRelationship.Owner);
							if (codeExpression4 != null)
							{
								codeExpression3 = new CodePropertyReferenceExpression(codeExpression4, memberRelationship.Member.Name);
							}
						}
					}
					if (codeExpression3 == null)
					{
						bool flag;
						object propertyValue = this.GetPropertyValue(manager, property, value, out flag);
						if (flag)
						{
							ExpressionContext expressionContext = null;
							if (propertyValue != value)
							{
								expressionContext = new ExpressionContext(codeExpression2, property.PropertyType, value);
								manager.Context.Push(expressionContext);
							}
							try
							{
								codeExpression3 = base.SerializeToExpression(manager, propertyValue);
							}
							finally
							{
								if (expressionContext != null)
								{
									manager.Context.Pop();
								}
							}
						}
					}
					if (codeExpression3 != null)
					{
						CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeExpression2, codeExpression3);
						statements.Add(codeAssignStatement);
					}
				}
			}
		}

		public override bool ShouldSerialize(IDesignerSerializationManager manager, object value, MemberDescriptor descriptor)
		{
			PropertyDescriptor propertyDescriptor = descriptor as PropertyDescriptor;
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (propertyDescriptor == null)
			{
				throw new ArgumentNullException("descriptor");
			}
			bool flag = propertyDescriptor.ShouldSerializeValue(value);
			if (!flag)
			{
				SerializeAbsoluteContext serializeAbsoluteContext = (SerializeAbsoluteContext)manager.Context[typeof(SerializeAbsoluteContext)];
				if (serializeAbsoluteContext != null && serializeAbsoluteContext.ShouldSerialize(propertyDescriptor))
				{
					flag = propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content);
				}
			}
			if (flag && !propertyDescriptor.Attributes.Contains(DesignOnlyAttribute.Yes))
			{
				return true;
			}
			MemberRelationshipService memberRelationshipService = manager.GetService(typeof(MemberRelationshipService)) as MemberRelationshipService;
			if (memberRelationshipService != null)
			{
				MemberRelationship memberRelationship = memberRelationshipService[value, descriptor];
				if (memberRelationship != MemberRelationship.Empty)
				{
					return true;
				}
			}
			return false;
		}

		private static PropertyMemberCodeDomSerializer _default;
	}
}
