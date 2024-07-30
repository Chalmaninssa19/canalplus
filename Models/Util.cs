using System;

namespace canalplus.Models;

class Util
{
   public static double sum(string [] prix) {
       double sum = 0.0;
       foreach(string item in prix) {
           double conv = double.Parse(item);
           sum += conv;
       }
       return sum;
   }
}