using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Design;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using Microsoft.Internal.Performance;

namespace System.ComponentModel.Design.Serialization
{
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class CodeDomDesignerLoader : BasicDesignerLoader, INameCreationService, IDesignerSerializationService
	{
		protected abstract CodeDomProvider CodeDomProvider { get; }

		protected abstract ITypeResolutionService TypeResolutionService { get; }

		private void ClearDocument()
		{
			if (this._documentType != null)
			{
				base.LoaderHost.RemoveService(typeof(CodeTypeDeclaration));
				this._documentType = null;
				this._documentNamespace = null;
				this._documentCompileUnit = null;
				this._rootSerializer = null;
				this._typeSerializer = null;
			}
		}

		public override void Dispose()
		{
			IDesignerHost designerHost = base.GetService(typeof(IDesignerHost)) as IDesignerHost;
			IComponentChangeService componentChangeService = base.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
				componentChangeService.ComponentRename -= this.OnComponentRename;
			}
			if (designerHost != null)
			{
				designerHost.RemoveService(typeof(INameCreationService));
				designerHost.RemoveService(typeof(IDesignerSerializationService));
				designerHost.RemoveService(typeof(ComponentSerializationService));
				if (this._state[CodeDomDesignerLoader.StateOwnTypeResolution])
				{
					designerHost.RemoveService(typeof(ITypeResolutionService));
					this._state[CodeDomDesignerLoader.StateOwnTypeResolution] = false;
				}
			}
			if (this._extenderProviderService != null)
			{
				foreach (IExtenderProvider extenderProvider in this._extenderProviders)
				{
					this._extenderProviderService.RemoveExtenderProvider(extenderProvider);
				}
			}
			base.Dispose();
		}

		private bool HasRootDesignerAttribute(Type t)
		{
			AttributeCollection attributes = TypeDescriptor.GetAttributes(t);
			for (int i = 0; i < attributes.Count; i++)
			{
				DesignerAttribute designerAttribute = attributes[i] as DesignerAttribute;
				if (designerAttribute != null)
				{
					Type type = Type.GetType(designerAttribute.DesignerBaseTypeName);
					if (type != null && type == typeof(IRootDesigner))
					{
						return true;
					}
				}
			}
			return false;
		}

		private void EnsureDocument(IDesignerSerializationManager manager)
		{
			if (this._documentCompileUnit == null)
			{
				this._documentCompileUnit = this.Parse();
				if (this._documentCompileUnit == null)
				{
					throw new NotSupportedException(SR.GetString("CodeDomDesignerLoaderNoLanguageSupport"))
					{
						HelpLink = "CodeDomDesignerLoaderNoLanguageSupport"
					};
				}
			}
			if (this._documentType == null)
			{
				ArrayList arrayList = null;
				bool flag = true;
				if (this._documentCompileUnit.UserData[typeof(InvalidOperationException)] != null)
				{
					InvalidOperationException ex = this._documentCompileUnit.UserData[typeof(InvalidOperationException)] as InvalidOperationException;
					if (ex != null)
					{
						this._documentCompileUnit = null;
						throw ex;
					}
				}
				foreach (object obj in this._documentCompileUnit.Namespaces)
				{
					CodeNamespace codeNamespace = (CodeNamespace)obj;
					foreach (object obj2 in codeNamespace.Types)
					{
						CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj2;
						Type type = null;
						foreach (object obj3 in codeTypeDeclaration.BaseTypes)
						{
							CodeTypeReference codeTypeReference = (CodeTypeReference)obj3;
							Type type2 = base.LoaderHost.GetType(CodeDomSerializerBase.GetTypeNameFromCodeTypeReference(manager, codeTypeReference));
							if (type2 != null && !type2.IsInterface)
							{
								type = type2;
								break;
							}
							if (type2 == null)
							{
								if (arrayList == null)
								{
									arrayList = new ArrayList();
								}
								arrayList.Add(SR.GetString("CodeDomDesignerLoaderDocumentFailureTypeNotFound", new object[] { codeTypeDeclaration.Name, codeTypeReference.BaseType }));
							}
						}
						if (type != null)
						{
							bool flag2 = false;
							AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
							foreach (object obj4 in attributes)
							{
								Attribute attribute = (Attribute)obj4;
								if (attribute is RootDesignerSerializerAttribute)
								{
									RootDesignerSerializerAttribute rootDesignerSerializerAttribute = (RootDesignerSerializerAttribute)attribute;
									string serializerBaseTypeName = rootDesignerSerializerAttribute.SerializerBaseTypeName;
									if (serializerBaseTypeName != null && base.LoaderHost.GetType(serializerBaseTypeName) == typeof(CodeDomSerializer))
									{
										Type type3 = base.LoaderHost.GetType(rootDesignerSerializerAttribute.SerializerTypeName);
										if (type3 != null && type3 != typeof(RootCodeDomSerializer))
										{
											flag2 = true;
											if (flag)
											{
												this._rootSerializer = (CodeDomSerializer)Activator.CreateInstance(type3, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
												break;
											}
											throw new InvalidOperationException(SR.GetString("CodeDomDesignerLoaderSerializerTypeNotFirstType", new object[] { codeTypeDeclaration.Name }));
										}
									}
								}
							}
							if (this._rootSerializer == null && this.HasRootDesignerAttribute(type))
							{
								this._typeSerializer = manager.GetSerializer(type, typeof(TypeCodeDomSerializer)) as TypeCodeDomSerializer;
								if (!flag && this._typeSerializer != null)
								{
									this._typeSerializer = null;
									this._documentCompileUnit = null;
									throw new InvalidOperationException(SR.GetString("CodeDomDesignerLoaderSerializerTypeNotFirstType", new object[] { codeTypeDeclaration.Name }));
								}
							}
							if (this._rootSerializer == null && this._typeSerializer == null)
							{
								if (arrayList == null)
								{
									arrayList = new ArrayList();
								}
								if (flag2)
								{
									arrayList.Add(SR.GetString("CodeDomDesignerLoaderDocumentFailureTypeDesignerNotInstalled", new object[] { codeTypeDeclaration.Name, type.FullName }));
								}
								else
								{
									arrayList.Add(SR.GetString("CodeDomDesignerLoaderDocumentFailureTypeNotDesignable", new object[] { codeTypeDeclaration.Name, type.FullName }));
								}
							}
						}
						if (this._rootSerializer != null || this._typeSerializer != null)
						{
							this._documentNamespace = codeNamespace;
							this._documentType = codeTypeDeclaration;
							break;
						}
						flag = false;
					}
					if (this._documentType != null)
					{
						break;
					}
				}
				if (this._documentType == null)
				{
					this._documentCompileUnit = null;
					Exception ex2;
					if (arrayList != null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						foreach (object obj5 in arrayList)
						{
							string text = (string)obj5;
							stringBuilder.Append("\r\n");
							stringBuilder.Append(text);
						}
						ex2 = new InvalidOperationException(SR.GetString("CodeDomDesignerLoaderNoRootSerializerWithFailures", new object[] { stringBuilder.ToString() }));
						ex2.HelpLink = "CodeDomDesignerLoaderNoRootSerializer";
					}
					else
					{
						ex2 = new InvalidOperationException(SR.GetString("CodeDomDesignerLoaderNoRootSerializer"));
						ex2.HelpLink = "CodeDomDesignerLoaderNoRootSerializer";
					}
					throw ex2;
				}
				base.LoaderHost.AddService(typeof(CodeTypeDeclaration), this._documentType);
			}
			CodeDomDesignerLoader.codemarkers.CodeMarker(CodeMarkerEvent.perfFXGetDocumentType);
		}

		private bool IntegrateSerializedTree(IDesignerSerializationManager manager, CodeTypeDeclaration newDecl)
		{
			this.EnsureDocument(manager);
			CodeTypeDeclaration documentType = this._documentType;
			bool flag = false;
			bool flag2 = false;
			CodeDomProvider codeDomProvider = this.CodeDomProvider;
			if (codeDomProvider != null)
			{
				flag = (codeDomProvider.LanguageOptions & LanguageOptions.CaseInsensitive) != LanguageOptions.None;
			}
			if (!string.Equals(documentType.Name, newDecl.Name, flag ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
			{
				documentType.Name = newDecl.Name;
				flag2 = true;
			}
			if (!documentType.Attributes.Equals(newDecl.Attributes))
			{
				documentType.Attributes = newDecl.Attributes;
				flag2 = true;
			}
			int num = 0;
			bool flag3 = false;
			int num2 = 0;
			bool flag4 = false;
			IDictionary dictionary = new HybridDictionary(documentType.Members.Count, flag);
			int count = documentType.Members.Count;
			for (int i = 0; i < count; i++)
			{
				CodeTypeMember codeTypeMember = documentType.Members[i];
				string text;
				if (codeTypeMember is CodeConstructor)
				{
					text = ".ctor";
				}
				else if (codeTypeMember is CodeTypeConstructor)
				{
					text = ".cctor";
				}
				else
				{
					text = codeTypeMember.Name;
				}
				dictionary[text] = i;
				if (codeTypeMember is CodeMemberField)
				{
					if (!flag3)
					{
						num = i;
					}
				}
				else if (num > 0)
				{
					flag3 = true;
				}
				if (codeTypeMember is CodeMemberMethod)
				{
					if (!flag4)
					{
						num2 = i;
					}
				}
				else if (num2 > 0)
				{
					flag4 = true;
				}
			}
			ArrayList arrayList = new ArrayList();
			foreach (object obj in newDecl.Members)
			{
				CodeTypeMember codeTypeMember2 = (CodeTypeMember)obj;
				string text2;
				if (codeTypeMember2 is CodeConstructor)
				{
					text2 = ".ctor";
				}
				else
				{
					text2 = codeTypeMember2.Name;
				}
				object obj2 = dictionary[text2];
				if (obj2 != null)
				{
					int num3 = (int)obj2;
					CodeTypeMember codeTypeMember3 = documentType.Members[num3];
					if (codeTypeMember3 != codeTypeMember2)
					{
						if (codeTypeMember2 is CodeMemberField)
						{
							if (codeTypeMember3 is CodeMemberField)
							{
								CodeMemberField codeMemberField = (CodeMemberField)codeTypeMember3;
								CodeMemberField codeMemberField2 = (CodeMemberField)codeTypeMember2;
								if (string.Equals(codeMemberField2.Name, codeMemberField.Name) && codeMemberField2.Attributes == codeMemberField.Attributes && CodeDomDesignerLoader.TypesEqual(codeMemberField2.Type, codeMemberField.Type))
								{
									continue;
								}
								documentType.Members[num3] = codeTypeMember2;
							}
							else
							{
								arrayList.Add(codeTypeMember2);
							}
						}
						else if (codeTypeMember2 is CodeMemberMethod)
						{
							if (codeTypeMember3 is CodeMemberMethod && !(codeTypeMember3 is CodeConstructor))
							{
								CodeMemberMethod codeMemberMethod = (CodeMemberMethod)codeTypeMember3;
								CodeMemberMethod codeMemberMethod2 = (CodeMemberMethod)codeTypeMember2;
								codeMemberMethod.Statements.Clear();
								codeMemberMethod.Statements.AddRange(codeMemberMethod2.Statements);
							}
						}
						else
						{
							documentType.Members[num3] = codeTypeMember2;
						}
						flag2 = true;
					}
				}
				else
				{
					arrayList.Add(codeTypeMember2);
				}
			}
			foreach (object obj3 in arrayList)
			{
				CodeTypeMember codeTypeMember4 = (CodeTypeMember)obj3;
				if (codeTypeMember4 is CodeMemberField)
				{
					if (num >= documentType.Members.Count)
					{
						documentType.Members.Add(codeTypeMember4);
					}
					else
					{
						documentType.Members.Insert(num, codeTypeMember4);
					}
					num++;
					num2++;
					flag2 = true;
				}
				else if (codeTypeMember4 is CodeMemberMethod)
				{
					if (num2 >= documentType.Members.Count)
					{
						documentType.Members.Add(codeTypeMember4);
					}
					else
					{
						documentType.Members.Insert(num2, codeTypeMember4);
					}
					num2++;
					flag2 = true;
				}
				else
				{
					documentType.Members.Add(codeTypeMember4);
					flag2 = true;
				}
			}
			return flag2;
		}

		protected override void Initialize()
		{
			base.Initialize();
			ServiceCreatorCallback serviceCreatorCallback = new ServiceCreatorCallback(this.OnCreateService);
			base.LoaderHost.AddService(typeof(ComponentSerializationService), serviceCreatorCallback);
			base.LoaderHost.AddService(typeof(INameCreationService), this);
			base.LoaderHost.AddService(typeof(IDesignerSerializationService), this);
			if (base.GetService(typeof(ITypeResolutionService)) == null)
			{
				ITypeResolutionService typeResolutionService = this.TypeResolutionService;
				if (typeResolutionService == null)
				{
					throw new InvalidOperationException(SR.GetString("CodeDomDesignerLoaderNoTypeResolution"));
				}
				base.LoaderHost.AddService(typeof(ITypeResolutionService), typeResolutionService);
				this._state[CodeDomDesignerLoader.StateOwnTypeResolution] = true;
			}
			this._extenderProviderService = base.GetService(typeof(IExtenderProviderService)) as IExtenderProviderService;
			if (this._extenderProviderService != null)
			{
				this._extenderProviders = new IExtenderProvider[]
				{
					new CodeDomDesignerLoader.ModifiersExtenderProvider(),
					new CodeDomDesignerLoader.ModifiersInheritedExtenderProvider()
				};
				foreach (IExtenderProvider extenderProvider in this._extenderProviders)
				{
					this._extenderProviderService.AddExtenderProvider(extenderProvider);
				}
			}
		}

		protected override bool IsReloadNeeded()
		{
			if (!base.IsReloadNeeded())
			{
				return false;
			}
			if (this._documentType == null)
			{
				return true;
			}
			ICodeDomDesignerReload codeDomDesignerReload = this.CodeDomProvider as ICodeDomDesignerReload;
			if (codeDomDesignerReload == null)
			{
				return true;
			}
			bool flag = true;
			string name = this._documentType.Name;
			try
			{
				this.ClearDocument();
				this.EnsureDocument(base.GetService(typeof(IDesignerSerializationManager)) as IDesignerSerializationManager);
			}
			catch
			{
			}
			if (this._documentCompileUnit != null)
			{
				flag = codeDomDesignerReload.ShouldReloadDesigner(this._documentCompileUnit);
				flag |= this._documentType == null || !this._documentType.Name.Equals(name);
			}
			return flag;
		}

		protected override void OnBeginLoad()
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)base.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
				componentChangeService.ComponentRename -= this.OnComponentRename;
			}
			base.OnBeginLoad();
		}

		protected override void OnBeginUnload()
		{
			base.OnBeginUnload();
			this.ClearDocument();
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			string name = e.Component.Site.Name;
			this.RemoveDeclaration(name);
		}

		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			this.OnComponentRename(e.Component, e.OldName, e.NewName);
		}

		private object OnCreateService(IServiceContainer container, Type serviceType)
		{
			if (serviceType == typeof(ComponentSerializationService))
			{
				return new CodeDomComponentSerializationService(base.LoaderHost);
			}
			return null;
		}

		protected override void OnEndLoad(bool successful, ICollection errors)
		{
			base.OnEndLoad(successful, errors);
			if (successful)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)base.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRemoved += this.OnComponentRemoved;
					componentChangeService.ComponentRename += this.OnComponentRename;
				}
			}
		}

		protected abstract CodeCompileUnit Parse();

		protected override void PerformFlush(IDesignerSerializationManager manager)
		{
			CodeTypeDeclaration codeTypeDeclaration = null;
			if (this._rootSerializer != null)
			{
				codeTypeDeclaration = this._rootSerializer.Serialize(manager, base.LoaderHost.RootComponent) as CodeTypeDeclaration;
			}
			else if (this._typeSerializer != null)
			{
				codeTypeDeclaration = this._typeSerializer.Serialize(manager, base.LoaderHost.RootComponent, base.LoaderHost.Container.Components);
			}
			CodeDomDesignerLoader.codemarkers.CodeMarker(CodeMarkerEvent.perfFXGenerateCodeTreeEnd);
			if (codeTypeDeclaration != null && this.IntegrateSerializedTree(manager, codeTypeDeclaration))
			{
				CodeDomDesignerLoader.codemarkers.CodeMarker(CodeMarkerEvent.perfFXIntegrateSerializedTreeEnd);
				this.Write(this._documentCompileUnit);
			}
		}

		protected override void PerformLoad(IDesignerSerializationManager manager)
		{
			this.EnsureDocument(manager);
			CodeDomDesignerLoader.codemarkers.CodeMarker(CodeMarkerEvent.perfFXDeserializeStart);
			if (this._rootSerializer != null)
			{
				this._rootSerializer.Deserialize(manager, this._documentType);
			}
			else
			{
				this._typeSerializer.Deserialize(manager, this._documentType);
			}
			CodeDomDesignerLoader.codemarkers.CodeMarker(CodeMarkerEvent.perfFXDeserializeEnd);
			string text = string.Format(CultureInfo.CurrentCulture, "{0}.{1}", new object[]
			{
				this._documentNamespace.Name,
				this._documentType.Name
			});
			base.SetBaseComponentClassName(text);
		}

		protected virtual void OnComponentRename(object component, string oldName, string newName)
		{
			if (base.LoaderHost.RootComponent == component)
			{
				if (this._documentType != null)
				{
					this._documentType.Name = newName;
					return;
				}
			}
			else if (this._documentType != null)
			{
				CodeTypeMemberCollection members = this._documentType.Members;
				for (int i = 0; i < members.Count; i++)
				{
					if (members[i] is CodeMemberField && members[i].Name.Equals(oldName) && ((CodeMemberField)members[i]).Type.BaseType.Equals(TypeDescriptor.GetClassName(component)))
					{
						members[i].Name = newName;
						return;
					}
				}
			}
		}

		private void RemoveDeclaration(string name)
		{
			if (this._documentType != null)
			{
				CodeTypeMemberCollection members = this._documentType.Members;
				for (int i = 0; i < members.Count; i++)
				{
					if (members[i] is CodeMemberField && members[i].Name.Equals(name))
					{
						((IList)members).RemoveAt(i);
						return;
					}
				}
			}
		}

		private void ThrowMissingService(Type serviceType)
		{
			throw new InvalidOperationException(SR.GetString("BasicDesignerLoaderMissingService", new object[] { serviceType.Name }))
			{
				HelpLink = "BasicDesignerLoaderMissingService"
			};
		}

		private static bool TypesEqual(CodeTypeReference typeLeft, CodeTypeReference typeRight)
		{
			if (typeLeft.ArrayRank != typeRight.ArrayRank)
			{
				return false;
			}
			if (!typeLeft.BaseType.Equals(typeRight.BaseType))
			{
				return false;
			}
			if (typeLeft.TypeArguments != null && typeRight.TypeArguments == null)
			{
				return false;
			}
			if (typeLeft.TypeArguments == null && typeRight.TypeArguments != null)
			{
				return false;
			}
			if (typeLeft.TypeArguments != null && typeRight.TypeArguments != null)
			{
				if (typeLeft.TypeArguments.Count != typeRight.TypeArguments.Count)
				{
					return false;
				}
				for (int i = 0; i < typeLeft.TypeArguments.Count; i++)
				{
					if (!CodeDomDesignerLoader.TypesEqual(typeLeft.TypeArguments[i], typeRight.TypeArguments[i]))
					{
						return false;
					}
				}
			}
			return typeLeft.ArrayRank <= 0 || CodeDomDesignerLoader.TypesEqual(typeLeft.ArrayElementType, typeRight.ArrayElementType);
		}

		protected abstract void Write(CodeCompileUnit unit);

		ICollection IDesignerSerializationService.Deserialize(object serializationData)
		{
			if (!(serializationData is SerializationStore))
			{
				throw new ArgumentException(SR.GetString("CodeDomDesignerLoaderBadSerializationObject"))
				{
					HelpLink = "CodeDomDesignerLoaderBadSerializationObject"
				};
			}
			ComponentSerializationService componentSerializationService = base.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
			if (componentSerializationService == null)
			{
				this.ThrowMissingService(typeof(ComponentSerializationService));
			}
			return componentSerializationService.Deserialize((SerializationStore)serializationData, base.LoaderHost.Container);
		}

		object IDesignerSerializationService.Serialize(ICollection objects)
		{
			if (objects == null)
			{
				objects = new object[0];
			}
			ComponentSerializationService componentSerializationService = base.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
			if (componentSerializationService == null)
			{
				this.ThrowMissingService(typeof(ComponentSerializationService));
			}
			SerializationStore serializationStore = componentSerializationService.CreateStore();
			using (serializationStore)
			{
				foreach (object obj in objects)
				{
					componentSerializationService.Serialize(serializationStore, obj);
				}
			}
			return serializationStore;
		}

		string INameCreationService.CreateName(IContainer container, Type dataType)
		{
			if (dataType == null)
			{
				throw new ArgumentNullException("dataType");
			}
			string text = dataType.Name;
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			for (int i = 0; i < text.Length; i++)
			{
				if (!char.IsUpper(text[i]) || (i != 0 && i != text.Length - 1 && !char.IsUpper(text[i + 1])))
				{
					stringBuilder.Append(text.Substring(i));
					break;
				}
				stringBuilder.Append(char.ToLower(text[i], CultureInfo.CurrentCulture));
			}
			stringBuilder.Replace('`', '_');
			text = stringBuilder.ToString();
			CodeTypeDeclaration documentType = this._documentType;
			Hashtable hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
			if (documentType != null)
			{
				foreach (object obj in documentType.Members)
				{
					CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
					hashtable[codeTypeMember.Name] = codeTypeMember;
				}
			}
			string text2;
			if (container != null)
			{
				int num = 0;
				bool flag;
				do
				{
					num++;
					flag = false;
					text2 = string.Format(CultureInfo.CurrentCulture, "{0}{1}", new object[]
					{
						text,
						num.ToString(CultureInfo.InvariantCulture)
					});
					if (container != null && container.Components[text2] != null)
					{
						flag = true;
					}
					if (!flag && hashtable[text2] != null)
					{
						flag = true;
					}
				}
				while (flag);
			}
			else
			{
				text2 = text;
			}
			if (this._codeGenerator == null)
			{
				CodeDomProvider codeDomProvider = this.CodeDomProvider;
				if (codeDomProvider != null)
				{
					this._codeGenerator = codeDomProvider.CreateGenerator();
				}
			}
			if (this._codeGenerator != null)
			{
				text2 = this._codeGenerator.CreateValidIdentifier(text2);
			}
			return text2;
		}

		bool INameCreationService.IsValidName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				return false;
			}
			if (this._codeGenerator == null)
			{
				CodeDomProvider codeDomProvider = this.CodeDomProvider;
				if (codeDomProvider != null)
				{
					this._codeGenerator = codeDomProvider.CreateGenerator();
				}
			}
			if (this._codeGenerator != null)
			{
				if (!this._codeGenerator.IsValidIdentifier(name))
				{
					return false;
				}
				if (!this._codeGenerator.IsValidIdentifier(name + "Handler"))
				{
					return false;
				}
			}
			if (!this.Loading)
			{
				CodeTypeDeclaration documentType = this._documentType;
				if (documentType != null)
				{
					foreach (object obj in documentType.Members)
					{
						CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
						if (string.Equals(codeTypeMember.Name, name, StringComparison.OrdinalIgnoreCase))
						{
							return false;
						}
					}
				}
				if (this.Modified && base.LoaderHost.Container.Components[name] != null)
				{
					return false;
				}
			}
			return true;
		}

		void INameCreationService.ValidateName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(SR.GetString("CodeDomDesignerLoaderInvalidBlankIdentifier"))
				{
					HelpLink = "CodeDomDesignerLoaderInvalidIdentifier"
				};
			}
			if (this._codeGenerator == null)
			{
				CodeDomProvider codeDomProvider = this.CodeDomProvider;
				if (codeDomProvider != null)
				{
					this._codeGenerator = codeDomProvider.CreateGenerator();
				}
			}
			if (this._codeGenerator != null)
			{
				this._codeGenerator.ValidateIdentifier(name);
				try
				{
					this._codeGenerator.ValidateIdentifier(name + "_");
				}
				catch
				{
					throw new ArgumentException(SR.GetString("CodeDomDesignerLoaderInvalidIdentifier", new object[] { name }))
					{
						HelpLink = "CodeDomDesignerLoaderInvalidIdentifier"
					};
				}
			}
			if (!this.Loading)
			{
				bool flag = false;
				CodeTypeDeclaration documentType = this._documentType;
				if (documentType != null)
				{
					foreach (object obj in documentType.Members)
					{
						CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
						if (string.Equals(codeTypeMember.Name, name, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag && this.Modified && base.LoaderHost.Container.Components[name] != null)
				{
					flag = true;
				}
				if (flag)
				{
					throw new ArgumentException(SR.GetString("CodeDomDesignerLoaderDupComponentName", new object[] { name }))
					{
						HelpLink = "CodeDomDesignerLoaderDupComponentName"
					};
				}
			}
		}

		private static TraceSwitch traceCDLoader = new TraceSwitch("CodeDomDesignerLoader", "Trace CodeDomDesignerLoader");

		private static CodeMarkers codemarkers = CodeMarkers.Instance;

		private static readonly int StateCodeDomDirty = BitVector32.CreateMask();

		private static readonly int StateCodeParserChecked = BitVector32.CreateMask(CodeDomDesignerLoader.StateCodeDomDirty);

		private static readonly int StateOwnTypeResolution = BitVector32.CreateMask(CodeDomDesignerLoader.StateCodeParserChecked);

		private BitVector32 _state = default(BitVector32);

		private IExtenderProvider[] _extenderProviders;

		private IExtenderProviderService _extenderProviderService;

		private ICodeGenerator _codeGenerator;

		private CodeDomSerializer _rootSerializer;

		private TypeCodeDomSerializer _typeSerializer;

		private CodeCompileUnit _documentCompileUnit;

		private CodeNamespace _documentNamespace;

		private CodeTypeDeclaration _documentType;

		[ProvideProperty("GenerateMember", typeof(IComponent))]
		[ProvideProperty("Modifiers", typeof(IComponent))]
		private class ModifiersExtenderProvider : IExtenderProvider
		{
			public bool CanExtend(object o)
			{
				IComponent component = o as IComponent;
				if (component == null)
				{
					return false;
				}
				IComponent baseComponent = this.GetBaseComponent(component);
				return o != baseComponent && TypeDescriptor.GetAttributes(o)[typeof(InheritanceAttribute)].Equals(InheritanceAttribute.NotInherited);
			}

			private IComponent GetBaseComponent(IComponent c)
			{
				IComponent component = null;
				if (c == null)
				{
					return null;
				}
				if (this._host == null)
				{
					ISite site = c.Site;
					if (site != null)
					{
						this._host = (IDesignerHost)site.GetService(typeof(IDesignerHost));
					}
				}
				if (this._host != null)
				{
					component = this._host.RootComponent;
				}
				return component;
			}

			[SRDescription("CodeDomDesignerLoaderPropGenerateMember")]
			[Category("Design")]
			[HelpKeyword("Designer_GenerateMember")]
			[DefaultValue(true)]
			[DesignOnly(true)]
			public bool GetGenerateMember(IComponent comp)
			{
				ISite site = comp.Site;
				if (site != null)
				{
					IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
					if (dictionaryService != null)
					{
						object value = dictionaryService.GetValue("GenerateMember");
						if (value is bool)
						{
							return (bool)value;
						}
					}
				}
				return true;
			}

			[DesignOnly(true)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[HelpKeyword("Designer_Modifiers")]
			[TypeConverter(typeof(CodeDomDesignerLoader.ModifierConverter))]
			[Category("Design")]
			[DefaultValue(MemberAttributes.Private)]
			[SRDescription("CodeDomDesignerLoaderPropModifiers")]
			public MemberAttributes GetModifiers(IComponent comp)
			{
				ISite site = comp.Site;
				if (site != null)
				{
					IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
					if (dictionaryService != null)
					{
						object value = dictionaryService.GetValue("Modifiers");
						if (value is MemberAttributes)
						{
							return (MemberAttributes)value;
						}
					}
				}
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(comp);
				PropertyDescriptor propertyDescriptor = properties["DefaultModifiers"];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(MemberAttributes))
				{
					return (MemberAttributes)propertyDescriptor.GetValue(comp);
				}
				return MemberAttributes.Private;
			}

			public void SetGenerateMember(IComponent comp, bool generate)
			{
				ISite site = comp.Site;
				if (site != null)
				{
					IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
					bool generateMember = this.GetGenerateMember(comp);
					if (dictionaryService != null)
					{
						dictionaryService.SetValue("GenerateMember", generate);
					}
					if (generateMember && !generate)
					{
						CodeTypeDeclaration codeTypeDeclaration = site.GetService(typeof(CodeTypeDeclaration)) as CodeTypeDeclaration;
						string name = site.Name;
						if (codeTypeDeclaration != null && name != null)
						{
							foreach (object obj in codeTypeDeclaration.Members)
							{
								CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
								CodeMemberField codeMemberField = codeTypeMember as CodeMemberField;
								if (codeMemberField != null && codeMemberField.Name.Equals(name))
								{
									codeTypeDeclaration.Members.Remove(codeMemberField);
									break;
								}
							}
						}
					}
				}
			}

			public void SetModifiers(IComponent comp, MemberAttributes modifiers)
			{
				ISite site = comp.Site;
				if (site != null)
				{
					IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
					if (dictionaryService != null)
					{
						dictionaryService.SetValue("Modifiers", modifiers);
					}
				}
			}

			private IDesignerHost _host;
		}

		[ProvideProperty("Modifiers", typeof(IComponent))]
		private class ModifiersInheritedExtenderProvider : IExtenderProvider
		{
			public bool CanExtend(object o)
			{
				IComponent component = o as IComponent;
				if (component == null)
				{
					return false;
				}
				IComponent baseComponent = this.GetBaseComponent(component);
				if (o == baseComponent)
				{
					return false;
				}
				AttributeCollection attributes = TypeDescriptor.GetAttributes(o);
				return !attributes[typeof(InheritanceAttribute)].Equals(InheritanceAttribute.NotInherited);
			}

			private IComponent GetBaseComponent(IComponent c)
			{
				IComponent component = null;
				if (c == null)
				{
					return null;
				}
				if (this._host == null)
				{
					ISite site = c.Site;
					if (site != null)
					{
						this._host = (IDesignerHost)site.GetService(typeof(IDesignerHost));
					}
				}
				if (this._host != null)
				{
					component = this._host.RootComponent;
				}
				return component;
			}

			[DesignOnly(true)]
			[Category("Design")]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[TypeConverter(typeof(CodeDomDesignerLoader.ModifierConverter))]
			[DefaultValue(MemberAttributes.Private)]
			[SRDescription("CodeDomDesignerLoaderPropModifiers")]
			public MemberAttributes GetModifiers(IComponent comp)
			{
				IComponent baseComponent = this.GetBaseComponent(comp);
				Type type = baseComponent.GetType();
				ISite site = comp.Site;
				if (site != null)
				{
					string name = site.Name;
					if (name != null)
					{
						FieldInfo field = TypeDescriptor.GetReflectionType(type).GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (field != null)
						{
							if (field.IsPrivate)
							{
								return MemberAttributes.Private;
							}
							if (field.IsPublic)
							{
								return MemberAttributes.Public;
							}
							if (field.IsFamily)
							{
								return MemberAttributes.Family;
							}
							if (field.IsAssembly)
							{
								return MemberAttributes.Assembly;
							}
							if (field.IsFamilyOrAssembly)
							{
								return MemberAttributes.FamilyOrAssembly;
							}
							if (field.IsFamilyAndAssembly)
							{
								return MemberAttributes.FamilyAndAssembly;
							}
						}
						else
						{
							PropertyInfo property = TypeDescriptor.GetReflectionType(type).GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							if (property != null)
							{
								MethodInfo[] accessors = property.GetAccessors(true);
								if (accessors != null && accessors.Length > 0)
								{
									MethodInfo methodInfo = accessors[0];
									if (methodInfo != null)
									{
										if (methodInfo.IsPrivate)
										{
											return MemberAttributes.Private;
										}
										if (methodInfo.IsPublic)
										{
											return MemberAttributes.Public;
										}
										if (methodInfo.IsFamily)
										{
											return MemberAttributes.Family;
										}
										if (methodInfo.IsAssembly)
										{
											return MemberAttributes.Assembly;
										}
										if (methodInfo.IsFamilyOrAssembly)
										{
											return MemberAttributes.FamilyOrAssembly;
										}
										if (methodInfo.IsFamilyAndAssembly)
										{
											return MemberAttributes.FamilyAndAssembly;
										}
									}
								}
							}
						}
					}
				}
				return MemberAttributes.Private;
			}

			private IDesignerHost _host;
		}

		private class ModifierConverter : TypeConverter
		{
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return this.GetConverter(context).CanConvertFrom(context, sourceType);
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return this.GetConverter(context).CanConvertTo(context, destinationType);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				return this.GetConverter(context).ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				return this.GetConverter(context).ConvertTo(context, culture, value, destinationType);
			}

			public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
			{
				return this.GetConverter(context).CreateInstance(context, propertyValues);
			}

			private TypeConverter GetConverter(ITypeDescriptorContext context)
			{
				TypeConverter typeConverter = null;
				if (context != null)
				{
					CodeDomProvider codeDomProvider = (CodeDomProvider)context.GetService(typeof(CodeDomProvider));
					if (codeDomProvider != null)
					{
						typeConverter = codeDomProvider.GetConverter(typeof(MemberAttributes));
					}
				}
				if (typeConverter == null)
				{
					typeConverter = TypeDescriptor.GetConverter(typeof(MemberAttributes));
				}
				return typeConverter;
			}

			public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
			{
				return this.GetConverter(context).GetCreateInstanceSupported(context);
			}

			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				return this.GetConverter(context).GetProperties(context, value, attributes);
			}

			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				return this.GetConverter(context).GetPropertiesSupported(context);
			}

			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				TypeConverter.StandardValuesCollection standardValuesCollection = this.GetConverter(context).GetStandardValues(context);
				if (standardValuesCollection != null && standardValuesCollection.Count > 0)
				{
					bool flag = false;
					foreach (object obj in standardValuesCollection)
					{
						MemberAttributes memberAttributes = (MemberAttributes)obj;
						if ((memberAttributes & MemberAttributes.AccessMask) == (MemberAttributes)0)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						ArrayList arrayList = new ArrayList(standardValuesCollection.Count);
						foreach (object obj2 in standardValuesCollection)
						{
							MemberAttributes memberAttributes2 = (MemberAttributes)obj2;
							if ((memberAttributes2 & MemberAttributes.AccessMask) != (MemberAttributes)0 && memberAttributes2 != MemberAttributes.AccessMask)
							{
								arrayList.Add(memberAttributes2);
							}
						}
						standardValuesCollection = new TypeConverter.StandardValuesCollection(arrayList);
					}
				}
				return standardValuesCollection;
			}

			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return this.GetConverter(context).GetStandardValuesExclusive(context);
			}

			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return this.GetConverter(context).GetStandardValuesSupported(context);
			}

			public override bool IsValid(ITypeDescriptorContext context, object value)
			{
				return this.GetConverter(context).IsValid(context, value);
			}
		}
	}
}
