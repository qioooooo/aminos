using System;
using System.IO;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x02000007 RID: 7
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class BitmapEditor : ImageEditor
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000025E4 File Offset: 0x000015E4
		protected override string GetFileDialogDescription()
		{
			return SR.GetString("bitmapFileDescription");
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025F0 File Offset: 0x000015F0
		protected override string[] GetExtensions()
		{
			return new string[] { "bmp", "gif", "jpg", "jpeg", "png", "ico" };
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002635 File Offset: 0x00001635
		protected override Image LoadFromStream(Stream stream)
		{
			return new Bitmap(stream);
		}
	}
}
