using System;

namespace Aladdin.HASP
{
	[Flags]
	[Serializable]
	public enum FeatureOptions
	{
		Default = 0,
		NotLocal = 32768,
		NotRemote = 16384,
		Process = 8192,
		Classic = 4096,
		IgnoreTS = 2048
	}
}
