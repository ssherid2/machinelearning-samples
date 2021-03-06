﻿Imports eShopForecast
Imports eShopDashboard.Infrastructure.Data.Catalog
Imports eShopDashboard.Infrastructure.Data.Ordering
Imports eShopDashboard.Infrastructure.Setup
Imports eShopDashboard.Queries
Imports eShopDashboard.Settings
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.ML
Imports Microsoft.ML
Imports Serilog

Namespace eShopDashboard
	Public Class Startup
'INSTANT VB NOTE: The variable configuration was renamed since Visual Basic does not handle local variables named the same as class members well:
		Public Sub New(configuration_Renamed As IConfiguration)
			Me.Configuration = configuration_Renamed
		End Sub

		Public ReadOnly Property Configuration As IConfiguration

		' This method gets called by the runtime. Use this method to add services to the container.
		Public Sub ConfigureServices(services As IServiceCollection)
			services.AddDbContext(Of CatalogContext)(Function(options) options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))

			services.AddDbContext(Of OrderingContext)(Function(options) options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))

			services.AddScoped(Of IOrderingQueries, OrderingQueries)()
			services.AddScoped(Of ICatalogQueries, CatalogQueries)()
			services.AddScoped(Of CatalogContextSetup)()
			services.AddScoped(Of OrderingContextSetup)()

			services.AddPredictionEnginePool(Of ProductData, ProductUnitRegressionPrediction)().FromFile(Configuration("ProductMLModelPath"))
			services.AddPredictionEnginePool(Of CountryData, CountrySalesPrediction)().FromFile(Configuration("CountryMLModelPath"))

			services.Configure(Of CatalogSettings)(Configuration.GetSection("CatalogSettings"))

			services.AddMvc()

			services.Configure(Of AppSettings)(Configuration)

			services.AddSwaggerGen(Sub(options)
				options.DescribeAllEnumsAsStrings()
				options.SwaggerDoc("v1", New Swashbuckle.AspNetCore.Swagger.Info With {
					.Title = "eShopDashboard - API",
					.Version = "v1",
					.Description = "Web Dashboard REST HTTP API.",
					.TermsOfService = "Terms Of Service"
				})
			End Sub)

			'Get info on Thread Pooling just for debugging/exploring, this code can be deleted:
			Dim worker As Integer = 0
			Dim io As Integer = 0
			System.Threading.ThreadPool.GetAvailableThreads(worker, io)

			Log.Information("Thread pool threads available at startup: ")
			Log.Information("   Worker threads: {0:N0}", worker)
			Log.Information("   Asynchronous I/O threads: {0:N0}", io)
		End Sub

		' This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		Public Sub Configure(app As IApplicationBuilder, env As IHostingEnvironment)
			If env.IsDevelopment() Then
				app.UseDeveloperExceptionPage()
			Else
				app.UseExceptionHandler("/Error")
			End If

			app.UseStaticFiles()

			app.UseMvc()

			Dim pathBase = Configuration("PATH_BASE")

			app.UseSwagger().UseSwaggerUI(Sub(c)
				  c.SwaggerEndpoint($"{(If(Not String.IsNullOrEmpty(pathBase), pathBase, String.Empty))}/swagger/v1/swagger.json", "eShopDashboard.API V1")
			End Sub)
		End Sub
	End Class
End Namespace
