using System;

namespace System.Configuration
{
	// Token: 0x020000A2 RID: 162
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SubclassTypeValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x0600064B RID: 1611 RVA: 0x0001CFD8 File Offset: 0x0001BFD8
		public SubclassTypeValidatorAttribute(Type baseClass)
		{
			this._baseClass = baseClass;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0001CFE7 File Offset: 0x0001BFE7
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return new SubclassTypeValidator(this._baseClass);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001CFF4 File Offset: 0x0001BFF4
		public Type BaseClass
		{
			get
			{
				return this._baseClass;
			}
		}

		// Token: 0x040003ED RID: 1005
		private Type _baseClass;
	}
}
