using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BIW_Extractor
{
    public class Project
    {

        /* Example of a project setup in JSON
         * {"Id":9333,"Name":" Cheadle (2304) Checkout Installation 2016","Code":"TBC",
         * "DisplayName":" Cheadle (2304) Checkout Installation 2016","Active":true}
         */

            
        public int id;
        public string name;
        public string code;
        public string displayName;
        public bool active;




        public override string ToString()
        {
            return $"{this.id},{this.name.Replace(',',' ').Trim()},{this.code},{this.displayName.Replace(',',' ').Trim()},{this.active}";
        }
    }

}
 