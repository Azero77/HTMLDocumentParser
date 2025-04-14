namespace HTMLParser.Models
{
    public class Question
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> QuestionChoices { get; set; } = null!;
        public string QuestionAnswer { get; set; } = string.Empty;
    }

    public class RawQuestion
    {
        public object? QuestionText { get; set; }
        public List<object?> QuestionChoices { get; set; } = null!;
        public object? QuestionAnswer { get; set; }
    }
}
