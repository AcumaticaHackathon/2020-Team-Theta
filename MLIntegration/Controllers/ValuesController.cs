using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace MLIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            var _mlContext = new MLContext(seed: 0);
            ITransformer _trainedModel = _mlContext.Model.Load("model.zip", out DataViewSchema inputSchema);
            PredictionEngine<ConversationInput, SentenceClassifiedOutput> _predEngine = _mlContext.Model.CreatePredictionEngine<ConversationInput, SentenceClassifiedOutput>(_trainedModel);

            ConversationInput issue = new ConversationInput()
            {
                Description = id
            };
            var prediction = _predEngine.Predict(issue);

            if (prediction.Area.Contains("Question"))
                return "true";
            return "false";
        }


        

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class ConversationInput
    {

        [LoadColumn(0)]
        public string Area { get; set; }

        [LoadColumn(1)]
        public string Description { get; set; }
    }

    public class SentenceClassifiedOutput
    {
        [ColumnName("PredictedLabel")]
        public string Area;
    }
}
