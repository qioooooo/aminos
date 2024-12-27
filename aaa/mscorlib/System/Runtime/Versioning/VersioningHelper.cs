using System;
using Microsoft.Win32;

namespace System.Runtime.Versioning
{
	// Token: 0x02000938 RID: 2360
	public static class VersioningHelper
	{
		// Token: 0x06005584 RID: 21892 RVA: 0x00137294 File Offset: 0x00136294
		public static string MakeVersionSafeName(string name, ResourceScope from, ResourceScope to)
		{
			return VersioningHelper.MakeVersionSafeName(name, from, to, null);
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x001372A0 File Offset: 0x001362A0
		public static string MakeVersionSafeName(string name, ResourceScope from, ResourceScope to, Type type)
		{
			ResourceScope resourceScope = from & VersioningHelper.ResTypeMask;
			ResourceScope resourceScope2 = to & VersioningHelper.ResTypeMask;
			if (resourceScope > resourceScope2)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ResourceScopeWrongDirection", new object[] { resourceScope, resourceScope2 }), "from");
			}
			SxSRequirements requirements = VersioningHelper.GetRequirements(to, from);
			if ((requirements & (SxSRequirements.AssemblyName | SxSRequirements.TypeName)) != SxSRequirements.None && type == null)
			{
				throw new ArgumentNullException("type", Environment.GetResourceString("ArgumentNull_TypeRequiredByResourceScope"));
			}
			string text = "";
			if ((requirements & SxSRequirements.ProcessID) != SxSRequirements.None)
			{
				text = text + "_" + Win32Native.GetCurrentProcessId();
			}
			if ((requirements & SxSRequirements.AppDomainID) != SxSRequirements.None)
			{
				text = text + "_" + AppDomain.CurrentDomain.GetAppDomainId();
			}
			if ((requirements & SxSRequirements.TypeName) != SxSRequirements.None)
			{
				text = text + "_" + type.Name;
			}
			if ((requirements & SxSRequirements.AssemblyName) != SxSRequirements.None)
			{
				text = text + "_" + type.Assembly.FullName;
			}
			return name + text;
		}

		// Token: 0x06005586 RID: 21894 RVA: 0x00137398 File Offset: 0x00136398
		private static SxSRequirements GetRequirements(ResourceScope consumeAsScope, ResourceScope calleeScope)
		{
			SxSRequirements sxSRequirements = SxSRequirements.None;
			switch (calleeScope & VersioningHelper.ResTypeMask)
			{
			case ResourceScope.Machine:
				switch (consumeAsScope & VersioningHelper.ResTypeMask)
				{
				case ResourceScope.Machine:
					goto IL_00AC;
				case ResourceScope.Process:
					sxSRequirements |= SxSRequirements.ProcessID;
					goto IL_00AC;
				case ResourceScope.AppDomain:
					sxSRequirements |= SxSRequirements.AppDomainID | SxSRequirements.ProcessID;
					goto IL_00AC;
				}
				throw new ArgumentException(Environment.GetResourceString("Argument_BadResourceScopeTypeBits", new object[] { consumeAsScope }), "consumeAsScope");
			case ResourceScope.Process:
				if ((consumeAsScope & ResourceScope.AppDomain) != ResourceScope.None)
				{
					sxSRequirements |= SxSRequirements.AppDomainID;
					goto IL_00AC;
				}
				goto IL_00AC;
			case ResourceScope.AppDomain:
				goto IL_00AC;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_BadResourceScopeTypeBits", new object[] { calleeScope }), "calleeScope");
			IL_00AC:
			ResourceScope resourceScope = calleeScope & VersioningHelper.VisibilityMask;
			if (resourceScope != ResourceScope.None)
			{
				if (resourceScope != ResourceScope.Private)
				{
					if (resourceScope != ResourceScope.Assembly)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_BadResourceScopeVisibilityBits", new object[] { calleeScope }), "calleeScope");
					}
					if ((consumeAsScope & ResourceScope.Private) != ResourceScope.None)
					{
						sxSRequirements |= SxSRequirements.TypeName;
					}
				}
			}
			else
			{
				ResourceScope resourceScope2 = consumeAsScope & VersioningHelper.VisibilityMask;
				if (resourceScope2 != ResourceScope.None)
				{
					if (resourceScope2 != ResourceScope.Private)
					{
						if (resourceScope2 != ResourceScope.Assembly)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_BadResourceScopeVisibilityBits", new object[] { consumeAsScope }), "consumeAsScope");
						}
						sxSRequirements |= SxSRequirements.AssemblyName;
					}
					else
					{
						sxSRequirements |= SxSRequirements.AssemblyName | SxSRequirements.TypeName;
					}
				}
			}
			return sxSRequirements;
		}

		// Token: 0x04002C9B RID: 11419
		private static ResourceScope ResTypeMask = ResourceScope.Machine | ResourceScope.Process | ResourceScope.AppDomain | ResourceScope.Library;

		// Token: 0x04002C9C RID: 11420
		private static ResourceScope VisibilityMask = ResourceScope.Private | ResourceScope.Assembly;
	}
}
