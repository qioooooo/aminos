using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Drawing.Design
{
	// Token: 0x02000004 RID: 4
	internal sealed class SR
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002114 File Offset: 0x00001114
		private static object InternalSyncObject
		{
			get
			{
				if (SR.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SR.s_InternalSyncObject, obj, null);
				}
				return SR.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00001140
		internal SR()
		{
			this.resources = new ResourceManager("System.Drawing.Design.SR", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00001164
		private static SR GetLoader()
		{
			if (SR.loader == null)
			{
				lock (SR.InternalSyncObject)
				{
					if (SR.loader == null)
					{
						SR.loader = new SR();
					}
				}
			}
			return SR.loader;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021B4 File Offset: 0x000011B4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021B7 File Offset: 0x000011B7
		public static ResourceManager Resources
		{
			get
			{
				return SR.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000011C4
		public static string GetString(string name, params object[] args)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			string @string = sr.resources.GetString(name, SR.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002248 File Offset: 0x00001248
		public static string GetString(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetString(name, SR.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00001274
		public static object GetObject(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetObject(name, SR.Culture);
		}

		// Token: 0x04000002 RID: 2
		internal const string imageFileDescription = "imageFileDescription";

		// Token: 0x04000003 RID: 3
		internal const string ColorEditorSystemTab = "ColorEditorSystemTab";

		// Token: 0x04000004 RID: 4
		internal const string ColorEditorStandardTab = "ColorEditorStandardTab";

		// Token: 0x04000005 RID: 5
		internal const string bitmapFileDescription = "bitmapFileDescription";

		// Token: 0x04000006 RID: 6
		internal const string ColorEditorPaletteTab = "ColorEditorPaletteTab";

		// Token: 0x04000007 RID: 7
		internal const string iconFileDescription = "iconFileDescription";

		// Token: 0x04000008 RID: 8
		internal const string metafileFileDescription = "metafileFileDescription";

		// Token: 0x04000009 RID: 9
		internal const string ContentAlignmentEditorAccName = "ContentAlignmentEditorAccName";

		// Token: 0x0400000A RID: 10
		internal const string ContentAlignmentEditorTopLeftAccName = "ContentAlignmentEditorTopLeftAccName";

		// Token: 0x0400000B RID: 11
		internal const string ContentAlignmentEditorTopCenterAccName = "ContentAlignmentEditorTopCenterAccName";

		// Token: 0x0400000C RID: 12
		internal const string ContentAlignmentEditorTopRightAccName = "ContentAlignmentEditorTopRightAccName";

		// Token: 0x0400000D RID: 13
		internal const string ContentAlignmentEditorMiddleLeftAccName = "ContentAlignmentEditorMiddleLeftAccName";

		// Token: 0x0400000E RID: 14
		internal const string ContentAlignmentEditorMiddleCenterAccName = "ContentAlignmentEditorMiddleCenterAccName";

		// Token: 0x0400000F RID: 15
		internal const string ContentAlignmentEditorMiddleRightAccName = "ContentAlignmentEditorMiddleRightAccName";

		// Token: 0x04000010 RID: 16
		internal const string ContentAlignmentEditorBottomLeftAccName = "ContentAlignmentEditorBottomLeftAccName";

		// Token: 0x04000011 RID: 17
		internal const string ContentAlignmentEditorBottomCenterAccName = "ContentAlignmentEditorBottomCenterAccName";

		// Token: 0x04000012 RID: 18
		internal const string ContentAlignmentEditorBottomRightAccName = "ContentAlignmentEditorBottomRightAccName";

		// Token: 0x04000013 RID: 19
		internal const string ColorEditorAccName = "ColorEditorAccName";

		// Token: 0x04000014 RID: 20
		internal const string ToolboxServiceBadToolboxItem = "ToolboxServiceBadToolboxItem";

		// Token: 0x04000015 RID: 21
		internal const string ToolboxServiceBadToolboxItemWithException = "ToolboxServiceBadToolboxItemWithException";

		// Token: 0x04000016 RID: 22
		internal const string ToolboxServiceAssemblyNotFound = "ToolboxServiceAssemblyNotFound";

		// Token: 0x04000017 RID: 23
		private static SR loader;

		// Token: 0x04000018 RID: 24
		private ResourceManager resources;

		// Token: 0x04000019 RID: 25
		private static object s_InternalSyncObject;
	}
}
