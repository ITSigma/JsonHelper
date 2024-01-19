using JsonHelper.Application;
using JsonHelper.Domain;

namespace JsonHelper.UserInterface
{
    public class GetAndWriteMatchCommand : ConsoleCommand
    {
        private GetAndWriteApplication<SteamAPIResult<Match>, Match> app;
        private static string baseUrl = "http://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/v1";
        private TextWriter writer;

        
        public GetAndWriteMatchCommand(TextWriter writer)
            : base("match", "match <matchID, directoryPath, keyPath>      "
                  + "# Get match by match ID and Save it to directory."
                  + "\n- match : ID of dota match "
                  + "\n- directoryPath : path to directory to save match "
                  + "\n- keyPath : path to file with steam keys ", 3)
        {
            this.writer = writer;
        }

        public override void Execute(string[] args)
        {
            if (!CheckArgumentsCount(writer, args))
                return;
            var matchID = args[0];
            var directoryPath = args[1];
            var keyPath = args[2];
            if (!long.TryParse(matchID, out var parseResult))
            {
                writer.WriteLine($"Your match ID {matchID} is wrong. Check that value is long int.");
                return;
            }
            HandleArgs(matchID, directoryPath, keyPath);
        }

        private void HandleArgs(string matchID, string directoryPath, string keyPath)
        {
            try
            {
                app = new GetAndWriteApplication<SteamAPIResult<Match>, Match>(directoryPath,
                   new DefaultFileNameBuilder<Match>(),
                   (getResult) => getResult.result,
                   baseUrl, new KeyGetter(keyPath), 0);
                writer.WriteLine($"Try get ang write match with id {matchID}.");
                app.Execute(new Dictionary<string, string>() { ["match_id"] = matchID });
            }
            catch (Exception e)
            {
                writer.WriteLine($"Error!\nError message: {e.Message}");
                return;
            }
            writer.WriteLine($"\rOk! {matchID} has already written to {directoryPath}.");
        }
    }
}
