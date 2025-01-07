using System;

namespace System.Data.Design
{
	internal class DataSetNameService : SimpleNameService
	{
		internal new static DataSetNameService DefaultInstance
		{
			get
			{
				if (DataSetNameService.defaultInstance == null)
				{
					DataSetNameService.defaultInstance = new DataSetNameService();
				}
				return DataSetNameService.defaultInstance;
			}
		}

		public override void ValidateName(string name)
		{
		}

		private static DataSetNameService defaultInstance;
	}
}
