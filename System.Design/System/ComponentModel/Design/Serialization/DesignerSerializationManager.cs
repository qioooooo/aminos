using System;
using System.Collections;
using System.Design;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace System.ComponentModel.Design.Serialization
{
	public class DesignerSerializationManager : IDesignerSerializationManager, IServiceProvider
	{
		public DesignerSerializationManager()
		{
			this.preserveNames = true;
			this.validateRecycledTypes = true;
		}

		public DesignerSerializationManager(IServiceProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.provider = provider;
			this.preserveNames = true;
			this.validateRecycledTypes = true;
		}

		public IContainer Container
		{
			get
			{
				if (this.container == null)
				{
					IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
					if (designerHost != null)
					{
						this.container = designerHost.Container;
					}
				}
				return this.container;
			}
			set
			{
				this.CheckNoSession();
				this.container = value;
			}
		}

		public IList Errors
		{
			get
			{
				this.CheckSession();
				if (this.errorList == null)
				{
					this.errorList = new ArrayList();
				}
				return this.errorList;
			}
		}

		public bool PreserveNames
		{
			get
			{
				return this.preserveNames;
			}
			set
			{
				this.CheckNoSession();
				this.preserveNames = value;
			}
		}

		public object PropertyProvider
		{
			get
			{
				return this.propertyProvider;
			}
			set
			{
				if (this.propertyProvider != value)
				{
					this.propertyProvider = value;
					this.properties = null;
				}
			}
		}

		public bool RecycleInstances
		{
			get
			{
				return this.recycleInstances;
			}
			set
			{
				this.CheckNoSession();
				this.recycleInstances = value;
			}
		}

		public bool ValidateRecycledTypes
		{
			get
			{
				return this.validateRecycledTypes;
			}
			set
			{
				this.CheckNoSession();
				this.validateRecycledTypes = value;
			}
		}

		public event EventHandler SessionCreated
		{
			add
			{
				this.sessionCreatedEventHandler = (EventHandler)Delegate.Combine(this.sessionCreatedEventHandler, value);
			}
			remove
			{
				this.sessionCreatedEventHandler = (EventHandler)Delegate.Remove(this.sessionCreatedEventHandler, value);
			}
		}

		public event EventHandler SessionDisposed
		{
			add
			{
				this.sessionDisposedEventHandler = (EventHandler)Delegate.Combine(this.sessionDisposedEventHandler, value);
			}
			remove
			{
				this.sessionDisposedEventHandler = (EventHandler)Delegate.Remove(this.sessionDisposedEventHandler, value);
			}
		}

		private void CheckNoSession()
		{
			if (this.session != null)
			{
				throw new InvalidOperationException(SR.GetString("SerializationManagerWithinSession"));
			}
		}

		private void CheckSession()
		{
			if (this.session == null)
			{
				throw new InvalidOperationException(SR.GetString("SerializationManagerNoSession"));
			}
		}

		protected virtual object CreateInstance(Type type, ICollection arguments, string name, bool addToContainer)
		{
			object[] array = null;
			if (arguments != null && arguments.Count > 0)
			{
				array = new object[arguments.Count];
				arguments.CopyTo(array, 0);
			}
			object obj = null;
			if (this.RecycleInstances && name != null)
			{
				if (this.instancesByName != null)
				{
					obj = this.instancesByName[name];
				}
				if (obj == null && addToContainer && this.Container != null)
				{
					obj = this.Container.Components[name];
				}
				if (obj != null && this.ValidateRecycledTypes && obj.GetType() != type)
				{
					obj = null;
				}
			}
			if (obj == null && addToContainer && typeof(IComponent).IsAssignableFrom(type) && (array == null || array.Length == 0 || (array.Length == 1 && array[0] == this.Container)))
			{
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null && designerHost.Container == this.Container)
				{
					bool flag = false;
					if (!this.PreserveNames && name != null && this.Container.Components[name] != null)
					{
						flag = true;
					}
					if (name == null || flag)
					{
						obj = designerHost.CreateComponent(type);
					}
					else
					{
						obj = designerHost.CreateComponent(type, name);
					}
				}
			}
			if (obj == null)
			{
				try
				{
					try
					{
						obj = TypeDescriptor.CreateInstance(this.provider, type, null, array);
					}
					catch (MissingMethodException ex)
					{
						Type[] array2 = new Type[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] != null)
							{
								array2[i] = array[i].GetType();
							}
						}
						object[] array3 = new object[array.Length];
						foreach (ConstructorInfo constructorInfo in TypeDescriptor.GetReflectionType(type).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance))
						{
							ParameterInfo[] parameters = constructorInfo.GetParameters();
							if (parameters != null && parameters.Length == array2.Length)
							{
								bool flag2 = true;
								for (int k = 0; k < array2.Length; k++)
								{
									if (array2[k] != null && !parameters[k].ParameterType.IsAssignableFrom(array2[k]))
									{
										if (array[k] is IConvertible)
										{
											try
											{
												array3[k] = ((IConvertible)array[k]).ToType(parameters[k].ParameterType, null);
												goto IL_020C;
											}
											catch (InvalidCastException)
											{
											}
										}
										flag2 = false;
										break;
									}
									array3[k] = array[k];
									IL_020C:;
								}
								if (flag2)
								{
									obj = TypeDescriptor.CreateInstance(this.provider, type, null, array3);
									break;
								}
							}
						}
						if (obj == null)
						{
							throw ex;
						}
					}
				}
				catch (MissingMethodException)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (object obj2 in array)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(", ");
						}
						if (obj2 != null)
						{
							stringBuilder.Append(obj2.GetType().Name);
						}
						else
						{
							stringBuilder.Append("null");
						}
					}
					throw new SerializationException(SR.GetString("SerializationManagerNoMatchingCtor", new object[]
					{
						type.FullName,
						stringBuilder.ToString()
					}))
					{
						HelpLink = "SerializationManagerNoMatchingCtor"
					};
				}
				if (addToContainer && obj is IComponent && this.Container != null)
				{
					bool flag3 = false;
					if (!this.PreserveNames && name != null && this.Container.Components[name] != null)
					{
						flag3 = true;
					}
					if (name == null || flag3)
					{
						this.Container.Add((IComponent)obj);
					}
					else
					{
						this.Container.Add((IComponent)obj, name);
					}
				}
			}
			return obj;
		}

		public IDisposable CreateSession()
		{
			if (this.session != null)
			{
				throw new InvalidOperationException(SR.GetString("SerializationManagerAreadyInSession"));
			}
			this.session = new DesignerSerializationManager.SerializationSession(this);
			this.OnSessionCreated(EventArgs.Empty);
			return this.session;
		}

		public object GetSerializer(Type objectType, Type serializerType)
		{
			if (serializerType == null)
			{
				throw new ArgumentNullException("serializerType");
			}
			object obj = null;
			if (objectType != null)
			{
				if (this.serializers != null)
				{
					obj = this.serializers[objectType];
					if (obj != null && !serializerType.IsAssignableFrom(obj.GetType()))
					{
						obj = null;
					}
				}
				if (obj == null)
				{
					AttributeCollection attributes = TypeDescriptor.GetAttributes(objectType);
					foreach (object obj2 in attributes)
					{
						Attribute attribute = (Attribute)obj2;
						if (attribute is DesignerSerializerAttribute)
						{
							DesignerSerializerAttribute designerSerializerAttribute = (DesignerSerializerAttribute)attribute;
							string serializerBaseTypeName = designerSerializerAttribute.SerializerBaseTypeName;
							if (serializerBaseTypeName != null)
							{
								Type type = this.GetType(serializerBaseTypeName);
								if (type == serializerType && designerSerializerAttribute.SerializerTypeName != null && designerSerializerAttribute.SerializerTypeName.Length > 0)
								{
									Type type2 = this.GetType(designerSerializerAttribute.SerializerTypeName);
									if (type2 != null)
									{
										obj = Activator.CreateInstance(type2, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
										break;
									}
								}
							}
						}
					}
					if (obj != null && this.session != null)
					{
						if (this.serializers == null)
						{
							this.serializers = new Hashtable();
						}
						this.serializers[objectType] = obj;
					}
				}
			}
			if (this.defaultProviderTable == null || !this.defaultProviderTable.ContainsKey(serializerType))
			{
				Type type3 = null;
				DefaultSerializationProviderAttribute defaultSerializationProviderAttribute = (DefaultSerializationProviderAttribute)TypeDescriptor.GetAttributes(serializerType)[typeof(DefaultSerializationProviderAttribute)];
				if (defaultSerializationProviderAttribute != null)
				{
					type3 = this.GetType(defaultSerializationProviderAttribute.ProviderTypeName);
					if (type3 != null && typeof(IDesignerSerializationProvider).IsAssignableFrom(type3))
					{
						IDesignerSerializationProvider designerSerializationProvider = (IDesignerSerializationProvider)Activator.CreateInstance(type3, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
						((IDesignerSerializationManager)this).AddSerializationProvider(designerSerializationProvider);
					}
				}
				if (this.defaultProviderTable == null)
				{
					this.defaultProviderTable = new Hashtable();
				}
				this.defaultProviderTable[serializerType] = type3;
			}
			if (this.designerSerializationProviders != null)
			{
				bool flag = true;
				int num = 0;
				while (flag && num < this.designerSerializationProviders.Count)
				{
					flag = false;
					foreach (object obj3 in this.designerSerializationProviders)
					{
						IDesignerSerializationProvider designerSerializationProvider2 = (IDesignerSerializationProvider)obj3;
						object serializer = designerSerializationProvider2.GetSerializer(this, obj, objectType, serializerType);
						if (serializer != null)
						{
							flag = obj != serializer;
							obj = serializer;
						}
					}
					num++;
				}
			}
			return obj;
		}

		protected virtual object GetService(Type serviceType)
		{
			if (serviceType == typeof(IContainer))
			{
				return this.Container;
			}
			if (this.provider != null)
			{
				return this.provider.GetService(serviceType);
			}
			return null;
		}

		protected virtual Type GetType(string typeName)
		{
			if (this.typeResolver == null && !this.searchedTypeResolver)
			{
				this.typeResolver = this.GetService(typeof(ITypeResolutionService)) as ITypeResolutionService;
				this.searchedTypeResolver = true;
			}
			if (this.typeResolver == null)
			{
				return Type.GetType(typeName);
			}
			return this.typeResolver.GetType(typeName);
		}

		protected virtual void OnResolveName(ResolveNameEventArgs e)
		{
			if (this.resolveNameEventHandler != null)
			{
				this.resolveNameEventHandler(this, e);
			}
		}

		protected virtual void OnSessionCreated(EventArgs e)
		{
			if (this.sessionCreatedEventHandler != null)
			{
				this.sessionCreatedEventHandler(this, e);
			}
		}

		protected virtual void OnSessionDisposed(EventArgs e)
		{
			try
			{
				try
				{
					if (this.sessionDisposedEventHandler != null)
					{
						this.sessionDisposedEventHandler(this, e);
					}
				}
				finally
				{
					if (this.serializationCompleteEventHandler != null)
					{
						this.serializationCompleteEventHandler(this, EventArgs.Empty);
					}
				}
			}
			finally
			{
				this.resolveNameEventHandler = null;
				this.serializationCompleteEventHandler = null;
				this.instancesByName = null;
				this.namesByInstance = null;
				this.serializers = null;
				this.contextStack = null;
				this.errorList = null;
				this.session = null;
			}
		}

		private PropertyDescriptor WrapProperty(PropertyDescriptor property, object owner)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			return new DesignerSerializationManager.WrappedPropertyDescriptor(property, owner);
		}

		ContextStack IDesignerSerializationManager.Context
		{
			get
			{
				if (this.contextStack == null)
				{
					this.CheckSession();
					this.contextStack = new ContextStack();
				}
				return this.contextStack;
			}
		}

		PropertyDescriptorCollection IDesignerSerializationManager.Properties
		{
			get
			{
				if (this.properties == null)
				{
					object obj = this.PropertyProvider;
					PropertyDescriptor[] array;
					if (obj == null)
					{
						array = new PropertyDescriptor[0];
					}
					else
					{
						PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(obj);
						array = new PropertyDescriptor[propertyDescriptorCollection.Count];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = this.WrapProperty(propertyDescriptorCollection[i], obj);
						}
					}
					this.properties = new PropertyDescriptorCollection(array);
				}
				return this.properties;
			}
		}

		event ResolveNameEventHandler IDesignerSerializationManager.ResolveName
		{
			add
			{
				this.CheckSession();
				this.resolveNameEventHandler = (ResolveNameEventHandler)Delegate.Combine(this.resolveNameEventHandler, value);
			}
			remove
			{
				this.resolveNameEventHandler = (ResolveNameEventHandler)Delegate.Remove(this.resolveNameEventHandler, value);
			}
		}

		event EventHandler IDesignerSerializationManager.SerializationComplete
		{
			add
			{
				this.CheckSession();
				this.serializationCompleteEventHandler = (EventHandler)Delegate.Combine(this.serializationCompleteEventHandler, value);
			}
			remove
			{
				this.serializationCompleteEventHandler = (EventHandler)Delegate.Remove(this.serializationCompleteEventHandler, value);
			}
		}

		void IDesignerSerializationManager.AddSerializationProvider(IDesignerSerializationProvider provider)
		{
			if (this.designerSerializationProviders == null)
			{
				this.designerSerializationProviders = new ArrayList();
			}
			if (!this.designerSerializationProviders.Contains(provider))
			{
				this.designerSerializationProviders.Add(provider);
			}
		}

		object IDesignerSerializationManager.CreateInstance(Type type, ICollection arguments, string name, bool addToContainer)
		{
			this.CheckSession();
			if (name != null && this.instancesByName != null && this.instancesByName.ContainsKey(name))
			{
				throw new SerializationException(SR.GetString("SerializationManagerDuplicateComponentDecl", new object[] { name }))
				{
					HelpLink = "SerializationManagerDuplicateComponentDecl"
				};
			}
			object obj = this.CreateInstance(type, arguments, name, addToContainer);
			if (name != null && (!(obj is IComponent) || !this.RecycleInstances))
			{
				if (this.instancesByName == null)
				{
					this.instancesByName = new Hashtable();
					this.namesByInstance = new Hashtable(new DesignerSerializationManager.ReferenceComparer());
				}
				this.instancesByName[name] = obj;
				this.namesByInstance[obj] = name;
			}
			return obj;
		}

		object IDesignerSerializationManager.GetInstance(string name)
		{
			object obj = null;
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.CheckSession();
			if (this.instancesByName != null)
			{
				obj = this.instancesByName[name];
			}
			if (obj == null && this.PreserveNames && this.Container != null)
			{
				obj = this.Container.Components[name];
			}
			if (obj == null)
			{
				ResolveNameEventArgs resolveNameEventArgs = new ResolveNameEventArgs(name);
				this.OnResolveName(resolveNameEventArgs);
				obj = resolveNameEventArgs.Value;
			}
			return obj;
		}

		string IDesignerSerializationManager.GetName(object value)
		{
			string text = null;
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.CheckSession();
			if (this.namesByInstance != null)
			{
				text = (string)this.namesByInstance[value];
			}
			if (text == null && value is IComponent)
			{
				ISite site = ((IComponent)value).Site;
				if (site != null)
				{
					INestedSite nestedSite = site as INestedSite;
					if (nestedSite != null)
					{
						text = nestedSite.FullName;
					}
					else
					{
						text = site.Name;
					}
				}
			}
			return text;
		}

		object IDesignerSerializationManager.GetSerializer(Type objectType, Type serializerType)
		{
			return this.GetSerializer(objectType, serializerType);
		}

		Type IDesignerSerializationManager.GetType(string typeName)
		{
			this.CheckSession();
			Type type = null;
			while (type == null)
			{
				type = this.GetType(typeName);
				if (type == null && typeName != null && typeName.Length > 0)
				{
					int num = typeName.LastIndexOf('.');
					if (num == -1 || num == typeName.Length - 1)
					{
						break;
					}
					typeName = typeName.Substring(0, num) + "+" + typeName.Substring(num + 1, typeName.Length - num - 1);
				}
			}
			return type;
		}

		void IDesignerSerializationManager.RemoveSerializationProvider(IDesignerSerializationProvider provider)
		{
			if (this.designerSerializationProviders != null)
			{
				this.designerSerializationProviders.Remove(provider);
			}
		}

		void IDesignerSerializationManager.ReportError(object errorInformation)
		{
			this.CheckSession();
			if (errorInformation != null)
			{
				this.Errors.Add(errorInformation);
			}
		}

		internal ArrayList SerializationProviders
		{
			get
			{
				if (this.designerSerializationProviders == null)
				{
					return new ArrayList();
				}
				return this.designerSerializationProviders.Clone() as ArrayList;
			}
		}

		void IDesignerSerializationManager.SetName(object instance, string name)
		{
			this.CheckSession();
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.instancesByName == null)
			{
				this.instancesByName = new Hashtable();
				this.namesByInstance = new Hashtable(new DesignerSerializationManager.ReferenceComparer());
			}
			if (this.instancesByName[name] != null)
			{
				throw new ArgumentException(SR.GetString("SerializationManagerNameInUse", new object[] { name }));
			}
			if (this.namesByInstance[instance] != null)
			{
				throw new ArgumentException(SR.GetString("SerializationManagerObjectHasName", new object[]
				{
					name,
					(string)this.namesByInstance[instance]
				}));
			}
			this.instancesByName[name] = instance;
			this.namesByInstance[instance] = name;
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}

		private IServiceProvider provider;

		private ITypeResolutionService typeResolver;

		private bool searchedTypeResolver;

		private bool recycleInstances;

		private bool validateRecycledTypes;

		private bool preserveNames;

		private IContainer container;

		private IDisposable session;

		private ResolveNameEventHandler resolveNameEventHandler;

		private EventHandler serializationCompleteEventHandler;

		private EventHandler sessionCreatedEventHandler;

		private EventHandler sessionDisposedEventHandler;

		private ArrayList designerSerializationProviders;

		private Hashtable defaultProviderTable;

		private Hashtable instancesByName;

		private Hashtable namesByInstance;

		private Hashtable serializers;

		private ArrayList errorList;

		private ContextStack contextStack;

		private PropertyDescriptorCollection properties;

		private object propertyProvider;

		private sealed class SerializationSession : IDisposable
		{
			internal SerializationSession(DesignerSerializationManager serializationManager)
			{
				this.serializationManager = serializationManager;
			}

			public void Dispose()
			{
				this.serializationManager.OnSessionDisposed(EventArgs.Empty);
			}

			private DesignerSerializationManager serializationManager;
		}

		private sealed class ReferenceComparer : IEqualityComparer
		{
			bool IEqualityComparer.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			int IEqualityComparer.GetHashCode(object x)
			{
				if (x != null)
				{
					return x.GetHashCode();
				}
				return 0;
			}
		}

		private sealed class WrappedPropertyDescriptor : PropertyDescriptor
		{
			internal WrappedPropertyDescriptor(PropertyDescriptor property, object target)
				: base(property.Name, null)
			{
				this.property = property;
				this.target = target;
			}

			public override AttributeCollection Attributes
			{
				get
				{
					return this.property.Attributes;
				}
			}

			public override Type ComponentType
			{
				get
				{
					return this.property.ComponentType;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return this.property.IsReadOnly;
				}
			}

			public override Type PropertyType
			{
				get
				{
					return this.property.PropertyType;
				}
			}

			public override bool CanResetValue(object component)
			{
				return this.property.CanResetValue(this.target);
			}

			public override object GetValue(object component)
			{
				return this.property.GetValue(this.target);
			}

			public override void ResetValue(object component)
			{
				this.property.ResetValue(this.target);
			}

			public override void SetValue(object component, object value)
			{
				this.property.SetValue(this.target, value);
			}

			public override bool ShouldSerializeValue(object component)
			{
				return this.property.ShouldSerializeValue(this.target);
			}

			private object target;

			private PropertyDescriptor property;
		}
	}
}
