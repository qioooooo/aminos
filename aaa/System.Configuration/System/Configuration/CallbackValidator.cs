using System;

namespace System.Configuration
{
	// Token: 0x02000014 RID: 20
	public sealed class CallbackValidator : ConfigurationValidatorBase
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x0000A908 File Offset: 0x00009908
		public CallbackValidator(Type type, ValidatorCallback callback)
			: this(callback)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._type = type;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000A926 File Offset: 0x00009926
		internal CallbackValidator(ValidatorCallback callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			this._type = null;
			this._callback = callback;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000A94A File Offset: 0x0000994A
		public override bool CanValidate(Type type)
		{
			return type == this._type || this._type == null;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000A960 File Offset: 0x00009960
		public override void Validate(object value)
		{
			this._callback(value);
		}

		// Token: 0x040001CE RID: 462
		private Type _type;

		// Token: 0x040001CF RID: 463
		private ValidatorCallback _callback;
	}
}
