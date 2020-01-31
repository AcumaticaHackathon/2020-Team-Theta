using GraphQL.Client;

namespace PX.FirefliesConnector.Ext
{
    public class User
    {
        public string user_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int num_transcripts { get; set; }
        public object recent_transcript { get; set; }
        public int minutes_consumed { get; set; }
        public bool is_admin { get; set; }
        public object[] integrations { get; set; }
    }

    public class Transcript
    {
        public string id { get; set; }
        public string title { get; set; }
        public Sentence[] sentences { get; set; }
        public string[] fireflies_users { get; set; }
        public long date { get; set; }
        public object duration { get; set; }
        public string transcript_url { get; set; }
        public string[] participants { get; set; }

        public User user { get; set; }
    }

    public class Sentence
    {
        public int index { get; set; }
        public string text { get; set; }
        public string raw_text { get; set; }
        public float start_time { get; set; }
        public float end_time { get; set; }
        public int speaker_id { get; set; }
    }
}