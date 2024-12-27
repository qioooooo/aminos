using System;
using System.Reflection;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x0200072B RID: 1835
	internal class RemotingTypeCachedData : RemotingCachedData
	{
		// Token: 0x06004221 RID: 16929 RVA: 0x000E1F5C File Offset: 0x000E0F5C
		internal RemotingTypeCachedData(object ri)
			: base(ri)
		{
			this._lastMethodCalled = null;
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x000E1F6C File Offset: 0x000E0F6C
		internal MethodBase GetLastCalledMethod(string newMeth)
		{
			RemotingTypeCachedData.LastCalledMethodClass lastMethodCalled = this._lastMethodCalled;
			if (lastMethodCalled == null)
			{
				return null;
			}
			string methodName = lastMethodCalled.methodName;
			MethodBase mb = lastMethodCalled.MB;
			if (mb == null || methodName == null)
			{
				return null;
			}
			if (methodName.Equals(newMeth))
			{
				return mb;
			}
			return null;
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x000E1FA8 File Offset: 0x000E0FA8
		internal void SetLastCalledMethod(string newMethName, MethodBase newMB)
		{
			this._lastMethodCalled = new RemotingTypeCachedData.LastCalledMethodClass
			{
				methodName = newMethName,
				MB = newMB
			};
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06004224 RID: 16932 RVA: 0x000E1FD0 File Offset: 0x000E0FD0
		internal TypeInfo TypeInfo
		{
			get
			{
				if (this._typeInfo == null)
				{
					this._typeInfo = new TypeInfo((Type)this.RI);
				}
				return this._typeInfo;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06004225 RID: 16933 RVA: 0x000E1FF6 File Offset: 0x000E0FF6
		internal string QualifiedTypeName
		{
			get
			{
				if (this._qualifiedTypeName == null)
				{
					this._qualifiedTypeName = RemotingServices.DetermineDefaultQualifiedTypeName((Type)this.RI);
				}
				return this._qualifiedTypeName;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06004226 RID: 16934 RVA: 0x000E201C File Offset: 0x000E101C
		internal string AssemblyName
		{
			get
			{
				if (this._assemblyName == null)
				{
					this._assemblyName = ((Type)this.RI).Module.Assembly.FullName;
				}
				return this._assemblyName;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06004227 RID: 16935 RVA: 0x000E204C File Offset: 0x000E104C
		internal string SimpleAssemblyName
		{
			get
			{
				if (this._simpleAssemblyName == null)
				{
					this._simpleAssemblyName = ((Type)this.RI).Module.Assembly.nGetSimpleName();
				}
				return this._simpleAssemblyName;
			}
		}

		// Token: 0x040020F7 RID: 8439
		private RemotingTypeCachedData.LastCalledMethodClass _lastMethodCalled;

		// Token: 0x040020F8 RID: 8440
		private TypeInfo _typeInfo;

		// Token: 0x040020F9 RID: 8441
		private string _qualifiedTypeName;

		// Token: 0x040020FA RID: 8442
		private string _assemblyName;

		// Token: 0x040020FB RID: 8443
		private string _simpleAssemblyName;

		// Token: 0x0200072C RID: 1836
		private class LastCalledMethodClass
		{
			// Token: 0x040020FC RID: 8444
			public string methodName;

			// Token: 0x040020FD RID: 8445
			public MethodBase MB;
		}
	}
}
