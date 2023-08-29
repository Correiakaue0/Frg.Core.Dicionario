namespace Domain.DTO
{
    public class WordsDTO
    {
        public WordsDTO(int language, string? name, string? value)
        {
            Language = language;
            Name = name;
            Value = value;
        }

        public int Language { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
