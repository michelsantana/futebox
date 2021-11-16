using Futebox.Models;
using Futebox.Services.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class YoutubeService : IYoutubeService
    {
        static UserCredential _credential;

        public YoutubeService()
        {
        }

        public bool IsLogged()
        {
            return !(_credential == null);
        }

        private UserCredential LoadCredential()
        {
            if (_credential == null)
                using (var stream = new FileStream($"{Settings.BackendRoot}/client_secrets.json", FileMode.Open, FileAccess.Read))
                {
                    _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        // This OAuth 2.0 access scope allows an application to upload files to the
                        // authenticated user's YouTube channel, but doesn't allow other types of access.
                        new[] { YouTubeService.Scope.YoutubeUpload },
                        "user",
                        CancellationToken.None
                    ).Result;
                }
            return _credential;
        }

        public async void DoLogin()
        {
            if (_credential == null) LoadCredential();
            await _credential.RefreshTokenAsync(CancellationToken.None);
        }

        public async void DoLogout()
        {
            if (_credential == null) LoadCredential();
            await _credential.RevokeTokenAsync(CancellationToken.None);
        }

        public Task Upload(Processo processo)
        {
            if (!IsLogged())
            {
                DoLogin();
                return Task.FromResult(true);
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = processo.attrTitulo;
            video.Snippet.Description = processo.attrDescricao;
            video.Snippet.Tags = new string[] { "futebol", "football", "classificação", "classificacao", "brasileirao", "rodada", "serie a", "serie b", "campeonato", "campeonato brasileiro", "brasileirão" };
            video.Snippet.CategoryId = "17";
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "private"; // or "private" or "public"
            var filePath = processo.arquivoVideo; // Replace with path to actual movie file.


            //var video = new Video();
            //video.Snippet = new VideoSnippet();
            //video.Snippet.Title = "Default Video Title";
            //video.Snippet.Description = "Default Video Description";
            //video.Snippet.Tags = new string[] { "tag1", "tag2" };
            //video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            //video.Status = new VideoStatus();
            //video.Status.PrivacyStatus = "unlisted"; // or "private" or "public"
            //var filePath = @"D:\Notebook\Workspace\pessoal\FuteBox\git\futebox\Robot\archive\Upload\a.mp4"; // Replace with path to actual movie file.

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                return videosInsertRequest.UploadAsync();
            }
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
        }
    }
}