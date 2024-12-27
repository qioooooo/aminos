using System;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x0200001A RID: 26
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class MetafileEditor : ImageEditor
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00005AD1 File Offset: 0x00004AD1
		protected override string GetFileDialogDescription()
		{
			return SR.GetString("metafileFileDescription");
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005AE0 File Offset: 0x00004AE0
		protected override string[] GetExtensions()
		{
			return new string[] { "emf", "wmf" };
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005B05 File Offset: 0x00004B05
		protected override Image LoadFromStream(Stream stream)
		{
			return new Metafile(stream);
		}
	}
}
