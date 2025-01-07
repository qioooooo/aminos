using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.ComponentModel.Design.Serialization
{
	public sealed class CodeDomComponentSerializationService : ComponentSerializationService
	{
		public CodeDomComponentSerializationService()
			: this(null)
		{
		}

		public CodeDomComponentSerializationService(IServiceProvider provider)
		{
			this._provider = provider;
		}

		public override SerializationStore CreateStore()
		{
			return new CodeDomComponentSerializationService.CodeDomSerializationStore(this._provider);
		}

		public override SerializationStore LoadStore(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			return CodeDomComponentSerializationService.CodeDomSerializationStore.Load(stream);
		}

		public override void Serialize(SerializationStore store, object value)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			CodeDomComponentSerializationService.CodeDomSerializationStore codeDomSerializationStore = store as CodeDomComponentSerializationService.CodeDomSerializationStore;
			if (codeDomSerializationStore == null)
			{
				throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceUnknownStore"));
			}
			codeDomSerializationStore.AddObject(value, false);
		}

		public override void SerializeAbsolute(SerializationStore store, object value)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			CodeDomComponentSerializationService.CodeDomSerializationStore codeDomSerializationStore = store as CodeDomComponentSerializationService.CodeDomSerializationStore;
			if (codeDomSerializationStore == null)
			{
				throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceUnknownStore"));
			}
			codeDomSerializationStore.AddObject(value, true);
		}

		public override void SerializeMember(SerializationStore store, object owningObject, MemberDescriptor member)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			if (owningObject == null)
			{
				throw new ArgumentNullException("owningObject");
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			CodeDomComponentSerializationService.CodeDomSerializationStore codeDomSerializationStore = store as CodeDomComponentSerializationService.CodeDomSerializationStore;
			if (codeDomSerializationStore == null)
			{
				throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceUnknownStore"));
			}
			codeDomSerializationStore.AddMember(owningObject, member, false);
		}

		public override void SerializeMemberAbsolute(SerializationStore store, object owningObject, MemberDescriptor member)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			if (owningObject == null)
			{
				throw new ArgumentNullException("owningObject");
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			CodeDomComponentSerializationService.CodeDomSerializationStore codeDomSerializationStore = store as CodeDomComponentSerializationService.CodeDomSerializationStore;
			if (codeDomSerializationStore == null)
			{
				throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceUnknownStore"));
			}
			codeDomSerializationStore.AddMember(owningObject, member, true);
		}

		public override ICollection Deserialize(SerializationStore store)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			CodeDomComponentSerializationService.CodeDomSerializationStore codeDomSerializationStore = store as CodeDomComponentSerializationService.CodeDomSerializationStore;
			if (codeDomSerializationStore == null)
			{
				throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceUnknownStore"));
			}
			return codeDomSerializationStore.Deserialize(this._provider);
		}

		public override ICollection Deserialize(SerializationStore store, IContainer container)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			CodeDomComponentSerializationService.CodeDomSerializationStore codeDomSerializationStore = store as CodeDomComponentSerializationService.CodeDomSerializationStore;
			if (codeDomSerializationStore == null)
			{
				throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceUnknownStore"));
			}
			return codeDomSerializationStore.Deserialize(this._provider, container);
		}

		public override void DeserializeTo(SerializationStore store, IContainer container, bool validateRecycledTypes, bool applyDefaults)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			CodeDomComponentSerializationService.CodeDomSerializationStore codeDomSerializationStore = store as CodeDomComponentSerializationService.CodeDomSerializationStore;
			if (codeDomSerializationStore == null)
			{
				throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceUnknownStore"));
			}
			codeDomSerializationStore.DeserializeTo(this._provider, container, validateRecycledTypes, applyDefaults);
		}

		private IServiceProvider _provider;

		[Serializable]
		private sealed class CodeDomSerializationStore : SerializationStore, ISerializable
		{
			internal CodeDomSerializationStore(IServiceProvider provider)
			{
				this._provider = provider;
				this._objects = new Hashtable();
				this._objectNames = new ArrayList();
				this._shimObjectNames = new List<string>();
			}

			private CodeDomSerializationStore(SerializationInfo info, StreamingContext context)
			{
				this._objectState = (Hashtable)info.GetValue("State", typeof(Hashtable));
				this._objectNames = (ArrayList)info.GetValue("Names", typeof(ArrayList));
				this._assemblies = (AssemblyName[])info.GetValue("Assemblies", typeof(AssemblyName[]));
				this._shimObjectNames = (List<string>)info.GetValue("Shim", typeof(List<string>));
				Hashtable hashtable = (Hashtable)info.GetValue("Resources", typeof(Hashtable));
				if (hashtable != null)
				{
					this._resources = new CodeDomComponentSerializationService.CodeDomSerializationStore.LocalResourceManager(hashtable);
				}
			}

			private AssemblyName[] AssemblyNames
			{
				get
				{
					return this._assemblies;
				}
			}

			public override ICollection Errors
			{
				get
				{
					if (this._errors == null)
					{
						this._errors = new object[0];
					}
					object[] array = new object[this._errors.Count];
					this._errors.CopyTo(array, 0);
					return array;
				}
			}

			private CodeDomComponentSerializationService.CodeDomSerializationStore.LocalResourceManager Resources
			{
				get
				{
					if (this._resources == null)
					{
						this._resources = new CodeDomComponentSerializationService.CodeDomSerializationStore.LocalResourceManager();
					}
					return this._resources;
				}
			}

			internal void AddMember(object value, MemberDescriptor member, bool absolute)
			{
				if (this._objectState != null)
				{
					throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceClosedStore"));
				}
				CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData objectData = (CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData)this._objects[value];
				if (objectData == null)
				{
					objectData = new CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData();
					objectData.Name = this.GetObjectName(value);
					objectData.Value = value;
					this._objects[value] = objectData;
					this._objectNames.Add(objectData.Name);
				}
				objectData.Members.Add(new CodeDomComponentSerializationService.CodeDomSerializationStore.MemberData(member, absolute));
			}

			internal void AddObject(object value, bool absolute)
			{
				if (this._objectState != null)
				{
					throw new InvalidOperationException(SR.GetString("CodeDomComponentSerializationServiceClosedStore"));
				}
				CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData objectData = (CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData)this._objects[value];
				if (objectData == null)
				{
					objectData = new CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData();
					objectData.Name = this.GetObjectName(value);
					objectData.Value = value;
					this._objects[value] = objectData;
					this._objectNames.Add(objectData.Name);
				}
				objectData.EntireObject = true;
				objectData.Absolute = absolute;
			}

			public override void Close()
			{
				if (this._objectState == null)
				{
					Hashtable hashtable = new Hashtable(this._objects.Count);
					DesignerSerializationManager designerSerializationManager = new DesignerSerializationManager(new CodeDomComponentSerializationService.CodeDomSerializationStore.LocalServices(this, this._provider));
					DesignerSerializationManager designerSerializationManager2 = this._provider.GetService(typeof(IDesignerSerializationManager)) as DesignerSerializationManager;
					if (designerSerializationManager2 != null)
					{
						foreach (object obj in designerSerializationManager2.SerializationProviders)
						{
							IDesignerSerializationProvider designerSerializationProvider = (IDesignerSerializationProvider)obj;
							((IDesignerSerializationManager)designerSerializationManager).AddSerializationProvider(designerSerializationProvider);
						}
					}
					using (designerSerializationManager.CreateSession())
					{
						foreach (object obj2 in this._objects.Values)
						{
							CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData objectData = (CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData)obj2;
							((IDesignerSerializationManager)designerSerializationManager).SetName(objectData.Value, objectData.Name);
						}
						CodeDomComponentSerializationService.CodeDomSerializationStore.ComponentListCodeDomSerializer.Instance.Serialize(designerSerializationManager, this._objects, hashtable, this._shimObjectNames);
						this._errors = designerSerializationManager.Errors;
					}
					if (this._resources != null && this._resourceStream == null)
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						this._resourceStream = new MemoryStream();
						binaryFormatter.Serialize(this._resourceStream, this._resources.Data);
					}
					Hashtable hashtable2 = new Hashtable(this._objects.Count);
					foreach (object obj3 in this._objects.Keys)
					{
						Assembly assembly = obj3.GetType().Assembly;
						hashtable2[assembly] = null;
					}
					this._assemblies = new AssemblyName[hashtable2.Count];
					int num = 0;
					foreach (object obj4 in hashtable2.Keys)
					{
						Assembly assembly2 = (Assembly)obj4;
						this._assemblies[num++] = assembly2.GetName(true);
					}
					this._objectState = hashtable;
					this._objects = null;
				}
			}

			internal ICollection Deserialize(IServiceProvider provider)
			{
				return this.Deserialize(provider, null, false, true, true);
			}

			internal ICollection Deserialize(IServiceProvider provider, IContainer container)
			{
				return this.Deserialize(provider, container, false, true, true);
			}

			private ICollection Deserialize(IServiceProvider provider, IContainer container, bool recycleInstances, bool validateRecycledTypes, bool applyDefaults)
			{
				CodeDomComponentSerializationService.CodeDomSerializationStore.PassThroughSerializationManager passThroughSerializationManager = new CodeDomComponentSerializationService.CodeDomSerializationStore.PassThroughSerializationManager(new CodeDomComponentSerializationService.CodeDomSerializationStore.LocalDesignerSerializationManager(this, new CodeDomComponentSerializationService.CodeDomSerializationStore.LocalServices(this, provider)));
				if (container != null)
				{
					passThroughSerializationManager.Manager.Container = container;
				}
				DesignerSerializationManager designerSerializationManager = provider.GetService(typeof(IDesignerSerializationManager)) as DesignerSerializationManager;
				if (designerSerializationManager != null)
				{
					foreach (object obj in designerSerializationManager.SerializationProviders)
					{
						IDesignerSerializationProvider designerSerializationProvider = (IDesignerSerializationProvider)obj;
						((IDesignerSerializationManager)passThroughSerializationManager.Manager).AddSerializationProvider(designerSerializationProvider);
					}
				}
				passThroughSerializationManager.Manager.RecycleInstances = recycleInstances;
				passThroughSerializationManager.Manager.PreserveNames = recycleInstances;
				passThroughSerializationManager.Manager.ValidateRecycledTypes = validateRecycledTypes;
				ArrayList arrayList = null;
				if (this._resourceStream != null)
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					this._resourceStream.Seek(0L, SeekOrigin.Begin);
					Hashtable hashtable = binaryFormatter.Deserialize(this._resourceStream) as Hashtable;
					this._resources = new CodeDomComponentSerializationService.CodeDomSerializationStore.LocalResourceManager(hashtable);
				}
				if (!recycleInstances)
				{
					arrayList = new ArrayList(this._objectNames.Count);
				}
				using (passThroughSerializationManager.Manager.CreateSession())
				{
					if (this._shimObjectNames.Count > 0)
					{
						List<string> shimObjectNames = this._shimObjectNames;
						IDesignerSerializationManager designerSerializationManager2 = passThroughSerializationManager;
						if (designerSerializationManager2 != null && container != null)
						{
							foreach (string text in shimObjectNames)
							{
								object obj2 = container.Components[text];
								if (obj2 != null && designerSerializationManager2.GetInstance(text) == null)
								{
									designerSerializationManager2.SetName(obj2, text);
								}
							}
						}
					}
					CodeDomComponentSerializationService.CodeDomSerializationStore.ComponentListCodeDomSerializer.Instance.Deserialize(passThroughSerializationManager, this._objectState, this._objectNames, applyDefaults);
					if (!recycleInstances)
					{
						foreach (object obj3 in this._objectNames)
						{
							string text2 = (string)obj3;
							object instance = ((IDesignerSerializationManager)passThroughSerializationManager.Manager).GetInstance(text2);
							if (instance != null)
							{
								arrayList.Add(instance);
							}
						}
					}
					this._errors = passThroughSerializationManager.Manager.Errors;
				}
				return arrayList;
			}

			internal void DeserializeTo(IServiceProvider provider, IContainer container, bool validateRecycledTypes, bool applyDefaults)
			{
				this.Deserialize(provider, container, true, validateRecycledTypes, applyDefaults);
			}

			private string GetObjectName(object value)
			{
				IComponent component = value as IComponent;
				if (component != null)
				{
					ISite site = component.Site;
					if (site != null)
					{
						INestedSite nestedSite = site as INestedSite;
						if (nestedSite != null && !string.IsNullOrEmpty(nestedSite.FullName))
						{
							return nestedSite.FullName;
						}
						if (!string.IsNullOrEmpty(site.Name))
						{
							return site.Name;
						}
					}
				}
				string text = Guid.NewGuid().ToString();
				text = text.Replace("-", "_");
				return string.Format(CultureInfo.CurrentCulture, "object_{0}", new object[] { text });
			}

			internal static CodeDomComponentSerializationService.CodeDomSerializationStore Load(Stream stream)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				return (CodeDomComponentSerializationService.CodeDomSerializationStore)binaryFormatter.Deserialize(stream);
			}

			public override void Save(Stream stream)
			{
				this.Close();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(stream, this);
			}

			[Conditional("DEBUG")]
			internal static void Trace(string message, params object[] args)
			{
			}

			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				Hashtable hashtable = null;
				if (this._resources != null)
				{
					hashtable = this._resources.Data;
				}
				info.AddValue("State", this._objectState);
				info.AddValue("Names", this._objectNames);
				info.AddValue("Assemblies", this._assemblies);
				info.AddValue("Resources", hashtable);
				info.AddValue("Shim", this._shimObjectNames);
			}

			private const string _stateKey = "State";

			private const string _nameKey = "Names";

			private const string _assembliesKey = "Assemblies";

			private const string _resourcesKey = "Resources";

			private const string _shimKey = "Shim";

			private const int _stateCode = 0;

			private const int _stateCtx = 1;

			private const int _stateProperties = 2;

			private const int _stateResources = 3;

			private const int _stateEvents = 4;

			private const int _stateModifier = 5;

			private MemoryStream _resourceStream;

			private Hashtable _objects;

			private IServiceProvider _provider;

			private ArrayList _objectNames;

			private Hashtable _objectState;

			private CodeDomComponentSerializationService.CodeDomSerializationStore.LocalResourceManager _resources;

			private AssemblyName[] _assemblies;

			private List<string> _shimObjectNames;

			private ICollection _errors;

			private class ComponentListCodeDomSerializer : CodeDomSerializer
			{
				public override object Deserialize(IDesignerSerializationManager manager, object state)
				{
					throw new NotSupportedException();
				}

				private void PopulateCompleteStatements(object data, string name, CodeStatementCollection completeStatements)
				{
					CodeStatementCollection codeStatementCollection;
					if ((codeStatementCollection = data as CodeStatementCollection) != null)
					{
						completeStatements.AddRange(codeStatementCollection);
						return;
					}
					CodeStatement codeStatement;
					if ((codeStatement = data as CodeStatement) != null)
					{
						completeStatements.Add(codeStatement);
						return;
					}
					CodeExpression codeExpression;
					if ((codeExpression = data as CodeExpression) != null)
					{
						ArrayList arrayList = null;
						if (this._expressions.ContainsKey(name))
						{
							arrayList = this._expressions[name];
						}
						if (arrayList == null)
						{
							arrayList = new ArrayList();
							this._expressions[name] = arrayList;
						}
						arrayList.Add(codeExpression);
					}
				}

				internal void Deserialize(IDesignerSerializationManager manager, IDictionary objectState, IList objectNames, bool applyDefaults)
				{
					CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
					this._expressions = new Dictionary<string, ArrayList>();
					this.applyDefaults = applyDefaults;
					foreach (object obj in objectNames)
					{
						string text = (string)obj;
						object[] array = (object[])objectState[text];
						if (array != null)
						{
							if (array[0] != null)
							{
								this.PopulateCompleteStatements(array[0], text, codeStatementCollection);
							}
							if (array[1] != null)
							{
								this.PopulateCompleteStatements(array[1], text, codeStatementCollection);
							}
						}
					}
					CodeStatementCollection codeStatementCollection2 = new CodeStatementCollection();
					CodeMethodMap codeMethodMap = new CodeMethodMap(codeStatementCollection2, null);
					codeMethodMap.Add(codeStatementCollection);
					codeMethodMap.Combine();
					this._statementsTable = new Hashtable();
					CodeDomSerializerBase.FillStatementTable(manager, this._statementsTable, codeStatementCollection2);
					ArrayList arrayList = new ArrayList(objectNames);
					foreach (object obj2 in this._statementsTable.Keys)
					{
						string text2 = (string)obj2;
						if (!arrayList.Contains(text2))
						{
							arrayList.Add(text2);
						}
					}
					this._objectState = new Hashtable(objectState.Keys.Count);
					foreach (object obj3 in objectState)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
						this._objectState.Add(dictionaryEntry.Key, dictionaryEntry.Value);
					}
					ResolveNameEventHandler resolveNameEventHandler = new ResolveNameEventHandler(this.OnResolveName);
					manager.ResolveName += resolveNameEventHandler;
					try
					{
						foreach (object obj4 in arrayList)
						{
							string text3 = (string)obj4;
							this.ResolveName(manager, text3, true);
						}
					}
					finally
					{
						this._objectState = null;
						manager.ResolveName -= resolveNameEventHandler;
					}
				}

				private void OnResolveName(object sender, ResolveNameEventArgs e)
				{
					if (this._nameResolveGuard.ContainsKey(e.Name))
					{
						return;
					}
					this._nameResolveGuard.Add(e.Name, true);
					try
					{
						IDesignerSerializationManager designerSerializationManager = (IDesignerSerializationManager)sender;
						if (this.ResolveName(designerSerializationManager, e.Name, false))
						{
							e.Value = designerSerializationManager.GetInstance(e.Name);
						}
					}
					finally
					{
						this._nameResolveGuard.Remove(e.Name);
					}
				}

				private void DeserializeDefaultProperties(IDesignerSerializationManager manager, string name, object state)
				{
					if (state != null && this.applyDefaults)
					{
						object instance = manager.GetInstance(name);
						if (instance != null)
						{
							PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(instance);
							string[] array = (string[])state;
							MemberRelationshipService memberRelationshipService = manager.GetService(typeof(MemberRelationshipService)) as MemberRelationshipService;
							foreach (string text in array)
							{
								PropertyDescriptor propertyDescriptor = properties[text];
								if (propertyDescriptor != null && propertyDescriptor.CanResetValue(instance))
								{
									if (memberRelationshipService != null && memberRelationshipService[instance, propertyDescriptor] != MemberRelationship.Empty)
									{
										memberRelationshipService[instance, propertyDescriptor] = MemberRelationship.Empty;
									}
									propertyDescriptor.ResetValue(instance);
								}
							}
						}
					}
				}

				private void DeserializeDesignTimeProperties(IDesignerSerializationManager manager, string name, object state)
				{
					if (state != null)
					{
						object instance = manager.GetInstance(name);
						if (instance != null)
						{
							PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(instance);
							foreach (object obj in ((IDictionary)state))
							{
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
								PropertyDescriptor propertyDescriptor = properties[(string)dictionaryEntry.Key];
								if (propertyDescriptor != null)
								{
									propertyDescriptor.SetValue(instance, dictionaryEntry.Value);
								}
							}
						}
					}
				}

				private IComponent ResolveNestedName(IDesignerSerializationManager manager, string name, ref string outerComponent)
				{
					IComponent component = null;
					if (name != null && manager != null)
					{
						bool flag = true;
						int num = name.IndexOf('.', 0);
						outerComponent = name.Substring(0, num);
						component = manager.GetInstance(outerComponent) as IComponent;
						int num2 = num;
						int num3 = name.IndexOf('.', num + 1);
						while (flag)
						{
							flag = num3 != -1;
							string text = (flag ? name.Substring(num2 + 1, num3) : name.Substring(num2 + 1));
							if (component == null || component.Site == null)
							{
								return null;
							}
							ISite site = component.Site;
							INestedContainer nestedContainer = site.GetService(typeof(INestedContainer)) as INestedContainer;
							if (nestedContainer == null || string.IsNullOrEmpty(text))
							{
								return null;
							}
							component = nestedContainer.Components[text];
							if (flag)
							{
								num2 = num3;
								num3 = name.IndexOf('.', num3 + 1);
							}
						}
					}
					return component;
				}

				private bool ResolveName(IDesignerSerializationManager manager, string name, bool canInvokeManager)
				{
					bool flag = false;
					CodeDomSerializerBase.OrderedCodeStatementCollection orderedCodeStatementCollection = this._statementsTable[name] as CodeDomSerializerBase.OrderedCodeStatementCollection;
					object[] array = (object[])this._objectState[name];
					if (name.IndexOf('.') > 0)
					{
						string text = null;
						IComponent component = this.ResolveNestedName(manager, name, ref text);
						if (component != null && text != null)
						{
							manager.SetName(component, name);
							this.ResolveName(manager, text, canInvokeManager);
						}
					}
					if (orderedCodeStatementCollection != null)
					{
						this._objectState[name] = null;
						this._statementsTable[name] = null;
						string text2 = null;
						foreach (object obj in orderedCodeStatementCollection)
						{
							CodeStatement codeStatement = (CodeStatement)obj;
							CodeVariableDeclarationStatement codeVariableDeclarationStatement;
							if ((codeVariableDeclarationStatement = codeStatement as CodeVariableDeclarationStatement) != null)
							{
								text2 = codeVariableDeclarationStatement.Type.BaseType;
								break;
							}
						}
						if (text2 != null)
						{
							Type type = manager.GetType(text2);
							if (type == null)
							{
								manager.ReportError(new CodeDomSerializerException(SR.GetString("SerializerTypeNotFound", new object[] { text2 }), manager));
								goto IL_01D4;
							}
							if (orderedCodeStatementCollection == null || orderedCodeStatementCollection.Count <= 0)
							{
								goto IL_01D4;
							}
							CodeDomSerializer serializer = base.GetSerializer(manager, type);
							if (serializer == null)
							{
								manager.ReportError(new CodeDomSerializerException(SR.GetString("SerializerNoSerializerForComponent", new object[] { type.FullName }), manager));
								goto IL_01D4;
							}
							try
							{
								object obj2 = serializer.Deserialize(manager, orderedCodeStatementCollection);
								flag = obj2 != null;
								if (flag)
								{
									this._statementsTable[name] = obj2;
								}
								goto IL_01D4;
							}
							catch (Exception ex)
							{
								manager.ReportError(ex);
								goto IL_01D4;
							}
						}
						foreach (object obj3 in orderedCodeStatementCollection)
						{
							CodeStatement codeStatement2 = (CodeStatement)obj3;
							base.DeserializeStatement(manager, codeStatement2);
						}
						flag = true;
						IL_01D4:
						if (array != null && array[2] != null)
						{
							this.DeserializeDefaultProperties(manager, name, array[2]);
						}
						if (array != null && array[3] != null)
						{
							this.DeserializeDesignTimeProperties(manager, name, array[3]);
						}
						if (array != null && array[4] != null)
						{
							this.DeserializeEventResets(manager, name, array[4]);
						}
						if (array != null && array[5] != null)
						{
							CodeDomComponentSerializationService.CodeDomSerializationStore.ComponentListCodeDomSerializer.DeserializeModifier(manager, name, array[5]);
						}
						if (this._expressions.ContainsKey(name))
						{
							ArrayList arrayList = this._expressions[name];
							foreach (object obj4 in arrayList)
							{
								CodeExpression codeExpression = (CodeExpression)obj4;
								base.DeserializeExpression(manager, name, codeExpression);
							}
							this._expressions.Remove(name);
							flag = true;
						}
					}
					else
					{
						flag = this._statementsTable[name] != null;
						if (!flag)
						{
							if (this._expressions.ContainsKey(name))
							{
								ArrayList arrayList2 = this._expressions[name];
								foreach (object obj5 in arrayList2)
								{
									CodeExpression codeExpression2 = (CodeExpression)obj5;
									object obj6 = base.DeserializeExpression(manager, name, codeExpression2);
									if (obj6 != null && !flag && canInvokeManager && manager.GetInstance(name) == null)
									{
										manager.SetName(obj6, name);
										flag = true;
									}
								}
							}
							if (!flag && canInvokeManager)
							{
								flag = manager.GetInstance(name) != null;
							}
							if (flag && array != null && array[2] != null)
							{
								this.DeserializeDefaultProperties(manager, name, array[2]);
							}
							if (flag && array != null && array[3] != null)
							{
								this.DeserializeDesignTimeProperties(manager, name, array[3]);
							}
							if (flag && array != null && array[4] != null)
							{
								this.DeserializeEventResets(manager, name, array[4]);
							}
							if (flag && array != null && array[5] != null)
							{
								CodeDomComponentSerializationService.CodeDomSerializationStore.ComponentListCodeDomSerializer.DeserializeModifier(manager, name, array[5]);
							}
						}
						if (!flag && (flag || canInvokeManager))
						{
							manager.ReportError(new CodeDomSerializerException(SR.GetString("CodeDomComponentSerializationServiceDeserializationError", new object[] { name }), manager));
						}
					}
					return flag;
				}

				private void DeserializeEventResets(IDesignerSerializationManager manager, string name, object state)
				{
					List<string> list = state as List<string>;
					if (list != null && manager != null && !string.IsNullOrEmpty(name))
					{
						IEventBindingService eventBindingService = manager.GetService(typeof(IEventBindingService)) as IEventBindingService;
						object instance = manager.GetInstance(name);
						if (instance != null && eventBindingService != null)
						{
							PropertyDescriptorCollection eventProperties = eventBindingService.GetEventProperties(TypeDescriptor.GetEvents(instance));
							if (eventProperties != null)
							{
								foreach (string text in list)
								{
									PropertyDescriptor propertyDescriptor = eventProperties[text];
									if (propertyDescriptor != null)
									{
										propertyDescriptor.SetValue(instance, null);
									}
								}
							}
						}
					}
				}

				private static void DeserializeModifier(IDesignerSerializationManager manager, string name, object state)
				{
					object instance = manager.GetInstance(name);
					if (instance != null)
					{
						MemberAttributes memberAttributes = (MemberAttributes)state;
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(instance)["Modifiers"];
						if (propertyDescriptor != null)
						{
							propertyDescriptor.SetValue(instance, memberAttributes);
						}
					}
				}

				public override object Serialize(IDesignerSerializationManager manager, object state)
				{
					throw new NotSupportedException();
				}

				internal void SetupVariableReferences(IDesignerSerializationManager manager, IContainer container, IDictionary objectData, IList shimObjectNames)
				{
					foreach (object obj in container.Components)
					{
						IComponent component = (IComponent)obj;
						string componentName = TypeDescriptor.GetComponentName(component);
						if (componentName != null && componentName.Length > 0)
						{
							bool flag = true;
							if (objectData.Contains(component) && ((CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData)objectData[component]).EntireObject)
							{
								flag = false;
							}
							if (flag)
							{
								CodeVariableReferenceExpression codeVariableReferenceExpression = new CodeVariableReferenceExpression(componentName);
								base.SetExpression(manager, component, codeVariableReferenceExpression);
								if (!shimObjectNames.Contains(componentName))
								{
									shimObjectNames.Add(componentName);
								}
								if (component.Site != null)
								{
									INestedContainer nestedContainer = component.Site.GetService(typeof(INestedContainer)) as INestedContainer;
									if (nestedContainer != null && nestedContainer.Components.Count > 0)
									{
										this.SetupVariableReferences(manager, nestedContainer, objectData, shimObjectNames);
									}
								}
							}
						}
					}
				}

				internal void Serialize(IDesignerSerializationManager manager, IDictionary objectData, IDictionary objectState, IList shimObjectNames)
				{
					IContainer container = manager.GetService(typeof(IContainer)) as IContainer;
					if (container != null)
					{
						this.SetupVariableReferences(manager, container, objectData, shimObjectNames);
					}
					StatementContext statementContext = new StatementContext();
					statementContext.StatementCollection.Populate(objectData.Keys);
					manager.Context.Push(statementContext);
					try
					{
						foreach (object obj in objectData.Values)
						{
							CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData objectData2 = (CodeDomComponentSerializationService.CodeDomSerializationStore.ObjectData)obj;
							CodeDomSerializer codeDomSerializer = (CodeDomSerializer)manager.GetSerializer(objectData2.Value.GetType(), typeof(CodeDomSerializer));
							object[] array = new object[6];
							CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
							manager.Context.Push(codeStatementCollection);
							if (codeDomSerializer != null)
							{
								if (objectData2.EntireObject)
								{
									if (!base.IsSerialized(manager, objectData2.Value))
									{
										if (objectData2.Absolute)
										{
											array[0] = codeDomSerializer.SerializeAbsolute(manager, objectData2.Value);
										}
										else
										{
											array[0] = codeDomSerializer.Serialize(manager, objectData2.Value);
										}
										CodeStatementCollection codeStatementCollection2 = statementContext.StatementCollection[objectData2.Value];
										if (codeStatementCollection2 != null && codeStatementCollection2.Count > 0)
										{
											array[1] = codeStatementCollection2;
										}
										if (codeStatementCollection.Count > 0)
										{
											CodeStatementCollection codeStatementCollection3 = array[0] as CodeStatementCollection;
											if (codeStatementCollection3 != null)
											{
												codeStatementCollection3.AddRange(codeStatementCollection);
											}
										}
									}
									else
									{
										array[0] = statementContext.StatementCollection[objectData2.Value];
									}
								}
								else
								{
									CodeStatementCollection codeStatementCollection4 = new CodeStatementCollection();
									foreach (object obj2 in objectData2.Members)
									{
										CodeDomComponentSerializationService.CodeDomSerializationStore.MemberData memberData = (CodeDomComponentSerializationService.CodeDomSerializationStore.MemberData)obj2;
										if (memberData.Member.Attributes.Contains(DesignOnlyAttribute.Yes))
										{
											PropertyDescriptor propertyDescriptor = memberData.Member as PropertyDescriptor;
											if (propertyDescriptor != null && propertyDescriptor.PropertyType.IsSerializable)
											{
												if (array[3] == null)
												{
													array[3] = new Hashtable();
												}
												((Hashtable)array[3])[propertyDescriptor.Name] = propertyDescriptor.GetValue(objectData2.Value);
											}
										}
										else if (memberData.Absolute)
										{
											codeStatementCollection4.AddRange(codeDomSerializer.SerializeMemberAbsolute(manager, objectData2.Value, memberData.Member));
										}
										else
										{
											codeStatementCollection4.AddRange(codeDomSerializer.SerializeMember(manager, objectData2.Value, memberData.Member));
										}
									}
									array[0] = codeStatementCollection4;
								}
							}
							if (codeStatementCollection.Count > 0)
							{
								CodeStatementCollection codeStatementCollection5 = array[0] as CodeStatementCollection;
								if (codeStatementCollection5 != null)
								{
									codeStatementCollection5.AddRange(codeStatementCollection);
								}
							}
							manager.Context.Pop();
							ArrayList arrayList = null;
							List<string> list = null;
							IEventBindingService eventBindingService = manager.GetService(typeof(IEventBindingService)) as IEventBindingService;
							if (!objectData2.EntireObject)
							{
								goto IL_03EA;
							}
							PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(objectData2.Value);
							foreach (object obj3 in properties)
							{
								PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj3;
								if (!propertyDescriptor2.ShouldSerializeValue(objectData2.Value) && !propertyDescriptor2.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden) && (propertyDescriptor2.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content) || !propertyDescriptor2.IsReadOnly))
								{
									if (arrayList == null)
									{
										arrayList = new ArrayList(objectData2.Members.Count);
									}
									arrayList.Add(propertyDescriptor2.Name);
								}
							}
							if (eventBindingService != null)
							{
								PropertyDescriptorCollection eventProperties = eventBindingService.GetEventProperties(TypeDescriptor.GetEvents(objectData2.Value));
								using (IEnumerator enumerator4 = eventProperties.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										object obj4 = enumerator4.Current;
										PropertyDescriptor propertyDescriptor3 = (PropertyDescriptor)obj4;
										if (propertyDescriptor3 != null && !propertyDescriptor3.IsReadOnly && propertyDescriptor3.GetValue(objectData2.Value) == null)
										{
											if (list == null)
											{
												list = new List<string>();
											}
											list.Add(propertyDescriptor3.Name);
										}
									}
									goto IL_049A;
								}
								goto IL_03EA;
							}
							IL_049A:
							PropertyDescriptor propertyDescriptor4 = TypeDescriptor.GetProperties(objectData2.Value)["Modifiers"];
							if (propertyDescriptor4 != null)
							{
								array[5] = propertyDescriptor4.GetValue(objectData2.Value);
							}
							if (arrayList != null)
							{
								array[2] = (string[])arrayList.ToArray(typeof(string));
							}
							if (list != null)
							{
								array[4] = list;
							}
							if (array[0] != null || array[2] != null)
							{
								objectState[objectData2.Name] = array;
								continue;
							}
							continue;
							IL_03EA:
							foreach (object obj5 in objectData2.Members)
							{
								CodeDomComponentSerializationService.CodeDomSerializationStore.MemberData memberData2 = (CodeDomComponentSerializationService.CodeDomSerializationStore.MemberData)obj5;
								PropertyDescriptor propertyDescriptor5 = memberData2.Member as PropertyDescriptor;
								if (propertyDescriptor5 != null && !propertyDescriptor5.ShouldSerializeValue(objectData2.Value))
								{
									if (eventBindingService != null && eventBindingService.GetEvent(propertyDescriptor5) != null)
									{
										if (list == null)
										{
											list = new List<string>();
										}
										list.Add(propertyDescriptor5.Name);
									}
									else
									{
										if (arrayList == null)
										{
											arrayList = new ArrayList(objectData2.Members.Count);
										}
										arrayList.Add(propertyDescriptor5.Name);
									}
								}
							}
							goto IL_049A;
						}
					}
					finally
					{
						manager.Context.Pop();
					}
				}

				internal static CodeDomComponentSerializationService.CodeDomSerializationStore.ComponentListCodeDomSerializer Instance = new CodeDomComponentSerializationService.CodeDomSerializationStore.ComponentListCodeDomSerializer();

				private Hashtable _statementsTable;

				private Dictionary<string, ArrayList> _expressions;

				private Hashtable _objectState;

				private bool applyDefaults = true;

				private Hashtable _nameResolveGuard = new Hashtable();
			}

			private class MemberData
			{
				internal MemberData(MemberDescriptor member, bool absolute)
				{
					this.Member = member;
					this.Absolute = absolute;
				}

				internal MemberDescriptor Member;

				internal bool Absolute;
			}

			private class ObjectData
			{
				internal bool EntireObject
				{
					get
					{
						return this._entireObject;
					}
					set
					{
						if (value && this._members != null)
						{
							this._members.Clear();
						}
						this._entireObject = value;
					}
				}

				internal bool Absolute
				{
					get
					{
						return this._absolute;
					}
					set
					{
						this._absolute = value;
					}
				}

				internal IList Members
				{
					get
					{
						if (this._members == null)
						{
							this._members = new ArrayList();
						}
						return this._members;
					}
				}

				private bool _entireObject;

				private bool _absolute;

				private ArrayList _members;

				internal object Value;

				internal string Name;
			}

			private class LocalResourceManager : ResourceManager, IResourceWriter, IResourceReader, IEnumerable, IDisposable
			{
				internal LocalResourceManager()
				{
				}

				internal LocalResourceManager(Hashtable data)
				{
					this._hashtable = data;
				}

				internal Hashtable Data
				{
					get
					{
						if (this._hashtable == null)
						{
							this._hashtable = new Hashtable();
						}
						return this._hashtable;
					}
				}

				public void AddResource(string name, object value)
				{
					this.Data[name] = value;
				}

				public void AddResource(string name, string value)
				{
					this.Data[name] = value;
				}

				public void AddResource(string name, byte[] value)
				{
					this.Data[name] = value;
				}

				public void Close()
				{
				}

				public void Dispose()
				{
					this.Data.Clear();
				}

				public void Generate()
				{
				}

				public override object GetObject(string name)
				{
					return this.Data[name];
				}

				public override string GetString(string name)
				{
					return this.Data[name] as string;
				}

				public IDictionaryEnumerator GetEnumerator()
				{
					return this.Data.GetEnumerator();
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				private Hashtable _hashtable;
			}

			private class LocalServices : IServiceProvider, IResourceService
			{
				internal LocalServices(CodeDomComponentSerializationService.CodeDomSerializationStore store, IServiceProvider provider)
				{
					this._store = store;
					this._provider = provider;
				}

				IResourceReader IResourceService.GetResourceReader(CultureInfo info)
				{
					return this._store.Resources;
				}

				IResourceWriter IResourceService.GetResourceWriter(CultureInfo info)
				{
					return this._store.Resources;
				}

				object IServiceProvider.GetService(Type serviceType)
				{
					if (serviceType == null)
					{
						throw new ArgumentNullException("serviceType");
					}
					if (serviceType == typeof(IResourceService))
					{
						return this;
					}
					if (this._provider != null)
					{
						return this._provider.GetService(serviceType);
					}
					return null;
				}

				private CodeDomComponentSerializationService.CodeDomSerializationStore _store;

				private IServiceProvider _provider;
			}

			private class PassThroughSerializationManager : IDesignerSerializationManager, IServiceProvider
			{
				public PassThroughSerializationManager(DesignerSerializationManager manager)
				{
					this.manager = manager;
				}

				public DesignerSerializationManager Manager
				{
					get
					{
						return this.manager;
					}
				}

				ContextStack IDesignerSerializationManager.Context
				{
					get
					{
						return ((IDesignerSerializationManager)this.manager).Context;
					}
				}

				PropertyDescriptorCollection IDesignerSerializationManager.Properties
				{
					get
					{
						return ((IDesignerSerializationManager)this.manager).Properties;
					}
				}

				event ResolveNameEventHandler IDesignerSerializationManager.ResolveName
				{
					add
					{
						((IDesignerSerializationManager)this.manager).ResolveName += value;
						this.resolveNameEventHandler = (ResolveNameEventHandler)Delegate.Combine(this.resolveNameEventHandler, value);
					}
					remove
					{
						((IDesignerSerializationManager)this.manager).ResolveName -= value;
						this.resolveNameEventHandler = (ResolveNameEventHandler)Delegate.Remove(this.resolveNameEventHandler, value);
					}
				}

				event EventHandler IDesignerSerializationManager.SerializationComplete
				{
					add
					{
						((IDesignerSerializationManager)this.manager).SerializationComplete += value;
					}
					remove
					{
						((IDesignerSerializationManager)this.manager).SerializationComplete -= value;
					}
				}

				void IDesignerSerializationManager.AddSerializationProvider(IDesignerSerializationProvider provider)
				{
					((IDesignerSerializationManager)this.manager).AddSerializationProvider(provider);
				}

				object IDesignerSerializationManager.CreateInstance(Type type, ICollection arguments, string name, bool addToContainer)
				{
					return ((IDesignerSerializationManager)this.manager).CreateInstance(type, arguments, name, addToContainer);
				}

				object IDesignerSerializationManager.GetInstance(string name)
				{
					object instance = ((IDesignerSerializationManager)this.manager).GetInstance(name);
					if (this.resolveNameEventHandler != null && instance != null && !this.resolved.ContainsKey(name) && this.manager.PreserveNames && this.manager.Container != null && this.manager.Container.Components[name] != null)
					{
						this.resolved[name] = true;
						this.resolveNameEventHandler(this, new ResolveNameEventArgs(name));
					}
					return instance;
				}

				string IDesignerSerializationManager.GetName(object value)
				{
					return ((IDesignerSerializationManager)this.manager).GetName(value);
				}

				object IDesignerSerializationManager.GetSerializer(Type objectType, Type serializerType)
				{
					return ((IDesignerSerializationManager)this.manager).GetSerializer(objectType, serializerType);
				}

				Type IDesignerSerializationManager.GetType(string typeName)
				{
					return ((IDesignerSerializationManager)this.manager).GetType(typeName);
				}

				void IDesignerSerializationManager.RemoveSerializationProvider(IDesignerSerializationProvider provider)
				{
					((IDesignerSerializationManager)this.manager).RemoveSerializationProvider(provider);
				}

				void IDesignerSerializationManager.ReportError(object errorInformation)
				{
					((IDesignerSerializationManager)this.manager).ReportError(errorInformation);
				}

				void IDesignerSerializationManager.SetName(object instance, string name)
				{
					((IDesignerSerializationManager)this.manager).SetName(instance, name);
				}

				object IServiceProvider.GetService(Type serviceType)
				{
					return ((IServiceProvider)this.manager).GetService(serviceType);
				}

				private Hashtable resolved = new Hashtable();

				private DesignerSerializationManager manager;

				private ResolveNameEventHandler resolveNameEventHandler;
			}

			private class LocalDesignerSerializationManager : DesignerSerializationManager
			{
				internal LocalDesignerSerializationManager(CodeDomComponentSerializationService.CodeDomSerializationStore store, IServiceProvider provider)
					: base(provider)
				{
					this._store = store;
				}

				protected override object CreateInstance(Type type, ICollection arguments, string name, bool addToContainer)
				{
					if (typeof(ResourceManager).IsAssignableFrom(type))
					{
						return this._store.Resources;
					}
					return base.CreateInstance(type, arguments, name, addToContainer);
				}

				private bool? TypeResolutionAvailable
				{
					get
					{
						if (this._typeSvcAvailable == null)
						{
							this._typeSvcAvailable = new bool?(this.GetService(typeof(ITypeResolutionService)) != null);
						}
						return new bool?(this._typeSvcAvailable.Value);
					}
				}

				protected override Type GetType(string name)
				{
					Type type = base.GetType(name);
					if (((type == null) ? (!this.TypeResolutionAvailable) : new bool?(false)).GetValueOrDefault())
					{
						AssemblyName[] assemblyNames = this._store.AssemblyNames;
						foreach (AssemblyName assemblyName in assemblyNames)
						{
							Assembly assembly = Assembly.Load(assemblyName);
							if (assembly != null)
							{
								type = assembly.GetType(name);
								if (type != null)
								{
									break;
								}
							}
						}
						if (type == null)
						{
							foreach (AssemblyName assemblyName2 in assemblyNames)
							{
								Assembly assembly2 = Assembly.Load(assemblyName2);
								if (assembly2 != null)
								{
									foreach (AssemblyName assemblyName3 in assembly2.GetReferencedAssemblies())
									{
										Assembly assembly3 = Assembly.Load(assemblyName3);
										if (assembly3 != null)
										{
											type = assembly3.GetType(name);
											if (type != null)
											{
												break;
											}
										}
									}
									if (type != null)
									{
										break;
									}
								}
							}
						}
					}
					return type;
				}

				private CodeDomComponentSerializationService.CodeDomSerializationStore _store;

				private bool? _typeSvcAvailable = null;
			}
		}
	}
}
