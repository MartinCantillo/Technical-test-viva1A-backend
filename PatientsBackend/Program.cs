using Microsoft.EntityFrameworkCore;
using Data;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<IPatientRepository, PatientRepository>();


builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseHttpsRedirection();

app.UseCors("NewPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();