using MCPG.CCA.CCAVTil;
using MCPG.CCA.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ApsMind.Olympiad.Portal.Models
{
    public class CCAPaymentModel
    {
       
        //compulsary info
        public string tid { get; set; }
        public Int64 order_id { get; set; }
        public string currency { get; set; }
        public double amount { get; set; }
        //Billing information(optional)
        public string billing_name { get; set; }
        public string billing_address { get; set; }
        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_zip { get; set; }
        public string billing_country { get; set; }
        public string billing_tel { get; set; }
        public string billing_email { get; set; }
        //Shipping information(optional):
        public string delivery_name { get; set; }
        public string delivery_address { get; set; }
        public string delivery_city { get; set; }
        public string delivery_state { get; set; }
        public string delivery_zip { get; set; }
        public string delivery_country { get; set; }
        public string delivery_tel { get; set; }
        public string merchant_param1 { get; set; }
        public string merchant_param2 { get; set; }
        public string merchant_param3 { get; set; }
        public string merchant_param4 { get; set; }
        public string merchant_param5 { get; set; }
        public string promo_code { get; set; }
        public string customer_identifier { get; set; }

        public CCACrypTo CCACrypTo()
        {
            return new CCACrypTo(MCPG.CCA.Payment.CCAConfig.validator, this.customer_identifier, CCAConfig.merchant_id, CCAConfig.redirect_url, CCAConfig.cancel_url);
        }
    }
    
   
}