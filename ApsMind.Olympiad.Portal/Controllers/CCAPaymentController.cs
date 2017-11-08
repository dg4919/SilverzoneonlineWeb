using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCA.Util;
using ApsMind.Olympiad.Portal.Models;
using System.Web.Configuration;
using MCPG.CCA.Payment;
using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Framework.Entity;
using System.Data.Entity;
using MCPG.CCA.CCAVTil;

namespace ApsMind.Olympiad.Portal.Controllers
{
    public class CCAPaymentController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        public ActionResult makepayment(Int32 userId)
        {

            UserProfile existingUser = objDB.UserProfiles.Where(x => x.Id == userId).FirstOrDefault();

            var ExistingItemsInCart = (from crt in objDB.Carts
                                       join qz in objDB.Quizs on crt.QuizId equals qz.QuizId
                                       join ord in objDB.QuizPurchaseOrders on new { a = crt.UserId, b = crt.OrderSqNo } equals new { a = ord.ByUserId, b = ord.OrderSqNo } into ordlist
                                       from tcheck in ordlist.DefaultIfEmpty()
                                       where crt.UserId == userId
                                       && tcheck == null
                                       select new _ItemsInCart
                                       {
                                           CartItem = crt,
                                           QuizTitle = qz.QuizName,
                                       }).ToList();
            
            CCAPaymentModel model = new CCAPaymentModel();
            ViewBag.Message = "Processing...";

            if (ExistingItemsInCart != null && ExistingItemsInCart.Count>0)
            {
               
                    //pasing parameters for pament gatway

                    model.tid = ExistingItemsInCart.FirstOrDefault().CartItem.OrderSqNo.ToString();
                    model.customer_identifier = ExistingItemsInCart.FirstOrDefault().CartItem.UserId.ToString() + "-" + ExistingItemsInCart.FirstOrDefault().CartItem.OrderSqNo.ToString();
                    model.currency = "INR";
                   
                    model.amount =ExistingItemsInCart.Sum(x=>x.CartItem.Price);
                    model.billing_name = existingUser.UserName.ToString();
                    model.billing_email = existingUser.EmailId.ToString();
                    //model.billing_city = existingUser.City.ToString();
                    model.billing_tel = existingUser.MobileNumber.ToString();
                   // model.billing_country = existingUser.Country.ToString();
                    //model.billing_state = existingUser.State.ToString();
                    // model.billing_state
                    if (model.amount <= 0)
                    {
                        ViewBag.Message = "Amount should be >0!";
                    }
                   
                    else
                    {
                        ViewBag.Message = "Processing...";
                    }
               
            }
            else
            {
                ViewBag.Message = "No Items in the cart!!";
            }

           

            //CCAPaymentModel model = new CCAPaymentModel()
            //{
            //    tid="1",
            //    order_id = 200001,
            //    amount = 100.00,
            //    currency = "INR",

            //    billing_name="",
            //    billing_address="",
            //    billing_city="",
            //    billing_state="",
            //    billing_zip="",
            //    billing_country="",
            //    billing_tel="",
            //    billing_email="",

            //    delivery_name="",
            //    delivery_address="",
            //    delivery_city="",
            //    delivery_state="",
            //    delivery_zip="",
            //    delivery_country="",
            //    delivery_tel="",
            //    merchant_param1="",
            //    merchant_param2="",
            //    merchant_param3="",
            //    merchant_param4="",
            //    merchant_param5="",
            //    promo_code="",
            //    customer_identifier="",


            //};



            return View(model);
        }

        [HttpPost]
        public ActionResult makepayment(CCAPaymentModel model)
        {
            ViewBag.Message = "";
            string strEncRequest = "";
            CCACrypTo ccaCrypto = model.CCACrypTo();
            strEncRequest = ccaCrypto.Encrypt(Request.Form, CCAConfig.workingKey);

            CCARemotePost myRPost = new CCARemotePost();
            myRPost.Add("encRequest", strEncRequest);
            myRPost.Add("access_code", CCAConfig.access_code);
            myRPost.Post();

            return View(model);
        }

        [HttpPost]
        public ActionResult paymentresponce()
        {
            string workingKey = CCAConfig.workingKey;//put in the 32bit alpha numeric key in the quotes provided here
            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
            string[] segments = encResponse.Split('&');
            ViewBag.Message = segments.Where(x => x.Contains("order_status")).FirstOrDefault();
            string status_message = segments.Where(x => x.Contains("status_message")).FirstOrDefault().Trim();
            if (status_message.Split(new char[] { '=' })[1] == "Y")
            {
                string orderId = segments.Where(x => x.Contains("order_id")).FirstOrDefault().Split(new char[] { '=' })[1];
                string[] usernordsqno = orderId.Split(new char[] { '-' });
                int userId = Convert.ToInt32(usernordsqno[0]);
                int orderSqNo = Convert.ToInt32(usernordsqno[1]);

                QuizPurchaseOrder newOrder = new QuizPurchaseOrder()
                {
                    OrderSqNo = orderSqNo,
                    TotalAmount = Convert.ToDouble(segments.Where(x => x.Contains("amount")).FirstOrDefault().Split(new char[] { '=' })[1]),
                    Currency = segments.Where(x => x.Contains("currency")).FirstOrDefault().Split(new char[] { '=' })[1],
                    ByUserId = userId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,

                    PRN_TrackingId = segments.Where(x => x.Contains("tracking_id")).FirstOrDefault().Split(new char[] { '=' })[1],
                    BID_BankRefNo = segments.Where(x => x.Contains("bank_ref_no")).FirstOrDefault().Split(new char[] { '=' })[1],
                    TransactionStatus = status_message.Split(new char[] { '=' })[1],
                    TXNDATETIME = DateTime.Now,

                };

                objDB.QuizPurchaseOrders.Add(newOrder);
                objDB.SaveChanges();

            }


            return View();
        }
    }
}