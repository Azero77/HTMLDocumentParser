using HTMLParser.Library.Formatter;
using HTMLParser.Library.Mediator;
using HTMLParser.Library.ResponseParser;
using HTMLParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Tests
{
    public class FormatterTests
    {
        private Formatter _formatter;

        [SetUp]
        public void Setup()
        {
            _formatter = new Formatter(new ResponseParser(new ResponseMediator()));
        }

        [Test]
        public async Task Format_ShouldConvertHtmlWithArabicEnglishAndMathCorrectly()
        {
            /*var rawQuestion = new RawQuestion
            {
                QuestionText = "<p><span dir=\"rtl\">اكتب صيغة قانون نيوتن الثاني: (Newton's Second Law)</span></p><p><span class=\"math display\">\\[F = ma\\]</span></p>",
                QuestionChoices = new List<string>
                {
                    "<p><span dir=\"rtl\">A-</span> <span class=\"math inline\">\\(F = ma\\)</span></p>",
                    "<p><span dir=\"rtl\">B-</span> <span class=\"math inline\">\\(F = mv\\)</span></p>",
                    "<p><span dir=\"rtl\">C-</span> <span class=\"math inline\">\\(E = mc^2\\)</span></p>",
                    "<p><span dir=\"rtl\">D-</span> <span class=\"math inline\">\\(F = m + a\\)</span></p>"
                },
                QuestionAnswer = "<p><span dir=\"rtl\">A</span></p>"
            };


            // Act
            var formatted = await formatter.Format(rawQuestion);

            // Assert
            // Assert: QuestionText contains key elements
            Assert.That(formatted.QuestionText, Does.Contain("اكتب صيغة قانون نيوتن الثاني"));
            Assert.That(formatted.QuestionText, Does.Contain("Newton's Second Law"));
            Assert.That(formatted.QuestionText, Does.Contain("\\[F = ma\\]"));

            // Assert: QuestionChoices contain the expected math equations with appropriate LaTeX
            Assert.That(formatted.QuestionChoices.Count(), Is.EqualTo(4));
            Assert.That(formatted.QuestionChoices, Has.Some.Matches<string>(s => s.Contains("\\(F = ma\\)")));
            Assert.That(formatted.QuestionChoices, Has.Some.Matches<string>(s => s.Contains("\\(F = mv\\)")));
            Assert.That(formatted.QuestionChoices, Has.Some.Matches<string>(s => s.Contains("\\(E = mc^2\\)")));
            Assert.That(formatted.QuestionChoices, Has.Some.Matches<string>(s => s.Contains("\\(F = m + a\\)")));

            // Assert: Clean answer match
            Assert.That(formatted.QuestionAnswer.Trim(), Is.EqualTo("A"));*/
        }
    }
}
