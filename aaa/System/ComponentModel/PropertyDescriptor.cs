using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000090 RID: 144
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class PropertyDescriptor : MemberDescriptor
	{
		// Token: 0x06000532 RID: 1330 RVA: 0x00016423 File Offset: 0x00015423
		protected PropertyDescriptor(string name, Attribute[] attrs)
			: base(name, attrs)
		{
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001642D File Offset: 0x0001542D
		protected PropertyDescriptor(MemberDescriptor descr)
			: base(descr)
		{
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00016436 File Offset: 0x00015436
		protected PropertyDescriptor(MemberDescriptor descr, Attribute[] attrs)
			: base(descr, attrs)
		{
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000535 RID: 1333
		public abstract Type ComponentType { get; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00016440 File Offset: 0x00015440
		public virtual TypeConverter Converter
		{
			get
			{
				AttributeCollection attributes = this.Attributes;
				if (this.converter == null)
				{
					TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)attributes[typeof(TypeConverterAttribute)];
					if (typeConverterAttribute.ConverterTypeName != null && typeConverterAttribute.ConverterTypeName.Length > 0)
					{
						Type typeFromName = this.GetTypeFromName(typeConverterAttribute.ConverterTypeName);
						if (typeFromName != null && typeof(TypeConverter).IsAssignableFrom(typeFromName))
						{
							this.converter = (TypeConverter)this.CreateInstance(typeFromName);
						}
					}
					if (this.converter == null)
					{
						this.converter = TypeDescriptor.GetConverter(this.PropertyType);
					}
				}
				return this.converter;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x000164DB File Offset: 0x000154DB
		public virtual bool IsLocalizable
		{
			get
			{
				return LocalizableAttribute.Yes.Equals(this.Attributes[typeof(LocalizableAttribute)]);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000538 RID: 1336
		public abstract bool IsReadOnly { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000164FC File Offset: 0x000154FC
		public DesignerSerializationVisibility SerializationVisibility
		{
			get
			{
				DesignerSerializationVisibilityAttribute designerSerializationVisibilityAttribute = (DesignerSerializationVisibilityAttribute)this.Attributes[typeof(DesignerSerializationVisibilityAttribute)];
				return designerSerializationVisibilityAttribute.Visibility;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600053A RID: 1338
		public abstract Type PropertyType { get; }

		// Token: 0x0600053B RID: 1339 RVA: 0x0001652C File Offset: 0x0001552C
		public virtual void AddValueChanged(object component, EventHandler handler)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (this.valueChangedHandlers == null)
			{
				this.valueChangedHandlers = new Hashtable();
			}
			EventHandler eventHandler = (EventHandler)this.valueChangedHandlers[component];
			this.valueChangedHandlers[component] = Delegate.Combine(eventHandler, handler);
		}

		// Token: 0x0600053C RID: 1340
		public abstract bool CanResetValue(object component);

		// Token: 0x0600053D RID: 1341 RVA: 0x00016590 File Offset: 0x00015590
		public override bool Equals(object obj)
		{
			try
			{
				if (obj == this)
				{
					return true;
				}
				if (obj == null)
				{
					return false;
				}
				PropertyDescriptor propertyDescriptor = obj as PropertyDescriptor;
				if (propertyDescriptor != null && propertyDescriptor.NameHashCode == this.NameHashCode && propertyDescriptor.PropertyType == this.PropertyType && propertyDescriptor.Name.Equals(this.Name))
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00016604 File Offset: 0x00015604
		protected object CreateInstance(Type type)
		{
			Type[] array = new Type[] { typeof(Type) };
			ConstructorInfo constructor = type.GetConstructor(array);
			if (constructor != null)
			{
				return TypeDescriptor.CreateInstance(null, type, array, new object[] { this.PropertyType });
			}
			return TypeDescriptor.CreateInstance(null, type, null, null);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00016655 File Offset: 0x00015655
		protected override void FillAttributes(IList attributeList)
		{
			this.converter = null;
			this.editors = null;
			this.editorTypes = null;
			this.editorCount = 0;
			base.FillAttributes(attributeList);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001667A File Offset: 0x0001567A
		public PropertyDescriptorCollection GetChildProperties()
		{
			return this.GetChildProperties(null, null);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00016684 File Offset: 0x00015684
		public PropertyDescriptorCollection GetChildProperties(Attribute[] filter)
		{
			return this.GetChildProperties(null, filter);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001668E File Offset: 0x0001568E
		public PropertyDescriptorCollection GetChildProperties(object instance)
		{
			return this.GetChildProperties(instance, null);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00016698 File Offset: 0x00015698
		public virtual PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			if (instance == null)
			{
				return TypeDescriptor.GetProperties(this.PropertyType, filter);
			}
			return TypeDescriptor.GetProperties(instance, filter);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000166B4 File Offset: 0x000156B4
		public virtual object GetEditor(Type editorBaseType)
		{
			object obj = null;
			AttributeCollection attributes = this.Attributes;
			if (this.editorTypes != null)
			{
				for (int i = 0; i < this.editorCount; i++)
				{
					if (this.editorTypes[i] == editorBaseType)
					{
						return this.editors[i];
					}
				}
			}
			if (obj == null)
			{
				for (int j = 0; j < attributes.Count; j++)
				{
					EditorAttribute editorAttribute = attributes[j] as EditorAttribute;
					if (editorAttribute != null)
					{
						Type typeFromName = this.GetTypeFromName(editorAttribute.EditorBaseTypeName);
						if (editorBaseType == typeFromName)
						{
							Type typeFromName2 = this.GetTypeFromName(editorAttribute.EditorTypeName);
							if (typeFromName2 != null)
							{
								obj = this.CreateInstance(typeFromName2);
								break;
							}
						}
					}
				}
				if (obj == null)
				{
					obj = TypeDescriptor.GetEditor(this.PropertyType, editorBaseType);
				}
				if (this.editorTypes == null)
				{
					this.editorTypes = new Type[5];
					this.editors = new object[5];
				}
				if (this.editorCount >= this.editorTypes.Length)
				{
					Type[] array = new Type[this.editorTypes.Length * 2];
					object[] array2 = new object[this.editors.Length * 2];
					Array.Copy(this.editorTypes, array, this.editorTypes.Length);
					Array.Copy(this.editors, array2, this.editors.Length);
					this.editorTypes = array;
					this.editors = array2;
				}
				this.editorTypes[this.editorCount] = editorBaseType;
				this.editors[this.editorCount++] = obj;
			}
			return obj;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00016818 File Offset: 0x00015818
		public override int GetHashCode()
		{
			return this.NameHashCode ^ this.PropertyType.GetHashCode();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001682C File Offset: 0x0001582C
		protected override object GetInvocationTarget(Type type, object instance)
		{
			object obj = base.GetInvocationTarget(type, instance);
			ICustomTypeDescriptor customTypeDescriptor = obj as ICustomTypeDescriptor;
			if (customTypeDescriptor != null)
			{
				obj = customTypeDescriptor.GetPropertyOwner(this);
			}
			return obj;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00016858 File Offset: 0x00015858
		protected Type GetTypeFromName(string typeName)
		{
			if (typeName == null || typeName.Length == 0)
			{
				return null;
			}
			Type type = Type.GetType(typeName);
			Type type2 = null;
			if (this.ComponentType != null && (type == null || this.ComponentType.Assembly.FullName.Equals(type.Assembly.FullName)))
			{
				int num = typeName.IndexOf(',');
				if (num != -1)
				{
					typeName = typeName.Substring(0, num);
				}
				type2 = this.ComponentType.Assembly.GetType(typeName);
			}
			return type2 ?? type;
		}

		// Token: 0x06000548 RID: 1352
		public abstract object GetValue(object component);

		// Token: 0x06000549 RID: 1353 RVA: 0x000168D8 File Offset: 0x000158D8
		protected virtual void OnValueChanged(object component, EventArgs e)
		{
			if (component != null && this.valueChangedHandlers != null)
			{
				EventHandler eventHandler = (EventHandler)this.valueChangedHandlers[component];
				if (eventHandler != null)
				{
					eventHandler(component, e);
				}
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00016910 File Offset: 0x00015910
		public virtual void RemoveValueChanged(object component, EventHandler handler)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (this.valueChangedHandlers != null)
			{
				EventHandler eventHandler = (EventHandler)this.valueChangedHandlers[component];
				eventHandler = (EventHandler)Delegate.Remove(eventHandler, handler);
				if (eventHandler != null)
				{
					this.valueChangedHandlers[component] = eventHandler;
					return;
				}
				this.valueChangedHandlers.Remove(component);
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001697D File Offset: 0x0001597D
		protected internal EventHandler GetValueChangedHandler(object component)
		{
			if (component != null && this.valueChangedHandlers != null)
			{
				return (EventHandler)this.valueChangedHandlers[component];
			}
			return null;
		}

		// Token: 0x0600054C RID: 1356
		public abstract void ResetValue(object component);

		// Token: 0x0600054D RID: 1357
		public abstract void SetValue(object component, object value);

		// Token: 0x0600054E RID: 1358
		public abstract bool ShouldSerializeValue(object component);

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001699D File Offset: 0x0001599D
		public virtual bool SupportsChangeEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040008C1 RID: 2241
		private TypeConverter converter;

		// Token: 0x040008C2 RID: 2242
		private Hashtable valueChangedHandlers;

		// Token: 0x040008C3 RID: 2243
		private object[] editors;

		// Token: 0x040008C4 RID: 2244
		private Type[] editorTypes;

		// Token: 0x040008C5 RID: 2245
		private int editorCount;
	}
}
