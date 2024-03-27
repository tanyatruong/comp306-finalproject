using Client.HttpClients;
using Client.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

Uri baseaddress = new Uri("https://34.49.150.157.nip.io/group6api/");


builder.Services.AddHttpClient<EmployeeService>(client =>
    { 
        client.BaseAddress = baseaddress;
		client.DefaultRequestHeaders.Accept.Clear();
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		client.DefaultRequestHeaders.Add("apikey", "Vcq1g7lUaFsf90AFIMcHYIWdlmoIXSZZpT0M1sCOAadf2UIg");
	});
builder.Services.AddHttpClient<JobService>(client =>
    { 
        client.BaseAddress = baseaddress;
		client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		client.DefaultRequestHeaders.Add("apikey", "Vcq1g7lUaFsf90AFIMcHYIWdlmoIXSZZpT0M1sCOAadf2UIg");
	});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
