using ResHub.Models;

namespace ResHub.Services.Interfaces
{
    public interface IMessageService
    {
        Task<List<Message>> MarkMessagesAsReadAsync(string otherUserId);
    }
}
