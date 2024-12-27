using System;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security
{
	// Token: 0x02000666 RID: 1638
	[Serializable]
	internal sealed class PermissionToken : ISecurityEncodable
	{
		// Token: 0x06003B9B RID: 15259 RVA: 0x000CC244 File Offset: 0x000CB244
		internal static bool IsMscorlibClassName(string className)
		{
			int num = className.IndexOf(',');
			if (num == -1)
			{
				return true;
			}
			num = className.LastIndexOf(']');
			if (num == -1)
			{
				num = 0;
			}
			for (int i = num; i < className.Length; i++)
			{
				if ((className[i] == 'm' || className[i] == 'M') && string.Compare(className, i, "mscorlib", 0, "mscorlib".Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x000CC2D0 File Offset: 0x000CB2D0
		internal PermissionToken()
		{
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x000CC2D8 File Offset: 0x000CB2D8
		internal PermissionToken(int index, PermissionTokenType type, string strTypeName)
		{
			this.m_index = index;
			this.m_type = type;
			this.m_strTypeName = strTypeName;
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x000CC2F8 File Offset: 0x000CB2F8
		public static PermissionToken GetToken(Type cls)
		{
			if (cls == null)
			{
				return null;
			}
			if (cls.GetInterface("System.Security.Permissions.IBuiltInPermission") != null)
			{
				if (PermissionToken.s_reflectPerm == null)
				{
					PermissionToken.s_reflectPerm = new ReflectionPermission(PermissionState.Unrestricted);
				}
				PermissionToken.s_reflectPerm.Assert();
				MethodInfo method = cls.GetMethod("GetTokenIndex", BindingFlags.Static | BindingFlags.NonPublic);
				RuntimeMethodInfo runtimeMethodInfo = method as RuntimeMethodInfo;
				int num = (int)runtimeMethodInfo.Invoke(null, BindingFlags.Default, null, null, null, true);
				return PermissionToken.s_theTokenFactory.BuiltInGetToken(num, null, cls);
			}
			return PermissionToken.s_theTokenFactory.GetToken(cls, null);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x000CC374 File Offset: 0x000CB374
		public static PermissionToken GetToken(IPermission perm)
		{
			if (perm == null)
			{
				return null;
			}
			IBuiltInPermission builtInPermission = perm as IBuiltInPermission;
			if (builtInPermission != null)
			{
				return PermissionToken.s_theTokenFactory.BuiltInGetToken(builtInPermission.GetTokenIndex(), perm, null);
			}
			return PermissionToken.s_theTokenFactory.GetToken(perm.GetType(), perm);
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x000CC3B4 File Offset: 0x000CB3B4
		public static PermissionToken GetToken(string typeStr)
		{
			return PermissionToken.GetToken(typeStr, false);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x000CC3C0 File Offset: 0x000CB3C0
		public static PermissionToken GetToken(string typeStr, bool bCreateMscorlib)
		{
			if (typeStr == null)
			{
				return null;
			}
			if (!PermissionToken.IsMscorlibClassName(typeStr))
			{
				return PermissionToken.s_theTokenFactory.GetToken(typeStr);
			}
			if (!bCreateMscorlib)
			{
				return null;
			}
			return PermissionToken.FindToken(Type.GetType(typeStr));
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x000CC3F8 File Offset: 0x000CB3F8
		public static PermissionToken FindToken(Type cls)
		{
			if (cls == null)
			{
				return null;
			}
			if (cls.GetInterface("System.Security.Permissions.IBuiltInPermission") != null)
			{
				if (PermissionToken.s_reflectPerm == null)
				{
					PermissionToken.s_reflectPerm = new ReflectionPermission(PermissionState.Unrestricted);
				}
				PermissionToken.s_reflectPerm.Assert();
				MethodInfo method = cls.GetMethod("GetTokenIndex", BindingFlags.Static | BindingFlags.NonPublic);
				RuntimeMethodInfo runtimeMethodInfo = method as RuntimeMethodInfo;
				int num = (int)runtimeMethodInfo.Invoke(null, BindingFlags.Default, null, null, null, true);
				return PermissionToken.s_theTokenFactory.BuiltInGetToken(num, null, cls);
			}
			return PermissionToken.s_theTokenFactory.FindToken(cls);
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x000CC473 File Offset: 0x000CB473
		public static PermissionToken FindTokenByIndex(int i)
		{
			return PermissionToken.s_theTokenFactory.FindTokenByIndex(i);
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x000CC480 File Offset: 0x000CB480
		public static bool IsTokenProperlyAssigned(IPermission perm, PermissionToken token)
		{
			PermissionToken token2 = PermissionToken.GetToken(perm);
			return token2.m_index == token.m_index && token.m_type == token2.m_type && (perm.GetType().Module.Assembly != Assembly.GetExecutingAssembly() || token2.m_index < 17);
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x000CC4D8 File Offset: 0x000CB4D8
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("PermissionToken");
			if ((this.m_type & PermissionTokenType.BuiltIn) != (PermissionTokenType)0)
			{
				securityElement.AddAttribute("Index", "" + this.m_index);
			}
			else
			{
				securityElement.AddAttribute("Name", SecurityElement.Escape(this.m_strTypeName));
			}
			securityElement.AddAttribute("Type", this.m_type.ToString("F"));
			return securityElement;
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x000CC554 File Offset: 0x000CB554
		public void FromXml(SecurityElement elRoot)
		{
			elRoot.Tag.Equals("PermissionToken");
			string text = elRoot.Attribute("Name");
			PermissionToken permissionToken;
			if (text != null)
			{
				permissionToken = PermissionToken.GetToken(text, true);
			}
			else
			{
				permissionToken = PermissionToken.FindTokenByIndex(int.Parse(elRoot.Attribute("Index"), CultureInfo.InvariantCulture));
			}
			this.m_index = permissionToken.m_index;
			this.m_type = (PermissionTokenType)Enum.Parse(typeof(PermissionTokenType), elRoot.Attribute("Type"));
			this.m_strTypeName = permissionToken.m_strTypeName;
		}

		// Token: 0x04001E9C RID: 7836
		private const string c_mscorlibName = "mscorlib";

		// Token: 0x04001E9D RID: 7837
		private static readonly PermissionTokenFactory s_theTokenFactory = new PermissionTokenFactory(4);

		// Token: 0x04001E9E RID: 7838
		private static ReflectionPermission s_reflectPerm = null;

		// Token: 0x04001E9F RID: 7839
		internal int m_index;

		// Token: 0x04001EA0 RID: 7840
		internal PermissionTokenType m_type;

		// Token: 0x04001EA1 RID: 7841
		internal string m_strTypeName;

		// Token: 0x04001EA2 RID: 7842
		internal static TokenBasedSet s_tokenSet = new TokenBasedSet();
	}
}
