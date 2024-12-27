using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200003E RID: 62
	internal class SmiExtendedMetaData : SmiMetaData
	{
		// Token: 0x06000247 RID: 583 RVA: 0x001CC6A0 File Offset: 0x001CBAA0
		[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped. Use ctor without columns param.")]
		internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, SmiMetaData[] columns, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
			: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
		{
		}

		// Token: 0x06000248 RID: 584 RVA: 0x001CC6C8 File Offset: 0x001CBAC8
		internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
			: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, false, null, null, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x001CC6F4 File Offset: 0x001CBAF4
		internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
			: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, null, isMultiValued, fieldMetaData, extendedProperties, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x001CC724 File Offset: 0x001CBB24
		internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string udtAssemblyQualifiedName, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
			: base(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, udtAssemblyQualifiedName, isMultiValued, fieldMetaData, extendedProperties)
		{
			this._name = name;
			this._typeSpecificNamePart1 = typeSpecificNamePart1;
			this._typeSpecificNamePart2 = typeSpecificNamePart2;
			this._typeSpecificNamePart3 = typeSpecificNamePart3;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600024B RID: 587 RVA: 0x001CC76C File Offset: 0x001CBB6C
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600024C RID: 588 RVA: 0x001CC780 File Offset: 0x001CBB80
		internal string TypeSpecificNamePart1
		{
			get
			{
				return this._typeSpecificNamePart1;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600024D RID: 589 RVA: 0x001CC794 File Offset: 0x001CBB94
		internal string TypeSpecificNamePart2
		{
			get
			{
				return this._typeSpecificNamePart2;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600024E RID: 590 RVA: 0x001CC7A8 File Offset: 0x001CBBA8
		internal string TypeSpecificNamePart3
		{
			get
			{
				return this._typeSpecificNamePart3;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x001CC7BC File Offset: 0x001CBBBC
		internal override string TraceString(int indent)
		{
			return string.Format(CultureInfo.InvariantCulture, "{2}                 Name={0}{1}{2}TypeSpecificNamePart1='{3}'\n\t{2}TypeSpecificNamePart2='{4}'\n\t{2}TypeSpecificNamePart3='{5}'\n\t", new object[]
			{
				(this._name != null) ? this._name : "<null>",
				base.TraceString(indent),
				new string(' ', indent),
				(this.TypeSpecificNamePart1 != null) ? this.TypeSpecificNamePart1 : "<null>",
				(this.TypeSpecificNamePart2 != null) ? this.TypeSpecificNamePart2 : "<null>",
				(this.TypeSpecificNamePart3 != null) ? this.TypeSpecificNamePart3 : "<null>"
			});
		}

		// Token: 0x040005CC RID: 1484
		private string _name;

		// Token: 0x040005CD RID: 1485
		private string _typeSpecificNamePart1;

		// Token: 0x040005CE RID: 1486
		private string _typeSpecificNamePart2;

		// Token: 0x040005CF RID: 1487
		private string _typeSpecificNamePart3;
	}
}
