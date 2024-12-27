using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace triFixAlmSndCyl
{
	// Token: 0x0200000A RID: 10
	public class clsPlaySound
	{
		// Token: 0x0600002D RID: 45
		[DllImport("winmm.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int PlaySound([MarshalAs(UnmanagedType.VBByRefStr)] ref string name, int hmod, int flags);

		// Token: 0x0600002E RID: 46 RVA: 0x00004C54 File Offset: 0x00003054
		public clsPlaySound(string sName, Callback_Method callbackDelegate)
		{
			this.speaker = new SpeechSynthesizer();
			this._sName = sName;
			this.Callback = callbackDelegate;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004C78 File Offset: 0x00003078
		public void ThreadProc()
		{
			string text = "";
			try
			{
				if (!modpublic.g_StopSound)
				{
					Type typeFromHandle = typeof(clsPlaySound);
					lock (typeFromHandle)
					{
						if (Strings.InStr(this._sName, ":\\", CompareMethod.Binary) < 1)
						{
							this._sName = Application.StartupPath + "\\" + this._sName;
						}
						if (this._sName.Length < 1)
						{
							if (!modpublic.g_Mute)
							{
								text = "開始播音, 但是沒有指定聲音檔案名稱";
								if (this.Callback != null)
								{
									this.Callback(text);
								}
							}
						}
						else if (File.Exists(this._sName))
						{
							if (!modpublic.g_Mute)
							{
								text = "PlaySound " + this._sName;
								text = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": " + text;
								if ((this.Callback != null) & modpublic.g_ShowEvt)
								{
									this.Callback(text);
								}
								this.subPlaySound(this._sName);
							}
						}
						else if (!modpublic.g_Mute)
						{
							text = "開始播音, 但是聲音檔案不存在 (" + this._sName + ")";
							if (this.Callback != null)
							{
								this.Callback(text);
							}
						}
					}
				}
			}
			catch (ThreadAbortException ex)
			{
			}
			catch (Exception ex2)
			{
				text = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": " + text;
				if (this.Callback != null)
				{
					this.Callback(text);
				}
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00004E60 File Offset: 0x00003260
		public void subPlaySound(string filename)
		{
			try
			{
				if (Strings.InStr(filename.ToUpper(), ".TXT", CompareMethod.Text) > 0)
				{
					FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read);
					StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
					string text = streamReader.ReadToEnd();
					streamReader.Close();
					fileStream.Close();
					this.speaker.SpeakAsync(text);
				}
				else
				{
					clsPlaySound.PlaySound(ref filename, 0, 131073);
				}
			}
			catch (Exception ex)
			{
				string text2 = Strings.Format(DateAndTime.Now, "yyyy/MM/dd HH:mm:ss") + ": " + ex.Message;
				if (this.Callback != null)
				{
					this.Callback(text2);
				}
			}
			finally
			{
				StreamReader streamReader;
				if (!Information.IsNothing(streamReader))
				{
					streamReader.Close();
				}
				FileStream fileStream;
				if (!Information.IsNothing(fileStream))
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x04000011 RID: 17
		private string _sName;

		// Token: 0x04000012 RID: 18
		private SpeechSynthesizer speaker;

		// Token: 0x04000013 RID: 19
		private Callback_Method Callback;

		// Token: 0x0200000C RID: 12
		protected enum SoundFlags
		{
			// Token: 0x04000015 RID: 21
			SND_SYNC,
			// Token: 0x04000016 RID: 22
			SND_ASYNC,
			// Token: 0x04000017 RID: 23
			SND_NODEFAULT,
			// Token: 0x04000018 RID: 24
			SND_MEMORY = 4,
			// Token: 0x04000019 RID: 25
			SND_LOOP = 8,
			// Token: 0x0400001A RID: 26
			SND_NOSTOP = 16,
			// Token: 0x0400001B RID: 27
			SND_NOWAIT = 8192,
			// Token: 0x0400001C RID: 28
			SND_ALIAS = 65536,
			// Token: 0x0400001D RID: 29
			SND_ALIAS_ID = 1114112,
			// Token: 0x0400001E RID: 30
			SND_FILENAME = 131072,
			// Token: 0x0400001F RID: 31
			SND_RESOURCE = 262148
		}
	}
}
