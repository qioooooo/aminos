using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000165 RID: 357
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class DesignerOptionService : IDesignerOptionService
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x000281D9 File Offset: 0x000271D9
		public DesignerOptionService.DesignerOptionCollection Options
		{
			get
			{
				if (this._options == null)
				{
					this._options = new DesignerOptionService.DesignerOptionCollection(this, null, string.Empty, null);
				}
				return this._options;
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x000281FC File Offset: 0x000271FC
		protected DesignerOptionService.DesignerOptionCollection CreateOptionCollection(DesignerOptionService.DesignerOptionCollection parent, string name, object value)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					name.Length.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}), "name.Length");
			}
			return new DesignerOptionService.DesignerOptionCollection(this, parent, name, value);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0002827C File Offset: 0x0002727C
		private PropertyDescriptor GetOptionProperty(string pageName, string valueName)
		{
			if (pageName == null)
			{
				throw new ArgumentNullException("pageName");
			}
			if (valueName == null)
			{
				throw new ArgumentNullException("valueName");
			}
			string[] array = pageName.Split(new char[] { '\\' });
			DesignerOptionService.DesignerOptionCollection designerOptionCollection = this.Options;
			foreach (string text in array)
			{
				designerOptionCollection = designerOptionCollection[text];
				if (designerOptionCollection == null)
				{
					return null;
				}
			}
			return designerOptionCollection.Properties[valueName];
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000282FC File Offset: 0x000272FC
		protected virtual void PopulateOptionCollection(DesignerOptionService.DesignerOptionCollection options)
		{
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000282FE File Offset: 0x000272FE
		protected virtual bool ShowDialog(DesignerOptionService.DesignerOptionCollection options, object optionObject)
		{
			return false;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00028304 File Offset: 0x00027304
		object IDesignerOptionService.GetOptionValue(string pageName, string valueName)
		{
			PropertyDescriptor optionProperty = this.GetOptionProperty(pageName, valueName);
			if (optionProperty != null)
			{
				return optionProperty.GetValue(null);
			}
			return null;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00028328 File Offset: 0x00027328
		void IDesignerOptionService.SetOptionValue(string pageName, string valueName, object value)
		{
			PropertyDescriptor optionProperty = this.GetOptionProperty(pageName, valueName);
			if (optionProperty != null)
			{
				optionProperty.SetValue(null, value);
			}
		}

		// Token: 0x04000AAE RID: 2734
		private DesignerOptionService.DesignerOptionCollection _options;

		// Token: 0x02000166 RID: 358
		[TypeConverter(typeof(DesignerOptionService.DesignerOptionConverter))]
		[Editor("", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public sealed class DesignerOptionCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06000B89 RID: 2953 RVA: 0x00028354 File Offset: 0x00027354
			internal DesignerOptionCollection(DesignerOptionService service, DesignerOptionService.DesignerOptionCollection parent, string name, object value)
			{
				this._service = service;
				this._parent = parent;
				this._name = name;
				this._value = value;
				if (this._parent != null)
				{
					if (this._parent._children == null)
					{
						this._parent._children = new ArrayList(1);
					}
					this._parent._children.Add(this);
				}
			}

			// Token: 0x17000248 RID: 584
			// (get) Token: 0x06000B8A RID: 2954 RVA: 0x000283BC File Offset: 0x000273BC
			public int Count
			{
				get
				{
					this.EnsurePopulated();
					return this._children.Count;
				}
			}

			// Token: 0x17000249 RID: 585
			// (get) Token: 0x06000B8B RID: 2955 RVA: 0x000283CF File Offset: 0x000273CF
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x1700024A RID: 586
			// (get) Token: 0x06000B8C RID: 2956 RVA: 0x000283D7 File Offset: 0x000273D7
			public DesignerOptionService.DesignerOptionCollection Parent
			{
				get
				{
					return this._parent;
				}
			}

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x06000B8D RID: 2957 RVA: 0x000283E0 File Offset: 0x000273E0
			public PropertyDescriptorCollection Properties
			{
				get
				{
					if (this._properties == null)
					{
						ArrayList arrayList;
						if (this._value != null)
						{
							PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this._value);
							arrayList = new ArrayList(properties.Count);
							using (IEnumerator enumerator = properties.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object obj = enumerator.Current;
									PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
									arrayList.Add(new DesignerOptionService.DesignerOptionCollection.WrappedPropertyDescriptor(propertyDescriptor, this._value));
								}
								goto IL_007C;
							}
						}
						arrayList = new ArrayList(1);
						IL_007C:
						this.EnsurePopulated();
						foreach (object obj2 in this._children)
						{
							DesignerOptionService.DesignerOptionCollection designerOptionCollection = (DesignerOptionService.DesignerOptionCollection)obj2;
							arrayList.AddRange(designerOptionCollection.Properties);
						}
						PropertyDescriptor[] array = (PropertyDescriptor[])arrayList.ToArray(typeof(PropertyDescriptor));
						this._properties = new PropertyDescriptorCollection(array, true);
					}
					return this._properties;
				}
			}

			// Token: 0x1700024C RID: 588
			public DesignerOptionService.DesignerOptionCollection this[int index]
			{
				get
				{
					this.EnsurePopulated();
					if (index < 0 || index >= this._children.Count)
					{
						throw new IndexOutOfRangeException("index");
					}
					return (DesignerOptionService.DesignerOptionCollection)this._children[index];
				}
			}

			// Token: 0x1700024D RID: 589
			public DesignerOptionService.DesignerOptionCollection this[string name]
			{
				get
				{
					this.EnsurePopulated();
					foreach (object obj in this._children)
					{
						DesignerOptionService.DesignerOptionCollection designerOptionCollection = (DesignerOptionService.DesignerOptionCollection)obj;
						if (string.Compare(designerOptionCollection.Name, name, true, CultureInfo.InvariantCulture) == 0)
						{
							return designerOptionCollection;
						}
					}
					return null;
				}
			}

			// Token: 0x06000B90 RID: 2960 RVA: 0x000285AC File Offset: 0x000275AC
			public void CopyTo(Array array, int index)
			{
				this.EnsurePopulated();
				this._children.CopyTo(array, index);
			}

			// Token: 0x06000B91 RID: 2961 RVA: 0x000285C1 File Offset: 0x000275C1
			private void EnsurePopulated()
			{
				if (this._children == null)
				{
					this._service.PopulateOptionCollection(this);
					if (this._children == null)
					{
						this._children = new ArrayList(1);
					}
				}
			}

			// Token: 0x06000B92 RID: 2962 RVA: 0x000285EB File Offset: 0x000275EB
			public IEnumerator GetEnumerator()
			{
				this.EnsurePopulated();
				return this._children.GetEnumerator();
			}

			// Token: 0x06000B93 RID: 2963 RVA: 0x000285FE File Offset: 0x000275FE
			public int IndexOf(DesignerOptionService.DesignerOptionCollection value)
			{
				this.EnsurePopulated();
				return this._children.IndexOf(value);
			}

			// Token: 0x06000B94 RID: 2964 RVA: 0x00028614 File Offset: 0x00027614
			private static object RecurseFindValue(DesignerOptionService.DesignerOptionCollection options)
			{
				if (options._value != null)
				{
					return options._value;
				}
				foreach (object obj in options)
				{
					DesignerOptionService.DesignerOptionCollection designerOptionCollection = (DesignerOptionService.DesignerOptionCollection)obj;
					object obj2 = DesignerOptionService.DesignerOptionCollection.RecurseFindValue(designerOptionCollection);
					if (obj2 != null)
					{
						return obj2;
					}
				}
				return null;
			}

			// Token: 0x06000B95 RID: 2965 RVA: 0x00028684 File Offset: 0x00027684
			public bool ShowDialog()
			{
				object obj = DesignerOptionService.DesignerOptionCollection.RecurseFindValue(this);
				return obj != null && this._service.ShowDialog(this, obj);
			}

			// Token: 0x1700024E RID: 590
			// (get) Token: 0x06000B96 RID: 2966 RVA: 0x000286AA File Offset: 0x000276AA
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700024F RID: 591
			// (get) Token: 0x06000B97 RID: 2967 RVA: 0x000286AD File Offset: 0x000276AD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000250 RID: 592
			// (get) Token: 0x06000B98 RID: 2968 RVA: 0x000286B0 File Offset: 0x000276B0
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000251 RID: 593
			// (get) Token: 0x06000B99 RID: 2969 RVA: 0x000286B3 File Offset: 0x000276B3
			bool IList.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000252 RID: 594
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x06000B9C RID: 2972 RVA: 0x000286C6 File Offset: 0x000276C6
			int IList.Add(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000B9D RID: 2973 RVA: 0x000286CD File Offset: 0x000276CD
			void IList.Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000B9E RID: 2974 RVA: 0x000286D4 File Offset: 0x000276D4
			bool IList.Contains(object value)
			{
				this.EnsurePopulated();
				return this._children.Contains(value);
			}

			// Token: 0x06000B9F RID: 2975 RVA: 0x000286E8 File Offset: 0x000276E8
			int IList.IndexOf(object value)
			{
				this.EnsurePopulated();
				return this._children.IndexOf(value);
			}

			// Token: 0x06000BA0 RID: 2976 RVA: 0x000286FC File Offset: 0x000276FC
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000BA1 RID: 2977 RVA: 0x00028703 File Offset: 0x00027703
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000BA2 RID: 2978 RVA: 0x0002870A File Offset: 0x0002770A
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x04000AAF RID: 2735
			private DesignerOptionService _service;

			// Token: 0x04000AB0 RID: 2736
			private DesignerOptionService.DesignerOptionCollection _parent;

			// Token: 0x04000AB1 RID: 2737
			private string _name;

			// Token: 0x04000AB2 RID: 2738
			private object _value;

			// Token: 0x04000AB3 RID: 2739
			private ArrayList _children;

			// Token: 0x04000AB4 RID: 2740
			private PropertyDescriptorCollection _properties;

			// Token: 0x02000167 RID: 359
			private sealed class WrappedPropertyDescriptor : PropertyDescriptor
			{
				// Token: 0x06000BA3 RID: 2979 RVA: 0x00028711 File Offset: 0x00027711
				internal WrappedPropertyDescriptor(PropertyDescriptor property, object target)
					: base(property.Name, null)
				{
					this.property = property;
					this.target = target;
				}

				// Token: 0x17000253 RID: 595
				// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0002872E File Offset: 0x0002772E
				public override AttributeCollection Attributes
				{
					get
					{
						return this.property.Attributes;
					}
				}

				// Token: 0x17000254 RID: 596
				// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0002873B File Offset: 0x0002773B
				public override Type ComponentType
				{
					get
					{
						return this.property.ComponentType;
					}
				}

				// Token: 0x17000255 RID: 597
				// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00028748 File Offset: 0x00027748
				public override bool IsReadOnly
				{
					get
					{
						return this.property.IsReadOnly;
					}
				}

				// Token: 0x17000256 RID: 598
				// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x00028755 File Offset: 0x00027755
				public override Type PropertyType
				{
					get
					{
						return this.property.PropertyType;
					}
				}

				// Token: 0x06000BA8 RID: 2984 RVA: 0x00028762 File Offset: 0x00027762
				public override bool CanResetValue(object component)
				{
					return this.property.CanResetValue(this.target);
				}

				// Token: 0x06000BA9 RID: 2985 RVA: 0x00028775 File Offset: 0x00027775
				public override object GetValue(object component)
				{
					return this.property.GetValue(this.target);
				}

				// Token: 0x06000BAA RID: 2986 RVA: 0x00028788 File Offset: 0x00027788
				public override void ResetValue(object component)
				{
					this.property.ResetValue(this.target);
				}

				// Token: 0x06000BAB RID: 2987 RVA: 0x0002879B File Offset: 0x0002779B
				public override void SetValue(object component, object value)
				{
					this.property.SetValue(this.target, value);
				}

				// Token: 0x06000BAC RID: 2988 RVA: 0x000287AF File Offset: 0x000277AF
				public override bool ShouldSerializeValue(object component)
				{
					return this.property.ShouldSerializeValue(this.target);
				}

				// Token: 0x04000AB5 RID: 2741
				private object target;

				// Token: 0x04000AB6 RID: 2742
				private PropertyDescriptor property;
			}
		}

		// Token: 0x02000168 RID: 360
		internal sealed class DesignerOptionConverter : TypeConverter
		{
			// Token: 0x06000BAD RID: 2989 RVA: 0x000287C2 File Offset: 0x000277C2
			public override bool GetPropertiesSupported(ITypeDescriptorContext cxt)
			{
				return true;
			}

			// Token: 0x06000BAE RID: 2990 RVA: 0x000287C8 File Offset: 0x000277C8
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext cxt, object value, Attribute[] attributes)
			{
				PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
				DesignerOptionService.DesignerOptionCollection designerOptionCollection = value as DesignerOptionService.DesignerOptionCollection;
				if (designerOptionCollection == null)
				{
					return propertyDescriptorCollection;
				}
				foreach (object obj in designerOptionCollection)
				{
					DesignerOptionService.DesignerOptionCollection designerOptionCollection2 = (DesignerOptionService.DesignerOptionCollection)obj;
					propertyDescriptorCollection.Add(new DesignerOptionService.DesignerOptionConverter.OptionPropertyDescriptor(designerOptionCollection2));
				}
				foreach (object obj2 in designerOptionCollection.Properties)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
					propertyDescriptorCollection.Add(propertyDescriptor);
				}
				return propertyDescriptorCollection;
			}

			// Token: 0x06000BAF RID: 2991 RVA: 0x00028890 File Offset: 0x00027890
			public override object ConvertTo(ITypeDescriptorContext cxt, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return SR.GetString("CollectionConverterText");
				}
				return base.ConvertTo(cxt, culture, value, destinationType);
			}

			// Token: 0x02000169 RID: 361
			private class OptionPropertyDescriptor : PropertyDescriptor
			{
				// Token: 0x06000BB1 RID: 2993 RVA: 0x000288BE File Offset: 0x000278BE
				internal OptionPropertyDescriptor(DesignerOptionService.DesignerOptionCollection option)
					: base(option.Name, null)
				{
					this._option = option;
				}

				// Token: 0x17000257 RID: 599
				// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x000288D4 File Offset: 0x000278D4
				public override Type ComponentType
				{
					get
					{
						return this._option.GetType();
					}
				}

				// Token: 0x17000258 RID: 600
				// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x000288E1 File Offset: 0x000278E1
				public override bool IsReadOnly
				{
					get
					{
						return true;
					}
				}

				// Token: 0x17000259 RID: 601
				// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x000288E4 File Offset: 0x000278E4
				public override Type PropertyType
				{
					get
					{
						return this._option.GetType();
					}
				}

				// Token: 0x06000BB5 RID: 2997 RVA: 0x000288F1 File Offset: 0x000278F1
				public override bool CanResetValue(object component)
				{
					return false;
				}

				// Token: 0x06000BB6 RID: 2998 RVA: 0x000288F4 File Offset: 0x000278F4
				public override object GetValue(object component)
				{
					return this._option;
				}

				// Token: 0x06000BB7 RID: 2999 RVA: 0x000288FC File Offset: 0x000278FC
				public override void ResetValue(object component)
				{
				}

				// Token: 0x06000BB8 RID: 3000 RVA: 0x000288FE File Offset: 0x000278FE
				public override void SetValue(object component, object value)
				{
				}

				// Token: 0x06000BB9 RID: 3001 RVA: 0x00028900 File Offset: 0x00027900
				public override bool ShouldSerializeValue(object component)
				{
					return false;
				}

				// Token: 0x04000AB7 RID: 2743
				private DesignerOptionService.DesignerOptionCollection _option;
			}
		}
	}
}
