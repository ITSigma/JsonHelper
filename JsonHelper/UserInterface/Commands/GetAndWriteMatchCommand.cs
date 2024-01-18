using JsonHelper.Application;
using JsonHelper.Domain;

namespace JsonHelper.UserInterface
{
    public class GetAndWriteMatchCommand : ConsoleCommand
    {
        private GetAndWriteApplication<SteamAPIResult<Match>, Match> app;
        private static string baseUrl = "http://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/v1";
        private static UrlCompiler urlCompiler = new UrlCompiler(baseUrl);
        private TextWriter writer;

        public GetAndWriteMatchCommand(TextWriter writer)
            : base("match", "match <matchID, directoryPath>      # Get match and Save it to directory ")
        {
            this.writer = writer;
        }

        public override void Execute(string[] args)
        {
            var matchID = args[0];
            var directoryPath = args[1];

            app = new GetAndWriteApplication<SteamAPIResult<Match>, Match>(directoryPath,
                new DefaultFileNameBuilder<Match>(),
                (getResult) => getResult.result);

            writer.WriteLine($"Start saving {matchID}...");
            var url = urlCompiler
                .Compile(new Dictionary<string, string>()
                {
                    ["match_id"] = matchID,
                });
            //var url = $"https://api.opendota.com/api/matches/{matchID}&";
            //writer.WriteLine(url);
            app.Execute(url);
            writer.WriteLine($"Save {matchID}");
        }
    }
}
