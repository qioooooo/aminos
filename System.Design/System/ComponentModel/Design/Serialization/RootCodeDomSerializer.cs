using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Design;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.ComponentModel.Design.Serialization
{
	internal sealed class RootCodeDomSerializer : ComponentCodeDomSerializer
	{
		public string ContainerName
		{
			get
			{
				return "components";
			}
		}

		public bool ContainerRequired
		{
			get
			{
				return this.containerRequired;
			}
			set
			{
				this.containerRequired = value;
			}
		}

		public string InitMethodName
		{
			get
			{
				return "InitializeComponent";
			}
		}

		private void AddStatement(string name, CodeStatement statement)
		{
			CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection = (CodeDomSerializerBase.OrderedCodeStatementCollection)this.statementTable[name];
			if (orderedCodeStatementCollection == null)
			{
				orderedCodeStatementCollection = new CodeDomSerializerBase.OrderedCodeStatementCollection();
				orderedCodeStatementCollection.Order = this.statementTable.Count;
				orderedCodeStatementCollection.Name = name;
				this.statementTable[name] = orderedCodeStatementCollection;
			}
			orderedCodeStatementCollection.Add(statement);
		}

		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			if (manager == null || codeObject == null)
			{
				throw new ArgumentNullException((manager == null) ? "manager" : "codeObject");
			}
			object obj = null;
			using (CodeDomSerializerBase.TraceScope("RootCodeDomSerializer::Deserialize"))
			{
				if (!(codeObject is CodeTypeDeclaration))
				{
					throw new ArgumentException(SR.GetString("SerializerBadElementType", new object[] { typeof(CodeTypeDeclaration).FullName }));
				}
				bool flag = false;
				CodeDomProvider codeDomProvider = manager.GetService(typeof(CodeDomProvider)) as CodeDomProvider;
				if (codeDomProvider != null)
				{
					flag = (codeDomProvider.LanguageOptions & LanguageOptions.CaseInsensitive) != LanguageOptions.None;
				}
				CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)codeObject;
				CodeTypeReference codeTypeReference = null;
				Type type = null;
				foreach (object obj2 in codeTypeDeclaration.BaseTypes)
				{
					CodeTypeReference codeTypeReference2 = (CodeTypeReference)obj2;
					Type type2 = manager.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeTypeReference2));
					if (type2 != null && !type2.IsInterface)
					{
						codeTypeReference = codeTypeReference2;
						type = type2;
						break;
					}
				}
				if (type == null)
				{
					throw new SerializationException(SR.GetString("SerializerTypeNotFound", new object[] { codeTypeReference.BaseType }))
					{
						HelpLink = "SerializerTypeNotFound"
					};
				}
				if (type.IsAbstract)
				{
					throw new SerializationException(SR.GetString("SerializerTypeAbstract", new object[] { type.FullName }))
					{
						HelpLink = "SerializerTypeAbstract"
					};
				}
				ResolveNameEventHandler resolveNameEventHandler = new ResolveNameEventHandler(this.OnResolveName);
				manager.ResolveName += resolveNameEventHandler;
				if (!(manager is DesignerSerializationManager))
				{
					manager.AddSerializationProvider(new CodeDomSerializationProvider());
				}
				obj = manager.CreateInstance(type, null, codeTypeDeclaration.Name, true);
				this.nameTable = new HybridDictionary(codeTypeDeclaration.Members.Count, flag);
				this.statementTable = new HybridDictionary(codeTypeDeclaration.Members.Count, flag);
				this.initMethod = null;
				RootContext rootContext = new RootContext(new CodeThisReferenceExpression(), obj);
				manager.Context.Push(rootContext);
				try
				{
					foreach (object obj3 in codeTypeDeclaration.Members)
					{
						CodeTypeMember codeTypeMember = (CodeTypeMember)obj3;
						if (codeTypeMember is CodeMemberField)
						{
							if (string.Compare(codeTypeMember.Name, codeTypeDeclaration.Name, flag, CultureInfo.InvariantCulture) != 0)
							{
								this.nameTable[codeTypeMember.Name] = codeTypeMember;
							}
						}
						else if (this.initMethod == null && codeTypeMember is CodeMemberMethod)
						{
							CodeMemberMethod codeMemberMethod = (CodeMemberMethod)codeTypeMember;
							if (string.Compare(codeMemberMethod.Name, this.InitMethodName, flag, CultureInfo.InvariantCulture) == 0 && codeMemberMethod.Parameters.Count == 0)
							{
								this.initMethod = codeMemberMethod;
							}
						}
					}
					if (this.initMethod != null)
					{
						foreach (object obj4 in this.initMethod.Statements)
						{
							CodeStatement codeStatement = (CodeStatement)obj4;
							CodeVariableDeclarationStatement codeVariableDeclarationStatement = codeStatement as CodeVariableDeclarationStatement;
							if (codeVariableDeclarationStatement != null)
							{
								this.nameTable[codeVariableDeclarationStatement.Name] = codeStatement;
							}
						}
					}
					if (this.nameTable[codeTypeDeclaration.Name] != null)
					{
						this.nameTable[codeTypeDeclaration.Name] = obj;
					}
					if (this.initMethod != null)
					{
						this.FillStatementTable(this.initMethod, codeTypeDeclaration.Name);
					}
					PropertyDescriptor propertyDescriptor = manager.Properties["SupportsStatementGeneration"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && (bool)propertyDescriptor.GetValue(manager))
					{
						foreach (object obj5 in this.nameTable.Keys)
						{
							string text = (string)obj5;
							CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection = (CodeDomSerializerBase.OrderedCodeStatementCollection)this.statementTable[text];
							if (orderedCodeStatementCollection != null)
							{
								bool flag2 = false;
								foreach (object obj6 in orderedCodeStatementCollection)
								{
									CodeStatement codeStatement2 = (CodeStatement)obj6;
									object obj7 = codeStatement2.UserData["GeneratedStatement"];
									if (obj7 == null || !(obj7 is bool) || !(bool)obj7)
									{
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									this.statementTable.Remove(text);
								}
							}
						}
					}
					IContainer container = (IContainer)manager.GetService(typeof(IContainer));
					if (container != null)
					{
						foreach (object obj8 in container.Components)
						{
							base.DeserializePropertiesFromResources(manager, obj8, RootCodeDomSerializer.designTimeProperties);
						}
					}
					object[] array = new object[this.statementTable.Values.Count];
					this.statementTable.Values.CopyTo(array, 0);
					Array.Sort(array, RootCodeDomSerializer.StatementOrderComparer.Default);
					foreach (CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection2 in array)
					{
						string name = orderedCodeStatementCollection2.Name;
						if (name != null && !name.Equals(codeTypeDeclaration.Name))
						{
							this.DeserializeName(manager, name);
						}
					}
					CodeStatementCollection codeStatementCollection = (CodeStatementCollection)this.statementTable[codeTypeDeclaration.Name];
					if (codeStatementCollection != null && codeStatementCollection.Count > 0)
					{
						foreach (object obj9 in codeStatementCollection)
						{
							CodeStatement codeStatement3 = (CodeStatement)obj9;
							base.DeserializeStatement(manager, codeStatement3);
						}
					}
				}
				finally
				{
					manager.ResolveName -= resolveNameEventHandler;
					this.initMethod = null;
					this.nameTable = null;
					this.statementTable = null;
					manager.Context.Pop();
				}
			}
			return obj;
		}

		private object DeserializeName(IDesignerSerializationManager manager, string name)
		{
			string text = null;
			object obj = this.nameTable[name];
			using (CodeDomSerializerBase.TraceScope("RootCodeDomSerializer::DeserializeName"))
			{
				CodeMemberField codeMemberField = null;
				CodeObject codeObject = obj as CodeObject;
				if (codeObject != null)
				{
					obj = null;
					this.nameTable[name] = null;
					if (codeObject is CodeVariableDeclarationStatement)
					{
						CodeVariableDeclarationStatement codeVariableDeclarationStatement = (CodeVariableDeclarationStatement)codeObject;
						text = CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeVariableDeclarationStatement.Type);
					}
					else if (codeObject is CodeMemberField)
					{
						codeMemberField = (CodeMemberField)codeObject;
						text = CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeMemberField.Type);
					}
				}
				else
				{
					if (obj != null)
					{
						return obj;
					}
					IContainer container = (IContainer)manager.GetService(typeof(IContainer));
					if (container != null)
					{
						IComponent component = container.Components[name];
						if (component != null)
						{
							text = component.GetType().FullName;
							this.nameTable[name] = component;
						}
					}
				}
				if (name.Equals(this.ContainerName))
				{
					IContainer container2 = (IContainer)manager.GetService(typeof(IContainer));
					if (container2 != null)
					{
						obj = container2;
					}
				}
				else if (text != null)
				{
					Type type = manager.GetType(text);
					if (type == null)
					{
						manager.ReportError(new SerializationException(SR.GetString("SerializerTypeNotFound", new object[] { text })));
					}
					else
					{
						CodeStatementCollection codeStatementCollection = (CodeStatementCollection)this.statementTable[name];
						if (codeStatementCollection != null && codeStatementCollection.Count > 0)
						{
							CodeDomSerializer codeDomSerializer = (CodeDomSerializer)manager.GetSerializer(type, typeof(CodeDomSerializer));
							if (codeDomSerializer == null)
							{
								manager.ReportError(SR.GetString("SerializerNoSerializerForComponent", new object[] { type.FullName }));
							}
							else
							{
								try
								{
									obj = codeDomSerializer.Deserialize(manager, codeStatementCollection);
									if (obj != null && codeMemberField != null)
									{
										PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj)["Modifiers"];
										if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(MemberAttributes))
										{
											MemberAttributes memberAttributes = codeMemberField.Attributes & MemberAttributes.AccessMask;
											propertyDescriptor.SetValue(obj, memberAttributes);
										}
									}
								}
								catch (Exception ex)
								{
									manager.ReportError(ex);
								}
							}
						}
					}
				}
				this.nameTable[name] = obj;
			}
			return obj;
		}

		private void FillStatementTable(CodeMemberMethod method, string className)
		{
			using (CodeDomSerializerBase.TraceScope("RootCodeDomSerializer::FillStatementTable"))
			{
				foreach (object obj in method.Statements)
				{
					CodeStatement codeStatement = (CodeStatement)obj;
					CodeExpression codeExpression = null;
					if (codeStatement is CodeAssignStatement)
					{
						codeExpression = ((CodeAssignStatement)codeStatement).Left;
					}
					else if (codeStatement is CodeAttachEventStatement)
					{
						codeExpression = ((CodeAttachEventStatement)codeStatement).Event;
					}
					else if (codeStatement is CodeRemoveEventStatement)
					{
						codeExpression = ((CodeRemoveEventStatement)codeStatement).Event;
					}
					else if (codeStatement is CodeExpressionStatement)
					{
						codeExpression = ((CodeExpressionStatement)codeStatement).Expression;
					}
					else if (codeStatement is CodeVariableDeclarationStatement)
					{
						CodeVariableDeclarationStatement codeVariableDeclarationStatement = (CodeVariableDeclarationStatement)codeStatement;
						if (codeVariableDeclarationStatement.InitExpression != null && this.nameTable.Contains(codeVariableDeclarationStatement.Name))
						{
							this.AddStatement(codeVariableDeclarationStatement.Name, codeVariableDeclarationStatement);
						}
						codeExpression = null;
					}
					if (codeExpression != null)
					{
						CodeFieldReferenceExpression codeFieldReferenceExpression;
						CodePropertyReferenceExpression codePropertyReferenceExpression;
						for (;;)
						{
							if (codeExpression is CodeCastExpression)
							{
								codeExpression = ((CodeCastExpression)codeExpression).Expression;
							}
							else if (codeExpression is CodeDelegateCreateExpression)
							{
								codeExpression = ((CodeDelegateCreateExpression)codeExpression).TargetObject;
							}
							else if (codeExpression is CodeDelegateInvokeExpression)
							{
								codeExpression = ((CodeDelegateInvokeExpression)codeExpression).TargetObject;
							}
							else if (codeExpression is CodeDirectionExpression)
							{
								codeExpression = ((CodeDirectionExpression)codeExpression).Expression;
							}
							else if (codeExpression is CodeEventReferenceExpression)
							{
								codeExpression = ((CodeEventReferenceExpression)codeExpression).TargetObject;
							}
							else if (codeExpression is CodeMethodInvokeExpression)
							{
								codeExpression = ((CodeMethodInvokeExpression)codeExpression).Method;
							}
							else if (codeExpression is CodeMethodReferenceExpression)
							{
								codeExpression = ((CodeMethodReferenceExpression)codeExpression).TargetObject;
							}
							else if (codeExpression is CodeArrayIndexerExpression)
							{
								codeExpression = ((CodeArrayIndexerExpression)codeExpression).TargetObject;
							}
							else if (codeExpression is CodeFieldReferenceExpression)
							{
								codeFieldReferenceExpression = (CodeFieldReferenceExpression)codeExpression;
								if (codeFieldReferenceExpression.TargetObject is CodeThisReferenceExpression)
								{
									break;
								}
								codeExpression = codeFieldReferenceExpression.TargetObject;
							}
							else
							{
								if (!(codeExpression is CodePropertyReferenceExpression))
								{
									goto IL_0206;
								}
								codePropertyReferenceExpression = (CodePropertyReferenceExpression)codeExpression;
								if (codePropertyReferenceExpression.TargetObject is CodeThisReferenceExpression && this.nameTable.Contains(codePropertyReferenceExpression.PropertyName))
								{
									goto Block_24;
								}
								codeExpression = codePropertyReferenceExpression.TargetObject;
							}
						}
						this.AddStatement(codeFieldReferenceExpression.FieldName, codeStatement);
						continue;
						Block_24:
						this.AddStatement(codePropertyReferenceExpression.PropertyName, codeStatement);
						continue;
						IL_0206:
						if (codeExpression is CodeVariableReferenceExpression)
						{
							CodeVariableReferenceExpression codeVariableReferenceExpression = (CodeVariableReferenceExpression)codeExpression;
							if (this.nameTable.Contains(codeVariableReferenceExpression.VariableName))
							{
								this.AddStatement(codeVariableReferenceExpression.VariableName, codeStatement);
							}
						}
						else if (codeExpression is CodeThisReferenceExpression || codeExpression is CodeBaseReferenceExpression)
						{
							this.AddStatement(className, codeStatement);
						}
					}
				}
			}
		}

		private string GetMethodName(object statement)
		{
			string text = null;
			while (text == null)
			{
				if (statement is CodeExpressionStatement)
				{
					statement = ((CodeExpressionStatement)statement).Expression;
				}
				else if (statement is CodeMethodInvokeExpression)
				{
					statement = ((CodeMethodInvokeExpression)statement).Method;
				}
				else
				{
					if (statement is CodeMethodReferenceExpression)
					{
						return ((CodeMethodReferenceExpression)statement).MethodName;
					}
					break;
				}
			}
			return text;
		}

		private void OnResolveName(object sender, ResolveNameEventArgs e)
		{
			using (CodeDomSerializerBase.TraceScope("RootCodeDomSerializer::OnResolveName"))
			{
				if (e.Value == null)
				{
					IDesignerSerializationManager designerSerializationManager = (IDesignerSerializationManager)sender;
					object obj = this.DeserializeName(designerSerializationManager, e.Name);
					e.Value = obj;
				}
			}
		}

		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			if (manager == null || value == null)
			{
				throw new ArgumentNullException((manager == null) ? "manager" : "value");
			}
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(manager.GetName(value));
			RootContext rootContext = new RootContext(new CodeThisReferenceExpression(), value);
			using (CodeDomSerializerBase.TraceScope("RootCodeDomSerializer::Serialize"))
			{
				codeTypeDeclaration.BaseTypes.Add(value.GetType());
				this.containerRequired = false;
				manager.Context.Push(rootContext);
				manager.Context.Push(this);
				manager.Context.Push(codeTypeDeclaration);
				if (!(manager is DesignerSerializationManager))
				{
					manager.AddSerializationProvider(new CodeDomSerializationProvider());
				}
				try
				{
					if (value is IComponent)
					{
						ISite site = ((IComponent)value).Site;
						if (site != null)
						{
							ICollection components = site.Container.Components;
							StatementContext statementContext = new StatementContext();
							statementContext.StatementCollection.Populate(components);
							manager.Context.Push(statementContext);
							try
							{
								foreach (object obj in components)
								{
									IComponent component = (IComponent)obj;
									if (component != value && !base.IsSerialized(manager, component))
									{
										CodeDomSerializer serializer = base.GetSerializer(manager, component);
										if (serializer != null)
										{
											base.SerializeToExpression(manager, component);
										}
										else
										{
											manager.ReportError(SR.GetString("SerializerNoSerializerForComponent", new object[] { component.GetType().FullName }));
										}
									}
								}
								manager.Context.Push(value);
								try
								{
									CodeDomSerializer serializer2 = base.GetSerializer(manager, value);
									if (serializer2 != null && !base.IsSerialized(manager, value))
									{
										base.SerializeToExpression(manager, value);
									}
									else
									{
										manager.ReportError(SR.GetString("SerializerNoSerializerForComponent", new object[] { value.GetType().FullName }));
									}
								}
								finally
								{
									manager.Context.Pop();
								}
							}
							finally
							{
								manager.Context.Pop();
							}
							CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
							codeMemberMethod.Name = this.InitMethodName;
							codeMemberMethod.Attributes = MemberAttributes.Private;
							codeTypeDeclaration.Members.Add(codeMemberMethod);
							ArrayList arrayList = new ArrayList();
							foreach (object obj2 in components)
							{
								if (obj2 != value)
								{
									arrayList.Add(statementContext.StatementCollection[obj2]);
								}
							}
							CodeStatementCollection codeStatementCollection = statementContext.StatementCollection[value];
							if (codeStatementCollection != null)
							{
								arrayList.Add(statementContext.StatementCollection[value]);
							}
							if (this.ContainerRequired)
							{
								this.SerializeContainerDeclaration(manager, codeMemberMethod.Statements);
							}
							this.SerializeElementsToStatements(arrayList, codeMemberMethod.Statements);
						}
					}
				}
				finally
				{
					manager.Context.Pop();
					manager.Context.Pop();
					manager.Context.Pop();
				}
			}
			return codeTypeDeclaration;
		}

		private void SerializeContainerDeclaration(IDesignerSerializationManager manager, CodeStatementCollection statements)
		{
			CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)manager.Context[typeof(CodeTypeDeclaration)];
			if (codeTypeDeclaration == null)
			{
				return;
			}
			Type type = typeof(IContainer);
			CodeTypeReference codeTypeReference = new CodeTypeReference(type);
			CodeMemberField codeMemberField = new CodeMemberField(codeTypeReference, this.ContainerName);
			codeMemberField.Attributes = MemberAttributes.Private;
			codeTypeDeclaration.Members.Add(codeMemberField);
			type = typeof(Container);
			codeTypeReference = new CodeTypeReference(type);
			CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
			codeObjectCreateExpression.CreateType = codeTypeReference;
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.ContainerName);
			CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeFieldReferenceExpression, codeObjectCreateExpression);
			statements.Add(codeAssignStatement);
		}

		private void SerializeElementsToStatements(ArrayList elements, CodeStatementCollection statements)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			ArrayList arrayList5 = new ArrayList();
			foreach (object obj in elements)
			{
				if (obj is CodeAssignStatement && ((CodeAssignStatement)obj).Left is CodeFieldReferenceExpression)
				{
					arrayList4.Add(obj);
				}
				else if (obj is CodeVariableDeclarationStatement)
				{
					arrayList3.Add(obj);
				}
				else if (obj is CodeStatement)
				{
					string text = ((CodeObject)obj).UserData["statement-ordering"] as string;
					if (text != null)
					{
						string text2;
						if ((text2 = text) != null)
						{
							if (text2 == "begin")
							{
								arrayList.Add(obj);
								continue;
							}
							if (text2 == "end")
							{
								arrayList2.Add(obj);
								continue;
							}
							if (!(text2 == "default"))
							{
							}
						}
						arrayList5.Add(obj);
					}
					else
					{
						arrayList5.Add(obj);
					}
				}
				else if (obj is CodeStatementCollection)
				{
					CodeStatementCollection codeStatementCollection = (CodeStatementCollection)obj;
					foreach (object obj2 in codeStatementCollection)
					{
						CodeStatement codeStatement = (CodeStatement)obj2;
						if (codeStatement is CodeAssignStatement && ((CodeAssignStatement)codeStatement).Left is CodeFieldReferenceExpression)
						{
							arrayList4.Add(codeStatement);
						}
						else if (codeStatement is CodeVariableDeclarationStatement)
						{
							arrayList3.Add(codeStatement);
						}
						else
						{
							string text3 = codeStatement.UserData["statement-ordering"] as string;
							if (text3 != null)
							{
								string text4;
								if ((text4 = text3) != null)
								{
									if (text4 == "begin")
									{
										arrayList.Add(codeStatement);
										continue;
									}
									if (text4 == "end")
									{
										arrayList2.Add(codeStatement);
										continue;
									}
									if (!(text4 == "default"))
									{
									}
								}
								arrayList5.Add(codeStatement);
							}
							else
							{
								arrayList5.Add(codeStatement);
							}
						}
					}
				}
			}
			statements.AddRange((CodeStatement[])arrayList3.ToArray(typeof(CodeStatement)));
			statements.AddRange((CodeStatement[])arrayList4.ToArray(typeof(CodeStatement)));
			statements.AddRange((CodeStatement[])arrayList.ToArray(typeof(CodeStatement)));
			statements.AddRange((CodeStatement[])arrayList5.ToArray(typeof(CodeStatement)));
			statements.AddRange((CodeStatement[])arrayList2.ToArray(typeof(CodeStatement)));
		}

		private CodeStatementCollection SerializeRootObject(IDesignerSerializationManager manager, object value, bool designTime)
		{
			if ((CodeTypeDeclaration)manager.Context[typeof(CodeTypeDeclaration)] == null)
			{
				return null;
			}
			CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
			using (CodeDomSerializerBase.TraceScope("RootCodeDomSerializer::SerializeRootObject"))
			{
				if (designTime)
				{
					base.SerializeProperties(manager, codeStatementCollection, value, RootCodeDomSerializer.designTimeProperties);
				}
				else
				{
					base.SerializeProperties(manager, codeStatementCollection, value, RootCodeDomSerializer.runTimeProperties);
					base.SerializeEvents(manager, codeStatementCollection, value, null);
				}
			}
			return codeStatementCollection;
		}

		private IDictionary nameTable;

		private IDictionary statementTable;

		private CodeMemberMethod initMethod;

		private bool containerRequired;

		private static readonly Attribute[] designTimeProperties = new Attribute[] { DesignOnlyAttribute.Yes };

		private static readonly Attribute[] runTimeProperties = new Attribute[] { DesignOnlyAttribute.No };

		private class StatementOrderComparer : IComparer
		{
			private StatementOrderComparer()
			{
			}

			public int Compare(object left, object right)
			{
				CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection = left as CodeDomSerializerBase.OrderedCodeStatementCollection;
				CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection2 = right as CodeDomSerializerBase.OrderedCodeStatementCollection;
				if (left == null)
				{
					return 1;
				}
				if (right == null)
				{
					return -1;
				}
				if (right == left)
				{
					return 0;
				}
				return orderedCodeStatementCollection.Order - orderedCodeStatementCollection2.Order;
			}

			public static readonly RootCodeDomSerializer.StatementOrderComparer Default = new RootCodeDomSerializer.StatementOrderComparer();
		}

		private class ComponentComparer : IComparer
		{
			private ComponentComparer()
			{
			}

			public int Compare(object left, object right)
			{
				int num = string.Compare(((IComponent)left).GetType().Name, ((IComponent)right).GetType().Name, false, CultureInfo.InvariantCulture);
				if (num == 0)
				{
					num = string.Compare(((IComponent)left).Site.Name, ((IComponent)right).Site.Name, true, CultureInfo.InvariantCulture);
				}
				return num;
			}

			public static readonly RootCodeDomSerializer.ComponentComparer Default = new RootCodeDomSerializer.ComponentComparer();
		}
	}
}
