namespace HTMLParser.Models
{
    public class Question
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> QuestionChoices { get; set; } = null!;
        public string Answer { get; set; } = string.Empty;
    }

    public class RawQuestion
    {
        public object? QuestionText { get; set; }
        public List<object?> QuestionChoices { get; set; } = null!;
        public object? Answer { get; set; }
    }
}
