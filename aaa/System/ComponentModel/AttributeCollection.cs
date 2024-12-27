using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200009B RID: 155
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class AttributeCollection : ICollection, IEnumerable
	{
		// Token: 0x06000588 RID: 1416 RVA: 0x00016F6C File Offset: 0x00015F6C
		public AttributeCollection(params Attribute[] attributes)
		{
			if (attributes == null)
			{
				attributes = new Attribute[0];
			}
			this._attributes = attributes;
			for (int i = 0; i < attributes.Length; i++)
			{
				if (attributes[i] == null)
				{
					throw new ArgumentNullException("attributes");
				}
			}
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00016FB0 File Offset: 0x00015FB0
		public static AttributeCollection FromExisting(AttributeCollection existing, params Attribute[] newAttributes)
		{
			if (existing == null)
			{
				throw new ArgumentNullException("existing");
			}
			if (newAttributes == null)
			{
				newAttributes = new Attribute[0];
			}
			Attribute[] array = new Attribute[existing.Count + newAttributes.Length];
			int count = existing.Count;
			existing.CopyTo(array, 0);
			for (int i = 0; i < newAttributes.Length; i++)
			{
				if (newAttributes[i] == null)
				{
					throw new ArgumentNullException("newAttributes");
				}
				bool flag = false;
				for (int j = 0; j < existing.Count; j++)
				{
					if (array[j].TypeId.Equals(newAttributes[i].TypeId))
					{
						flag = true;
						array[j] = newAttributes[i];
						break;
					}
				}
				if (!flag)
				{
					array[count++] = newAttributes[i];
				}
			}
			Attribute[] array2;
			if (count < array.Length)
			{
				array2 = new Attribute[count];
				Array.Copy(array, 0, array2, 0, count);
			}
			else
			{
				array2 = array;
			}
			return new AttributeCollection(array2);
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00017082 File Offset: 0x00016082
		public int Count
		{
			get
			{
				return this._attributes.Length;
			}
		}

		// Token: 0x17000114 RID: 276
		public virtual Attribute this[int index]
		{
			get
			{
				return this._attributes[index];
			}
		}

		// Token: 0x17000115 RID: 277
		public virtual Attribute this[Type attributeType]
		{
			get
			{
				Attribute defaultAttribute;
				lock (AttributeCollection.internalSyncObject)
				{
					if (this._foundAttributeTypes == null)
					{
						this._foundAttributeTypes = new AttributeCollection.AttributeEntry[5];
					}
					int i = 0;
					while (i < 5)
					{
						if (this._foundAttributeTypes[i].type == attributeType)
						{
							int index = this._foundAttributeTypes[i].index;
							if (index != -1)
							{
								return this._attributes[index];
							}
							return this.GetDefaultAttribute(attributeType);
						}
						else
						{
							if (this._foundAttributeTypes[i].type == null)
							{
								break;
							}
							i++;
						}
					}
					i = this._index++;
					if (this._index >= 5)
					{
						this._index = 0;
					}
					this._foundAttributeTypes[i].type = attributeType;
					int num = this._attributes.Length;
					for (int j = 0; j < num; j++)
					{
						Attribute attribute = this._attributes[j];
						Type type = attribute.GetType();
						if (type == attributeType)
						{
							this._foundAttributeTypes[i].index = j;
							return attribute;
						}
					}
					for (int k = 0; k < num; k++)
					{
						Attribute attribute2 = this._attributes[k];
						Type type2 = attribute2.GetType();
						if (attributeType.IsAssignableFrom(type2))
						{
							this._foundAttributeTypes[i].index = k;
							return attribute2;
						}
					}
					this._foundAttributeTypes[i].index = -1;
					defaultAttribute = this.GetDefaultAttribute(attributeType);
				}
				return defaultAttribute;
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00017234 File Offset: 0x00016234
		public bool Contains(Attribute attribute)
		{
			Attribute attribute2 = this[attribute.GetType()];
			return attribute2 != null && attribute2.Equals(attribute);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00017260 File Offset: 0x00016260
		public bool Contains(Attribute[] attributes)
		{
			if (attributes == null)
			{
				return true;
			}
			for (int i = 0; i < attributes.Length; i++)
			{
				if (!this.Contains(attributes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00017290 File Offset: 0x00016290
		protected Attribute GetDefaultAttribute(Type attributeType)
		{
			Attribute attribute;
			lock (AttributeCollection.internalSyncObject)
			{
				if (AttributeCollection._defaultAttributes == null)
				{
					AttributeCollection._defaultAttributes = new Hashtable();
				}
				if (AttributeCollection._defaultAttributes.ContainsKey(attributeType))
				{
					attribute = (Attribute)AttributeCollection._defaultAttributes[attributeType];
				}
				else
				{
					Attribute attribute2 = null;
					Type reflectionType = TypeDescriptor.GetReflectionType(attributeType);
					FieldInfo field = reflectionType.GetField("Default", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
					if (field != null && field.IsStatic)
					{
						attribute2 = (Attribute)field.GetValue(null);
					}
					else
					{
						ConstructorInfo constructor = reflectionType.UnderlyingSystemType.GetConstructor(new Type[0]);
						if (constructor != null)
						{
							attribute2 = (Attribute)constructor.Invoke(new object[0]);
							if (!attribute2.IsDefaultAttribute())
							{
								attribute2 = null;
							}
						}
					}
					AttributeCollection._defaultAttributes[attributeType] = attribute2;
					attribute = attribute2;
				}
			}
			return attribute;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001736C File Offset: 0x0001636C
		public IEnumerator GetEnumerator()
		{
			return this._attributes.GetEnumerator();
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001737C File Offset: 0x0001637C
		public bool Matches(Attribute attribute)
		{
			for (int i = 0; i < this._attributes.Length; i++)
			{
				if (this._attributes[i].Match(attribute))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000173B0 File Offset: 0x000163B0
		public bool Matches(Attribute[] attributes)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				if (!this.Matches(attributes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x000173D9 File Offset: 0x000163D9
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x000173E1 File Offset: 0x000163E1
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x000173E4 File Offset: 0x000163E4
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000173E7 File Offset: 0x000163E7
		public void CopyTo(Array array, int index)
		{
			Array.Copy(this._attributes, 0, array, index, this._attributes.Length);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000173FF File Offset: 0x000163FF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040008D4 RID: 2260
		private const int FOUND_TYPES_LIMIT = 5;

		// Token: 0x040008D5 RID: 2261
		public static readonly AttributeCollection Empty = new AttributeCollection(null);

		// Token: 0x040008D6 RID: 2262
		private static Hashtable _defaultAttributes;

		// Token: 0x040008D7 RID: 2263
		private Attribute[] _attributes;

		// Token: 0x040008D8 RID: 2264
		private static object internalSyncObject = new object();

		// Token: 0x040008D9 RID: 2265
		private AttributeCollection.AttributeEntry[] _foundAttributeTypes;

		// Token: 0x040008DA RID: 2266
		private int _index;

		// Token: 0x0200009C RID: 156
		private struct AttributeEntry
		{
			// Token: 0x040008DB RID: 2267
			public Type type;

			// Token: 0x040008DC RID: 2268
			public int index;
		}
	}
}
