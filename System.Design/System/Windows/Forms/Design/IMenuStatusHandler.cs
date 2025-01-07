using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal interface IMenuStatusHandler
	{
		bool OverrideInvoke(MenuCommand cmd);

		bool OverrideStatus(MenuCommand cmd);
	}
}
