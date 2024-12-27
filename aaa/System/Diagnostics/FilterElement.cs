using System;

namespace System.Diagnostics
{
	// Token: 0x020001C4 RID: 452
	internal class FilterElement : TypedElement
	{
		// Token: 0x06000E22 RID: 3618 RVA: 0x0002D047 File Offset: 0x0002C047
		public FilterElement()
			: base(typeof(TraceFilter))
		{
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0002D05C File Offset: 0x0002C05C
		public TraceFilter GetRuntimeObject()
		{
			TraceFilter traceFilter = (TraceFilter)base.BaseGetRuntimeObject();
			traceFilter.initializeData = base.InitData;
			return traceFilter;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0002D082 File Offset: 0x0002C082
		internal TraceFilter RefreshRuntimeObject(TraceFilter filter)
		{
			if (Type.GetType(this.TypeName) != filter.GetType() || base.InitData != filter.initializeData)
			{
				this._runtimeObject = null;
				return this.GetRuntimeObject();
			}
			return filter;
		}
	}
}
