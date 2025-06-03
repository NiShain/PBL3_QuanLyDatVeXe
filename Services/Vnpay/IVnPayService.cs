using PBL3_QuanLyDatXe.Models.Vnpay;

namespace PBL3_QuanLyDatXe.Services.Vnpay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
