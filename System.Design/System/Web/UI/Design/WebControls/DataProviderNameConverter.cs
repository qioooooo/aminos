using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace System.Web.UI.Design.WebControls
{
	public class DataProviderNameConverter : StringConverter
	{
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

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
