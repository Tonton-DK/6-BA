namespace ClassLibrary.Classes;

public class Offer
{
    // With previous offer
    public Offer(int id, int jobId, int providerId, int previousOfferId, int price, string duration, DateTime date)
    {
        Id = id;
        JobId = jobId;
        ProviderId = providerId;
        PreviousOfferId = previousOfferId;
        Price = price;
        Duration = duration;
        Date = date;
    }

    // Without previous offer
    public Offer(int id, int jobId, int providerId, int price, string duration, DateTime date)
    {
        Id = id;
        JobId = jobId;
        ProviderId = providerId;
        Price = price;
        Duration = duration;
        Date = date;
    }

    public int Id{get;set;}
    public int JobId{get;set;}
    public int ProviderId{get;set;}
    public int PreviousOfferId{get;set;}
    public int Price{get;set;}
    public string Duration{get;set;}
    public DateTime Date{get;set;}
}