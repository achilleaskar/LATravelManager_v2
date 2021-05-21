using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;

namespace LATravelManager.UI.ApiConnectors.MyData
{
    public static class ProgramDev
    {
        public static async Task MakeRequest()
        {

            string myData = "";
            myData = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
            myData += "<InvoicesDoc xmlns=\"http://www.aade.gr/myDATA/invoice/v1.0\" xsi:schemaLocation=\"http://www.aade.gr/myDATA/invoice/v1.0 schema.xsd\" xmlns:icls=\"https://www.aade.gr/myDATA/incomeClassificaton/v1.0\" xmlns:ecls=\"https://www.aade.gr/myDATA/expensesClassificaton/v1.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";
            myData += "  <invoice>";
            myData += "    <issuer>";
            myData += "      <vatNumber>801252307</vatNumber>";
            myData += "      <country>GR</country>";
            myData += "      <branch>0</branch>";
            myData += "    </issuer>";
            myData += "    <counterpart>";
            myData += "      <vatNumber>148017999</vatNumber>";
            myData += "      <country>GR</country>";
            myData += "      <branch>0</branch>";
            myData += "      <address>";
            myData += "        <street>ODOS PELATH</street>";
            myData += "        <number>11</number>";
            myData += "        <postalCode>11111</postalCode>";
            myData += "        <city>PERIOXH PELATH</city>";
            myData += "      </address>";
            myData += "    </counterpart>";
            myData += "    <invoiceHeader>";
            myData += "      <series>TIM</series>";
            myData += "      <aa>000152</aa>";
            myData += "      <issueDate>2020-09-30</issueDate>";
            myData += "      <invoiceType>1.1</invoiceType>";
            myData += "      <currency>EUR</currency>";
            myData += "    </invoiceHeader>";
            myData += "    <paymentMethods>";
            myData += "      <paymentMethodDetails>";
            myData += "        <type>5</type>";
            myData += "        <amount>31.00</amount>";
            myData += "        </paymentMethodDetails>";
            myData += "    </paymentMethods>";
            myData += "    <invoiceDetails>";
            myData += "      <lineNumber>10001</lineNumber>";
            myData += "      <netValue>25.00</netValue>";
            myData += "      <vatCategory>1</vatCategory>";
            myData += "      <vatAmount>6.00</vatAmount>";
            myData += "      <incomeClassification>";
            myData += "        <icls:classificationType>E3_561_001</icls:classificationType>";
            myData += "        <icls:classificationCategory>category1_1</icls:classificationCategory>";
            myData += "        <icls:amount>25.00</icls:amount>";
            myData += "        <icls:id>1</icls:id>";
            myData += "      </incomeClassification>";
            myData += "    </invoiceDetails>";
            myData += "    <invoiceSummary>";
            myData += "      <totalNetValue>25.00</totalNetValue>";
            myData += "      <totalVatAmount>6.00</totalVatAmount>";
            myData += "      <totalWithheldAmount>0.00</totalWithheldAmount>";
            myData += "      <totalFeesAmount>0.00</totalFeesAmount>";
            myData += "      <totalStampDutyAmount>0.00</totalStampDutyAmount>";
            myData += "      <totalOtherTaxesAmount>0.00</totalOtherTaxesAmount>";
            myData += "      <totalDeductionsAmount>0.00</totalDeductionsAmount>";
            myData += "      <totalGrossValue>31.00</totalGrossValue>";
            myData += "      <incomeClassification>";
            myData += "        <icls:classificationType>E3_561_001</icls:classificationType>";
            myData += "        <icls:classificationCategory>category1_1</icls:classificationCategory>";
            myData += "        <icls:amount>25.00</icls:amount>";
            myData += "        <icls:id>1</icls:id>";
            myData += "      </incomeClassification>";
            myData += "    </invoiceSummary>";
            myData += "  </invoice>";
            myData += "</InvoicesDoc>";

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("aade-user-id", "latravel");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0becfe9deb874b2d8350e2f0b670d746");

            var uri = "https://mydata-dev.azure-api.net/SendInvoices?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(myData);

            using var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(uri, content);
            var contents = await response.Content.ReadAsStringAsync();
        }


       
    }
}
