using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000E1 RID: 225
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	[Serializable]
	public sealed class ObsoleteAttribute : Attribute
	{
		// Token: 0x06000C62 RID: 3170 RVA: 0x00025360 File Offset: 0x00024360
		public ObsoleteAttribute()
		{
			this._message = null;
			this._error = false;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00025376 File Offset: 0x00024376
		public ObsoleteAttribute(string message)
		{
			this._message = message;
			this._error = false;
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0002538C File Offset: 0x0002438C
		public ObsoleteAttribute(string message, bool error)
		{
			this._message = message;
			this._error = error;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x000253A2 File Offset: 0x000243A2
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x000253AA File Offset: 0x000243AA
		public bool IsError
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x0400042B RID: 1067
		private string _message;

		// Token: 0x0400042C RID: 1068
		private bool _error;
	}
}
