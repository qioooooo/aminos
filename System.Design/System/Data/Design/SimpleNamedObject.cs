using System;

namespace System.Data.Design
{
	internal class SimpleNamedObject : INamedObject
	{
		public SimpleNamedObject(object obj)
		{
			this._obj = obj;
		}

		public string Name
		{
			get
			{
				if (this._obj is INamedObject)
				{
					return (this._obj as INamedObject).Name;
				}
				if (this._obj is string)
				{
					return this._obj as string;
				}
				return this._obj.ToString();
			}
			set
			{
				if (this._obj is INamedObject)
				{
					(this._obj as INamedObject).Name = value;
					return;
				}
				if (this._obj is string)
				{
					this._obj = value;
				}
			}
		}

		private object _obj;
	}
}
