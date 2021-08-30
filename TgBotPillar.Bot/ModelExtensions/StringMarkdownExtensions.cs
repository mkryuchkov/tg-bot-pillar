using System.Text;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class StringExtensions
    {
        private static readonly string[] MarkdownV2EscapeCharacters =
        {
            "_", "*", "[", "]", "(", ")", "~", "`", ">", "#", "+", "-", "=", "|", "{", "}", ".", "!"
        };

        public static string EscapeMarkdownV2(this string value)
        {
            var builder = new StringBuilder(value);

            foreach (var character in MarkdownV2EscapeCharacters)
            {
                builder.Replace(character, $"\\{character}");
                builder.Replace($"\\\\{character}", character);
            }

            return builder.ToString();
        }
    }
}