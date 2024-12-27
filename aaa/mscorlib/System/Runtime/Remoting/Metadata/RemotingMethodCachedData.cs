using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x0200072D RID: 1837
	internal class RemotingMethodCachedData : RemotingCachedData
	{
		// Token: 0x06004229 RID: 16937 RVA: 0x000E2084 File Offset: 0x000E1084
		internal RemotingMethodCachedData(object ri)
			: base(ri)
		{
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x0600422A RID: 16938 RVA: 0x000E208D File Offset: 0x000E108D
		internal string TypeAndAssemblyName
		{
			get
			{
				if (this._typeAndAssemblyName == null)
				{
					this.UpdateNames();
				}
				return this._typeAndAssemblyName;
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x0600422B RID: 16939 RVA: 0x000E20A3 File Offset: 0x000E10A3
		internal string MethodName
		{
			get
			{
				if (this._methodName == null)
				{
					this.UpdateNames();
				}
				return this._methodName;
			}
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x000E20BC File Offset: 0x000E10BC
		private void UpdateNames()
		{
			MethodBase methodBase = (MethodBase)this.RI;
			this._methodName = methodBase.Name;
			if (methodBase.DeclaringType != null)
			{
				this._typeAndAssemblyName = RemotingServices.GetDefaultQualifiedTypeName(methodBase.DeclaringType);
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600422D RID: 16941 RVA: 0x000E20FA File Offset: 0x000E10FA
		internal ParameterInfo[] Parameters
		{
			get
			{
				if (this._parameters == null)
				{
					this._parameters = ((MethodBase)this.RI).GetParameters();
				}
				return this._parameters;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600422E RID: 16942 RVA: 0x000E2120 File Offset: 0x000E1120
		internal int[] OutRefArgMap
		{
			get
			{
				if (this._outRefArgMap == null)
				{
					this.GetArgMaps();
				}
				return this._outRefArgMap;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x0600422F RID: 16943 RVA: 0x000E2136 File Offset: 0x000E1136
		internal int[] OutOnlyArgMap
		{
			get
			{
				if (this._outOnlyArgMap == null)
				{
					this.GetArgMaps();
				}
				return this._outOnlyArgMap;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06004230 RID: 16944 RVA: 0x000E214C File Offset: 0x000E114C
		internal int[] NonRefOutArgMap
		{
			get
			{
				if (this._nonRefOutArgMap == null)
				{
					this.GetArgMaps();
				}
				return this._nonRefOutArgMap;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06004231 RID: 16945 RVA: 0x000E2162 File Offset: 0x000E1162
		internal int[] MarshalRequestArgMap
		{
			get
			{
				if (this._marshalRequestMap == null)
				{
					this.GetArgMaps();
				}
				return this._marshalRequestMap;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x000E2178 File Offset: 0x000E1178
		internal int[] MarshalResponseArgMap
		{
			get
			{
				if (this._marshalResponseMap == null)
				{
					this.GetArgMaps();
				}
				return this._marshalResponseMap;
			}
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x000E2190 File Offset: 0x000E1190
		private void GetArgMaps()
		{
			lock (this)
			{
				if (this._inRefArgMap == null)
				{
					int[] array = null;
					int[] array2 = null;
					int[] array3 = null;
					int[] array4 = null;
					int[] array5 = null;
					int[] array6 = null;
					ArgMapper.GetParameterMaps(this.Parameters, out array, out array2, out array3, out array4, out array5, out array6);
					this._inRefArgMap = array;
					this._outRefArgMap = array2;
					this._outOnlyArgMap = array3;
					this._nonRefOutArgMap = array4;
					this._marshalRequestMap = array5;
					this._marshalResponseMap = array6;
				}
			}
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x000E221C File Offset: 0x000E121C
		internal bool IsOneWayMethod()
		{
			if ((this.flags & RemotingMethodCachedData.MethodCacheFlags.CheckedOneWay) == RemotingMethodCachedData.MethodCacheFlags.None)
			{
				RemotingMethodCachedData.MethodCacheFlags methodCacheFlags = RemotingMethodCachedData.MethodCacheFlags.CheckedOneWay;
				object[] customAttributes = ((ICustomAttributeProvider)this.RI).GetCustomAttributes(typeof(OneWayAttribute), true);
				if (customAttributes != null && customAttributes.Length > 0)
				{
					methodCacheFlags |= RemotingMethodCachedData.MethodCacheFlags.IsOneWay;
				}
				this.flags |= methodCacheFlags;
				return (methodCacheFlags & RemotingMethodCachedData.MethodCacheFlags.IsOneWay) != RemotingMethodCachedData.MethodCacheFlags.None;
			}
			return (this.flags & RemotingMethodCachedData.MethodCacheFlags.IsOneWay) != RemotingMethodCachedData.MethodCacheFlags.None;
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x000E2284 File Offset: 0x000E1284
		internal bool IsOverloaded()
		{
			if ((this.flags & RemotingMethodCachedData.MethodCacheFlags.CheckedOverloaded) == RemotingMethodCachedData.MethodCacheFlags.None)
			{
				RemotingMethodCachedData.MethodCacheFlags methodCacheFlags = RemotingMethodCachedData.MethodCacheFlags.CheckedOverloaded;
				MethodBase methodBase = (MethodBase)this.RI;
				if (methodBase.IsOverloaded)
				{
					methodCacheFlags |= RemotingMethodCachedData.MethodCacheFlags.IsOverloaded;
				}
				this.flags |= methodCacheFlags;
				return (methodCacheFlags & RemotingMethodCachedData.MethodCacheFlags.IsOverloaded) != RemotingMethodCachedData.MethodCacheFlags.None;
			}
			return (this.flags & RemotingMethodCachedData.MethodCacheFlags.IsOverloaded) != RemotingMethodCachedData.MethodCacheFlags.None;
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06004236 RID: 16950 RVA: 0x000E22DC File Offset: 0x000E12DC
		internal Type ReturnType
		{
			get
			{
				if ((this.flags & RemotingMethodCachedData.MethodCacheFlags.CheckedForReturnType) == RemotingMethodCachedData.MethodCacheFlags.None)
				{
					MethodInfo methodInfo = this.RI as MethodInfo;
					if (methodInfo != null)
					{
						Type returnType = methodInfo.ReturnType;
						if (returnType != typeof(void))
						{
							this._returnType = returnType;
						}
					}
					this.flags |= RemotingMethodCachedData.MethodCacheFlags.CheckedForReturnType;
				}
				return this._returnType;
			}
		}

		// Token: 0x040020FE RID: 8446
		private ParameterInfo[] _parameters;

		// Token: 0x040020FF RID: 8447
		private RemotingMethodCachedData.MethodCacheFlags flags;

		// Token: 0x04002100 RID: 8448
		private string _typeAndAssemblyName;

		// Token: 0x04002101 RID: 8449
		private string _methodName;

		// Token: 0x04002102 RID: 8450
		private Type _returnType;

		// Token: 0x04002103 RID: 8451
		private int[] _inRefArgMap;

		// Token: 0x04002104 RID: 8452
		private int[] _outRefArgMap;

		// Token: 0x04002105 RID: 8453
		private int[] _outOnlyArgMap;

		// Token: 0x04002106 RID: 8454
		private int[] _nonRefOutArgMap;

		// Token: 0x04002107 RID: 8455
		private int[] _marshalRequestMap;

		// Token: 0x04002108 RID: 8456
		private int[] _marshalResponseMap;

		// Token: 0x0200072E RID: 1838
		[Flags]
		[Serializable]
		private enum MethodCacheFlags
		{
			// Token: 0x0400210A RID: 8458
			None = 0,
			// Token: 0x0400210B RID: 8459
			CheckedOneWay = 1,
			// Token: 0x0400210C RID: 8460
			IsOneWay = 2,
			// Token: 0x0400210D RID: 8461
			CheckedOverloaded = 4,
			// Token: 0x0400210E RID: 8462
			IsOverloaded = 8,
			// Token: 0x0400210F RID: 8463
			CheckedForAsync = 16,
			// Token: 0x04002110 RID: 8464
			CheckedForReturnType = 32
		}
	}
}
