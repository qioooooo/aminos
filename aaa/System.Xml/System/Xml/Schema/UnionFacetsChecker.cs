using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000200 RID: 512
	internal class UnionFacetsChecker : FacetsChecker
	{
		// Token: 0x06001857 RID: 6231 RVA: 0x0006CD18 File Offset: 0x0006BD18
		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			RestrictionFacets restriction = datatype.Restriction;
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			if ((restrictionFlags & RestrictionFlags.Enumeration) != (RestrictionFlags)0 && !this.MatchEnumeration(value, restriction.Enumeration, datatype))
			{
				return new XmlSchemaException("Sch_EnumerationConstraintFailed", string.Empty);
			}
			return null;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0006CD60 File Offset: 0x0006BD60
		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			foreach (object obj in enumeration)
			{
				if (datatype.Compare(value, obj) == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
