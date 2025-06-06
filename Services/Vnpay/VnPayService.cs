﻿using PBL3_QuanLyDatXe.Libraries;
using PBL3_QuanLyDatXe.Models.Vnpay;

namespace PBL3_QuanLyDatXe.Services.Vnpay
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            //var urlCallBack = _configuration["Vnpay:PaymentBackReturnUrl"];
            var scheme = context.Request.Scheme; // http hoặc https
            var host = context.Request.Host.Value; // domain:port
            var urlCallBack = $"{scheme}://{host}/Payment/PaymentCallbackVnpay";

            Console.WriteLine($"Creating payment URL with callback: {urlCallBack}");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);

            //pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            //pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            //pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            //pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            //pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            //pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            //pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            //pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            //pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
            //pay.AddRequestData("vnp_OrderType", model.OrderType);
            //pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            //pay.AddRequestData("vnp_TxnRef", tick);

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", model.OrderDescription);
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            Console.WriteLine($"Generated payment URL: {paymentUrl}");
            return paymentUrl;
        }
        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
            return response;
        }


    }
}
