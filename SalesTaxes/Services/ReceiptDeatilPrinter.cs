using System;
using SalesTaxes.Models;

namespace SalesTaxes.Services
{
    public class ReceiptDeatilPrinter
    {
        public void Print(ReceiptDetail receiptDetail)
        {
            if (receiptDetail == null) throw new ArgumentNullException(nameof(receiptDetail));
                
            Console.WriteLine(receiptDetail.Receipt);
            Console.ReadLine(); 
        }
    }
}