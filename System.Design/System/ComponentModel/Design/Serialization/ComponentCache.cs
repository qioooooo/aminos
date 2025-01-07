using System;
using System.CodeDom;
using System.Collections.Generic;

namespace System.ComponentModel.Design.Serialization
{
	internal class ComponentCache : IDisposable
	{
		internal ComponentCache(IDesignerSerializationManager manager)
		{
			this.serManager = manager;
			IComponentChangeService componentChangeService = (IComponentChangeService)manager.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentChanging += this.OnComponentChanging;
				componentChangeService.ComponentChanged += this.OnComponentChanged;
				componentChangeService.ComponentRemoving += this.OnComponentRemove;
				componentChangeService.ComponentRemoved += this.OnComponentRemove;
				componentChangeService.ComponentRename += this.OnComponentRename;
			}
			DesignerOptionService designerOptionService = manager.GetService(typeof(DesignerOptionService)) as DesignerOptionService;
			object obj = null;
			if (designerOptionService != null)
			{
				PropertyDescriptor propertyDescriptor = designerOptionService.Options.Properties["UseOptimizedCodeGeneration"];
				if (propertyDescriptor != null)
				{
					obj = propertyDescriptor.GetValue(null);
				}
				if (obj != null && obj is bool)
				{
					this.enabled = (bool)obj;
				}
			}
		}

		internal bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		internal ComponentCache.Entry this[object component]
		{
			get
			{
				if (component == null)
				{
					throw new ArgumentNullException("component");
				}
				ComponentCache.Entry entry;
				if (this.cache != null && this.cache.TryGetValue(component, out entry) && entry != null && entry.Valid && this.Enabled)
				{
					return entry;
				}
				return null;
			}
			set
			{
				if (this.cache == null && this.Enabled)
				{
					this.cache = new Dictionary<object, ComponentCache.Entry>();
				}
				if (this.cache != null && component is IComponent)
				{
					if (value != null && value.Component == null)
					{
						value.Component = component;
					}
					this.cache[component] = value;
				}
			}
		}

		internal ComponentCache.Entry GetEntryAll(object component)
		{
			ComponentCache.Entry entry = null;
			if (this.cache != null && this.cache.TryGetValue(component, out entry))
			{
				return entry;
			}
			return null;
		}

		internal bool ContainsLocalName(string name)
		{
			if (this.cache == null)
			{
				return false;
			}
			foreach (KeyValuePair<object, ComponentCache.Entry> keyValuePair in this.cache)
			{
				List<string> localNames = keyValuePair.Value.LocalNames;
				if (localNames != null && localNames.Contains(name))
				{
					return true;
				}
			}
			return false;
		}

		public void Dispose()
		{
			if (this.serManager != null)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.serManager.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentChanging -= this.OnComponentChanging;
					componentChangeService.ComponentChanged -= this.OnComponentChanged;
					componentChangeService.ComponentRemoving -= this.OnComponentRemove;
					componentChangeService.ComponentRemoved -= this.OnComponentRemove;
					componentChangeService.ComponentRename -= this.OnComponentRename;
				}
			}
		}

		private void OnComponentRename(object source, ComponentRenameEventArgs args)
		{
			if (this.cache != null)
			{
				this.cache.Clear();
			}
		}

		private void OnComponentChanging(object source, ComponentChangingEventArgs ce)
		{
			if (this.cache != null)
			{
				if (ce.Component != null)
				{
					this.RemoveEntry(ce.Component);
					if (!(ce.Component is IComponent) && this.serManager != null)
					{
						IReferenceService referenceService = this.serManager.GetService(typeof(IReferenceService)) as IReferenceService;
						if (referenceService != null)
						{
							IComponent component = referenceService.GetComponent(ce.Component);
							if (component != null)
							{
								this.RemoveEntry(component);
								return;
							}
							this.cache.Clear();
							return;
						}
					}
				}
				else
				{
					this.cache.Clear();
				}
			}
		}

		private void OnComponentChanged(object source, ComponentChangedEventArgs ce)
		{
			if (this.cache != null)
			{
				if (ce.Component != null)
				{
					this.RemoveEntry(ce.Component);
					if (!(ce.Component is IComponent) && this.serManager != null)
					{
						IReferenceService referenceService = this.serManager.GetService(typeof(IReferenceService)) as IReferenceService;
						if (referenceService != null)
						{
							IComponent component = referenceService.GetComponent(ce.Component);
							if (component != null)
							{
								this.RemoveEntry(component);
								return;
							}
							this.cache.Clear();
							return;
						}
					}
				}
				else
				{
					this.cache.Clear();
				}
			}
		}

		private void OnComponentRemove(object source, ComponentEventArgs ce)
		{
			if (this.cache != null)
			{
				if (ce.Component != null && !(ce.Component is IExtenderProvider))
				{
					this.RemoveEntry(ce.Component);
					return;
				}
				this.cache.Clear();
			}
		}

		internal void RemoveEntry(object component)
		{
			ComponentCache.Entry entry = null;
			if (this.cache != null && this.cache.TryGetValue(component, out entry))
			{
				if (entry.Tracking)
				{
					this.cache.Clear();
					return;
				}
				this.cache.Remove(component);
				if (entry.Dependencies != null)
				{
					foreach (object obj in entry.Dependencies)
					{
						this.RemoveEntry(obj);
					}
				}
			}
		}

		private Dictionary<object, ComponentCache.Entry> cache;

		private IDesignerSerializationManager serManager;

		private bool enabled = true;

		internal struct ResourceEntry
		{
			public bool ForceInvariant;

			public bool EnsureInvariant;

			public bool ShouldSerializeValue;

			public string Name;

			public object Value;

			public PropertyDescriptor PropertyDescriptor;

			public ExpressionContext ExpressionContext;
		}

		internal sealed class Entry
		{
			internal Entry(ComponentCache cache)
			{
				this.cache = cache;
				this.valid = true;
			}

			public ICollection<ComponentCache.ResourceEntry> Metadata
			{
				get
				{
					return this.metadata;
				}
			}

			public ICollection<ComponentCache.ResourceEntry> Resources
			{
				get
				{
					return this.resources;
				}
			}

			public List<object> Dependencies
			{
				get
				{
					return this.dependencies;
				}
			}

			internal List<string> LocalNames
			{
				get
				{
					return this.localNames;
				}
			}

			internal bool Valid
			{
				get
				{
					return this.valid;
				}
				set
				{
					this.valid = value;
				}
			}

			internal bool Tracking
			{
				get
				{
					return this.tracking;
				}
				set
				{
					this.tracking = value;
				}
			}

			internal void AddLocalName(string name)
			{
				if (this.localNames == null)
				{
					this.localNames = new List<string>();
				}
				this.localNames.Add(name);
			}

			public void AddDependency(object dep)
			{
				if (this.dependencies == null)
				{
					this.dependencies = new List<object>();
				}
				if (!this.dependencies.Contains(dep))
				{
					this.dependencies.Add(dep);
				}
			}

			public void AddMetadata(ComponentCache.ResourceEntry re)
			{
				if (this.metadata == null)
				{
					this.metadata = new List<ComponentCache.ResourceEntry>();
				}
				this.metadata.Add(re);
			}

			public void AddResource(ComponentCache.ResourceEntry re)
			{
				if (this.resources == null)
				{
					this.resources = new List<ComponentCache.ResourceEntry>();
				}
				this.resources.Add(re);
			}

			private ComponentCache cache;

			private List<object> dependencies;

			private List<string> localNames;

			private List<ComponentCache.ResourceEntry> resources;

			private List<ComponentCache.ResourceEntry> metadata;

			private bool valid;

			private bool tracking;

			public object Component;

			public CodeStatementCollection Statements;
		}
	}
}
