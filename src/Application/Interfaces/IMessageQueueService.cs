namespace Application.Interfaces;

public interface IMessageQueueService
{
    void PublishMessage(string queue, string message);
    void ConsumeMessages(string queue, Action<string> callback);
    void CloseConnection();
}