namespace ClassLibrary.Classes;

public class Offer
{
    public Offer(Guid id, Guid jobId, Guid providerId, int price, string duration, DateTime date)
    {
        Id = id;
        JobId = jobId;
        ProviderId = providerId;
        Price = price;
        Duration = duration;
        Date = date;
    }

    public Guid Id{get;set;}
    public Guid JobId{get;set;}
    public Guid ProviderId{get;set;}
    public Guid? PreviousOfferId{get;set;}
    public int Price{get;set;}
    public string Duration{get;set;}
    public DateTime Date{get;set;}
}