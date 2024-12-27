using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000386 RID: 902
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AttributeCollection
	{
		// Token: 0x06002C25 RID: 11301 RVA: 0x000C5598 File Offset: 0x000C4598
		public AttributeCollection(StateBag bag)
		{
			this._bag = bag;
		}

		// Token: 0x17000981 RID: 2433
		public string this[string key]
		{
			get
			{
				if (this._styleColl != null && StringUtil.EqualsIgnoreCase(key, "style"))
				{
					return this._styleColl.Value;
				}
				return this._bag[key] as string;
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06002C28 RID: 11304 RVA: 0x000C55E5 File Offset: 0x000C45E5
		public ICollection Keys
		{
			get
			{
				return this._bag.Keys;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06002C29 RID: 11305 RVA: 0x000C55F2 File Offset: 0x000C45F2
		public int Count
		{
			get
			{
				return this._bag.Count;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x000C55FF File Offset: 0x000C45FF
		public CssStyleCollection CssStyle
		{
			get
			{
				if (this._styleColl == null)
				{
					this._styleColl = new CssStyleCollection(this._bag);
				}
				return this._styleColl;
			}
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000C5620 File Offset: 0x000C4620
		public void Add(string key, string value)
		{
			if (this._styleColl != null && StringUtil.EqualsIgnoreCase(key, "style"))
			{
				this._styleColl.Value = value;
				return;
			}
			this._bag[key] = value;
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000C5654 File Offset: 0x000C4654
		public override bool Equals(object o)
		{
			AttributeCollection attributeCollection = o as AttributeCollection;
			if (attributeCollection == null)
			{
				return false;
			}
			if (attributeCollection.Count != this._bag.Count)
			{
				return false;
			}
			foreach (object obj in this._bag)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (this[(string)dictionaryEntry.Key] != attributeCollection[(string)dictionaryEntry.Key])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000C56FC File Offset: 0x000C46FC
		public override int GetHashCode()
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			foreach (object obj in this._bag)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				hashCodeCombiner.AddObject(dictionaryEntry.Key);
				hashCodeCombiner.AddObject(dictionaryEntry.Value);
			}
			return hashCodeCombiner.CombinedHash32;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x000C5774 File Offset: 0x000C4774
		public void Remove(string key)
		{
			if (this._styleColl != null && StringUtil.EqualsIgnoreCase(key, "style"))
			{
				this._styleColl.Clear();
				return;
			}
			this._bag.Remove(key);
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x000C57A3 File Offset: 0x000C47A3
		public void Clear()
		{
			this._bag.Clear();
			if (this._styleColl != null)
			{
				this._styleColl.Clear();
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000C57C4 File Offset: 0x000C47C4
		public void Render(HtmlTextWriter writer)
		{
			if (this._bag.Count > 0)
			{
				IDictionaryEnumerator enumerator = this._bag.GetEnumerator();
				while (enumerator.MoveNext())
				{
					StateItem stateItem = enumerator.Value as StateItem;
					if (stateItem != null)
					{
						string text = stateItem.Value as string;
						string text2 = enumerator.Key as string;
						if (text2 != null && text != null)
						{
							writer.WriteAttribute(text2, text, true);
						}
					}
				}
			}
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000C582C File Offset: 0x000C482C
		public void AddAttributes(HtmlTextWriter writer)
		{
			if (this._bag.Count > 0)
			{
				IDictionaryEnumerator enumerator = this._bag.GetEnumerator();
				while (enumerator.MoveNext())
				{
					StateItem stateItem = enumerator.Value as StateItem;
					if (stateItem != null)
					{
						string text = stateItem.Value as string;
						string text2 = enumerator.Key as string;
						if (text2 != null && text != null)
						{
							writer.AddAttribute(text2, text, true);
						}
					}
				}
			}
		}

		// Token: 0x0400207E RID: 8318
		private StateBag _bag;

		// Token: 0x0400207F RID: 8319
		private CssStyleCollection _styleColl;
	}
}
