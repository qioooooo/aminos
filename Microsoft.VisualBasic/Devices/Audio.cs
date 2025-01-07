﻿using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Security;
using System.Security.Permissions;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.Devices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class Audio
	{
		public void Play(string location)
		{
			this.Play(location, AudioPlayMode.Background);
		}

		public void Play(string location, AudioPlayMode playMode)
		{
			this.ValidateAudioPlayModeEnum(playMode, "playMode");
			string text = this.ValidateFilename(location);
			SoundPlayer soundPlayer = new SoundPlayer(text);
			this.Play(soundPlayer, playMode);
		}

		public void Play(byte[] data, AudioPlayMode playMode)
		{
			if (data == null)
			{
				throw ExceptionUtils.GetArgumentNullException("data");
			}
			this.ValidateAudioPlayModeEnum(playMode, "playMode");
			MemoryStream memoryStream = new MemoryStream(data);
			this.Play(memoryStream, playMode);
			memoryStream.Close();
		}

		public void Play(Stream stream, AudioPlayMode playMode)
		{
			this.ValidateAudioPlayModeEnum(playMode, "playMode");
			if (stream == null)
			{
				throw ExceptionUtils.GetArgumentNullException("stream");
			}
			this.Play(new SoundPlayer(stream), playMode);
		}

		public void PlaySystemSound(SystemSound systemSound)
		{
			if (systemSound == null)
			{
				throw ExceptionUtils.GetArgumentNullException("systemSound");
			}
			systemSound.Play();
		}

		public void Stop()
		{
			SoundPlayer soundPlayer = new SoundPlayer();
			Audio.InternalStop(soundPlayer);
		}

		private void Play(SoundPlayer sound, AudioPlayMode mode)
		{
			if (this.m_Sound != null)
			{
				Audio.InternalStop(this.m_Sound);
			}
			this.m_Sound = sound;
			switch (mode)
			{
			case AudioPlayMode.WaitToComplete:
				this.m_Sound.PlaySync();
				break;
			case AudioPlayMode.Background:
				this.m_Sound.Play();
				break;
			case AudioPlayMode.BackgroundLoop:
				this.m_Sound.PlayLooping();
				break;
			}
		}

		private static void InternalStop(SoundPlayer sound)
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			try
			{
				sound.Stop();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		private string ValidateFilename(string location)
		{
			if (Operators.CompareString(location, "", false) == 0)
			{
				throw ExceptionUtils.GetArgumentNullException("location");
			}
			return location;
		}

		private void ValidateAudioPlayModeEnum(AudioPlayMode value, string paramName)
		{
			if (value < AudioPlayMode.WaitToComplete || value > AudioPlayMode.BackgroundLoop)
			{
				throw new InvalidEnumArgumentException(paramName, (int)value, typeof(AudioPlayMode));
			}
		}

		private SoundPlayer m_Sound;
	}
}
