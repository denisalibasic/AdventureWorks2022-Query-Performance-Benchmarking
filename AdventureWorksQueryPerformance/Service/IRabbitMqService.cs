namespace AdventureWorksQueryPerformance.Service
{
    public interface IRabbitMqService
    {
        void PublishMessage(string message);
        void Close();
    }
}
