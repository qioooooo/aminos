using System;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x020003E1 RID: 993
	public abstract class FeatureSupport : IFeatureSupport
	{
		// Token: 0x06003B76 RID: 15222 RVA: 0x000D7DCC File Offset: 0x000D6DCC
		public static bool IsPresent(string featureClassName, string featureConstName)
		{
			return FeatureSupport.IsPresent(featureClassName, featureConstName, new Version(0, 0, 0, 0));
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x000D7DE0 File Offset: 0x000D6DE0
		public static bool IsPresent(string featureClassName, string featureConstName, Version minimumVersion)
		{
			object obj = null;
			Type type = null;
			try
			{
				type = Type.GetType(featureClassName);
			}
			catch (ArgumentException)
			{
			}
			if (type != null)
			{
				FieldInfo field = type.GetField(featureConstName);
				if (field != null)
				{
					obj = field.GetValue(null);
				}
			}
			if (obj != null && typeof(IFeatureSupport).IsAssignableFrom(type))
			{
				IFeatureSupport featureSupport = (IFeatureSupport)SecurityUtils.SecureCreateInstance(type);
				if (featureSupport != null)
				{
					return featureSupport.IsPresent(obj, minimumVersion);
				}
			}
			return false;
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x000D7E54 File Offset: 0x000D6E54
		public static Version GetVersionPresent(string featureClassName, string featureConstName)
		{
			object obj = null;
			Type type = null;
			try
			{
				type = Type.GetType(featureClassName);
			}
			catch (ArgumentException)
			{
			}
			if (type != null)
			{
				FieldInfo field = type.GetField(featureConstName);
				if (field != null)
				{
					obj = field.GetValue(null);
				}
			}
			if (obj != null)
			{
				IFeatureSupport featureSupport = (IFeatureSupport)SecurityUtils.SecureCreateInstance(type);
				if (featureSupport != null)
				{
					return featureSupport.GetVersionPresent(obj);
				}
			}
			return null;
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x000D7EB4 File Offset: 0x000D6EB4
		public virtual bool IsPresent(object feature)
		{
			return this.IsPresent(feature, new Version(0, 0, 0, 0));
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x000D7EC8 File Offset: 0x000D6EC8
		public virtual bool IsPresent(object feature, Version minimumVersion)
		{
			Version versionPresent = this.GetVersionPresent(feature);
			return versionPresent != null && versionPresent.CompareTo(minimumVersion) >= 0;
		}

		// Token: 0x06003B7B RID: 15227
		public abstract Version GetVersionPresent(object feature);
	}
}
