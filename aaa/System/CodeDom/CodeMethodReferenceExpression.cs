using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x02000064 RID: 100
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMethodReferenceExpression : CodeExpression
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x00013BF9 File Offset: 0x00012BF9
		public CodeMethodReferenceExpression()
		{
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00013C01 File Offset: 0x00012C01
		public CodeMethodReferenceExpression(CodeExpression targetObject, string methodName)
		{
			this.TargetObject = targetObject;
			this.MethodName = methodName;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00013C17 File Offset: 0x00012C17
		public CodeMethodReferenceExpression(CodeExpression targetObject, string methodName, params CodeTypeReference[] typeParameters)
		{
			this.TargetObject = targetObject;
			this.MethodName = methodName;
			if (typeParameters != null && typeParameters.Length > 0)
			{
				this.TypeArguments.AddRange(typeParameters);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00013C42 File Offset: 0x00012C42
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00013C4A File Offset: 0x00012C4A
		public CodeExpression TargetObject
		{
			get
			{
				return this.targetObject;
			}
			set
			{
				this.targetObject = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00013C53 File Offset: 0x00012C53
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00013C69 File Offset: 0x00012C69
		public string MethodName
		{
			get
			{
				if (this.methodName != null)
				{
					return this.methodName;
				}
				return string.Empty;
			}
			set
			{
				this.methodName = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00013C72 File Offset: 0x00012C72
		[ComVisible(false)]
		public CodeTypeReferenceCollection TypeArguments
		{
			get
			{
				if (this.typeArguments == null)
				{
					this.typeArguments = new CodeTypeReferenceCollection();
				}
				return this.typeArguments;
			}
		}

		// Token: 0x0400084E RID: 2126
		private CodeExpression targetObject;

		// Token: 0x0400084F RID: 2127
		private string methodName;

		// Token: 0x04000850 RID: 2128
		[OptionalField]
		private CodeTypeReferenceCollection typeArguments;
	}
}
