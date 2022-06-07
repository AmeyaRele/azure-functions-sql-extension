// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.Sql.Samples.TriggerBindingSamples;
using Microsoft.Azure.WebJobs.Extensions.Sql.Samples.Common;

namespace Microsoft.Azure.WebJobs.Extensions.Sql.Tests.Integration
{
    [Collection("IntegrationTests")]
    public class SqlTriggerBindingIntegrationTests : IntegrationTestBase
    {
        public SqlTriggerBindingIntegrationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async void InsertProductsTest()
        {
            this.StartFunctionHost(nameof(ProductsTrigger));
            Product[] products = GetProducts(3, 100);
            this.InsertProducts(products);
            int countInsert = 0;
            this.FunctionHost.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                if (e != null && !string.IsNullOrEmpty(e.Data) && e.Data.Contains($"Change occurred to Products table row"))
                {
                    countInsert++;
                }
            };
            await Task.Delay(5000);
            Assert.Equal(3, countInsert);
        }
        private static Product[] GetProducts(int n, int cost)
        {
            var result = new Product[n];
            for (int i = 1; i <= n; i++)
            {
                result[i - 1] = new Product
                {
                    ProductID = i,
                    Name = "test",
                    Cost = cost * i
                };
            }
            return result;
        }
        private void InsertProducts(Product[] products)
        {
            if (products.Length == 0)
            {
                return;
            }

            var queryBuilder = new StringBuilder();
            foreach (Product p in products)
            {
                queryBuilder.AppendLine($"INSERT INTO dbo.Products VALUES({p.ProductID}, '{p.Name}', {p.Cost});");
            }

            this.ExecuteNonQuery(queryBuilder.ToString());
        }
    }
}