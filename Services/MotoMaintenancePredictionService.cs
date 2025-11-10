using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;

namespace MottoMap.Services
{
    /// <summary>
    /// Dados de entrada para previsão de manutenção
    /// </summary>
    public class MotoMaintenanceData
  {
        [LoadColumn(0)]
        public float Quilometragem { get; set; }

        [LoadColumn(1)]
    public float Ano { get; set; }

   [LoadColumn(2)]
    public float IdadeMoto { get; set; }

        [LoadColumn(3)]
        public bool NecessitaManutencao { get; set; }
    }

    /// <summary>
    /// Previsão de manutenção
    /// </summary>
    public class MaintenancePrediction
    {
  [ColumnName("PredictedLabel")]
        public bool NecessitaManutencao { get; set; }

        [ColumnName("Probability")]
        public float Probabilidade { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }

    /// <summary>
    /// Resultado da previsão com informações adicionais
    /// </summary>
    public class MaintenancePredictionResult
    {
     public bool NecessitaManutencao { get; set; }
    public float ProbabilidadeManutencao { get; set; }
        public string Recomendacao { get; set; } = string.Empty;
        public string Prioridade { get; set; } = string.Empty;
        public int KmProximaRevisao { get; set; }
    }

    /// <summary>
    /// Serviço de Machine Learning para previsão de manutenção de motos
    /// </summary>
    public class MotoMaintenancePredictionService
    {
  private readonly MLContext _mlContext;
        private ITransformer? _model;
   private readonly string _modelPath = "moto_maintenance_model.zip";

        public MotoMaintenancePredictionService()
        {
            _mlContext = new MLContext(seed: 0);
    TrainModel();
        }

        /// <summary>
        /// Treina o modelo de ML com dados sintéticos
        /// </summary>
        private void TrainModel()
        {
// Criar dados de treinamento sintéticos
   var trainingData = new List<MotoMaintenanceData>
            {
    // Motos que NÃO precisam manutenção (novas, baixa km)
            new() { Quilometragem = 1000, Ano = 2024, IdadeMoto = 0, NecessitaManutencao = false },
                new() { Quilometragem = 3000, Ano = 2024, IdadeMoto = 0, NecessitaManutencao = false },
new() { Quilometragem = 5000, Ano = 2023, IdadeMoto = 1, NecessitaManutencao = false },
 new() { Quilometragem = 8000, Ano = 2023, IdadeMoto = 1, NecessitaManutencao = false },
  new() { Quilometragem = 2000, Ano = 2024, IdadeMoto = 0, NecessitaManutencao = false },
     new() { Quilometragem = 4000, Ano = 2023, IdadeMoto = 1, NecessitaManutencao = false },
      new() { Quilometragem = 6000, Ano = 2023, IdadeMoto = 1, NecessitaManutencao = false },
           new() { Quilometragem = 7000, Ano = 2022, IdadeMoto = 2, NecessitaManutencao = false },
       
             // Motos que PRECISAM manutenção (alta km ou antigas)
                new() { Quilometragem = 15000, Ano = 2022, IdadeMoto = 2, NecessitaManutencao = true },
              new() { Quilometragem = 20000, Ano = 2021, IdadeMoto = 3, NecessitaManutencao = true },
          new() { Quilometragem = 25000, Ano = 2020, IdadeMoto = 4, NecessitaManutencao = true },
         new() { Quilometragem = 30000, Ano = 2019, IdadeMoto = 5, NecessitaManutencao = true },
     new() { Quilometragem = 35000, Ano = 2018, IdadeMoto = 6, NecessitaManutencao = true },
       new() { Quilometragem = 18000, Ano = 2021, IdadeMoto = 3, NecessitaManutencao = true },
    new() { Quilometragem = 22000, Ano = 2020, IdadeMoto = 4, NecessitaManutencao = true },
         new() { Quilometragem = 28000, Ano = 2019, IdadeMoto = 5, NecessitaManutencao = true },
        new() { Quilometragem = 12000, Ano = 2022, IdadeMoto = 2, NecessitaManutencao = true },
   new() { Quilometragem = 40000, Ano = 2018, IdadeMoto = 6, NecessitaManutencao = true },

  // Casos intermediários
          new() { Quilometragem = 10000, Ano = 2023, IdadeMoto = 1, NecessitaManutencao = false },
          new() { Quilometragem = 11000, Ano = 2022, IdadeMoto = 2, NecessitaManutencao = true },
     new() { Quilometragem = 9000, Ano = 2023, IdadeMoto = 1, NecessitaManutencao = false },
   new() { Quilometragem = 13000, Ano = 2021, IdadeMoto = 3, NecessitaManutencao = true },
            };

            IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Pipeline de treinamento
            var pipeline = _mlContext.Transforms.Concatenate("Features", 
        nameof(MotoMaintenanceData.Quilometragem),
       nameof(MotoMaintenanceData.Ano),
  nameof(MotoMaintenanceData.IdadeMoto))
              .Append(_mlContext.BinaryClassification.Trainers.FastTree(
         labelColumnName: nameof(MotoMaintenanceData.NecessitaManutencao),
         featureColumnName: "Features",
  numberOfLeaves: 20,
         numberOfTrees: 100,
            minimumExampleCountPerLeaf: 10,
          learningRate: 0.2));

 // Treinar o modelo
            _model = pipeline.Fit(dataView);

        // Salvar o modelo
     _mlContext.Model.Save(_model, dataView.Schema, _modelPath);
 }

     /// <summary>
        /// Faz previsão de manutenção para uma moto
     /// </summary>
 public MaintenancePredictionResult PredictMaintenance(int quilometragem, int ano)
        {
if (_model == null)
        {
  throw new InvalidOperationException("Modelo não foi treinado.");
    }

       var idadeMoto = DateTime.Now.Year - ano;

            var input = new MotoMaintenanceData
          {
           Quilometragem = quilometragem,
   Ano = ano,
           IdadeMoto = idadeMoto
 };

     var predictionEngine = _mlContext.Model.CreatePredictionEngine<MotoMaintenanceData, MaintenancePrediction>(_model);
 var prediction = predictionEngine.Predict(input);

        // Calcular próxima revisão
    var kmProximaRevisao = CalcularProximaRevisao(quilometragem);

            // Determinar prioridade
            var prioridade = DeterminarPrioridade(prediction.Probabilidade);

         // Gerar recomendação
            var recomendacao = GerarRecomendacao(prediction.NecessitaManutencao, prediction.Probabilidade, quilometragem, idadeMoto);

 return new MaintenancePredictionResult
    {
      NecessitaManutencao = prediction.NecessitaManutencao,
          ProbabilidadeManutencao = prediction.Probabilidade * 100,
      Recomendacao = recomendacao,
         Prioridade = prioridade,
           KmProximaRevisao = kmProximaRevisao
            };
        }

      private int CalcularProximaRevisao(int quilometragemAtual)
        {
         // Revisões a cada 5000 km
            var proximaRevisao = ((quilometragemAtual / 5000) + 1) * 5000;
            return proximaRevisao;
        }

        private string DeterminarPrioridade(float probabilidade)
   {
            if (probabilidade >= 0.8f) return "ALTA";
  if (probabilidade >= 0.5f) return "MÉDIA";
            return "BAIXA";
        }

     private string GerarRecomendacao(bool necessitaManutencao, float probabilidade, int km, int idade)
    {
            if (necessitaManutencao)
        {
   if (probabilidade >= 0.8f)
      return $"URGENTE: Agende manutenção imediatamente! Moto com {km:N0} km e {idade} anos.";
                else if (probabilidade >= 0.6f)
 return $"ATENÇÃO: Manutenção recomendada em breve. Moto com {km:N0} km e {idade} anos.";
                else
         return $"Considere agendar manutenção preventiva. Moto com {km:N0} km e {idade} anos.";
            }
            else
          {
        if (km < 5000)
             return "Moto em excelente estado. Continue com as revisões preventivas.";
   else
     return "Moto em bom estado. Mantenha as revisões periódicas em dia.";
            }
        }
    }
}
