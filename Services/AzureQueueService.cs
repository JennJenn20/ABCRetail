using ABCRetailWebApplication.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text.Json;

namespace ABCRetailWebApplication.Services
{
    public class AzureQueueService
    {
        private readonly QueueClient _queueClient;

        public AzureQueueService(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists(); // Create the queue if it doesn't exist
        }

        public async Task SendMessage<T>(T message)
        {
            string messageContent = JsonSerializer.Serialize(message);
            await _queueClient.SendMessageAsync(messageContent);
        }

        public async Task<List<QueueMessage>> GetMessages()
        {
            var messages = await _queueClient.ReceiveMessagesAsync(maxMessages: 10);
            return new List<QueueMessage>(messages.Value);
        }

        public async Task<QueueMessage> GetMessageById(string messageId)
        {
            var messages = await GetMessages();
            return messages.Find(m => m.MessageId == messageId);
        }

        public async Task DeleteMessage(string messageId)
        {
            await _queueClient.DeleteMessageAsync(messageId, "POP_RECEIPT"); // Replace POP_RECEIPT accordingly.
        }
    }
}
