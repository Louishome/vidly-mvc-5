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
            var viewModel = new NewCustomerViewModel
            {
                MembershipTypes = m_context.MembershipTypes.ToList(),
            };
            return View(viewModel);
        }

        public ActionResult Create(Customer customer)
        {
            m_context.Customers.Add(customer);
            m_context.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }

        public ActionResult Details(int id)
        {
            var customer = GetCustomers().SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        private IEnumerable<Customer> GetCustomers() => m_context.Customers.Include(c => c.MembershipType).ToList();
    }
}