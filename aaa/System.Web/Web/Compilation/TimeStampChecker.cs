using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Web.Hosting;

namespace System.Web.Compilation
{
	// Token: 0x02000196 RID: 406
	internal class TimeStampChecker
	{
		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x0004CEC0 File Offset: 0x0004BEC0
		private static TimeStampChecker Current
		{
			get
			{
				TimeStampChecker timeStampChecker = (TimeStampChecker)CallContext.GetData("TSC");
				if (timeStampChecker == null)
				{
					timeStampChecker = new TimeStampChecker();
					CallContext.SetData("TSC", timeStampChecker);
				}
				return timeStampChecker;
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0004CEF2 File Offset: 0x0004BEF2
		internal static void AddFile(string virtualPath, string path)
		{
			TimeStampChecker.Current.AddFileInternal(virtualPath, path);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0004CF00 File Offset: 0x0004BF00
		private void AddFileInternal(string virtualPath, string path)
		{
			DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(path);
			if (this._timeStamps.Contains(virtualPath))
			{
				DateTime dateTime = (DateTime)this._timeStamps[virtualPath];
				if (dateTime == DateTime.MaxValue)
				{
					return;
				}
				if (dateTime != lastWriteTimeUtc)
				{
					this._timeStamps[virtualPath] = DateTime.MaxValue;
					return;
				}
			}
			else
			{
				this._timeStamps[virtualPath] = lastWriteTimeUtc;
			}
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0004CF74 File Offset: 0x0004BF74
		internal static bool CheckFilesStillValid(string key, ICollection virtualPaths)
		{
			return virtualPaths == null || TimeStampChecker.Current.CheckFilesStillValidInternal(key, virtualPaths);
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0004CF88 File Offset: 0x0004BF88
		private bool CheckFilesStillValidInternal(string key, ICollection virtualPaths)
		{
			foreach (object obj in virtualPaths)
			{
				string text = (string)obj;
				if (this._timeStamps.Contains(text))
				{
					string text2 = HostingEnvironment.MapPath(text);
					DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(text2);
					DateTime dateTime = (DateTime)this._timeStamps[text];
					if (lastWriteTimeUtc != dateTime)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001692 RID: 5778
		internal const string CallContextSlotName = "TSC";

		// Token: 0x04001693 RID: 5779
		private Hashtable _timeStamps = new Hashtable(StringComparer.OrdinalIgnoreCase);
	}
}
