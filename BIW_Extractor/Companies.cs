using System;
namespace BIW_Extractor
{
    public class Companies
    {

        /* Sample Company Response
         * "[{\"Id\":364,\"Name\":\"Air Link Systems Limited\"},{\"Id\":375,\"Name\":\"CONJECT Limited\"},
         * {\"Id\":18250,\"Name\":\"Eco Instruments Ltd\"},{\"Id\":791,\"Name\":\"Leslie Clark\"},
         * {\"Id\":1,\"Name\":\"Sainsburys Property Company\"},{\"Id\":2917,\"Name\":\"Sainsburys Store Development\"},
         * {\"Id\":4688,\"Name\":\"Sainsburys Supermarkets Ltd\"}]"
         */

        public int id;
        public string name;
        public bool active;


        public override string ToString()
        {
            return $"{this.id},{this.name.Replace(',',' ').Trim()},{this.active}";
        }
    }

}
