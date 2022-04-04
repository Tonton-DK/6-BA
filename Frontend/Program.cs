using ClassLibrary.Interfaces;
using Frontend.Data_Brokers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IUserService, UserBroker>();
builder.Services.AddTransient<IJobService, JobBroker>();
builder.Services.AddTransient<IOfferService, OfferBroker>();
builder.Services.AddTransient<IContractService, ContractBroker>();
builder.Services.AddTransient<IReviewService, ReviewBroker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
