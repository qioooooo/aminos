using System;
using System.Reflection;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000077 RID: 119
	internal sealed class EarlyBoundInfo
	{
		// Token: 0x060006F1 RID: 1777 RVA: 0x00025455 File Offset: 0x00024455
		public EarlyBoundInfo(string namespaceUri, Type ebType)
		{
			this.namespaceUri = namespaceUri;
			this.constrInfo = ebType.GetConstructor(Type.EmptyTypes);
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00025475 File Offset: 0x00024475
		public string NamespaceUri
		{
			get
			{
				return this.namespaceUri;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0002547D File Offset: 0x0002447D
		public Type EarlyBoundType
		{
			get
			{
				return this.constrInfo.DeclaringType;
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0002548A File Offset: 0x0002448A
		public object CreateObject()
		{
			return this.constrInfo.Invoke(new object[0]);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000254A0 File Offset: 0x000244A0
		public override bool Equals(object obj)
		{
			EarlyBoundInfo earlyBoundInfo = obj as EarlyBoundInfo;
			return earlyBoundInfo != null && this.namespaceUri == earlyBoundInfo.namespaceUri && this.constrInfo == earlyBoundInfo.constrInfo;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000254DC File Offset: 0x000244DC
		public override int GetHashCode()
		{
			return this.namespaceUri.GetHashCode();
		}

		// Token: 0x04000462 RID: 1122
		private string namespaceUri;

		// Token: 0x04000463 RID: 1123
		private ConstructorInfo constrInfo;
	}
}
