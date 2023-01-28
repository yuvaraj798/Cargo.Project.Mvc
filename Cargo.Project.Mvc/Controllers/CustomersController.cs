using Cargo.Project.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;
using System;
using System.Net.Http.Json;

namespace Cargo.Project.Mvc.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IConfiguration _configuration;
        public CustomersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<CustomerViewModel> customers = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Customers/GetAllCustomers");
                if(result.IsSuccessStatusCode)
                {
                    customers = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                }
            }
                return View(customers);
        }
        public async Task<IActionResult> Details(int Custid)
        {
            CustomerViewModel customer  = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Customers/GetCustomerById/{Custid}");
                if (result.IsSuccessStatusCode)
                {
                    customer = await result.Content.ReadAsAsync<CustomerViewModel>();
                }
            }
            return View(customer);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {

                    // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Customers/CreateCustomer", customer);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Custid)
        {
            if (ModelState.IsValid)
            {
              CustomerViewModel customer = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"customers/UpdateCustomer/{Custid}");
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                   // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"customers/UpdateCustomer/{customer.CustId}", customer);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error. Please try later");
                    }
                }
            }
         
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int Custid)
        {
            CustomerViewModel customer = await this.CustomerById(Custid);
            if (customer != null)
            {
                return View(customer);
            }
            ModelState.AddModelError("", "Server Error. Please try later");
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(CustomerViewModel customerVM)
        {
            using (var client = new HttpClient())
            {
              //  client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Movies/DeleteMovie/{customerVM.CustId}");
                //if (result.IsSuccessStatusCode)
                //{
                //    return RedirectToAction("Index");
                //}
                //else
                //{
                //    return RedirectToAction("Login", "Accounts");
                //}
            }
            return View(customerVM);


        }
        [NonAction]
        public async Task<CustomerViewModel> CustomerById(int Custid)
        {
            CustomerViewModel customer = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Customers/GetCustomerById/{Custid}");
                if (result.IsSuccessStatusCode)
                {
                    customer = await result.Content.ReadAsAsync<CustomerViewModel>();
                }
            }
            return customer;
        }
    }

}
