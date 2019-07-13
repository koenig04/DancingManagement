using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Common
{
    public static class PublicMathods
    {
        public static BitmapImage GetImage(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();

            if (File.Exists(imagePath))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;

                using (Stream ms = new MemoryStream(File.ReadAllBytes(imagePath)))
                {
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();
                }
            }

            return bitmap;


        }

        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable<T>
        {
            List<T> sortedList = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sortedList.Count(); i++)
            {
                collection.Move(collection.IndexOf(sortedList[i]), i);
            }
        }

        public static List<DateTime> GetDateSpan(DateTime startDate,DateTime endDate,bool isSortByMonth)
        {
            List<DateTime> res = new List<DateTime>();
            int datespanCount = 0;
            if (isSortByMonth)
            {
                datespanCount = (endDate.Month - startDate.Month + 1) + 12 * (endDate.Year - startDate.Year);
            }
            else
            {
                datespanCount = endDate.Year - startDate.Year + 1;
            }
            for (int i = 0; i < datespanCount; i++)
            {
                res.Add(isSortByMonth ? startDate.AddMonths(i) : startDate.AddYears(i));
            }
            return res;
        }
    }
}
