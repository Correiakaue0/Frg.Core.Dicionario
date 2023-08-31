using Domain.DTO;
using Domain.Services;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.Design;
using System.Resources;

namespace Frg.Core.Dicionario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWords()
        {
            try
            {
                ResXResourceReader resourceReaderPortuguese = new ResXResourceReader(ConfigSystem.FilePathPortuguese);
                ResXResourceReader resourceReaderEnglish = new ResXResourceReader(ConfigSystem.FilePathEnglish);
                ResXResourceReader resourceReaderSpanish = new ResXResourceReader(ConfigSystem.FilePathSpanish);

                var wordsList = new List<WordsDTO>();

                resourceReaderPortuguese.UseResXDataNodes = true;
                IDictionaryEnumerator enumeratorPortuguese = resourceReaderPortuguese.GetEnumerator();

                resourceReaderEnglish.UseResXDataNodes = true;
                IDictionaryEnumerator enumeratorEnglish = resourceReaderEnglish.GetEnumerator();

                resourceReaderSpanish.UseResXDataNodes = true;
                IDictionaryEnumerator enumeratorSpanish = resourceReaderSpanish.GetEnumerator();

                while (enumeratorPortuguese.MoveNext())
                {
                    string name = enumeratorPortuguese.Key.ToString();
                    string value = (enumeratorPortuguese.Value as ResXDataNode)?.GetValue((ITypeResolutionService)null).ToString();

                    var word = new WordsDTO(0, name, value);
                    wordsList.Add(word);
                }

                while (enumeratorEnglish.MoveNext())
                {
                    string name = enumeratorEnglish.Key.ToString();
                    string value = (enumeratorEnglish.Value as ResXDataNode)?.GetValue((ITypeResolutionService)null).ToString();

                    var word = new WordsDTO(1, name, value);
                    wordsList.Add(word);
                }

                while (enumeratorSpanish.MoveNext())
                {
                    string name = enumeratorSpanish.Key.ToString();
                    string value = (enumeratorSpanish.Value as ResXDataNode)?.GetValue((ITypeResolutionService)null).ToString();

                    var word = new WordsDTO(2, name, value);
                    wordsList.Add(word);
                }

                var response = wordsList
                   .GroupBy(x => x.Name)
                   .Select(x => new WordsViewModel
                   {
                       Name = x.Key,
                       Portuguese = x.Where(x => x.Language == 0).Select(x => x.Value).FirstOrDefault(),
                       English = x.Where(x => x.Language == 1).Select(x => x.Value).FirstOrDefault(),
                       Spanish = x.Where(x => x.Language == 2).Select(x => x.Value).FirstOrDefault()
                   })
                   .ToList();


                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Insert([FromBody] WordsViewModel word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word.Portuguese))
                    InsertWord(ConfigSystem.FilePathPortuguese, word.Name, word.Portuguese);

                if (!string.IsNullOrEmpty(word.English))
                    InsertWord(ConfigSystem.FilePathEnglish, word.Name, word.English);

                if (!string.IsNullOrEmpty(word.Spanish))
                    InsertWord(ConfigSystem.FilePathSpanish, word.Name, word.Spanish);

                return Ok(word);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] WordsViewModel word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word.Portuguese))
                    UpdateWord(word.Name, word.Portuguese, ConfigSystem.FilePathPortuguese);

                if (!string.IsNullOrEmpty(word.English))
                    UpdateWord(word.Name, word.English, ConfigSystem.FilePathEnglish);

                if (!string.IsNullOrEmpty(word.Spanish))
                    UpdateWord(word.Name, word.Spanish, ConfigSystem.FilePathSpanish);

                return Ok(word);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{nome}")]
        public IActionResult Delete(string nome)
        {
            try
            {
                DeleteWord(nome, ConfigSystem.FilePathPortuguese);
                DeleteWord(nome, ConfigSystem.FilePathEnglish);
                DeleteWord(nome, ConfigSystem.FilePathSpanish);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("Batch")]
        public IActionResult Batch(BatchViewModel receive)
        {
            try
            {
                var listWords = receive.Receive.Split("/n");

                foreach (var words in listWords)
                {
                    if (words.Count() != 1)
                    {
                        var word = words.Split("|");
                        if (!string.IsNullOrEmpty(word[0]))
                        { 
                            var newWord = new WordsViewModel();
                            newWord.Name = word[0].Replace("/n", "").Trim();
                            newWord.Portuguese = word[1];
                            newWord.English = word[2];
                            newWord.Spanish = word[3];

                            Insert(newWord);
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public static void InsertWord(string filePath, string newName, string newValue)
        {
            var wordsList = new List<WordsDTO>();

            using (ResXResourceReader resourceReader = new ResXResourceReader(filePath))
            {
                resourceReader.UseResXDataNodes = true;
                IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    string name = enumerator.Key.ToString();
                    string value = (enumerator.Value as ResXDataNode)?.GetValue((ITypeResolutionService)null).ToString();

                    if (newName.Trim() == name)
                        throw new Exception("'Name' ja utilizado!");

                    var word = new WordsDTO(1, name, value);
                    wordsList.Add(word);
                }

                var newWord = new WordsDTO(1, newName.Trim(), newValue);
                wordsList.Add(newWord);

                using (ResXResourceWriter resourceWriter = new ResXResourceWriter(filePath))
                    foreach (var wordObj in wordsList)
                        resourceWriter.AddResource(wordObj.Name, wordObj.Value);
            }
        }

        public static void DeleteWord(string chave, string filePath)
        {
            var wordsList = new List<WordsDTO>();

            using (ResXResourceReader resourceReader = new ResXResourceReader(filePath))
            {
                resourceReader.UseResXDataNodes = true;
                IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    string name = enumerator.Key.ToString();
                    string value = (enumerator.Value as ResXDataNode)?.GetValue((ITypeResolutionService)null).ToString();
                    if (chave != name)
                    {
                        var word = new WordsDTO(1, name, value);
                        wordsList.Add(word);
                    }
                }

                using (ResXResourceWriter resourceWriter = new ResXResourceWriter(filePath))
                    foreach (var wordObj in wordsList)
                        resourceWriter.AddResource(wordObj.Name, wordObj.Value);
            }
        }

        public static void UpdateWord(string chave, string newValue, string filePath)
        {
            var wordsList = new List<WordsDTO>();

            using (ResXResourceReader resourceReader = new ResXResourceReader(filePath))
            {
                resourceReader.UseResXDataNodes = true;
                IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    string name = enumerator.Key.ToString();
                    string value = (enumerator.Value as ResXDataNode)?.GetValue((ITypeResolutionService)null).ToString();
                    if (chave == name)
                    {
                        value = newValue;
                    }

                    var word = new WordsDTO(1, name, value);
                    wordsList.Add(word);
                }

                using (ResXResourceWriter resourceWriter = new ResXResourceWriter(filePath))
                    foreach (var wordObj in wordsList)
                        resourceWriter.AddResource(wordObj.Name, wordObj.Value);
            }
        }
    }
}
