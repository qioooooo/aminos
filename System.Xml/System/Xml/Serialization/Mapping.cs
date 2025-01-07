using System;

namespace System.Xml.Serialization
{
	internal abstract class Mapping
	{
		internal Mapping()
		{
		}

		internal bool IsSoap
		{
			get
			{
				return this.isSoap;
			}
			set
			{
				this.isSoap = value;
			}
		}

		private bool isSoap;
	}
}
