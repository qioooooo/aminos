using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E7 RID: 2023
	[Serializable]
	internal class BinaryMethodReturnMessage
	{
		// Token: 0x0600480B RID: 18443 RVA: 0x000FA9EC File Offset: 0x000F99EC
		internal BinaryMethodReturnMessage(object returnValue, object[] args, Exception e, LogicalCallContext callContext, object[] properties)
		{
			this._returnValue = returnValue;
			if (args == null)
			{
				args = new object[0];
			}
			this._outargs = args;
			this._args = args;
			this._exception = e;
			if (callContext == null)
			{
				this._logicalCallContext = new LogicalCallContext();
			}
			else
			{
				this._logicalCallContext = callContext;
			}
			this._properties = properties;
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x0600480C RID: 18444 RVA: 0x000FAA47 File Offset: 0x000F9A47
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600480D RID: 18445 RVA: 0x000FAA4F File Offset: 0x000F9A4F
		public object ReturnValue
		{
			get
			{
				return this._returnValue;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600480E RID: 18446 RVA: 0x000FAA57 File Offset: 0x000F9A57
		public object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x0600480F RID: 18447 RVA: 0x000FAA5F File Offset: 0x000F9A5F
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this._logicalCallContext;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06004810 RID: 18448 RVA: 0x000FAA67 File Offset: 0x000F9A67
		public bool HasProperties
		{
			get
			{
				return this._properties != null;
			}
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x000FAA78 File Offset: 0x000F9A78
		internal void PopulateMessageProperties(IDictionary dict)
		{
			foreach (DictionaryEntry dictionaryEntry in this._properties)
			{
				dict[dictionaryEntry.Key] = dictionaryEntry.Value;
			}
		}

		// Token: 0x040024FB RID: 9467
		private object[] _outargs;

		// Token: 0x040024FC RID: 9468
		private Exception _exception;

		// Token: 0x040024FD RID: 9469
		private object _returnValue;

		// Token: 0x040024FE RID: 9470
		private object[] _args;

		// Token: 0x040024FF RID: 9471
		private LogicalCallContext _logicalCallContext;

		// Token: 0x04002500 RID: 9472
		private object[] _properties;
	}
}
