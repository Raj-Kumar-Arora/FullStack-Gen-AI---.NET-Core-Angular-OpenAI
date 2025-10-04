using System.Threading.Tasks;
using WebAPI.Services;
using Xabe.FFmpeg;

namespace WebAPI.Services
{
    public class VideoService : IVideoService
    {
        private readonly IWhisperService? _whisperService;
        
        public VideoService(IWhisperService? whisperService)
        {
            _whisperService = whisperService;

            var ffmpegPath = "C:\\ffmpeg-7.1.1-essentials_build\\ffmpeg-7.1.1-essentials_build\\bin"; // Set the path to ffmpeg executable if needed

            // Directory.Exists(ffmpegPath) - not working - to fix LATER
            //if (string.IsNullOrEmpty(ffmpegPath) || !Directory.Exists(ffmpegPath))
            //{
            //    throw new DirectoryNotFoundException("Please add FFmpeg executables to your PATH variable or specify a valid directory path.");
            //}

            FFmpeg.SetExecutablesPath(ffmpegPath);
        }
        public async Task<string> ExtractAudioAndTranscribeAsync(string videoPath)
        {
            try
            {
                var audioPath = Path.ChangeExtension(videoPath, ".mp3");
                // Extract audio from video
                var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(videoPath, audioPath);
                await conversion.Start();

                // Transcribe audio using Whisper service
                if (_whisperService == null)
                {
                    throw new InvalidOperationException("WhisperService is not initialized.");
                }
                return await _whisperService.TranscribeAudioAsync(audioPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}