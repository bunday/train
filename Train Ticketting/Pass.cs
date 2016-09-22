using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train_Ticketting
{
    class Pass
    {
        private static string selectedStation="";
        public void getStation(string a){
            selectedStation = a;
        }
        public string returnStation(){
            return selectedStation;
        }
        
    }
}
