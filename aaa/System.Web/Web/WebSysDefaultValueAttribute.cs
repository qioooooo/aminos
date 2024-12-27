using System;
using System.ComponentModel;

namespace System.Web
{
	// Token: 0x020000EC RID: 236
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class WebSysDefaultValueAttribute : DefaultValueAttribute
	{
		// Token: 0x06000B0F RID: 2831 RVA: 0x0002C405 File Offset: 0x0002B405
		internal WebSysDefaultValueAttribute(Type type, string value)
			: base(value)
		{
			this._type = type;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0002C415 File Offset: 0x0002B415
		internal WebSysDefaultValueAttribute(string value)
			: base(value)
		{
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0002C41E File Offset: 0x0002B41E
		public override object TypeId
		{
			get
			{
				return typeof(DefaultValueAttribute);
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x0002C42C File Offset: 0x0002B42C
		public override object Value
		{
			get
			{
				if (!this._localized)
				{
					this._localized = true;
					string text = (string)base.Value;
					if (!string.IsNullOrEmpty(text))
					{
						object obj = SR.GetString(text);
						if (this._type != null)
						{
							try
							{
								obj = TypeDescriptor.GetConverter(this._type).ConvertFromInvariantString((string)obj);
							}
							catch (NotSupportedException)
							{
								obj = null;
							}
						}
						base.SetValue(obj);
					}
				}
				return base.Value;
			}
		}

		// Token: 0x0400133E RID: 4926
		private Type _type;

		// Token: 0x0400133F RID: 4927
		private bool _localized;
	}
}
