using System;
using System.Runtime.InteropServices;
using System.Text;
using HtmlAgilityPack;
using HTMLParser.AI.Models;
using HTMLParser.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Console;

namespace HTMLParser.Console
{
    internal class Program
    {   
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((context,services) => services.AddParser()).Build();
            OutputEncoding = Encoding.UTF8;

            DocumentChunker chunker = host.Services.GetRequiredService<DocumentChunker>();
            IAIModel model = host.Services.GetRequiredService<IAIModel>();
            string prompt = @"Here is my HTML content. I want you to extract questions from it and write a JSON file using the following structure:

[
  {
    ""QuestionText"": ""HTML string of the question"",
    ""QuestionChoices"": [""HTML string of choice A"", ""HTML string of choice B"", ""...""],
    ""QuestionAnswer"": ""HTML string of the answer""
  }
]

Instructions:
- The values of all fields must be raw HTML strings from the input, including any inline formatting like <b>, <i>, <u>, <code>, <sup>, <sub>, etc.
- LaTeX math should be preserved and properly escaped using double backslashes. Inline math must be in \\\\( ... \\\\), block math in \\\\[ ... \\\\].
- Strings must be fully escaped for JSON (e.g., double quotes, backslashes).
- If you encounter incomplete or ambiguous questions, leave the corresponding field(s) blank.
- The response **must** be only the JSON file — no explanations or comments.
- The JSON must be syntactically valid, fully closed, and well-formatted, so it can be parsed directly.

The HTML may be truncated, so partial questions are acceptable. Your reasoning is needed to identify and extract what qualifies as a question and its parts. Focus on structure and correctness.
";
            StreamWriter writer = new("test.txt");
            /*
                        foreach (var chunk in chunker.Chunk(new StreamReader(@"E:\Chemistry\Bank\output.html")))
                        {
                            await foreach (var item in model.GetStreamingResponseAsync($"{prompt} \n {chunk.Content}"))
                            {
                                writer.Write(item);
                                Write(item);
                            }
                            writer.Write("-------------------------------");

                        }*/
            string answer = @"[
  {
    ""QuestionText"": ""كل مما يلي يحدث عند التوازن ما عدا"",
    ""QuestionChoices"": [
      ""A- تثبت تراكيز المواد المتفاعلة"",
      ""B- تتساوي سرعة التفاعل المباشر و العكسي"",
      ""C- تتساوي تراكيز المواد المتفاعلة و المواد الناتجة"",
      ""D- يثبت لون المحلول""
    ],
    ""QuestionAnswer"": ""C""
  },
  {
    ""QuestionText"": ""التوازن الكيميائي"",
    ""QuestionChoices"": [
      ""A- حركي"",
      ""B- ساكن"",
      ""C- غير ثابت"",
      ""D- ليس أيا مما سبق""
    ],
    ""QuestionAnswer"": ""A""
  },
  {
    ""QuestionText"": ""اكتب علاقة ثابت التوازن الكيميائي بدلالة التراكيز للتفاعل الآتي:\n\\[{H_{2}}_{(g)} + {I_{2}}_{(g)} \\rightleftharpoons 2HI_{(g)}\\]"",
    ""QuestionChoices"": [
      ""A- \\(K_c = \\frac{[HI]^2}{[H_2][I_2]}\\)"",
      ""B- \\(K_c = \\frac{[H_2][I_2]}{[HI]^2}\\)"",
      ""C- \\(K_c = \\frac{[H_2][I_2]}{[HI]}\\)"",
      ""D- \\(K_c = \\frac{[HI]}{[H_2][I_2]}\\)""
    ],
    ""QuestionAnswer"": ""A""
  },
  {
    ""QuestionText"": ""اكتب علاقة ثابت التوازن الكيميائي بدلالة التراكيز للتفاعل الآتي:\n\\[CaCO_3(s) \\rightleftharpoons CaO(s) + CO_2(g)\\]"",
    ""QuestionChoices"": [
      ""A- \\(K_c = \\frac{[CaCO_3]}{[CaO][CO_2]}\\)"",
      ""B- \\(K_c = [CO_2]\\)"",
      ""C- \\(K_c = [CaCO_3]\\)"",
      ""D- \\(K_c = [CO_2][CaCO_3]\\)""
    ],
    ""QuestionAnswer"": ""B""
  },
  {
    ""QuestionText"": ""علاقة Kp للتفاعل السابق:"",
    ""QuestionChoices"": [
      ""A- \\(K_p = K_c\\)"",
      ""B- \\(K_p = K_c(RT)\\)"",
      ""C- \\(K_p = \\frac{K_c}{RT}\\)"",
      ""D- \\(K_c = K_p(RT)\\)""
    ],
    ""QuestionAnswer"": ""B""
  },
  {
    ""QuestionText"": ""ليكن التفاعل:\n\\[WO_3(s) + 3H_2(g) \\rightleftharpoons W(s) + 3H_2O(l)\\]\nفإن علاقة ثابت التوازن بدلالة التراكيز هي:"",
    ""QuestionChoices"": [
      ""A- \\(K_c = 1\\)"",
      ""B- \\(K_c = [H_2]^{-3}\\)"",
      ""C- \\(K_c = [H_2]^3\\)"",
      ""D- \\(K_c = \\frac{[W]}{[WO_3][H_2]^3}\\)""
    ],
    ""QuestionAnswer"": ""B""
  },
  {
    ""QuestionText"": ""العلاقة بين \\(K_p, K_c\\) للتفاعل السابق:"",
    ""QuestionChoices"": [
      ""A- \\(K_c = K_p(RT)^3\\)"",
      ""B- \\(K_p = K_c(RT)^3\\)"",
      ""C- غير مكتمل"",
      ""D- غير متوفر""
    ],
    ""QuestionAnswer"": ""B""
  },
  {
    ""QuestionText"": ""إذا علمت أن \\(K_c = 50.5\\) عند درجة حرارة 400K وتم حساب حاصل التفاعل Q = 8، أي مما يلي صحيح:"",
    ""QuestionChoices"": [
      ""A- التفاعل يرجح بالاتجاه المباشر"",
      ""B- التفاعل يرجح بالاتجاه العكسي"",
      ""C- متوازن"",
      ""D- ليس أيا مما سبق""
    ],
    ""QuestionAnswer"": ""A""
  },
  {
    ""QuestionText"": ""كل مما يلي من العوامل المؤثرة في حالة التوازن ما عدا:"",
    ""QuestionChoices"": [
      ""A- درجة الحرارة"",
      ""B- تغير التراكيز"",
      ""C- الضغط"",
      ""D- حفاز""
    ],
    ""QuestionAnswer"": ""D""
  },
  {
    ""QuestionText"": ""ليكن التفاعل:\n\\[PCl_5 \\rightleftharpoons PCl_3 + Cl_2 \\quad \\Delta H < 0\\]\nإذا اختل التوازن و أصبح راجحاً بالاتجاه المباشر، أي من التغيرات ممكنة:"",
    ""QuestionChoices"": [
      ""A- زيادة كمية \\(PCl_5\\)"",
      ""B- زيادة الضغط"",
      ""C- زيادة كمية \\(Cl_2\\)"",
      ""D- زيادة درجة الحرارة""
    ],
    ""QuestionAnswer"": ""A""
  },
  {
    ""QuestionText"": ""نقصان كمية \\(Cl_2\\) للتفاعل تؤدي إلى:"",
    ""QuestionChoices"": [
      ""A- رجحان التفاعل المباشر"",
      ""B- رجحان التفاعل العكسي"",
      ""C- ليس أيّا مما سبق""
    ],
    ""QuestionAnswer"": ""A""
  },
  {
    ""QuestionText"": ""ليكن التفاعل المتوازن:\n\\[N_2O_4(g) \\rightleftharpoons 2NO_2(g)\\]\nزيادة ضغط هذا التفاعل تؤدي إلى:"",
    ""QuestionChoices"": [
      ""A- زيادة تركيز \\(N_2O_4\\)"",
      ""B- زيادة تركيز \\(NO_2\\)"",
      ""C- لا تؤثر"",
      ""D- رجحان التفاعل بالاتجاه المباشر""
    ],
    ""QuestionAnswer"": ""A""
  },
  {
    ""QuestionText"": ""ليكن التفاعل المتوازن:\n\\(H_2 + I_2 \\rightleftharpoons 2HI\\)\nزيادة الضغط تؤدي إلى:"",
    ""QuestionChoices"": [
      ""A- رجحان التفاعل المباشر"",
      ""B- رجحان التفاعل العكسي"",
      ""C- لا تؤثر""
    ],
    ""QuestionAnswer"": ""C""
  },
  {
    ""QuestionText"": ""ليكن التفاعل:\n\\[CaCO_3(s) \\rightleftharpoons CaO(g) + CO_2(g)\\]\nتم إنقاص حجم الوعاء للتفاعل السابق:"",
    ""QuestionChoices"": [
      ""A- رجحان التفاعل العكسي"",
      ""B- رجحان التفاعل المباشر"",
      ""C- لا يؤثر على حالة التوازن"",
      ""D- تزداد Kp""
    ],
    ""QuestionAnswer"": ""A""
  },
  {
    ""QuestionText"": ""ليكن التفاعل:\n\\[2SO_2(g) + O_2(g) \\rightleftharpoons 2SO_3(g)\\]\nإذا علمت أن زيادة درجة الحرارة تقلل من مردود \\(SO_3\\)، فإن:"",
    ""QuestionChoices"": [
      ""A- \\(\\Delta H < 0\\)"",
      ""B- \\(\\Delta H > 0\\)"",
      ""C- \\(\\Delta H = 0\\)"",
      ""D- لا يمكن تحديد ذلك""
    ],
    ""QuestionAnswer"": ""A""
  }
]
";
            ResponseParser parser = new();
            await foreach (var item in parser.Parse(answer))
            {
                if (item.QuestionAnswer is not null)
                {
                    Write(item.QuestionAnswer);
                }
            }
        }
    }
}
