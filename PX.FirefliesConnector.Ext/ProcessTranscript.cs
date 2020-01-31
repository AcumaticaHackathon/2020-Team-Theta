using GraphQL.Client;
using GraphQL.Common.Request;
using PX.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.ML;
using PX.FirefliesConnector.Ext.ML;
using PX.Objects.CR;
using PX.Objects.EP;

namespace PX.FirefliesConnector.Ext
{
    public class ProcessTranscript : PXGraph<ProcessTranscript>
    {
        public PXProcessing<TransScriptDetail> transcriptdetail;
        public PXSelect<FirefliesConfig> config;

        protected virtual IEnumerable tranScriptDetail()
        {
            int i = 0;
            foreach (var row in transcriptdetail.Cache.Cached)
            {
                i++;
                yield return row;
            }
            if (i == 0)
            {
                FirefliesConfig firefliesConfig = config.Select();
                string userid = string.Empty;
                if (firefliesConfig != null && (!string.IsNullOrEmpty(firefliesConfig.APIEndPoint) && !string.IsNullOrEmpty(firefliesConfig.Apikey)))
                {
                    var flAuthRequest = new GraphQLRequest
                    {
                        Query = @"
                     {
                          user {
                            user_id
                            email
                            recent_transcript
                          }
                        }
                      "
                    };
                    var graphQLClient = new GraphQLClient(firefliesConfig.APIEndPoint);
                    graphQLClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", firefliesConfig.Apikey));
                    var graphQLResponse = graphQLClient.PostAsync(flAuthRequest).Result;
                    if (graphQLResponse.Errors == null)
                    {
                        var oUser = graphQLResponse.GetDataFieldAs<User>("user");
                        if (!string.IsNullOrEmpty(oUser.user_id))
                        {
                            var variables = new { login = oUser.user_id };
                            var flAuthTranRqu = new GraphQLRequest
                            {
                                Query = @"query ($login: String!)
                                    {
                                           transcripts (user_id: $login){
                                           id
                                           title
                                           date
                                           transcript_url
                                           participants
                                       }
                                    }
                                  ",
                                Variables = variables
                            };
                            var graphQLTranResponse = graphQLClient.PostAsync(flAuthTranRqu).Result;
                            var transcripts = graphQLTranResponse.GetDataFieldAs<Transcript[]>("transcripts");
                            foreach (Transcript transcript in transcripts)
                            {
                                var sentences = transcript.sentences;
                                string participants = String.Join(",", transcript.participants);

                                TransScriptDetail row = new TransScriptDetail();
                                row.Id = transcript.id;
                                row.Title = transcript.title;
                                row.Date = Epoch2UTCNow(transcript.date);
                                row.Transcripturl = transcript.transcript_url;
                                row.Participants = participants;
                                transcriptdetail.Cache.Insert(row);
                                yield return row;
                            }
                        }
                    }
                }
            }
        }

        private DateTime? Epoch2UTCNow(long epoch)
        {
            try
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(epoch);
            }
            catch (Exception ex)
            {
                string exString = ex.Message;
            }
            return null;
        }

        //protected virtual IEnumerable tranScriptDetail()
        //{
        //    int i = 0;
        //    foreach (var row in transcriptdetail.Cache.Cached)
        //    {
        //        i++;
        //        yield return row;
        //    }
        //    if (i == 0)
        //    {
        //        FirefliesConfig firefliesConfig = config.Select();
        //        string userid = string.Empty;
        //        if (firefliesConfig != null && (!string.IsNullOrEmpty(firefliesConfig.APIEndPoint) && !string.IsNullOrEmpty(firefliesConfig.Apikey)))
        //        {
        //            var flAuthRequest = new GraphQLRequest
        //            {
        //                Query = @"
        //             {
        //                  user {
        //                    user_id
        //                    email
        //                    recent_transcript
        //                  }
        //                }
        //              "
        //            };
        //            var graphQLClient = new GraphQLClient(firefliesConfig.APIEndPoint);
        //            graphQLClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", firefliesConfig.Apikey));
        //            var graphQLResponse = graphQLClient.PostAsync(flAuthRequest).Result;
        //            if (graphQLResponse.Errors == null)
        //            {
        //                var oUser = graphQLResponse.GetDataFieldAs<User>("user");
        //                if (!string.IsNullOrEmpty(oUser.user_id))
        //                {
        //                    var variables = new { login = oUser.user_id };
        //                    var flAuthTranRqu = new GraphQLRequest
        //                    {
        //                        Query = @"query ($login: String!)
        //                            {
        //                                   transcripts (user_id: $login){
        //                                   id
        //                                   title
        //                                   sentences
        //                                   {
        //                                      index
        //                                      text
        //                                      raw_text
        //                                      start_time
        //                                      end_time
        //                                      speaker_id
        //                                    }
        //                                   fireflies_users
        //                                   date
        //                                   duration
        //                                   transcript_url
        //                                   participants
        //                               }
        //                            }
        //                          ",
        //                        Variables = variables
        //                    };
        //                    var graphQLTranResponse = graphQLClient.PostAsync(flAuthTranRqu).Result;
        //                    var transcripts = graphQLTranResponse.GetDataFieldAs<Transcript[]>("transcripts");
        //                    foreach (Transcript transcript in transcripts)
        //                    {
        //                        var sentences = transcript.sentences;
        //                        string ffusers = String.Join(",", transcript.fireflies_users);
        //                        string participants = String.Join(",", transcript.participants);
        //                        foreach (Sentence sentence in sentences)
        //                        {
        //                            TransScriptDetail row = new TransScriptDetail();
        //                            row.Id = transcript.id;
        //                            row.Title = transcript.title;
        //                            row.Firefliesusers = ffusers;
        //                            //row.Date = Convert.ToDateTime(transcript.date);
        //                            row.Duration = Convert.ToString(transcript.duration ?? "");
        //                            row.Transcripturl = transcript.transcript_url;
        //                            row.Participants = participants;
        //                            row.Index = sentence.index;
        //                            row.Text = sentence.text;
        //                            row.Rawtext = sentence.raw_text;
        //                            row.Starttime = sentence.start_time.ToString();
        //                            row.Endtime = sentence.end_time.ToString();
        //                            row.Speakerid = sentence.speaker_id.ToString();
        //                            transcriptdetail.Cache.Insert(row);
        //                            yield return row;
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //}
        #region Ctor
        public ProcessTranscript()
        {
            //LoadData();
            transcriptdetail.SetProcessDelegate(
                (list) =>
                {
                    var graph = PXGraph.CreateInstance<ProcessTranscript>();
                    ProcessData(list, graph);
                });
        }
        #endregion

        #region Method
        public static void ProcessData(List<TransScriptDetail> list, ProcessTranscript graph)
        {

            var contMaint = PXGraph.CreateInstance<ContactMaint>();

            //var _mlContext = new MLContext(seed: 0);
            //ITransformer _trainedModel = _mlContext.Model.Load("model.zip", out DataViewSchema inputSchema);
            //PredictionEngine<ConversationInput, SentenceClassifiedOutput> _predEngine = _mlContext.Model.CreatePredictionEngine<ConversationInput, SentenceClassifiedOutput>(_trainedModel);

            foreach (TransScriptDetail transScriptDetail in list)
            {
                var emails = new List<string>();//( transScriptDetail.Participants.Split(','));
                //if (emails.Count == 0)
                {
                    emails.Add("fred@fireflies.ai");
                }
                StringBuilder sb = new StringBuilder();

                
                foreach (var emailContact in emails)
                {
                    var meetingTransciprt = CreateMTActivity(graph, contMaint, emailContact, transScriptDetail);
                    var sentences = meetingTransciprt.Split('.', '!', '?');

                    for (int i = 0; i < sentences.Length; i++)
                    {
                        string sentence = sentences[i];
                        string url = $"https://localhost:44343/api/values/{sentence}";
                        var typeSentence = WebRequestML.HttpGet(url, "", "", "");

                        int addedLength = 0;
                        if (typeSentence == "true")
                        {
                            sb.Append("<br> <span style=\"background-color: rgb(255, 255, 0);\">" + sentence + "?</span>");

                            if (i < sentences.Length - 2)// add few sentences after in order to preserve context
                            {
                                for (int j = i + 1; j < sentences.Length - 1; j++)
                                {
                                    sentence = sentences[j];
                                    addedLength += sentence.Length;
                                    sb.Append("<br>" + sentence);
                                    i = j;
                                    if (addedLength > 300)
                                    {
                                        break;
                                    }
                                }
                            }

                            sb.Append(
                                "<br>----------------------------------------------------------------------------------<br>");
                        }
                    }

                    var questionsText = sb.ToString();
                    CreateMQActivity(graph, contMaint, emailContact, transScriptDetail, questionsText);

                }
                
            }
        }


        //private static bool IsSentenceQuestion(string sentence, PredictionEngine<ConversationInput, SentenceClassifiedOutput> predEngine)
        //{
        //    var convInput = new ConversationInput()
        //    {
        //        Description = sentence
        //    };
        //    var prediction = predEngine.Predict(convInput);
        //    return prediction.Area.Contains("question");
        //}

        private static string CreateMQActivity(ProcessTranscript graph, ContactMaint contMaint, string emailContact,
            TransScriptDetail transScriptDetail, string textBody = null)
        {
            contMaint.Clear();

            contMaint.Contact.Current = PXSelect<Contact,
                Where<Contact.eMail, Equal<Required<Contact.eMail>>>>.Select(contMaint, emailContact);

            var activityData = contMaint.Activities.Insert();
            activityData.Type = "MQ";

            activityData = CreateActivity(graph, contMaint, transScriptDetail, activityData, textBody);

            return activityData.Body;
        }


        private static string CreateMTActivity(ProcessTranscript graph, ContactMaint contMaint, string emailContact,
            TransScriptDetail transScriptDetail)
        {
            contMaint.Clear();

            contMaint.Contact.Current = PXSelect<Contact,
                Where<Contact.eMail, Equal<Required<Contact.eMail>>>>.Select(contMaint, emailContact);

            var activityData = contMaint.Activities.Insert();
            activityData.Type = "MT";

            activityData = CreateActivity(graph, contMaint, transScriptDetail, activityData);

            return activityData.Body;
        }

        private static CRPMTimeActivity CreateActivity(ProcessTranscript graph, ContactMaint contMaint,
            TransScriptDetail transScriptDetail, CRPMTimeActivity activityData, string meetingText = null)
        {
            int minsConsumed = 0;
            if (string.IsNullOrEmpty(meetingText))
            {
                activityData.Body = GetTranscriptInfo(transScriptDetail, graph, out minsConsumed);
            }
            else
            {
                activityData.Body = meetingText;
            }

            activityData.TrackTime = true;
            activityData.IsBillable = false;
            activityData.Summary = transScriptDetail.Title;
            activityData.Subject = transScriptDetail.Title;
            activityData.TimeSpent = minsConsumed == 0 ? 1 : minsConsumed;
            activityData = contMaint.Activities.Update(activityData);
            activityData.ProjectID = 0;
            activityData = contMaint.Activities.Update(activityData);

            contMaint.Persist();
            return activityData;
        }


        public static string GetTranscriptInfo(TransScriptDetail detail, ProcessTranscript graph, out int minutesConsumed)

        {
            minutesConsumed = 0;
            StringBuilder sb = new StringBuilder();
            FirefliesConfig firefliesConfig = graph.config.Select();
            string userid = string.Empty;
            if (firefliesConfig != null && (!string.IsNullOrEmpty(firefliesConfig.APIEndPoint) &&
                                            !string.IsNullOrEmpty(firefliesConfig.Apikey)))
            {
                var variables = new {id = detail.Id};

                var flAuthTranRqu = new GraphQLRequest
                {
                    Query = @"query ($id: String!)
                                        {
                                               transcript (id: $id){
                                               id
                                               title
                                                user{
                                                  minutes_consumed
                                                 
                                                }

                                               sentences
                                               {
                                                  index
                                                  text
                                                  raw_text
                                                  start_time
                                                  end_time
                                                  speaker_id
                                                }
                                           }
                                        }
                                      ",
                    Variables = variables
                };
                var graphQLClient = new GraphQLClient(firefliesConfig.APIEndPoint);
                graphQLClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", firefliesConfig.Apikey));
                var graphQLTranResponse = graphQLClient.PostAsync(flAuthTranRqu).Result;
                var transcript = graphQLTranResponse.GetDataFieldAs<Transcript>("transcript");

                var sentences = transcript.sentences;

                foreach (Sentence sentence in sentences)
                {
                    sb.Append("- Speaker(" + sentence.speaker_id + "):");
                    sb.Append(sentence.text + "." + "<br>");
                }

                minutesConsumed = transcript.user.minutes_consumed;
            }

            return sb.ToString();
        }

            #endregion
    }
}