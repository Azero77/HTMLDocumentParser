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

        [Test]
        public async Task Validate_HandlesIncompleteJsonAndEscapedBackslashes()
        {
            // Arrange
            string malformedJson = @"[
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل</span></p><p><span class=\""math display\"">\\[2NO_{2} \\rightleftharpoons N_{2}O_{4}\\]</span></p><p><span dir=\""rtl\"">فإن علاقة بين ثابت التوازن بدلالة التراكيز و بدلالة الضغوط الجزئية هو:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A-</span> <span class=\""math inline\"">\\(K_{c} = K_{p}\\)</span>"",
            ""<span dir=\""rtl\"">B-</span> <span class=\""math inline\"">\\(K_{c} = K_{p}\\left( \\text{RT} \\right)\\)</span>"",
            ""<span dir=\""rtl\"">C-</span> <span class=\""math inline\"">\\(K_{c} = \\frac{K_{p}}{\\text{RT}}\\)</span>"",
            ""<span dir=\""rtl\"">D-</span> <span class=\""math inline\"">\\(K_{p} = K_{c}\\left( \\text{RT} \\right)\\)</span>""
        ],
        ""QuestionAnswer"": """"
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل:</span></p><p><span class=\""math display\"">\\[{I_{2}}_{\\left( g \\right)} + {H_{2}}_{\\left( g \\right)} \\rightleftharpoons2HI_{\\left( g \\right)}\\]</span></p><p><span dir=\""rtl\"">فإذا علمت أن</span> <span class=\""math inline\"">\\(K_{c} =50.5\\)</span> <span dir=\""rtl\"">في درجة حرارة</span>400K <span dir=\""rtl\"">و تم حساب حاصل التفاعل في لحظة و كان</span></p><p>Q =8 <span dir=\""rtl\"">، أي مما يلي صحيح:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- التفاعل يرجح بالاتجاه المباشر</span>"",
            ""<span dir=\""rtl\"">B- التفاعل يرجح بالاتجاه العكسي</span>"",
            ""<span dir=\""rtl\"">C- متوازن</span>"",
            ""<span dir=\""rtl\"">D- ليس أيا مما سبق</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<span dir=\""rtl\"">كل مما يلي من العوامل المؤثرة في حالة التوازن ما عدا:</span>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- درجة الحرارة</span>"",
            ""<span dir=\""rtl\"">B- تغير التراكيز</span>"",
            ""<span dir=\""rtl\"">C- الضغط</span>"",
            ""<span dir=\""rtl\"">D- حفاز</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">D</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل:</span></p><p><span class=\""math display\"">\\[\\text{PC}l_{5} \\rightleftharpoons PCl_{3} + Cl_{2}\\text{\\ \ \ \ }\\mathrm{\\Delta}\\mathrm{\\Delta}H &lt;0\\]</span></p><p><span dir=\""rtl\"">إذا علمت أن التفاعل قد اختلّ توازنه و أصبح راجحاً بالاتجاه المباشر، أي من التغيرات ممكنة:</span><span class=\""math inline\"">\\(\\ \\)</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- زيادة كمية</span> <span class=\""math inline\"">\\(\\text{PC}l_{5}\\)</span>"",
            ""<span dir=\""rtl\"">B- زيادة الضغط</span>"",
            ""<span dir=\""rtl\"">C- زيادة كمية</span> <span class=\""math inline\"">\\(Cl_{2}\\)</span>"",
            ""<span dir=\""rtl\"">D- زيادة درجة الحرارة</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<span dir=\""rtl\"">نقصان كمية</span> <span class=\""math inline\"">\\(Cl_{2}\\)</span> <span dir=\""rtl\"">للتفاعل تؤدي:</span>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- رجحان التفاعل المباشر</span>"",
            ""<span dir=\""rtl\"">B- رجحان التفاعل العكسي</span>"",
            ""<span dir=\""rtl\"">C- ليس أيّا مما سبق</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل المتوازن :</span></p><p><span class=\""math display\"">\\[N_{2}{O_{4}}_{\\left( g \\right)} \\rightleftharpoons2N{O_{2}}_{\\left( g \\right)}\\]</span></p><p><span dir=\""rtl\"">زيادة ضغط هذا التفاعل تؤدي:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- زيادة تركيز</span> <span class=\""math inline\"">\\(N_{2}{O_{4}}_{\\left( g \\right)}\\)</span>"",
            ""<span dir=\""rtl\"">B- زيادة تركيز</span> <span class=\""math inline\"">\\(2N{O_{2}}_{\\left( g \\right)}\\)</span>"",
            ""<span dir=\""rtl\"">C- لا تؤثر</span>"",
            ""<span dir=\""rtl\"">D- رجحان التفاعل بالاتجاه المباشر</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل المتوازن:</span></p><p><span class=\""math inline\"">\\(H_{2} + I_{2} \\rightleftharpoons2HI\\)</span></p><p><span dir=\""rtl\"">زيادة ضغط تؤدي إلى:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- رجحان التفاعل المباشر</span>"",
            ""<span dir=\""rtl\"">B- رجحان التفاعل العكسي</span>"",
            ""<span dir=\""rtl\"">C- لا تؤثر</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">C</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل</span></p><p><span class=\""math display\"">\\[\\text{CaC}{O_{3}}_{\\left( s \\right)} \\rightleftharpoons CaO_{\\left( g \\right)} + C{O_{2}}_{\\left( g \\right)}\\]</span></p><p><span dir=\""rtl\"">تم إنقاص حجم الوعاء للتفاعل السابق:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- رجحان التفاعل العكوس</span>"",
            ""<span dir=\""rtl\"">B- رجحان التفاعل المباشر</span>"",
            ""<span dir=\""rtl\"">C- لا يؤثر على حالة التوازن</span>"",
            ""<span dir=\""rtl\"">D- تزداد</span> Kp""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل:</span></p><p><span class=\""math display\"">\\[2S{O_{2}}_{\\left( g \\right)} + {O_{2}}_{\\left( g \\right)} \\rightleftharpoons2S{O_{3}}_{\\left( g \\right)}\\]</span></p><p><span dir=\""rtl\"">إذا علمت أن زيادة درجة الحرارة تقلل من مردود ال</span><span class=\""math inline\"">\\(S{O_{3}}_{\\left( g \\right)}\\)</span> <span dir=\""rtl\"">فإن:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A-</span> <span class=\""math inline\"">\\(\\mathrm{\\Delta}H &lt;0\\)</span>"",
            ""<span dir=\""rtl\"">B-</span> <span class=\""math inline\"">\\(\\mathrm{\\Delta}H &gt;0\\)</span>"",
            ""<span dir=\""rtl\"">C-</span> <span class=\""math inline\"">\\(\\mathrm{\\Delta}H =0\\)</span>"",
            ""<span dir=\""rtl\"">D- لا يمكن تحديد ذلك</span>""
        ],
        ""QuestionAnswer"":";

            string expected = @"[
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل</span></p><p><span class=\""math display\"">\\[2NO_{2} \\rightleftharpoons N_{2}O_{4}\\]</span></p><p><span dir=\""rtl\"">فإن علاقة بين ثابت التوازن بدلالة التراكيز و بدلالة الضغوط الجزئية هو:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A-</span> <span class=\""math inline\"">\\(K_{c} = K_{p}\\)</span>"",
            ""<span dir=\""rtl\"">B-</span> <span class=\""math inline\"">\\(K_{c} = K_{p}\\left( \\text{RT} \\right)\\)</span>"",
            ""<span dir=\""rtl\"">C-</span> <span class=\""math inline\"">\\(K_{c} = \\frac{K_{p}}{\\text{RT}}\\)</span>"",
            ""<span dir=\""rtl\"">D-</span> <span class=\""math inline\"">\\(K_{p} = K_{c}\\left( \\text{RT} \\right)\\)</span>""
        ],
        ""QuestionAnswer"": """"
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل:</span></p><p><span class=\""math display\"">\\[{I_{2}}_{\\left( g \\right)} + {H_{2}}_{\\left( g \\right)} \\rightleftharpoons2HI_{\\left( g \\right)}\\]</span></p><p><span dir=\""rtl\"">فإذا علمت أن</span> <span class=\""math inline\"">\\(K_{c} =50.5\\)</span> <span dir=\""rtl\"">في درجة حرارة</span>400K <span dir=\""rtl\"">و تم حساب حاصل التفاعل في لحظة و كان</span></p><p>Q =8 <span dir=\""rtl\"">، أي مما يلي صحيح:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- التفاعل يرجح بالاتجاه المباشر</span>"",
            ""<span dir=\""rtl\"">B- التفاعل يرجح بالاتجاه العكسي</span>"",
            ""<span dir=\""rtl\"">C- متوازن</span>"",
            ""<span dir=\""rtl\"">D- ليس أيا مما سبق</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<span dir=\""rtl\"">كل مما يلي من العوامل المؤثرة في حالة التوازن ما عدا:</span>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- درجة الحرارة</span>"",
            ""<span dir=\""rtl\"">B- تغير التراكيز</span>"",
            ""<span dir=\""rtl\"">C- الضغط</span>"",
            ""<span dir=\""rtl\"">D- حفاز</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">D</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل:</span></p><p><span class=\""math display\"">\\[\\text{PC}l_{5} \\rightleftharpoons PCl_{3} + Cl_{2}\\text{\\ \\ \\ \\ }\\mathrm{\\Delta}\\mathrm{\\Delta}H &lt;0\\]</span></p><p><span dir=\""rtl\"">إذا علمت أن التفاعل قد اختلّ توازنه و أصبح راجحاً بالاتجاه المباشر، أي من التغيرات ممكنة:</span><span class=\""math inline\"">\\(\\ \\)</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- زيادة كمية</span> <span class=\""math inline\"">\\(\\text{PC}l_{5}\\)</span>"",
            ""<span dir=\""rtl\"">B- زيادة الضغط</span>"",
            ""<span dir=\""rtl\"">C- زيادة كمية</span> <span class=\""math inline\"">\\(Cl_{2}\\)</span>"",
            ""<span dir=\""rtl\"">D- زيادة درجة الحرارة</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<span dir=\""rtl\"">نقصان كمية</span> <span class=\""math inline\"">\\(Cl_{2}\\)</span> <span dir=\""rtl\"">للتفاعل تؤدي:</span>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- رجحان التفاعل المباشر</span>"",
            ""<span dir=\""rtl\"">B- رجحان التفاعل العكسي</span>"",
            ""<span dir=\""rtl\"">C- ليس أيّا مما سبق</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل المتوازن :</span></p><p><span class=\""math display\"">\\[N_{2}{O_{4}}_{\\left( g \\right)} \\rightleftharpoons2N{O_{2}}_{\\left( g \\right)}\\]</span></p><p><span dir=\""rtl\"">زيادة ضغط هذا التفاعل تؤدي:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- زيادة تركيز</span> <span class=\""math inline\"">\\(N_{2}{O_{4}}_{\\left( g \\right)}\\)</span>"",
            ""<span dir=\""rtl\"">B- زيادة تركيز</span> <span class=\""math inline\"">\\(2N{O_{2}}_{\\left( g \\right)}\\)</span>"",
            ""<span dir=\""rtl\"">C- لا تؤثر</span>"",
            ""<span dir=\""rtl\"">D- رجحان التفاعل بالاتجاه المباشر</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل المتوازن:</span></p><p><span class=\""math inline\"">\\(H_{2} + I_{2} \\rightleftharpoons2HI\\)</span></p><p><span dir=\""rtl\"">زيادة ضغط تؤدي إلى:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- رجحان التفاعل المباشر</span>"",
            ""<span dir=\""rtl\"">B- رجحان التفاعل العكسي</span>"",
            ""<span dir=\""rtl\"">C- لا تؤثر</span>""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">C</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل</span></p><p><span class=\""math display\"">\\[\\text{CaC}{O_{3}}_{\\left( s \\right)} \\rightleftharpoons CaO_{\\left( g \\right)} + C{O_{2}}_{\\left( g \\right)}\\]</span></p><p><span dir=\""rtl\"">تم إنقاص حجم الوعاء للتفاعل السابق:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A- رجحان التفاعل العكوس</span>"",
            ""<span dir=\""rtl\"">B- رجحان التفاعل المباشر</span>"",
            ""<span dir=\""rtl\"">C- لا يؤثر على حالة التوازن</span>"",
            ""<span dir=\""rtl\"">D- تزداد</span> Kp""
        ],
        ""QuestionAnswer"": ""<span dir=\""rtl\"">A</span>""
    },
    {
        ""QuestionText"": ""<p><span dir=\""rtl\"">ليكن التفاعل:</span></p><p><span class=\""math display\"">\\[2S{O_{2}}_{\\left( g \\right)} + {O_{2}}_{\\left( g \\right)} \\rightleftharpoons2S{O_{3}}_{\\left( g \\right)}\\]</span></p><p><span dir=\""rtl\"">إذا علمت أن زيادة درجة الحرارة تقلل من مردود ال</span><span class=\""math inline\"">\\(S{O_{3}}_{\\left( g \\right)}\\)</span> <span dir=\""rtl\"">فإن:</span></p>"",
        ""QuestionChoices"": [
            ""<span dir=\""rtl\"">A-</span> <span class=\""math inline\"">\\(\\mathrm{\\Delta}H &lt;0\\)</span>"",
            ""<span dir=\""rtl\"">B-</span> <span class=\""math inline\"">\\(\\mathrm{\\Delta}H &gt;0\\)</span>"",
            ""<span dir=\""rtl\"">C-</span> <span class=\""math inline\"">\\(\\mathrm{\\Delta}H =0\\)</span>"",
            ""<span dir=\""rtl\"">D- لا يمكن تحديد ذلك</span>""
        ],
        ""QuestionAnswer"":null}]";

            malformedJson = malformedJson.Trim();
            expected = expected.Trim();

            // Act
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(malformedJson));
            ResponseValidator validator = new ResponseValidator();
            var correctedStream = await validator.Validate(stream);
            StreamReader reader = new StreamReader(correctedStream, Encoding.UTF8);
            string actual = (await reader.ReadToEndAsync()).Trim();
            var actualJson = JToken.Parse(actual);
            var expectedJson = JToken.Parse(expected);

            // Assert
            Assert.That(JToken.DeepEquals(actualJson, expectedJson));
        }
    }
}
