﻿using KitAplication.Models;
using System.Text.Json.Serialization;
using System.Text;
using Newtonsoft.Json;
using KitAplication.Interface;

namespace KitAplication.Services
{
    public class ChatAIService : IChatAIService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IMessageService _messageService;
        private readonly IChatSettingsService _chatSettingsService;
        private readonly ILogger<ChatAIService> _logger;

        public ChatAIService(IConfiguration config, HttpClient httpClient, IMessageService messageService, IChatSettingsService chatSettingsService, ILogger<ChatAIService> logger)
        {
            _config = config;
            _httpClient = httpClient;
            _messageService = messageService;
            _chatSettingsService = chatSettingsService;
            _logger = logger;
        }

        /// <summary>
        /// Generate a response from OpenAI's chatbot API using the user input and system messages
        /// </summary>
        /// <param name="userinput">The user's input to the chatbot</param>
        /// <param name="systemModel">The system model to use for generating the response</param>
        /// <param name="chatSettingsModel">The chat settings model containing the fail message to return if a response cannot be generated</param>
        /// <returns>A string representing the response generated by the chatbot API. or chatsettings fail message if respons is null</returns>
        public async Task<string> GetResponsFromChatAI(string userinput, SystemModel systemModel, ChatSettingsModel chatSettingsModel)
        {
           
            var apiKey = _config["MySecretValues:ApiKey"];
            var userName = "user";

            var chatRequest = new ChatRequest
            {
                model = systemModel.Model
            };

            /*add system role and content first to list*/
            var chatPrompts = new List<ChatPrompt>
            {
                new ChatPrompt { role = systemModel.RoleName, content = systemModel.SystemContent }
            };
            /* Get the systems messages role and content from database and add to list */
            var systemMessages = await _messageService.GetMessagesAsync(systemModel.Id); 
            if (systemMessages != null) 
            {
                foreach (var message in systemMessages)
                {
                    chatPrompts.Add(new ChatPrompt { role = message.RoleName, content = message.Content});
                };
            }

            /* Add system suffix to userinput, add to list (always add userinput at the end of list)*/
            var formatUserInput = String.Format("{0} {1}", systemModel.Prefix, userinput);
            chatPrompts.Add(new ChatPrompt() { role = userName, content = formatUserInput });

            chatRequest.messages = chatPrompts;

            /*Serielize payload to jason and make post request to open AI api*/
            var payloadJsonString = JsonConvert.SerializeObject(chatRequest);
            var responseJson = await GetCompletionAsync(payloadJsonString, apiKey);

           
            if (responseJson == null)
                return chatSettingsModel.RequestFailMessage;

            var responseMessage = JsonConvert.DeserializeObject<ResponsAnswer>(responseJson);

            /* if responsmessage exist returs a respons string, else return chatsettings failmessage string*/
            if (responseMessage != null && responseMessage.choices!=null)
            {
                foreach (var c in responseMessage.choices)
                {
                    if (c.message != null && c.message.content != null)
                    {
                        return c.message.content;
                    };
                }
            }
            return chatSettingsModel.RequestFailMessage;
        }
        /// <summary>
        /// Sends a POST request to OpenAI API to generate a completion of the given text prompt using the provided API key.
        ///  Returns the response JSON as a string if the request is successful, otherwise logs the error and returns null.
        /// </summary>
        /// <param name="jsonString">The text prompt to generate a completion for in JSON format.</param>
        /// <param name="apiKey">The API key to use for authentication</param>
        /// <returns>The response JSON as a string if the request is successful, otherwise null</returns>
        private async Task<string?> GetCompletionAsync(string jsonString, string apiKey)
        {

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return responseJson;
            }
            else
            {
                //var responseJson =  response.StatusCode.ToString();
                _logger.LogError("Date: {0}.Request to openai {1} ", DateTime.Now, response.ReasonPhrase.ToString() );
                return null;
            }
        }

        public class ChatPrompt
        {
            public string? role { get; set; }
            public string? content { get; set; }
        }
        public class ChatRequest
        {
            public string? model { get; set; }
            public IList<ChatPrompt>? messages { get; set; }
        }
        public class ResponsAnswer
        {
            public string? id { get; set; }
            [JsonPropertyName("object")]
            public string? responsobject { get; set; }
            public string? created { get; set; }
            public string? model { get; set; }
            public usage? usage { get; set; }
            public List<choices>? choices { get; set; }
        }
        public class choices
        {
            public message? message { get; set; }
            public string? finish_reason { get; set; }
            public int index { get; set; }
        }
        public class message
        {
            public string? role { get; set; }
            public string? content { get; set; }
        }
        public class usage
        {
            public int prompt_tokens { get; set; }
            public int completion_tokens { get; set; }
            public int total_tokens { get; set; }
        }
    }
}
