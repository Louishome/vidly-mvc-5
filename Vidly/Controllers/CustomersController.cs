using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext m_context;

        public CustomersController()
        {
            m_context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            m_context.Dispose();
        }

        public ViewResult Index()
        {
            var customers = GetCustomers();

            return View(customers);
        }

        public ActionResult New()
        {
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = m_context.MembershipTypes.ToList(),
            };
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = m_context.MembershipTypes.ToList()
                }; 
                return View("CustomerForm", viewModel);
            }

            if (customer.Id == 0)
            {
                m_context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = m_context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.BirthDay = customer.BirthDay;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
            }
            
            m_context.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }

        public ActionResult Details(int id)
        {
            var customer = GetCustomers().SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = m_context.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }

        private IEnumerable<Customer> GetCustomers() => m_context.Customers.Include(c => c.MembershipType).ToList();

        public ActionResult Edit(int id)
        {
            var customer = m_context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CustomerFormViewModel()
            {
                Customer = customer,
                MembershipTypes = m_context.MembershipTypes.ToList()
            };

            return View("CustomerForm", viewModel);
        }
    }
}