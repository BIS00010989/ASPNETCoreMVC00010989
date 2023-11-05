using MVCWeb00010989.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCWeb00010989.Controllers
{
    public class ProductController : Controller
    {
        // Create an instance of HttpClient to make HTTP requests
        private readonly HttpClient _httpClient;

        // Initialize the HttpClient and set the base address for API requests
        public ProductController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://ec2-35-173-184-185.compute-1.amazonaws.com");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: Product
        public async Task<ActionResult> Index()
        {
            // Initialize a list to store the product information
            List<Product> ProductInfo = new List<Product>();

            // Send a GET request to the API to retrieve product data
            HttpResponseMessage Response = await _httpClient.GetAsync("api/Product");

            // Check if the response is successful
            if (Response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var ProductResponse = await Response.Content.ReadAsStringAsync();

                // Deserialize the JSON string into a list of Product objects
                ProductInfo = JsonConvert.DeserializeObject<List<Product>>(ProductResponse);
            }
            // Return the product information to the view
            return View(ProductInfo);
        }


        // GET: Product/Details/5
        public async Task<ActionResult> Details(int Id)
        {
            // Send a GET request to the API to retrieve product details by Id
            var response = await _httpClient.GetAsync($"api/Product/{Id}");

            // Read the response content as a string
            var productResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON string into a Product object
            var product = JsonConvert.DeserializeObject<Product>(productResponse);

            // Return the product details to the view
            return View(product);
        }


        // GET: Product/Create
        public ActionResult Create()
        {
            // Render the Create view which displays the form for creating a new product
            return View();
        }
        

        [HttpPost]
        public ActionResult Create(Product product)
        {
            // Send a GET request to the API to retrieve the Supplier associated with the product.Id
            HttpResponseMessage supplierResponse = _httpClient.GetAsync($"api/Supplier/{product.Id}").Result;

            // Deserialize the response content into a Supplier object and assign it to the ProductCategory property of the product
            var supplier = JsonConvert.DeserializeObject<Supplier>(supplierResponse.Content.ReadAsStringAsync().Result);
            product.ProductCategory = supplier;

            // Convert the product object into a JSON string
            string createProductInfo = JsonConvert.SerializeObject(product);

            // Create a StringContent object with the JSON string and specify the media type as application/json
            StringContent stringContentInfo = new StringContent(createProductInfo, Encoding.UTF8, "application/json");

            // Send a POST request to the API endpoint to create a new product
            HttpResponseMessage createHttpResponseMessage = _httpClient.PostAsync("api/Product", stringContentInfo).Result;

            // After the product is created, redirect to the Index action to display the updated list of products
            return RedirectToAction(nameof(Index));
        }


        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // Send a GET request to the API to retrieve the product details by Id
            var response = await _httpClient.GetAsync($"api/Product/{id}");

                // Read the response content as a string
                var productResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON string into a Product object
                var product = JsonConvert.DeserializeObject<Product>(productResponse);

                // Return the Edit view with the retrieved product details
                return View(product);
        }


        // POST: Product/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            // Send a GET request to the API to retrieve the product details by Id
            var response = await _httpClient.GetAsync($"api/Product/{id}");

                // Read the response content as a string
                var productResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON string into a Product object
                var product = JsonConvert.DeserializeObject<Product>(productResponse);

            // Update the product with the new values from the form collection
                product.Id = int.Parse(collection["Id"]);
                product.Name = collection["Name"];
                product.Description = collection["Description"];
                product.Weight = int.Parse(collection["Weight"]);
                product.Units = collection["Units"];
                product.SuppliedDate = DateTime.Parse(collection["SuppliedDate"]);

                // Convert the updated product to JSON
                var serializedProduct = JsonConvert.SerializeObject(product);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");

                // Send a PUT request to the API to update the product
                var updateResponse = await _httpClient.PutAsync($"api/Product/{id}", content);

                // Check if the update response is successful
                if (updateResponse.IsSuccessStatusCode)
                {
                    // Redirect to the Index action to show the updated product list
                    return RedirectToAction("Index");
            }

            // If there was an error or the product was not found, return to the edit view with the original values
            var Product = new Product
            {
                Id = id,
                Name = collection["Name"],
                Description = collection["Description"],
                Weight = int.Parse(collection["Weight"]),
                Units = collection["Units"],
                SuppliedDate = DateTime.Parse(collection["SuppliedDate"])
            };
            return View(Product);
        }


        // GET: Product/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            // Send a GET request to the API to retrieve the product details by Id
            var response = await _httpClient.GetAsync($"api/Product/{id}");

                // Read the response content as a string
                var productResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON string into a Product object
                var product = JsonConvert.DeserializeObject<Product>(productResponse);

                // Return the Delete view with the retrieved product details
                return View(product);
        }


        // POST: Product/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            // Send a DELETE request to the API to delete the product by Id
            var deleteResponse = await _httpClient.DeleteAsync($"api/Product/{id}");

                // Redirect to the Index action to show the updated product list
                return RedirectToAction("Index");
        }
    }
}

