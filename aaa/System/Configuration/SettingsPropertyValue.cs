using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace System.Configuration
{
	// Token: 0x02000717 RID: 1815
	public class SettingsPropertyValue
	{
		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06003784 RID: 14212 RVA: 0x000EB3AD File Offset: 0x000EA3AD
		public string Name
		{
			get
			{
				return this._Property.Name;
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06003785 RID: 14213 RVA: 0x000EB3BA File Offset: 0x000EA3BA
		// (set) Token: 0x06003786 RID: 14214 RVA: 0x000EB3C2 File Offset: 0x000EA3C2
		public bool IsDirty
		{
			get
			{
				return this._IsDirty;
			}
			set
			{
				this._IsDirty = value;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06003787 RID: 14215 RVA: 0x000EB3CB File Offset: 0x000EA3CB
		public SettingsProperty Property
		{
			get
			{
				return this._Property;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06003788 RID: 14216 RVA: 0x000EB3D3 File Offset: 0x000EA3D3
		public bool UsingDefaultValue
		{
			get
			{
				return this._UsingDefaultValue;
			}
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x000EB3DB File Offset: 0x000EA3DB
		public SettingsPropertyValue(SettingsProperty property)
		{
			this._Property = property;
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x0600378A RID: 14218 RVA: 0x000EB3F4 File Offset: 0x000EA3F4
		// (set) Token: 0x0600378B RID: 14219 RVA: 0x000EB46B File Offset: 0x000EA46B
		public object PropertyValue
		{
			get
			{
				if (!this._Deserialized)
				{
					this._Value = this.Deserialize();
					this._Deserialized = true;
				}
				if (this._Value != null && !this.Property.PropertyType.IsPrimitive && !(this._Value is string) && !(this._Value is DateTime))
				{
					this._UsingDefaultValue = false;
					this._ChangedSinceLastSerialized = true;
					this._IsDirty = true;
				}
				return this._Value;
			}
			set
			{
				this._Value = value;
				this._IsDirty = true;
				this._ChangedSinceLastSerialized = true;
				this._Deserialized = true;
				this._UsingDefaultValue = false;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x0600378C RID: 14220 RVA: 0x000EB490 File Offset: 0x000EA490
		// (set) Token: 0x0600378D RID: 14221 RVA: 0x000EB4B3 File Offset: 0x000EA4B3
		public object SerializedValue
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			get
			{
				if (this._ChangedSinceLastSerialized)
				{
					this._ChangedSinceLastSerialized = false;
					this._SerializedValue = this.SerializePropertyValue();
				}
				return this._SerializedValue;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			set
			{
				this._UsingDefaultValue = false;
				this._SerializedValue = value;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x0600378E RID: 14222 RVA: 0x000EB4C3 File Offset: 0x000EA4C3
		// (set) Token: 0x0600378F RID: 14223 RVA: 0x000EB4CB File Offset: 0x000EA4CB
		public bool Deserialized
		{
			get
			{
				return this._Deserialized;
			}
			set
			{
				this._Deserialized = value;
			}
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x000EB4D4 File Offset: 0x000EA4D4
		private bool IsHostedInAspnet()
		{
			return AppDomain.CurrentDomain.GetData(".appDomain") != null;
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x000EB4EC File Offset: 0x000EA4EC
		private object Deserialize()
		{
			object obj = null;
			if (this.SerializedValue != null)
			{
				try
				{
					if (this.SerializedValue is string)
					{
						obj = SettingsPropertyValue.GetObjectFromString(this.Property.PropertyType, this.Property.SerializeAs, (string)this.SerializedValue);
					}
					else
					{
						MemoryStream memoryStream = new MemoryStream((byte[])this.SerializedValue);
						try
						{
							obj = new BinaryFormatter().Deserialize(memoryStream);
						}
						finally
						{
							memoryStream.Close();
						}
					}
				}
				catch (Exception ex)
				{
					try
					{
						if (this.IsHostedInAspnet())
						{
							object[] array = new object[] { this.Property, this, ex };
							Type type = Type.GetType("System.Web.Management.WebBaseEvent, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
							type.InvokeMember("RaisePropertyDeserializationWebErrorEvent", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, array, CultureInfo.InvariantCulture);
						}
					}
					catch
					{
					}
				}
				if (obj != null && !this.Property.PropertyType.IsAssignableFrom(obj.GetType()))
				{
					obj = null;
				}
			}
			if (obj == null)
			{
				this._UsingDefaultValue = true;
				if (this.Property.DefaultValue == null || this.Property.DefaultValue.ToString() == "[null]")
				{
					if (this.Property.PropertyType.IsValueType)
					{
						return Activator.CreateInstance(this.Property.PropertyType);
					}
					return null;
				}
				else
				{
					if (!(this.Property.DefaultValue is string))
					{
						obj = this.Property.DefaultValue;
					}
					else
					{
						try
						{
							obj = SettingsPropertyValue.GetObjectFromString(this.Property.PropertyType, this.Property.SerializeAs, (string)this.Property.DefaultValue);
						}
						catch (Exception ex2)
						{
							throw new ArgumentException(SR.GetString("Could_not_create_from_default_value", new object[]
							{
								this.Property.Name,
								ex2.Message
							}));
						}
					}
					if (obj != null && !this.Property.PropertyType.IsAssignableFrom(obj.GetType()))
					{
						throw new ArgumentException(SR.GetString("Could_not_create_from_default_value_2", new object[] { this.Property.Name }));
					}
				}
			}
			if (obj == null)
			{
				if (this.Property.PropertyType == typeof(string))
				{
					obj = "";
				}
				else
				{
					try
					{
						obj = Activator.CreateInstance(this.Property.PropertyType);
					}
					catch
					{
					}
				}
			}
			return obj;
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x000EB774 File Offset: 0x000EA774
		private static object GetObjectFromString(Type type, SettingsSerializeAs serializeAs, string attValue)
		{
			if (type == typeof(string) && (attValue == null || attValue.Length < 1 || serializeAs == SettingsSerializeAs.String))
			{
				return attValue;
			}
			if (attValue == null || attValue.Length < 1)
			{
				return null;
			}
			switch (serializeAs)
			{
			case SettingsSerializeAs.String:
			{
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
				{
					return converter.ConvertFromInvariantString(attValue);
				}
				throw new ArgumentException(SR.GetString("Unable_to_convert_type_from_string", new object[] { type.ToString() }), "type");
			}
			case SettingsSerializeAs.Xml:
				break;
			case SettingsSerializeAs.Binary:
			{
				byte[] array = Convert.FromBase64String(attValue);
				MemoryStream memoryStream = null;
				try
				{
					memoryStream = new MemoryStream(array);
					return new BinaryFormatter().Deserialize(memoryStream);
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
				}
				break;
			}
			default:
				return null;
			}
			StringReader stringReader = new StringReader(attValue);
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(stringReader);
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x000EB884 File Offset: 0x000EA884
		private object SerializePropertyValue()
		{
			if (this._Value == null)
			{
				return null;
			}
			if (this.Property.SerializeAs != SettingsSerializeAs.Binary)
			{
				return SettingsPropertyValue.ConvertObjectToString(this._Value, this.Property.PropertyType, this.Property.SerializeAs, this.Property.ThrowOnErrorSerializing);
			}
			MemoryStream memoryStream = new MemoryStream();
			object obj;
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, this._Value);
				obj = memoryStream.ToArray();
			}
			finally
			{
				memoryStream.Close();
			}
			return obj;
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x000EB910 File Offset: 0x000EA910
		private static string ConvertObjectToString(object propValue, Type type, SettingsSerializeAs serializeAs, bool throwOnError)
		{
			if (serializeAs == SettingsSerializeAs.ProviderSpecific)
			{
				if (type == typeof(string) || type.IsPrimitive)
				{
					serializeAs = SettingsSerializeAs.String;
				}
				else
				{
					serializeAs = SettingsSerializeAs.Xml;
				}
			}
			try
			{
				switch (serializeAs)
				{
				case SettingsSerializeAs.String:
				{
					TypeConverter converter = TypeDescriptor.GetConverter(type);
					if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
					{
						return converter.ConvertToInvariantString(propValue);
					}
					throw new ArgumentException(SR.GetString("Unable_to_convert_type_to_string", new object[] { type.ToString() }), "type");
				}
				case SettingsSerializeAs.Xml:
					break;
				case SettingsSerializeAs.Binary:
				{
					MemoryStream memoryStream = new MemoryStream();
					try
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						binaryFormatter.Serialize(memoryStream, propValue);
						byte[] array = memoryStream.ToArray();
						return Convert.ToBase64String(array);
					}
					finally
					{
						memoryStream.Close();
					}
					break;
				}
				default:
					goto IL_0100;
				}
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				xmlSerializer.Serialize(stringWriter, propValue);
				return stringWriter.ToString();
			}
			catch (Exception)
			{
				if (throwOnError)
				{
					throw;
				}
			}
			IL_0100:
			return null;
		}

		// Token: 0x040031CA RID: 12746
		private object _Value;

		// Token: 0x040031CB RID: 12747
		private object _SerializedValue;

		// Token: 0x040031CC RID: 12748
		private bool _Deserialized;

		// Token: 0x040031CD RID: 12749
		private bool _IsDirty;

		// Token: 0x040031CE RID: 12750
		private SettingsProperty _Property;

		// Token: 0x040031CF RID: 12751
		private bool _ChangedSinceLastSerialized;

		// Token: 0x040031D0 RID: 12752
		private bool _UsingDefaultValue = true;
	}
}
