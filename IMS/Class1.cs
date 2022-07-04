namespace IMS
{
    public class Class1
    {
        insuranceContext dc=new insuranceContext();


        public int Register(Customer r)
        {

            dc.Customers.Add(r);
            int res = dc.SaveChanges();
            return res;



        }
        public int Login(int cand, string uname, string pwd)
        {
            if (cand == 0)
            {

                var res = (from t in dc.Customers
                           where t.Mail == uname & t.Pwd == pwd
                           select t).Count();

                return res;

            }

            else if (cand == 1)
            {
                if (uname == "praveen@gmail.com" && pwd == "praveen")
                {
                    var res = 1;
                    return res;
                }
                else
                {
                    var res = 0;
                    return res;

                }


            }
            else
            {
                var res = 0;
                return res;

            }
        }

        public List<Policy> DisplayItems()
        {

            var res = dc.Policies.ToList();


            return res;
        }

        public int policyno()
        {

            var res = dc.Policies.Count();


            return res;
        }
        public int noofusers()
        {

            var res = dc.Customers.Count();


            return res;
        }
        public int noofapprovals()
        {

            var res = (from t in dc.AppliedPolicies where t.Status == null select t).Count();

            return res;
        }


        public can_dat allpolicy()
        {

            can_dat posts = new can_dat();
            posts.Customer = dc.Customers.ToList();
            posts.Policy = dc.Policies.ToList();
            posts.appliedpolicy = dc.AppliedPolicies.ToList();
            return posts;


        }


        public class can_dat
        {
            public List<Customer> Customer { get; set; }
            public List<Policy> Policy { get; set; }
            public List<AppliedPolicy> appliedpolicy { get; set; }

        }
        public int Policybutton(int Customerid, int Pid, String Button)
        {
            var result = (from t in dc.AppliedPolicies
                          where t.Pid == Pid && t.CustId == Customerid
                          select t).FirstOrDefault();
            result.Status = Button;

            int res = dc.SaveChanges();

            return res;
        }

        //----customer

        public int Customerid(string user)
        {

            var res = (from t in dc.Customers
                       where t.Mail == user
                       select t.CustId).FirstOrDefault();
            return res;

        }
        public List<Policy> policyy(int Pid)
        {

            var res = (from t in dc.Policies
                       where t.Pid == Pid
                       select t);
            return res.ToList();
        }


        public int toapply(AppliedPolicy j)
        {

            dc.AppliedPolicies.Add(j);
            int res = dc.SaveChanges();
            return res;

        }
        public int Policycheck(int Pid, int candidateid)
        {

            var res = (from t in dc.AppliedPolicies
                       where t.Pid == Pid && t.CustId == candidateid
                       select t).Count();
            return res;

        }

        public int updatereply(int Sno, String comment)
        {
            var result = (from t in dc.Questions
                          where t.Sno == Sno
                          select t).FirstOrDefault();
            result.Answer = comment;

            int res = dc.SaveChanges();

            return res;
        }

        public List<Customer> Profile(string p)
        {
            var result = (from t in dc.Customers
                          where t.Mail == p
                          select t).ToList();
            return result;

        }
    }
}