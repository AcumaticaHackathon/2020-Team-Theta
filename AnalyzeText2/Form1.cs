using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace AnalyzeText2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private void btnTrain_Click(object sender, EventArgs e)
        {
            Implemented.Execute();
        }

        private void btnTestSentence_Click(object sender, EventArgs e)
        {
            var _mlContext = new MLContext(seed: 0);
            ITransformer _trainedModel = _mlContext.Model.Load("model.zip", out DataViewSchema inputSchema);
            PredictionEngine<ConversationInput, SentenceClassifiedOutput> _predEngine = _mlContext.Model.CreatePredictionEngine<ConversationInput, SentenceClassifiedOutput>(_trainedModel);
            
            ConversationInput issue = new ConversationInput()
            {
                Description = txtInput.Text
            };
            var prediction = _predEngine.Predict(issue);

            txtOutPut.Text = prediction.Area;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long milliseconds = 1579190400000;
            var newDt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(milliseconds);

        }
    }

    public class SentencePrediction
    {
        [ColumnName("ClassificationOut")]
        public string ClassificationOut;
    }

    internal class SentencesDataModel
    {
        [LoadColumn(fieldIndex: 0), ColumnName("ClassificationInput")]
        public string ClassificationInput;
        [LoadColumn(fieldIndex: 1), ColumnName("ClassificationOutput")]
        public string ClassificationOutput;

    }

}
