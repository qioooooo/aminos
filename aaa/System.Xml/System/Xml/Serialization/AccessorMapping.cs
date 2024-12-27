using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x020002CD RID: 717
	internal abstract class AccessorMapping : Mapping
	{
		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x060021EF RID: 8687 RVA: 0x0009F8E8 File Offset: 0x0009E8E8
		internal bool IsAttribute
		{
			get
			{
				return this.attribute != null;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x060021F0 RID: 8688 RVA: 0x0009F8F6 File Offset: 0x0009E8F6
		internal bool IsText
		{
			get
			{
				return this.text != null && (this.elements == null || this.elements.Length == 0);
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x060021F1 RID: 8689 RVA: 0x0009F917 File Offset: 0x0009E917
		internal bool IsParticle
		{
			get
			{
				return this.elements != null && this.elements.Length > 0;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x0009F92E File Offset: 0x0009E92E
		// (set) Token: 0x060021F3 RID: 8691 RVA: 0x0009F936 File Offset: 0x0009E936
		internal TypeDesc TypeDesc
		{
			get
			{
				return this.typeDesc;
			}
			set
			{
				this.typeDesc = value;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x0009F93F File Offset: 0x0009E93F
		// (set) Token: 0x060021F5 RID: 8693 RVA: 0x0009F947 File Offset: 0x0009E947
		internal AttributeAccessor Attribute
		{
			get
			{
				return this.attribute;
			}
			set
			{
				this.attribute = value;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x0009F950 File Offset: 0x0009E950
		// (set) Token: 0x060021F7 RID: 8695 RVA: 0x0009F958 File Offset: 0x0009E958
		internal ElementAccessor[] Elements
		{
			get
			{
				return this.elements;
			}
			set
			{
				this.elements = value;
				this.sortedElements = null;
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0009F968 File Offset: 0x0009E968
		internal static void SortMostToLeastDerived(ElementAccessor[] elements)
		{
			Array.Sort(elements, new AccessorMapping.AccessorComparer());
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x0009F978 File Offset: 0x0009E978
		internal ElementAccessor[] ElementsSortedByDerivation
		{
			get
			{
				if (this.sortedElements != null)
				{
					return this.sortedElements;
				}
				if (this.elements == null)
				{
					return null;
				}
				this.sortedElements = new ElementAccessor[this.elements.Length];
				Array.Copy(this.elements, 0, this.sortedElements, 0, this.elements.Length);
				AccessorMapping.SortMostToLeastDerived(this.sortedElements);
				return this.sortedElements;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x0009F9DD File Offset: 0x0009E9DD
		// (set) Token: 0x060021FB RID: 8699 RVA: 0x0009F9E5 File Offset: 0x0009E9E5
		internal TextAccessor Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x060021FC RID: 8700 RVA: 0x0009F9EE File Offset: 0x0009E9EE
		// (set) Token: 0x060021FD RID: 8701 RVA: 0x0009F9F6 File Offset: 0x0009E9F6
		internal ChoiceIdentifierAccessor ChoiceIdentifier
		{
			get
			{
				return this.choiceIdentifier;
			}
			set
			{
				this.choiceIdentifier = value;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x0009F9FF File Offset: 0x0009E9FF
		// (set) Token: 0x060021FF RID: 8703 RVA: 0x0009FA07 File Offset: 0x0009EA07
		internal XmlnsAccessor Xmlns
		{
			get
			{
				return this.xmlns;
			}
			set
			{
				this.xmlns = value;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x0009FA10 File Offset: 0x0009EA10
		// (set) Token: 0x06002201 RID: 8705 RVA: 0x0009FA18 File Offset: 0x0009EA18
		internal bool Ignore
		{
			get
			{
				return this.ignore;
			}
			set
			{
				this.ignore = value;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002202 RID: 8706 RVA: 0x0009FA24 File Offset: 0x0009EA24
		internal Accessor Accessor
		{
			get
			{
				if (this.xmlns != null)
				{
					return this.xmlns;
				}
				if (this.attribute != null)
				{
					return this.attribute;
				}
				if (this.elements != null && this.elements.Length > 0)
				{
					return this.elements[0];
				}
				return this.text;
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x0009FA74 File Offset: 0x0009EA74
		private static bool IsNeedNullableMember(ElementAccessor element)
		{
			if (element.Mapping is ArrayMapping)
			{
				ArrayMapping arrayMapping = (ArrayMapping)element.Mapping;
				return arrayMapping.Elements != null && arrayMapping.Elements.Length == 1 && AccessorMapping.IsNeedNullableMember(arrayMapping.Elements[0]);
			}
			return element.IsNullable && element.Mapping.TypeDesc.IsValueType;
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002204 RID: 8708 RVA: 0x0009FAD7 File Offset: 0x0009EAD7
		internal bool IsNeedNullable
		{
			get
			{
				return this.xmlns == null && this.attribute == null && (this.elements != null && this.elements.Length == 1) && AccessorMapping.IsNeedNullableMember(this.elements[0]);
			}
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x0009FB10 File Offset: 0x0009EB10
		internal static bool ElementsMatch(ElementAccessor[] a, ElementAccessor[] b)
		{
			if (a == null)
			{
				return b == null;
			}
			if (b == null)
			{
				return false;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i].Name != b[i].Name || a[i].Namespace != b[i].Namespace || a[i].Form != b[i].Form || a[i].IsNullable != b[i].IsNullable)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x0009FB9C File Offset: 0x0009EB9C
		internal bool Match(AccessorMapping mapping)
		{
			if (this.Elements != null && this.Elements.Length > 0)
			{
				if (!AccessorMapping.ElementsMatch(this.Elements, mapping.Elements))
				{
					return false;
				}
				if (this.Text == null)
				{
					return mapping.Text == null;
				}
			}
			if (this.Attribute != null)
			{
				return mapping.Attribute != null && (this.Attribute.Name == mapping.Attribute.Name && this.Attribute.Namespace == mapping.Attribute.Namespace) && this.Attribute.Form == mapping.Attribute.Form;
			}
			if (this.Text != null)
			{
				return mapping.Text != null;
			}
			return mapping.Accessor == null;
		}

		// Token: 0x0400148B RID: 5259
		private TypeDesc typeDesc;

		// Token: 0x0400148C RID: 5260
		private AttributeAccessor attribute;

		// Token: 0x0400148D RID: 5261
		private ElementAccessor[] elements;

		// Token: 0x0400148E RID: 5262
		private ElementAccessor[] sortedElements;

		// Token: 0x0400148F RID: 5263
		private TextAccessor text;

		// Token: 0x04001490 RID: 5264
		private ChoiceIdentifierAccessor choiceIdentifier;

		// Token: 0x04001491 RID: 5265
		private XmlnsAccessor xmlns;

		// Token: 0x04001492 RID: 5266
		private bool ignore;

		// Token: 0x020002CE RID: 718
		internal class AccessorComparer : IComparer
		{
			// Token: 0x06002208 RID: 8712 RVA: 0x0009FC70 File Offset: 0x0009EC70
			public int Compare(object o1, object o2)
			{
				if (o1 == o2)
				{
					return 0;
				}
				Accessor accessor = (Accessor)o1;
				Accessor accessor2 = (Accessor)o2;
				int weight = accessor.Mapping.TypeDesc.Weight;
				int weight2 = accessor2.Mapping.TypeDesc.Weight;
				if (weight == weight2)
				{
					return 0;
				}
				if (weight < weight2)
				{
					return 1;
				}
				return -1;
			}
		}
	}
}
