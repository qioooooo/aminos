using System;

namespace System.Xml.Schema
{
	// Token: 0x020001D7 RID: 471
	internal class Datatype_NOTATION : Datatype_anySimpleType
	{
		// Token: 0x060016F6 RID: 5878 RVA: 0x00063996 File Offset: 0x00062996
		internal override XmlValueConverter CreateValueConverter(XmlSchemaType schemaType)
		{
			return XmlMiscConverter.Create(schemaType);
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060016F7 RID: 5879 RVA: 0x0006399E File Offset: 0x0006299E
		internal override FacetsChecker FacetsChecker
		{
			get
			{
				return DatatypeImplementation.qnameFacetsChecker;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060016F8 RID: 5880 RVA: 0x000639A5 File Offset: 0x000629A5
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Notation;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x000639A9 File Offset: 0x000629A9
		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.NOTATION;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x000639AC File Offset: 0x000629AC
		internal override RestrictionFlags ValidRestrictionFlags
		{
			get
			{
				return RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength | RestrictionFlags.Pattern | RestrictionFlags.Enumeration | RestrictionFlags.WhiteSpace;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x000639B0 File Offset: 0x000629B0
		public override Type ValueType
		{
			get
			{
				return Datatype_NOTATION.atomicValueType;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x060016FC RID: 5884 RVA: 0x000639B7 File Offset: 0x000629B7
		internal override Type ListValueType
		{
			get
			{
				return Datatype_NOTATION.listValueType;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x000639BE File Offset: 0x000629BE
		internal override XmlSchemaWhiteSpace BuiltInWhitespaceFacet
		{
			get
			{
				return XmlSchemaWhiteSpace.Collapse;
			}
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x000639C4 File Offset: 0x000629C4
		internal override Exception TryParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr, out object typedValue)
		{
			typedValue = null;
			if (s == null || s.Length == 0)
			{
				return new XmlSchemaException("Sch_EmptyAttributeValue", string.Empty);
			}
			Exception ex = DatatypeImplementation.qnameFacetsChecker.CheckLexicalFacets(ref s, this);
			if (ex == null)
			{
				XmlQualifiedName xmlQualifiedName = null;
				try
				{
					string text;
					xmlQualifiedName = XmlQualifiedName.Parse(s, nsmgr, out text);
				}
				catch (ArgumentException ex2)
				{
					return ex2;
				}
				catch (XmlException ex3)
				{
					return ex3;
				}
				ex = DatatypeImplementation.qnameFacetsChecker.CheckValueFacets(xmlQualifiedName, this);
				if (ex == null)
				{
					typedValue = xmlQualifiedName;
					return null;
				}
			}
			return ex;
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00063A50 File Offset: 0x00062A50
		internal override void VerifySchemaValid(XmlSchemaObjectTable notations, XmlSchemaObject caller)
		{
			for (Datatype_NOTATION datatype_NOTATION = this; datatype_NOTATION != null; datatype_NOTATION = (Datatype_NOTATION)datatype_NOTATION.Base)
			{
				if (datatype_NOTATION.Restriction != null && (datatype_NOTATION.Restriction.Flags & RestrictionFlags.Enumeration) != (RestrictionFlags)0)
				{
					foreach (object obj in datatype_NOTATION.Restriction.Enumeration)
					{
						XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
						if (!notations.Contains(xmlQualifiedName))
						{
							throw new XmlSchemaException("Sch_NotationRequired", caller);
						}
					}
					return;
				}
			}
			throw new XmlSchemaException("Sch_NotationRequired", caller);
		}

		// Token: 0x04000D8F RID: 3471
		private static readonly Type atomicValueType = typeof(XmlQualifiedName);

		// Token: 0x04000D90 RID: 3472
		private static readonly Type listValueType = typeof(XmlQualifiedName[]);
	}
}
