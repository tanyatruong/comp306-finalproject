using Client.HttpClients;
using Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

Uri baseaddress = new Uri("https://localhost:7154");
builder.Services.AddHttpClient<EmployeeService>(client =>
    { client.BaseAddress = baseaddress; });
builder.Services.AddHttpClient<JobService>(client =>
    { client.BaseAddress = baseaddress; });

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
