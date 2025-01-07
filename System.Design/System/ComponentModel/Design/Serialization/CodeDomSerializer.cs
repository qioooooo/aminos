using System;
using System.CodeDom;
using System.Collections;
using System.Design;
using System.Globalization;

namespace System.ComponentModel.Design.Serialization
{
	[DefaultSerializationProvider(typeof(CodeDomSerializationProvider))]
	public class CodeDomSerializer : CodeDomSerializerBase
	{
		internal static CodeDomSerializer Default
		{
			get
			{
				if (CodeDomSerializer._default == null)
				{
					CodeDomSerializer._default = new CodeDomSerializer();
				}
				return CodeDomSerializer._default;
			}
		}

		public virtual string GetTargetComponentName(CodeStatement statement, CodeExpression expression, Type targetType)
		{
			string text = null;
			CodeVariableReferenceExpression codeVariableReferenceExpression;
			CodeFieldReferenceExpression codeFieldReferenceExpression;
			if ((codeVariableReferenceExpression = expression as CodeVariableReferenceExpression) != null)
			{
				text = codeVariableReferenceExpression.VariableName;
			}
			else if ((codeFieldReferenceExpression = expression as CodeFieldReferenceExpression) != null)
			{
				text = codeFieldReferenceExpression.FieldName;
			}
			return text;
		}

		public virtual object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			object obj = null;
			if (manager == null || codeObject == null)
			{
				throw new ArgumentNullException((manager == null) ? "manager" : "codeObject");
			}
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializer::Deserialize"))
			{
				CodeExpression codeExpression = codeObject as CodeExpression;
				if (codeExpression != null)
				{
					obj = base.DeserializeExpression(manager, null, codeExpression);
				}
				else
				{
					CodeStatementCollection codeStatementCollection = codeObject as CodeStatementCollection;
					if (codeStatementCollection != null)
					{
						using (IEnumerator enumerator = codeStatementCollection.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj2 = enumerator.Current;
								CodeStatement codeStatement = (CodeStatement)obj2;
								if (obj == null)
								{
									obj = this.DeserializeStatementToInstance(manager, codeStatement);
								}
								else
								{
									base.DeserializeStatement(manager, codeStatement);
								}
							}
							goto IL_012E;
						}
					}
					if (!(codeObject is CodeStatement))
					{
						string text = string.Format(CultureInfo.CurrentCulture, "{0}, {1}, {2}", new object[]
						{
							typeof(CodeExpression).Name,
							typeof(CodeStatement).Name,
							typeof(CodeStatementCollection).Name
						});
						throw new ArgumentException(SR.GetString("SerializerBadElementTypes", new object[]
						{
							codeObject.GetType().Name,
							text
						}));
					}
				}
				IL_012E:;
			}
			return obj;
		}

		protected object DeserializeStatementToInstance(IDesignerSerializationManager manager, CodeStatement statement)
		{
			object obj = null;
			CodeAssignStatement codeAssignStatement;
			CodeVariableDeclarationStatement codeVariableDeclarationStatement;
			if ((codeAssignStatement = statement as CodeAssignStatement) != null)
			{
				CodeFieldReferenceExpression codeFieldReferenceExpression = codeAssignStatement.Left as CodeFieldReferenceExpression;
				if (codeFieldReferenceExpression != null)
				{
					obj = base.DeserializeExpression(manager, codeFieldReferenceExpression.FieldName, codeAssignStatement.Right);
				}
				else
				{
					CodeVariableReferenceExpression codeVariableReferenceExpression = codeAssignStatement.Left as CodeVariableReferenceExpression;
					if (codeVariableReferenceExpression != null)
					{
						obj = base.DeserializeExpression(manager, codeVariableReferenceExpression.VariableName, codeAssignStatement.Right);
					}
					else
					{
						base.DeserializeStatement(manager, codeAssignStatement);
					}
				}
			}
			else if ((codeVariableDeclarationStatement = statement as CodeVariableDeclarationStatement) != null && codeVariableDeclarationStatement.InitExpression != null)
			{
				obj = base.DeserializeExpression(manager, codeVariableDeclarationStatement.Name, codeVariableDeclarationStatement.InitExpression);
			}
			else
			{
				base.DeserializeStatement(manager, statement);
			}
			return obj;
		}

		public virtual object Serialize(IDesignerSerializationManager manager, object value)
		{
			object obj = null;
			if (manager == null || value == null)
			{
				throw new ArgumentNullException((manager == null) ? "manager" : "value");
			}
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializer::Serialize"))
			{
				if (value is Type)
				{
					obj = new CodeTypeOfExpression((Type)value);
				}
				else
				{
					bool flag = false;
					bool flag2;
					CodeExpression codeExpression = base.SerializeCreationExpression(manager, value, out flag2);
					if (!(value is IComponent))
					{
						flag = flag2;
					}
					ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
					bool flag3 = expressionContext != null && object.ReferenceEquals(expressionContext.PresetValue, value);
					if (codeExpression != null)
					{
						if (flag)
						{
							obj = codeExpression;
						}
						else
						{
							CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
							if (flag3)
							{
								base.SetExpression(manager, value, codeExpression, true);
							}
							else
							{
								string uniqueName = base.GetUniqueName(manager, value);
								string className = TypeDescriptor.GetClassName(value);
								codeStatementCollection.Add(new CodeVariableDeclarationStatement(className, uniqueName)
								{
									InitExpression = codeExpression
								});
								CodeExpression codeExpression2 = new CodeVariableReferenceExpression(uniqueName);
								base.SetExpression(manager, value, codeExpression2);
							}
							base.SerializePropertiesToResources(manager, codeStatementCollection, value, CodeDomSerializer._designTimeFilter);
							base.SerializeProperties(manager, codeStatementCollection, value, CodeDomSerializer._runTimeFilter);
							base.SerializeEvents(manager, codeStatementCollection, value, CodeDomSerializer._runTimeFilter);
							obj = codeStatementCollection;
						}
					}
				}
			}
			return obj;
		}

		public virtual object SerializeAbsolute(IDesignerSerializationManager manager, object value)
		{
			SerializeAbsoluteContext serializeAbsoluteContext = new SerializeAbsoluteContext();
			manager.Context.Push(serializeAbsoluteContext);
			object obj;
			try
			{
				obj = this.Serialize(manager, value);
			}
			finally
			{
				manager.Context.Pop();
			}
			return obj;
		}

		public virtual CodeStatementCollection SerializeMember(IDesignerSerializationManager manager, object owningObject, MemberDescriptor member)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (owningObject == null)
			{
				throw new ArgumentNullException("owningObject");
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
			if (base.GetExpression(manager, owningObject) == null)
			{
				string uniqueName = base.GetUniqueName(manager, owningObject);
				CodeExpression codeExpression = new CodeVariableReferenceExpression(uniqueName);
				base.SetExpression(manager, owningObject, codeExpression);
			}
			PropertyDescriptor propertyDescriptor = member as PropertyDescriptor;
			if (propertyDescriptor != null)
			{
				base.SerializeProperty(manager, codeStatementCollection, owningObject, propertyDescriptor);
			}
			else
			{
				EventDescriptor eventDescriptor = member as EventDescriptor;
				if (eventDescriptor == null)
				{
					throw new NotSupportedException(SR.GetString("SerializerMemberTypeNotSerializable", new object[] { member.GetType().FullName }));
				}
				base.SerializeEvent(manager, codeStatementCollection, owningObject, eventDescriptor);
			}
			return codeStatementCollection;
		}

		public virtual CodeStatementCollection SerializeMemberAbsolute(IDesignerSerializationManager manager, object owningObject, MemberDescriptor member)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (owningObject == null)
			{
				throw new ArgumentNullException("owningObject");
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			SerializeAbsoluteContext serializeAbsoluteContext = new SerializeAbsoluteContext(member);
			manager.Context.Push(serializeAbsoluteContext);
			CodeStatementCollection codeStatementCollection;
			try
			{
				codeStatementCollection = this.SerializeMember(manager, owningObject, member);
			}
			finally
			{
				manager.Context.Pop();
			}
			return codeStatementCollection;
		}

		[Obsolete("This method has been deprecated. Use SerializeToExpression or GetExpression instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected CodeExpression SerializeToReferenceExpression(IDesignerSerializationManager manager, object value)
		{
			CodeExpression codeExpression = null;
			using (CodeDomSerializerBase.TraceScope("CodeDomSerializer::SerializeToReferenceExpression"))
			{
				codeExpression = base.GetExpression(manager, value);
				if (codeExpression == null && value is IComponent)
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
						if (rootContext != null && rootContext.Value == value)
						{
							codeExpression = rootContext.Expression;
						}
						else if (flag && text.IndexOf('.') != -1)
						{
							int num = text.IndexOf('.');
							codeExpression = new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(CodeDomSerializer._thisRef, text.Substring(0, num)), text.Substring(num + 1));
						}
						else
						{
							codeExpression = new CodeFieldReferenceExpression(CodeDomSerializer._thisRef, text);
						}
					}
				}
			}
			return codeExpression;
		}

		private static CodeDomSerializer _default;

		private static readonly Attribute[] _runTimeFilter = new Attribute[] { DesignOnlyAttribute.No };

		private static readonly Attribute[] _designTimeFilter = new Attribute[] { DesignOnlyAttribute.Yes };

		private static CodeThisReferenceExpression _thisRef = new CodeThisReferenceExpression();
	}
}
