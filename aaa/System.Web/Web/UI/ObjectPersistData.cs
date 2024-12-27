using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000431 RID: 1073
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ObjectPersistData
	{
		// Token: 0x06003367 RID: 13159 RVA: 0x000DF2E0 File Offset: 0x000DE2E0
		public ObjectPersistData(ControlBuilder builder, IDictionary builtObjects)
		{
			this._objectType = builder.ControlType;
			this._localize = builder.Localize;
			this._resourceKey = builder.GetResourceKey();
			this._builtObjects = builtObjects;
			if (typeof(ICollection).IsAssignableFrom(this._objectType))
			{
				this._isCollection = true;
			}
			this._collectionItems = new ArrayList();
			this._propertyTableByFilter = new HybridDictionary(true);
			this._propertyTableByProperty = new HybridDictionary(true);
			this._allPropertyEntries = new ArrayList();
			this._eventEntries = new ArrayList();
			foreach (object obj in builder.SimplePropertyEntries)
			{
				PropertyEntry propertyEntry = (PropertyEntry)obj;
				this.AddPropertyEntry(propertyEntry);
			}
			foreach (object obj2 in builder.ComplexPropertyEntries)
			{
				PropertyEntry propertyEntry2 = (PropertyEntry)obj2;
				this.AddPropertyEntry(propertyEntry2);
			}
			foreach (object obj3 in builder.TemplatePropertyEntries)
			{
				PropertyEntry propertyEntry3 = (PropertyEntry)obj3;
				this.AddPropertyEntry(propertyEntry3);
			}
			foreach (object obj4 in builder.BoundPropertyEntries)
			{
				PropertyEntry propertyEntry4 = (PropertyEntry)obj4;
				this.AddPropertyEntry(propertyEntry4);
			}
			foreach (object obj5 in builder.EventEntries)
			{
				EventEntry eventEntry = (EventEntry)obj5;
				this.AddEventEntry(eventEntry);
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06003368 RID: 13160 RVA: 0x000DF508 File Offset: 0x000DE508
		public ICollection AllPropertyEntries
		{
			get
			{
				return this._allPropertyEntries;
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06003369 RID: 13161 RVA: 0x000DF510 File Offset: 0x000DE510
		public IDictionary BuiltObjects
		{
			get
			{
				return this._builtObjects;
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x0600336A RID: 13162 RVA: 0x000DF518 File Offset: 0x000DE518
		public ICollection CollectionItems
		{
			get
			{
				return this._collectionItems;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x000DF520 File Offset: 0x000DE520
		public ICollection EventEntries
		{
			get
			{
				return this._eventEntries;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x0600336C RID: 13164 RVA: 0x000DF528 File Offset: 0x000DE528
		public bool IsCollection
		{
			get
			{
				return this._isCollection;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x0600336D RID: 13165 RVA: 0x000DF530 File Offset: 0x000DE530
		public bool Localize
		{
			get
			{
				return this._localize;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x0600336E RID: 13166 RVA: 0x000DF538 File Offset: 0x000DE538
		public Type ObjectType
		{
			get
			{
				return this._objectType;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x0600336F RID: 13167 RVA: 0x000DF540 File Offset: 0x000DE540
		public string ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x000DF548 File Offset: 0x000DE548
		private void AddPropertyEntry(PropertyEntry entry)
		{
			if (this._isCollection && entry is ComplexPropertyEntry && ((ComplexPropertyEntry)entry).IsCollectionItem)
			{
				this._collectionItems.Add(entry);
			}
			else
			{
				IDictionary dictionary = (IDictionary)this._propertyTableByFilter[entry.Filter];
				if (dictionary == null)
				{
					dictionary = new HybridDictionary(true);
					this._propertyTableByFilter[entry.Filter] = dictionary;
				}
				dictionary[entry.Name] = entry;
				ArrayList arrayList = (ArrayList)this._propertyTableByProperty[entry.Name];
				if (arrayList == null)
				{
					arrayList = new ArrayList();
					this._propertyTableByProperty[entry.Name] = arrayList;
				}
				arrayList.Add(entry);
			}
			this._allPropertyEntries.Add(entry);
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x000DF608 File Offset: 0x000DE608
		private void AddEventEntry(EventEntry entry)
		{
			this._eventEntries.Add(entry);
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x000DF618 File Offset: 0x000DE618
		public void AddToObjectControlBuilderTable(IDictionary table)
		{
			if (this._builtObjects != null)
			{
				foreach (object obj in this._builtObjects)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					table[dictionaryEntry.Key] = dictionaryEntry.Value;
				}
			}
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x000DF688 File Offset: 0x000DE688
		public PropertyEntry GetFilteredProperty(string filter, string name)
		{
			IDictionary filteredProperties = this.GetFilteredProperties(filter);
			if (filteredProperties != null)
			{
				return (PropertyEntry)filteredProperties[name];
			}
			return null;
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x000DF6AE File Offset: 0x000DE6AE
		public IDictionary GetFilteredProperties(string filter)
		{
			return (IDictionary)this._propertyTableByFilter[filter];
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x000DF6C4 File Offset: 0x000DE6C4
		public ICollection GetPropertyAllFilters(string name)
		{
			ICollection collection = (ICollection)this._propertyTableByProperty[name];
			if (collection == null)
			{
				return new ArrayList();
			}
			return collection;
		}

		// Token: 0x040023FD RID: 9213
		private Type _objectType;

		// Token: 0x040023FE RID: 9214
		private bool _isCollection;

		// Token: 0x040023FF RID: 9215
		private ArrayList _collectionItems;

		// Token: 0x04002400 RID: 9216
		private bool _localize;

		// Token: 0x04002401 RID: 9217
		private string _resourceKey;

		// Token: 0x04002402 RID: 9218
		private IDictionary _propertyTableByFilter;

		// Token: 0x04002403 RID: 9219
		private IDictionary _propertyTableByProperty;

		// Token: 0x04002404 RID: 9220
		private ArrayList _allPropertyEntries;

		// Token: 0x04002405 RID: 9221
		private ArrayList _eventEntries;

		// Token: 0x04002406 RID: 9222
		private IDictionary _builtObjects;
	}
}
