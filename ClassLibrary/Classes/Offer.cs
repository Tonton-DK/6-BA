namespace ClassLibrary.Classes;

public class Offer
{
    public Offer(Guid id, Guid jobId, Guid providerId, int price, string duration, DateTime date, State state, string comment)
    {
        Id = id;
        JobId = jobId;
        ProviderId = providerId;
        Price = price;
        Duration = duration;
        Date = date;
        State = state;
        Comment = comment;
    }

    public Offer()
    {
        Id = Guid.Empty;
        JobId = Guid.Empty;
        ProviderId = Guid.Empty;
        Price = 0;
        Duration = string.Empty;
        Date = DateTime.Now.Date;
        State = State.Open;
        Comment = string.Empty;
    }

    public Guid Id{get;set;}
    public Guid JobId{get;set;}
    public Guid ProviderId{get;set;}
    public Guid? PreviousOfferId{get;set;}
    public int Price{get;set;}
    public string Duration{get;set;}
    public DateTime Date{get;set;}
    public State State{get;set;}
    public string Comment{get;set;}
}