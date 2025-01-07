using System;

namespace System.Diagnostics.Design
{
	internal class EditableDictionaryEntry
	{
		public EditableDictionaryEntry(string name, string value)
		{
			this._name = name;
			this._value = value;
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		public string _name;

		public string _value;
	}
}
