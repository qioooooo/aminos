using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Design;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000157 RID: 343
	[DefaultSerializationProvider(typeof(CodeDomSerializationProvider))]
	public class TypeCodeDomSerializer : CodeDomSerializerBase
	{
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00032DDD File Offset: 0x00031DDD
		internal static TypeCodeDomSerializer Default
		{
			get
			{
				if (TypeCodeDomSerializer._default == null)
				{
					TypeCodeDomSerializer._default = new TypeCodeDomSerializer();
				}
				return TypeCodeDomSerializer._default;
			}
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00032DF8 File Offset: 0x00031DF8
		public virtual object Deserialize(IDesignerSerializationManager manager, CodeTypeDeclaration declaration)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (declaration == null)
			{
				throw new ArgumentNullException("declaration");
			}
			object obj = null;
			using (CodeDomSerializerBase.TraceScope("TypeCodeDomSerializer::Deserialize"))
			{
				bool flag = false;
				CodeDomProvider codeDomProvider = manager.GetService(typeof(CodeDomProvider)) as CodeDomProvider;
				if (codeDomProvider != null)
				{
					flag = (codeDomProvider.LanguageOptions & LanguageOptions.CaseInsensitive) != LanguageOptions.None;
				}
				Type type = null;
				string text = declaration.Name;
				foreach (object obj2 in declaration.BaseTypes)
				{
					CodeTypeReference codeTypeReference = (CodeTypeReference)obj2;
					Type type2 = manager.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeTypeReference));
					text = codeTypeReference.BaseType;
					if (type2 != null && !type2.IsInterface)
					{
						type = type2;
						break;
					}
				}
				if (type == null)
				{
					base.Error(manager, SR.GetString("SerializerTypeNotFound", new object[] { text }), "SerializerTypeNotFound");
				}
				if (TypeDescriptor.GetReflectionType(type).IsAbstract)
				{
					base.Error(manager, SR.GetString("SerializerTypeAbstract", new object[] { type.FullName }), "SerializerTypeAbstract");
				}
				ResolveNameEventHandler resolveNameEventHandler = new ResolveNameEventHandler(this.OnResolveName);
				manager.ResolveName += resolveNameEventHandler;
				obj = manager.CreateInstance(type, null, declaration.Name, true);
				this._nameTable = new HybridDictionary(declaration.Members.Count, flag);
				this._statementTable = new Dictionary<string, CodeDomSerializerBase.OrderedCodeStatementCollection>(declaration.Members.Count);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				RootContext rootContext = new RootContext(new CodeThisReferenceExpression(), obj);
				manager.Context.Push(rootContext);
				try
				{
					StringComparison stringComparison = (flag ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
					foreach (object obj3 in declaration.Members)
					{
						CodeTypeMember codeTypeMember = (CodeTypeMember)obj3;
						CodeMemberField codeMemberField = codeTypeMember as CodeMemberField;
						if (codeMemberField != null && !string.Equals(codeMemberField.Name, declaration.Name, stringComparison))
						{
							this._nameTable[codeMemberField.Name] = codeMemberField;
							if (codeMemberField.Type != null && !string.IsNullOrEmpty(codeMemberField.Type.BaseType))
							{
								dictionary[codeMemberField.Name] = CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeMemberField.Type);
							}
						}
					}
					CodeMemberMethod[] initializeMethods = this.GetInitializeMethods(manager, declaration);
					if (initializeMethods == null)
					{
						throw new InvalidOperationException();
					}
					foreach (CodeMemberMethod codeMemberMethod in initializeMethods)
					{
						foreach (object obj4 in codeMemberMethod.Statements)
						{
							CodeStatement codeStatement = (CodeStatement)obj4;
							CodeVariableDeclarationStatement codeVariableDeclarationStatement = codeStatement as CodeVariableDeclarationStatement;
							if (codeVariableDeclarationStatement != null)
							{
								this._nameTable[codeVariableDeclarationStatement.Name] = codeStatement;
							}
						}
					}
					this._nameTable[declaration.Name] = rootContext.Expression;
					foreach (CodeMemberMethod codeMemberMethod2 in initializeMethods)
					{
						CodeDomSerializerBase.FillStatementTable(manager, this._statementTable, dictionary, codeMemberMethod2.Statements, declaration.Name);
					}
					PropertyDescriptor propertyDescriptor = manager.Properties["SupportsStatementGeneration"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(bool) && (bool)propertyDescriptor.GetValue(manager))
					{
						foreach (object obj5 in this._nameTable.Keys)
						{
							string text2 = (string)obj5;
							if (this._statementTable.ContainsKey(text2))
							{
								CodeStatementCollection codeStatementCollection = this._statementTable[text2];
								bool flag2 = false;
								foreach (object obj6 in codeStatementCollection)
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
									this._statementTable.Remove(text2);
								}
							}
						}
					}
					base.DeserializePropertiesFromResources(manager, obj, TypeCodeDomSerializer._designTimeFilter);
					CodeDomSerializerBase.OrderedCodeStatementCollection[] array3 = new CodeDomSerializerBase.OrderedCodeStatementCollection[this._statementTable.Count];
					this._statementTable.Values.CopyTo(array3, 0);
					Array.Sort(array3, TypeCodeDomSerializer.StatementOrderComparer.Default);
					CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection = null;
					foreach (CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection2 in array3)
					{
						if (orderedCodeStatementCollection2.Name.Equals(declaration.Name))
						{
							orderedCodeStatementCollection = orderedCodeStatementCollection2;
						}
						else
						{
							this.DeserializeName(manager, orderedCodeStatementCollection2.Name, orderedCodeStatementCollection2);
						}
					}
					if (orderedCodeStatementCollection != null)
					{
						this.DeserializeName(manager, orderedCodeStatementCollection.Name, orderedCodeStatementCollection);
					}
				}
				finally
				{
					this._nameTable = null;
					this._statementTable = null;
					manager.ResolveName -= resolveNameEventHandler;
					manager.Context.Pop();
				}
			}
			return obj;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000333E0 File Offset: 0x000323E0
		private object DeserializeName(IDesignerSerializationManager manager, string name, CodeStatementCollection statements)
		{
			object obj = null;
			using (CodeDomSerializerBase.TraceScope("RootCodeDomSerializer::DeserializeName"))
			{
				obj = this._nameTable[name];
				CodeObject codeObject = obj as CodeObject;
				string text = null;
				CodeMemberField codeMemberField = null;
				if (codeObject != null)
				{
					obj = null;
					this._nameTable[name] = null;
					CodeVariableDeclarationStatement codeVariableDeclarationStatement = codeObject as CodeVariableDeclarationStatement;
					if (codeVariableDeclarationStatement != null)
					{
						text = CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeVariableDeclarationStatement.Type);
					}
					else
					{
						codeMemberField = codeObject as CodeMemberField;
						if (codeMemberField != null)
						{
							text = CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeMemberField.Type);
						}
						else
						{
							CodeExpression codeExpression = codeObject as CodeExpression;
							RootContext rootContext = manager.Context[typeof(RootContext)] as RootContext;
							if (rootContext != null && codeExpression != null && rootContext.Expression == codeExpression)
							{
								obj = rootContext.Value;
								text = TypeDescriptor.GetClassName(obj);
							}
						}
					}
				}
				else if (obj == null)
				{
					IContainer container = (IContainer)manager.GetService(typeof(IContainer));
					if (container != null)
					{
						IComponent component = container.Components[name];
						if (component != null)
						{
							text = component.GetType().FullName;
							this._nameTable[name] = component;
						}
					}
				}
				if (text != null)
				{
					Type type = manager.GetType(text);
					if (type == null)
					{
						manager.ReportError(new CodeDomSerializerException(SR.GetString("SerializerTypeNotFound", new object[] { text }), manager));
					}
					else
					{
						if (statements == null && this._statementTable.ContainsKey(name))
						{
							statements = this._statementTable[name];
						}
						if (statements != null && statements.Count > 0)
						{
							CodeDomSerializer serializer = base.GetSerializer(manager, type);
							if (serializer == null)
							{
								manager.ReportError(new CodeDomSerializerException(SR.GetString("SerializerNoSerializerForComponent", new object[] { type.FullName }), manager));
							}
							else
							{
								try
								{
									obj = serializer.Deserialize(manager, statements);
									if (obj != null && codeMemberField != null)
									{
										PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj)["Modifiers"];
										if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(MemberAttributes))
										{
											MemberAttributes memberAttributes = codeMemberField.Attributes & MemberAttributes.AccessMask;
											propertyDescriptor.SetValue(obj, memberAttributes);
										}
									}
									this._nameTable[name] = obj;
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
			return obj;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00033658 File Offset: 0x00032658
		protected virtual CodeMemberMethod GetInitializeMethod(IDesignerSerializationManager manager, CodeTypeDeclaration declaration, object value)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (declaration == null)
			{
				throw new ArgumentNullException("declaration");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			CodeConstructor codeConstructor = declaration.UserData[TypeCodeDomSerializer._initMethodKey] as CodeConstructor;
			if (codeConstructor == null)
			{
				codeConstructor = new CodeConstructor();
				declaration.UserData[TypeCodeDomSerializer._initMethodKey] = codeConstructor;
			}
			return codeConstructor;
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000336C0 File Offset: 0x000326C0
		protected virtual CodeMemberMethod[] GetInitializeMethods(IDesignerSerializationManager manager, CodeTypeDeclaration declaration)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (declaration == null)
			{
				throw new ArgumentNullException("declaration");
			}
			foreach (object obj in declaration.Members)
			{
				CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
				CodeConstructor codeConstructor = codeTypeMember as CodeConstructor;
				if (codeConstructor != null && codeConstructor.Parameters.Count == 0)
				{
					return new CodeMemberMethod[] { codeConstructor };
				}
			}
			return new CodeMemberMethod[0];
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00033764 File Offset: 0x00032764
		private void OnResolveName(object sender, ResolveNameEventArgs e)
		{
			using (CodeDomSerializerBase.TraceScope("RootCodeDomSerializer::OnResolveName"))
			{
				if (e.Value == null)
				{
					IDesignerSerializationManager designerSerializationManager = (IDesignerSerializationManager)sender;
					e.Value = this.DeserializeName(designerSerializationManager, e.Name, null);
				}
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x000337BC File Offset: 0x000327BC
		public virtual CodeTypeDeclaration Serialize(IDesignerSerializationManager manager, object root, ICollection members)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(manager.GetName(root));
			CodeThisReferenceExpression codeThisReferenceExpression = new CodeThisReferenceExpression();
			RootContext rootContext = new RootContext(codeThisReferenceExpression, root);
			StatementContext statementContext = new StatementContext();
			statementContext.StatementCollection.Populate(root);
			if (members != null)
			{
				statementContext.StatementCollection.Populate(members);
			}
			codeTypeDeclaration.BaseTypes.Add(root.GetType());
			manager.Context.Push(codeTypeDeclaration);
			manager.Context.Push(rootContext);
			manager.Context.Push(statementContext);
			try
			{
				if (members != null)
				{
					foreach (object obj in members)
					{
						if (obj != root)
						{
							base.SerializeToExpression(manager, obj);
						}
					}
				}
				base.SerializeToExpression(manager, root);
				this.IntegrateStatements(manager, root, members, statementContext, codeTypeDeclaration);
			}
			finally
			{
				manager.Context.Pop();
				manager.Context.Pop();
				manager.Context.Pop();
			}
			return codeTypeDeclaration;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000338F4 File Offset: 0x000328F4
		private void IntegrateStatements(IDesignerSerializationManager manager, object root, ICollection members, StatementContext statementCxt, CodeTypeDeclaration typeDecl)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			List<CodeMethodMap> list = new List<CodeMethodMap>();
			if (members != null)
			{
				foreach (object obj in members)
				{
					if (obj != root)
					{
						CodeStatementCollection codeStatementCollection = statementCxt.StatementCollection[obj];
						if (codeStatementCollection != null)
						{
							CodeMemberMethod initializeMethod = this.GetInitializeMethod(manager, typeDecl, obj);
							if (initializeMethod == null)
							{
								throw new InvalidOperationException();
							}
							int num;
							CodeMethodMap codeMethodMap;
							if (dictionary.TryGetValue(initializeMethod.Name, out num))
							{
								codeMethodMap = list[num];
							}
							else
							{
								codeMethodMap = new CodeMethodMap(initializeMethod);
								list.Add(codeMethodMap);
								dictionary[initializeMethod.Name] = list.Count - 1;
							}
							if (codeStatementCollection.Count > 0)
							{
								codeMethodMap.Add(codeStatementCollection);
							}
						}
					}
				}
			}
			CodeStatementCollection codeStatementCollection2 = statementCxt.StatementCollection[root];
			if (codeStatementCollection2 != null)
			{
				CodeMemberMethod initializeMethod2 = this.GetInitializeMethod(manager, typeDecl, root);
				if (initializeMethod2 == null)
				{
					throw new InvalidOperationException();
				}
				int num2;
				CodeMethodMap codeMethodMap2;
				if (dictionary.TryGetValue(initializeMethod2.Name, out num2))
				{
					codeMethodMap2 = list[num2];
				}
				else
				{
					codeMethodMap2 = new CodeMethodMap(initializeMethod2);
					list.Add(codeMethodMap2);
					dictionary[initializeMethod2.Name] = list.Count - 1;
				}
				if (codeStatementCollection2.Count > 0)
				{
					codeMethodMap2.Add(codeStatementCollection2);
				}
			}
			foreach (CodeMethodMap codeMethodMap3 in list)
			{
				codeMethodMap3.Combine();
				typeDecl.Members.Add(codeMethodMap3.Method);
			}
		}

		// Token: 0x04000EDF RID: 3807
		private IDictionary _nameTable;

		// Token: 0x04000EE0 RID: 3808
		private Dictionary<string, CodeDomSerializerBase.OrderedCodeStatementCollection> _statementTable;

		// Token: 0x04000EE1 RID: 3809
		private static readonly Attribute[] _designTimeFilter = new Attribute[] { DesignOnlyAttribute.Yes };

		// Token: 0x04000EE2 RID: 3810
		private static readonly Attribute[] _runTimeFilter = new Attribute[] { DesignOnlyAttribute.No };

		// Token: 0x04000EE3 RID: 3811
		private static object _initMethodKey = new object();

		// Token: 0x04000EE4 RID: 3812
		private static TypeCodeDomSerializer _default;

		// Token: 0x02000158 RID: 344
		private class StatementOrderComparer : IComparer
		{
			// Token: 0x06000D02 RID: 3330 RVA: 0x00033AF5 File Offset: 0x00032AF5
			private StatementOrderComparer()
			{
			}

			// Token: 0x06000D03 RID: 3331 RVA: 0x00033B00 File Offset: 0x00032B00
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

			// Token: 0x04000EE5 RID: 3813
			public static readonly TypeCodeDomSerializer.StatementOrderComparer Default = new TypeCodeDomSerializer.StatementOrderComparer();
		}
	}
}
