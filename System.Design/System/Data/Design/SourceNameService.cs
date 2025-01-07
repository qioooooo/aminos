using System;

namespace System.Data.Design
{
	internal class SourceNameService : SimpleNameService
	{
		internal new static SourceNameService DefaultInstance
		{
			get
			{
				if (SourceNameService.defaultInstance == null)
				{
					SourceNameService.defaultInstance = new SourceNameService();
				}
				return SourceNameService.defaultInstance;
			}
		}

		private static SourceNameService defaultInstance;
	}
}
