using JsonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper
{
    public class GetAndSaveMatchCommand : ConsoleCommand
    {
        private GetAndSaveApplication<Match> app;
        private static string baseUrl = "http://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/v1";
        private UrlCompiler urlCompiler = new UrlCompiler(baseUrl);
        private TextWriter writer;

        public GetAndSaveMatchCommand(TextWriter writer) 
            : base("match", "match <matchID, directoryPath>      # Get match and Save it to directory ")
        {
            this.writer = writer;
        }

        public override void Execute(string[] args)
        {
            var matchID = args[0];
            var directoryPath = args[1];
            app = new GetAndSaveApplication<Match>(directoryPath, new DefaultFileNameBuilder<Match>());
            writer.WriteLine($"Start saving {matchID}");
            //var url = urlCompiler
            //    .Compile(new Dictionary<string, string>()
            //    {
            //        ["match_id"] = matchID,
            //        ["key"] = "A893C825081A1C0EE8F9E7DA5E090928"
            //    });
            var url = $"https://api.opendota.com/api/matches/{matchID}";
            writer.WriteLine(url);
            app.Execute(url);
            writer.WriteLine($"Save {matchID}");
        }
    }

    internal class DefaultFileNameBuilder<T> : IFileNameBuilder<T>
    {
        public string BuildName(T value, string fileExtension)
            => $"{value}.{fileExtension}";
    }
}


 namespace JsonHelper
{
    internal class GetAndSaveApplication<T>
    {
        private string directoryPath { get; }
        private IFileNameBuilder<T> nameBuilder { get; }
        private static readonly HttpRequestHandler requestHandler = new HttpRequestHandler();
        private static readonly object lockObject = new object();

        public GetAndSaveApplication(string directoryPath, IFileNameBuilder<T> nameBuilder)
        {
            this.directoryPath = directoryPath;
            this.nameBuilder = nameBuilder;
        }
 
        public async void Execute(string url)
        {
            var valueToSave = await requestHandler.GetRequestAsync<T>(url);
            var filename = nameBuilder.BuildName(valueToSave, "json");
            lock (lockObject)
            {
                FileSaverBase<T>.SaveToFileAsync(valueToSave, directoryPath, filename);
            }
        }
    }

    internal interface IUrlCompiler
    {
        public string BaseUrl { get; }
        public string Compile(Dictionary<string, string> headerValues);
    }

    internal class UrlCompiler : IUrlCompiler
    {
        public string BaseUrl { get; }

        public UrlCompiler(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string Compile(Dictionary<string,string> headerValues)
        {
            var headers = string.Join('&', headerValues
                .Select(header => $"{header.Key}={header.Value}")
                .ToArray());
            return headers.Length > 0 ? $"{BaseUrl}?{headers}" : BaseUrl;
        }
    }
}

namespace JsonHelper
{
    internal interface IUrlValue<T>
    {
        public string Url { get; }
    }

    internal interface UrlValue<Match>
    {
        public string Url { get => "IDOTA2Match_570/GetMatchDetails/v1"; }

    }

    public class PickBan
    {
        public bool is_pick { get; set; }
        public int hero_id { get; set; }
        public int team { get; set; }
        public int order { get; set; }

    }

    public class PlayerPicks
    {
        public int hero_id { get; set; }
        public int team_number { get; set; }
        public int leaver_status { get; set; }

    }

    internal class Match
    {
        public long match_id { get; set; }
        public bool radiant_win { get; set; }
        public int duration { get; set; }
        public int? skill { get; set; }
        public int region { get; set; }
        public int game_mode { get; set; }
        public int lobby_type { get; set; }
        public object all_word_counts { get; set; }
        public IList<PickBan> picks_bans { get; set; }
        public IList<PlayerPicks> players { get; set; }
        public int human_players { get; set; }

        public override string ToString()
        {
            return match_id.ToString();
        }
    }
}
