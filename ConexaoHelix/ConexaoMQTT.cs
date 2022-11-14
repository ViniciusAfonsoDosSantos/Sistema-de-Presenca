using RestSharp;
using System;
using TrabalhoInterdisciplinar.Models;

namespace TrabalhoInterdisciplinar.ConexãoHelix
{
    public class ConexaoMQTT
    {
        public void ProvisionaDadosMQTT(AlunoViewModel model)
        {
            var client = new RestClient("http://191.233.28.24:4041/iot/devices");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("fiware-service", "helixiot");
            request.AddHeader("fiware-servicepath", "/");
            var body = @"{" + "\n" +
            @"  ""devices"": [" + "\n" +
            @"    {" + "\n" +
            @$"      ""device_id"": ""aluno0{model.IdBiometria}""," + "\n" +
            @$"      ""entity_name"": ""urn:ngsi-ld:aluno:0{model.IdBiometria}""," + "\n" +
            @"      ""entity_type"": ""Aluno""," + "\n" +
            @"      ""protocol"": ""PDI-IoTA-UltraLight""," + "\n" +
            @"      ""transport"": ""MQTT""," + "\n" +
            @"      ""commands"": [" + "\n" +
            @"        {""name"": ""create"",""type"": ""command""}," + "\n" +
            @"        {""name"": ""delete"",""type"": ""command""}," + "\n" +
            @"        {""name"":""read"", ""type"":""command""}" + "\n" +
            @"       ]," + "\n" +
            @"       ""attributes"": [" + "\n" +
            @"        {""object_id"": ""alunoId"", ""name"": ""alunoid"", ""type"":""Text""}," + "\n" +
            @"        {""object_id"": ""presenca"", ""name"": ""presenca"", ""type"":""Text""}" + "\n" +
            @"       ]" + "\n" +
            @"    }" + "\n" +
            @"  ]" + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void RegistraDadosMQTT(AlunoViewModel model)
        {
            var client = new RestClient("http://191.233.28.24:1026/v2/registrations");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("fiware-service", "helixiot");
            request.AddHeader("fiware-servicepath", "/");
            var body = @"{" + "\n" +
            @"  ""description"": ""Student Commands""," + "\n" +
            @"  ""dataProvided"": {" + "\n" +
            @"    ""entities"": [" + "\n" +
            @"      {" + "\n" +
            @$"        ""id"": ""urn:ngsi-ld:aluno:0{model.IdBiometria}"",""type"": ""Aluno""" + "\n" +
            @"      }" + "\n" +
            @"    ]," + "\n" +
            @"    ""attrs"": [ ""create"", ""delete"", ""read"" ]" + "\n" +
            @"  }," + "\n" +
            @"  ""provider"": {" + "\n" +
            @"    ""http"": {""url"": ""http://191.233.28.24:4041""}," + "\n" +
            @"    ""legacyForwarding"": true" + "\n" +
            @"  }" + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void PublishMQTT(AlunoViewModel model)
        {
            var client = new RestClient($"http://191.233.28.24:1026/v2/entities/urn:ngsi-ld:aluno:0{model.IdBiometria}/attrs");
            var request = new RestRequest();
            request.Method = Method.Patch;
            request.Timeout = 10000;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("fiware-service", "helixiot");
            request.AddHeader("fiware-servicepath", "/");
            var body = @"{" + "\n" +
            @"  ""create"": {" + "\n" +
            @"      ""type"" : ""command""," + "\n" +
            @"      ""value"" : """"" + "\n" +
            @"  }" + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public string RecebeMQTT()
        {
            //Método para receber qual foi a mensagem -> Erro ou Sucesso
            var client = new RestClient("http://191.233.28.24:1026/v2/entities/urn:ngsi-ld:aluno:022/attrs/msg");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.Timeout = 10000;
            request.AddHeader("fiware-service", "helixiot");
            request.AddHeader("fiware-servicepath", "/");
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);
            string mensagem = response.Content.ToString();
            return mensagem;
        }
    }
}
