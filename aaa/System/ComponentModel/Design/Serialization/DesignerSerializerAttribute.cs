using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001A6 RID: 422
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	public sealed class DesignerSerializerAttribute : Attribute
	{
		// Token: 0x06000D0B RID: 3339 RVA: 0x0002A579 File Offset: 0x00029579
		public DesignerSerializerAttribute(Type serializerType, Type baseSerializerType)
		{
			this.serializerTypeName = serializerType.AssemblyQualifiedName;
			this.serializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0002A599 File Offset: 0x00029599
		public DesignerSerializerAttribute(string serializerTypeName, Type baseSerializerType)
		{
			this.serializerTypeName = serializerTypeName;
			this.serializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0002A5B4 File Offset: 0x000295B4
		public DesignerSerializerAttribute(string serializerTypeName, string baseSerializerTypeName)
		{
			this.serializerTypeName = serializerTypeName;
			this.serializerBaseTypeName = baseSerializerTypeName;
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0002A5CA File Offset: 0x000295CA
		public string SerializerTypeName
		{
			get
			{
				return this.serializerTypeName;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0002A5D2 File Offset: 0x000295D2
		public string SerializerBaseTypeName
		{
			get
			{
				return this.serializerBaseTypeName;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0002A5DC File Offset: 0x000295DC
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					string text = this.serializerBaseTypeName;
					int num = text.IndexOf(',');
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					this.typeId = base.GetType().FullName + text;
				}
				return this.typeId;
			}
		}

		// Token: 0x04000EAA RID: 3754
		private string serializerTypeName;

		// Token: 0x04000EAB RID: 3755
		private string serializerBaseTypeName;

		// Token: 0x04000EAC RID: 3756
		private string typeId;
	}
}
