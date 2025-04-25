using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace TextRpg
{
    class Music
    {
        string audioFilePath;

        private AudioFileReader _audioFile;
        private WaveOutEvent _outputDevice;
        public Music(string filePath)
        {
            audioFilePath = filePath;
            _audioFile = new AudioFileReader(audioFilePath);
            _outputDevice = new WaveOutEvent();

            // 초기 필수
            _outputDevice.Init(_audioFile);
        }
        public void Play()
        {
            _outputDevice.Play();
        }
        public void PlayLooping()
        {
            _outputDevice.PlaybackStopped += PlaybackStoppedHandler;
            _outputDevice.Play();
        }
        public void Stop()
        {
            _outputDevice.PlaybackStopped -= PlaybackStoppedHandler;
            _outputDevice.Stop();
        }
        public void SetVolume(float volume)
        {
            _outputDevice.Volume = volume;
        }
        public void Dispose()
        {
            _outputDevice.Dispose();
            _audioFile.Dispose();
        }
        public void PlaybackStoppedHandler(object sender, StoppedEventArgs e)
        {
            // 오디오 파일의 위치를 처음으로 재설정
            _audioFile.Position = 0;
            _outputDevice.Play();
        }
    }
}
