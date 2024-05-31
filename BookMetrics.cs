using System.Diagnostics.Metrics;

namespace BookService;

public class BookMetrics:IBookMetrics
{
    private Counter<int> RequestCounter { get; } 
    public  BookMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("book-service");

        RequestCounter = meter.CreateCounter<int>("total-rq");
    }

    public void AddRequest()
    {
        RequestCounter.Add(1);
    }
}