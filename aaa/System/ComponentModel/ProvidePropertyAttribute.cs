using System;

namespace System.ComponentModel
{
	// Token: 0x0200012E RID: 302
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class ProvidePropertyAttribute : Attribute
	{
		// Token: 0x060009B0 RID: 2480 RVA: 0x000200F8 File Offset: 0x0001F0F8
		public ProvidePropertyAttribute(string propertyName, Type receiverType)
		{
			this.propertyName = propertyName;
			this.receiverTypeName = receiverType.AssemblyQualifiedName;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00020113 File Offset: 0x0001F113
		public ProvidePropertyAttribute(string propertyName, string receiverTypeName)
		{
			this.propertyName = propertyName;
			this.receiverTypeName = receiverTypeName;
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00020129 File Offset: 0x0001F129
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00020131 File Offset: 0x0001F131
		public string ReceiverTypeName
		{
			get
			{
				return this.receiverTypeName;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00020139 File Offset: 0x0001F139
		public override object TypeId
		{
			get
			{
				return base.GetType().FullName + this.propertyName;
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00020154 File Offset: 0x0001F154
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ProvidePropertyAttribute providePropertyAttribute = obj as ProvidePropertyAttribute;
			return providePropertyAttribute != null && providePropertyAttribute.propertyName == this.propertyName && providePropertyAttribute.receiverTypeName == this.receiverTypeName;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00020197 File Offset: 0x0001F197
		public override int GetHashCode()
		{
			return this.propertyName.GetHashCode() ^ this.receiverTypeName.GetHashCode();
		}

		// Token: 0x04000A1C RID: 2588
		private readonly string propertyName;

		// Token: 0x04000A1D RID: 2589
		private readonly string receiverTypeName;
	}
}
