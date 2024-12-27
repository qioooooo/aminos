using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Cache;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x02000754 RID: 1876
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class InternalRemotingServices
	{
		// Token: 0x06004377 RID: 17271 RVA: 0x000E79DD File Offset: 0x000E69DD
		[Conditional("_LOGGING")]
		public static void DebugOutChnl(string s)
		{
			Message.OutToUnmanagedDebugger("CHNL:" + s + "\n");
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x000E79F4 File Offset: 0x000E69F4
		[Conditional("_LOGGING")]
		public static void RemotingTrace(params object[] messages)
		{
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x000E79F6 File Offset: 0x000E69F6
		[Conditional("_DEBUG")]
		public static void RemotingAssert(bool condition, string message)
		{
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x000E79F8 File Offset: 0x000E69F8
		[CLSCompliant(false)]
		public static void SetServerIdentity(MethodCall m, object srvID)
		{
			((IInternalMessage)m).ServerIdentityObject = (ServerIdentity)srvID;
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x000E7A14 File Offset: 0x000E6A14
		internal static RemotingMethodCachedData GetReflectionCachedData(MethodBase mi)
		{
			RemotingMethodCachedData remotingMethodCachedData = (RemotingMethodCachedData)mi.Cache[CacheObjType.RemotingData];
			if (remotingMethodCachedData == null)
			{
				remotingMethodCachedData = new RemotingMethodCachedData(mi);
				mi.Cache[CacheObjType.RemotingData] = remotingMethodCachedData;
			}
			return remotingMethodCachedData;
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x000E7A50 File Offset: 0x000E6A50
		internal static RemotingTypeCachedData GetReflectionCachedData(Type mi)
		{
			RemotingTypeCachedData remotingTypeCachedData = (RemotingTypeCachedData)mi.Cache[CacheObjType.RemotingData];
			if (remotingTypeCachedData == null)
			{
				remotingTypeCachedData = new RemotingTypeCachedData(mi);
				mi.Cache[CacheObjType.RemotingData] = remotingTypeCachedData;
			}
			return remotingTypeCachedData;
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x000E7A8C File Offset: 0x000E6A8C
		internal static RemotingCachedData GetReflectionCachedData(MemberInfo mi)
		{
			RemotingCachedData remotingCachedData = (RemotingCachedData)mi.Cache[CacheObjType.RemotingData];
			if (remotingCachedData == null)
			{
				if (mi is MethodBase)
				{
					remotingCachedData = new RemotingMethodCachedData(mi);
				}
				else if (mi is Type)
				{
					remotingCachedData = new RemotingTypeCachedData(mi);
				}
				else
				{
					remotingCachedData = new RemotingCachedData(mi);
				}
				mi.Cache[CacheObjType.RemotingData] = remotingCachedData;
			}
			return remotingCachedData;
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x000E7AE8 File Offset: 0x000E6AE8
		internal static RemotingCachedData GetReflectionCachedData(ParameterInfo reflectionObject)
		{
			RemotingCachedData remotingCachedData = (RemotingCachedData)reflectionObject.Cache[CacheObjType.RemotingData];
			if (remotingCachedData == null)
			{
				remotingCachedData = new RemotingCachedData(reflectionObject);
				reflectionObject.Cache[CacheObjType.RemotingData] = remotingCachedData;
			}
			return remotingCachedData;
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x000E7B24 File Offset: 0x000E6B24
		public static SoapAttribute GetCachedSoapAttribute(object reflectionObject)
		{
			MemberInfo memberInfo = reflectionObject as MemberInfo;
			if (memberInfo != null)
			{
				return InternalRemotingServices.GetReflectionCachedData(memberInfo).GetSoapAttribute();
			}
			return InternalRemotingServices.GetReflectionCachedData((ParameterInfo)reflectionObject).GetSoapAttribute();
		}
	}
}
