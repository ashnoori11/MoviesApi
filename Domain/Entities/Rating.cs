using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;
public class Rating
{
    private Rating()
    {

    }

    public Rating(int rate, int movieId, string userId)
    {
        Rate = rate;
        MovieId = movieId;
        UserId = userId;
    }

    #region methods
    public void SetRelatedData(Movie movie, IdentityUser user)
    {
        Movie = movie;
        User = user;
    }
    public void SetRelatedData(Movie movie) => Movie = movie;
    public void SetRelatedData(IdentityUser user) => User = user;
    public void UpdateRate(int rate)
    {
        if (rate >= 1 || rate <= 5)
        {
            Rate = rate;
        }
    }

    public void IncreaseRate()
    {
        if (Rate < 5)
        {
            Rate++;
        }
    }
    public void DecreaseRate()
    {
        if (Rate > 1)
        {
            Rate--;
        }
    }
    #endregion

    public int Id { get; private set; }
    public int Rate { get; private set; }

    public int MovieId { get; private set; }
    public string UserId { get; private set; }

    public virtual Movie Movie { get; private set; }
    public virtual IdentityUser User { get; private set; }
}
