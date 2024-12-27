using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200031E RID: 798
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
	public sealed class ObfuscationAttribute : Attribute
	{
		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x0004F2E2 File Offset: 0x0004E2E2
		// (set) Token: 0x06001F41 RID: 8001 RVA: 0x0004F2EA File Offset: 0x0004E2EA
		public bool StripAfterObfuscation
		{
			get
			{
				return this.m_strip;
			}
			set
			{
				this.m_strip = value;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001F42 RID: 8002 RVA: 0x0004F2F3 File Offset: 0x0004E2F3
		// (set) Token: 0x06001F43 RID: 8003 RVA: 0x0004F2FB File Offset: 0x0004E2FB
		public bool Exclude
		{
			get
			{
				return this.m_exclude;
			}
			set
			{
				this.m_exclude = value;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001F44 RID: 8004 RVA: 0x0004F304 File Offset: 0x0004E304
		// (set) Token: 0x06001F45 RID: 8005 RVA: 0x0004F30C File Offset: 0x0004E30C
		public bool ApplyToMembers
		{
			get
			{
				return this.m_applyToMembers;
			}
			set
			{
				this.m_applyToMembers = value;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x0004F315 File Offset: 0x0004E315
		// (set) Token: 0x06001F47 RID: 8007 RVA: 0x0004F31D File Offset: 0x0004E31D
		public string Feature
		{
			get
			{
				return this.m_feature;
			}
			set
			{
				this.m_feature = value;
			}
		}

		// Token: 0x04000D35 RID: 3381
		private bool m_strip = true;

		// Token: 0x04000D36 RID: 3382
		private bool m_exclude = true;

		// Token: 0x04000D37 RID: 3383
		private bool m_applyToMembers = true;

		// Token: 0x04000D38 RID: 3384
		private string m_feature = "all";
	}
}
