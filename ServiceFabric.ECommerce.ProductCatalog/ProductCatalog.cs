﻿using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.ECommerce.ProductCatalog.Model;

namespace ServiceFabric.ECommerce.ProductCatalog
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProductCatalog : StatefulService
    {
        private readonly IProductRepository _repository;

        public ProductCatalog(StatefulServiceContext context)
            : base(context)
        {
            _repository = new SfProductCatalogRepository(this.StateManager);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await _repository.AddProduct(
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Dell XPS 1520",
                    Description = "Powerfull laptop",
                    Price = 1630,
                    Availability = 10
                }
            );

            await _repository.AddProduct(
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "WASD Keyboard",
                    Description = "Mechanical Keyboard",
                    Price = 150,
                    Availability = 20
                }
            );

            var prods = await _repository.GetAllProducts();
        }
    }
}
