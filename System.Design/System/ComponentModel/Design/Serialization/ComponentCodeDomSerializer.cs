using System;
using System.CodeDom;
using System.Configuration;
using System.Reflection;

namespace System.ComponentModel.Design.Serialization
{
	internal class ComponentCodeDomSerializer : CodeDomSerializer
	{
		internal new static ComponentCodeDomSerializer Default
		{
			get
			{
				if (ComponentCodeDomSerializer._default == null)
				{
					ComponentCodeDomSerializer._default = new ComponentCodeDomSerializer();
				}
				return ComponentCodeDomSerializer._default;
			}
		}

		private bool CanCacheComponent(IDesignerSerializationManager manager, object value, PropertyDescriptorCollection props)
		{
			IComponent component = value as IComponent;
			if (component != null)
			{
				if (component.Site != null)
				{
					INestedSite nestedSite = component.Site as INestedSite;
					if (nestedSite != null && !string.IsNullOrEmpty(nestedSite.FullName))
					{
						return false;
					}
				}
				if (props == null)
				{
					props = TypeDescriptor.GetProperties(component);
				}
				foreach (object obj in props)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					if (typeof(IComponent).IsAssignableFrom(propertyDescriptor.PropertyType) && !propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden))
					{
						MemberCodeDomSerializer memberCodeDomSerializer = (MemberCodeDomSerializer)manager.GetSerializer(propertyDescriptor.GetType(), typeof(MemberCodeDomSerializer));
						if (memberCodeDomSerializer != null && memberCodeDomSerializer.ShouldSerialize(manager, value, propertyDescriptor))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		protected override object DeserializeInstance(IDesignerSerializationManager manager, Type type, object[] parameters, string name, bool addToContainer)
		{
			object obj = base.DeserializeInstance(manager, type, parameters, name, addToContainer);
			if (obj != null)
			{
				base.DeserializePropertiesFromResources(manager, obj, ComponentCodeDomSerializer._designTimeFilter);
			}
			return obj;
		}

		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			CodeStatementCollection codeStatementCollection = null;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
			using (CodeDomSerializerBase.TraceScope("ComponentCodeDomSerializer::Serialize"))
			{
				if (manager == null || value == null)
				{
					throw new ArgumentNullException((manager == null) ? "manager" : "value");
				}
				if (base.IsSerialized(manager, value))
				{
					return base.GetExpression(manager, value);
				}
				InheritanceLevel inheritanceLevel = InheritanceLevel.NotInherited;
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(value)[typeof(InheritanceAttribute)];
				if (inheritanceAttribute != null)
				{
					inheritanceLevel = inheritanceAttribute.InheritanceLevel;
				}
				if (inheritanceLevel != InheritanceLevel.InheritedReadOnly)
				{
					codeStatementCollection = new CodeStatementCollection();
					CodeTypeDeclaration codeTypeDeclaration = manager.Context[typeof(CodeTypeDeclaration)] as CodeTypeDeclaration;
					RootContext rootContext = manager.Context[typeof(RootContext)] as RootContext;
					CodeExpression codeExpression = null;
					bool flag = false;
					bool flag2 = true;
					bool flag3 = true;
					bool flag4 = false;
					codeExpression = base.GetExpression(manager, value);
					if (codeExpression != null)
					{
						flag = false;
						flag2 = false;
						flag3 = false;
						IComponent component = value as IComponent;
						if (component != null && component.Site == null)
						{
							ExpressionContext expressionContext = manager.Context[typeof(ExpressionContext)] as ExpressionContext;
							if (expressionContext == null || expressionContext.PresetValue != value)
							{
								flag4 = true;
							}
						}
					}
					else
					{
						if (inheritanceLevel == InheritanceLevel.NotInherited)
						{
							PropertyDescriptor propertyDescriptor = properties["GenerateMember"];
							if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && !(bool)propertyDescriptor.GetValue(value))
							{
								flag = true;
								flag2 = false;
							}
						}
						else
						{
							flag3 = false;
						}
						if (rootContext == null)
						{
							flag = true;
							flag2 = false;
						}
					}
					manager.Context.Push(value);
					manager.Context.Push(codeStatementCollection);
					try
					{
						string name = manager.GetName(value);
						TypeDescriptor.GetReflectionType(value);
						string className = TypeDescriptor.GetClassName(value);
						if ((flag2 || flag) && name != null)
						{
							if (flag2)
							{
								if (inheritanceLevel == InheritanceLevel.NotInherited)
								{
									CodeMemberField codeMemberField = new CodeMemberField(className, name);
									PropertyDescriptor propertyDescriptor2 = properties["Modifiers"];
									if (propertyDescriptor2 == null)
									{
										propertyDescriptor2 = properties["DefaultModifiers"];
									}
									MemberAttributes memberAttributes;
									if (propertyDescriptor2 != null && propertyDescriptor2.PropertyType == typeof(MemberAttributes))
									{
										memberAttributes = (MemberAttributes)propertyDescriptor2.GetValue(value);
									}
									else
									{
										memberAttributes = MemberAttributes.Private;
									}
									codeMemberField.Attributes = memberAttributes;
									codeTypeDeclaration.Members.Add(codeMemberField);
								}
								codeExpression = new CodeFieldReferenceExpression(rootContext.Expression, name);
							}
							else
							{
								if (inheritanceLevel == InheritanceLevel.NotInherited)
								{
									CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(className, name);
									codeStatementCollection.Add(codeVariableDeclarationStatement);
								}
								codeExpression = new CodeVariableReferenceExpression(name);
							}
						}
						if (flag3)
						{
							IContainer container = manager.GetService(typeof(IContainer)) as IContainer;
							ConstructorInfo constructor = TypeDescriptor.GetReflectionType(value).GetConstructor(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, ComponentCodeDomSerializer._containerConstructor, null);
							CodeExpression codeExpression2;
							if (constructor != null && container != null)
							{
								codeExpression2 = new CodeObjectCreateExpression(className, new CodeExpression[] { base.SerializeToExpression(manager, container) });
							}
							else
							{
								bool flag5;
								codeExpression2 = base.SerializeCreationExpression(manager, value, out flag5);
							}
							if (codeExpression2 != null)
							{
								if (codeExpression == null)
								{
									if (flag4)
									{
										codeExpression = codeExpression2;
									}
								}
								else
								{
									CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeExpression, codeExpression2);
									codeStatementCollection.Add(codeAssignStatement);
								}
							}
						}
						if (codeExpression != null)
						{
							base.SetExpression(manager, value, codeExpression);
						}
						if (codeExpression != null && !flag4)
						{
							bool flag6 = value is ISupportInitialize && manager.GetType(typeof(ISupportInitialize).FullName) != null;
							bool flag7 = value is IPersistComponentSettings && ((IPersistComponentSettings)value).SaveSettings && manager.GetType(typeof(IPersistComponentSettings).FullName) != null;
							IDesignerSerializationManager designerSerializationManager = (IDesignerSerializationManager)manager.GetService(typeof(IDesignerSerializationManager));
							if (flag6)
							{
								this.SerializeSupportInitialize(manager, codeStatementCollection, codeExpression, value, "BeginInit");
							}
							base.SerializePropertiesToResources(manager, codeStatementCollection, value, ComponentCodeDomSerializer._designTimeFilter);
							ComponentCache componentCache = (ComponentCache)manager.GetService(typeof(ComponentCache));
							ComponentCache.Entry entry = null;
							if (componentCache == null)
							{
								IServiceContainer serviceContainer = (IServiceContainer)manager.GetService(typeof(IServiceContainer));
								if (serviceContainer != null)
								{
									componentCache = new ComponentCache(manager);
									serviceContainer.AddService(typeof(ComponentCache), componentCache);
								}
							}
							else if (manager == designerSerializationManager && componentCache != null && componentCache.Enabled)
							{
								entry = componentCache[value];
							}
							if (entry == null || entry.Tracking)
							{
								if (entry == null)
								{
									entry = new ComponentCache.Entry(componentCache);
									ComponentCache.Entry entryAll = componentCache.GetEntryAll(value);
									if (entryAll != null && entryAll.Dependencies != null && entryAll.Dependencies.Count > 0)
									{
										foreach (object obj in entryAll.Dependencies)
										{
											entry.AddDependency(obj);
										}
									}
								}
								entry.Component = value;
								bool flag8 = manager == designerSerializationManager;
								entry.Valid = flag8 && this.CanCacheComponent(manager, value, properties);
								if (flag8 && componentCache != null && componentCache.Enabled)
								{
									manager.Context.Push(componentCache);
									manager.Context.Push(entry);
								}
								try
								{
									entry.Statements = new CodeStatementCollection();
									base.SerializeProperties(manager, entry.Statements, value, ComponentCodeDomSerializer._runTimeFilter);
									base.SerializeEvents(manager, entry.Statements, value, null);
									foreach (object obj2 in entry.Statements)
									{
										CodeStatement codeStatement = (CodeStatement)obj2;
										CodeVariableDeclarationStatement codeVariableDeclarationStatement2 = codeStatement as CodeVariableDeclarationStatement;
										if (codeVariableDeclarationStatement2 != null)
										{
											entry.Tracking = true;
											break;
										}
									}
									if (entry.Statements.Count > 0)
									{
										entry.Statements.Insert(0, new CodeCommentStatement(string.Empty));
										entry.Statements.Insert(0, new CodeCommentStatement(name));
										entry.Statements.Insert(0, new CodeCommentStatement(string.Empty));
										if (flag8 && componentCache != null && componentCache.Enabled)
										{
											componentCache[value] = entry;
										}
									}
									goto IL_0634;
								}
								finally
								{
									if (flag8 && componentCache != null && componentCache.Enabled)
									{
										manager.Context.Pop();
										manager.Context.Pop();
									}
								}
							}
							if ((entry.Resources != null || entry.Metadata != null) && componentCache != null && componentCache.Enabled)
							{
								ResourceCodeDomSerializer @default = ResourceCodeDomSerializer.Default;
								@default.ApplyCacheEntry(manager, entry);
							}
							IL_0634:
							codeStatementCollection.AddRange(entry.Statements);
							if (flag7)
							{
								this.SerializeLoadComponentSettings(manager, codeStatementCollection, codeExpression, value);
							}
							if (flag6)
							{
								this.SerializeSupportInitialize(manager, codeStatementCollection, codeExpression, value, "EndInit");
							}
						}
					}
					catch (CheckoutException)
					{
						throw;
					}
					catch (Exception ex)
					{
						manager.ReportError(ex);
					}
					finally
					{
						manager.Context.Pop();
						manager.Context.Pop();
					}
				}
			}
			return codeStatementCollection;
		}

		private void SerializeLoadComponentSettings(IDesignerSerializationManager manager, CodeStatementCollection statements, CodeExpression valueExpression, object value)
		{
			CodeTypeReference codeTypeReference = new CodeTypeReference(typeof(IPersistComponentSettings));
			CodeCastExpression codeCastExpression = new CodeCastExpression(codeTypeReference, valueExpression);
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(codeCastExpression, "LoadComponentSettings");
			CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(new CodeMethodInvokeExpression
			{
				Method = codeMethodReferenceExpression
			});
			codeExpressionStatement.UserData["statement-ordering"] = "end";
			statements.Add(codeExpressionStatement);
		}

		private void SerializeSupportInitialize(IDesignerSerializationManager manager, CodeStatementCollection statements, CodeExpression valueExpression, object value, string methodName)
		{
			CodeTypeReference codeTypeReference = new CodeTypeReference(typeof(ISupportInitialize));
			CodeCastExpression codeCastExpression = new CodeCastExpression(codeTypeReference, valueExpression);
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(codeCastExpression, methodName);
			CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement(new CodeMethodInvokeExpression
			{
				Method = codeMethodReferenceExpression
			});
			if (methodName == "BeginInit")
			{
				codeExpressionStatement.UserData["statement-ordering"] = "begin";
			}
			else
			{
				codeExpressionStatement.UserData["statement-ordering"] = "end";
			}
			statements.Add(codeExpressionStatement);
		}

		private static readonly Type[] _containerConstructor = new Type[] { typeof(IContainer) };

		private static readonly Attribute[] _runTimeFilter = new Attribute[] { DesignOnlyAttribute.No };

		private static readonly Attribute[] _designTimeFilter = new Attribute[] { DesignOnlyAttribute.Yes };

		private static ComponentCodeDomSerializer _default;
	}
}
