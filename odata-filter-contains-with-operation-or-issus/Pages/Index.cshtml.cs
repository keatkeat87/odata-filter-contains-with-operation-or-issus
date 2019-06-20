using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData;
using Microsoft.OData.UriParser;

namespace odata_filter_contains_with_operation_or_issus.Pages
{
    public class Product
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string supplier { get; set; }
        public string category { get; set; }
        public double price { get; set; }
    }

    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            var model = builder.GetEdmModel();

            var parser = new ODataUriParser(model, new Uri("http://192.168.1.61547"), 
                new Uri($"http://192.168.1.61547/Products?$filter=(name eq 'IPhone') and (contains(supplier, 'Apple') or contains(category,'Phone'))")
            );
            var odataUri = parser.ParseUri();
            var xx = odataUri.BuildUri(ODataUrlKeyDelimiter.Parentheses);
            var result = xx.Query;
            result = result.Replace("%20", " ");
            result = result.Replace("%27", "'");
            result = result.Replace("%28", "(");
            result = result.Replace("%29", ")");
            result = result.Replace("%2F", "/");
            /*
                ?$filter=name eq 'IPhone' and contains(supplier%2C'Apple') or contains(category%2C'Phone') 
                ?$filter=name eq 'IPhone' and (contains(supplier%2C'Apple') or contains(category%2C'Phone')) 
                the first one mission parentheses
            */


            parser = new ODataUriParser(model, new Uri("http://192.168.1.61547"),
                new Uri($"http://192.168.1.61547/Products?$filter=(name eq 'IPhone') and (supplier eq 'Apple' or category eq 'Phone')")
            );
            odataUri = parser.ParseUri();
            xx = odataUri.BuildUri(ODataUrlKeyDelimiter.Parentheses);
            result = xx.Query;
            result = result.Replace("%20", " ");
            result = result.Replace("%27", "'");
            result = result.Replace("%28", "(");
            result = result.Replace("%29", ")");
            result = result.Replace("%2F", "/");

            /*
                ?$filter=name eq 'IPhone' and (supplier eq 'Apple' or category eq 'Phone')
                correct
            */


            parser = new ODataUriParser(model, new Uri("http://192.168.1.61547"),
                 new Uri($"http://192.168.1.61547/Products?$filter=(name eq 'IPhone') and (contains(supplier, 'Apple') or contains(category,'Phone') or category eq 'impossible match')")
            );
            odataUri = parser.ParseUri();
            xx = odataUri.BuildUri(ODataUrlKeyDelimiter.Parentheses);
            result = xx.Query;
            result = result.Replace("%20", " ");
            result = result.Replace("%27", "'");
            result = result.Replace("%28", "(");
            result = result.Replace("%29", ")");
            result = result.Replace("%2F", "/");

            /*
                ?$filter=name eq 'IPhone' and (contains(supplier%2C'Apple') or contains(category%2C'Phone') or category eq 'impossible match')
                correct
            */

        }
    }
}
