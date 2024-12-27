using System;
using System.Reflection;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x0200014B RID: 331
	internal class BuildResultCompiledType : BuildResultCompiledAssemblyBase, ITypedWebObjectFactory, IWebObjectFactory
	{
		// Token: 0x06000F59 RID: 3929 RVA: 0x00044E91 File Offset: 0x00043E91
		internal BuildResultCompiledType()
		{
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00044E99 File Offset: 0x00043E99
		internal BuildResultCompiledType(Type t)
		{
			this._builtType = t;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00044EA8 File Offset: 0x00043EA8
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.BuildResultCompiledType;
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x00044EAB File Offset: 0x00043EAB
		// (set) Token: 0x06000F5D RID: 3933 RVA: 0x00044EB8 File Offset: 0x00043EB8
		internal override Assembly ResultAssembly
		{
			get
			{
				return this._builtType.Assembly;
			}
			set
			{
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x00044EBA File Offset: 0x00043EBA
		// (set) Token: 0x06000F5F RID: 3935 RVA: 0x00044EC2 File Offset: 0x00043EC2
		internal Type ResultType
		{
			get
			{
				return this._builtType;
			}
			set
			{
				this._builtType = value;
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00044ECC File Offset: 0x00043ECC
		public object CreateInstance()
		{
			if (!this._triedToGetInstObj)
			{
				this._instObj = ObjectFactoryCodeDomTreeGenerator.GetFastObjectCreationDelegate(this.ResultType);
				this._triedToGetInstObj = true;
			}
			if (this._instObj == null)
			{
				return HttpRuntime.CreatePublicInstance(this.ResultType);
			}
			return this._instObj();
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x00044F18 File Offset: 0x00043F18
		public virtual Type InstantiatedType
		{
			get
			{
				return this.ResultType;
			}
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00044F20 File Offset: 0x00043F20
		protected override void ComputeHashCode(HashCodeCombiner hashCodeCombiner)
		{
			base.ComputeHashCode(hashCodeCombiner);
			if (base.VirtualPath != null)
			{
				VirtualPath parent = base.VirtualPath.Parent;
				Assembly localResourcesAssembly = BuildManager.GetLocalResourcesAssembly(parent);
				if (localResourcesAssembly != null)
				{
					hashCodeCombiner.AddFile(localResourcesAssembly.Location);
				}
			}
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00044F64 File Offset: 0x00043F64
		internal override void GetPreservedAttributes(PreservationFileReader pfr)
		{
			base.GetPreservedAttributes(pfr);
			Assembly preservedAssembly = BuildResultCompiledAssemblyBase.GetPreservedAssembly(pfr);
			string attribute = pfr.GetAttribute("type");
			this.ResultType = preservedAssembly.GetType(attribute, true);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00044F99 File Offset: 0x00043F99
		internal override void SetPreservedAttributes(PreservationFileWriter pfw)
		{
			base.SetPreservedAttributes(pfw);
			pfw.SetAttribute("type", this.ResultType.FullName);
		}

		// Token: 0x040015E4 RID: 5604
		private InstantiateObject _instObj;

		// Token: 0x040015E5 RID: 5605
		private bool _triedToGetInstObj;

		// Token: 0x040015E6 RID: 5606
		private Type _builtType;
	}
}
