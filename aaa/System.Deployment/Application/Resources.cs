using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;

namespace System.Deployment.Application
{
	// Token: 0x020000E7 RID: 231
	internal static class Resources
	{
		// Token: 0x060005E2 RID: 1506 RVA: 0x0001E77C File Offset: 0x0001D77C
		public static string GetString(string s)
		{
			if (Resources._resources == null)
			{
				lock (Resources.lockObject)
				{
					if (Resources._resources == null)
					{
						Resources.InitializeReferenceToAssembly();
						Resources._resources = new ResourceManager("System.Deployment", Resources._assembly);
					}
				}
			}
			return Resources._resources.GetString(s);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001E7E0 File Offset: 0x0001D7E0
		public static Image GetImage(string imageName)
		{
			Resources.InitializeReferenceToAssembly();
			Stream stream = null;
			Image image;
			try
			{
				stream = Resources._assembly.GetManifestResourceStream(imageName);
				image = Image.FromStream(stream);
			}
			catch
			{
				if (stream != null)
				{
					stream.Close();
				}
				throw;
			}
			return image;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001E828 File Offset: 0x0001D828
		public static Icon GetIcon(string iconName)
		{
			Resources.InitializeReferenceToAssembly();
			Icon icon;
			using (Stream manifestResourceStream = Resources._assembly.GetManifestResourceStream(iconName))
			{
				icon = new Icon(manifestResourceStream);
			}
			return icon;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001E86C File Offset: 0x0001D86C
		private static void InitializeReferenceToAssembly()
		{
			if (Resources._assembly == null)
			{
				lock (Resources.lockObject)
				{
					if (Resources._assembly == null)
					{
						Resources._assembly = Assembly.GetExecutingAssembly();
					}
				}
			}
		}

		// Token: 0x040004B5 RID: 1205
		private static object lockObject = new object();

		// Token: 0x040004B6 RID: 1206
		private static ResourceManager _resources = null;

		// Token: 0x040004B7 RID: 1207
		private static Assembly _assembly = null;
	}
}
