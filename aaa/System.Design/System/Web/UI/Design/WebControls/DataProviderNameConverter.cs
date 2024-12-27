using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000443 RID: 1091
	public class DataProviderNameConverter : StringConverter
	{
		// Token: 0x06002782 RID: 10114 RVA: 0x000D8454 File Offset: 0x000D7454
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			DataTable factoryClasses = DbProviderFactories.GetFactoryClasses();
			DataRowCollection rows = factoryClasses.Rows;
			string[] array = new string[rows.Count];
			for (int i = 0; i < rows.Count; i++)
			{
				array[i] = (string)rows[i]["InvariantName"];
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000D84AA File Offset: 0x000D74AA
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000D84AD File Offset: 0x000D74AD
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
