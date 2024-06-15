using BootstrapBlazor.Components;
using BootstrapBlazor.IFrameDemo.Components;
using BootstrapBlazor.IFrameDemo.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using static Microsoft.Extensions.DependencyInjection.CultureStorageExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//添加JSON配置文件
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBootstrapBlazor();
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddCultureStorage();

builder.Services.AddSingleton<ICultureStorage, DefaultCultureStorage>();
//配置多语言文化资源文件
builder.Services.ConfigureJsonLocalizationOptions(options =>
{
    //根据语言配置查找对应的JSON文件。
    var cultures = builder.Configuration.GetSection("BootstrapBlazorOptions:SupportedCultures").GetChildren();
    if (cultures.Any())
    {
        List<string> jsonFiles = new();
        foreach (var item in cultures)
        {
            //AppDomain.CurrentDomain.SetupInformation.ApplicationBase, $"Locales\\{item.Value}.json");
            jsonFiles.Add(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase ?? "", $"Locales\\{item.Value}.json"));
        }
        // 设置 Json 物理路径文件
        options.AdditionalJsonFiles = jsonFiles.ToArray();

        // 忽略本地化键值文化信息丢失
        options.IgnoreLocalizerMissing = true;
    }
});

builder.Services.AddRequestLocalization<IOptionsMonitor<BootstrapBlazorOptions>>((localizerOption, blazorOptions) =>
{
    blazorOptions.OnChange(op => Invoke(op));
    Invoke(blazorOptions.CurrentValue);
    void Invoke(BootstrapBlazorOptions options)
    {
        var supportedCultures = options.GetSupportedCultures();
        localizerOption.SupportedCultures = supportedCultures;
        localizerOption.SupportedUICultures = supportedCultures;
        localizerOption.SetDefaultCulture("en-US");
    }
});

builder.Services.AddSingleton<WeatherForecastService>();

// 增加 Table 数据服务操作类
builder.Services.AddTableDemoDataService();

// 增加 SignalR 服务数据传输大小限制配置
builder.Services.Configure<HubOptions>(option => option.MaximumReceiveMessageSize = null);

var app = builder.Build();

//启用本地化
var option = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (option != null)
{
    app.UseRequestLocalization(option.Value);
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseResponseCompression();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
