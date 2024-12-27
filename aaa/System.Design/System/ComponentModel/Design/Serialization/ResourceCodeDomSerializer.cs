using System;
using System.CodeDom;
using System.Collections;
using System.Design;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.Serialization;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000165 RID: 357
	internal class ResourceCodeDomSerializer : CodeDomSerializer
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x00034B40 File Offset: 0x00033B40
		internal new static ResourceCodeDomSerializer Default
		{
			get
			{
				if (ResourceCodeDomSerializer.defaultSerializer == null)
				{
					ResourceCodeDomSerializer.defaultSerializer = new ResourceCodeDomSerializer();
				}
				return ResourceCodeDomSerializer.defaultSerializer;
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00034B58 File Offset: 0x00033B58
		public override string GetTargetComponentName(CodeStatement statement, CodeExpression expression, Type type)
		{
			string text = null;
			CodeExpressionStatement codeExpressionStatement = statement as CodeExpressionStatement;
			if (codeExpressionStatement != null)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression = codeExpressionStatement.Expression as CodeMethodInvokeExpression;
				if (codeMethodInvokeExpression != null)
				{
					CodeMethodReferenceExpression method = codeMethodInvokeExpression.Method;
					if (method != null && string.Equals(method.MethodName, "ApplyResources", StringComparison.OrdinalIgnoreCase) && codeMethodInvokeExpression.Parameters.Count > 0)
					{
						CodeFieldReferenceExpression codeFieldReferenceExpression = codeMethodInvokeExpression.Parameters[0] as CodeFieldReferenceExpression;
						CodeVariableReferenceExpression codeVariableReferenceExpression = codeMethodInvokeExpression.Parameters[0] as CodeVariableReferenceExpression;
						if (codeFieldReferenceExpression != null && codeFieldReferenceExpression.TargetObject is CodeThisReferenceExpression)
						{
							text = codeFieldReferenceExpression.FieldName;
						}
						else if (codeVariableReferenceExpression != null)
						{
							text = codeVariableReferenceExpression.VariableName;
						}
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = base.GetTargetComponentName(statement, expression, type);
			}
			return text;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00034C0F File Offset: 0x00033C0F
		private string ResourceManagerName
		{
			get
			{
				return "resources";
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00034C18 File Offset: 0x00033C18
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			object obj = null;
			if (manager == null || codeObject == null)
			{
				throw new ArgumentNullException((manager == null) ? "manager" : "codeObject");
			}
			using (CodeDomSerializerBase.TraceScope("ResourceCodeDomSerializer::Deserialize"))
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
								if (codeStatement is CodeVariableDeclarationStatement)
								{
									CodeVariableDeclarationStatement codeVariableDeclarationStatement = (CodeVariableDeclarationStatement)codeStatement;
									if (codeVariableDeclarationStatement.Name.Equals(this.ResourceManagerName))
									{
										obj = this.CreateResourceManager(manager);
									}
								}
								else if (obj == null)
								{
									obj = base.DeserializeStatementToInstance(manager, codeStatement);
								}
								else
								{
									base.DeserializeStatement(manager, codeStatement);
								}
							}
							goto IL_015C;
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
				IL_015C:;
			}
			return obj;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00034DC4 File Offset: 0x00033DC4
		private ResourceCodeDomSerializer.SerializationResourceManager CreateResourceManager(IDesignerSerializationManager manager)
		{
			ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
			if (!resourceManager.DeclarationAdded)
			{
				resourceManager.DeclarationAdded = true;
				manager.SetName(resourceManager, this.ResourceManagerName);
			}
			return resourceManager;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00034DF8 File Offset: 0x00033DF8
		protected override object DeserializeInstance(IDesignerSerializationManager manager, Type type, object[] parameters, string name, bool addToContainer)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (name != null && name.Equals(this.ResourceManagerName) && typeof(ResourceManager).IsAssignableFrom(type))
			{
				return this.CreateResourceManager(manager);
			}
			return manager.CreateInstance(type, parameters, name, addToContainer);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00034E5C File Offset: 0x00033E5C
		public object DeserializeInvariant(IDesignerSerializationManager manager, string resourceName)
		{
			ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
			return resourceManager.GetObject(resourceName, true);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00034E7C File Offset: 0x00033E7C
		private Type GetCastType(IDesignerSerializationManager manager, object value)
		{
			ExpressionContext expressionContext = (ExpressionContext)manager.Context[typeof(ExpressionContext)];
			if (expressionContext != null)
			{
				return expressionContext.ExpressionType;
			}
			if (value != null)
			{
				Type type = value.GetType();
				while (!type.IsPublic && !type.IsNestedPublic)
				{
					type = type.BaseType;
				}
				return type;
			}
			return null;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00034ED4 File Offset: 0x00033ED4
		public IDictionaryEnumerator GetEnumerator(IDesignerSerializationManager manager, CultureInfo culture)
		{
			ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
			return resourceManager.GetEnumerator(culture);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00034EF0 File Offset: 0x00033EF0
		public IDictionaryEnumerator GetMetadataEnumerator(IDesignerSerializationManager manager)
		{
			ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
			return resourceManager.GetMetadataEnumerator();
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00034F0C File Offset: 0x00033F0C
		private ResourceCodeDomSerializer.SerializationResourceManager GetResourceManager(IDesignerSerializationManager manager)
		{
			ResourceCodeDomSerializer.SerializationResourceManager serializationResourceManager = manager.Context[typeof(ResourceCodeDomSerializer.SerializationResourceManager)] as ResourceCodeDomSerializer.SerializationResourceManager;
			if (serializationResourceManager == null)
			{
				serializationResourceManager = new ResourceCodeDomSerializer.SerializationResourceManager(manager);
				manager.Context.Append(serializationResourceManager);
			}
			return serializationResourceManager;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00034F4B File Offset: 0x00033F4B
		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			return this.Serialize(manager, value, false, false, true);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00034F58 File Offset: 0x00033F58
		public object Serialize(IDesignerSerializationManager manager, object value, bool shouldSerializeInvariant)
		{
			return this.Serialize(manager, value, false, shouldSerializeInvariant, true);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00034F65 File Offset: 0x00033F65
		public object Serialize(IDesignerSerializationManager manager, object value, bool shouldSerializeInvariant, bool ensureInvariant)
		{
			return this.Serialize(manager, value, false, shouldSerializeInvariant, ensureInvariant);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00034F74 File Offset: 0x00033F74
		private object Serialize(IDesignerSerializationManager manager, object value, bool forceInvariant, bool shouldSerializeInvariant, bool ensureInvariant)
		{
			CodeExpression codeExpression = null;
			using (CodeDomSerializerBase.TraceScope("ResourceCodeDomSerializer::Serialize"))
			{
				ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
				CodeStatementCollection codeStatementCollection = (CodeStatementCollection)manager.Context[typeof(CodeStatementCollection)];
				if (!forceInvariant)
				{
					if (!resourceManager.DeclarationAdded)
					{
						resourceManager.DeclarationAdded = true;
						RootContext rootContext = manager.Context[typeof(RootContext)] as RootContext;
						if (codeStatementCollection != null)
						{
							CodeExpression[] array;
							if (rootContext != null)
							{
								string name = manager.GetName(rootContext.Value);
								array = new CodeExpression[]
								{
									new CodeTypeOfExpression(name)
								};
							}
							else
							{
								array = new CodeExpression[]
								{
									new CodePrimitiveExpression(this.ResourceManagerName)
								};
							}
							CodeExpression codeExpression2 = new CodeObjectCreateExpression(typeof(ComponentResourceManager), array);
							codeStatementCollection.Add(new CodeVariableDeclarationStatement(typeof(ComponentResourceManager), this.ResourceManagerName, codeExpression2));
							base.SetExpression(manager, resourceManager, new CodeVariableReferenceExpression(this.ResourceManagerName));
							resourceManager.ExpressionAdded = true;
							object obj = manager.Context[typeof(ComponentCache)];
							object obj2 = manager.Context[typeof(ComponentCache.Entry)];
						}
					}
					else if (!resourceManager.ExpressionAdded)
					{
						if (base.GetExpression(manager, resourceManager) == null)
						{
							base.SetExpression(manager, resourceManager, new CodeVariableReferenceExpression(this.ResourceManagerName));
						}
						resourceManager.ExpressionAdded = true;
					}
				}
				ExpressionContext expressionContext = (ExpressionContext)manager.Context[typeof(ExpressionContext)];
				string text = resourceManager.SetValue(manager, expressionContext, value, forceInvariant, shouldSerializeInvariant, ensureInvariant, false);
				bool flag;
				string text2;
				if (value is string || (expressionContext != null && expressionContext.ExpressionType == typeof(string)))
				{
					flag = false;
					text2 = "GetString";
				}
				else
				{
					flag = true;
					text2 = "GetObject";
				}
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
				codeMethodInvokeExpression.Method = new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(this.ResourceManagerName), text2);
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(text));
				if (flag)
				{
					Type castType = this.GetCastType(manager, value);
					if (castType != null)
					{
						codeExpression = new CodeCastExpression(castType, codeMethodInvokeExpression);
					}
					else
					{
						codeExpression = codeMethodInvokeExpression;
					}
				}
				else
				{
					codeExpression = codeMethodInvokeExpression;
				}
			}
			return codeExpression;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x000351C0 File Offset: 0x000341C0
		public object SerializeInvariant(IDesignerSerializationManager manager, object value, bool shouldSerializeValue)
		{
			return this.Serialize(manager, value, true, shouldSerializeValue, true);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x000351D0 File Offset: 0x000341D0
		public void SerializeMetadata(IDesignerSerializationManager manager, string name, object value, bool shouldSerializeValue)
		{
			using (CodeDomSerializerBase.TraceScope("ResourceCodeDomSerializer::SerializeMetadata"))
			{
				ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
				resourceManager.SetMetadata(manager, name, value, shouldSerializeValue, false);
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00035218 File Offset: 0x00034218
		public void WriteResource(IDesignerSerializationManager manager, string name, object value)
		{
			using (CodeDomSerializerBase.TraceScope("ResourceCodeDomSerializer::WriteResource"))
			{
				ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
				resourceManager.SetValue(manager, name, value, false, false, true, false);
			}
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00035264 File Offset: 0x00034264
		public void WriteResourceInvariant(IDesignerSerializationManager manager, string name, object value)
		{
			using (CodeDomSerializerBase.TraceScope("ResourceCodeDomSerializer::WriteResourceInvariant"))
			{
				ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
				resourceManager.SetValue(manager, name, value, true, true, true, false);
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000352B0 File Offset: 0x000342B0
		internal void ApplyCacheEntry(IDesignerSerializationManager manager, ComponentCache.Entry entry)
		{
			ResourceCodeDomSerializer.SerializationResourceManager resourceManager = this.GetResourceManager(manager);
			if (entry.Metadata != null)
			{
				foreach (ComponentCache.ResourceEntry resourceEntry in entry.Metadata)
				{
					resourceManager.SetMetadata(manager, resourceEntry.Name, resourceEntry.Value, resourceEntry.ShouldSerializeValue, true);
				}
			}
			if (entry.Resources != null)
			{
				foreach (ComponentCache.ResourceEntry resourceEntry2 in entry.Resources)
				{
					manager.Context.Push(resourceEntry2.PropertyDescriptor);
					manager.Context.Push(resourceEntry2.ExpressionContext);
					try
					{
						resourceManager.SetValue(manager, resourceEntry2.Name, resourceEntry2.Value, resourceEntry2.ForceInvariant, resourceEntry2.ShouldSerializeValue, resourceEntry2.EnsureInvariant, true);
					}
					finally
					{
						manager.Context.Pop();
						manager.Context.Pop();
					}
				}
			}
		}

		// Token: 0x04000EF8 RID: 3832
		private static ResourceCodeDomSerializer defaultSerializer;

		// Token: 0x02000166 RID: 358
		private class SerializationResourceManager : ComponentResourceManager
		{
			// Token: 0x06000D4C RID: 3404 RVA: 0x000353E8 File Offset: 0x000343E8
			public SerializationResourceManager(IDesignerSerializationManager manager)
			{
				this.manager = manager;
				this.nameTable = new Hashtable();
				manager.SerializationComplete += this.OnSerializationComplete;
			}

			// Token: 0x1700020D RID: 525
			// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00035414 File Offset: 0x00034414
			// (set) Token: 0x06000D4E RID: 3406 RVA: 0x0003541C File Offset: 0x0003441C
			public bool DeclarationAdded
			{
				get
				{
					return this.declarationAdded;
				}
				set
				{
					this.declarationAdded = value;
				}
			}

			// Token: 0x1700020E RID: 526
			// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00035425 File Offset: 0x00034425
			// (set) Token: 0x06000D50 RID: 3408 RVA: 0x0003542D File Offset: 0x0003442D
			public bool ExpressionAdded
			{
				get
				{
					return this.expressionAdded;
				}
				set
				{
					this.expressionAdded = value;
				}
			}

			// Token: 0x1700020F RID: 527
			// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00035438 File Offset: 0x00034438
			private CultureInfo LocalizationLanguage
			{
				get
				{
					if (!this.checkedLocalizationLanguage)
					{
						RootContext rootContext = this.manager.Context[typeof(RootContext)] as RootContext;
						if (rootContext != null)
						{
							object value = rootContext.Value;
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(value)["LoadLanguage"];
							if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(CultureInfo))
							{
								this.localizationLanguage = (CultureInfo)propertyDescriptor.GetValue(value);
							}
						}
						this.checkedLocalizationLanguage = true;
					}
					return this.localizationLanguage;
				}
			}

			// Token: 0x17000210 RID: 528
			// (get) Token: 0x06000D52 RID: 3410 RVA: 0x000354BC File Offset: 0x000344BC
			private CultureInfo ReadCulture
			{
				get
				{
					if (this.readCulture == null)
					{
						CultureInfo cultureInfo = this.LocalizationLanguage;
						if (cultureInfo != null)
						{
							this.readCulture = cultureInfo;
						}
						else
						{
							this.readCulture = CultureInfo.InvariantCulture;
						}
					}
					return this.readCulture;
				}
			}

			// Token: 0x17000211 RID: 529
			// (get) Token: 0x06000D53 RID: 3411 RVA: 0x000354F5 File Offset: 0x000344F5
			private Hashtable ResourceTable
			{
				get
				{
					if (this.resourceSets == null)
					{
						this.resourceSets = new Hashtable();
					}
					return this.resourceSets;
				}
			}

			// Token: 0x17000212 RID: 530
			// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00035510 File Offset: 0x00034510
			private object RootComponent
			{
				get
				{
					if (this.rootComponent == null)
					{
						RootContext rootContext = this.manager.Context[typeof(RootContext)] as RootContext;
						if (rootContext != null)
						{
							this.rootComponent = rootContext.Value;
						}
					}
					return this.rootComponent;
				}
			}

			// Token: 0x17000213 RID: 531
			// (get) Token: 0x06000D55 RID: 3413 RVA: 0x0003555C File Offset: 0x0003455C
			private IResourceWriter Writer
			{
				get
				{
					if (this.writer == null)
					{
						IResourceService resourceService = (IResourceService)this.manager.GetService(typeof(IResourceService));
						if (resourceService != null)
						{
							this.writer = resourceService.GetResourceWriter(this.ReadCulture);
						}
						else
						{
							this.writer = new ResourceWriter(new MemoryStream());
						}
					}
					return this.writer;
				}
			}

			// Token: 0x06000D56 RID: 3414 RVA: 0x000355BC File Offset: 0x000345BC
			private void AddCacheEntry(IDesignerSerializationManager manager, string name, object value, bool isMetadata, bool forceInvariant, bool shouldSerializeValue, bool ensureInvariant)
			{
				ComponentCache.Entry entry = manager.Context[typeof(ComponentCache.Entry)] as ComponentCache.Entry;
				if (entry != null)
				{
					ComponentCache.ResourceEntry resourceEntry = default(ComponentCache.ResourceEntry);
					resourceEntry.Name = name;
					resourceEntry.Value = value;
					resourceEntry.ForceInvariant = forceInvariant;
					resourceEntry.ShouldSerializeValue = shouldSerializeValue;
					resourceEntry.EnsureInvariant = ensureInvariant;
					resourceEntry.PropertyDescriptor = (PropertyDescriptor)manager.Context[typeof(PropertyDescriptor)];
					resourceEntry.ExpressionContext = (ExpressionContext)manager.Context[typeof(ExpressionContext)];
					if (isMetadata)
					{
						entry.AddMetadata(resourceEntry);
						return;
					}
					entry.AddResource(resourceEntry);
				}
			}

			// Token: 0x06000D57 RID: 3415 RVA: 0x00035674 File Offset: 0x00034674
			public bool AddPropertyFill(object value)
			{
				bool flag = false;
				if (this.propertyFillAdded == null)
				{
					this.propertyFillAdded = new Hashtable();
				}
				else
				{
					flag = this.propertyFillAdded.ContainsKey(value);
				}
				if (!flag)
				{
					this.propertyFillAdded[value] = value;
				}
				return !flag;
			}

			// Token: 0x06000D58 RID: 3416 RVA: 0x000356B9 File Offset: 0x000346B9
			public override void ApplyResources(object value, string objectName, CultureInfo culture)
			{
				if (culture == null)
				{
					culture = this.ReadCulture;
				}
				base.ApplyResources(value, objectName, culture);
			}

			// Token: 0x06000D59 RID: 3417 RVA: 0x000356D0 File Offset: 0x000346D0
			private ResourceCodeDomSerializer.SerializationResourceManager.CompareValue CompareWithParentValue(string name, object value)
			{
				if (this.ReadCulture.Equals(CultureInfo.InvariantCulture))
				{
					return ResourceCodeDomSerializer.SerializationResourceManager.CompareValue.Different;
				}
				CultureInfo parent = this.ReadCulture;
				Hashtable resourceSet;
				for (;;)
				{
					parent = parent.Parent;
					resourceSet = this.GetResourceSet(parent);
					bool flag = resourceSet != null && resourceSet.ContainsKey(name);
					if (flag)
					{
						break;
					}
					if (parent.Equals(CultureInfo.InvariantCulture))
					{
						return ResourceCodeDomSerializer.SerializationResourceManager.CompareValue.New;
					}
				}
				object obj = ((resourceSet != null) ? resourceSet[name] : null);
				if (obj == value)
				{
					return ResourceCodeDomSerializer.SerializationResourceManager.CompareValue.Same;
				}
				if (obj == null)
				{
					return ResourceCodeDomSerializer.SerializationResourceManager.CompareValue.Different;
				}
				if (obj.Equals(value))
				{
					return ResourceCodeDomSerializer.SerializationResourceManager.CompareValue.Same;
				}
				return ResourceCodeDomSerializer.SerializationResourceManager.CompareValue.Different;
			}

			// Token: 0x06000D5A RID: 3418 RVA: 0x0003574C File Offset: 0x0003474C
			private Hashtable CreateResourceSet(IResourceReader reader, CultureInfo culture)
			{
				Hashtable hashtable = new Hashtable();
				try
				{
					IDictionaryEnumerator enumerator = reader.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string text = (string)enumerator.Key;
						object value = enumerator.Value;
						hashtable[text] = value;
					}
				}
				catch (Exception ex)
				{
					string text2 = ex.Message;
					if (text2 == null || text2.Length == 0)
					{
						text2 = ex.GetType().Name;
					}
					Exception ex2;
					if (culture == CultureInfo.InvariantCulture)
					{
						ex2 = new SerializationException(SR.GetString("SerializerResourceExceptionInvariant", new object[] { text2 }), ex);
					}
					else
					{
						ex2 = new SerializationException(SR.GetString("SerializerResourceException", new object[]
						{
							culture.ToString(),
							text2
						}), ex);
					}
					this.manager.ReportError(ex2);
				}
				return hashtable;
			}

			// Token: 0x06000D5B RID: 3419 RVA: 0x00035830 File Offset: 0x00034830
			public IDictionaryEnumerator GetMetadataEnumerator()
			{
				if (this.mergedMetadata == null)
				{
					Hashtable hashtable = this.GetMetadata();
					if (hashtable != null)
					{
						Hashtable resourceSet = this.GetResourceSet(CultureInfo.InvariantCulture);
						if (resourceSet != null)
						{
							foreach (object obj in resourceSet)
							{
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
								if (!hashtable.ContainsKey(dictionaryEntry.Key))
								{
									hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
								}
							}
						}
						this.mergedMetadata = hashtable;
					}
				}
				if (this.mergedMetadata != null)
				{
					return this.mergedMetadata.GetEnumerator();
				}
				return null;
			}

			// Token: 0x06000D5C RID: 3420 RVA: 0x000358E0 File Offset: 0x000348E0
			public IDictionaryEnumerator GetEnumerator(CultureInfo culture)
			{
				Hashtable resourceSet = this.GetResourceSet(culture);
				if (resourceSet != null)
				{
					return resourceSet.GetEnumerator();
				}
				return null;
			}

			// Token: 0x06000D5D RID: 3421 RVA: 0x00035900 File Offset: 0x00034900
			private Hashtable GetMetadata()
			{
				if (this.metadata == null)
				{
					IResourceService resourceService = (IResourceService)this.manager.GetService(typeof(IResourceService));
					if (resourceService != null)
					{
						IResourceReader resourceReader = resourceService.GetResourceReader(CultureInfo.InvariantCulture);
						if (resourceReader != null)
						{
							try
							{
								ResXResourceReader resXResourceReader = resourceReader as ResXResourceReader;
								if (resXResourceReader != null)
								{
									this.metadata = new Hashtable();
									IDictionaryEnumerator metadataEnumerator = resXResourceReader.GetMetadataEnumerator();
									while (metadataEnumerator.MoveNext())
									{
										this.metadata[metadataEnumerator.Key] = metadataEnumerator.Value;
									}
								}
							}
							finally
							{
								resourceReader.Close();
							}
						}
					}
				}
				return this.metadata;
			}

			// Token: 0x06000D5E RID: 3422 RVA: 0x000359A0 File Offset: 0x000349A0
			public override object GetObject(string resourceName)
			{
				return this.GetObject(resourceName, false);
			}

			// Token: 0x06000D5F RID: 3423 RVA: 0x000359AC File Offset: 0x000349AC
			public object GetObject(string resourceName, bool forceInvariant)
			{
				CultureInfo cultureInfo;
				if (forceInvariant)
				{
					cultureInfo = CultureInfo.InvariantCulture;
				}
				else
				{
					cultureInfo = this.ReadCulture;
				}
				object obj = null;
				while (obj == null)
				{
					Hashtable resourceSet = this.GetResourceSet(cultureInfo);
					if (resourceSet != null)
					{
						obj = resourceSet[resourceName];
					}
					CultureInfo cultureInfo2 = cultureInfo;
					cultureInfo = cultureInfo.Parent;
					if (cultureInfo2.Equals(cultureInfo))
					{
						break;
					}
				}
				return obj;
			}

			// Token: 0x06000D60 RID: 3424 RVA: 0x000359F8 File Offset: 0x000349F8
			private Hashtable GetResourceSet(CultureInfo culture)
			{
				Hashtable hashtable = null;
				object obj = this.ResourceTable[culture];
				if (obj == null)
				{
					IResourceService resourceService = (IResourceService)this.manager.GetService(typeof(IResourceService));
					if (resourceService != null)
					{
						IResourceReader resourceReader = resourceService.GetResourceReader(culture);
						if (resourceReader != null)
						{
							try
							{
								hashtable = this.CreateResourceSet(resourceReader, culture);
							}
							finally
							{
								resourceReader.Close();
							}
							this.ResourceTable[culture] = hashtable;
						}
						else if (culture.Equals(CultureInfo.InvariantCulture))
						{
							hashtable = new Hashtable();
							this.ResourceTable[culture] = hashtable;
						}
						else
						{
							this.ResourceTable[culture] = ResourceCodeDomSerializer.SerializationResourceManager.resourceSetSentinel;
						}
					}
				}
				else
				{
					hashtable = obj as Hashtable;
				}
				return hashtable;
			}

			// Token: 0x06000D61 RID: 3425 RVA: 0x00035AB0 File Offset: 0x00034AB0
			public override ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
			{
				if (culture == null)
				{
					throw new ArgumentNullException("culture");
				}
				Hashtable resourceSet;
				for (;;)
				{
					resourceSet = this.GetResourceSet(culture);
					if (resourceSet != null)
					{
						break;
					}
					CultureInfo cultureInfo = culture;
					culture = culture.Parent;
					if (!tryParents || cultureInfo.Equals(culture))
					{
						goto IL_0038;
					}
				}
				return new ResourceCodeDomSerializer.SerializationResourceManager.CodeDomResourceSet(resourceSet);
				IL_0038:
				if (createIfNotExists)
				{
					return new ResourceCodeDomSerializer.SerializationResourceManager.CodeDomResourceSet();
				}
				return null;
			}

			// Token: 0x06000D62 RID: 3426 RVA: 0x00035AFF File Offset: 0x00034AFF
			public override string GetString(string resourceName)
			{
				return this.GetObject(resourceName, false) as string;
			}

			// Token: 0x06000D63 RID: 3427 RVA: 0x00035B10 File Offset: 0x00034B10
			private void OnSerializationComplete(object sender, EventArgs e)
			{
				if (this.writer != null)
				{
					this.writer.Close();
					this.writer = null;
				}
				if (this.invariantCultureResourcesDirty || this.metadataResourcesDirty)
				{
					IResourceService resourceService = (IResourceService)this.manager.GetService(typeof(IResourceService));
					if (resourceService != null)
					{
						IResourceWriter resourceWriter = resourceService.GetResourceWriter(CultureInfo.InvariantCulture);
						try
						{
							object obj = this.ResourceTable[CultureInfo.InvariantCulture];
							Hashtable hashtable = (Hashtable)obj;
							IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
							while (enumerator.MoveNext())
							{
								string text = (string)enumerator.Key;
								object value = enumerator.Value;
								resourceWriter.AddResource(text, value);
							}
							this.invariantCultureResourcesDirty = false;
							ResXResourceWriter resXResourceWriter = resourceWriter as ResXResourceWriter;
							if (resXResourceWriter != null)
							{
								foreach (object obj2 in this.metadata)
								{
									DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
									resXResourceWriter.AddMetadata((string)dictionaryEntry.Key, dictionaryEntry.Value);
								}
							}
							this.metadataResourcesDirty = false;
							return;
						}
						finally
						{
							resourceWriter.Close();
						}
					}
					this.invariantCultureResourcesDirty = false;
					this.metadataResourcesDirty = false;
				}
			}

			// Token: 0x06000D64 RID: 3428 RVA: 0x00035C68 File Offset: 0x00034C68
			public void SetMetadata(IDesignerSerializationManager manager, string resourceName, object value, bool shouldSerializeValue, bool applyingCachedResources)
			{
				if (value != null && !value.GetType().IsSerializable)
				{
					return;
				}
				if (this.ReadCulture.Equals(CultureInfo.InvariantCulture))
				{
					ResXResourceWriter resXResourceWriter = this.Writer as ResXResourceWriter;
					if (shouldSerializeValue)
					{
						if (resXResourceWriter != null)
						{
							resXResourceWriter.AddMetadata(resourceName, value);
						}
						else
						{
							this.Writer.AddResource(resourceName, value);
						}
					}
				}
				else
				{
					IResourceWriter resourceWriter = null;
					IResourceService resourceService = (IResourceService)manager.GetService(typeof(IResourceService));
					if (resourceService != null)
					{
						resourceWriter = resourceService.GetResourceWriter(CultureInfo.InvariantCulture);
					}
					Hashtable resourceSet = this.GetResourceSet(CultureInfo.InvariantCulture);
					Hashtable hashtable;
					if (resourceWriter == null || resourceWriter is ResXResourceWriter)
					{
						hashtable = this.GetMetadata();
						if (hashtable == null)
						{
							this.metadata = new Hashtable();
							hashtable = this.metadata;
						}
						if (resourceSet.ContainsKey(resourceName))
						{
							resourceSet.Remove(resourceName);
						}
						this.metadataResourcesDirty = true;
					}
					else
					{
						hashtable = resourceSet;
						this.invariantCultureResourcesDirty = true;
					}
					if (hashtable != null)
					{
						if (shouldSerializeValue)
						{
							hashtable[resourceName] = value;
						}
						else
						{
							hashtable.Remove(resourceName);
						}
					}
					this.mergedMetadata = null;
				}
				if (!applyingCachedResources)
				{
					this.AddCacheEntry(manager, resourceName, value, true, false, shouldSerializeValue, false);
				}
			}

			// Token: 0x06000D65 RID: 3429 RVA: 0x00035D80 File Offset: 0x00034D80
			public void SetValue(IDesignerSerializationManager manager, string resourceName, object value, bool forceInvariant, bool shouldSerializeInvariant, bool ensureInvariant, bool applyingCachedResources)
			{
				if (value != null && !value.GetType().IsSerializable)
				{
					return;
				}
				if (forceInvariant)
				{
					if (this.ReadCulture.Equals(CultureInfo.InvariantCulture))
					{
						if (shouldSerializeInvariant)
						{
							this.Writer.AddResource(resourceName, value);
						}
					}
					else
					{
						Hashtable resourceSet = this.GetResourceSet(CultureInfo.InvariantCulture);
						if (shouldSerializeInvariant)
						{
							resourceSet[resourceName] = value;
						}
						else
						{
							resourceSet.Remove(resourceName);
						}
						this.invariantCultureResourcesDirty = true;
					}
				}
				else
				{
					switch (this.CompareWithParentValue(resourceName, value))
					{
					case ResourceCodeDomSerializer.SerializationResourceManager.CompareValue.Different:
						this.Writer.AddResource(resourceName, value);
						break;
					case ResourceCodeDomSerializer.SerializationResourceManager.CompareValue.New:
						if (ensureInvariant)
						{
							Hashtable resourceSet2 = this.GetResourceSet(CultureInfo.InvariantCulture);
							resourceSet2[resourceName] = value;
							this.invariantCultureResourcesDirty = true;
							this.Writer.AddResource(resourceName, value);
						}
						else
						{
							bool flag = true;
							bool flag2 = false;
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)manager.Context[typeof(PropertyDescriptor)];
							if (propertyDescriptor != null)
							{
								ExpressionContext expressionContext = (ExpressionContext)manager.Context[typeof(ExpressionContext)];
								if (expressionContext != null && expressionContext.Expression is CodePropertyReferenceExpression)
								{
									flag = propertyDescriptor.ShouldSerializeValue(expressionContext.Owner);
									flag2 = !propertyDescriptor.CanResetValue(expressionContext.Owner);
								}
							}
							if (flag)
							{
								this.Writer.AddResource(resourceName, value);
								if (flag2)
								{
									Hashtable resourceSet3 = this.GetResourceSet(CultureInfo.InvariantCulture);
									resourceSet3[resourceName] = value;
									this.invariantCultureResourcesDirty = true;
								}
							}
						}
						break;
					}
				}
				if (!applyingCachedResources)
				{
					this.AddCacheEntry(manager, resourceName, value, false, forceInvariant, shouldSerializeInvariant, ensureInvariant);
				}
			}

			// Token: 0x06000D66 RID: 3430 RVA: 0x00035F1C File Offset: 0x00034F1C
			public string SetValue(IDesignerSerializationManager manager, ExpressionContext tree, object value, bool forceInvariant, bool shouldSerializeInvariant, bool ensureInvariant, bool applyingCachedResources)
			{
				bool flag = false;
				string text;
				if (tree != null)
				{
					if (tree.Owner == this.RootComponent)
					{
						text = "$this";
					}
					else
					{
						text = manager.GetName(tree.Owner);
						if (text == null)
						{
							IReferenceService referenceService = (IReferenceService)manager.GetService(typeof(IReferenceService));
							if (referenceService != null)
							{
								text = referenceService.GetName(tree.Owner);
							}
						}
					}
					CodeExpression expression = tree.Expression;
					string text2;
					if (expression is CodePropertyReferenceExpression)
					{
						text2 = ((CodePropertyReferenceExpression)expression).PropertyName;
					}
					else if (expression is CodeFieldReferenceExpression)
					{
						text2 = ((CodeFieldReferenceExpression)expression).FieldName;
					}
					else if (expression is CodeMethodReferenceExpression)
					{
						text2 = ((CodeMethodReferenceExpression)expression).MethodName;
						if (text2.StartsWith("Set"))
						{
							text2 = text2.Substring(3);
						}
					}
					else
					{
						text2 = null;
					}
					if (text == null)
					{
						text = "resource";
					}
					if (text2 != null)
					{
						text = text + "." + text2;
					}
				}
				else
				{
					text = "resource";
					flag = true;
				}
				string text3 = text;
				int num = 1;
				do
				{
					if (flag)
					{
						text3 = text + num.ToString(CultureInfo.InvariantCulture);
						num++;
					}
					else
					{
						flag = true;
					}
				}
				while (this.nameTable.ContainsKey(text3));
				this.SetValue(manager, text3, value, forceInvariant, shouldSerializeInvariant, ensureInvariant, applyingCachedResources);
				this.nameTable[text3] = text3;
				return text3;
			}

			// Token: 0x04000EF9 RID: 3833
			private static object resourceSetSentinel = new object();

			// Token: 0x04000EFA RID: 3834
			private IDesignerSerializationManager manager;

			// Token: 0x04000EFB RID: 3835
			private bool checkedLocalizationLanguage;

			// Token: 0x04000EFC RID: 3836
			private CultureInfo localizationLanguage;

			// Token: 0x04000EFD RID: 3837
			private IResourceWriter writer;

			// Token: 0x04000EFE RID: 3838
			private CultureInfo readCulture;

			// Token: 0x04000EFF RID: 3839
			private Hashtable nameTable;

			// Token: 0x04000F00 RID: 3840
			private Hashtable resourceSets;

			// Token: 0x04000F01 RID: 3841
			private Hashtable metadata;

			// Token: 0x04000F02 RID: 3842
			private Hashtable mergedMetadata;

			// Token: 0x04000F03 RID: 3843
			private object rootComponent;

			// Token: 0x04000F04 RID: 3844
			private bool declarationAdded;

			// Token: 0x04000F05 RID: 3845
			private bool expressionAdded;

			// Token: 0x04000F06 RID: 3846
			private Hashtable propertyFillAdded;

			// Token: 0x04000F07 RID: 3847
			private bool invariantCultureResourcesDirty;

			// Token: 0x04000F08 RID: 3848
			private bool metadataResourcesDirty;

			// Token: 0x02000167 RID: 359
			private class CodeDomResourceSet : ResourceSet
			{
				// Token: 0x06000D68 RID: 3432 RVA: 0x00036070 File Offset: 0x00035070
				public CodeDomResourceSet()
				{
				}

				// Token: 0x06000D69 RID: 3433 RVA: 0x00036078 File Offset: 0x00035078
				public CodeDomResourceSet(Hashtable resources)
				{
					this.Table = resources;
				}
			}

			// Token: 0x02000168 RID: 360
			private enum CompareValue
			{
				// Token: 0x04000F0A RID: 3850
				Same,
				// Token: 0x04000F0B RID: 3851
				Different,
				// Token: 0x04000F0C RID: 3852
				New
			}
		}
	}
}
