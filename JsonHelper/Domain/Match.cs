namespace JsonHelper.Domain
{
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
            => match_id;
    }
}
