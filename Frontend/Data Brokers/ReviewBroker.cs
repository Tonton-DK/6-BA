using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace Frontend.Data_Brokers;

public class ReviewBroker : BaseBroker, IReviewService
{
    private static readonly string baseUri = "http://review-service:80/ReviewService";
    
    public bool Get()
    {
        var t = Get<bool>(baseUri+"/Get");
        if (t != null) return t.Result;
        return false;
    }

    public Review? CreateReview(Review review)
    {
        var uri = baseUri + "/CreateReview";
        var content = new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json");
        var t = Post<Review?>(uri, content);
        if (t != null) return t.Result;
        return null;
    }

    public Review? GetReviewById(Guid id)
    {
        var uri = baseUri + "/GetReviewById/" + id;
        var t = Get<Review?>(uri);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Review> ListReviews(Guid userId, ReviewType type)
    {
        var uri = baseUri + "/ListReviews/"+userId;
        var content = new StringContent(JsonConvert.SerializeObject(type), Encoding.UTF8, "application/json");
        var t = Post<Review[]>(uri, content);
        if (t != null) return new List<Review>(t.Result);
        return null;
    }

    public double GetRating(Guid userId, ReviewType type)
    {
        var uri = baseUri + "/GetRating/"+userId;
        var content = new StringContent(JsonConvert.SerializeObject(type), Encoding.UTF8, "application/json");
        var t = Post<double>(uri, content);
        if (t != null) return t.Result;
        return 0;
    }

    public Review? UpdateReview(Review review)
    {
        var uri = baseUri + "/UpdateReview";
        var content = new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json");
        var t = Put<Review?>(uri, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool DeleteReview(Guid id)
    {
        var uri = baseUri + "/DeleteReviewById/" + id;
        var t = Delete<bool>(uri);
        if (t != null) return t.Result;
        return false;
    }
}