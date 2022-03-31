namespace ClassLibrary.Classes;

public class Contract
{
    public Contract(Guid id, Guid jobId, Guid offerId, Guid clientId, Guid providerId, DateTime creationDate, State contractState)
    {
        Id = id;
        JobId = jobId;
        OfferId = offerId;
        ClientId = clientId;
        ProviderId = providerId;
        CreationDate = creationDate;
        ContractState = contractState;
    }

    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Guid OfferId { get; set; }
    public Guid ClientId { get; set; }
    public Guid ProviderId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public State ContractState { get; set; }
}
public enum State
{
    Open,
    Concluded,
    Cancelled
}