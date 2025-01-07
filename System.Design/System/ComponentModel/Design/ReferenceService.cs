using System;
using System.Collections;
using System.Globalization;

namespace System.ComponentModel.Design
{
	internal sealed class ReferenceService : IReferenceService, IDisposable
	{
		internal ReferenceService(IServiceProvider provider)
		{
			this._provider = provider;
		}

		private void CreateReferences(IComponent component)
		{
			this.CreateReferences(string.Empty, component, component);
		}

		private void CreateReferences(string trailingName, object reference, IComponent sitedComponent)
		{
			if (object.ReferenceEquals(reference, null))
			{
				return;
			}
			this._references.Add(new ReferenceService.ReferenceHolder(trailingName, reference, sitedComponent));
			foreach (object obj in TypeDescriptor.GetProperties(reference, ReferenceService._attributes))
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				if (propertyDescriptor.IsReadOnly)
				{
					this.CreateReferences(string.Format(CultureInfo.CurrentCulture, "{0}.{1}", new object[] { trailingName, propertyDescriptor.Name }), propertyDescriptor.GetValue(reference), sitedComponent);
				}
			}
		}

		private void EnsureReferences()
		{
			if (this._references == null)
			{
				if (this._provider == null)
				{
					throw new ObjectDisposedException("IReferenceService");
				}
				IComponentChangeService componentChangeService = this._provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdded += this.OnComponentAdded;
					componentChangeService.ComponentRemoved += this.OnComponentRemoved;
					componentChangeService.ComponentRename += this.OnComponentRename;
				}
				IContainer container = this._provider.GetService(typeof(IContainer)) as IContainer;
				if (container == null)
				{
					throw new InvalidOperationException();
				}
				this._references = new ArrayList(container.Components.Count);
				using (IEnumerator enumerator = container.Components.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						IComponent component = (IComponent)obj;
						this.CreateReferences(component);
					}
					return;
				}
			}
			if (!this._populating)
			{
				this._populating = true;
				try
				{
					if (this._addedComponents != null && this._addedComponents.Count > 0)
					{
						foreach (object obj2 in this._addedComponents)
						{
							IComponent component2 = (IComponent)obj2;
							this.RemoveReferences(component2);
							this.CreateReferences(component2);
						}
						this._addedComponents.Clear();
					}
					if (this._removedComponents != null && this._removedComponents.Count > 0)
					{
						foreach (object obj3 in this._removedComponents)
						{
							IComponent component3 = (IComponent)obj3;
							this.RemoveReferences(component3);
						}
						this._removedComponents.Clear();
					}
				}
				finally
				{
					this._populating = false;
				}
			}
		}

		private void OnComponentAdded(object sender, ComponentEventArgs cevent)
		{
			if (this._addedComponents == null)
			{
				this._addedComponents = new ArrayList();
			}
			IComponent component = cevent.Component;
			if (!(component.Site is INestedSite))
			{
				this._addedComponents.Add(component);
				if (this._removedComponents != null)
				{
					this._removedComponents.Remove(component);
				}
			}
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs cevent)
		{
			if (this._removedComponents == null)
			{
				this._removedComponents = new ArrayList();
			}
			IComponent component = cevent.Component;
			if (!(component.Site is INestedSite))
			{
				this._removedComponents.Add(component);
				if (this._addedComponents != null)
				{
					this._addedComponents.Remove(component);
				}
			}
		}

		private void OnComponentRename(object sender, ComponentRenameEventArgs cevent)
		{
			foreach (object obj in this._references)
			{
				ReferenceService.ReferenceHolder referenceHolder = (ReferenceService.ReferenceHolder)obj;
				if (object.ReferenceEquals(referenceHolder.SitedComponent, cevent.Component))
				{
					referenceHolder.ResetName();
					break;
				}
			}
		}

		private void RemoveReferences(IComponent component)
		{
			if (this._references != null)
			{
				int count = this._references.Count;
				for (int i = count - 1; i >= 0; i--)
				{
					if (object.ReferenceEquals(((ReferenceService.ReferenceHolder)this._references[i]).SitedComponent, component))
					{
						this._references.RemoveAt(i);
					}
				}
			}
		}

		void IDisposable.Dispose()
		{
			if (this._references != null && this._provider != null)
			{
				IComponentChangeService componentChangeService = this._provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdded -= this.OnComponentAdded;
					componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
					componentChangeService.ComponentRename -= this.OnComponentRename;
				}
				this._references = null;
				this._provider = null;
			}
		}

		IComponent IReferenceService.GetComponent(object reference)
		{
			if (object.ReferenceEquals(reference, null))
			{
				throw new ArgumentNullException("reference");
			}
			this.EnsureReferences();
			foreach (object obj in this._references)
			{
				ReferenceService.ReferenceHolder referenceHolder = (ReferenceService.ReferenceHolder)obj;
				if (object.ReferenceEquals(referenceHolder.Reference, reference))
				{
					return referenceHolder.SitedComponent;
				}
			}
			return null;
		}

		string IReferenceService.GetName(object reference)
		{
			if (object.ReferenceEquals(reference, null))
			{
				throw new ArgumentNullException("reference");
			}
			this.EnsureReferences();
			foreach (object obj in this._references)
			{
				ReferenceService.ReferenceHolder referenceHolder = (ReferenceService.ReferenceHolder)obj;
				if (object.ReferenceEquals(referenceHolder.Reference, reference))
				{
					return referenceHolder.Name;
				}
			}
			return null;
		}

		object IReferenceService.GetReference(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.EnsureReferences();
			foreach (object obj in this._references)
			{
				ReferenceService.ReferenceHolder referenceHolder = (ReferenceService.ReferenceHolder)obj;
				if (string.Equals(referenceHolder.Name, name, StringComparison.OrdinalIgnoreCase))
				{
					return referenceHolder.Reference;
				}
			}
			return null;
		}

		object[] IReferenceService.GetReferences()
		{
			this.EnsureReferences();
			object[] array = new object[this._references.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((ReferenceService.ReferenceHolder)this._references[i]).Reference;
			}
			return array;
		}

		object[] IReferenceService.GetReferences(Type baseType)
		{
			if (baseType == null)
			{
				throw new ArgumentNullException("baseType");
			}
			this.EnsureReferences();
			ArrayList arrayList = new ArrayList(this._references.Count);
			foreach (object obj in this._references)
			{
				ReferenceService.ReferenceHolder referenceHolder = (ReferenceService.ReferenceHolder)obj;
				object reference = referenceHolder.Reference;
				if (baseType.IsAssignableFrom(reference.GetType()))
				{
					arrayList.Add(reference);
				}
			}
			object[] array = new object[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		private static readonly Attribute[] _attributes = new Attribute[] { DesignerSerializationVisibilityAttribute.Content };

		private IServiceProvider _provider;

		private ArrayList _addedComponents;

		private ArrayList _removedComponents;

		private ArrayList _references;

		private bool _populating;

		private sealed class ReferenceHolder
		{
			internal ReferenceHolder(string trailingName, object reference, IComponent sitedComponent)
			{
				this._trailingName = trailingName;
				this._reference = reference;
				this._sitedComponent = sitedComponent;
			}

			internal void ResetName()
			{
				this._fullName = null;
			}

			internal string Name
			{
				get
				{
					if (this._fullName == null)
					{
						if (this._sitedComponent != null)
						{
							string componentName = TypeDescriptor.GetComponentName(this._sitedComponent);
							if (componentName != null)
							{
								this._fullName = string.Format(CultureInfo.CurrentCulture, "{0}{1}", new object[] { componentName, this._trailingName });
							}
						}
						if (this._fullName == null)
						{
							this._fullName = string.Empty;
						}
					}
					return this._fullName;
				}
			}

			internal object Reference
			{
				get
				{
					return this._reference;
				}
			}

			internal IComponent SitedComponent
			{
				get
				{
					return this._sitedComponent;
				}
			}

			private string _trailingName;

			private object _reference;

			private IComponent _sitedComponent;

			private string _fullName;
		}
	}
}
