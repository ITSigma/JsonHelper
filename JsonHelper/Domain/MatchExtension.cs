namespace JsonHelper.Domain
{
    internal static class MatchExtension
    {
        public static MatchDataToWrite ToMatchDataToWrite(this Match match)
        {
            if (!match.Check())
                return null;
            var (radiantTeam, direTeam) = match.GetRadiantAndDireTeams();
            var matchData = new MatchDataToWrite
            {
                RadiantFeatures = radiantTeam.ToArray(),
                DireFeatures = direTeam.ToArray(),
                RadiantWin = match.radiant_win
            };
            return matchData;
        }

        private static bool Check(this Match match)
        {
            var requiredGameMods = new HashSet<int>() { 23, 18, 15, 11, 10, 7, 0 };
            if (match.picks_bans == null
                || !requiredGameMods.Contains(match.game_mode)
                || match.human_players < 10)
                return false;

            var isNobodyLeft = match.players
                .Select(player => player.leaver_status)
                .All(status => status == 0);
            if (!isNobodyLeft)
                return false;
            return true;
        }

        private static (List<float> radiantTeam, List<float> direTeam)
            GetRadiantAndDireTeams(this Match match)
        {
            var radiantTeam = new List<float>();
            var direTeam = new List<float>();
            foreach (var player in match.players)
            {
                if (player.team_number == 0)
                    radiantTeam.Add(player.hero_id);
                else if (player.team_number == 1)
                    direTeam.Add(player.hero_id);
            }
            return (radiantTeam, direTeam);
        }
    }
}
