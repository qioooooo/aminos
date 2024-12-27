using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002A7 RID: 679
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	[ComVisible(true)]
	public sealed class DebuggerTypeProxyAttribute : Attribute
	{
		// Token: 0x06001AD2 RID: 6866 RVA: 0x00046B49 File Offset: 0x00045B49
		public DebuggerTypeProxyAttribute(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00046B6B File Offset: 0x00045B6B
		public DebuggerTypeProxyAttribute(string typeName)
		{
			this.typeName = typeName;
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x00046B7A File Offset: 0x00045B7A
		public string ProxyTypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x00046BA5 File Offset: 0x00045BA5
		// (set) Token: 0x06001AD5 RID: 6869 RVA: 0x00046B82 File Offset: 0x00045B82
		public Type Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.targetName = value.AssemblyQualifiedName;
				this.target = value;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x00046BAD File Offset: 0x00045BAD
		// (set) Token: 0x06001AD8 RID: 6872 RVA: 0x00046BB5 File Offset: 0x00045BB5
		public string TargetTypeName
		{
			get
			{
				return this.targetName;
			}
			set
			{
				this.targetName = value;
			}
		}

		// Token: 0x04000A0E RID: 2574
		private string typeName;

		// Token: 0x04000A0F RID: 2575
		private string targetName;

		// Token: 0x04000A10 RID: 2576
		private Type target;
	}
}
