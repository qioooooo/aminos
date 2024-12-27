using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000078 RID: 120
	public class ErrorObject : JSObject
	{
		// Token: 0x06000593 RID: 1427 RVA: 0x000270E8 File Offset: 0x000260E8
		internal ErrorObject(ErrorPrototype parent, object[] args)
			: base(parent)
		{
			this.exception = null;
			this.description = "";
			this.number = 0;
			if (args.Length == 1)
			{
				if (args[0] == null || Convert.IsPrimitiveNumericType(args[0].GetType()))
				{
					this.number = Convert.ToNumber(args[0]);
				}
				else
				{
					this.description = Convert.ToString(args[0]);
				}
			}
			else if (args.Length > 1)
			{
				this.number = Convert.ToNumber(args[0]);
				this.description = Convert.ToString(args[1]);
			}
			this.message = this.description;
			this.noExpando = false;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00027194 File Offset: 0x00026194
		internal ErrorObject(ErrorPrototype parent, object e)
			: base(parent)
		{
			this.exception = e;
			this.number = -2146823266;
			if (e is Exception)
			{
				if (e is JScriptException)
				{
					this.number = ((JScriptException)e).Number;
				}
				else if (e is ExternalException)
				{
					this.number = ((ExternalException)e).ErrorCode;
				}
				this.description = ((Exception)e).Message;
				if (((string)this.description).Length == 0)
				{
					this.description = e.GetType().FullName;
				}
			}
			this.message = this.description;
			this.noExpando = false;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0002724C File Offset: 0x0002624C
		internal override string GetClassName()
		{
			return "Error";
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00027253 File Offset: 0x00026253
		internal string Message
		{
			get
			{
				return Convert.ToString(this.message);
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00027260 File Offset: 0x00026260
		public static explicit operator Exception(ErrorObject err)
		{
			return err.exception as Exception;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0002726D File Offset: 0x0002626D
		public static Exception ToException(ErrorObject err)
		{
			return (Exception)err;
		}

		// Token: 0x04000267 RID: 615
		public object message;

		// Token: 0x04000268 RID: 616
		public object number;

		// Token: 0x04000269 RID: 617
		public object description;

		// Token: 0x0400026A RID: 618
		internal object exception;
	}
}
