using Google.Cloud.Dialogflow.V2;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using System;
using System.Threading.Tasks;

namespace PD.Services
{
    public class DialogflowService
    {
        private string ProjectId { get; set; }
        private string ServiceAccountKeyPath { get; set; }

        public DialogflowService(string projectId, string serviceAccountKeyPath)
        {
            ProjectId = projectId;
            ServiceAccountKeyPath = serviceAccountKeyPath;
        }

        public async Task<string> DetectIntentAsync(string query, string sessionId = "unique-session-id", string languageCode = "en-US")
        {
            try
            {
                // Create the SessionsClient
                SessionsClient client = CreateSessionsClient();

                // Build the session path
                SessionName sessionName = SessionName.FromProjectSession(ProjectId, sessionId);

                // Build the query input
                QueryInput queryInput = new QueryInput
                {
                    Text = new TextInput
                    {
                        Text = query,
                        LanguageCode = languageCode
                    }
                };

                // Build the request
                DetectIntentRequest request = new DetectIntentRequest
                {
                    SessionAsSessionName = sessionName,
                    QueryInput = queryInput
                };

                // Call the DetectIntent method
                DetectIntentResponse response = await client.DetectIntentAsync(request);

                // Extract the response text
                string fulfillmentText = response.QueryResult.FulfillmentText;

                Console.WriteLine($"  Query: {response.QueryResult.QueryText}");
                Console.WriteLine($"  Response: {fulfillmentText}");
                if (response.QueryResult.Intent != null)
                {
                    Console.WriteLine($"  Intent: {response.QueryResult.Intent.DisplayName}");
                }
                else
                {
                    Console.WriteLine($"  No intent matched.");
                }

                return fulfillmentText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error interacting with Dialogflow: {ex.Message}");
                return "Sorry, I encountered an error.";
            }
        }

        private SessionsClient CreateSessionsClient()
        {
            throw new NotImplementedException();
        }
    }
}
