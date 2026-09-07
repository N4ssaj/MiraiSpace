using Microsoft.Extensions.DependencyInjection;
using MiraiSpace.Application.Documents;
using MiraiSpace.Presentation.CommandLine;
using MiraiSpace.Presentation.Features.Documents;
using MiraiSpace.Presentation.Navigation;

namespace MiraiSpace.Presentation.DependencyInjection;

internal static class DocumentsServiceCollectionExtensions
{
    internal static IServiceCollection AddDocuments(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentRepository, DemoDocumentRepository>();
        services.AddTransient<DocumentsPageViewModel>();
        services.AddTransient<DocumentPageViewModel>();
        services.AddTransient<DocumentEditorComponent>();
        services.AddTransient<DocumentDetailsComponent>();
        services.AddTransient<DocumentNameComponent>();
        services.AddTransient<RenameDocumentDialog>();
        services.AddTransient<DeleteDocumentDialog>();
        services.AddSingleton<NavigationRoute, DocumentsRoute>();
        services.AddSingleton<NavigationRoute, DocumentRoute>();
        services.AddScoped<ICommandModule, DocumentCommands>();
        return services;
    }
}
