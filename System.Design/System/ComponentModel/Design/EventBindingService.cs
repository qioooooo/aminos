using System;
using System.Collections;
using System.Collections.Generic;
using System.Design;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using Microsoft.Internal.Performance;

namespace System.ComponentModel.Design
{
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public abstract class EventBindingService : IEventBindingService
	{
		protected EventBindingService(IServiceProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this._provider = provider;
		}

		protected abstract string CreateUniqueMethodName(IComponent component, EventDescriptor e);

		protected virtual void FreeMethod(IComponent component, EventDescriptor e, string methodName)
		{
		}

		protected abstract ICollection GetCompatibleMethods(EventDescriptor e);

		private string GetEventDescriptorHashCode(EventDescriptor eventDesc)
		{
			StringBuilder stringBuilder = new StringBuilder(eventDesc.Name);
			stringBuilder.Append(eventDesc.EventType.GetHashCode().ToString(CultureInfo.InvariantCulture));
			foreach (object obj in eventDesc.Attributes)
			{
				Attribute attribute = (Attribute)obj;
				stringBuilder.Append(attribute.GetHashCode().ToString(CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}

		protected object GetService(Type serviceType)
		{
			if (this._provider != null)
			{
				return this._provider.GetService(serviceType);
			}
			return null;
		}

		protected abstract bool ShowCode();

		protected abstract bool ShowCode(int lineNumber);

		protected abstract bool ShowCode(IComponent component, EventDescriptor e, string methodName);

		protected virtual void UseMethod(IComponent component, EventDescriptor e, string methodName)
		{
		}

		protected virtual void ValidateMethodName(string methodName)
		{
		}

		string IEventBindingService.CreateUniqueMethodName(IComponent component, EventDescriptor e)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			return this.CreateUniqueMethodName(component, e);
		}

		ICollection IEventBindingService.GetCompatibleMethods(EventDescriptor e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			return this.GetCompatibleMethods(e);
		}

		EventDescriptor IEventBindingService.GetEvent(PropertyDescriptor property)
		{
			if (property is EventBindingService.EventPropertyDescriptor)
			{
				return ((EventBindingService.EventPropertyDescriptor)property).Event;
			}
			return null;
		}

		private bool HasGenericArgument(EventDescriptor ed)
		{
			if (ed == null || ed.ComponentType == null)
			{
				return false;
			}
			EventInfo @event = ed.ComponentType.GetEvent(ed.Name);
			if (@event == null || !@event.EventHandlerType.IsGenericType)
			{
				return false;
			}
			Type[] genericArguments = @event.EventHandlerType.GetGenericArguments();
			if (genericArguments != null && genericArguments.Length > 0)
			{
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (genericArguments[i].IsGenericType)
					{
						return true;
					}
				}
			}
			return false;
		}

		PropertyDescriptorCollection IEventBindingService.GetEventProperties(EventDescriptorCollection events)
		{
			if (events == null)
			{
				throw new ArgumentNullException("events");
			}
			List<PropertyDescriptor> list = new List<PropertyDescriptor>(events.Count);
			if (this._eventProperties == null)
			{
				this._eventProperties = new Hashtable();
			}
			for (int i = 0; i < events.Count; i++)
			{
				if (!this.HasGenericArgument(events[i]))
				{
					object eventDescriptorHashCode = this.GetEventDescriptorHashCode(events[i]);
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)this._eventProperties[eventDescriptorHashCode];
					if (propertyDescriptor == null)
					{
						propertyDescriptor = new EventBindingService.EventPropertyDescriptor(events[i], this);
						this._eventProperties[eventDescriptorHashCode] = propertyDescriptor;
					}
					list.Add(propertyDescriptor);
				}
			}
			return new PropertyDescriptorCollection(list.ToArray());
		}

		PropertyDescriptor IEventBindingService.GetEventProperty(EventDescriptor e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this._eventProperties == null)
			{
				this._eventProperties = new Hashtable();
			}
			object eventDescriptorHashCode = this.GetEventDescriptorHashCode(e);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)this._eventProperties[eventDescriptorHashCode];
			if (propertyDescriptor == null)
			{
				propertyDescriptor = new EventBindingService.EventPropertyDescriptor(e, this);
				this._eventProperties[eventDescriptorHashCode] = propertyDescriptor;
			}
			return propertyDescriptor;
		}

		bool IEventBindingService.ShowCode()
		{
			return this.ShowCode();
		}

		bool IEventBindingService.ShowCode(int lineNumber)
		{
			return this.ShowCode(lineNumber);
		}

		bool IEventBindingService.ShowCode(IComponent component, EventDescriptor e)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			PropertyDescriptor eventProperty = ((IEventBindingService)this).GetEventProperty(e);
			string text = (string)eventProperty.GetValue(component);
			if (text == null)
			{
				return false;
			}
			this.showCodeComponent = component;
			this.showCodeEventDescriptor = e;
			this.showCodeMethodName = text;
			Application.Idle += this.ShowCodeIdle;
			return true;
		}

		private void ShowCodeIdle(object sender, EventArgs e)
		{
			Application.Idle -= this.ShowCodeIdle;
			try
			{
				this.ShowCode(this.showCodeComponent, this.showCodeEventDescriptor, this.showCodeMethodName);
			}
			finally
			{
				this.showCodeComponent = null;
				this.showCodeEventDescriptor = null;
				this.showCodeMethodName = null;
				EventBindingService.codemarkers.CodeMarker(CodeMarkerEvent.perfFXDesignShowCode);
			}
		}

		private Hashtable _eventProperties;

		private IServiceProvider _provider;

		private IComponent showCodeComponent;

		private EventDescriptor showCodeEventDescriptor;

		private string showCodeMethodName;

		private static CodeMarkers codemarkers = CodeMarkers.Instance;

		private class EventPropertyDescriptor : PropertyDescriptor
		{
			internal EventPropertyDescriptor(EventDescriptor eventDesc, EventBindingService eventSvc)
				: base(eventDesc, null)
			{
				this._eventDesc = eventDesc;
				this._eventSvc = eventSvc;
			}

			public override bool CanResetValue(object component)
			{
				return this.GetValue(component) != null;
			}

			public override Type ComponentType
			{
				get
				{
					return this._eventDesc.ComponentType;
				}
			}

			public override TypeConverter Converter
			{
				get
				{
					if (this._converter == null)
					{
						this._converter = new EventBindingService.EventPropertyDescriptor.EventConverter(this._eventDesc);
					}
					return this._converter;
				}
			}

			internal EventDescriptor Event
			{
				get
				{
					return this._eventDesc;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return this.Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes);
				}
			}

			public override Type PropertyType
			{
				get
				{
					return this._eventDesc.EventType;
				}
			}

			public override object GetValue(object component)
			{
				if (component == null)
				{
					throw new ArgumentNullException("component");
				}
				ISite site = null;
				if (component is IComponent)
				{
					site = ((IComponent)component).Site;
				}
				if (site == null)
				{
					IReferenceService referenceService = this._eventSvc._provider.GetService(typeof(IReferenceService)) as IReferenceService;
					if (referenceService != null)
					{
						IComponent component2 = referenceService.GetComponent(component);
						if (component2 != null)
						{
							site = component2.Site;
						}
					}
				}
				if (site == null)
				{
					return null;
				}
				IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
				if (dictionaryService == null)
				{
					return null;
				}
				return (string)dictionaryService.GetValue(new EventBindingService.EventPropertyDescriptor.ReferenceEventClosure(component, this));
			}

			public override void ResetValue(object component)
			{
				this.SetValue(component, null);
			}

			public override void SetValue(object component, object value)
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(SR.GetString("EventBindingServiceEventReadOnly", new object[] { this.Name }))
					{
						HelpLink = "EventBindingServiceEventReadOnly"
					};
				}
				if (value != null && !(value is string))
				{
					throw new ArgumentException(SR.GetString("EventBindingServiceBadArgType", new object[]
					{
						this.Name,
						typeof(string).Name
					}))
					{
						HelpLink = "EventBindingServiceBadArgType"
					};
				}
				string text = (string)value;
				if (text != null && text.Length == 0)
				{
					text = null;
				}
				ISite site = null;
				if (component is IComponent)
				{
					site = ((IComponent)component).Site;
				}
				if (site == null)
				{
					IReferenceService referenceService = this._eventSvc._provider.GetService(typeof(IReferenceService)) as IReferenceService;
					if (referenceService != null)
					{
						IComponent component2 = referenceService.GetComponent(component);
						if (component2 != null)
						{
							site = component2.Site;
						}
					}
				}
				if (site == null)
				{
					throw new InvalidOperationException(SR.GetString("EventBindingServiceNoSite"))
					{
						HelpLink = "EventBindingServiceNoSite"
					};
				}
				IDictionaryService dictionaryService = site.GetService(typeof(IDictionaryService)) as IDictionaryService;
				if (dictionaryService == null)
				{
					throw new InvalidOperationException(SR.GetString("EventBindingServiceMissingService", new object[] { typeof(IDictionaryService).Name }))
					{
						HelpLink = "EventBindingServiceMissingService"
					};
				}
				EventBindingService.EventPropertyDescriptor.ReferenceEventClosure referenceEventClosure = new EventBindingService.EventPropertyDescriptor.ReferenceEventClosure(component, this);
				string text2 = (string)dictionaryService.GetValue(referenceEventClosure);
				if (object.ReferenceEquals(text2, text))
				{
					return;
				}
				if (text2 != null && text != null && text2.Equals(text))
				{
					return;
				}
				if (text != null)
				{
					this._eventSvc.ValidateMethodName(text);
				}
				IDesignerHost designerHost = site.GetService(typeof(IDesignerHost)) as IDesignerHost;
				DesignerTransaction designerTransaction = null;
				if (designerHost != null)
				{
					designerTransaction = designerHost.CreateTransaction(SR.GetString("EventBindingServiceSetValue", new object[] { site.Name, text }));
				}
				try
				{
					IComponentChangeService componentChangeService = site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					if (componentChangeService != null)
					{
						try
						{
							componentChangeService.OnComponentChanging(component, this);
							componentChangeService.OnComponentChanging(component, this.Event);
						}
						catch (CheckoutException ex)
						{
							if (ex == CheckoutException.Canceled)
							{
								return;
							}
							throw;
						}
					}
					if (text != null)
					{
						this._eventSvc.UseMethod((IComponent)component, this._eventDesc, text);
					}
					if (text2 != null)
					{
						this._eventSvc.FreeMethod((IComponent)component, this._eventDesc, text2);
					}
					dictionaryService.SetValue(referenceEventClosure, text);
					if (componentChangeService != null)
					{
						componentChangeService.OnComponentChanged(component, this.Event, null, null);
						componentChangeService.OnComponentChanged(component, this, text2, text);
					}
					this.OnValueChanged(component, EventArgs.Empty);
					if (designerTransaction != null)
					{
						designerTransaction.Commit();
					}
				}
				finally
				{
					if (designerTransaction != null)
					{
						((IDisposable)designerTransaction).Dispose();
					}
				}
			}

			public override bool ShouldSerializeValue(object component)
			{
				return this.CanResetValue(component);
			}

			private EventDescriptor _eventDesc;

			private EventBindingService _eventSvc;

			private TypeConverter _converter;

			private class EventConverter : TypeConverter
			{
				internal EventConverter(EventDescriptor evt)
				{
					this._evt = evt;
				}

				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
				}

				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
				{
					return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
				}

				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					if (value == null)
					{
						return value;
					}
					if (!(value is string))
					{
						return base.ConvertFrom(context, culture, value);
					}
					if (((string)value).Length == 0)
					{
						return null;
					}
					return value;
				}

				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType != typeof(string))
					{
						return base.ConvertTo(context, culture, value, destinationType);
					}
					if (value != null)
					{
						return value;
					}
					return string.Empty;
				}

				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					string[] array = null;
					if (context != null)
					{
						IEventBindingService eventBindingService = (IEventBindingService)context.GetService(typeof(IEventBindingService));
						if (eventBindingService != null)
						{
							ICollection compatibleMethods = eventBindingService.GetCompatibleMethods(this._evt);
							array = new string[compatibleMethods.Count];
							int num = 0;
							foreach (object obj in compatibleMethods)
							{
								string text = (string)obj;
								array[num++] = text;
							}
						}
					}
					return new TypeConverter.StandardValuesCollection(array);
				}

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return false;
				}

				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}

				private EventDescriptor _evt;
			}

			private class ReferenceEventClosure
			{
				public ReferenceEventClosure(object reference, EventBindingService.EventPropertyDescriptor prop)
				{
					this.reference = reference;
					this.propertyDescriptor = prop;
				}

				public override int GetHashCode()
				{
					return this.reference.GetHashCode() * this.propertyDescriptor.GetHashCode();
				}

				public override bool Equals(object otherClosure)
				{
					if (otherClosure is EventBindingService.EventPropertyDescriptor.ReferenceEventClosure)
					{
						EventBindingService.EventPropertyDescriptor.ReferenceEventClosure referenceEventClosure = (EventBindingService.EventPropertyDescriptor.ReferenceEventClosure)otherClosure;
						return referenceEventClosure.reference == this.reference && referenceEventClosure.propertyDescriptor.Equals(this.propertyDescriptor);
					}
					return false;
				}

				private object reference;

				private EventBindingService.EventPropertyDescriptor propertyDescriptor;
			}
		}
	}
}
