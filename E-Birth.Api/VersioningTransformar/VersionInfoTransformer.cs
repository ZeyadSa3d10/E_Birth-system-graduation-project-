//using Microsoft.OpenApi.Models;
//using Microsoft.AspNetCore.OpenApi;

//namespace E_Birth.Api.VersioningTransformar
//{
//    public class VersionInfoTransformer : IOpenApiDocumentTransformer
//    {
//        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
//        {
//            var Versions = context.DocumentName;
//            document.Info.Version = Versions;
//            document.Info.Title = $"E-Birth API {Versions}";
//            return Task.CompletedTask;
//        }
//    }
//}
