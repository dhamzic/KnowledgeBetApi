namespace KnowledgeBet.API.Api.V1.Models.Requests
{
    public class CreatePlayerRequest
    {
        public CreatePlayerRequest(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
