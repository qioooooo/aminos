using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x020001FE RID: 510
	internal class BinaryFacetsChecker : FacetsChecker
	{
		// Token: 0x0600184F RID: 6223 RVA: 0x0006CABC File Offset: 0x0006BABC
		internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
		{
			byte[] array = (byte[])value;
			return this.CheckValueFacets(array, datatype);
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0006CAD8 File Offset: 0x0006BAD8
		internal override Exception CheckValueFacets(byte[] value, XmlSchemaDatatype datatype)
		{
			RestrictionFacets restriction = datatype.Restriction;
			int num = value.Length;
			RestrictionFlags restrictionFlags = ((restriction != null) ? restriction.Flags : ((RestrictionFlags)0));
			if (restrictionFlags != (RestrictionFlags)0)
			{
				if ((restrictionFlags & RestrictionFlags.Length) != (RestrictionFlags)0 && restriction.Length != num)
				{
					return new XmlSchemaException("Sch_LengthConstraintFailed", string.Empty);
				}
				if ((restrictionFlags & RestrictionFlags.MinLength) != (RestrictionFlags)0 && num < restriction.MinLength)
				{
					return new XmlSchemaException("Sch_MinLengthConstraintFailed", string.Empty);
				}
				if ((restrictionFlags & RestrictionFlags.MaxLength) != (RestrictionFlags)0 && restriction.MaxLength < num)
				{
					return new XmlSchemaException("Sch_MaxLengthConstraintFailed", string.Empty);
				}
				if ((restrictionFlags & RestrictionFlags.Enumeration) != (RestrictionFlags)0 && !this.MatchEnumeration(value, restriction.Enumeration, datatype))
				{
					return new XmlSchemaException("Sch_EnumerationConstraintFailed", string.Empty);
				}
			}
			return null;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0006CB84 File Offset: 0x0006BB84
		internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			return this.MatchEnumeration((byte[])value, enumeration, datatype);
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0006CB94 File Offset: 0x0006BB94
		private bool MatchEnumeration(byte[] value, ArrayList enumeration, XmlSchemaDatatype datatype)
		{
			foreach (object obj in enumeration)
			{
				byte[] array = (byte[])obj;
				if (datatype.Compare(value, array) == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
