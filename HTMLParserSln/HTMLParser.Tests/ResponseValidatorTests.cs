using HTMLParser.Library;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace HTMLParser.Tests
{
    internal class ResponseValidatorTests
    {
        [Test]
        public async Task IsJsonRepairedAsync()
        {
            //Arrange
            string malformedJson = @"[
 {
 ""QuestionText"": ""<td><p><span dir=\""rtl\"">اكتب علاقة ثابت التوازن الكيميائي بدلالة التراكيز للتفاعل الآتي:</span></p><p><span class=\""math display\"">\\[\\text{CaC}{O_{3}}_{\\left( s \\right)} \\rightleftharpoons CaO_{\\left( s \\right)} + C{O_{2}}_{\\left( g \\right)}\\]</span></p></td>"",
 ""QuestionChoices"": [
 ""<p><span dir=\""rtl\"">A-</span> <span class=\""math inline\"">\\(K_{c} = \\frac{\\left\\lbrack \\text{CaC}O_{3} \\right\\rbrack}{\\left\\lbrack \\text{CaO} \\right\\rbrack.\\left\\lbrack CO_{2} \\right\\rbrack}\\)</span></p>"",
 ""<p><span dir=\""rtl\"">B-</span> <span class=\""math inline\"">\\(K_{c} = \\lbrack CO_{2}\\rbrack\\)</span></p>"",
 ""<p><span dir=\""rtl\"">C-</span> <span class=\""math inline\"">\\(Kc = \\lbrack CaCO_{3}\\rbrack\\)</span></p>"",
 ""<p><span dir=\""rtl\"">D-</span> <span class=\""math inline\"">\\(K_{c} = \\left\\lbrack CO_{2} \\right\\rbrack.\\lbrack CaCO_{3}\\)</span></p>""
 ],
 ""QuestionAnswer"": ""<td>B</td>""
 },
 {
 ""QuestionText"": ""<td><span dir=\""rtl\"">العلاقة بين</span> <span class=\""math inline\"">\\(K_{p},\\ K_{c}\\) </span> <span dir=\""rtl\"">للتفاعل السابق:</span></td>"",
 ""QuestionChoices"": [
 ""<p><span dir=\""rtl\"">A-</span> <span class=\""math inline\"">\\(K_{c} = K_{p}\\left( \\text{RT} \\right)^{3}\\)</span></p>"",
 ""<p><span dir=\""rtl\"">B-</span> <span class=\""math inline\"">\\(K_{p} = K_{c}\\left( \\text{RT} \\right)^{3}\\)</span></p>"",
 ""<p><span dir=\""rtl\"">C-</span> <span class=\""math inline\"">\\(K_{p} = K_{c}\\)</span></p>"",
 ""<p><span dir=\""rtl\"">D-</span> <span class=\""math inline\"">\\(K_{p} = K_{c}\\left( \\text{RT} \\right)^{2}\\)</span></p>""
 ],
 ""QuestionAnswer"": ""<td>A</td>""
 },
 {
 ""QuestionText"": ""<td><p><span dir=\""rtl\"">ليكن التفاعل</span></p><p><span class=\""math";
            string expected = @"[
 {
 ""QuestionText"": ""<td><p><span dir=\""rtl\"">اكتب علاقة ثابت التوازن الكيميائي بدلالة التراكيز للتفاعل الآتي:</span></p><p><span class=\""math display\"">\\[\\text{CaC}{O_{3}}_{\\left( s \\right)} \\rightleftharpoons CaO_{\\left( s \\right)} + C{O_{2}}_{\\left( g \\right)}\\]</span></p></td>"",
 ""QuestionChoices"": [
 ""<p><span dir=\""rtl\"">A-</span> <span class=\""math inline\"">\\(K_{c} = \\frac{\\left\\lbrack \\text{CaC}O_{3} \\right\\rbrack}{\\left\\lbrack \\text{CaO} \\right\\rbrack.\\left\\lbrack CO_{2} \\right\\rbrack}\\)</span></p>"",
 ""<p><span dir=\""rtl\"">B-</span> <span class=\""math inline\"">\\(K_{c} = \\lbrack CO_{2}\\rbrack\\)</span></p>"",
 ""<p><span dir=\""rtl\"">C-</span> <span class=\""math inline\"">\\(Kc = \\lbrack CaCO_{3}\\rbrack\\)</span></p>"",
 ""<p><span dir=\""rtl\"">D-</span> <span class=\""math inline\"">\\(K_{c} = \\left\\lbrack CO_{2} \\right\\rbrack.\\lbrack CaCO_{3}\\)</span></p>""
 ],
 ""QuestionAnswer"": ""<td>B</td>""
 },
 {
 ""QuestionText"": ""<td><span dir=\""rtl\"">العلاقة بين</span> <span class=\""math inline\"">\\(K_{p},\\ K_{c}\\) </span> <span dir=\""rtl\"">للتفاعل السابق:</span></td>"",
 ""QuestionChoices"": [
 ""<p><span dir=\""rtl\"">A-</span> <span class=\""math inline\"">\\(K_{c} = K_{p}\\left( \\text{RT} \\right)^{3}\\)</span></p>"",
 ""<p><span dir=\""rtl\"">B-</span> <span class=\""math inline\"">\\(K_{p} = K_{c}\\left( \\text{RT} \\right)^{3}\\)</span></p>"",
 ""<p><span dir=\""rtl\"">C-</span> <span class=\""math inline\"">\\(K_{p} = K_{c}\\)</span></p>"",
 ""<p><span dir=\""rtl\"">D-</span> <span class=\""math inline\"">\\(K_{p} = K_{c}\\left( \\text{RT} \\right)^{2}\\)</span></p>""
 ],
 ""QuestionAnswer"": ""<td>A</td>""
 }, {
 ""QuestionText"": ""<td><p><span dir=\""rtl\"">ليكن التفاعل</span></p><p><span class=\""math""}]";
            malformedJson = malformedJson.Trim();
            expected = expected.Trim();

            //Act
            MemoryStream stream = new(Encoding.UTF8.GetBytes(malformedJson));
            ResponseValidator validator = new();
            var correctedStream =  await validator.Validate(stream);
            StreamReader reader = new(correctedStream,Encoding.UTF8);
            string actual = (await reader.ReadToEndAsync()).Trim();
            var actual_json = JToken.Parse(actual);
            var expected_json = JToken.Parse(expected);

            //Assert
            Assert.That(JToken.DeepEquals(actual_json,expected_json));
        }
    }
}
