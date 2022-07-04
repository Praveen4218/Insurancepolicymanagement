using Microsoft.AspNetCore.Mvc;
using IMS;

namespace Insurance.Controllers
{
    
    public class CustomerController : Controller
    {
        Class1 ob = new Class1();
        insuranceContext dc = new insuranceContext();

        public ActionResult Home()
        {
            
                var res = ob.DisplayItems();
                return View(res);
        }

            


        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer c)
        {
            HttpContext.Session.Remove("user");
            if (ModelState.IsValid)
            {
                var i = ob.Register(c);



                if (i > 0)
                {

                    ViewData["a"] = "User created Successfully";
                }
                else
                {
                    ViewData["a"] = "Please enter valid details ";
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ViewResult Signin()
        {

            return View();

        }


        [HttpPost]
        public ActionResult Signin(int cand, string uname, string pwd)
        {




            var res = ob.Login(cand, uname, pwd);
            if (res > 0 && cand == 0)
            {
                HttpContext.Session.SetString("user", uname);

                return RedirectToAction("Home");

                //ViewData["v"]= HttpContext.Session.GetString("user");
            }

            else if (res > 0 && cand == 1)
            {
                HttpContext.Session.SetString("user", uname);
                return RedirectToAction("Home", "admin");
            }

            else
            {
                ViewData["a"] = "Invalid username or password";
                return View();
            }

        }
        public ViewResult Signout()
        {// logic for login page goes here

            HttpContext.Session.Remove("user");

            return View();

        }



        public ActionResult Appliedpolicy()
        {

            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction("signin");
            }

            else
            {
                string user = HttpContext.Session.GetString("user");

                var id = ob.Customerid(user);
                var res = ob.allpolicy();
                TempData["idd"] = id;
                TempData.Keep("idd");

                var result = from t in res.Policy
                             from t1 in res.appliedpolicy
                             where t.Pid == t1.Pid && t1.CustId == id
                             select new { Policy = t, appliedpolicy = t1 };

                return View(result);
            }

        }



        [HttpGet]
        public ViewResult Policy(int pid)
        {

            TempData["id"] = pid;
            TempData.Keep("id");
            var res = ob.policyy(pid);

            return View(res);

        }
        [HttpPost]
        public ActionResult Policy(AppliedPolicy c)
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction("Signin");
            }

            else
            {


                string user = HttpContext.Session.GetString("user");

                int candidateid = ob.Customerid(user);
                int Pid = Convert.ToInt32(TempData["id"]);
                var check = ob.Policycheck(Pid, candidateid);

                if (check > 0)
                {
                   
                    ViewData["a"] ="you already applied";

                    return View();
                }
                else
                {
                    IMS.AppliedPolicy j = new IMS.AppliedPolicy();
                    j.Pid = Convert.ToInt32(TempData["id"]);
                    j.CustId = candidateid;
                    j.Dateofapplied = DateTime.Today;

                    var i = ob.toapply(j);

                    if (i > 0)
                    {
                        ViewData["a"] = "applied successfully";

                    }
                    else
                    {
                        ViewData["a"] = "please enter valid details ";

                    }
                    return View();
                }

            }
        }

        //private void myfunction()
        //{
        //    throw new NotImplementedException();
        //}

        [HttpGet]
        public ActionResult edit(int name)
        {
            var result = dc.Customers.ToList().Find(c => c.CustId == name);

            return View(result);

        }

        [HttpPost]
        public ActionResult edit(Customer r)
        {


            dc.Customers.Update(r);
            int i = dc.SaveChanges();

            if (i > 0)
            {
                ViewData["a"] = " updated Successfully";

            }
            else
            {
                ViewData["a"] = "Try again";
            }

            return View();
        }



        [HttpGet]
        public ActionResult message()
        {

            return View();
        }
        [HttpPost]
        public ActionResult message(Question q)
        {
            string user = HttpContext.Session.GetString("user");

            int id = ob.Customerid(user);
            q.CustId = id;
            dc.Questions.Add(q);
            int i = dc.SaveChanges();

            if (i > 0)
            {
                ViewData["a"] = "   messagesent Successfully";

            }
            else
            {
                ViewData["a"] = "Try again";
            }


            return View();
        }

        public ActionResult viewmessage()
        {
            string user = HttpContext.Session.GetString("user");

            int id = ob.Customerid(user);
            var result = dc.Questions.ToList();
            return View(result);
        }

        public ActionResult Profile()
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction("Signin");
            }

            else
            {
                String p = HttpContext.Session.GetString("user");
                var res = ob.Profile(p);


                return View(res);
            }

        }

        public List<Customer> getusers()
        {
            return dc.Customers.ToList();
        }
        public Customer getuser(int id)
        {
            return dc.Customers.Where(x => x.CustId == id).FirstOrDefault();
        }

        public ViewResult ListofPolicy()
        {
            return View();
        }

    }
}
