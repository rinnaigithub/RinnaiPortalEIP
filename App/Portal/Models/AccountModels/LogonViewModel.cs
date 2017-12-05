using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models.AccountModels
{
    public class LogonViewModel
    {

        public string Account { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public string DomainName { get; set; }


    }
}