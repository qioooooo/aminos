using System;
using System.Diagnostics;

namespace System.Runtime.Versioning
{
	// Token: 0x02000935 RID: 2357
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
	[Conditional("RESOURCE_ANNOTATION_WORK")]
	public sealed class ResourceExposureAttribute : Attribute
	{
		// Token: 0x06005582 RID: 21890 RVA: 0x0013727D File Offset: 0x0013627D
		public ResourceExposureAttribute(ResourceScope exposureLevel)
		{
			this._resourceExposureLevel = exposureLevel;
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06005583 RID: 21891 RVA: 0x0013728C File Offset: 0x0013628C
		public ResourceScope ResourceExposureLevel
		{
			get
			{
				return this._resourceExposureLevel;
			}
		}

		// Token: 0x04002C8C RID: 11404
		private ResourceScope _resourceExposureLevel;
	}
}
