using Refit;

namespace BotSharp.Plugin.MetaMessenger;

/// <summary>
/// https://developers.facebook.com/docs/messenger-platform/overview
/// </summary>
public class MetaMessengerPlugin : IBotSharpPlugin
{
    public string Name => "Meta Messenger";

    public void RegisterDI(IServiceCollection services, IConfiguration config)
    {
        var settings = new MetaMessengerSetting();
        config.Bind("MetaMessenger", settings);
        services.AddSingleton(x =>
        {
            Console.WriteLine($"Loaded MetaMessenger settings: {settings.Endpoint}/{settings.ApiVersion} {settings.PageId} {settings.PageAccessToken.SubstringMax(4)}");
            return settings;
        });

        // services.AddScoped<AuthHeaderHandler>();
        services.AddRefitClient<IMessengerGraphAPI>()
            .ConfigureHttpClient((sp, c) =>
                {
                    var setting = sp.GetRequiredService<MetaMessengerSetting>();
                    c.BaseAddress = new Uri($"{setting.Endpoint}");
                });
        //.AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddScoped<MessageHandleService>();
    }
}
