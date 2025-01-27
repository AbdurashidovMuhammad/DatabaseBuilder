
using Application.Services;
using DataAccess;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddSingleton(new UserRepository(connectionString));
            builder.Services.AddSingleton(new DatabaseRepository(connectionString));
            builder.Services.AddSingleton(new SchemaRepository(connectionString));
            builder.Services.AddSingleton(new TableRepository(connectionString));
            builder.Services.AddSingleton(new ColumnRepository(connectionString));

            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<DatabaseService>();
            builder.Services.AddTransient<SchemaService>();
            builder.Services.AddTransient<TableService>();
            builder.Services.AddTransient<ColumnService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
