using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	public sealed class Globals
	{
		public static string ScriptEngine
		{
			get
			{
				return "VB";
			}
		}

		public static int ScriptEngineMajorVersion
		{
			get
			{
				return 8;
			}
		}

		public static int ScriptEngineMinorVersion
		{
			get
			{
				return 0;
			}
		}

		public static int ScriptEngineBuildVersion
		{
			get
			{
				return 50727;
			}
		}
	}
}
