﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace AnalyzeText2
{
    // <SnippetDeclareTypes>
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
    // </SnippetDeclareTypes>

    public class Implemented
    {
        // <SnippetDeclareGlobalVariables>
        private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        private static string _trainDataPath => Path.Combine(_appPath, "..", "..", "..", "Data", "issues_train.tsv");
        private static string _testDataPath => Path.Combine(_appPath, "..", "..", "..", "Data", "issues_test.tsv");
        //private static string _modelPath => Path.Combine(_appPath, "..", "..", "..", "Models", "model.zip");
        private static string _modelPath => "model.zip";

        private static MLContext _mlContext;
        private static PredictionEngine<ConversationInput, SentenceClassifiedOutput> _predEngine;
        private static ITransformer _trainedModel;
        static IDataView _trainingDataView;
        // </SnippetDeclareGlobalVariables>
        public static void Execute()
        {
            // Create MLContext to be shared across the model creation workflow objects 
            // Set a random seed for repeatable/deterministic results across multiple trainings.
            // <SnippetCreateMLContext>
            _mlContext = new MLContext(seed: 0);
            // </SnippetCreateMLContext>

            // STEP 1: Common data loading configuration 
            // CreateTextReader<ConversationInput>(hasHeader: true) - Creates a TextLoader by inferencing the dataset schema from the ConversationInput data model type.
            // .Read(_trainDataPath) - Loads the training text file into an IDataView (_trainingDataView) and maps from input columns to IDataView columns.
            //Console.WriteLine($"=============== Loading Dataset  ===============");

            var data = LoadFromXml();

            // <SnippetLoadTrainData>
            _trainingDataView = _mlContext.Data.LoadFromEnumerable(data);
            // </SnippetLoadTrainData>

            //Console.WriteLine($"=============== Finished Loading Dataset  ===============");

            // <SnippetSplitData>
            //   var (trainData, testData) = _mlContext.MulticlassClassification.TrainTestSplit(_trainingDataView, testFraction: 0.1);
            // </SnippetSplitData>

            // <SnippetCallProcessData>
            var pipeline = ProcessData();
            // </SnippetCallProcessData>

            // <SnippetCallBuildAndTrainModel>
            var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);
            // </SnippetCallBuildAndTrainModel>

            // <SnippetCallEvaluate>
            Evaluate(_trainingDataView.Schema);
            // </SnippetCallEvaluate>

            // <SnippetCallPredictIssue>
            PredictIssue();
            // </SnippetCallPredictIssue>
        }

        public static IEstimator<ITransformer> ProcessData()
        {
            //Console.WriteLine($"=============== Processing Data ===============");
            // STEP 2: Common data process configuration with pipeline data transformations
            // <SnippetMapValueToKey>
            var pipeline = _mlContext.Transforms.Conversion
                .MapValueToKey(inputColumnName: "Area", outputColumnName: "Label")
                // </SnippetMapValueToKey>
                // <SnippetFeaturizeText>
                //.Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title", outputColumnName: "TitleFeaturized"))
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Description",
                    outputColumnName: "DescriptionFeaturized"))
                // </SnippetFeaturizeText>
                // <SnippetConcatenate>
                .Append(_mlContext.Transforms.Concatenate("Features", "DescriptionFeaturized"))
                            // </SnippetConcatenate>
                            //Sample Caching the DataView so estimators iterating over the data multiple times, instead of always reading from file, using the cache might get better performance.
                            // <SnippetAppendCache>
                            .AppendCacheCheckpoint(_mlContext);
            // </SnippetAppendCache>

            //Console.WriteLine($"=============== Finished Processing Data ===============");

            // <SnippetReturnPipeline>
            return pipeline;
            // </SnippetReturnPipeline>
        }

        public static IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            // STEP 3: Create the training algorithm/trainer
            // Use the multi-class SDCA algorithm to predict the label using features.
            //Set the trainer/algorithm and map label to value (original readable state)
            // <SnippetAddTrainer> 
            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            // </SnippetAddTrainer> 

            // STEP 4: Train the model fitting to the DataSet
            //Console.WriteLine($"=============== Training the model  ===============");

            // <SnippetTrainModel> 
            _trainedModel = trainingPipeline.Fit(trainingDataView);
            // </SnippetTrainModel> 
            //Console.WriteLine($"=============== Finished Training the model Ending time: {DateTime.Now.ToString()} ===============");

            // (OPTIONAL) Try/test a single prediction with the "just-trained model" (Before saving the model)
            //Console.WriteLine($"=============== Single Prediction just-trained-model ===============");

            // Create prediction engine related to the loaded trained model
            // <SnippetCreatePredictionEngine1>
            _predEngine = _mlContext.Model.CreatePredictionEngine<ConversationInput, SentenceClassifiedOutput>(_trainedModel);
            // </SnippetCreatePredictionEngine1>
            // <SnippetCreateTestIssue1> 
            ConversationInput issue = new ConversationInput()
            {
                //Title = "WebSockets communication is slow in my machine",
                Description = "The WebSockets communication used under the covers by SignalR looks like is going slow in my development machine.."
            };
            // </SnippetCreateTestIssue1>

            // <SnippetPredict>
            var prediction = _predEngine.Predict(issue);
            // </SnippetPredict>

            // <SnippetOutputPrediction>
            //Console.WriteLine($"=============== Single Prediction just-trained-model - Result: {prediction.Area} ===============");
            // </SnippetOutputPrediction>

            // <SnippetReturnModel>
            return trainingPipeline;
            // </SnippetReturnModel>
        }

        public static void Evaluate(DataViewSchema trainingDataViewSchema)
        {
            // STEP 5:  Evaluate the model in order to get the model's accuracy metrics
            //Console.WriteLine($"=============== Evaluating to get model's accuracy metrics - Starting time: {DateTime.Now.ToString()} ===============");

            //Load the test dataset into the IDataView
            // <SnippetLoadTestDataset>
            //var testDataView = _mlContext.Data.LoadFromTextFile<ConversationInput>(_testDataPath, hasHeader: true);
            var data = LoadFromXml();
            var testDataView = _mlContext.Data.LoadFromEnumerable(data);
            // </SnippetLoadTestDataset>

            //Evaluate the model on a test dataset and calculate metrics of the model on the test data.
            // <SnippetEvaluate>
            var testMetrics = _mlContext.MulticlassClassification.Evaluate(_trainedModel.Transform(testDataView));
            // </SnippetEvaluate>

            //Console.WriteLine($"=============== Evaluating to get model's accuracy metrics - Ending time: {DateTime.Now.ToString()} ===============");
            // <SnippetDisplayMetrics>
            //Console.WriteLine($"*************************************************************************************************************");
            //Console.WriteLine($"*       Metrics for Multi-class Classification model - Test Data     ");
            //Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            //Console.WriteLine($"*       MicroAccuracy:    {testMetrics.MicroAccuracy:0.###}");
            //Console.WriteLine($"*       MacroAccuracy:    {testMetrics.MacroAccuracy:0.###}");
            //Console.WriteLine($"*       LogLoss:          {testMetrics.LogLoss:#.###}");
            //Console.WriteLine($"*       LogLossReduction: {testMetrics.LogLossReduction:#.###}");
            //Console.WriteLine($"*************************************************************************************************************");
            // </SnippetDisplayMetrics>

            // Save the new model to .ZIP file
            // <SnippetCallSaveModel>
            SaveModelAsFile(_mlContext, trainingDataViewSchema, _trainedModel);
            // </SnippetCallSaveModel>
        }

        public static void PredictIssue()
        {
            // <SnippetLoadModel>
            ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);
            // </SnippetLoadModel>

            // <SnippetAddTestIssue> 
            ConversationInput singleIssue = new ConversationInput() {  Description = "When connecting to the database, EF is crashing" };
            //ConversationInput singleIssue = new ConversationInput() { Description = "What is your name"};
            // </SnippetAddTestIssue> 

            //Predict label for single hard-coded issue
            // <SnippetCreatePredictionEngine>
            _predEngine = _mlContext.Model.CreatePredictionEngine<ConversationInput, SentenceClassifiedOutput>(loadedModel);
            // </SnippetCreatePredictionEngine>

            // <SnippetPredictIssue>
            var prediction = _predEngine.Predict(singleIssue);
            // </SnippetPredictIssue>

            // <SnippetDisplayResults>
            //Console.WriteLine($"=============== Single Prediction - Result: {prediction.Area} ===============");
            // </SnippetDisplayResults>
        }

        private static List<ConversationInput> LoadFromXml()
        {
            var result = new List<ConversationInput>();
            var document = new XmlDocument();
            document.Load("questions.xml");
            var posts = document.LastChild.LastChild.ChildNodes;
            int i = 0;
            foreach (XmlNode post in posts)
            {
                try
                {
                    i++;
                    var inputRow = new ConversationInput();
                    inputRow.Description = post.FirstChild.Value;
                    inputRow.Area = post.Attributes["class"].Value;
                    //inputRow.Title = post.FirstChild.Value;
                    //inputRow.ClassificationOutput = (uint) (post.Attributes["class"].Value.Contains("Question") ? 1 : 0);
                    result.Add(inputRow);
                }
                catch (Exception)
                {
                }
                if(i == 1000)
                    break;
            }

            return result;
        }

        private static void SaveModelAsFile(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            // <SnippetSaveModel> 
            mlContext.Model.Save(model, trainingDataViewSchema, _modelPath);
            // </SnippetSaveModel>

            //Console.WriteLine("The model is saved to {0}", _modelPath);
        }
    }
}
