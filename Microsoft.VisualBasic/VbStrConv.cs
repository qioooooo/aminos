using System;

namespace Microsoft.VisualBasic
{
	[Flags]
	public enum VbStrConv
	{
		None = 0,
		Uppercase = 1,
		Lowercase = 2,
		ProperCase = 3,
		Wide = 4,
		Narrow = 8,
		Katakana = 16,
		Hiragana = 32,
		SimplifiedChinese = 256,
		TraditionalChinese = 512,
		LinguisticCasing = 1024
	}
}
