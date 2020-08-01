using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAppli4.Models;

namespace MvcAppli4.Controllers
{
    public class EmployeeController : Controller
    {
        private hdfcEntities db = new hdfcEntities();

        //
        // GET: /Employee/

        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Department);
            return View(employees.ToList());
        }

        //
        // GET: /Employee/Details/5

        public ActionResult Details(int id = 0)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name");
            return View();
        }

        //
        // POST: /Employee/Create

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Name))
            {
                ModelState.AddModelError("Name", "The Name field is required.");
            }

            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        //
        // GET: /Employee/Edit/5

        
        public ActionResult Edit(int id = 0)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        public ActionResult Edit([Bind(Exclude="Name")]Employee employee)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(employee).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", employee.DepartmentId);
            //return View(employee);
            Employee employeeFromDB = db.Employees.Single(x => x.EmployeeId == employee.EmployeeId);
            employeeFromDB.EmployeeId = employee.EmployeeId;
            employeeFromDB.Gender = employee.Gender;
            employeeFromDB.City = employee.City;
            employeeFromDB.DepartmentId = employee.DepartmentId;
            employee.Name = employeeFromDB.Name;

            if (ModelState.IsValid)
            {
                //db.ObjectStateManager.ChangeObjectState(employeeFromDB, EntityState.Modified);
                db.Entry(employeeFromDB).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", employee.DepartmentId);

            //Employee employee = db.Employees.Find(id);
            //if (employee == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        //
        // GET: /Employee/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // POST: /Employee/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        //public ActionResult EmployeesByDepartment()
        //{
        //    var departmentTotals = db.Employees.Include("Department")
        //        .GroupBy(x => x.Department.Name)
        //        .Select(y => new DepartmentTotals
        //        {
        //            Name = y.Key,
        //            Total = y.Count()
        //        }).ToList().OrderBy(y => y.Total); 
        //    return View(departmentTotals);
        //}

        public ActionResult EmployeesByDepartment()
        {
            var departmentTotals = db.Employees.Include("Department")
                                        .GroupBy(x => x.Department.Name)
                                        .Select(y => new DepartmentTotals
                                        {
                                            Name = y.Key,
                                            Total = y.Count()
                                        }).ToList();
            return View(departmentTotals);
        }

    }
}