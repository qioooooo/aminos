using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000444 RID: 1092
	internal class ImageListConverter : ComponentConverter
	{
		// Token: 0x0600416A RID: 16746 RVA: 0x000EA779 File Offset: 0x000E9779
		public ImageListConverter()
			: base(typeof(ImageList))
		{
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x000EA78B File Offset: 0x000E978B
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
