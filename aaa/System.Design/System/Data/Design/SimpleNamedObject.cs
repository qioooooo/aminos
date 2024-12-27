using System;

namespace System.Data.Design
{
	// Token: 0x020000B6 RID: 182
	internal class SimpleNamedObject : INamedObject
	{
		// Token: 0x06000831 RID: 2097 RVA: 0x00014EE0 File Offset: 0x00013EE0
		public SimpleNamedObject(object obj)
		{
			this._obj = obj;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00014EF0 File Offset: 0x00013EF0
		// (set) Token: 0x06000833 RID: 2099 RVA: 0x00014F3F File Offset: 0x00013F3F
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

		// Token: 0x04000C06 RID: 3078
		private object _obj;
	}
}
