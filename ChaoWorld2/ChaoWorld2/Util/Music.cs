using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Util
{
  public static class Music
  {
    const string PATH = @"Content\music\";
    const string EXT = ".mp3";

    public static bool IsMuted { get { return muted; } }

    static WaveOut waveOutDevice;
    static WaveChannel32 volumeStream;
    static AudioFileReader audioFileReader;
    static LoopStream loop;
    static float volume = 1f;
    static bool muted = false;

    public static void Play(string mName)
    {
      if (!File.Exists(PATH + mName + EXT))
        return;
      audioFileReader = new AudioFileReader(PATH + mName + EXT);
      loop = new LoopStream(audioFileReader);
      volumeStream = new WaveChannel32(loop);
      volumeStream.Volume = muted ? volume : 0f;
      waveOutDevice = new WaveOut();
      waveOutDevice.Init(volumeStream);
      waveOutDevice.Play();
    }

    public static void Volume(float vol)
    {
      volume = vol;
      if (volumeStream != null && !muted)
        volumeStream.Volume = vol;
    }

    public static void Mute()
    {
      muted = true;
      if (volumeStream != null)
        volumeStream.Volume = 0;
    }

    public static void UnMute()
    {
      muted = false;
      if (volumeStream != null)
        volumeStream.Volume = volume;
    }

    public static void Dispose()
    {
      if (waveOutDevice != null)
      {
        waveOutDevice.Stop();
        waveOutDevice.Dispose();
        waveOutDevice = null;
      }
      if (audioFileReader != null)
      {
        audioFileReader.Dispose();
        audioFileReader = null;
      }
      if (loop != null)
      {
        loop.Dispose();
        loop = null;
      }
    }
  }

  /// <summary>
  /// Stream for looping playback
  /// </summary>
  public class LoopStream : WaveStream
  {
    WaveStream sourceStream;

    /// <summary>
    /// Creates a new Loop stream
    /// </summary>
    /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
    /// or else we will not loop to the start again.</param>
    public LoopStream(WaveStream sourceStream)
    {
      this.sourceStream = sourceStream;
      this.EnableLooping = true;
    }

    /// <summary>
    /// Use this to turn looping on or off
    /// </summary>
    public bool EnableLooping { get; set; }

    /// <summary>
    /// Return source stream's wave format
    /// </summary>
    public override WaveFormat WaveFormat
    {
      get { return sourceStream.WaveFormat; }
    }

    /// <summary>
    /// LoopStream simply returns
    /// </summary>
    public override long Length
    {
      get { return sourceStream.Length; }
    }

    /// <summary>
    /// LoopStream simply passes on positioning to source stream
    /// </summary>
    public override long Position
    {
      get { return sourceStream.Position; }
      set { sourceStream.Position = value; }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int totalBytesRead = 0;

      while (totalBytesRead < count)
      {
        int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
        if (bytesRead == 0)
        {
          if (sourceStream.Position == 0 || !EnableLooping)
          {
            // something wrong with the source stream
            break;
          }
          // loop
          sourceStream.Position = 0;
        }
        totalBytesRead += bytesRead;
      }
      return totalBytesRead;
    }
  }
}
