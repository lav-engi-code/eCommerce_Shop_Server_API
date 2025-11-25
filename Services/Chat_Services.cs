using eCommerce_Shop_Server_API.Modals;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace eCommerce_Shop_Server_API.Services
{
    public class Chat_Services
    {
        private readonly IChatCompletionService _chatCompletion;

        public Chat_Services(IChatCompletionService chatCompletion)
        {
            _chatCompletion = chatCompletion;
        }

        public async Task<Chat_Message> SendMessage(Chat_Message msg)
        {
            var result = await _chatCompletion.GetChatMessageContentAsync(msg.Chat);

            Chat_Message response = new Chat_Message
            {
                Chat = msg.Chat,
                Response = result.ToString()
            };

            return response;
        }
    }
}











