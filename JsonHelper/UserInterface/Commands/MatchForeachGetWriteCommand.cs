using Castle.Core.Internal;
using JsonHelper.Application;
using JsonHelper.Domain;
using System.Collections.Generic;
using System.Linq;

namespace JsonHelper.UserInterface.Commands
{
    public class MatchForeachGetWriteCommand : ConsoleCommand
    {
        private const int thresholdCount = 20;
        private const int writeDelay = 2000;
        private const string baseUrl = "http://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/v1";
        private readonly TextWriter writer;
        private List<Match> writeValues;
        private bool isEndGetting;



        public MatchForeachGetWriteCommand(TextWriter writer)
            : base("matchFor", "matchFor <matchIDFrom, matchIDTo, directoryPath, keyPath>      "
                  + "# Get matches from matchIDFrom to matchIDTo by match ID and Save it to directory."
                  + "\n- matchIDFrom : start ID of dota match "
                  + "\n- matchIDTo : end ID of dota match "
                  + "\n- directoryPath : path to directory to save match "
                  + "\n- keyPath : path to file with steam keys ", 4)
        {
            this.writer = writer;
            writeValues = new();
        }

        public async override Task Execute(string[] args)
        {
            var (matchIDFrom, matchIDTo, directoryPath, keyPath) = GetArgs(args);
            await HandleArgsAsync(matchIDFrom, matchIDTo, directoryPath, keyPath);
        }

        private async Task HandleArgsAsync(long matchIDFrom, long matchIDTo, string directoryPath, string keyPath)
        {
            var keyGetter = new KeyGetter(keyPath);
            var getter = new GetApplication<SteamAPIResult<Match>>(baseUrl,
                keyGetter, 1000 / keyGetter.Count);
            var matchWriter = new WriteApplication<List<Match>>(directoryPath,
                new DefaultFileNameBuilder<List<Match>>(match =>
                    $"{match.Select(match => match.match_id).Min()}" +
                    $"_{match.Select(match => match.match_id).Max()}", "Matches"));

            await writer.WriteLineAsync($"Try get and write from {matchIDFrom} to {matchIDTo}.");
            await writer.WriteLineAsync("");

            var tasks = new List<Task>();
            tasks.Add(HandleGettingAllMatches(matchIDFrom, matchIDTo, keyGetter.Count, getter));
            tasks.Add(WriteSavedValuesInfinitely(matchWriter, directoryPath));
            await Task.WhenAll(tasks);

            await writer.WriteLineAsync("");
            await writer.WriteLineAsync("OK!");
            await writer.WriteLineAsync($"Matches from {matchIDFrom} to {matchIDTo} " +
                                        $"has already written to {directoryPath}.");
        }

        private async Task HandleGettingAllMatches(
            long matchIDFrom,
            long matchIDTo,
            int keysCount,
            GetApplication<SteamAPIResult<Match>> getter)
        {
            for (var matchStart = matchIDFrom; matchStart <= matchIDTo; matchStart += keysCount)
            {
                var startCount = writeValues.Count;
                var matchEnd = Math.Min(matchStart + keysCount - 1, matchIDTo);
                await writer.WriteLineAsync($"Try get matches from {matchStart} to {matchEnd}.");
                await HandleMatchesByBatch(matchStart, matchEnd, getter);
                await writer.WriteLineAsync($"Ok! Matches from {matchStart} to {matchEnd} has already getted.");
            }
            isEndGetting = true;
        }

        private async Task WriteSavedValuesInfinitely(WriteApplication<List<Match>> matchWriter,
            string directoryPath)
        {
            while (!isEndGetting || writeValues.Count > 0)
            {
                if (writeValues.Count > thresholdCount || isEndGetting)
                {
                    await WriteSavedValues(matchWriter, directoryPath,
                        writeValues.Take(thresholdCount).ToList());
                    writeValues = writeValues.Skip(thresholdCount).ToList();
                }
                await Task.Delay(writeDelay);
            }
        }

        private async Task WriteSavedValues(WriteApplication<List<Match>> matchWriter, string directoryPath,
            List<Match> values)
        {
            await matchWriter.WriteValueAsync(values);
            var matchIDs = values
                .Select(match => match.match_id);
            await writer.WriteLineAsync($"Ok! Matches from {matchIDs.Min()} to {matchIDs.Max()} " +
                                    $"has already written to {directoryPath}.");
        }

        private async Task HandleMatchesByBatch(
            long matchIDFrom,
            long matchIDTo,
            GetApplication<SteamAPIResult<Match>> getter)
        {
            var getTasks = new List<Task<SteamAPIResult<Match>>>();
            for (var matchID = matchIDFrom; matchID <= matchIDTo; matchID++)
                getTasks.Add(HandleGetMatch(matchID, getter));
            var matchResponses = await Task.WhenAll(getTasks);
            writeValues.AddRange(matchResponses
                .Where(matchResponse => matchResponse.result is not null)
                .Select(matchResponse => matchResponse.result)
                .Where(match => match.match_id != 0));
        }

        private (long, long, string, string) GetArgs(string[] args)
        {
            CheckArgumentsCount(args);
            var matchIDFrom = args[0];
            var matchIDTo = args[1];
            var directoryPath = args[2];
            var keyPath = args[3];

            if (!long.TryParse(matchIDFrom, out var parseResultFrom))
                throw new ArgumentException($"Provided start match ID {matchIDFrom} is wrong. " +
                    $"Check that value is long int.");

            if (!long.TryParse(matchIDTo, out var parseResultTo))
                throw new ArgumentException($"Provided end match ID {matchIDTo} is wrong. " +
                    $"Check that value is long int.");

            if (parseResultFrom > parseResultTo)
                throw new ArgumentException("Provided matchIDFrom is greater than matchIDTo.");

            return (parseResultFrom, parseResultTo, directoryPath, keyPath);
        }

        private async Task<SteamAPIResult<Match>> HandleGetMatch(long matchID,
            GetApplication<SteamAPIResult<Match>> getter)
        {
            return await getter.GetValueAsync(new Dictionary<string, string>()
            {
                ["match_id"] = matchID.ToString()
            });
        }
    }
}