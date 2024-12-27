using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200009B RID: 155
	internal class WhitespaceRuleLookup
	{
		// Token: 0x0600076A RID: 1898 RVA: 0x0002683B File Offset: 0x0002583B
		public WhitespaceRuleLookup()
		{
			this.qnames = new Hashtable();
			this.wildcards = new ArrayList();
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0002685C File Offset: 0x0002585C
		public WhitespaceRuleLookup(IList<WhitespaceRule> rules)
			: this()
		{
			for (int i = rules.Count - 1; i >= 0; i--)
			{
				WhitespaceRule whitespaceRule = rules[i];
				WhitespaceRuleLookup.InternalWhitespaceRule internalWhitespaceRule = new WhitespaceRuleLookup.InternalWhitespaceRule(whitespaceRule.LocalName, whitespaceRule.NamespaceName, whitespaceRule.PreserveSpace, -i);
				if (whitespaceRule.LocalName == null || whitespaceRule.NamespaceName == null)
				{
					this.wildcards.Add(internalWhitespaceRule);
				}
				else
				{
					this.qnames[internalWhitespaceRule] = internalWhitespaceRule;
				}
			}
			this.ruleTemp = new WhitespaceRuleLookup.InternalWhitespaceRule();
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000268DC File Offset: 0x000258DC
		public void Atomize(XmlNameTable nameTable)
		{
			if (nameTable != this.nameTable)
			{
				this.nameTable = nameTable;
				foreach (object obj in this.qnames.Values)
				{
					WhitespaceRuleLookup.InternalWhitespaceRule internalWhitespaceRule = (WhitespaceRuleLookup.InternalWhitespaceRule)obj;
					internalWhitespaceRule.Atomize(nameTable);
				}
				foreach (object obj2 in this.wildcards)
				{
					WhitespaceRuleLookup.InternalWhitespaceRule internalWhitespaceRule2 = (WhitespaceRuleLookup.InternalWhitespaceRule)obj2;
					internalWhitespaceRule2.Atomize(nameTable);
				}
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002699C File Offset: 0x0002599C
		public bool ShouldStripSpace(string localName, string namespaceName)
		{
			this.ruleTemp.Init(localName, namespaceName, false, 0);
			WhitespaceRuleLookup.InternalWhitespaceRule internalWhitespaceRule = this.qnames[this.ruleTemp] as WhitespaceRuleLookup.InternalWhitespaceRule;
			int count = this.wildcards.Count;
			while (count-- != 0)
			{
				WhitespaceRuleLookup.InternalWhitespaceRule internalWhitespaceRule2 = this.wildcards[count] as WhitespaceRuleLookup.InternalWhitespaceRule;
				if (internalWhitespaceRule != null)
				{
					if (internalWhitespaceRule.Priority > internalWhitespaceRule2.Priority)
					{
						return !internalWhitespaceRule.PreserveSpace;
					}
					if (internalWhitespaceRule.PreserveSpace == internalWhitespaceRule2.PreserveSpace)
					{
						continue;
					}
				}
				if ((internalWhitespaceRule2.LocalName == null || internalWhitespaceRule2.LocalName == localName) && (internalWhitespaceRule2.NamespaceName == null || internalWhitespaceRule2.NamespaceName == namespaceName))
				{
					return !internalWhitespaceRule2.PreserveSpace;
				}
			}
			return internalWhitespaceRule != null && !internalWhitespaceRule.PreserveSpace;
		}

		// Token: 0x0400050E RID: 1294
		private Hashtable qnames;

		// Token: 0x0400050F RID: 1295
		private ArrayList wildcards;

		// Token: 0x04000510 RID: 1296
		private WhitespaceRuleLookup.InternalWhitespaceRule ruleTemp;

		// Token: 0x04000511 RID: 1297
		private XmlNameTable nameTable;

		// Token: 0x0200009C RID: 156
		private class InternalWhitespaceRule : WhitespaceRule
		{
			// Token: 0x0600076E RID: 1902 RVA: 0x00026A59 File Offset: 0x00025A59
			public InternalWhitespaceRule()
			{
			}

			// Token: 0x0600076F RID: 1903 RVA: 0x00026A61 File Offset: 0x00025A61
			public InternalWhitespaceRule(string localName, string namespaceName, bool preserveSpace, int priority)
			{
				this.Init(localName, namespaceName, preserveSpace, priority);
			}

			// Token: 0x06000770 RID: 1904 RVA: 0x00026A74 File Offset: 0x00025A74
			public void Init(string localName, string namespaceName, bool preserveSpace, int priority)
			{
				base.Init(localName, namespaceName, preserveSpace);
				this.priority = priority;
				if (localName != null && namespaceName != null)
				{
					this.hashCode = localName.GetHashCode();
				}
			}

			// Token: 0x06000771 RID: 1905 RVA: 0x00026A99 File Offset: 0x00025A99
			public void Atomize(XmlNameTable nameTable)
			{
				if (base.LocalName != null)
				{
					base.LocalName = nameTable.Add(base.LocalName);
				}
				if (base.NamespaceName != null)
				{
					base.NamespaceName = nameTable.Add(base.NamespaceName);
				}
			}

			// Token: 0x17000119 RID: 281
			// (get) Token: 0x06000772 RID: 1906 RVA: 0x00026ACF File Offset: 0x00025ACF
			public int Priority
			{
				get
				{
					return this.priority;
				}
			}

			// Token: 0x06000773 RID: 1907 RVA: 0x00026AD7 File Offset: 0x00025AD7
			public override int GetHashCode()
			{
				return this.hashCode;
			}

			// Token: 0x06000774 RID: 1908 RVA: 0x00026AE0 File Offset: 0x00025AE0
			public override bool Equals(object obj)
			{
				WhitespaceRuleLookup.InternalWhitespaceRule internalWhitespaceRule = obj as WhitespaceRuleLookup.InternalWhitespaceRule;
				return base.LocalName == base.LocalName && base.NamespaceName == internalWhitespaceRule.NamespaceName;
			}

			// Token: 0x04000512 RID: 1298
			private int priority;

			// Token: 0x04000513 RID: 1299
			private int hashCode;
		}
	}
}
