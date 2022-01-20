using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using ECommerce1.Models;

namespace ECommerce1.Controllers
{
    public class AdminController : Controller
    // GET: Admin
    {
        public ActionResult Index()
        {
            return View();
        }
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;



        void mycon()
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["Ecommerce"].ToString());
                con.Open();
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {

            }

        }


        [HttpGet]
        public ActionResult CategoryList()
        {
            List<Category_Model> cat = new List<Category_Model>();

            mycon();
            cmd = new SqlCommand("Select * from Category_Master", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Category_Model model = new Category_Model();
                model.Cid = Convert.ToInt32(dr["C_id"]);
                model.Category = dr["Category"].ToString();
                model.Status = Convert.ToByte(dr["Status"]);
                model.UpdateOn = Convert.ToDateTime(dr["UpdateOn"].ToString());
                cat.Add(model);
               

            }
            con.Close();
            return View(cat);
           

        }





        // GET: Admin

        [HttpGet]
        public ActionResult AddCategory(int cid = 0)
        {
            try
            {
                if (cid > 0)
                {
                    mycon();
                    cmd = new SqlCommand("Select * from Category_Master where C_id=@cid", con);
                    cmd.Parameters.AddWithValue("@cid", cid);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        Category_Model model = new Category_Model();
                        model.Cid = Convert.ToInt32(dr["C_id"]);
                        model.Category = dr["Category"].ToString();
                        model.Status = Convert.ToByte(dr["Status"]);
                        model.UpdateOn = Convert.ToDateTime(dr["UpdateOn"].ToString());
                        return View(model);
                    }


                }

            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }

            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(Category_Model model)
        {
            try
            {


                if (model.Cid > 0)
                {
                    mycon();
                    cmd = new SqlCommand("Update Category_Master set Category=@category,Status=@status,UpdateOn=@updateon where C_id=@id", con);
                    cmd.Parameters.AddWithValue("@category", model.Category);
                    cmd.Parameters.AddWithValue("@status", model.Status);
                    cmd.Parameters.AddWithValue("@updateon", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@id", model.Cid);
                    cmd.ExecuteNonQuery();
                    ViewBag.msg = "Succefully Update Category";
                    con.Close();
                    return RedirectToAction("CategoryList");

                }
                else
                {
                    mycon();
                    cmd = new SqlCommand("Select * from Category_Master where Category=@cat", con);
                    cmd.Parameters.AddWithValue("@cat", model.Category);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        ViewBag.msg = "All RedyExists Category";
                        con.Close();

                    }
                    else
                    {
                        mycon();
                        cmd = new SqlCommand("Insert into Category_Master values(@category,@status,@updateon)", con);
                        cmd.Parameters.AddWithValue("@category", model.Category);
                        cmd.Parameters.AddWithValue("@status", model.Status);
                        cmd.Parameters.AddWithValue("@updateon", DateTime.UtcNow);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        ViewBag.msg = "Succefully Save Category";

                    }

                }

            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;

            }
            finally
            {

                con.Close();
            }
            return View();
        }
        public ActionResult CategoryDelete(int id = 0)
        {

            try
            {
                if (id > 0)
                {
                    mycon();
                    cmd = new SqlCommand("Delete from Category_Master where C_id=@cid", con);
                    cmd.Parameters.AddWithValue("@cid", id);
                    cmd.ExecuteNonQuery();
                    ViewBag.msg = "Succefully Delete Category.....";
                    return RedirectToAction("CategoryList");

                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;

            }
            finally
            {
                con.Close();
            }



            return View();
        }


        [HttpPost]
        public ActionResult Subdropfill(int id)
        {


            List<SelectListItem> list = new List<SelectListItem>()
             {
                   new SelectListItem {Text="---Select Category---",Value="0",Selected=true }

               };


            try
            {
                mycon();
                cmd = new SqlCommand("Select * from SubCategory_Master", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    list.Add(new SelectListItem
                    {
                        Value = Convert.ToString(dr["Sid"]),
                        Text = Convert.ToString(dr["SubCategory"])

                    });
                }

                ViewData["dropfill"] = list;
                con.Close();
                return Json(list, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult SubCategoryList()
        {
            // ViewBag.cid = cid;

            try
            {
                List<SubCategoryAdd> cat = new List<SubCategoryAdd>();

                mycon();
                cmd = new SqlCommand("Select * from SubCategory_Master As s inner join Category_Master As c on s.cid=c.C_id", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    SubCategoryAdd model = new SubCategoryAdd();
                    model.Sid = Convert.ToInt32(dr["Sid"]);
                    model.CategoryName = dr["Category"].ToString();
                    model.SubCategory = dr["SubCategory"].ToString();
                    model.Status = Convert.ToByte(dr["Status"]);
                    model.UpdateOn = Convert.ToDateTime(dr["UpdateOn"].ToString());
                    cat.Add(model);
                    
                }
                con.Close();
                return View(cat);

            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {

            }
            return View();

        }

        [HttpGet]
        public ActionResult SubAdd_Category(int Sid = 0)
        {


            try
            {
                if (Sid > 0)
                {
                    dropfill();
                    mycon();
                    cmd = new SqlCommand("select * from SubCategory_Master As s inner join Category_Master AS c on s.cid=c.C_id where Sid=@sid", con);
                    cmd.Parameters.AddWithValue("@sid", Sid);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        SubCategoryAdd model = new SubCategoryAdd();

                        model.Sid = Convert.ToInt32(dr["Sid"]);
                        model.Cid = dr["C_id"].ToString();
                        model.CategoryName = Convert.ToString(dr["Category"]);
                        model.SubCategory = dr["SubCategory"].ToString();
                        model.Status = Convert.ToByte(dr["Status"]);
                        model.UpdateOn = Convert.ToDateTime(dr["UpdateOn"]);
                        con.Close();
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }


            return View();
        }
        [HttpPost]
        public ActionResult SubAdd_Category(SubCategoryAdd model)
        {
            try
            {
                dropfill();

                if (model.Sid > 0)
                {
                    mycon();
                    cmd = new SqlCommand("Update [SubCategory_Master] set cid=@cid,Subcategory=@subcategory,Status=@status,UpdateOn=@updateon where Sid=@id", con);
                    cmd.Parameters.AddWithValue("@cid", model.Cid);
                    cmd.Parameters.AddWithValue("@subcategory", model.SubCategory);
                    cmd.Parameters.AddWithValue("@status", model.Status);
                    cmd.Parameters.AddWithValue("@updateon", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@id", model.Sid);
                    cmd.ExecuteNonQuery();
                    ViewBag.msg = "Update Category";
                    con.Close();
                    return RedirectToAction("SubCategoryList");

                }
                else
                {
                    mycon();
                    cmd = new SqlCommand("Select * from SubCategory_Master where SubCategory=@cat", con);
                    cmd.Parameters.AddWithValue("@cat", model.SubCategory);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        ViewBag.msg = "All RedyExists Category";
                        con.Close();

                    }
                    else
                    {
                        mycon();
                        cmd = new SqlCommand("Insert into SubCategory_Master values(@cid,@subcategory,@status,@updateon)", con);
                        cmd.Parameters.AddWithValue("@cid", model.Cid);
                        cmd.Parameters.AddWithValue("@subcategory", model.SubCategory);
                        cmd.Parameters.AddWithValue("@status", model.Status);
                        cmd.Parameters.AddWithValue("@updateon", DateTime.UtcNow);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        ViewBag.msg = "Succefully Save SubCategory";

                    }

                }



            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;

            }
            finally
            {

                con.Close();
            }

            return View();
        }


        public ActionResult SubCategoryDelete(int sid = 0)
        {

            try
            {
                if (sid > 0)
                {
                    mycon();
                    cmd = new SqlCommand("Delete from SubCategory_Master where Sid=@sid", con);
                    cmd.Parameters.AddWithValue("scid", sid);
                    cmd.ExecuteNonQuery();
                    ViewBag.msg = "Succefully Delete Category.....";
                    return RedirectToAction("SuCategoryList");

                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;

            }
            finally
            {
                con.Close();
            }



            return View();
        }
        [HttpGet]
        public ActionResult ThirdCategoryList()
        {
            try
            {
                try
                {
                    List<ThirdCategoryModel> cat = new List<ThirdCategoryModel>();

                    mycon();
                    cmd = new SqlCommand("select * from ThairdCategory_Master As t   inner join Category_Master AS c on t.Cid=c.C_id inner join SubCategory_Master AS s on s.Sid=t.Cid", con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ThirdCategoryModel model = new ThirdCategoryModel();

                        model.Tid = Convert.ToInt32(dr["Tid"]);
                        model.CategoryName = dr["Category"].ToString();
                        model.SubCategory = dr["SubCategory"].ToString();
                        model.ThirdCategory = dr["ThirdCategory"].ToString();

                        model.Status = Convert.ToByte(dr["Status"]);
                        model.UpdateOn = Convert.ToDateTime(dr["UpdateOn"].ToString());
                        cat.Add(model);
                    }

                    return View(cat);
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex;
                }
                finally
                {
                    con.Close();
                }
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }
            return View();
        }
        public List<SelectListItem> dropfill()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select Category---", Value = "0", Selected = true }

               };
            try
            {
                mycon();
                cmd = new SqlCommand("select *from Category_Master;", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SelectListItem
                    {
                        Value = Convert.ToString(dr["C_id"]),
                        Text = Convert.ToString(dr["Category"])

                    });
                }
                ViewBag.Catdrop = list;


                con.Close();
                return list;
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }
            return list;

        }

        public ActionResult ThirdCategoryDrop(int cid, int sid)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {
                mycon();
                cmd = new SqlCommand("select * from ThairdCategory_Master  where Cid=@cid And Sid=@sid", con);
                cmd.Parameters.AddWithValue("@cid", cid);
                cmd.Parameters.AddWithValue("@sid", sid);

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    list.Add(new SelectListItem
                    {
                        Value = Convert.ToString(dr["Tid"]),
                        Text = Convert.ToString(dr["ThirdCategory"])

                    });
                }


                con.Close();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }


            return Json(list, JsonRequestBehavior.AllowGet);

        }



        public ActionResult Subdrop(int cid)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {
                mycon();
                cmd = new SqlCommand("select * from SubCategory_Master  where cid=@id;", con);
                cmd.Parameters.AddWithValue("@id", cid);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    list.Add(new SelectListItem
                    {
                        Value = Convert.ToString(dr["Sid"]),
                        Text = Convert.ToString(dr["SubCategory"])

                    });
                }


                con.Close();
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }


            return Json(list, JsonRequestBehavior.AllowGet);


        }





        [HttpPost]
        public ActionResult Subcat(int cid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            //{
            //      new SelectListItem {Text="---Select Category---",Value="0",Selected=true }

            //  };


            try
            {
                mycon();
                cmd = new SqlCommand("select * from SubCategory_Master  where cid=@id;", con);
                cmd.Parameters.AddWithValue("@id", cid);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    list.Add(new SelectListItem
                    {
                        Value = Convert.ToString(dr["Sid"]),
                        Text = Convert.ToString(dr["SubCategory"])

                    });
                }


                con.Close();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }


            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ThirdCategoryAdd(int tid = 0)
        {
            try
            {
                dropfill();
                if (tid > 0)
                {
                    ViewData["Catdrop"] = dropfill();

                    mycon();
                    cmd = new SqlCommand("select * from ThairdCategory_Master As t   inner join Category_Master AS c on t.Cid=c.C_id inner join SubCategory_Master AS s on s.Sid=t.Cid where t.Tid=@tid", con);
                    cmd.Parameters.AddWithValue("@tid", tid);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ThirdCategoryModel model = new ThirdCategoryModel();
                        model.Tid = Convert.ToInt32(dr["Tid"]);
                        model.Cid = dr["Cid"].ToString();
                        model.Sid = Convert.ToInt32(dr["Sid"]);


                        model.CategoryName = dr["Category"].ToString();
                        ViewBag.sc = dr["SubCategory"].ToString();
                        model.ThirdCategory = dr["ThirdCategory"].ToString();
                        model.Status = Convert.ToByte(dr["Status"]);

                        con.Close();
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }
            return View();

        }
        [HttpPost]
        public ActionResult ThirdCategoryAdd(ThirdCategoryModel model)
        {
            try
            {
                dropfill();


                if (model.Tid > 0)
                {
                    mycon();
                    cmd = new SqlCommand("select * from ThairdCategory_Master  where ThirdCategory=@thirdCategory;", con);
                    cmd.Parameters.AddWithValue("@thirdCategory", model.ThirdCategory);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        ViewBag.msg = "All Redy Exists ThirdCategory";
                        con.Close();
                    }
                    else
                    {
                        mycon();
                        cmd = new SqlCommand("Update [ThairdCategory_Master] set cid=@cid,Sid=@sid,ThirdCategory=@thirdcategory,Status=@status,UpdateOn=@updateon where Tid=@id", con);
                        cmd.Parameters.AddWithValue("@cid", model.Cid);
                        cmd.Parameters.AddWithValue("@sid", model.Sid);
                        cmd.Parameters.AddWithValue("@thirdcategory", model.ThirdCategory);
                        cmd.Parameters.AddWithValue("@status", model.Status);
                        cmd.Parameters.AddWithValue("@updateon", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@id", model.Tid);
                        cmd.ExecuteNonQuery();
                        ViewBag.msg = "Update Category";
                        con.Close();
                        return RedirectToAction("SubCategoryList");

                    }


                }
                else
                {
                    mycon();
                    cmd = new SqlCommand("Insert into ThairdCategory_Master values(@cid,@sid,@category,@status,@updateon)", con);
                    cmd.Parameters.AddWithValue("@cid", model.Cid);
                    cmd.Parameters.AddWithValue("@sid", Convert.ToInt32(model.SubCategory));
                    cmd.Parameters.AddWithValue("@category", model.ThirdCategory);
                    cmd.Parameters.AddWithValue("@status", model.Status);
                    cmd.Parameters.AddWithValue("@updateon", DateTime.UtcNow);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ViewBag.msg = "Succefully Save Third Category Save";
                    return View();

                }

            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }

            return View();


        }



        public ActionResult DeleteThirdCategory(int tid)
        {
            if (tid > 0)
            {
                try
                {
                    mycon();
                    cmd = new SqlCommand("Delete from ThairdCategory_Master where Tid=@id", con);
                    cmd.Parameters.AddWithValue("@id", tid);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    RedirectToAction("ThirdCategoryList");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex;
                }
                finally
                {

                    con.Close();
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult ProductList()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ProductList(int id = 0)
        {
            List<ProductModel> list = new List<ProductModel>();
            try
            {
                mycon();
                cmd = new SqlCommand("select * from ProductMaster As p inner join Category_Master As c on p.Cid=c.C_id inner join SubCategory_Master As s on p.Sid=s.Sid inner join ThairdCategory_Master As t on p.Tid=t.Tid", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProductModel model = new ProductModel();
                    model.Pid = Convert.ToInt32(dr["Pid"]);
                    model.Category = dr["Category"].ToString();
                    model.SubCategory = dr["SubCategory"].ToString();
                    model.ThirdCategory = dr["ThirdCategory"].ToString();
                    model.ProductName = dr["ProductName"].ToString();
                    model.ProductPrice = Convert.ToDecimal(dr["ProductPrice"].ToString());
                    model.ProductColor = dr["ProductColor"].ToString();
                    model.Description = dr["Description"].ToString();
                    model.Image = dr["Image"].ToString();
                    list.Add(model);

                }
                return Json(list, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }
            return View();
        }




        public ActionResult ProductDelete(int data = 0)
        {

            if (data > 0)
            {
                try
                {
                    mycon();
                    cmd = new SqlCommand("Delete From ProductMaster where Pid=@pid", con);
                    cmd.Parameters.AddWithValue("@pid", data);
                    cmd.ExecuteNonQuery();
                    ViewBag.msg = "Succefully Save...";
                    con.Close();
                    return View("ProductList");

                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return View();
        }
        [HttpGet]

        public ActionResult ProductAdd(int data = 0)
        {
            dropfill();

            if (data > 0)
            {
                try
                {


                    mycon();
                    cmd = new SqlCommand("select * from ProductMaster As p inner join Category_Master As c on p.Cid=c.C_id inner join SubCategory_Master As s on p.Sid=s.Sid inner join ThairdCategory_Master As t on p.Tid=t.Tid where p.Pid=@pid ", con);
                    cmd.Parameters.AddWithValue("@pid", data);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        ProductModel model = new ProductModel();
                        model.Pid = Convert.ToInt32(dr["Pid"]);
                        model.Cid = Convert.ToInt32(dr["Cid"]);
                        model.Sid = Convert.ToInt32(dr["Sid"]);
                        model.Tid = Convert.ToInt32(dr["Tid"]);
                        model.ProductName = dr["ProductName"].ToString();
                        model.ProductPrice = Convert.ToDecimal(dr["ProductPrice"]);
                        model.ProductColor = dr["ProductColor"].ToString();
                        model.Description = dr["Description"].ToString();
                        model.Image = dr["Image"].ToString();

                        ViewBag.imaess = dr["Image"].ToString();
                        con.Close();
                        return View(model);

                    }
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex;
                }
                finally
                {
                    con.Close();
                }


            }

            return View();
        }


        [HttpPost]
        public ActionResult ProductAdd(ProductModel model)
        {
            dropfill();
            string path = "";

            try
            {
                if (model.Pid > 0)
                {
                    mycon();
                    cmd = new SqlCommand("Update  ProductMaster Set Cid=@cid,Sid=@sid,Tid=@tid,ProductName=@pname,ProductPrice=@price,ProductColor=@pcolor,Description=@des,Image=@image,EnteryDate=@edate where Pid=@pid", con);
                    cmd.Parameters.AddWithValue("@cid", model.Cid);
                    cmd.Parameters.AddWithValue("@sid", Convert.ToInt32(model.SubCategory));
                    cmd.Parameters.AddWithValue("@tid", Convert.ToInt32(model.ThirdCategory));
                    cmd.Parameters.AddWithValue("@pname", model.ProductName);
                    cmd.Parameters.AddWithValue("@price", model.ProductPrice);
                    cmd.Parameters.AddWithValue("@pcolor", model.ProductColor);
                    cmd.Parameters.AddWithValue("@des", model.Description);

                    if (model.Image != null)
                    {
                        cmd.Parameters.AddWithValue("@image", model.Image);

                    }
                    else
                    {
                        if (model.fileupload.ContentLength > 0)
                        {
                            model.fileupload.SaveAs(Server.MapPath("~/Content/Image/" + model.fileupload.FileName));
                            path = "~/Content/Image/" + model.fileupload.FileName;
                            cmd.Parameters.AddWithValue("@image", path);

                        }
                    }

                    cmd.Parameters.AddWithValue("@edate", model.EnterDate = System.DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@pid", model.Pid);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return Json(2, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    mycon();
                    cmd = new SqlCommand("select * from ProductMaster where ProductName=@pname", con);
                    cmd.Parameters.AddWithValue("@pname", model.ProductName);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {

                        con.Close();
                        return Json(3, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        mycon();
                        cmd = new SqlCommand("Insert Into ProductMaster Values(@cid,@sid,@tid,@pname,@price,@pcolor,@des,@image,@edate)", con);
                        cmd.Parameters.AddWithValue("@cid", model.Cid);
                        cmd.Parameters.AddWithValue("@sid", Convert.ToInt32(model.SubCategory));
                        cmd.Parameters.AddWithValue("@tid", Convert.ToInt32(model.ThirdCategory));
                        cmd.Parameters.AddWithValue("@pname", model.ProductName);
                        cmd.Parameters.AddWithValue("@price", model.ProductPrice);
                        cmd.Parameters.AddWithValue("@pcolor", model.ProductColor);
                        cmd.Parameters.AddWithValue("@des", model.Description);
                        if (model.fileupload.ContentLength > 0)
                        {
                            model.fileupload.SaveAs(Server.MapPath("~/Content/Image/" + model.fileupload.FileName));
                            path = "~/Content/Image/" + model.fileupload.FileName;
                        }

                        cmd.Parameters.AddWithValue("@image", path);
                        cmd.Parameters.AddWithValue("@edate", model.EnterDate = System.DateTime.UtcNow);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        ViewBag.msg = "Succefully Save Product";
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }


                }

            }
            catch (Exception ex)
            {
                ViewBag.msg = ex;
            }
            finally
            {
                con.Close();
            }
            return View();

        }

    }
}
