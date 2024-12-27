using System;
using System.Collections;
using System.Globalization;

namespace System.Xml.Serialization
{
	// Token: 0x020002B0 RID: 688
	public class CodeIdentifiers
	{
		// Token: 0x0600210B RID: 8459 RVA: 0x0009C617 File Offset: 0x0009B617
		public CodeIdentifiers()
			: this(true)
		{
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0009C620 File Offset: 0x0009B620
		public CodeIdentifiers(bool caseSensitive)
		{
			if (caseSensitive)
			{
				this.identifiers = new Hashtable();
				this.reservedIdentifiers = new Hashtable();
			}
			else
			{
				IEqualityComparer equalityComparer = new CaseInsensitiveKeyComparer();
				this.identifiers = new Hashtable(equalityComparer);
				this.reservedIdentifiers = new Hashtable(equalityComparer);
			}
			this.list = new ArrayList();
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0009C677 File Offset: 0x0009B677
		public void Clear()
		{
			this.identifiers.Clear();
			this.list.Clear();
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x0009C68F File Offset: 0x0009B68F
		// (set) Token: 0x0600210F RID: 8463 RVA: 0x0009C697 File Offset: 0x0009B697
		public bool UseCamelCasing
		{
			get
			{
				return this.camelCase;
			}
			set
			{
				this.camelCase = value;
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x0009C6A0 File Offset: 0x0009B6A0
		public string MakeRightCase(string identifier)
		{
			if (this.camelCase)
			{
				return CodeIdentifier.MakeCamel(identifier);
			}
			return CodeIdentifier.MakePascal(identifier);
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0009C6B8 File Offset: 0x0009B6B8
		public string MakeUnique(string identifier)
		{
			if (this.IsInUse(identifier))
			{
				int num = 1;
				string text;
				for (;;)
				{
					text = identifier + num.ToString(CultureInfo.InvariantCulture);
					if (!this.IsInUse(text))
					{
						break;
					}
					num++;
				}
				identifier = text;
			}
			if (identifier.Length > 511)
			{
				return this.MakeUnique("Item");
			}
			return identifier;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x0009C711 File Offset: 0x0009B711
		public void AddReserved(string identifier)
		{
			this.reservedIdentifiers.Add(identifier, identifier);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x0009C720 File Offset: 0x0009B720
		public void RemoveReserved(string identifier)
		{
			this.reservedIdentifiers.Remove(identifier);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x0009C72E File Offset: 0x0009B72E
		public string AddUnique(string identifier, object value)
		{
			identifier = this.MakeUnique(identifier);
			this.Add(identifier, value);
			return identifier;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x0009C742 File Offset: 0x0009B742
		public bool IsInUse(string identifier)
		{
			return this.identifiers.Contains(identifier) || this.reservedIdentifiers.Contains(identifier);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0009C760 File Offset: 0x0009B760
		public void Add(string identifier, object value)
		{
			this.identifiers.Add(identifier, value);
			this.list.Add(value);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x0009C77C File Offset: 0x0009B77C
		public void Remove(string identifier)
		{
			this.list.Remove(this.identifiers[identifier]);
			this.identifiers.Remove(identifier);
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x0009C7A4 File Offset: 0x0009B7A4
		public object ToArray(Type type)
		{
			Array array = Array.CreateInstance(type, this.list.Count);
			this.list.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0009C7D4 File Offset: 0x0009B7D4
		internal CodeIdentifiers Clone()
		{
			return new CodeIdentifiers
			{
				identifiers = (Hashtable)this.identifiers.Clone(),
				reservedIdentifiers = (Hashtable)this.reservedIdentifiers.Clone(),
				list = (ArrayList)this.list.Clone(),
				camelCase = this.camelCase
			};
		}

		// Token: 0x0400142E RID: 5166
		private Hashtable identifiers;

		// Token: 0x0400142F RID: 5167
		private Hashtable reservedIdentifiers;

		// Token: 0x04001430 RID: 5168
		private ArrayList list;

		// Token: 0x04001431 RID: 5169
		private bool camelCase;
	}
}
