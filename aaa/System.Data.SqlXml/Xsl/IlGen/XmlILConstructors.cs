using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x0200001F RID: 31
	internal static class XmlILConstructors
	{
		// Token: 0x06000139 RID: 313 RVA: 0x00009CF8 File Offset: 0x00008CF8
		private static ConstructorInfo GetConstructor(Type className)
		{
			return className.GetConstructor(new Type[0]);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009D14 File Offset: 0x00008D14
		private static ConstructorInfo GetConstructor(Type className, params Type[] args)
		{
			return className.GetConstructor(args);
		}

		// Token: 0x0400015F RID: 351
		public static readonly ConstructorInfo DecFromParts = XmlILConstructors.GetConstructor(typeof(decimal), new Type[]
		{
			typeof(int),
			typeof(int),
			typeof(int),
			typeof(bool),
			typeof(byte)
		});

		// Token: 0x04000160 RID: 352
		public static readonly ConstructorInfo DecFromInt32 = XmlILConstructors.GetConstructor(typeof(decimal), new Type[] { typeof(int) });

		// Token: 0x04000161 RID: 353
		public static readonly ConstructorInfo DecFromInt64 = XmlILConstructors.GetConstructor(typeof(decimal), new Type[] { typeof(long) });

		// Token: 0x04000162 RID: 354
		public static readonly ConstructorInfo Debuggable = XmlILConstructors.GetConstructor(typeof(DebuggableAttribute), new Type[] { typeof(DebuggableAttribute.DebuggingModes) });

		// Token: 0x04000163 RID: 355
		public static readonly ConstructorInfo NonUserCode = XmlILConstructors.GetConstructor(typeof(DebuggerNonUserCodeAttribute));

		// Token: 0x04000164 RID: 356
		public static readonly ConstructorInfo QName = XmlILConstructors.GetConstructor(typeof(XmlQualifiedName), new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x04000165 RID: 357
		public static readonly ConstructorInfo StepThrough = XmlILConstructors.GetConstructor(typeof(DebuggerStepThroughAttribute));

		// Token: 0x04000166 RID: 358
		public static readonly ConstructorInfo Transparent = XmlILConstructors.GetConstructor(typeof(SecurityTransparentAttribute));
	}
}
