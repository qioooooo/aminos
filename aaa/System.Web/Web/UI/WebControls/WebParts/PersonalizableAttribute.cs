using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006D9 RID: 1753
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PersonalizableAttribute : Attribute
	{
		// Token: 0x0600560A RID: 22026 RVA: 0x0015BC30 File Offset: 0x0015AC30
		public PersonalizableAttribute()
			: this(true, PersonalizationScope.User, false)
		{
		}

		// Token: 0x0600560B RID: 22027 RVA: 0x0015BC3B File Offset: 0x0015AC3B
		public PersonalizableAttribute(bool isPersonalizable)
			: this(isPersonalizable, PersonalizationScope.User, false)
		{
		}

		// Token: 0x0600560C RID: 22028 RVA: 0x0015BC46 File Offset: 0x0015AC46
		public PersonalizableAttribute(PersonalizationScope scope)
			: this(true, scope, false)
		{
		}

		// Token: 0x0600560D RID: 22029 RVA: 0x0015BC51 File Offset: 0x0015AC51
		public PersonalizableAttribute(PersonalizationScope scope, bool isSensitive)
			: this(true, scope, isSensitive)
		{
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x0015BC5C File Offset: 0x0015AC5C
		private PersonalizableAttribute(bool isPersonalizable, PersonalizationScope scope, bool isSensitive)
		{
			this._isPersonalizable = isPersonalizable;
			this._isSensitive = isSensitive;
			if (this._isPersonalizable)
			{
				this._scope = scope;
			}
		}

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x0600560F RID: 22031 RVA: 0x0015BC81 File Offset: 0x0015AC81
		public bool IsPersonalizable
		{
			get
			{
				return this._isPersonalizable;
			}
		}

		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x06005610 RID: 22032 RVA: 0x0015BC89 File Offset: 0x0015AC89
		public bool IsSensitive
		{
			get
			{
				return this._isSensitive;
			}
		}

		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x06005611 RID: 22033 RVA: 0x0015BC91 File Offset: 0x0015AC91
		public PersonalizationScope Scope
		{
			get
			{
				return this._scope;
			}
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x0015BC9C File Offset: 0x0015AC9C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			PersonalizableAttribute personalizableAttribute = obj as PersonalizableAttribute;
			return personalizableAttribute != null && (personalizableAttribute.IsPersonalizable == this.IsPersonalizable && personalizableAttribute.Scope == this.Scope) && personalizableAttribute.IsSensitive == this.IsSensitive;
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x0015BCE7 File Offset: 0x0015ACE7
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this._isPersonalizable.GetHashCode(), this._scope.GetHashCode(), this._isSensitive.GetHashCode());
		}

		// Token: 0x06005614 RID: 22036 RVA: 0x0015BD14 File Offset: 0x0015AD14
		public static ICollection GetPersonalizableProperties(Type type)
		{
			PersonalizableTypeEntry personalizableTypeEntry = (PersonalizableTypeEntry)PersonalizableAttribute.PersonalizableTypeTable[type];
			if (personalizableTypeEntry == null)
			{
				personalizableTypeEntry = new PersonalizableTypeEntry(type);
				PersonalizableAttribute.PersonalizableTypeTable[type] = personalizableTypeEntry;
			}
			return personalizableTypeEntry.PropertyInfos;
		}

		// Token: 0x06005615 RID: 22037 RVA: 0x0015BD50 File Offset: 0x0015AD50
		internal static IDictionary GetPersonalizablePropertyEntries(Type type)
		{
			PersonalizableTypeEntry personalizableTypeEntry = (PersonalizableTypeEntry)PersonalizableAttribute.PersonalizableTypeTable[type];
			if (personalizableTypeEntry == null)
			{
				personalizableTypeEntry = new PersonalizableTypeEntry(type);
				PersonalizableAttribute.PersonalizableTypeTable[type] = personalizableTypeEntry;
			}
			return personalizableTypeEntry.PropertyEntries;
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x0015BD8C File Offset: 0x0015AD8C
		internal static IDictionary GetPersonalizablePropertyValues(Control control, PersonalizationScope scope, bool excludeSensitive)
		{
			IDictionary dictionary = null;
			IDictionary personalizablePropertyEntries = PersonalizableAttribute.GetPersonalizablePropertyEntries(control.GetType());
			if (personalizablePropertyEntries.Count != 0)
			{
				foreach (object obj in personalizablePropertyEntries)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text = (string)dictionaryEntry.Key;
					PersonalizablePropertyEntry personalizablePropertyEntry = (PersonalizablePropertyEntry)dictionaryEntry.Value;
					if ((!excludeSensitive || !personalizablePropertyEntry.IsSensitive) && (scope != PersonalizationScope.User || personalizablePropertyEntry.Scope != PersonalizationScope.Shared))
					{
						if (dictionary == null)
						{
							dictionary = new HybridDictionary(personalizablePropertyEntries.Count, false);
						}
						object property = FastPropertyAccessor.GetProperty(control, text, control.DesignMode);
						dictionary[text] = new Pair(personalizablePropertyEntry.PropertyInfo, property);
					}
				}
			}
			if (dictionary == null)
			{
				dictionary = new HybridDictionary(false);
			}
			return dictionary;
		}

		// Token: 0x06005617 RID: 22039 RVA: 0x0015BE6C File Offset: 0x0015AE6C
		public override bool IsDefaultAttribute()
		{
			return this.Equals(PersonalizableAttribute.Default);
		}

		// Token: 0x06005618 RID: 22040 RVA: 0x0015BE7C File Offset: 0x0015AE7C
		public override bool Match(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			PersonalizableAttribute personalizableAttribute = obj as PersonalizableAttribute;
			return personalizableAttribute != null && personalizableAttribute.IsPersonalizable == this.IsPersonalizable;
		}

		// Token: 0x04002F3F RID: 12095
		internal static readonly Type PersonalizableAttributeType = typeof(PersonalizableAttribute);

		// Token: 0x04002F40 RID: 12096
		private static readonly IDictionary PersonalizableTypeTable = Hashtable.Synchronized(new Hashtable());

		// Token: 0x04002F41 RID: 12097
		public static readonly PersonalizableAttribute NotPersonalizable = new PersonalizableAttribute(false);

		// Token: 0x04002F42 RID: 12098
		public static readonly PersonalizableAttribute Personalizable = new PersonalizableAttribute(true);

		// Token: 0x04002F43 RID: 12099
		public static readonly PersonalizableAttribute UserPersonalizable = new PersonalizableAttribute(PersonalizationScope.User);

		// Token: 0x04002F44 RID: 12100
		public static readonly PersonalizableAttribute SharedPersonalizable = new PersonalizableAttribute(PersonalizationScope.Shared);

		// Token: 0x04002F45 RID: 12101
		public static readonly PersonalizableAttribute Default = PersonalizableAttribute.NotPersonalizable;

		// Token: 0x04002F46 RID: 12102
		private bool _isPersonalizable;

		// Token: 0x04002F47 RID: 12103
		private bool _isSensitive;

		// Token: 0x04002F48 RID: 12104
		private PersonalizationScope _scope;
	}
}
