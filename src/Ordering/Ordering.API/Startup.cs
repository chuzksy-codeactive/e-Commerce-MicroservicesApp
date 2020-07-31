using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Application.Handlers;
using Ordering.Core.Repositories;
using Ordering.Core.Repositories.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repository;
using Ordering.Infrastructure.Repository.Base;
using System.Reflection;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region SqlServer Dependencies

            //// use in-memory database
            //services.AddDbContext<OrderContext>(c =>
            //    c.UseInMemoryDatabase("OrderConnection"));

            // use real database
            services.AddDbContext<OrderContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("OrderConnection"))); // we made singleton this in order to resolve in mediatR when consuming Rabbit

            #endregion

            #region Project Dependencies

            // Add Infrastructure Layer
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddTransient<IOrderRepository, OrderRepository>(); // we made transient this in order to resolve in mediatR when consuming Rabbit

            // Add AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Add MediatR
            services.AddMediatR(typeof(CheckoutOrderHandler).GetTypeInfo().Assembly);
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
