using DIContainer;
using Test.Interfaces;
using Test.Services;
using Test.Tests;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Container.Register<IOperationScoped, Operation>(ServiceLifecycle.Scoped);
            Container.Register<IOperationSingleton, Operation>(ServiceLifecycle.Singleton);
            Container.Register<IOperationTransient, Operation>(ServiceLifecycle.Transient);
              
            Container.Register<OperationService, OperationService>();

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
