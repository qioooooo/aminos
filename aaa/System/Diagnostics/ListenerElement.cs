using System;
using System.Collections;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001C7 RID: 455
	internal class ListenerElement : TypedElement
	{
		// Token: 0x06000E33 RID: 3635 RVA: 0x0002D28C File Offset: 0x0002C28C
		public ListenerElement(bool allowReferences)
			: base(typeof(TraceListener))
		{
			this._allowReferences = allowReferences;
			ConfigurationPropertyOptions configurationPropertyOptions = ConfigurationPropertyOptions.None;
			if (!this._allowReferences)
			{
				configurationPropertyOptions |= ConfigurationPropertyOptions.IsRequired;
			}
			this._propListenerTypeName = new ConfigurationProperty("type", typeof(string), null, configurationPropertyOptions);
			this._properties.Remove("type");
			this._properties.Add(this._propListenerTypeName);
			this._properties.Add(ListenerElement._propFilter);
			this._properties.Add(ListenerElement._propName);
			this._properties.Add(ListenerElement._propOutputOpts);
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0002D32C File Offset: 0x0002C32C
		public Hashtable Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					this._attributes = new Hashtable(StringComparer.OrdinalIgnoreCase);
				}
				return this._attributes;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x0002D34C File Offset: 0x0002C34C
		[ConfigurationProperty("filter")]
		public FilterElement Filter
		{
			get
			{
				return (FilterElement)base[ListenerElement._propFilter];
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0002D35E File Offset: 0x0002C35E
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x0002D370 File Offset: 0x0002C370
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[ListenerElement._propName];
			}
			set
			{
				base[ListenerElement._propName] = value;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x0002D37E File Offset: 0x0002C37E
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x0002D390 File Offset: 0x0002C390
		[ConfigurationProperty("traceOutputOptions", DefaultValue = TraceOptions.None)]
		public TraceOptions TraceOutputOptions
		{
			get
			{
				return (TraceOptions)base[ListenerElement._propOutputOpts];
			}
			set
			{
				base[ListenerElement._propOutputOpts] = value;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x0002D3A3 File Offset: 0x0002C3A3
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x0002D3B6 File Offset: 0x0002C3B6
		[ConfigurationProperty("type")]
		public override string TypeName
		{
			get
			{
				return (string)base[this._propListenerTypeName];
			}
			set
			{
				base[this._propListenerTypeName] = value;
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0002D3C8 File Offset: 0x0002C3C8
		public override bool Equals(object compareTo)
		{
			if (this.Name.Equals("Default") && this.TypeName.Equals(typeof(DefaultTraceListener).FullName))
			{
				ListenerElement listenerElement = compareTo as ListenerElement;
				return listenerElement != null && listenerElement.Name.Equals("Default") && listenerElement.TypeName.Equals(typeof(DefaultTraceListener).FullName);
			}
			return base.Equals(compareTo);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0002D443 File Offset: 0x0002C443
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0002D44C File Offset: 0x0002C44C
		public TraceListener GetRuntimeObject()
		{
			if (this._runtimeObject != null)
			{
				return (TraceListener)this._runtimeObject;
			}
			TraceListener traceListener;
			try
			{
				string typeName = this.TypeName;
				if (string.IsNullOrEmpty(typeName))
				{
					if (this._attributes != null || base.ElementInformation.Properties[ListenerElement._propFilter.Name].ValueOrigin == PropertyValueOrigin.SetHere || this.TraceOutputOptions != TraceOptions.None || !string.IsNullOrEmpty(base.InitData))
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_listener_cant_have_properties", new object[] { this.Name }));
					}
					if (DiagnosticsConfiguration.SharedListeners == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_to_nonexistent_listener", new object[] { this.Name }));
					}
					ListenerElement listenerElement = DiagnosticsConfiguration.SharedListeners[this.Name];
					if (listenerElement == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_to_nonexistent_listener", new object[] { this.Name }));
					}
					this._runtimeObject = listenerElement.GetRuntimeObject();
					traceListener = (TraceListener)this._runtimeObject;
				}
				else
				{
					TraceListener traceListener2 = (TraceListener)base.BaseGetRuntimeObject();
					traceListener2.initializeData = base.InitData;
					traceListener2.Name = this.Name;
					traceListener2.SetAttributes(this.Attributes);
					traceListener2.TraceOutputOptions = this.TraceOutputOptions;
					if (this.Filter != null && this.Filter.TypeName != null && this.Filter.TypeName.Length != 0)
					{
						traceListener2.Filter = this.Filter.GetRuntimeObject();
					}
					this._runtimeObject = traceListener2;
					traceListener = traceListener2;
				}
			}
			catch (ArgumentException ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Could_not_create_listener", new object[] { this.Name }), ex);
			}
			return traceListener;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0002D628 File Offset: 0x0002C628
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			ConfigurationProperty configurationProperty = new ConfigurationProperty(name, typeof(string), value);
			this._properties.Add(configurationProperty);
			base[configurationProperty] = value;
			this.Attributes.Add(name, value);
			return true;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0002D66C File Offset: 0x0002C66C
		internal void ResetProperties()
		{
			if (this._attributes != null)
			{
				this._attributes.Clear();
				this._properties.Clear();
				this._properties.Add(this._propListenerTypeName);
				this._properties.Add(ListenerElement._propFilter);
				this._properties.Add(ListenerElement._propName);
				this._properties.Add(ListenerElement._propOutputOpts);
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0002D6D8 File Offset: 0x0002C6D8
		internal TraceListener RefreshRuntimeObject(TraceListener listener)
		{
			this._runtimeObject = null;
			TraceListener traceListener;
			try
			{
				string typeName = this.TypeName;
				if (string.IsNullOrEmpty(typeName))
				{
					if (this._attributes != null || base.ElementInformation.Properties[ListenerElement._propFilter.Name].ValueOrigin == PropertyValueOrigin.SetHere || this.TraceOutputOptions != TraceOptions.None || !string.IsNullOrEmpty(base.InitData))
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_listener_cant_have_properties", new object[] { this.Name }));
					}
					if (DiagnosticsConfiguration.SharedListeners == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_to_nonexistent_listener", new object[] { this.Name }));
					}
					ListenerElement listenerElement = DiagnosticsConfiguration.SharedListeners[this.Name];
					if (listenerElement == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_to_nonexistent_listener", new object[] { this.Name }));
					}
					this._runtimeObject = listenerElement.RefreshRuntimeObject(listener);
					traceListener = (TraceListener)this._runtimeObject;
				}
				else if (Type.GetType(typeName) != listener.GetType() || base.InitData != listener.initializeData)
				{
					traceListener = this.GetRuntimeObject();
				}
				else
				{
					listener.SetAttributes(this.Attributes);
					listener.TraceOutputOptions = this.TraceOutputOptions;
					if (base.ElementInformation.Properties[ListenerElement._propFilter.Name].ValueOrigin == PropertyValueOrigin.SetHere)
					{
						listener.Filter = this.Filter.RefreshRuntimeObject(listener.Filter);
					}
					else
					{
						listener.Filter = null;
					}
					this._runtimeObject = listener;
					traceListener = listener;
				}
			}
			catch (ArgumentException ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Could_not_create_listener", new object[] { this.Name }), ex);
			}
			return traceListener;
		}

		// Token: 0x04000EE8 RID: 3816
		private static readonly ConfigurationProperty _propFilter = new ConfigurationProperty("filter", typeof(FilterElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000EE9 RID: 3817
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04000EEA RID: 3818
		private static readonly ConfigurationProperty _propOutputOpts = new ConfigurationProperty("traceOutputOptions", typeof(TraceOptions), TraceOptions.None, ConfigurationPropertyOptions.None);

		// Token: 0x04000EEB RID: 3819
		private ConfigurationProperty _propListenerTypeName;

		// Token: 0x04000EEC RID: 3820
		private bool _allowReferences;

		// Token: 0x04000EED RID: 3821
		private Hashtable _attributes;

		// Token: 0x04000EEE RID: 3822
		internal bool _isAddedByDefault;
	}
}
