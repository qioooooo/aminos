using System;
using System.CodeDom;
using System.Collections.Generic;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000153 RID: 339
	internal class ComponentCache : IDisposable
	{
		// Token: 0x06000CD7 RID: 3287 RVA: 0x00031E54 File Offset: 0x00030E54
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

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00031F3B File Offset: 0x00030F3B
		internal bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x170001F5 RID: 501
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

		// Token: 0x06000CDB RID: 3291 RVA: 0x00031FE8 File Offset: 0x00030FE8
		internal ComponentCache.Entry GetEntryAll(object component)
		{
			ComponentCache.Entry entry = null;
			if (this.cache != null && this.cache.TryGetValue(component, out entry))
			{
				return entry;
			}
			return null;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00032014 File Offset: 0x00031014
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

		// Token: 0x06000CDD RID: 3293 RVA: 0x0003208C File Offset: 0x0003108C
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

		// Token: 0x06000CDE RID: 3294 RVA: 0x00032119 File Offset: 0x00031119
		private void OnComponentRename(object source, ComponentRenameEventArgs args)
		{
			if (this.cache != null)
			{
				this.cache.Clear();
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00032130 File Offset: 0x00031130
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

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000321BC File Offset: 0x000311BC
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

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00032247 File Offset: 0x00031247
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

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00032280 File Offset: 0x00031280
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

		// Token: 0x04000EC8 RID: 3784
		private Dictionary<object, ComponentCache.Entry> cache;

		// Token: 0x04000EC9 RID: 3785
		private IDesignerSerializationManager serManager;

		// Token: 0x04000ECA RID: 3786
		private bool enabled = true;

		// Token: 0x02000154 RID: 340
		internal struct ResourceEntry
		{
			// Token: 0x04000ECB RID: 3787
			public bool ForceInvariant;

			// Token: 0x04000ECC RID: 3788
			public bool EnsureInvariant;

			// Token: 0x04000ECD RID: 3789
			public bool ShouldSerializeValue;

			// Token: 0x04000ECE RID: 3790
			public string Name;

			// Token: 0x04000ECF RID: 3791
			public object Value;

			// Token: 0x04000ED0 RID: 3792
			public PropertyDescriptor PropertyDescriptor;

			// Token: 0x04000ED1 RID: 3793
			public ExpressionContext ExpressionContext;
		}

		// Token: 0x02000155 RID: 341
		internal sealed class Entry
		{
			// Token: 0x06000CE3 RID: 3299 RVA: 0x00032318 File Offset: 0x00031318
			internal Entry(ComponentCache cache)
			{
				this.cache = cache;
				this.valid = true;
			}

			// Token: 0x170001F6 RID: 502
			// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0003232E File Offset: 0x0003132E
			public ICollection<ComponentCache.ResourceEntry> Metadata
			{
				get
				{
					return this.metadata;
				}
			}

			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00032336 File Offset: 0x00031336
			public ICollection<ComponentCache.ResourceEntry> Resources
			{
				get
				{
					return this.resources;
				}
			}

			// Token: 0x170001F8 RID: 504
			// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0003233E File Offset: 0x0003133E
			public List<object> Dependencies
			{
				get
				{
					return this.dependencies;
				}
			}

			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00032346 File Offset: 0x00031346
			internal List<string> LocalNames
			{
				get
				{
					return this.localNames;
				}
			}

			// Token: 0x170001FA RID: 506
			// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0003234E File Offset: 0x0003134E
			// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x00032356 File Offset: 0x00031356
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

			// Token: 0x170001FB RID: 507
			// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0003235F File Offset: 0x0003135F
			// (set) Token: 0x06000CEB RID: 3307 RVA: 0x00032367 File Offset: 0x00031367
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

			// Token: 0x06000CEC RID: 3308 RVA: 0x00032370 File Offset: 0x00031370
			internal void AddLocalName(string name)
			{
				if (this.localNames == null)
				{
					this.localNames = new List<string>();
				}
				this.localNames.Add(name);
			}

			// Token: 0x06000CED RID: 3309 RVA: 0x00032391 File Offset: 0x00031391
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

			// Token: 0x06000CEE RID: 3310 RVA: 0x000323C0 File Offset: 0x000313C0
			public void AddMetadata(ComponentCache.ResourceEntry re)
			{
				if (this.metadata == null)
				{
					this.metadata = new List<ComponentCache.ResourceEntry>();
				}
				this.metadata.Add(re);
			}

			// Token: 0x06000CEF RID: 3311 RVA: 0x000323E1 File Offset: 0x000313E1
			public void AddResource(ComponentCache.ResourceEntry re)
			{
				if (this.resources == null)
				{
					this.resources = new List<ComponentCache.ResourceEntry>();
				}
				this.resources.Add(re);
			}

			// Token: 0x04000ED2 RID: 3794
			private ComponentCache cache;

			// Token: 0x04000ED3 RID: 3795
			private List<object> dependencies;

			// Token: 0x04000ED4 RID: 3796
			private List<string> localNames;

			// Token: 0x04000ED5 RID: 3797
			private List<ComponentCache.ResourceEntry> resources;

			// Token: 0x04000ED6 RID: 3798
			private List<ComponentCache.ResourceEntry> metadata;

			// Token: 0x04000ED7 RID: 3799
			private bool valid;

			// Token: 0x04000ED8 RID: 3800
			private bool tracking;

			// Token: 0x04000ED9 RID: 3801
			public object Component;

			// Token: 0x04000EDA RID: 3802
			public CodeStatementCollection Statements;
		}
	}
}
