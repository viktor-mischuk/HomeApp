using HomeApi.Contracts.Validation;
using FluentValidation.AspNetCore;
using HomeApI.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using HomeApi.Data.Repos;
using HomeApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace HomeApI
{
    public class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile("HomeOptions.json")
            .Build();


        public static void Main(string[] args)
        {
            // Подключаем автомаппинг
            var assembly = Assembly.GetAssembly(typeof(MappingProfile));

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(assembly);

            // регистрация сервиса репозитория для взаимодействия с базой данных
            builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
            builder.Services.AddSingleton<IRoomRepository, RoomRepository>();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);
            // Подключаем валидацию
            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());
            
            // Добавляем новый сервис
            builder.Services.Configure<HomeOptions>(Configuration);
            // Загружаем только адресс (вложенный Json-объект))
            builder.Services.Configure<Address>(Configuration.GetSection("Address"));

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // поддерживает автоматическую генерацию документации WebApi с использованием Swagger
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" }); });

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
