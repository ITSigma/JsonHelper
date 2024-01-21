using JsonHelper.Application;
using JsonHelper.Domain;

namespace JsonHelper.UserInterface
{
    public class GetAndWriteMatchCommand : ConsoleCommand
    {
        private static string baseUrl = "http://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/v1";
        private TextWriter writer;

        public GetAndWriteMatchCommand(TextWriter writer)
            : base("match", "match <matchID, directoryPath, keyPath>      "
                  + "# Get match by match ID and save it to directory."
                  + "\n- match : ID of dota match "
                  + "\n- directoryPath : path to directory to save match "
                  + "\n- keyPath : path to file with steam keys ", 3)
        {
            this.writer = writer;
        }

        public async override Task Execute(string[] args)
        {
            var (matchID, directoryPath, keyPath) = GetArgs(args);
            await HandleArgsAsync(matchID, directoryPath, keyPath);
        }


        private async Task HandleArgsAsync(string matchID, string directoryPath, string keyPath)
        {
            writer.WriteLine($"Try get and write {matchID}.");
            var getter = new GetApplication<SteamAPIResult<Match>>(baseUrl,
                new KeyGetter(keyPath), 0);
            var matchWriter = new WriteApplication<Match>(directoryPath,
                new DefaultFileNameBuilder<Match>(match => match.match_id.ToString(), "Match"));
            var getValue = await getter.GetValueAsync(new Dictionary<string, string>() { ["match_id"] = matchID });
            await matchWriter.WriteValueAsync(getValue.result);
            writer.WriteLine($"Ok! {matchID} has already written to {directoryPath}.");
        }

        private (string, string, string) GetArgs(string[] args)
        {
            CheckArgumentsCount(args);
            var matchID = args[0];
            var directoryPath = args[1];
            var keyPath = args[2];
            if (!long.TryParse(matchID, out var parseResult))
                throw new ArgumentException($"Provided match ID {matchID} is wrong. Check that value is long int.");
            return (matchID, directoryPath, keyPath);
        }
    }
}
