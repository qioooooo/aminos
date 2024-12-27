using System;
using System.Reflection;
using System.Web.Hosting;

namespace System.Web.Compilation
{
	// Token: 0x02000147 RID: 327
	internal class BuildResultMainCodeAssembly : BuildResultCompiledAssembly
	{
		// Token: 0x06000F46 RID: 3910 RVA: 0x00044C5C File Offset: 0x00043C5C
		internal BuildResultMainCodeAssembly()
		{
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x00044C64 File Offset: 0x00043C64
		internal BuildResultMainCodeAssembly(Assembly a)
			: base(a)
		{
			this.FindAppInitializeMethod();
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00044C73 File Offset: 0x00043C73
		internal override BuildResultTypeCode GetCode()
		{
			return BuildResultTypeCode.BuildResultMainCodeAssembly;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00044C78 File Offset: 0x00043C78
		internal override void GetPreservedAttributes(PreservationFileReader pfr)
		{
			base.GetPreservedAttributes(pfr);
			string attribute = pfr.GetAttribute("appInitializeClass");
			if (attribute != null)
			{
				Type type = this.ResultAssembly.GetType(attribute);
				this._appInitializeMethod = this.FindAppInitializeMethod(type);
			}
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00044CB5 File Offset: 0x00043CB5
		internal override void SetPreservedAttributes(PreservationFileWriter pfw)
		{
			base.SetPreservedAttributes(pfw);
			if (this._appInitializeMethod != null)
			{
				pfw.SetAttribute("appInitializeClass", this._appInitializeMethod.ReflectedType.FullName);
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00044CE4 File Offset: 0x00043CE4
		private void FindAppInitializeMethod()
		{
			foreach (Type type in this.ResultAssembly.GetExportedTypes())
			{
				MethodInfo methodInfo = this.FindAppInitializeMethod(type);
				if (methodInfo != null)
				{
					if (this._appInitializeMethod != null)
					{
						throw new HttpException(SR.GetString("Duplicate_appinitialize", new object[]
						{
							this._appInitializeMethod.ReflectedType.FullName,
							type.FullName
						}));
					}
					this._appInitializeMethod = methodInfo;
				}
			}
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00044D61 File Offset: 0x00043D61
		private MethodInfo FindAppInitializeMethod(Type t)
		{
			return t.GetMethod("AppInitialize", BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public, null, new Type[0], null);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00044D78 File Offset: 0x00043D78
		internal void CallAppInitializeMethod()
		{
			if (this._appInitializeMethod != null)
			{
				using (new ApplicationImpersonationContext())
				{
					using (HostingEnvironment.SetCultures())
					{
						this._appInitializeMethod.Invoke(null, null);
					}
				}
			}
		}

		// Token: 0x040015E1 RID: 5601
		private const string appInitializeMethodName = "AppInitialize";

		// Token: 0x040015E2 RID: 5602
		private MethodInfo _appInitializeMethod;
	}
}
