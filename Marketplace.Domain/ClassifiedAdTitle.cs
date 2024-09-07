using Marketplace.Framework;
using System.Text.RegularExpressions;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        public static ClassifiedAdTitle FromString(string title) => new ClassifiedAdTitle(title);
        private readonly string _value;
        public ClassifiedAdTitle(string value)
        {
            if (value.Length > 100)
                throw new ArgumentException("Title cannot be longer that 100 characters", nameof(value));

            _value = value;
        }

        public static ClassifiedAdTitle FromHtml(string htmlTitle)
        {
            var supportedTagsReplaced = htmlTitle
                .Replace("<i>", "*")
                .Replace("</i>", "*")
                .Replace("<b>", "**")
                .Replace("</b>", "**");

            return new ClassifiedAdTitle(Regex.Replace(supportedTagsReplaced, "<.*?>", string.Empty));
        }
    }
}
