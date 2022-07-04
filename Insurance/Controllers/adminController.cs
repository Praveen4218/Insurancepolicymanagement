using Microsoft.AspNetCore.Mvc;
using IMS;


namespace Insurance.Controllers
{
    public class adminController : Controller
    {
        Class1 ob = new Class1();
        insuranceContext dc= new insuranceContext();  
        public ActionResult Home()
        {
            ViewData["a"] = ob.policyno();
            ViewData["b"] = ob.noofusers();
            ViewData["c"] = ob.noofapprovals();
            return View();
        }



        [HttpGet]
        public ActionResult Policies()
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction("signin");
            }

            else
            {
                var res = ob.allpolicy();

                var result = from t in res.Customer
                             from t1 in res.Policy
                             from t2 in res.appliedpolicy
                             where t2.Pid == t1.Pid &&
                             t2.CustId == t.CustId && t2.Status == null
                             select new { Customer = t, Policy = t1, appiledpolicy = t2 };

                return View(result);
            }

        }

        public ActionResult viewpolicy()
        {
            var result = ob.DisplayItems();

            return View(result);
        }

        [HttpPost]
        public ActionResult Policies(int Customerid, int Pid, String Button)

        {
            var i = ob.Policybutton(Customerid, Pid, Button);
            if (i > 0)
            {

                return RedirectToAction("Policies");

            }
            else
            {
                ViewData["Approve"] = "invalid result";
                return View();
            }


        }

        [HttpGet]
        public ActionResult addpolicy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addpolicy(Policy p)
        {
            p.Doc = DateTime.Today;

            dc.Policies.Add(p);
            int i = dc.SaveChanges();

            if (i > 0)
            {
                ViewData["a"] = " added Successfully";

            }
            else
            {
                ViewData["a"] = "Try again";
            }

            return View();
        }


        [HttpGet]
        public ActionResult deletepolicy(int Pid)
        {
            var result = ob.policyy(Pid);
            return View(result);
        }
        [HttpPost]
        public ActionResult deletepolicy(Policy p, int Pid)
        {
            var result = dc.Policies.ToList().Find(c => c.Pid == Pid);
            dc.Policies.Remove(result);
            dc.SaveChanges();
            ViewData["a"] = "Sucessfully deleted!!";
            return View();
        }



        [HttpGet]
        public ActionResult editpolicy(int Pid)
        {
            var result = dc.Policies.ToList().Find(c => c.Pid == Pid);
            return View(result);
        }
        [HttpPost]
        public ActionResult editpolicy(Policy p)
        {
            p.Doc = DateTime.Now;

            dc.Policies.Update(p);
            int i = dc.SaveChanges();
            if (i > 0)
            {
                ViewData["a"] = " updated Sucessfully !!";
            }

            return View();
        }

        [HttpGet]
        public ActionResult answer()
        {
            //dc.Questions.Update(item);
            var result = dc.Questions.ToList();

            return View(result);
        }
        [HttpPost]
        public ActionResult answer(int Sno, string comment)
        {
            //dc.Questions.Update(item);
            var i = ob.updatereply(Sno, comment);
            if (i > 0)
            {
                ViewData["a"] = "reply updated";
            }
            else
            {
                ViewData["a"] = "error occured";
            }
            return View();
        }


        public ActionResult Signout()
        {
            return RedirectToAction("Signout", "Customer");
        }

    }
}
