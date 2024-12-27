using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001B4 RID: 436
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	[Obsolete("This attribute has been deprecated. Use DesignerSerializerAttribute instead.  For example, to specify a root designer for CodeDom, use DesignerSerializerAttribute(...,typeof(TypeCodeDomSerializer)).  http://go.microsoft.com/fwlink/?linkid=14202")]
	public sealed class RootDesignerSerializerAttribute : Attribute
	{
		// Token: 0x06000D53 RID: 3411 RVA: 0x0002AC94 File Offset: 0x00029C94
		public RootDesignerSerializerAttribute(Type serializerType, Type baseSerializerType, bool reloadable)
		{
			this.serializerTypeName = serializerType.AssemblyQualifiedName;
			this.serializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
			this.reloadable = reloadable;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0002ACBB File Offset: 0x00029CBB
		public RootDesignerSerializerAttribute(string serializerTypeName, Type baseSerializerType, bool reloadable)
		{
			this.serializerTypeName = serializerTypeName;
			this.serializerBaseTypeName = baseSerializerType.AssemblyQualifiedName;
			this.reloadable = reloadable;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0002ACDD File Offset: 0x00029CDD
		public RootDesignerSerializerAttribute(string serializerTypeName, string baseSerializerTypeName, bool reloadable)
		{
			this.serializerTypeName = serializerTypeName;
			this.serializerBaseTypeName = baseSerializerTypeName;
			this.reloadable = reloadable;
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0002ACFA File Offset: 0x00029CFA
		public bool Reloadable
		{
			get
			{
				return this.reloadable;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x0002AD02 File Offset: 0x00029D02
		public string SerializerTypeName
		{
			get
			{
				return this.serializerTypeName;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0002AD0A File Offset: 0x00029D0A
		public string SerializerBaseTypeName
		{
			get
			{
				return this.serializerBaseTypeName;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x0002AD14 File Offset: 0x00029D14
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

		// Token: 0x04000EB9 RID: 3769
		private bool reloadable;

		// Token: 0x04000EBA RID: 3770
		private string serializerTypeName;

		// Token: 0x04000EBB RID: 3771
		private string serializerBaseTypeName;

		// Token: 0x04000EBC RID: 3772
		private string typeId;
	}
}
