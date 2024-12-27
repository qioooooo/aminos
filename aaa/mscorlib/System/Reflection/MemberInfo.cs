using System;
using System.Reflection.Cache;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x020000F0 RID: 240
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MemberInfo))]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class MemberInfo : ICustomAttributeProvider, _MemberInfo
	{
		// Token: 0x06000CAE RID: 3246 RVA: 0x00025C05 File Offset: 0x00024C05
		internal virtual bool CacheEquals(object o)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00025C0C File Offset: 0x00024C0C
		internal InternalCache Cache
		{
			get
			{
				InternalCache internalCache = this.m_cachedData;
				if (internalCache == null)
				{
					internalCache = new InternalCache("MemberInfo");
					InternalCache internalCache2 = Interlocked.CompareExchange<InternalCache>(ref this.m_cachedData, internalCache, null);
					if (internalCache2 != null)
					{
						internalCache = internalCache2;
					}
					GC.ClearCache += this.OnCacheClear;
				}
				return internalCache;
			}
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00025C53 File Offset: 0x00024C53
		internal void OnCacheClear(object sender, ClearCacheEventArgs cacheEventArgs)
		{
			this.m_cachedData = null;
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000CB1 RID: 3249
		public abstract MemberTypes MemberType { get; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000CB2 RID: 3250
		public abstract string Name { get; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000CB3 RID: 3251
		public abstract Type DeclaringType { get; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000CB4 RID: 3252
		public abstract Type ReflectedType { get; }

		// Token: 0x06000CB5 RID: 3253
		public abstract object[] GetCustomAttributes(bool inherit);

		// Token: 0x06000CB6 RID: 3254
		public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06000CB7 RID: 3255
		public abstract bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00025C5C File Offset: 0x00024C5C
		public virtual int MetadataToken
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00025C63 File Offset: 0x00024C63
		internal virtual int MetadataTokenInternal
		{
			get
			{
				return this.MetadataToken;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00025C6B File Offset: 0x00024C6B
		public virtual Module Module
		{
			get
			{
				if (this is Type)
				{
					return ((Type)this).Module;
				}
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00025C86 File Offset: 0x00024C86
		Type _MemberInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00025C8E File Offset: 0x00024C8E
		void _MemberInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00025C95 File Offset: 0x00024C95
		void _MemberInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00025C9C File Offset: 0x00024C9C
		void _MemberInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00025CA3 File Offset: 0x00024CA3
		void _MemberInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000486 RID: 1158
		internal const uint INVOCATION_FLAGS_UNKNOWN = 0U;

		// Token: 0x04000487 RID: 1159
		internal const uint INVOCATION_FLAGS_INITIALIZED = 1U;

		// Token: 0x04000488 RID: 1160
		internal const uint INVOCATION_FLAGS_NO_INVOKE = 2U;

		// Token: 0x04000489 RID: 1161
		internal const uint INVOCATION_FLAGS_NEED_SECURITY = 4U;

		// Token: 0x0400048A RID: 1162
		internal const uint INVOCATION_FLAGS_NO_CTOR_INVOKE = 8U;

		// Token: 0x0400048B RID: 1163
		internal const uint INVOCATION_FLAGS_IS_CTOR = 16U;

		// Token: 0x0400048C RID: 1164
		internal const uint INVOCATION_FLAGS_RISKY_METHOD = 32U;

		// Token: 0x0400048D RID: 1165
		internal const uint INVOCATION_FLAGS_SECURITY_IMPOSED = 64U;

		// Token: 0x0400048E RID: 1166
		internal const uint INVOCATION_FLAGS_IS_DELEGATE_CTOR = 128U;

		// Token: 0x0400048F RID: 1167
		internal const uint INVOCATION_FLAGS_CONTAINS_STACK_POINTERS = 256U;

		// Token: 0x04000490 RID: 1168
		internal const uint INVOCATION_FLAGS_SPECIAL_FIELD = 16U;

		// Token: 0x04000491 RID: 1169
		internal const uint INVOCATION_FLAGS_FIELD_SPECIAL_CAST = 32U;

		// Token: 0x04000492 RID: 1170
		internal const uint INVOCATION_FLAGS_CONSTRUCTOR_INVOKE = 268435456U;

		// Token: 0x04000493 RID: 1171
		private InternalCache m_cachedData;
	}
}
